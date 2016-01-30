using UnityEngine;
using System.Collections;

public class FloorTile : Square
{
    public Sprite[] crackedSprites;
    public Sprite[] regularSprites;
    /// <summary>Determines how many regular sprites should exist in comparison to cracked ones.</summary>
    public int regularPerCrackedSprite = 7;

    private bool isCracked;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        isCracked = Random.Range(0, 7) == 0;
        if (isCracked) { spriteRenderer.sprite = crackedSprites[Random.Range(0, crackedSprites.Length - 1)]; }
        else { spriteRenderer.sprite = regularSprites[Random.Range(0, regularSprites.Length - 1)]; }
        if (hasItem) { AddItem(); }
    }

    private void AddItem()
    {
        Debug.Log("Add Item");
    }
    private void RemoveItem()
    {
        Debug.Log("Remove Item");
    }

    public override Vector3 GetCellDimensions()
    {
        return 0.98f * GetComponent<SpriteRenderer>().bounds.size;
    }

    public void OnLeftItem() {
        if (hasItem) { AddItem(); }
        else { RemoveItem(); }
    }

    public override void MarkAsHighlighted()
    {
        spriteRenderer.color = Color.red;
    }

    public override void MarkAsPath()
    {
        spriteRenderer.color = Color.yellow;
    }

    public override void MarkAsReachable()
    {
        spriteRenderer.color = Color.green;
    }

    public override void UnMark()
    {
        spriteRenderer.color = Color.white;
    }
}
