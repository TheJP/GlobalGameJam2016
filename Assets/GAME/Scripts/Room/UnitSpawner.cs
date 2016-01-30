using System.Collections.Generic;
using System.Linq;
using UnityEngine;

class UnitSpawner : MonoBehaviour, IUnitGenerator
{
    public RoomGenerator roomGenerator;
    public Transform unitsParent;
    public GameObject[] playerPrefabs;
    public GameObject[] enemies;

    public int numberOfPlayers = 2;
    public int minimumShadowStalker = 0;
    public int maximumShadowStalker = 3;
    public int minimumShadowLurker = 1;
    public int maximumShadowLurker = 5;
    public int minimumShadowArchDemon = 0;
    public int maximumShadowArchDemon = 1;

    void Start()
    {
        numberOfPlayers = PlayerPrefs.GetInt("Players_Number", 1);
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
        SpawnEnemiesOfType(cells, result, enemies[0], minimumShadowStalker, maximumShadowStalker);
        SpawnEnemiesOfType(cells, result, enemies[1], minimumShadowLurker, maximumShadowLurker);
        SpawnEnemiesOfType(cells, result, enemies[2], minimumShadowArchDemon, maximumShadowArchDemon);
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
        var enemies = Random.Range(min, max + 1);
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
            var player = (GameObject)Instantiate(playerPrefabs[i], cell.transform.position, Quaternion.identity);
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
