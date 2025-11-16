using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Kromeshnik : MonoBehaviour, IRayCastHit
{
    [SerializeField] private Inventory playerInventory;

    [SerializeField] private NavMeshAgent agent;
    private Transform player;
    private List<Transform> darkAreas = new List<Transform>();

    private int currentDarkAreaIndex = 0;
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float attackRange = 2f;
    [SerializeField] private float lightDestroyRange = 3f;
    [SerializeField] private float searchPlayerInterval = 5f;
    [SerializeField] private float attackInterval = 10f;

    [SerializeField] private GameObject poisonCloudPrefab;

    private bool isInDark = true;
    private bool isAttacking = false;
    private bool isSearchingPlayer = false;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.speed = moveSpeed;

        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        if (player == null)
        {
            Debug.LogError("Player not found!");
            return;
        }

        foreach (var darkArea in GameObject.FindGameObjectsWithTag("DarkArea"))
        {
            darkAreas.Add(darkArea.transform);
        }

        SetNextDarkAreaAsDestination();
        StartCoroutine(SearchPlayerRoutine());
    }

    private void Update()
    {
        if (isInDark && !isSearchingPlayer)
        {
            MoveInDark();
        }

        if (player != null && Vector3.Distance(transform.position, player.position) <= attackRange)
        {
            if (!isAttacking)
            {
                StartCoroutine(AttackPlayer());
            }
        }
    }

    private void MoveInDark()
    {
        if (darkAreas.Count == 0) return;

        if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            currentDarkAreaIndex = (currentDarkAreaIndex + 1) % darkAreas.Count;
            SetNextDarkAreaAsDestination();
        }
    }

    private void SetNextDarkAreaAsDestination()
    {
        if (darkAreas.Count > 0)
        {
            agent.SetDestination(darkAreas[currentDarkAreaIndex].position);
        }
    }

    private void ReleasePoison()
    {
        if (poisonCloudPrefab != null)
        {
            Instantiate(poisonCloudPrefab, transform.position, Quaternion.identity);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (!isAttacking)
            {
                StartCoroutine(AttackPlayer());
            }
            if (playerInventory != null && playerInventory.HasCurrentItem("FlashLight"))
            {
                playerInventory.DropCurrentItem();
                Debug.Log("Фонарик выбит из рук!");
            }
        }
    }

    private IEnumerator AttackPlayer()
    {
        if (!isAttacking)
        {
            isAttacking = true;
            yield return new WaitForSeconds(1f);
            isAttacking = false;
        }
    }

    private IEnumerator SearchPlayerRoutine()
    {
        var wait = new WaitForSeconds(searchPlayerInterval);

        while (true)
        {
            yield return wait;

            if (!isAttacking && player != null)
            {
                isSearchingPlayer = true;
                agent.SetDestination(player.position);

                // Подождать немного, чтобы имитировать поисковое поведение
                yield return new WaitForSeconds(1f);
                isSearchingPlayer = false;
                SetNextDarkAreaAsDestination(); // Возвращаемся в темные зоны
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("DarkArea"))
        {
            isInDark = true;
        }
    }

    public void OnRayHit()
    {
        Debug.Log("Enemy hit!");
        ReleasePoison();
    }
}