using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;

public class WhyAreYouRunning : MonoBehaviour
{
    [SerializeField] private float runSpeed = 5;
    [SerializeField] private Animator anim;
    [SerializeField] private Rigidbody rigid;
    [SerializeField] private Team team;
    public Team Team => team;

    private bool _isDead;

    private void Awake()
    {
        if (team == Team.B)
        {
            var phaos = FindObjectsOfType<PhaoController>().ToList();
            foreach (var phao in phaos)
            {
                phao.Enemies.Add(transform);
            }
        }
    }

    void Update()
    {
        RunForward();
    }

    private void RunForward()
    {
        Vector3 newVelocity = rigid.velocity;
        Vector3 direction = runSpeed * transform.forward;
        newVelocity.x = direction.x;
        newVelocity.z = direction.z;
        rigid.velocity = newVelocity;
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Dead") && team == Team.A)
        {
            CastleController.Instance.Hit();
            Destroy(gameObject);
        }
        if (other.CompareTag("Bullet"))
        {
            if (team == Team.A)
            {
                return;
            }    
            Dead();
        }
    }



    void Dead()
    {
        Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.collider.CompareTag("Player"))
        {
            if (_isDead)
            {
                return;
            }
            if (other.gameObject.GetComponent<WhyAreYouRunning>().team != team)
            {
                _isDead = true;
                Invoke(nameof(Dead), 0.5f);
            }
            
        }
    }
}

public enum Team
{
    A,
    B
}
