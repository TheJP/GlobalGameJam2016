using System.Collections.Generic;
using UnityEngine;

class CustomCellGridGenerator : MonoBehaviour, ICellGridGenerator
{
    public Transform CellsParent;

    /// <summary>
    /// Returns cells that are already children of CellsParent object.
    /// </summary>
    public List<Cell> GenerateGrid()
    {
        List<Cell> ret = new List<Cell>();
        for (int i = 0; i < CellsParent.childCount; i++)
        {
            var cell = CellsParent.GetChild(i).gameObject.GetComponent<Cell>();
            if (cell != null)
                ret.Add(cell);
            else
                Debug.LogError("Invalid object in cells paretn game object");
        }
        return ret;
    }
}

