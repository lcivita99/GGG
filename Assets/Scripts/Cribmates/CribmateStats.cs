using System;

public class CribmateStats : ICloneable
{
    public string name { get; set; }
    public int poolOdds { get; set; }
    public int cribID { get; set; }
    public int cost { get; set; }
    public bool swap = true;

    public object Clone()
    {
        return new CribmateStats
        {
            name = String.Copy(this.name),
            poolOdds = this.poolOdds,
            cost = this.cost,
            cribID = this.cribID,
            swap = this.swap
        };
    }
}