using System;
using System.Data;
using Mono.Data.Sqlite;
using System.IO;
using UnityEngine;
using System.Collections.Generic;

public class SQLliteHandler
{

    private string sqlLocString;
    private string relativeLoc;
    public SQLliteHandler(string sqlconString, string relativeLoc)
    {
        this.sqlLocString = sqlconString;
        this.relativeLoc = relativeLoc;
    }

    public void CreateTable()
    {
        string databasePath = Path.Combine(Application.persistentDataPath, relativeLoc);
        if (!File.Exists(databasePath))
        {
            string createPlayerTable = @"
        CREATE TABLE IF NOT EXISTS PlayerTable (
            playerId INTEGER PRIMARY KEY AUTOINCREMENT,
            Name TEXT NOT NULL
        )";



            string createMatchTable = @"
        CREATE TABLE IF NOT EXISTS MatchTable (
            MatchId INTEGER PRIMARY KEY AUTOINCREMENT
        )";

            string createStatementsTable = @"
        CREATE TABLE IF NOT EXISTS StatementsTable (
            statementId INTEGER PRIMARY KEY AUTOINCREMENT,
            playerId INTEGER NOT NULL,
            MatchId INTEGER NOT NULL,
            statement TEXT,
            ratingCNNSVM REAL,
            ratingChatFilter REAL,
            boolCNNSVM INTEGER,
            boolChatFilter INTEGER,
            trueEval INTEGER,
            FOREIGN KEY(playerId) REFERENCES PlayerTable(playerId),
            FOREIGN KEY(MatchId) REFERENCES MatchTable(MatchId)
        )";

            using (IDbConnection dbConnection = new SqliteConnection(sqlLocString))
            {
                dbConnection.Open();
                using (IDbCommand dbCmd = dbConnection.CreateCommand())
                {

                    dbCmd.CommandText = createPlayerTable;
                    dbCmd.ExecuteNonQuery();

                    dbCmd.CommandText = createMatchTable;
                    dbCmd.ExecuteNonQuery();

                    dbCmd.CommandText = createStatementsTable;
                    dbCmd.ExecuteNonQuery();
                }
                dbConnection.Close();
            }
            Debug.Log("Database created");
        }
        else
        {
            Debug.Log("Database already exists");
        }
    }

    public int newPlayer(playerObj newPlayer)
    {
        int id = -1;

        try {
            string insertNewPlayer = "INSERT INTO PlayerTable (Name) VALUES (@Name); SELECT last_insert_rowid()";

            using (IDbConnection dbConnection = new SqliteConnection(this.sqlLocString))
            {
                dbConnection.Open();
                using (IDbCommand dbCmd = dbConnection.CreateCommand())
                {
                    dbCmd.CommandText = insertNewPlayer;

                    IDbDataParameter nameParam = dbCmd.CreateParameter();
                    nameParam.ParameterName = "@Name";
                    nameParam.Value = newPlayer.playerName;
                    dbCmd.Parameters.Add(nameParam);

                    object playerIdOBJ = dbCmd.ExecuteScalar();
                    if (playerIdOBJ != null && playerIdOBJ != DBNull.Value)
                    {
                        int playerId = Convert.ToInt32(playerIdOBJ);
                        Debug.Log($"Player ID: {playerId}");
                        id = playerId;
                    }
                    else
                    {
                        Debug.LogError("Failed to retrieve the generated player ID.");
                    }

                }
                dbConnection.Close();
            }

        } catch (Exception e)
        {
            Debug.Log($"Error: {e.Message}");
        }
        return id;
    }

    public bool UpdatePlayerName(playerObj selectedPlayer)
    {
        bool inserted = false;
        string updatePlayerSql = @"UPDATE PlayerTable SET Name = @NewName WHERE playerId = @PlayerId";

        try
        {

            using (IDbConnection dbConnection = new SqliteConnection(sqlLocString))
            {
                dbConnection.Open();
                using (IDbCommand dbCmd = dbConnection.CreateCommand())
                {
                    dbCmd.CommandText = updatePlayerSql;

                    IDbDataParameter playerIdParam = dbCmd.CreateParameter();
                    playerIdParam.ParameterName = "@PlayerId";
                    playerIdParam.Value = selectedPlayer.playerId;
                    dbCmd.Parameters.Add(playerIdParam);

                    IDbDataParameter newNameParam = dbCmd.CreateParameter();
                    newNameParam.ParameterName = "@NewName";
                    newNameParam.Value = selectedPlayer.playerName;
                    dbCmd.Parameters.Add(newNameParam);

                    dbCmd.ExecuteNonQuery();
                }
                dbConnection.Close();
            }
            inserted = true;
        } catch (Exception e)
        {

        }
        return inserted;
    }

    public bool InsertStatement(statementObj newstatement)
    {
        bool inserted = false;
        string insertStatementSql = @"INSERT INTO StatementsTable (playerId, MatchId, statement, ratingCNNSVM, ratingChatFilter, boolCNNSVM, boolChatFilter)
            VALUES (@PlayerId, @MatchId, @Statement, @RatingCNNSVM, @RatingChatFilter, @BoolCNNSVM, @BoolChatFilter)";

        try
        {

            using (IDbConnection dbConnection = new SqliteConnection(sqlLocString))
            {
                dbConnection.Open();
                using (IDbCommand dbCmd = dbConnection.CreateCommand())
                {
                    dbCmd.CommandText = insertStatementSql;

                    dbCmd.Parameters.Add(new SqliteParameter("@PlayerId", newstatement.playerId));
                    dbCmd.Parameters.Add(new SqliteParameter("@MatchId", newstatement.matchId));
                    dbCmd.Parameters.Add(new SqliteParameter("@Statement", newstatement.statement));
                    dbCmd.Parameters.Add(new SqliteParameter("@RatingCNNSVM", newstatement.ratingCNNSVM));
                    dbCmd.Parameters.Add(new SqliteParameter("@RatingChatFilter", newstatement.ratingChatFilter));
                    dbCmd.Parameters.Add(new SqliteParameter("@BoolCNNSVM", newstatement.boolCNNSVM));
                    dbCmd.Parameters.Add(new SqliteParameter("@BoolChatFilter", newstatement.boolChatFilter));

                    dbCmd.ExecuteNonQuery();
                }
                dbConnection.Close();
            }
            inserted = true;
        }
        catch (Exception e)
        {
            Debug.Log($"insertion statement failed: {e.Message}");
        }
        return inserted;
    }

    public bool updateStatement(statementObj updateStatement)
    {
        bool updated = false;
        string updateStatementSql = "UPDATE StatementsTable SET trueEval = @trueEval WHERE statementId = @statementId";
        try
        {
            using (IDbConnection dbConnection = new SqliteConnection(sqlLocString))
            {
                dbConnection.Open();
                using (IDbCommand dbCmd = dbConnection.CreateCommand())
                {
                    dbCmd.CommandText = updateStatementSql;
                    dbCmd.Parameters.Add(new SqliteParameter("@trueEval", updateStatement.trueEval));
                    dbCmd.Parameters.Add(new SqliteParameter("@statementId", updateStatement.statementId));

                    dbCmd.ExecuteNonQuery();
                }
                dbConnection.Close();
            }
            updated = true;
        }
        catch (Exception e)
        {

        }
        return updated;
    }

    public List<statementObj> selectStatements(int matchId, int playerId)
    {
        string selectStatementSql = "SELECT StatementsTable.*, PlayerTable.Name FROM StatementsTable " +
                                "JOIN PlayerTable ON StatementsTable.playerId = PlayerTable.playerId " +
                                "WHERE StatementsTable.MatchId = @matchId AND StatementsTable.playerId = @playerId;";
        List<statementObj> statementList = new List<statementObj>();
        try
        {
            using (IDbConnection dbConnection = new SqliteConnection(sqlLocString))
            {
                dbConnection.Open();
                using (IDbCommand dbCmd = dbConnection.CreateCommand())
                {
                    dbCmd.Parameters.Add(new SqliteParameter("@matchId", matchId));
                    dbCmd.Parameters.Add(new SqliteParameter("@playerId", playerId));

                    using (IDataReader reader = dbCmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int StatementId = Convert.ToInt32(reader["statementId"]);
                            int PlayerId = Convert.ToInt32(reader["playerId"]);
                            int MatchId = Convert.ToInt32(reader["MatchId"]);
                            string StatementText = reader["statement"].ToString();
                            double RatingCNNSVM = Convert.ToDouble(reader["ratingCNNSVM"]);
                            double RatingChatFilter = Convert.ToDouble(reader["ratingChatFilter"]);
                            int BoolCNNSVM = Convert.ToInt32(reader["boolCNNSVM"]);
                            int BoolChatFilter = Convert.ToInt32(reader["boolChatFilter"]);
                            int TrueEval = Convert.ToInt32(reader["trueEval"]);
                            string PlayerName = reader["Name"].ToString();

                            statementObj addstatement = new statementObj(StatementId,
                                PlayerId,
                                MatchId,
                                StatementText,
                                RatingCNNSVM,
                                RatingChatFilter,
                                BoolCNNSVM,
                                BoolChatFilter,
                                TrueEval,
                                PlayerName);
                            statementList.Add(addstatement);
                        }
                    }
                }
                dbConnection.Close();
            }
        }
        catch (Exception e)
        {

        }
        return statementList;
    }

    public bool rateStatement(statementObj rateStatement)
    {
        bool inserted = false;
        string updateStatementSql = @"UPDATE StatementsTable SET trueEval = @rateEval WHERE statementId = @StatementId";

        try
        {

            using (IDbConnection dbConnection = new SqliteConnection(sqlLocString))
            {
                dbConnection.Open();
                using (IDbCommand dbCmd = dbConnection.CreateCommand())
                {
                    dbCmd.CommandText = updateStatementSql;

                    dbCmd.Parameters.Add(new SqliteParameter("@rateEval", rateStatement.trueEval));
                    dbCmd.Parameters.Add(new SqliteParameter("@StatementId", rateStatement.statementId));

                    dbCmd.ExecuteNonQuery();
                }
                dbConnection.Close();
            }
            inserted = true;
        }
        catch (Exception e)
        {

        }
        return inserted;
    }

    public bool rateStatements(List<statementObj> statements)
    {
        bool updated = false;
        string updateStatementSql = @"UPDATE StatementsTable SET trueEval = @rateEval WHERE statementId = @StatementId";

        try
        {
            using (IDbConnection dbConnection = new SqliteConnection(sqlLocString))
            {
                dbConnection.Open();
                using (IDbCommand dbCmd = dbConnection.CreateCommand())
                {
                    dbCmd.CommandText = updateStatementSql;

                    
                    var rateEvalParameter = new SqliteParameter("@rateEval", DbType.Int32);
                    var statementIdParameter = new SqliteParameter("@StatementId", DbType.Int32);
                    dbCmd.Parameters.Add(rateEvalParameter);
                    dbCmd.Parameters.Add(statementIdParameter);

                    foreach (var statement in statements)
                    {
                        rateEvalParameter.Value = statement.trueEval;
                        statementIdParameter.Value = statement.statementId;

                        dbCmd.ExecuteNonQuery();
                    }
                }
                dbConnection.Close();
            }
            updated = true;
        }
        catch (Exception e)
        {
            
        }
        return updated;
    }

    public int newMatch()
    {
        int matchId = -1;

        string insertMatchSql = "INSERT INTO MatchTable DEFAULT VALUES; SELECT last_insert_rowid();";
        try
        {
            using (IDbConnection dbConnection = new SqliteConnection(sqlLocString))
            {
                dbConnection.Open();
                using (IDbCommand dbCmd = dbConnection.CreateCommand())
                {
                    dbCmd.CommandText = insertMatchSql;

                    matchId = Convert.ToInt32(dbCmd.ExecuteScalar());

                }
            }
        } catch(Exception e)
        {

        }
        return matchId;
    }
}
