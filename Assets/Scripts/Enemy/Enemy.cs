using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    private StateController stateController;
    private NavMeshAgent agent;
    private GameObject player;
    private Vector3 lastKnownPosition;

    public NavMeshAgent Agent { get => agent; }
    public GameObject Player { get => player; }
    public Vector3 LastKnownPosition { get => lastKnownPosition; set => lastKnownPosition = value; }

    public EnemyPath enemyPath;
    [SerializeField] private string currentState;

    [SerializeField] private int enemyHealth = 100;
    private Animator animator;
    public bool isDead = false;

    [Header("Sight Values")]
    public float detectionRange = 20f;
    public float fieldOfView = 85f;
    public float eyeLevel;

    [Header("Sight Values")]
    public Transform gunBarrel;
    [Range(0.1f, 10f)]
    public float fireRate;

    void Start()
    {
        animator = GetComponent<Animator>();
        stateController = GetComponent<StateController>();
        agent = GetComponent<NavMeshAgent>();
        stateController.Initialise();
        player = GameObject.FindGameObjectWithTag("Player");
        agent.avoidancePriority = Random.Range(30, 70);
    }

    void Update()
    {
        DetectPlayer();
        currentState = stateController.activeState.ToString();
    }

    public bool DetectPlayer()
    {
        if (player != null)
        {
            if (Vector3.Distance(transform.position, player.transform.position) < detectionRange)
            {
                Ray ray = new Ray(transform.position + (Vector3.up * eyeLevel), player.transform.position - transform.position);
                RaycastHit hitInfo;
                if (Physics.Raycast(ray, out hitInfo, detectionRange))
                {
                    if (hitInfo.transform.gameObject == player)
                    {
                        Debug.DrawRay(ray.origin, ray.direction * detectionRange, Color.red);
                        return true;
                    }
                }
            }
        }
        return false;
    }

    public void TakeDamage(int damageAmount)
    {
        Debug.Log("Enemy took damage: " + damageAmount);
        enemyHealth -= damageAmount;

        if (enemyHealth <= 0)
        {
            Die();
        }
        else
        {
            animator.SetTrigger("DAMAGE");
            SoundManager.Instance.enemyChannel.PlayOneShot(SoundManager.Instance.enemyHurt);
        }
    }

    private void Die()
    {
        ScoreManager.instance.AddPoints();
        int randomValue = Random.Range(0, 2);
        animator.SetTrigger(randomValue == 0 ? "DIE1" : "DIE2");

        SoundManager.Instance.enemyChannel.PlayOneShot(SoundManager.Instance.enemyDeath);
        SoundManager.Instance.enemyLoopChannel.loop = false;
        SoundManager.Instance.enemyLoopChannel.Stop();
        isDead = true;
        agent.isStopped = true; // Stop movement
        agent.enabled = false;  // Disable NavMeshAgent 
        stateController.enabled = false; // Stop AI state controller

        this.GetComponent<Collider>().enabled = false; // Disable collider 

        Destroy(gameObject, 10f); // Destroy enemy after 10 seconds
    }

}