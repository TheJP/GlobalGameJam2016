using UnityEngine;
using System.Collections;

public class EnemyAdapter : ShadowWorldUnit
{
    public override void MarkAsAttacking(Unit other) { }

    public override void MarkAsDefending(Unit other) { }

    public override void MarkAsDestroyed() { }

    public override void MarkAsFinished() { }

    public override void MarkAsFriendly() { }

    public override void MarkAsReachableEnemy() { }

    public override void MarkAsSelected() { }

    public override void UnMark() { }
}
