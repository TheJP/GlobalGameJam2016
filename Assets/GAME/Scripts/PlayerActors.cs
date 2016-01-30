using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerActors : Player {

    public GameManager manager;
    private CellGrid _cellGrid;

    public override void Play(CellGrid cellGrid)
    {
        cellGrid.CellGridState = new CellGridStateAiTurn(cellGrid);
        _cellGrid = cellGrid;
        SelectCharacter();
    }

    void SelectCharacter() {
        var myUnits = _cellGrid.Units.FindAll(u => u.PlayerNumber.Equals(PlayerNumber)).ToList();
        foreach (var unit in myUnits.OrderByDescending(u => u.Cell.GetNeighbours(_cellGrid.Cells).FindAll(u.IsCellTraversable).Count))
        {
            manager.actualPlayer = unit.GetComponent<Actor>();
        }
    }
}
