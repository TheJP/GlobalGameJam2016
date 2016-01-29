using System.Linq;

public abstract class CellGridState
{
    protected CellGrid _cellGrid;
    
    protected CellGridState(CellGrid cellGrid)
    {
        _cellGrid = cellGrid;
    }

    public virtual void OnUnitClicked(Unit unit)
    { }
    
    public virtual void OnCellDeselected(Cell cell)
    {
        cell.UnMark();
    }
    public virtual void OnCellSelected(Cell cell)
    {
        cell.MarkAsHighlighted();
    }
    public virtual void OnCellClicked(Cell cell)
    { }

    public virtual void OnStateEnter()
    {
        if (_cellGrid.Units.Select(u => u.PlayerNumber).Distinct().ToList().Count == 1)
        {
            _cellGrid.CellGridState = new CellGridStateGameOver(_cellGrid);
        }
    }
    public virtual void OnStateExit()
    {
    }
}