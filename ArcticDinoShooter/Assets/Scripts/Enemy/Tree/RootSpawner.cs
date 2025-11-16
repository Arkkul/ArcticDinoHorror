using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RootSpawner : MonoBehaviour
{
    public GameObject prefabToSpawn;
    public Transform spawnPoint;
    public float spawnInterval = 1.0f;
    public float speed = 2.0f;

    [SerializeField] private Transform playerTransform;
    public float nextSpawnTime;
    public Vector3 lastSpawnPosition;

    private TreeHealth _treeHealth;

    private void Start()
    {
        nextSpawnTime = Time.time + spawnInterval;
        lastSpawnPosition = spawnPoint.position;
    }

    public void SpawnPrefab()
    {

        Vector3 spawnPosition = lastSpawnPosition;
        Vector3 direction = (playerTransform.position - spawnPosition).normalized;

        spawnPosition += direction * spawnInterval;

        GameObject newPrefab = Instantiate(prefabToSpawn, spawnPosition, Quaternion.identity);
        Rigidbody rb = newPrefab.GetComponent<Rigidbody>();
        if (rb != null)
        {
            // �������� �������� �� Y
            rb.linearVelocity = new Vector3(direction.x * speed, 0f, direction.z * speed);
        }

        lastSpawnPosition = spawnPosition;

        StartCoroutine(DestroyAfterDelay(newPrefab, 6.0f));
    }

    private IEnumerator SpawnInterval()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnInterval);
            SpawnPrefab();
        }
    }

    private IEnumerator DestroyAfterDelay(GameObject obj, float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(obj);
    }
}
