class DefenceBuff : Buff
{
    private int _factor;

    public DefenceBuff(int duration, int factor)
    {
        Duration = duration;
        _factor = factor;
    }

    public int Duration { get; set; }
    public void Apply(Unit unit)
    {
        unit.DefenceFactor += _factor;
    }

    public void Undo(Unit unit)
    {
        unit.DefenceFactor -= _factor;
    }

    public Buff Clone()
    {
        return new DefenceBuff(Duration, _factor);
    }
}

