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
    private Transform _tfmTower;

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
        if (state == TroopState.MoveToCastle)
        {
            MoveToCastle();
        }
        else if (state == TroopState.Paratrooper)
        {
            Vector3 force = windForce * transform.forward;
            force.y = -fallForce;
            rigid.AddForce(force);
        }
        else if (state == TroopState.Attack)
        {
            AttackTower();
        }
        else if (state == TroopState.MoveToTower)
        {
            MoveToTower();
        }
        else if (state == TroopState.MoveToCanon)
        {
            MoveToCanon();
        }
    }

    private void MoveToTower()
    {
        if (_tfmTower == null) return;
        Vector3 newVelocity = rigid.velocity;
        Vector3 direction = runSpeed * (_tfmTower.position - transform.position).normalized;
        newVelocity.x = direction.x;
        newVelocity.z = direction.z;
        rigid.velocity = newVelocity;
    }

    private void MoveToCanon()
    {
        var _canonPos = PlayerController.Instance.transform.position;
        transform.LookAt(_canonPos);
        Vector3 newVelocity = rigid.velocity;
        Vector3 direction = runSpeed * (_canonPos - transform.position).normalized;
        newVelocity.x = direction.x;
        newVelocity.z = direction.z;
        rigid.velocity = newVelocity;
    }

    private void MoveToCastle()
    {
        if (state == TroopState.MoveToCastle)
        {
            Vector3 newVelocity = rigid.velocity;
            var _castlePos = LauDaiTinhAi.Instance.transform.position;
            Vector3 direction = runSpeed * (_castlePos - transform.position).normalized;
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
        state = TroopState.MoveToCastle;
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

        if (other.CompareTag("TowerTarget"))
        {
            //Debug.Log("======== Attact Tower");

            var _tower = other.GetComponentInParent<TowerController>();
            if (_tower.Owner != team)
            {
                _tfmTower = _tower.transform;
                state = TroopState.MoveToTower;
                transform.DOLookAt(_tfmTower.transform.position, 0.5f);
                MoveToTower();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("TowerTarget"))
        {
            if (team == Team.A)
            {
                transform.DOLookAt(LauDaiTinhAi.Instance.transform.position, 0.5f);
                state = TroopState.MoveToCastle;
            }
            else if (team == Team.B)
            {
                state = TroopState.MoveToCanon;
            }
            
        }
    }

    [ContextMenu("AttackTower")]
    public void AttackTower()
    {

    }

    void Dead()
    {
        Destroy(gameObject);
    }

    public IEnumerator FreezeYPosIEnumerator()
    {
        rigid.constraints = (RigidbodyConstraints)116;
        yield return null;
        yield return null;
        rigid.constraints = RigidbodyConstraints.FreezeRotation;
    }

    private void OnCollisionEnter(Collision other)
    {
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

        if (other.collider.CompareTag("Tower"))
        {
            var _tower = other.collider.GetComponentInParent<TowerController>();
            var _towerTeam = _tower.Owner;
            if (team != _towerTeam)
            {
                _tower.TakeDamage(1, team);
                Dead();
            }
        }

        if (other.collider.CompareTag("Castle"))
        {
            Dead();
        }
    }
}

public enum Team
{
    A,
    B,
    None
}
