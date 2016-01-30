using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

class UnitSpawner : MonoBehaviour, IUnitGenerator
{
    public Transform UnitsParent;

    /// <summary>
    /// Returns units that are already children of UnitsParent object
    /// and also adds enemy units.
    /// </summary>
    public List<Unit> SpawnUnits(List<Cell> cells)
    {
        List<Unit> ret = new List<Unit>();
        for (int i = 0; i < UnitsParent.childCount; i++)
        {
            var unit = UnitsParent.GetChild(i).GetComponent<Unit>();
            if (unit != null)
            {
                var cell = cells.OrderBy(h => Math.Abs((h.transform.position - unit.transform.position).magnitude)).First();
                if (!cell.IsTaken)
                {
                    cell.IsTaken = true;
                    unit.Cell = cell;
                    unit.transform.position = cell.transform.position;
                    unit.Initialize();
                    ret.Add(unit);
                }//Unit gets snapped to the nearest cell
                else
                {
                    Destroy(unit.gameObject);
                }//If the nearest cell is taken, the unit gets destroyed.
            }
            else
            {
                Debug.LogError("Invalid object in Units Parent game object");
            }

        }
        return ret;
    }

    private void SpawnEnemies(List<Cell> cells, List<Unit> result)
    {
        
    }
}
