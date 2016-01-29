using UnityEngine;

class HealingBuff : Buff
{
    private int _healingFactor;

    public HealingBuff(int duration, int healingFactor)
    {
        _healingFactor = healingFactor;
        Duration = duration;
    }

    public int Duration { get; set; }
    public void Apply(Unit unit)
    {
        AddHitPoints(unit, _healingFactor);
    }
    public void Undo(Unit unit)
    {
        //Note that healing buff has empty Undo method implementation.
    }

    public Buff Clone()
    {
        return new HealingBuff(Duration, _healingFactor);
    }

    private void AddHitPoints(Unit unit, int amount)
    {
        unit.HitPoints = Mathf.Clamp(unit.HitPoints + amount, 0, unit.TotalHitPoints);
    }
}

