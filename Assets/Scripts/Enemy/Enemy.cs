using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    private StateController stateController;
    private NavMeshAgent agent;
    public NavMeshAgent Agent {get => agent;}

    [SerializeField]
    private string currentState;
    public EnemyPath enemyPath;

    private GameObject player;
    public float detectionRange = 20f;
    public float fieldOfView = 85f;
    public float eyeLevel;

    // Start is called before the first frame update
    void Start()
    {
        stateController = GetComponent<StateController>();
        agent = GetComponent<NavMeshAgent>();
        stateController.Initialise();
        player = GameObject.FindGameObjectWithTag("Player");
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
                Vector3 targetDirection = player.transform.position - transform.position - (Vector3.up * eyeLevel);
                float angleToPlayer = Vector3.Angle(targetDirection, transform.forward);
                if (angleToPlayer >= -fieldOfView && angleToPlayer <= fieldOfView)
                {
                    Ray ray = new Ray(transform.position + (Vector3.up * eyeLevel), targetDirection);
                    RaycastHit hitInfo = new RaycastHit();
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
        }
        return false;
    }
}
