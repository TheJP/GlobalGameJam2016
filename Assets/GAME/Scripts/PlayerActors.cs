using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

class PlayerActors : HumanPlayer {

    public GameManager manager;
    public CameraController cameraController;

    public override void Play(CellGrid cellGrid)
    {
        base.Play(cellGrid);
        SelectCharacter(cellGrid);
    }

    void SelectCharacter(CellGrid cellGrid) {
        var unit = cellGrid.Units.FindAll(u => u.PlayerNumber.Equals(PlayerNumber)).FirstOrDefault();
        if(unit != null)
        {
            manager.actualPlayer = unit.GetComponent<Actor>();
            cameraController.target = unit.transform;
        }
    }
}
