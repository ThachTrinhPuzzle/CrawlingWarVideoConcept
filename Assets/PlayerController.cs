using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private WhyAreYouRunning creepPrefab;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private Animator anim;
    [SerializeField] private AnimationEvent onFire;
    [SerializeField] private GameObject fxGun;
    [SerializeField] private float delayTime = 0.5f;
    private float _delayTimeTemp;
    [SerializeField] [Range(1, 5)] private int numPerShoot = 1;

    private void Start()
    {
        onFire.onFired = Spawn;
    }

    void Update()
    {
        Vector3 mousePos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10);
        Vector3 lookPos = Camera.main.ScreenToWorldPoint(mousePos);
        lookPos = lookPos - transform.position;
        float angle = Mathf.Atan2(lookPos.x, lookPos.y) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.up);

        if (Input.GetKey(KeyCode.Space))
        {
            _delayTimeTemp -= Time.deltaTime;

            if (_delayTimeTemp <= 0)
            {
                _delayTimeTemp = delayTime;
                anim.SetTrigger("fire");
            }
        }
    }

    void Spawn()
    {
        Vector3 mockPos = spawnPoint.forward - spawnPoint.position;
        float totalAngle = 90;
        float angle = totalAngle / numPerShoot;
        float globalAngle = spawnPoint.eulerAngles.y;
        
        for (int i = 0; i < numPerShoot; i++)
        {

            float sign = Mathf.Sign(180 - globalAngle);
            float rAngle = Mathf.Deg2Rad * (globalAngle - sign * (angle * i) + sign * totalAngle / 2);

            var dir = new Vector3(Mathf.Sin(rAngle), 0, Mathf.Cos(rAngle));
            var ob = Instantiate(creepPrefab);
            ob.GetComponent<Transform>().position = spawnPoint.position;
            var fx = Instantiate(fxGun);
            fx.transform.position = spawnPoint.position;

            ob.Force(spawnPoint.position + dir * 2);
        }
        
    }
}
