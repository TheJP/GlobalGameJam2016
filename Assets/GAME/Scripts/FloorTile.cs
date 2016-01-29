using UnityEngine;
using System.Collections;
using System;

public class FloorTile : Square
{

    public Sprite defaultSprite;
    public Sprite highlightedSprite;
    public Sprite markedAsPathSprite;
    public Sprite markedAsReachableSprite;

    private SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {

    }

    public override Vector3 GetCellDimensions()
    {
        return 0.98f * GetComponent<SpriteRenderer>().bounds.size;
    }

    public override void MarkAsHighlighted()
    {
        spriteRenderer.sprite = highlightedSprite;
    }

    public override void MarkAsPath()
    {
        spriteRenderer.sprite = markedAsPathSprite;
    }

    public override void MarkAsReachable()
    {
        spriteRenderer.sprite = markedAsReachableSprite;
    }

    public override void UnMark()
    {
        spriteRenderer.sprite = defaultSprite;
    }
}
