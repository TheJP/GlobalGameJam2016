using UnityEngine;
using System.Collections;

public class FloorTile : Square
{
    public Sprite[] crackedSprites;
    public Sprite[] regularSprites;
    public Rune runePrefab;
    /// <summary>Determines how many regular sprites should exist in comparison to cracked ones.</summary>
    public int regularPerCrackedSprite = 7;

    private Rune rune;
    public Targets? Rune
    {
        get { return rune == null ? (Targets?)null : rune.Target; }
        set
        {
            if(!value.HasValue && rune != null)
            {
                Destroy(rune);
                rune = null;
            }
            else if(rune == null && value.HasValue)
            {
                rune = Instantiate(runePrefab);
                rune.transform.parent = transform;
                rune.transform.position = transform.position;
                rune.Target = value.Value;
            }
        }
    }

    private ItemBase item = null;
    public bool HasItem { get { return item != null; } }
    private bool isCracked;
    private SpriteRenderer spriteRenderer;


    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        isCracked = Random.Range(0, 7) == 0;
        if (isCracked) { spriteRenderer.sprite = crackedSprites[Random.Range(0, crackedSprites.Length - 1)]; }
        else { spriteRenderer.sprite = regularSprites[Random.Range(0, regularSprites.Length - 1)]; }
    }

    public void AddItem(ItemBase item)
    {
        this.item = item;
        item.Show();
        item.transform.position = transform.position;
    }
    public ItemBase RemoveItem()
    {
        var item = this.item;
        item.Hide();
        this.item = null;
        return item;
    }

    public override Vector3 GetCellDimensions()
    {
        return 0.98f * GetComponent<SpriteRenderer>().bounds.size;
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
