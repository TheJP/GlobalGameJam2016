using UnityEngine;
using System.Collections;

public class MainController : MonoBehaviour
{
    public CellGrid cellGrid;

    void FixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.N))
        {
            cellGrid.EndTurn();
        }
    }
}
