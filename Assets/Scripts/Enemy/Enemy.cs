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

    [Header("Sight Values")]
    public float detectionRange = 20f;
    public float fieldOfView = 85f;
    public float eyeLevel;

    [Header("Sight Values")]
    public Transform gunBarrel;
    [Range(0.1f, 10f)]
    public float fireRate;

    [SerializeField]
    private string currentState;

    public bool isDead = false;

    // Start is called before the first frame update
    void Start()
    {
        stateController = GetComponent<StateController>();
        agent = GetComponent<NavMeshAgent>();
        stateController.Initialise();
        player = GameObject.FindGameObjectWithTag("Player");

        // Set a random avoidance priority (lower = higher priority)
        agent.avoidancePriority = Random.Range(30, 70);
    }


    // Update is called once per frame
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

}
