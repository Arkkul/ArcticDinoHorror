using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(NavMeshAgent))]
public class Enemy : MonoBehaviour,IRayCastHit
{
    [SerializeField] private NavMeshAgent _navAgent;
    [SerializeField] private Animator aiAnim;

    [SerializeField] private List<Transform> destinations;

    [Header("-----Properties-----")]
    [SerializeField] private float walkSpeed;
    [SerializeField] private float chaseSpeed;

    [SerializeField] private float minIdleTime;
    [SerializeField] private float maxIdleTime;
    [SerializeField] private float idleTime;

    [SerializeField] private float catchDistance;

    [SerializeField] private float chaseTime;
    [SerializeField] private float minChaseTime;
    [SerializeField] private float maxChaseTime;

    [SerializeField] private float jumpscareTime;

    [SerializeField] private Transform _player;
    [SerializeField] private List<Transform> foodTargets = new List<Transform>();
    [SerializeField] private Transform safePlace;

    private bool walking;
    private bool chasing;

    private int randNum;

    private Transform currentDest;
    private Vector3 dest;
    // public string deathScene;

    [Header("-----View-----")]
    [SerializeField] private float viewRadius;
    [Range(0, 360)]
    [SerializeField] private float viewAngle;
    [SerializeField] private LayerMask targetMask;
    [SerializeField] private LayerMask obstacleMask;
    [SerializeField] private List<Transform> visibleTargets = new List<Transform>();

    void Start()
    {
        walking = true;
        StartCoroutine("FindTargetsWithDelay", 0.2f);
        randNum = Random.Range(0, destinations.Count);
        currentDest = destinations[randNum];
    }
    void Update()
    {
        if (chasing == true)
        {
            Chase();
        }
        if (walking == true)
        {
            Walk();
        }
    }
    IEnumerator stayIdle()
    {
        idleTime = Random.Range(minIdleTime, maxIdleTime);
        yield return new WaitForSeconds(idleTime);
        walking = true;
        randNum = Random.Range(0, destinations.Count);
        currentDest = destinations[randNum];
    }
    IEnumerator chaseRoutine()
    {
        chaseTime = Random.Range(minChaseTime, maxChaseTime);
        yield return new WaitForSeconds(chaseTime);
        walking = true;
        chasing = false;
        randNum = Random.Range(0, destinations.Count);
        currentDest = destinations[randNum];
    }
    IEnumerator deathRoutine()
    {
        yield return new WaitForSeconds(jumpscareTime);
        //SceneManager.LoadScene(deathScene);
    }

    IEnumerator FindTargetsWithDelay(float delay)
    {
        while (true)
        {
            yield return new WaitForSeconds(delay);
            FindVisibleTargets();
        }
    }

    private void Walk()
    {
        dest = currentDest.position;
        _navAgent.destination = dest;
        _navAgent.speed = walkSpeed;
        aiAnim.ResetTrigger("sprint");
        aiAnim.ResetTrigger("idle");
        aiAnim.SetTrigger("walk");
        if (_navAgent.remainingDistance <= _navAgent.stoppingDistance)
        {
            aiAnim.ResetTrigger("sprint");
            aiAnim.ResetTrigger("walk");
            aiAnim.SetTrigger("idle");
            _navAgent.speed = 0;
            StopCoroutine("stayIdle");
            StartCoroutine("stayIdle");
            walking = false;
        }
    }

    public void Chase()
    {
        if (!chasing) return;

        Transform target = null;
        float closestDistance = float.MaxValue;

        // Check if player is in view
        if (Vector3.Distance(_player.position, _navAgent.transform.position) <= viewRadius)
        {
            target = _player;
        }
        else
        {
            // Find the closest food target
            foreach (Transform t in foodTargets)
            {
                if (t != null)
                {
                    float distance = Vector3.Distance(t.position, _navAgent.transform.position);
                    if (distance < closestDistance)
                    {
                        closestDistance = distance;
                        target = t;
                    }
                }
            }
        }

        if (target != null)
        {
            _navAgent.destination = target.position;
            _navAgent.speed = chaseSpeed;
            aiAnim.ResetTrigger("walk");
            aiAnim.ResetTrigger("idle");
            aiAnim.SetTrigger("sprint");

            if (Vector3.Distance(_navAgent.transform.position, target.position) <= catchDistance)
            {
                // Add logic for catching the player or food
            }
        }
    }


    public void MoveToSafePlace()
    {
        if (safePlace != null)
        {
            _navAgent.SetDestination(safePlace.position);
            _navAgent.speed = chaseSpeed;
            aiAnim.SetTrigger("run");
        }
    }

    void FindVisibleTargets()
    {
        visibleTargets.Clear();
        Collider[] targetsInViewRadius = Physics.OverlapSphere(transform.position, viewRadius, targetMask);

        for (int i = 0; i < targetsInViewRadius.Length; i++)
        {
            Transform target = targetsInViewRadius[i].transform;
            Vector3 dirToTarget = (target.position - transform.position).normalized;
            if (Vector3.Angle(transform.forward, dirToTarget) < viewAngle / 2)
            {
                float dstToTarget = Vector3.Distance(transform.position, target.position);

                if (!Physics.Raycast(transform.position, dirToTarget, dstToTarget, obstacleMask))
                {
                    visibleTargets.Add(target);

                    if (target.CompareTag("Player"))
                    {
                        walking = false;
                        StopCoroutine("stayIdle");
                        StopCoroutine("chaseRoutine");
                        StartCoroutine("chaseRoutine");
                        chasing = true;
                    }
                    else if (target.CompareTag("Eat"))
                    {
                        foodTargets.Add(target); // Добавляем объект в список foodTargets
                        walking = false;
                        StopCoroutine("stayIdle");
                        StopCoroutine("chaseRoutine");
                        StartCoroutine("chaseRoutine");
                        chasing = true;
                    }
                }
            }
        }
    }

    public Vector3 DirFromAngle(float angleInDegrees, bool angleIsGlobal)
    {
        if (!angleIsGlobal)
        {
            angleInDegrees += transform.eulerAngles.y;
        }
        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, viewRadius);

        Vector3 viewAngleA = DirFromAngle(-viewAngle / 2, false);
        Vector3 viewAngleB = DirFromAngle(viewAngle / 2, false);

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, transform.position + viewAngleA * viewRadius);
        Gizmos.DrawLine(transform.position, transform.position + viewAngleB * viewRadius);
    }
    void IRayCastHit.OnRayHit()
    {
        Debug.Log("Враг получил попадание!");
        StopChase();
        MoveToSafePlace();
    }
    public void StopChase()
    {
        chasing = false;
        _navAgent.ResetPath();
        aiAnim.SetTrigger("idle");
    }
}
