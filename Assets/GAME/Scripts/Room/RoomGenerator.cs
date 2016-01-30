using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class RoomGenerator : MonoBehaviour, ICellGridGenerator
{
    public Transform cellsParent;
    public GameObject floorPrefab;
    public GameObject wallPrefab;
    public GameObject[] itemPrefabs;
    public int width = 20;
    public int height = 20;

    private Targets[] targets = TargetExtensions.GetTargets();
    private const int WallProbability = 25;
    private const int ContinueWallBonus = 12;
    private const int BranchWallBonus = -2;

    /// <summary>
    /// Cretes room floor and walls.
    /// </summary>
    /// <returns></returns>
    public List<Cell> GenerateGrid()
    {
        var result = new List<Cell>();
        WallType? previousWallType = null;
        var runes = new Stack<Targets>(TargetExtensions.GetTargets());
        for (int j = 0; j < height; ++j) 
        {
            for (int i = 0; i < width; ++i)
            {
                var isOuterWall = j <= 0 || i <= 0 || j + 1 >= height || i + 1 >= width;
                var isHorizontalWall = !isOuterWall && Random.Range(0, WallProbability) - WallBonus(previousWallType, WallType.Horizontal) <= 0;
                var isVerticalWall = !isOuterWall && !isHorizontalWall && Random.Range(0, WallProbability) - WallBonus(GetWallType(result[result.Count - width]), WallType.Vertical) <= 0;
                var square = Instantiate(isOuterWall || isHorizontalWall || isVerticalWall ? wallPrefab : floorPrefab);
                if (isOuterWall || isHorizontalWall || isVerticalWall)
                {
                    var wallType = isOuterWall ? WallType.OuterWall : (isHorizontalWall ? WallType.Horizontal : WallType.Vertical);
                    square.GetComponent<WallTile>().WallType = wallType;
                    previousWallType = wallType;
                }
                else { previousWallType = null; }
                if (isOuterWall){ square.GetComponent<WallTile>().UseDefaultSprite(); }

                var squareSize = square.GetComponent<Cell>().GetCellDimensions();

                square.transform.position = new Vector3(i * squareSize.x, j * squareSize.y, 0);
                square.GetComponent<Cell>().OffsetCoord = new Vector2(i, j);
                result.Add(square.GetComponent<Cell>());

                square.transform.parent = cellsParent;
            }
        }
        SpawnRunes(result);
        SpawnItems(result);
        return result;
    }

    /// <summary>Spawns run places for every posible target.</summary>
    private void SpawnRunes(List<Cell> cells)
    {
        foreach (var target in targets)
        {
            Cell cell;
            do { cell = cells.GetRandomElement(); }
            while (!cell.IsSpawnable());
            //If IsSpawnable returned true it has to be a floor tile.
            var floor = cell.GetComponent<FloorTile>();
            floor.Rune = target;
        }
    }

    /// <summary>Spawns items of all targets in the room.</summary>
    private void SpawnItems(List<Cell> cells)
    {
        foreach(var target in targets)
        {
            var item = Instantiate(itemPrefabs[Random.Range(0, itemPrefabs.Length)]);
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

    private int WallBonus(WallType? wallType, WallType reference)
    {
        if (!wallType.HasValue) { return 0; }
        if (wallType.Value == reference) { return ContinueWallBonus; }
        if (wallType.Value == WallType.OuterWall) { return BranchWallBonus; }
        return 0;
    }

    private WallType? GetWallType(Cell cell)
    {
        var wall = cell.GetComponent<WallTile>();
        if (wall != null) { return wall.WallType; }
        return null;
    }
}
