using UnityEngine;
using System.Collections;

public enum WallType
{
    OuterWall,
    Horizontal,
    Vertical
}

public class WallTile : Square
{
    public Sprite[] wallSprites;
    public Sprite defaultSprite;
    public bool useDefaultSprite = false;
    public WallType WallType { get; set; }

    private SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = useDefaultSprite ? defaultSprite : wallSprites[Random.Range(0, wallSprites.Length - 1)];
        IsTaken = true;
    }

    /// <summary>Forces the wall to use the default sprite. Used for a more uniformed look.</summary>
    public void UseDefaultSprite(bool value = true) { useDefaultSprite = value; }

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
