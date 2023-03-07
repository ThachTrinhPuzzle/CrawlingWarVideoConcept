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
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            Hit();
        }
    }


    void Spawn()
    {
        Instantiate(normalEnemyPrefab, door.position, door.rotation);
    }
    public int strengthRotate = 20;
    public int strengthScale = 10;
    public void Hit()
    {
        transform.DOKill();
        transform.DOShakeRotation(1, 3, strengthRotate);
        transform.DOShakeScale(1, 0.5f, strengthScale);
    }

}
