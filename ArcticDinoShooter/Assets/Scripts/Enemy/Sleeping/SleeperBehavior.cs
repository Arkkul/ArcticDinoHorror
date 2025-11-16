using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SleeperBehavior : MonoBehaviour
{
    public float lightParalyzeRadius = 5f;
    public float distractionRadius = 10f;
    public float distractionDuration = 5f;
    public LayerMask sleeperLayer;
    public Light flashlight;

    private bool isDistracted = false;
    private bool isAlerted = false;
    private List<GameObject> sleepers = new List<GameObject>();

    void Start()
    {
        // Найти всех спящих в сцене
        sleepers.AddRange(GameObject.FindGameObjectsWithTag("Sleeper"));
    }

    void Update()
    {
        HandleFlashlight();
        HandleDistraction();
    }

    void HandleFlashlight()
    {
        if (flashlight.enabled)
        {
            foreach (GameObject sleeper in sleepers)
            {
                float distance = Vector3.Distance(flashlight.transform.position, sleeper.transform.position);
                if (distance <= lightParalyzeRadius)
                {
                    // Парализовать спящего
                    sleeper.GetComponent<Sleeper>().Paralyze();
                }
                else
                {
                    // Разбудить спящего
                    sleeper.GetComponent<Sleeper>().WakeUp();
                }
            }
        }
    }

    void HandleDistraction()
    {
        if (isDistracted)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.G)) // Клавиша для создания шума
        {
            StartCoroutine(DistractionCoroutine());
        }
    }

    IEnumerator DistractionCoroutine()
    {
        isDistracted = true;
        Collider[] distractedSleepers = Physics.OverlapSphere(transform.position, distractionRadius, sleeperLayer);

        foreach (Collider sleeper in distractedSleepers)
        {
            sleeper.GetComponent<Sleeper>().Distract();
        }

        yield return new WaitForSeconds(distractionDuration);

        foreach (Collider sleeper in distractedSleepers)
        {
            sleeper.GetComponent<Sleeper>().StopDistract();
        }

        isDistracted = false;
    }

    public void AlertSleepers(Vector3 alertPosition)
    {
        isAlerted = true;
        Collider[] alertedSleepers = Physics.OverlapSphere(alertPosition, distractionRadius, sleeperLayer);

        foreach (Collider sleeper in alertedSleepers)
        {
            sleeper.GetComponent<Sleeper>().Alert();
        }
    }
}
