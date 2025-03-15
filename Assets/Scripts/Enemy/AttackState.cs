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
        enemy.GetComponent<Animator>().SetBool("isAttacking", true);
    }

    public override void Perform()
    {
        if (enemy.DetectPlayer())
        {
            playerLostTimer = 0;
            moveTimer += Time.deltaTime;
            shootCooldown += Time.deltaTime;
            enemy.transform.LookAt(enemy.Player.transform);

            if (shootCooldown > enemy.fireRate)
            {
                Shoot();
            }

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
            if (playerLostTimer > 1)
            {
                stateController.ChangeState(new SearchState());
            }
        }
    }

    public void Shoot()
    {
        Transform gunbarrel = enemy.gunBarrel;
        GameObject bullet = GameObject.Instantiate(Resources.Load("Prefabs/Bullet") as GameObject, gunbarrel.position, enemy.transform.rotation);
        Vector3 directionOfFire = (enemy.Player.transform.position - gunbarrel.transform.position).normalized;
        bullet.GetComponent<Rigidbody>().velocity = directionOfFire * 40;
        Debug.Log("Shoot");
        shootCooldown = 0;

        SoundManager.Instance.enemyChannel.PlayOneShot(SoundManager.Instance.enemyAttacking, 0.1f);

    }

    public override void Exit()
    {
        enemy.GetComponent<Animator>().SetBool("isAttacking", false);
    }
}

