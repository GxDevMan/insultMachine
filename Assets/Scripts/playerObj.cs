public class playerObj
{
    public int playerId { get; set; }
    public string playerName { get; set; }
    public int maxHeal { get; }
    public int maxDamage { get; }
    public int health { get; set; }
    public int maxHealth { get; set; }


    public playerObj(string playerName,int maxHealth, int maxHeal, int maxDamage)
    {
        this.playerId = playerId;
        this.playerName = playerName;
        this.maxHeal = maxHeal;
        this.health = maxHealth;
        this.maxHealth = maxHealth;
        this.maxDamage = maxDamage;
    }
    public playerObj(int playerId, string playerName, int maxHeal, int maxDamage)
    {
        this.playerId = playerId;
        this.playerName = playerName;
        this.maxHeal = maxHeal;
        this.maxDamage = maxDamage;
    }
}
