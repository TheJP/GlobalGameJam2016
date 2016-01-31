using UnityEngine;
using System.Collections;

public class EnemyAdapter : ShadowWorldUnit
{
    public override void MarkAsAttacking(Unit other) {
        StartCoroutine(Jerk(other));
    }

    public override void MarkAsDefending(Unit other) { }

    public override void MarkAsDestroyed() {
        PlayerPrefs.SetInt("Enemies_Killed", PlayerPrefs.GetInt("Enemies_Killed") + 1);
    }

    public override void MarkAsFinished() { }

    public override void MarkAsFriendly() { }

    public override void MarkAsReachableEnemy() { }

    public override void MarkAsSelected() { }

    public override void UnMark() { }

    private IEnumerator Jerk(Unit other)
    {
        GetComponent<SpriteRenderer>().sortingOrder = 6;
        var heading = other.transform.position - transform.position;
        var direction = heading / heading.magnitude;
        float startTime = Time.time;

        while (startTime + 0.25f > Time.time)
        {
            transform.position = Vector3.Lerp(transform.position, transform.position + (direction / 50f), ((startTime + 0.25f) - Time.time));
            yield return 0;
        }
        startTime = Time.time;
        while (startTime + 0.25f > Time.time)
        {
            transform.position = Vector3.Lerp(transform.position, transform.position - (direction / 50f), ((startTime + 0.25f) - Time.time));
            yield return 0;
        }
        transform.position = Cell.transform.position + new Vector3(0, 0, -0.1f);
        GetComponent<SpriteRenderer>().sortingOrder = 4;
    }
}
