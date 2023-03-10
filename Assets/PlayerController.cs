using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance;
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
    
    private void Awake() => Instance = this;
    private void Start()
    {
        onFire.onFired = SpawnTroop;
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
        float xInput = Input.GetAxis("Horizontal");
        transform.Rotate(-xInput * rotateSpeed * Time.deltaTime * Vector3.up);
        Vector3 newEuler = transform.eulerAngles;
        float yEuler = newEuler.y;
        if (yEuler > 180) yEuler -= 360;
        else if (yEuler < -180) yEuler += 360;
        yEuler = Mathf.Clamp(yEuler, -rotateRange, rotateRange);        
        newEuler.y = yEuler;
        transform.eulerAngles = newEuler;
    }


    void SpawnTroop()
    {
        float totalAngle = numPerShoot > 1 ? 90 : 0;
        float deltaAngle = 0;
        if (numPerShoot > 1)
        {
            deltaAngle = totalAngle / (numPerShoot - 1);
        }
        Quaternion direction = spawnPoint.rotation * Quaternion.Euler(-90, 0, 0);
        for (int i = 0; i < numPerShoot; i++)
        {
            WhyAreYouRunning troop = Instantiate(creepPrefab, spawnPoint.position, direction * Quaternion.Euler(0, -totalAngle / 2 + i * deltaAngle, 0));
            Instantiate(fxGun, spawnPoint.position, Quaternion.identity);
            StartCoroutine(troop.FreezeYPosIEnumerator());
            troop.transform.DORotate(spawnPoint.eulerAngles + new Vector3(-90, 0, 0), 1f);
        }
        
    }
}
