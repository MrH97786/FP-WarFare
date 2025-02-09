using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    private StateController stateController;
    private NavMeshAgent agent;
    public NavMeshAgent Agent {get => agent;}
    [SerializeField]
    private string currentState;
    public EnemyPath enemyPath;

    // Start is called before the first frame update
    void Start()
    {
        stateController = GetComponent<StateController>();
        agent = GetComponent<NavMeshAgent>();
        stateController.Initialise();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
