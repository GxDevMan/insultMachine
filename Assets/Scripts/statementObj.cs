using System;

public class statementObj
{
    public int statementId { get; set; }
    public int playerId { get; set; }
    public int matchId { get; set; }
    public string statement { get; }
    public double ratingCNNSVM { get; set; }
    public double ratingChatFilter { get; set; }
    public int boolCNNSVM { get; set; }
    public int boolChatFilter { get; set; }
    public int trueEval { get; set; }

    public string playerName { get; set; }


    public statementObj(int statementId, int playerId, int matchId, string statement, double ratingCNNSVM, double ratingChatFilter, int boolCNNSVM, int boolChatFilter, int trueEval)
    {
        this.statementId = statementId;
        this.playerId = playerId;
        this.matchId = matchId;
        this.statement = statement;
        this.ratingCNNSVM = Math.Abs(ratingCNNSVM);
        this.ratingChatFilter = Math.Abs(ratingChatFilter);
        this.boolCNNSVM = boolCNNSVM;
        this.boolChatFilter = boolChatFilter;
        this.trueEval = trueEval;
    }

    public statementObj(int statementId, int playerId, int matchId, string statement, double ratingCNNSVM, double ratingChatFilter, int boolCNNSVM, int boolChatFilter, int trueEval, string playerName)
    {
        this.statementId = statementId;
        this.playerId = playerId;
        this.matchId = matchId;
        this.statement = statement;
        this.ratingCNNSVM = Math.Abs(ratingCNNSVM);
        this.ratingChatFilter = Math.Abs(ratingChatFilter);
        this.boolCNNSVM = boolCNNSVM;
        this.boolChatFilter = boolChatFilter;
        this.trueEval = trueEval;
        this.playerName = playerName;
    }
    public statementObj(int playerId, int matchId, string statement, double ratingCNNSVM, double ratingChatFilter, int boolCNNSVM, int boolChatFilter)
    {
        this.playerId = playerId;
        this.matchId = matchId;
        this.statement = statement;
        this.ratingCNNSVM = Math.Abs(ratingCNNSVM);
        this.ratingChatFilter = Math.Abs(ratingChatFilter);
        this.boolCNNSVM = boolCNNSVM;
        this.boolChatFilter = boolChatFilter;
    }

    public statementObj(int playerId, int matchId, string statement)
    {
        this.playerId = playerId;
        this.matchId = matchId;
        this.statement = statement;
    }
}
