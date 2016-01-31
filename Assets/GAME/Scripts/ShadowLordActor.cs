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

    public override void Play(CellGrid cellGrid)
    {
        base.Play(cellGrid);
        SelectFirstEnemy(cellGrid);
    }

    void SelectFirstEnemy(CellGrid cellGrid)
    {
        this.cellGrid = cellGrid;
        var unit = cellGrid.Units.FindAll(u => u.PlayerNumber.Equals(PlayerNumber)).FirstOrDefault();
        if (unit != null)
        {
            manager.actualPlayer = unit.GetComponent<Actor>();
            cameraController.target = unit.transform;
        }
    }

    /// <summary>
    /// Switches to the next monster to be controlled.
    /// </summary>
    /// <returns>true = monster found and switched to it, false otherwise</returns>
    public bool NextMonster()
    {
        return false;
    }
}
