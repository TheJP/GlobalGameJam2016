public class UnitStateMarkedAsReachableEnemy : UnitState
{
    public UnitStateMarkedAsReachableEnemy(Unit unit) : base(unit)
    {        
    }

    public override void Apply()
    {
        _unit.MarkAsReachableEnemy();
    }

    public override void MakeTransition(UnitState state)
    {
        state.Apply();
        _unit.UnitState = state;
    }
}

