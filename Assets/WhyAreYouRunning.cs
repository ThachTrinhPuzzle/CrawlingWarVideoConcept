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
    
    private bool _isRunning;
    [SerializeField] private Team team;
    public Team Team => team;

    private bool _isDead;

    private void Awake()
    {
        if (team == Team.A)
        {
            rigid.isKinematic = true;
        }
        else
        {
            var phaos = GameObject.FindObjectsOfType<PhaoController>().ToList();
            foreach (var phao in phaos)
            {
                phao.Enemies.Add(transform);
            }
        }
    }

    void Update()
    {
        if (_isRunning)
        {
            Vector3 dir = team == Team.A ? Vector3.forward : -Vector3.forward;
            rigid.velocity = runSpeed * Time.deltaTime * dir;
            anim.SetBool("running", true);
        }
    }

    public void Running()
    {
        if (team == Team.B)
        {
            runSpeed *= -1;
        }
        else
        {
            rigid.isKinematic = false;
        }
        _isRunning = true;
    }

    public void Force(Vector3 dir)
    {
        transform.DOMove(dir, 0.3f).OnComplete(() => { Running(); });
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
