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
    private bool isShaking;
    private void Awake() => Instance = this;
    private void Start() => isShaking = false;

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
    public void Hit()
    {
        if (isShaking) return;
        isShaking = true;
        float duration = 1f;
        transform.DOShakeRotation(duration, 3, 20);
        transform.DOShakeScale(duration, 0.5f, 10).OnComplete(() => isShaking = false);
    }
}
