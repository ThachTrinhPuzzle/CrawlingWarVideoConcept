using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParatrooperSpawner : MonoBehaviour
{
    public WhyAreYouRunning prefab;
    public Transform[] points;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            Spawn();
        }
    }

    public void Spawn()
    {
        Transform point = points.ChonDai();
        Vector3 direction = LauDaiTinhAi.Instance.transform.position - point.position;
        direction.y = 0;
        WhyAreYouRunning troop = Instantiate(prefab, point.position, point.rotation);
        troop.transform.forward = direction;
        troop.EnterParatrooperState();
    }
}
