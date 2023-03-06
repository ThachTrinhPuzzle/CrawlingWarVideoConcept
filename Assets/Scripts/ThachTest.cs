using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThachTest : MonoBehaviour
{
    public Rigidbody rigid;
    public float speed;
    public float force;
    private void Start()
    {
        
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            rigid.AddForce(force * Vector3.forward);
        }
        //rigid.velocity = Vector3.forward * speed;
    }

}
