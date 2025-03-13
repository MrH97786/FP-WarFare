using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SearchState : EnemyBaseState
{
    private float searchPlayerTimer;
    private float searchAroundTimer;

    public override void Enter()
    {
        enemy.Agent.SetDestination(enemy.LastKnownPosition);
        enemy.GetComponent<Animator>().SetBool("IsSearching", true);
    }

    public override void Perform()
    {
        if (enemy.DetectPlayer())
        {
            stateController.ChangeState(new AttackState());
        }

        if (enemy.Agent.remainingDistance < enemy.Agent.stoppingDistance)
        {
            searchPlayerTimer += Time.deltaTime;
            searchAroundTimer += Time.deltaTime;

            if (searchAroundTimer > Random.Range(3, 5))
            {
                enemy.Agent.SetDestination(enemy.transform.position + (Random.insideUnitSphere * 10));
                searchAroundTimer = 0;
            }
            if (searchPlayerTimer > 10)
            {
                stateController.ChangeState(new PatrolState());
            }
        }
    }
    
    public override void Exit()
    {
        enemy.GetComponent<Animator>().SetBool("IsSearching", false);
    }
}
