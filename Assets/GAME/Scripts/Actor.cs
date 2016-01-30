using UnityEngine;
using System.Collections;

public class Actor : ShadowWorldUnit
{
    public bool gotItem;
    public override void Initialize()
    {
        base.Initialize();
        HitPoints = PlayerPrefs.GetInt("Player_" + PlayerNumber + "_Hitpoints");
        if (HitPoints == 0)
            HitPoints = 1;
        AttackFactor = PlayerPrefs.GetInt("Player_" + PlayerNumber + "_AttackFactor");
        MovementPoints = PlayerPrefs.GetInt("Player_" + PlayerNumber + "_MovementPoints");
        ActionPoints = PlayerPrefs.GetInt("Player_" + PlayerNumber + "_ActionPoints");
        DefenceFactor = PlayerPrefs.GetInt("Player_" + PlayerNumber + "_DefenceFactor");
    }

    public override void OnUnitDeselected()
    {
        base.OnUnitDeselected();
    }

    public override void MarkAsAttacking(Unit other)
    {
        //StartCoroutine(Jerk(other));
    }
    public override void MarkAsDefending(Unit other)
    {

    }
    public override void MarkAsDestroyed()
    {
    }

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

    public override void MarkAsFriendly()
    {
        SetColor(new Color(0.8f, 1, 0.8f));
    }
    public override void MarkAsReachableEnemy()
    {
        SetColor(new Color(1, 0.8f, 0.8f));
    }
    public override void MarkAsSelected()
    {
        SetColor(new Color(0.8f, 0.8f, 1));
    }
    public override void MarkAsFinished()
    {
        if (Cell.hasItem && !gotItem)
        {
            Cell.hasItem = false;
            Cell.GetComponent<FloorTile>().OnLeftItem();
        }
        SetColor(Color.gray);
    }
    public override void UnMark()
    {
        SetColor(Color.white);
    }

    private void SetColor(Color color)
    {
        var _renderer = GetComponent<SpriteRenderer>();
        if (_renderer != null)
        {
            _renderer.color = color;
        }
    }

    public void ThrowItem()
    {
        gotItem = false;
        Cell.hasItem = true;
        Cell.GetComponent<FloorTile>().OnLeftItem();
    }
}
