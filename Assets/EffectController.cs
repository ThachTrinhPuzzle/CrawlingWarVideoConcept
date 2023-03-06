using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class EffectController : MonoBehaviour
{
    private float duration;
    [SerializeField] private ParticleSystem ps;
    private Coroutine co;
    
    private void OnEnable()
    {
        duration = ps.main.duration;

        if (co != null)
        {
            StopCoroutine(co);
        }
        
        co = StartCoroutine(Play());
    }

    private IEnumerator Play()
    {
        yield return new WaitForSeconds(duration);
        Destroy(gameObject);
    }
}
