using System;
using System.Data;
using Mono.Data.Sqlite;
public class SQLliteHandler
{

    private string sqlConString;
    public SQLliteHandler(String sqlconString)
    {
        sqlConString = sqlconString;
    }

    public void CreateTable()
    {
        string createPlayerTable = @"
            CREATE TABLE IF NOT EXISTS PlayerTable (
                playerId INTEGER PRIMARY KEY AUTOINCREMENT,
                Name TEXT
            )";

        string createMatchTable = @"
            CREATE TABLE IF NOT EXISTS MatchTable (
                MatchId INTEGER PRIMARY KEY AUTOINCREMENT
            )";

        string createStatementsTable = @"
            CREATE TABLE IF NOT EXISTS StatementsTable (
                statementId INTEGER PRIMARY KEY AUTOINCREMENT,
                playerId INTEGER,
                MatchId INTEGER,
                statement TEXT,
                ratingCNNSVM REAL,
                ratingChatFilter REAL,
                boolCNNSVM INTEGER,
                boolChatFilter INTEGER,
                trueEval INTEGER,
                FOREIGN KEY(playerId) REFERENCES PlayerTable(playerId),
                FOREIGN KEY(MatchId) REFERENCES MatchTable(MatchId)
            )";

        using (IDbConnection dbConnection = new SqliteConnection(sqlConString))
        {
            dbConnection.Open();
            using (IDbCommand dbCmd = dbConnection.CreateCommand())
            {
                dbCmd.CommandText = createPlayerTable;
                dbCmd.ExecuteNonQuery();

                dbCmd.CommandText = createMatchTable;
                dbCmd.ExecuteNonQuery();


            }
            dbConnection.Close();
        }
    }

    public void insertEval()
    {

    }

    public void updateEval()
    {

    }

    public void deleteEval()
    {

    }

}
