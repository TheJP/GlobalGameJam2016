using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Generates equilateral shaped grid of hexagons.
/// </summary>
[ExecuteInEditMode()]
public class EquilateralHexGridGenerator : MonoBehaviour, ICellGridGenerator
{
    public Transform CellsParent;
    public GameObject HexagonPrefab;
    public int Side_a;
    public int Side_b;

    void Start()
    {
        if (Application.isEditor)
            GenerateGrid();
    }
        
    public List<Cell> GenerateGrid()
    {
        var hexGridType = Side_a % 2 == 0 ? HexGridType.even_q : HexGridType.odd_q; ;
        var hexagons = new List<Cell>();

        if(HexagonPrefab.GetComponent<Hexagon>() == null)
        {
            Debug.LogError("Invalid hexagon prefab provided");
            return hexagons;
        }

        for (int i = 0; i < Side_a; i++)
        {
            for (int j = 0; j < Side_b; j++)
            {
                GameObject hexagon = Instantiate(HexagonPrefab);

                var hexSize = hexagon.GetComponent<Cell>().GetCellDimensions();

                hexagon.transform.position = new Vector3((i * hexSize.x * 0.75f), (i * hexSize.y * 0.5f) + (j * hexSize.y));
                hexagon.GetComponent<Hexagon>().OffsetCoord = new Vector2(Side_a - i - 1, Side_b - j - 1 - (i/2));
                hexagon.GetComponent<Hexagon>().HexGridType = hexGridType;
                hexagon.GetComponent<Hexagon>().MovementCost = 1;
                hexagons.Add(hexagon.GetComponent<Cell>());

                hexagon.transform.parent = CellsParent;
            }
        }
        return hexagons;
    }
}
