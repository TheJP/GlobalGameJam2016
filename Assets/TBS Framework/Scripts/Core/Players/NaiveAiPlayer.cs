using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Simple implementation of AI for the game.
/// </summary>
public class NaiveAiPlayer : Player
{
    private CellGrid _cellGrid;
    private System.Random _rnd;

    public NaiveAiPlayer()
    {
        _rnd = new System.Random();
    }

    public override void Play(CellGrid cellGrid)
    {
        cellGrid.CellGridState = new CellGridStateAiTurn(cellGrid);
        _cellGrid = cellGrid;

        StartCoroutine(Play()); //Coroutine is necessary to allow Unity to run updates on other objects (like UI).
                                //Implementing this with threads would require a lot of modifications in other classes, as Unity API is not thread safe.
    }
    private IEnumerator Play()
    {
        var myUnits = _cellGrid.Units.FindAll(u => u.PlayerNumber.Equals(PlayerNumber)).ToList();
        foreach (var unit in myUnits.OrderByDescending(u => u.Cell.GetNeighbours(_cellGrid.Cells).FindAll(u.IsCellTraversable).Count))
        {
            var enemyUnits = _cellGrid.Units.Except(myUnits).ToList();
            var unitCell = unit.Cell;
            var unitsInRange = new List<Unit>();
            foreach (var enemyUnit in enemyUnits)
            {
                var enemyCell = enemyUnit.Cell;
                if (enemyCell.GetDistance(unitCell) <= unit.AttackRange)
                {
                    unitsInRange.Add(enemyUnit);
                }
            }//Looking for enemies that are in attack range.
            if (unitsInRange.Count != 0)
            {
                var index = _rnd.Next(0, unitsInRange.Count);
                unit.DealDamage(unitsInRange[index]);
                yield return new WaitForSeconds(0.5f);
                continue;
            }//If there is an enemy in range, attack it.

            List<Cell> potentialDestinations = new List<Cell>();
            
            foreach (var enemyUnit in enemyUnits)
            {
                potentialDestinations.AddRange(enemyUnit.Cell.GetNeighbours(_cellGrid.Cells).FindAll(unit.IsCellMovableTo));
            }//Making a list of cells adjacent to enemies.
            potentialDestinations = potentialDestinations.OrderBy(h => _rnd.Next()).ToList();

            List<Cell> shortestPath = null;
            foreach (var potentialDestination in potentialDestinations)
            {
                var path = unit.FindPath(_cellGrid.Cells, potentialDestination);
                if ((shortestPath == null && path.Sum(h => h.MovementCost) > 0) || shortestPath != null && (path.Sum(h => h.MovementCost) < shortestPath.Sum(h => h.MovementCost) && path.Sum(h => h.MovementCost) > 0))
                    shortestPath = path;

                var pathCost = path.Sum(h => h.MovementCost);
                if (pathCost > 0 && pathCost <= unit.MovementPoints)
                {
                    unit.Move(potentialDestination, path);
                    while (unit.isMoving)
                        yield return 0;
                    shortestPath = null;
                    break;
                }
                yield return 0;
            }//If there is a path to any of the cells adjacent to enemy, move there.

            if (shortestPath != null)
            {      
                foreach (var potentialDestination in shortestPath.Intersect(unit.GetAvailableDestinations(_cellGrid.Cells)).OrderByDescending(h => h.GetDistance(unit.Cell)))
                {
                    var path = unit.FindPath(_cellGrid.Cells, potentialDestination);
                    var pathCost = path.Sum(h => h.MovementCost);
                    if (pathCost > 0 && pathCost <= unit.MovementPoints)
                    {
                        unit.Move(potentialDestination, path);
                        while (unit.isMoving)
                            yield return 0;
                        break;
                    }
                    yield return 0;
                }
            }//If the path cost is greater than unit movement points, move as far as possible.
           
            unitCell = unit.Cell;
            foreach (var enemyUnit in enemyUnits)
            {
                var enemyCell = enemyUnit.Cell;
                if (enemyCell.GetDistance(unitCell) <= unit.AttackRange)
                { 
                    unit.DealDamage(enemyUnit);
                    yield return new WaitForSeconds(0.5f);
                    break;
                }
            }//Look for enemies in range and attack.
        }    
        _cellGrid.EndTurn();     
    }
}