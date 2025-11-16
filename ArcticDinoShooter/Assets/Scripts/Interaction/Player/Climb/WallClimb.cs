using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallClimb : MonoBehaviour
{
    private int vaultLayer;
    private Camera _camera;
    private float playerHeight = 2f;
    private float playerRadius = 0.5f;

    void Start()
    {
        vaultLayer = LayerMask.NameToLayer("VaultLayer");
        vaultLayer = ~vaultLayer;
        _camera = Camera.main;
    }

    private void Update()
    {
        Vault();
    }

    private void Vault()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (Physics.Raycast(_camera.transform.position, _camera.transform.forward, out var firstHit, 1f, vaultLayer))
            {
                print("vaultable in front");
                if (Physics.Raycast(firstHit.point + (_camera.transform.forward * playerRadius) + (Vector3.up * 0.6f * playerHeight), Vector3.down, out var secondHit, playerHeight))
                {
                    print("found place to land");
                    StartCoroutine(LerpVault(secondHit.point, 0.5f));
                }
            }
        }

    }

    IEnumerator LerpVault(Vector3 targetPosition, float duration)
    {
        float time = 0;
        Vector3 startPosition = transform.position;

        while (time < duration)
        {
            transform.position = Vector3.Lerp(startPosition, targetPosition, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        transform.position = targetPosition;
    }
}