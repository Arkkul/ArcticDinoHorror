
using System.Collections;
using UnityEngine;

public class TriggerEnemyTree : MonoBehaviour
{
    public RootSpawner rootSpawner;
    private bool isPlayerInsideTrigger = false;

    private void Start()
    {
        StartCoroutine(SpawnObjects());
    }

    private IEnumerator SpawnObjects()
    {
        while (true)
        {
            if (isPlayerInsideTrigger)
            {
                rootSpawner.SpawnPrefab();              
            }

            yield return new WaitForSeconds(rootSpawner.spawnInterval);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInsideTrigger = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInsideTrigger = false;
        }
    }

    public IEnumerator PursuitDelay()
    {
        while (true)
        {
            yield return new WaitForSeconds(3);
            rootSpawner.lastSpawnPosition = rootSpawner.spawnPoint.position;
        }
    }
}
