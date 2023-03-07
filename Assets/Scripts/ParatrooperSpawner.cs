using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParatrooperSpawner : MonoBehaviour
{
    public WhyAreYouRunning prefab;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            Spawn();
        }
    }

    public void Spawn()
    {
        WhyAreYouRunning troop = Instantiate(prefab, transform.position, transform.rotation);
        troop.EnterParatrooperState();
    }
}
