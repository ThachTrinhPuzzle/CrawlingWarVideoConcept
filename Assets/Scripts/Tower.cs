using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    [SerializeField] private Material _bowMat;
    [SerializeField] private Material _towerRedMat;
    [SerializeField] private Material _towerGreenMat;
    [SerializeField] private Material _towerGreyMat;
    [SerializeField] private SkinnedMeshRenderer _bowSkinMeshRenderer;
    [SerializeField] private MeshRenderer _towerMeshRenderer;

    [SerializeField] private GameObject _arrowPrefab;
    [SerializeField] private Transform _shootPos;
    [SerializeField] private Transform _tfmBow;
    
    private List<WhyAreYouRunning> listTroopTarget;

    private void Start()
    {
     
    }

    public void Shoot()
    { 
        
    }
}
