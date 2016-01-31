using UnityEngine;
using System.Collections;

public abstract class ShadowWorldUnit : Unit
{
    void LateUpdate()
    {
        var position = transform.position;
        position.z = -0.1f;
        transform.position = position;
    }

    public override void OnTurnStart()
    {
        base.OnTurnStart();
        GetComponentInChildren<Light>().enabled = true;
    }

    public override void OnTurnEnd()
    {
        base.OnTurnEnd();
        GetComponentInChildren<Light>().enabled = false;
    }

    public override void MarkAsAttacking(Unit other)
    {
        var gameManager = FindObjectOfType<GameManager>();
        gameManager.PlaySound(gameManager.attackSoundEffect);
    }
}
