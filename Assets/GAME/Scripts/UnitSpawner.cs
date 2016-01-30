using System.Collections.Generic;
using System.Linq;
using UnityEngine;

class UnitSpawner : MonoBehaviour, IUnitGenerator
{
    public Transform unitsParent;
    public GameObject shadowStalker;

    public int minimumShadowStalker = 1;
    public int maximumShadowStalker = 5;

    /// <summary>
    /// Returns units that are already children of UnitsParent object
    /// and also adds enemy units.
    /// </summary>
    public List<Unit> SpawnUnits(List<Cell> cells)
    {
        List<Unit> result = new List<Unit>();
        for (int i = 0; i < unitsParent.childCount; i++)
        {
            var unit = unitsParent.GetChild(i).GetComponent<Unit>();
            if (unit != null)
            {
                var cell = cells.OrderBy(h => System.Math.Abs((h.transform.position - unit.transform.position).magnitude)).First();
                if (!cell.IsTaken)
                {
                    cell.IsTaken = true;
                    unit.Cell = cell;
                    unit.transform.position = cell.transform.position;
                    unit.Initialize();
                    result.Add(unit);
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
        SpawnEnemies(cells, result);
        return result;
    }

    /// <summary>Spawns all needed enemies.</summary>
    private void SpawnEnemies(List<Cell> cells, List<Unit> result)
    {
        SpawnEnemiesOfType(cells, result, shadowStalker, minimumShadowStalker, maximumShadowStalker);
    }

    /// <summary>
    /// Spawns enemies of the given prefab type.
    /// </summary>
    /// <param name="cells"></param>
    /// <param name="result">New enemies will be added to this list.</param>
    /// <param name="prefab">Prefab, which determines the enemy type.</param>
    /// <param name="min">Minimum number of enemies to be spawned.</param>
    /// <param name="max">Maximum number of enemies to be spawned.</param>
    private void SpawnEnemiesOfType(List<Cell> cells, List<Unit> result, GameObject prefab, int min, int max)
    {
        var enemies = Random.Range(min, max);
        for (int i = 0; i < enemies; ++i)
        {
            //Choose cell where the shadow stalker should be spawned
            Cell cell;
            do { cell = cells.GetRandomElement(); }
            while (cell.IsTaken);
            var enemyObject = (GameObject)Instantiate(prefab, cell.transform.position, Quaternion.identity);
            enemyObject.transform.parent = unitsParent.transform;
            //Initialize unit
            var unit = enemyObject.GetComponent<Unit>();
            unit.Cell = cell;
            unit.Initialize();
            result.Add(unit);
        }
    }
}
