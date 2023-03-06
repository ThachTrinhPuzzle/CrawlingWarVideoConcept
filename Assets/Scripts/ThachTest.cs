using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThachTest : MonoBehaviour
{
    public Rigidbody rigid;
    public float speed;
    public float force;
    public float angle;
    public bool useVelocity;

    private void Update()
    {
        Vector3 newVelocity = rigid.velocity;
        newVelocity.z = speed;
        rigid.velocity = newVelocity;
        if (Input.GetKeyDown(KeyCode.Space))
        {
            TestPhysic();
        }
    }

    private void Rotate()
    {
        Debug.Log(transform.rotation);
        transform.rotation = transform.rotation * Quaternion.Euler(angle, 0, 0);
    }

    private void TestPhysic()
    {
        if (useVelocity) rigid.velocity = Vector3.forward * speed;
        else rigid.AddForce(force * Vector3.forward);
    }


}
