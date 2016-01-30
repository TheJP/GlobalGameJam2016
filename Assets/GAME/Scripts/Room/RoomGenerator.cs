using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RoomGenerator : MonoBehaviour, ICellGridGenerator
{
    public Transform cellsParent;
    public GameObject floorPrefab;
    public GameObject wallPrefab;
    public GameObject[] itemPrefabs;
    public int width = 20;
    public int height = 20;

    private Targets[] targets = TargetExtensions.GetTargets();

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
                result.Add(square.GetComponent<Cell>());

                square.transform.parent = cellsParent;
            }
        }
        SpawnItems(result);
        return result;
    }

    /// <summary>Spawns items of all targets in the room.</summary>
    private void SpawnItems(List<Cell> cells)
    {
        foreach(var target in targets)
        {
            var item = Instantiate(itemPrefabs[Random.Range(0, itemPrefabs.Length - 1)]);
            var itemScript = item.GetComponent<ItemBase>();
            itemScript.Target = target;
            Cell cell;
            do { cell = cells.GetRandomElement(); }
            while (!cell.IsSpawnable());
            //If IsSpawnable returned true it has to be a floor tile.
            var floor = cell.GetComponent<FloorTile>();
            floor.AddItem(itemScript);
        }
    }
}
