using UnityEngine;

class MyHexagon : Hexagon
{
    public void Start()
    {
        SetColor(Color.white);
        SetOutlineColor(Color.black);
    }

    public override Vector3 GetCellDimensions()
    {
        var center = Vector3.zero;

        foreach (Transform child in transform.FindChild("Outline"))
        {
            if (child.GetComponent<Renderer>() == null)
                continue;
            center += child.GetComponent<Renderer>().bounds.center;
        }
        center /= transform.FindChild("Outline").childCount;

        Bounds ret = new Bounds(center, Vector3.zero);
        foreach (Transform child in transform.FindChild("Outline"))
        {
            if (child.GetComponent<Renderer>() == null)
                continue;
            ret.Encapsulate(child.GetComponent<Renderer>().bounds);
        }
        return ret.size;
    }

    public override void MarkAsReachable()
    {
        SetColor(Color.yellow);
    }
    public override void MarkAsPath()
    {
        SetColor(Color.green);;
    }
    public override void MarkAsHighlighted()
    {
        SetOutlineColor(Color.blue);
    }
    public override void UnMark()
    {
        SetColor(Color.white);
        SetOutlineColor(Color.black);
    }

    private void SetColor(Color color)
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            var rendererComponent = transform.GetChild(i).GetComponent<Renderer>();
            if (rendererComponent != null)
                rendererComponent.material.color = color;
        }
    }
    private void SetOutlineColor(Color color)
    {
        var outline = transform.FindChild("Outline");
        for (int i = 0; i < outline.transform.childCount; i++)
        {
            var rendererComponent = outline.transform.GetChild(i).GetComponent<Renderer>();
            if (rendererComponent != null)
                rendererComponent.material.color = color;
        }
    }

    
}

