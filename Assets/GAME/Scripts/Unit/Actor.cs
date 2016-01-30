﻿using UnityEngine;
using System.Collections;

public class Actor : ShadowWorldUnit
{
    private ItemBase item = null;
    public bool HasItem { get { return item != null; } }

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
        UnitMoved += ActorUnitMoved;
    }

    private void ActorUnitMoved(object sender, MovementEventArgs e)
    {
        var floor = Cell.GetComponent<FloorTile>();
        if (floor != null)
        {
            if (floor.HasItem && !HasItem) { this.item = floor.RemoveItem(); }
        }
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
        var floor = Cell.GetComponent<FloorTile>();
        if (floor != null)
        {
            floor.AddItem(this.item);
            this.item = null;
        }
    }
}