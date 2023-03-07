using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

public class TowerController : MonoBehaviour
{
    [SerializeField] private Material _bowMat;
    [SerializeField] private Material _towerRedMat;
    [SerializeField] private Material _towerGreenMat;
    [SerializeField] private Material _towerGreyMat;
    [SerializeField] private SkinnedMeshRenderer _bowSkinMeshRenderer;
    [SerializeField] private MeshRenderer _towerMeshRenderer;

    [SerializeField] private Transform tower;
    [SerializeField] private Transform fxPrefab;
    [SerializeField] private Transform tfmBow;
    [SerializeField] private Arrow arrowPrefab;
    [SerializeField] private Transform spawnPoint;
    //  private Coroutine co;
    [SerializeField] private List<Transform> enemyList = new List<Transform>();
    [SerializeField] private float delayFired = 1;
    private float delayFireTemp = 0;

    [SerializeField] private Animator anim;
    [SerializeField] private AnimationEvent eventAnim;
    [SerializeField] private float rotationSpeed = 100f;

    private Transform target;

    private Team owner;
    public int HP = 10;
    public Team Owner
    {
        get => owner;
        set
        {
            owner = value;
            if (owner == Team.None)
            {
                _towerMeshRenderer.material = _towerGreyMat;
            }
            else if (owner == Team.A)
            {
                _towerMeshRenderer.material = _towerGreenMat;
            }
            else if (owner == Team.B)
            {
                _towerMeshRenderer.material = _towerRedMat;
            }
        } 
    }

    private void Start()
    {
        Owner = Team.None;       
    }

    [ContextMenu("Appear")]
    public void Appear()
    {
        delayFireTemp = delayFired;
        eventAnim.onFired = SpawnArrow;
        //if (co != null)
        //{
        //    StopCoroutine(co);
        //}

        // co = StartCoroutine(PlayFx());

        tower.DOLocalMoveY(0, 3).OnComplete(() =>
        {
            //  StopCoroutine(co);
            StartCoroutine(FindToFire());
        });
    }

    IEnumerator PlayFx()
    {
        int count = Random.Range(5, 10);
        yield return null;

        for (int i = 0; i < count; i++)
        {
            var ob = Instantiate(fxPrefab, transform);
            float x = Random.Range(-1f, 1f);
            float z = Random.Range(-2f, 0f);

            ob.localPosition = new Vector3(x, 0, z);

            yield return new WaitForSeconds(Random.Range(0.1f, 0.3f));
        }

        //  co = StartCoroutine(PlayFx());
    }

    IEnumerator FindToFire()
    {
        Debug.Log("======== FindToFire");
        yield return new WaitForSeconds(0.1f);
        Transform _target = GetNearestEnemy();

        if (_target != null)
        {
            target = _target;
        }

        StartCoroutine(FindToFire());
    }


    void SpawnArrow()
    {
        var arr = Instantiate(arrowPrefab);
        arr.transform.position = spawnPoint.position;
        arr.transform.rotation = spawnPoint.rotation;

        arr.SetTarget(target);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            var wayr = other.GetComponent<WhyAreYouRunning>();
            if (wayr != null && wayr.Team != Owner)
            {
                enemyList.Add(other.transform);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            var wayr = other.GetComponent<WhyAreYouRunning>();
            if (wayr != null && wayr.Team != Owner)
            {
                enemyList.Remove(other.transform);
            }
        }
    }

    Transform GetNearestEnemy()
    {
        if (enemyList.Count <= 0)
        {
            return null;
        }

        Transform _target = null;
        float distance = float.MaxValue;

        for (int i = 0; i < enemyList.Count; i++)
        {
            if (enemyList[i] == null)
            {
                enemyList.RemoveAt(i);
                i--;
                continue;
            }

            var dis = Vector3.Distance(transform.position, enemyList[i].position);

            if (dis < distance)
            {
                distance = dis;
                _target = enemyList[i];
            }
        }

        return _target;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.V))
        {
            Appear();
        }

        if (target != null)
        {
            Vector3 pos1 = tfmBow.position;
            pos1.y = 0;

            Vector3 pos2 = target.position;
            pos2.y = 0;

            var a = pos2 - pos1;


            float angle = Vector3.SignedAngle(a, Vector3.forward, Vector3.up);

            Quaternion targetRotation = Quaternion.Euler(0, -angle, 0);

            tfmBow.rotation = Quaternion.RotateTowards(tfmBow.rotation, targetRotation, rotationSpeed * Time.deltaTime);

            delayFireTemp -= Time.deltaTime;

            if (delayFireTemp <= 0)
            {
                delayFireTemp = delayFired;
                anim.SetTrigger("Attack");
            }
        }
    }
}
