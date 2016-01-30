using UnityEngine;
using System.Collections;
using System;

public class FlameItem : ItemBase
{
    private Targets itemTarget;
    public override Targets Target
    {
        get { return itemTarget; }
        set
        {
            itemTarget = value;
            ColorUpdate(color => Target.ToColor());
        }
    }

    public override void Hide()
    {
        ColorUpdate(color => { color.a = 0.0f; return color; });
    }

    public override void Show()
    {
        ColorUpdate(color => { color.a = 1.0f; return color; });
    }

    private void ColorUpdate(Func<Color, Color> update)
    {
        var spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null) { spriteRenderer.color = update(spriteRenderer.color); }
    }
}
