using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

class ShadowLordActor : HumanPlayer
{

    public GameManager manager;
    public CameraController cameraController;
    private CellGrid cellGrid;
    private List<Unit> units = new List<Unit>();

    public override void Play(CellGrid cellGrid)
    {
        base.Play(cellGrid);
        SelectFirstEnemy(cellGrid);
    }

    void SelectFirstEnemy(CellGrid cellGrid)
    {
        this.cellGrid = cellGrid;
        units = cellGrid.Units.FindAll(u => u.PlayerNumber.Equals(PlayerNumber)).ToList();
        NextMonster();
    }

    /// <summary>
    /// Switches to the next monster to be controlled.
    /// </summary>
    /// <returns>true = monster found and switched to it, false otherwise</returns>
    public bool NextMonster()
    {
        if (!units.Any()) { return false; }
        var unit = units[0];
        units.RemoveAt(0);
        manager.actualPlayer = unit.GetComponent<ShadowWorldUnit>();
        cameraController.target = unit.transform;
        return true;
    }
}
