using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AnimationEvent : MonoBehaviour
{
    public UnityAction onFired;

    void OnFire()
    {
        onFired?.Invoke();
    }
}
