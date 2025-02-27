using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : EnemyBaseState
{
    private float moveTimer;
    private float attackStateTimer;

    public override void Enter()  
    {
        moveTimer = 0;
        attackStateTimer = 0;
    }

    public override void Perform()  
    {
        if (enemy.DetectPlayer())
        {
            attackStateTimer = 0;
            moveTimer += Time.deltaTime; 
            if (moveTimer > Random.Range(3, 7))
            {
                enemy.Agent.SetDestination(enemy.transform.position + (Random.insideUnitSphere * 5));
                moveTimer = 0;
            }
        }
        else
        {
            attackStateTimer += Time.deltaTime; 
            if (attackStateTimer > 8)
            {
                stateController.ChangeState(new PatrolState()); // change to patrol state
            }
        }
    }

    public override void Exit()
    {
       
    }
}
