using System.Collections.Generic;
using System.Linq;
using UnityEngine;

class UnitSpawner : MonoBehaviour, IUnitGenerator
{
    public RoomGenerator roomGenerator;
    public Transform unitsParent;
    public GameObject playerPrefab;
    public GameObject shadowStalker;

    public int numberOfPlayers = 2;
    public int minimumShadowStalker = 1;
    public int maximumShadowStalker = 5;

    void Start()
    {
        roomGenerator = GetComponent<RoomGenerator>();
    }

    /// <summary>
    /// Returns units that are already children of UnitsParent object
    /// and also adds enemy units.
    /// </summary>
    public List<Unit> SpawnUnits(List<Cell> cells)
    {
        List<Unit> result = new List<Unit>();
        SpawnPlayers(cells, result);
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
            while (!cell.IsSpawnable());
            var enemyObject = (GameObject)Instantiate(prefab, cell.transform.position, Quaternion.identity);
            enemyObject.transform.parent = unitsParent;
            //Initialize unit
            cell.IsTaken = true;
            var unit = enemyObject.GetComponent<Unit>();
            unit.Cell = cell;
            unit.Initialize();
            result.Add(unit);
        }
    }

    /// <summary>Spawns one unit per player.</summary>
    private void SpawnPlayers(List<Cell> cells, List<Unit> result)
    {
        //Spawn players in the middle of the bottom layer
        var position = roomGenerator.width / 2 - numberOfPlayers / 2 + 1 + roomGenerator.width;
        for (int i = 0; i < numberOfPlayers; ++i)
        {
            Cell cell;
            do { cell = cells[position--]; }
            while (!cell.IsSpawnable());
            var player = (GameObject)Instantiate(playerPrefab, cell.transform.position, Quaternion.identity);
            player.transform.parent = unitsParent;
            //Initialize unit
            cell.IsTaken = true;
            var unit = player.GetComponent<Unit>();
            unit.PlayerNumber = i;
            unit.Cell = cell;
            unit.Initialize();
            result.Add(unit);
        }
    }
}
