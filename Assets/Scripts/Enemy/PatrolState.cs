using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolState : EnemyBaseState
{
    public int waypointIndex;
    public float waitTimer;
    private Animator animator;

    public override void Enter()
    {
        animator = enemy.GetComponent<Animator>();
        animator.SetBool("isWalking", true);
    }

    public override void Perform()
    {
        Patrol();
        if (enemy.DetectPlayer())
        {
            stateController.ChangeState(new AttackState());
        }
    }

    public override void Exit()
    {
        animator.SetBool("isWalking", false);
    }

    public void Patrol()
    {
        if (enemy.Agent.remainingDistance < 0.2f && !enemy.Agent.pathPending)
        {
            animator.SetBool("isWalking", false);
            waitTimer += Time.deltaTime;

            if (waitTimer > 3)
            {
                waypointIndex = (waypointIndex + 1) % enemy.enemyPath.waypoints.Count;
                enemy.Agent.SetDestination(enemy.enemyPath.waypoints[waypointIndex].position);

                animator.SetBool("isWalking", true);
                waitTimer = 0;
            }
        }
    }
}
