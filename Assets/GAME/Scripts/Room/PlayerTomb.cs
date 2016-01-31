using UnityEngine;
using System.Collections;

public class PlayerTomb : MonoBehaviour
{

    void LateUpdate()
    {
        var position = transform.position;
        position.z = -0.01f;
        transform.position = position;
    }
}
