using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : EnemyBaseState
{
    private float moveTimer;
    private float playerLostTimer;
    private float shootCooldown;

    public override void Enter()  
    {
        moveTimer = 0;
        playerLostTimer = 0;
    }

    public override void Perform()  
    {
        if (enemy.DetectPlayer())
        {
            playerLostTimer = 0;
            moveTimer += Time.deltaTime; 
            shootCooldown += Time.deltaTime; 
            enemy.transform.LookAt(enemy.Player.transform);
            // cooldown betwen shots fired
            if (shootCooldown > enemy.fireRate)
            {
                Shoot();
            }
            // Enemy moves at random time
            if (moveTimer > Random.Range(3, 7))
            {
                enemy.Agent.SetDestination(enemy.transform.position + (Random.insideUnitSphere * 5));
                moveTimer = 0;
            }
            enemy.LastKnownPosition = enemy.Player.transform.position;
        }
        else
        {
            playerLostTimer += Time.deltaTime; 
            if (playerLostTimer > 8)
            {
                stateController.ChangeState(new SearchState()); // change to patrol state
            }
        }
    }

    public void Shoot()
    {
        // Reference to gun barrel 
        Transform gunbarrel = enemy.gunBarrel;

        // Create new bullet
        GameObject bullet = GameObject.Instantiate(Resources.Load("Prefabs/Bullet") as GameObject, gunbarrel.position, enemy.transform.rotation);
        
        // Calculate direction to player and adding force to the bullet
        Vector3 directionOfFire = (enemy.Player.transform.position - gunbarrel.transform.position).normalized;
        bullet.GetComponent<Rigidbody>().velocity = directionOfFire * 40;

        Debug.Log("Shoot");
        shootCooldown = 0;
    }

    public override void Exit()
    {
       
    }
}
