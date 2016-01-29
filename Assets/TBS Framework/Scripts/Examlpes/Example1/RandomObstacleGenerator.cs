using System;
using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class RandomObstacleGenerator : MonoBehaviour
{
    private System.Random _rnd = new System.Random();

    public int Amount;
    public Transform ObstaclesParent;
    public GameObject ObstaclePrefab;

    public CellGrid CellGrid;

    public void Start()
    {
        StartCoroutine(SpawnObstacles());
    }

    public IEnumerator SpawnObstacles()
    {
        while (CellGrid.Cells == null)
        {
            yield return 0;
        }

        var cells = CellGrid.Cells;

        List<GameObject> ret = new List<GameObject>();

        if (ObstaclesParent.childCount != 0)
        {
            for (int i = 0; i < ObstaclesParent.childCount; i++)
            {
                var obstacle = ObstaclesParent.GetChild(i);

                var cell = cells.OrderBy(h => Math.Abs((h.transform.position - obstacle.transform.position).magnitude)).First();
                if (!cell.IsTaken)
                {
                    cell.IsTaken = true;
                    obstacle.position = cell.transform.position + new Vector3(0, 0, -1f);
                }
                else
                {
                    Destroy(obstacle.gameObject);
                }
            }
        }

        List<Cell> freeCells = cells.FindAll(h => h.GetComponent<Cell>().IsTaken == false);
        freeCells = freeCells.OrderBy(h => _rnd.Next()).ToList();

        for (int i = 0; i < Mathf.Clamp(Amount,Amount,freeCells.Count); i++)
        {
            var cell = freeCells.ElementAt(i);
            cell.GetComponent<Cell>().IsTaken = true;

            var obstacle = Instantiate(ObstaclePrefab);
            obstacle.transform.position = cell.transform.position + new Vector3(0, 0, -0.5f);
            obstacle.transform.parent = ObstaclesParent.transform;
            ret.Add(obstacle);   
        }
    }
}
