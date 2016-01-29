using System;
using System.Collections;
using System.Linq;
using UnityEngine;

class CustomObstacleGenerator : MonoBehaviour
{
    public Transform ObstaclesParent;
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
}

