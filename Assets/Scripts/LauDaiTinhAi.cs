using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


public class LauDaiTinhAi : MonoBehaviour
{
    public static LauDaiTinhAi Instance;
    public WhyAreYouRunning normalEnemyPrefab;
    public WhyAreYouRunning giantEnemyPrefab;
    public Transform door;
    private void Awake() => Instance = this;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            Spawn();
        }
    }



    void Spawn()
    {
        Instantiate(normalEnemyPrefab, door.position, door.rotation);
    }

    public void Hit()
    {
        transform.DOShakeRotation(1, 3, 20);
        transform.DOShakeScale(1, 0.5f, 10);
    }
}
