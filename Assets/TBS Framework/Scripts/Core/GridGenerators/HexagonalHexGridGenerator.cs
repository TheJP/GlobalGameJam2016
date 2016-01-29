using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Generates hexagonal shaped grid of hexagons.
/// </summary>
[ExecuteInEditMode()]
class HexagonalHexGridGenerator : MonoBehaviour, ICellGridGenerator
{
    public Transform CellsParent;
    public GameObject HexagonPrefab;
    public int Radius;

    void Start()
    {
        if (Application.isEditor)
            GenerateGrid();
    }

    public List<Cell> GenerateGrid()
    {
        var hexagons = new List<Cell>();

        if (HexagonPrefab.GetComponent<Hexagon>() == null)
        {
            Debug.LogError("Invalid hexagon prefab provided");
            return hexagons;
        }

        for (int i = 0; i < Radius; i++)
        {
            for (int j = 0; j < (Radius*2) - i - 1; j++)
            {
                GameObject hexagon = Instantiate(HexagonPrefab);
                var hexSize = hexagon.GetComponent<Cell>().GetCellDimensions();

                hexagon.transform.position = new Vector3((i * hexSize.x * 0.75f),(i * hexSize.y * 0.5f) + (j * hexSize.y));
                hexagon.GetComponent<Hexagon>().OffsetCoord = new Vector2(i, Radius - j - 1 - (i/2));
                hexagon.GetComponent<Hexagon>().HexGridType = HexGridType.odd_q;
                hexagon.GetComponent<Hexagon>().MovementCost = 1;
                hexagons.Add(hexagon.GetComponent<Cell>());

                hexagon.transform.parent = CellsParent;

                if (i == 0) continue;

                GameObject hexagon2 = Instantiate(HexagonPrefab);
                hexagon2.transform.position = new Vector3((-i * hexSize.x * 0.75f), (i * hexSize.y * 0.5f) + (j * hexSize.y));
                hexagon2.GetComponent<Hexagon>().OffsetCoord = new Vector2(-i, Radius - j - 1 - (i/2));
                hexagon2.GetComponent<Hexagon>().HexGridType = HexGridType.odd_q;
                hexagon2.GetComponent<Hexagon>().MovementCost = 1;
                hexagons.Add(hexagon2.GetComponent<Cell>());

                hexagon2.transform.parent = CellsParent;
            }
        }   
        return hexagons;
    }
}

