using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Generates rectangular shaped grid of hexagons.
/// </summary>
[ExecuteInEditMode()]
class RectangularHexGridGenerator : MonoBehaviour, ICellGridGenerator
{
    public Transform CellsParent;
    public GameObject HexagonPrefab;
    public int Height;
    public int Width;

    void Start()
    {
        if(Application.isEditor)
            GenerateGrid();
    }

    public List<Cell> GenerateGrid()
    {
        HexGridType hexGridType = Width % 2 == 0 ? HexGridType.even_q : HexGridType.odd_q;
        List<Cell> hexagons = new List<Cell>();

        if (HexagonPrefab.GetComponent<Hexagon>() == null)
        {
            Debug.LogError("Invalid hexagon prefab provided");
            return hexagons;
        }

        for (int i = 0; i < Height; i++)
        {
            for (int j = 0; j < Width; j++)
            {
                GameObject hexagon = Instantiate(HexagonPrefab);
                var hexSize = hexagon.GetComponent<Cell>().GetCellDimensions();

                hexagon.transform.position = new Vector3((j * hexSize.x * 0.75f), (i * hexSize.y) + (j%2 == 0 ? 0 : hexSize.y * 0.5f));
                hexagon.GetComponent<Hexagon>().OffsetCoord = new Vector2(Width - j - 1, Height - i - 1);
                hexagon.GetComponent<Hexagon>().HexGridType = hexGridType;
                hexagon.GetComponent<Hexagon>().MovementCost = 1;
                hexagons.Add(hexagon.GetComponent<Cell>());

                hexagon.transform.parent = CellsParent;
            }
        }
        return hexagons;
    }
}

