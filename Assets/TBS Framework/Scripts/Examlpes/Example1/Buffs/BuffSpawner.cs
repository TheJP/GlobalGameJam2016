using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Applies buffs to units. It requires unit parent game object to be named "Units" to work.
/// </summary>
public class BuffSpawner
{
    private List<Unit> unitsList;

    public BuffSpawner()
    {
        var unitsParent = GameObject.Find("Units");
        unitsList = new List<Unit>();

        for (int i = 0; i < unitsParent.transform.childCount; i++)
        {
            var unit = unitsParent.transform.GetChild(i);
            unitsList.Add(unit.GetComponent<Unit>());
        }
    }

    /// <summary>
    /// Method applies areo of effect buffs to units.
    /// </summary>
    /// <param name="buff">
    /// Buff to apply.
    /// </param>
    /// <param name="buffer">
    /// Unit that "casted" the buff.
    /// <param name="self">
    /// Indicates if buff should be applied to its caster.
    /// </param>
    public void SpawnBuff(Buff buff, Cell center, Unit buffer, int range, bool self)
    {
        foreach (var unit in unitsList.FindAll(u => u.PlayerNumber.Equals(buffer.PlayerNumber) && u.Cell.GetDistance(center) <= range))
        {
            if (!self && unit.Equals(buffer)) continue;
            var _buff = buff.Clone();
            unit.Buffs.Add(_buff);
            _buff.Apply(unit);
        }
    }
}
