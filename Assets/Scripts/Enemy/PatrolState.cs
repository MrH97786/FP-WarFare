using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolState : EnemyBaseState
{
    public int waypointIndex;
    public float waitTimer;

    public override void Enter()
    {

    }

    public override void Perform()
    {
        Patrol();
    }

    public override void Exit()
    {

    }

    public void Patrol()
    {
        if (enemy.Agent.remainingDistance < 0.2f)
        {
            waitTimer += Time.deltaTime; 
            if (waitTimer > 3)
            {
                if (waypointIndex < enemy.enemyPath.waypoints.Count - 1)
                    waypointIndex++;
                else
                    waypointIndex = 0;
                enemy.Agent.SetDestination(enemy.enemyPath.waypoints[waypointIndex].position);
                waitTimer = 0;
            }    
        }
    }
}
