using System.Collections.Generic;
using System.Linq;
using UnityEngine;

class RandomUnitGenerator : MonoBehaviour, IUnitGenerator
{
    private System.Random _rnd = new System.Random();

    public Transform UnitsParent;

    public GameObject UnitPrefab;
    public int NumberOfPlayers;
    public int UnitsPerPlayer;

    public List<Unit> SpawnUnits(List<Cell> cells)
    {
        List<Unit> ret = new List<Unit>();

        List<Cell> freeCells = cells.FindAll(h => h.GetComponent<Cell>().IsTaken == false);
        freeCells = freeCells.OrderBy(h => _rnd.Next()).ToList();

        for (int i = 0; i < NumberOfPlayers; i++)
        {
            for (int j = 0; j < UnitsPerPlayer; j++)
            {
                var cell = freeCells.ElementAt(0);
                freeCells.RemoveAt(0);
                cell.GetComponent<Cell>().IsTaken = true;

                var unit = Instantiate(UnitPrefab);
                unit.transform.position = cell.transform.position + new Vector3(0, 0, 0);
                unit.GetComponent<Unit>().PlayerNumber = i;
                unit.GetComponent<Unit>().Cell = cell.GetComponent<Cell>();
                unit.GetComponent<Unit>().Initialize();
                unit.transform.parent = UnitsParent;


                ret.Add(unit.GetComponent<Unit>());
            }
        }
        return ret;
    }
}

