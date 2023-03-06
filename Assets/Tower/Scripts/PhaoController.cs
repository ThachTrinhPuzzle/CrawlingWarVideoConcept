using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhaoController : MonoBehaviour
{
    [SerializeField] private Transform horizontalTra;

    private Transform target;
    [SerializeField] private float rotationSpeed = 200f;
    [SerializeField] private float fireTime = 0.5f;
    [SerializeField] private PhaoBullet bulletPrefab;
    [SerializeField] private Animator animator;
    [SerializeField] private AnimationEvent animationEvent;
    [SerializeField] private Transform spawnPoint;

    [SerializeField] private List<Transform> enemies;

    public List<Transform> Enemies
    {
        get => enemies;
        set => enemies = value;
    }

    [SerializeField] private float _minRadius = 3f;
    
    private float _fireTimeTemp;

    private void Start()
    {
        animationEvent.onFired = Fired;
        StartCoroutine(FindTarget());
    }

    IEnumerator FindTarget()
    {
        yield return new WaitForSeconds(0.5f);
        target = GetNearestEnemy();
        StartCoroutine(FindTarget());
    }

    private void Update()
    {
        if (target)
        {
            MoveHorizontal();
//            MoveVertical();

            _fireTimeTemp -= Time.deltaTime;
            if (_fireTimeTemp <= 0)
            {
                _fireTimeTemp = fireTime;
                animator.SetTrigger("fire");
            }
        }
    }

    void MoveHorizontal()
    {
        Vector3 pos1 = horizontalTra.position;
        pos1.y = 0;

        Vector3 pos2 = target.position;
        pos2.y = 0;

        var a = pos2 - pos1;


        float angle = Vector3.SignedAngle(a, Vector3.forward, Vector3.up);
            
        Quaternion targetRotation = Quaternion.Euler(0, -angle, 0);

        horizontalTra.rotation = Quaternion.RotateTowards(horizontalTra.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }
    
    Transform GetNearestEnemy()
    {
        if (enemies.Count <= 0)
        {
            return null;
        }

        Transform _target = null;
        float distance = float.MaxValue;

        for (int i = 0; i < enemies.Count; i++)
        {
            if (enemies[i] == null)
            {
                enemies.RemoveAt(i);
                i--;
                continue;
            }

            var dis = Vector3.Distance(transform.position, enemies[i].position);

            if (dis < distance && dis > _minRadius)
            {
                distance = dis;
                _target = enemies[i];
            }
        }

        return _target;
    }

    void Fired()
    {
        if (target == null)
        {
            target = GetNearestEnemy();
        }

        var bullet = Instantiate(bulletPrefab);
        bullet.transform.position = spawnPoint.position;
        bullet.Fire(target.position);
    }
}
