
using UnityEngine;
using UnityEngine.AI;

public class Baby : MonoBehaviour
{
    public NavMeshAgent navAgent;
    public Transform player;
    public LayerMask groundLayer, playerLayer;
    public float walkPointRange;
    public float sightRange;
    public float chaseDuration = 5f; // Duration to chase the player

    private Vector3 walkPoint;
    private bool walkPointSet;
    private bool isChasing;
    private float chaseTimer;

    private void Awake()
    {
        player = GameObject.Find("Player").transform;
        navAgent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        bool playerInSightRange = Physics.CheckSphere(transform.position, sightRange, playerLayer);

        if (playerInSightRange)
        {
            ChasePlayer();
        }
        else if (isChasing)
        {
            chaseTimer -= Time.deltaTime;
            if (chaseTimer <= 0)
            {
                isChasing = false;
                navAgent.isStopped = false;
            }
            else
            {
                navAgent.SetDestination(player.position);
            }
        }
        else
        {
            Patroling();
        }
    }

    private void Patroling()
    {
        if (!walkPointSet)
        {
            SearchWalkPoint();
        }

        if (walkPointSet)
        {
            navAgent.SetDestination(walkPoint);
        }

        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        if (distanceToWalkPoint.magnitude < 1f)
        {
            walkPointSet = false;
        }
    }

    private void SearchWalkPoint()
    {
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);
        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if (Physics.Raycast(walkPoint, -transform.up, 2f, groundLayer))
        {
            walkPointSet = true;
        }
    }

    private void ChasePlayer()
    {
        isChasing = true;
        chaseTimer = chaseDuration;
        navAgent.SetDestination(player.position);
        navAgent.isStopped = false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }
}
