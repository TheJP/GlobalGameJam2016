using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class Utility
{
    public static T GetRandomElement<T>(this IList<T> list)
    {
        return list[Random.Range(0, list.Count - 1)];
    }
}
