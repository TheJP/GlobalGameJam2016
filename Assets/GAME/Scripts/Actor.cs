using UnityEngine;
using System.Collections;

public class Actor : Unit {

    public bool gotItem;
    public GameObject item;
    
    public override void Initialize()
    {
        base.Initialize();
        HitPoints = PlayerPrefs.GetInt("Player_"+PlayerNumber+"_Hitpoints");
        AttackFactor = PlayerPrefs.GetInt("Player_" + PlayerNumber + "_AttackFactor");
        MovementPoints = PlayerPrefs.GetInt("Player_" + PlayerNumber + "_MovementPoints");
        ActionPoints = PlayerPrefs.GetInt("Player_" + PlayerNumber + "_ActionPoints");
        DefenceFactor = PlayerPrefs.GetInt("Player_" + PlayerNumber + "_DefenceFactor");
        /*        HitPoints = Random.Range(6, 10);
        AttackFactor = Random.Range(1,8);
        MovementPoints = Random.Range(5, 10);
        ActionPoints = Random.Range(1, 3);
        DefenceFactor = Random.Range(0, 3);*/
    }

    public override void OnUnitDeselected()
    {
        base.OnUnitDeselected();
    }

    public override void MarkAsAttacking(Unit other)
    {
        StartCoroutine(Jerk(other));
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
}
