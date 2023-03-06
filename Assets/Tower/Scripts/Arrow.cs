using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    private Transform _target;
    [SerializeField] private float speed;
    private bool isHas;
    

    private void Update()
    {
        if (_target)
        {
            Vector3 pos1 = transform.position;
            pos1.y = 0;
            Vector3 pos2 = _target.position;
            pos2.y = 0;
            var a = pos2 - pos1;
            
            float angle = Vector3.SignedAngle(a, Vector3.forward, Vector3.up);

            transform.rotation =
                Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, -angle, 0), 100 * Time.deltaTime);

            transform.position = Vector3.MoveTowards(transform.position, _target.position, speed * Time.deltaTime);
        }
        else
        {
            if (isHas)
            {
                Destroy(gameObject);
            }
        }
    }

    public void SetTarget(Transform target)
    {
        _target = target;
        isHas = true;
        
        Invoke(nameof(AutoDestroy), 5);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            var pl = other.GetComponent<WhyAreYouRunning>();
            if (pl != null && pl.Team == Team.B)
            {
                Destroy(gameObject);
                Destroy(other.gameObject);
            }
        }
    }

    void AutoDestroy()
    {
        Destroy(gameObject);
    }
}
