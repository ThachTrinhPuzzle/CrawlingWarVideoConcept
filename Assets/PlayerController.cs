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
    public int rotateSpeed;
    public float rotateRange;
    private float _delayTimeTemp;
    [SerializeField] [Range(1, 5)] private int numPerShoot = 1;

    private void Start()
    {
        onFire.onFired = Spawn;
    }

    void Update()
    {
        UpdateRotating();
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

    private void AnhGiangRotate()
    {
        if (!Input.GetMouseButton(0)) return;
        Vector3 mousePos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10);
        Vector3 lookPos = Camera.main.ScreenToWorldPoint(mousePos);
        lookPos = lookPos - transform.position;
        float angle = Mathf.Atan2(lookPos.x, lookPos.y) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.up);
    }

    private void UpdateRotating()
    {
        if (!Input.GetMouseButton(0)) return;
        float xInput = Input.GetAxis("Mouse X");
        transform.Rotate(xInput * rotateSpeed * Time.deltaTime * Vector3.up);
        Vector3 newEuler = transform.eulerAngles;
        float yEuler = newEuler.y;
        if (yEuler > 180) yEuler -= 360;
        else if (yEuler < -180) yEuler += 360;
        yEuler = Mathf.Clamp(yEuler, -rotateRange, rotateRange);        
        newEuler.y = yEuler;
        transform.eulerAngles = newEuler;
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
            ob.transform.position = spawnPoint.position;
            Instantiate(fxGun, spawnPoint.position, Quaternion.identity);
            ob.Force(spawnPoint.position + dir * 2);
        }
        
    }
}
