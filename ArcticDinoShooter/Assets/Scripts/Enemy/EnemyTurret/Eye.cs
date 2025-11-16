using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eye : MonoBehaviour
{
    public Transform player; // —сылка на игрока
    public float rotationSpeed = 1.0f; // —корость поворота

    private void Update()
    {
        Vector3 targetDirection = player.position - transform.position;
        float step = rotationSpeed * Time.deltaTime;
        Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, step, 0.0f);
        transform.rotation = Quaternion.LookRotation(newDirection);
    }
}
