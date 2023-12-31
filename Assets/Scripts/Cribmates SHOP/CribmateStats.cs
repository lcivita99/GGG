using System;

public class CribmateStats : ICloneable
{
    public string name { get; set; }
    public int poolOdds { get; set; }
    public int cribID { get; set; }
    public int cost { get; set; }

    public bool swap = true;

    public int slot { get; set; }

    //public SlotManager slotType { get; set; }

    public object Clone()
    {
        return new CribmateStats
        {
            name = String.Copy(this.name),
            poolOdds = this.poolOdds,
            cost = this.cost,
            cribID = this.cribID,
            swap = this.swap,
            slot = this.slot
           // slotType = this.slotType
        };
    }
}