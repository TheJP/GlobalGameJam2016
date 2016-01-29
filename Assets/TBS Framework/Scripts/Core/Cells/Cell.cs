using UnityEngine;
using System;
using System.Collections.Generic;

public abstract class Cell : MonoBehaviour, IGraphNode
{
    [HideInInspector]
    [SerializeField]
    private Vector2 _offsetCoord;
    public Vector2 OffsetCoord { get { return _offsetCoord; } set { _offsetCoord = value; } }

    /// <summary>
    /// Indicates if something is occupying the cell.
    /// </summary>
    public bool IsTaken;
    /// <summary>
    /// Cost of moving through the cell.
    /// </summary>
    public int MovementCost;

    /// <summary>
    /// CellClicked event is invoked when user clicks the unit. It requires a collider on the cell game object to work.
    /// </summary>
    public event EventHandler CellClicked;
    /// <summary>
    /// CellHighlighed event is invoked when user moves cursor over the cell. It requires a collider on the cell game object to work.
    /// </summary>
    public event EventHandler CellHighlighted;
    public event EventHandler CellDehighlighted;

    protected virtual void OnMouseEnter()
    {
        if (CellHighlighted != null)
            CellHighlighted.Invoke(this, new EventArgs());
    }
    protected virtual void OnMouseExit()
    {    
        if (CellDehighlighted != null)
            CellDehighlighted.Invoke(this, new EventArgs());
    }
    void OnMouseDown()
    {
        if (CellClicked != null)
            CellClicked.Invoke(this, new EventArgs());
    }

    /// <summary>
    /// Method returns distance to other cell, that is given as parameter. 
    /// </summary>
    public abstract int GetDistance(Cell other);

    /// <summary>
    /// Method returns cells adjacent to current cell, from list of cells given as parameter.
    /// </summary>
    public abstract List<Cell> GetNeighbours(List<Cell> cells);
      
    public abstract Vector3 GetCellDimensions(); //Cell dimensions are necessary for grid generators.

    /// <summary>
    ///  Method marks the cell to give user an indication, that selected unit can reach it.
    /// </summary>
    public abstract void MarkAsReachable();
    /// <summary>
    /// Method marks the cell as a part of a path.
    /// </summary>
    public abstract void MarkAsPath();
    /// <summary>
    /// Method marks the cell as highlighted. It gets called when the mouse is over the cell.
    /// </summary>
    public abstract void MarkAsHighlighted();
    /// <summary>
    /// Method returns the cell to its base appearance.
    /// </summary>
    public abstract void UnMark();

    public int GetDistance(IGraphNode other)
    {
        return GetDistance(other as Cell);
    }
}