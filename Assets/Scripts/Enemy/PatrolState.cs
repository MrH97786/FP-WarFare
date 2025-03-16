using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolState : EnemyBaseState
{
    public int waypointIndex;
    public float waitTimer;
    private Animator animator;

    // Add a new flag to determine the patrol direction
    private bool isPatrollingForward = true; // By default, the enemy will patrol forward

    public override void Enter()
    {
        animator = enemy.GetComponent<Animator>();
        animator.SetBool("isWalking", true);

        // Randomly choose the direction for each enemy when they start patrolling
        isPatrollingForward = Random.value > 0.5f; // 50% chance to patrol forward or backward
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
        // Check if the enemy has reached the current waypoint
        if (enemy.Agent.remainingDistance < 0.2f && !enemy.Agent.pathPending)
        {
            animator.SetBool("isWalking", false);
            SoundManager.Instance.enemyLoopChannel.loop = false;
            SoundManager.Instance.enemyLoopChannel.Stop();
            waitTimer += Time.deltaTime;

            if (waitTimer > 3) // Wait for 3 seconds at each waypoint
            {
                // Update waypointIndex based on the patrol direction
                if (isPatrollingForward)
                {
                    waypointIndex = (waypointIndex + 1) % enemy.enemyPath.waypoints.Count; // Forward direction
                }
                else
                {
                    waypointIndex = (waypointIndex - 1 + enemy.enemyPath.waypoints.Count) % enemy.enemyPath.waypoints.Count; // Backward direction
                }

                enemy.Agent.SetDestination(enemy.enemyPath.waypoints[waypointIndex].position);

                animator.SetBool("isWalking", true);
                waitTimer = 0;

                if (!SoundManager.Instance.enemyLoopChannel.isPlaying)
                {
                    SoundManager.Instance.enemyLoopChannel.clip = SoundManager.Instance.enemyWalking;
                    SoundManager.Instance.enemyLoopChannel.loop = true;
                    SoundManager.Instance.enemyLoopChannel.Play();
                }
            }
        }
    }
}
