using UnityEngine;
using System.Collections;

public class Rune : MonoBehaviour
{
    public Sprite[] runeSprites;

    private Targets itemTarget;
    public Targets Target
    {
        get { return itemTarget; }
        set
        {
            itemTarget = value;
            GetComponent<SpriteRenderer>().color = itemTarget.ToColor();
        }
    }

    void Start()
    {
        GetComponent<SpriteRenderer>().sprite = runeSprites[Random.Range(0, runeSprites.Length - 1)];
    }

    void FixedUpdate()
    {
        var position = transform.position;
        position.z = -0.025f;
        transform.position = position;
    }
}
