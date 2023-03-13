using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ThachTest : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"touch {other.name}");
    }
}
