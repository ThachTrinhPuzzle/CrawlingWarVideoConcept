using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class CastleController : MonoBehaviour
{
    private static CastleController instance;
    public static CastleController Instance => instance;
    
    [SerializeField] private WhyAreYouRunning prefab;
    [SerializeField] private float minX;
    [SerializeField] private float maxX;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private int minE, maxE;
    [SerializeField] private Animator anim;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            StartCoroutine(SpawnEnemy());
        }
    }

    IEnumerator SpawnEnemy()
    {
        int num = Random.Range(minE, maxE);

        for (int i = 0; i < num; i++)
        {
            Spawn();
            yield return new WaitForSeconds(Random.Range(0.1f, 1.2f));
        }
    }

    void Spawn()
    {
        var enemy = Instantiate(prefab);
        float x = Random.Range(minX, maxX);
        float z = spawnPoint.position.z;
        enemy.GetComponent<Transform>().position = new Vector3(x, 0, z);
        enemy.GetComponent<Transform>().rotation = Quaternion.Euler(0, -180, 0);
        enemy.Running();
    }

    public void Hit()
    {
        anim.SetTrigger("hited");
    }
}
