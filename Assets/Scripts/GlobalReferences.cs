using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalReferences : MonoBehaviour
{
    public static GlobalReferences Instance { get; set;}

    public GameObject bulletImpactEffect;
    public GameObject bloodSprayEffect;

    public int waveNumber;
    public int scoreNumber;

    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else 
        {
            Instance = this;
        }
    }
}
