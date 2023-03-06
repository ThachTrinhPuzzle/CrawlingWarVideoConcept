using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class PhaoBullet : MonoBehaviour
{
    [SerializeField] private GameObject fx;
    [SerializeField] private Collider col;
    public void Fire(Vector3 target)
    {
        transform.DOJump(target, 10, 1, 1).SetSpeedBased(true).SetEase(Ease.Linear).OnComplete(() =>
        {
            col.enabled = true;
            var ob = Instantiate(fx);
            ob.transform.position = new Vector3(transform.position.x, 0.31f, transform.position.z);
            Invoke(nameof(Des), 0.1f);
        });
    }

    void Des()
    {
        Destroy(gameObject);
    }
}
