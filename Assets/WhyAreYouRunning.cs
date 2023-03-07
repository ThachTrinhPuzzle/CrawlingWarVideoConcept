using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;

public class WhyAreYouRunning : MonoBehaviour
{
    public float runSpeed = 5;
    public Animator anim;
    public Rigidbody rigid;
    [SerializeField] private Team team;
    public GameObject parachute;
    public TroopState state;
    public Team Team => team;
    public float windForce;
    public float fallForce;
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
        if (state == TroopState.Run)
        {
            RunForward();
        }
        else if (state == TroopState.Paratrooper)
        {
            Vector3 force = windForce * transform.forward;
            force.y = -fallForce;
            rigid.AddForce(force);
        }
    }

    private void RunForward()
    {
        if (state == TroopState.Run)
        {
            Vector3 newVelocity = rigid.velocity;
            Vector3 direction = runSpeed * transform.forward;
            newVelocity.x = direction.x;
            newVelocity.z = direction.z;
            rigid.velocity = newVelocity;
        }
    }

    public void EnterParatrooperState()
    {
        parachute.SetActive(true);
        state = TroopState.Paratrooper;
        rigid.useGravity = false;
    }



    private void ExitParatrooperState()
    {
        parachute.SetActive(false);
        state = TroopState.Run;
        rigid.useGravity = true;
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
        Debug.Log(state);
        Debug.Log(other.gameObject.tag);
        if (state == TroopState.Paratrooper && other.gameObject.CompareTag("Ground"))
        {
            ExitParatrooperState();
            return;
        }
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
