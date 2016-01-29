public class UnitStateMarkedAsSelected : UnitState
{
    public UnitStateMarkedAsSelected(Unit unit) : base(unit)
    {      
    }

    public override void Apply()
    {
        _unit.MarkAsSelected();
    }

    public override void MakeTransition(UnitState state)
    {
        state.Apply();
        _unit.UnitState = state;
    }
}

