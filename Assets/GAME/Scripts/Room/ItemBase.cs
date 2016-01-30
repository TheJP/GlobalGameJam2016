using UnityEngine;
using System.Collections;

public abstract class ItemBase : MonoBehaviour
{
    public abstract Targets Target { get; set; }
    public abstract void Hide();
    public abstract void Show();
    void LateUpdate()
    {
        var position = transform.position;
        position.z = -0.05f; //Between units and floor
        transform.position = position;
    }
}
