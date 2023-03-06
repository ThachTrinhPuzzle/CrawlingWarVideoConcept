using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ThachTest : MonoBehaviour
{
    public Transform castle;
    public float duration;
    public float strength;
    public int vibrato;
    public int id;

    private void Update()
    {
        if (Input.GetKeyDown((KeyCode.Alpha1 + id)))
        {
            if (id == 0) castle.DOShakePosition(duration, strength, vibrato);
            else if (id == 1) castle.DOShakeRotation(duration, strength, vibrato);
            else castle.DOShakeScale(duration, strength, vibrato);
        }
    }
}
