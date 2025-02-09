using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateController : MonoBehaviour
{
    public EnemyBaseState activeState;
    public PatrolState patrolState;

    public void Initialise()
    {
        patrolState = new PatrolState();
        ChangeState(patrolState);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (activeState != null)
        {
            activeState.Perform();
        }
    }

    public void ChangeState (EnemyBaseState newState)
    {
        if (activeState != null)
        {
            activeState.Exit();
        }
        activeState = newState;

        //Making sure new state is not null
        if (activeState != null)
        {
            activeState.stateController = this;
            activeState.enemy = GetComponent<Enemy>();
            activeState.Enter();
        }
    }
}
