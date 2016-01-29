using UnityEngine;

public class MyOtherHexagon : Hexagon
{
    public GroundType GroundType;
    public bool IsSkyTaken;//Indicates if a flying unit is occupying the cell.

    public void Start()
    {
        SetColor(new Color(1,1,1,0));
    }

    public override void MarkAsReachable()
    {
        SetColor(new Color(1, 0.92f, 0.016f, 1));
    }
    public override void MarkAsPath()
    {
        SetColor(new Color(0,1,0,1));
    }
    public override void MarkAsHighlighted()
    {
        SetColor(new Color(0.5f,0.5f,0.5f,0.25f));
    }
    public override void UnMark()
    {
        SetColor(new Color(1,1,1,0));
    }

    private void SetColor(Color color)
    {
        var highlighter = transform.FindChild("Highlighter");
        var spriteRenderer = highlighter.GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            spriteRenderer.color = color;
        }
        foreach (Transform child in highlighter.transform)
        {
            var childColor = new Color(color.r,color.g,color.b,1);
            spriteRenderer = child.GetComponent<SpriteRenderer>();
            if (spriteRenderer == null) continue;

            child.GetComponent<SpriteRenderer>().color = childColor;
        }
    }

    public override Vector3 GetCellDimensions()
    {
        var ret = GetComponent<SpriteRenderer>().bounds.size;
        return ret*0.98f;
    }
}

public enum GroundType
{
    Land,
    Water
};