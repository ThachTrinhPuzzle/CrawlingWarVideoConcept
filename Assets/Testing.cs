using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Testing : MonoBehaviour
{
    [ContextMenu("test")]
    void Test()
    {
        float angle = 20;
        Vector3 euler = transform.eulerAngles;
        
        print(euler.y);

        angle = euler.y - angle;
        
        print($"{Mathf.Cos(410 * Mathf.Deg2Rad)} : {Mathf.Sin(340 * Mathf.Deg2Rad)}");
        
        transform.rotation = Quaternion.Euler(0, 410, 0);
        
        print($"=====> {transform.forward - transform.position} : {transform.forward}");
        
    }
}
