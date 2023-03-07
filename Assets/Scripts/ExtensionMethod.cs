using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Random = UnityEngine.Random;

public static class ExtensionMethod
{
    public static T ChonDai<T>(this T[] arr)
    {
        return arr[Random.Range(0, arr.Length)];
    }
}
