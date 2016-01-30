using UnityEngine;
using System.Collections;

public static class CellExtensions
{
    /// <summary>Determines if the Cell is spawnable</summary>
    public static bool IsSpawnable(this Cell cell)
    {
        var floor = cell as FloorTile;
        if (floor == null || floor.HasItem) { return false; }
        return !cell.IsTaken;
    }
}
