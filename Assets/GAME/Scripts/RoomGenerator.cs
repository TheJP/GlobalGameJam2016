using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class RoomGenerator : MonoBehaviour, ICellGridGenerator
{
    public Transform cellsParent;
    public GameObject floorPrefab;
    public GameObject wallPrefab;
    public int width = 20;
    public int height = 20;

    /// <summary>
    /// Cretes room floor and walls.
    /// </summary>
    /// <returns></returns>
    public List<Cell> GenerateGrid()
    {
        var result = new List<Cell>();
        for (int j = 0; j < height; j++)
        {
            for (int i = 0; i < width; i++)
            {
                var square = Instantiate(floorPrefab);
                var squareSize = square.GetComponent<Cell>().GetCellDimensions();

                square.transform.position = new Vector3(i * squareSize.x, j * squareSize.y, 0);
                square.GetComponent<Cell>().OffsetCoord = new Vector2(i, j);
                square.GetComponent<Cell>().MovementCost = 1;
                result.Add(square.GetComponent<Cell>());

                square.transform.parent = cellsParent;
            }
        }
        return result;
    }
}
