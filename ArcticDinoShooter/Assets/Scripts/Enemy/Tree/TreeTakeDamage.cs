using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeTakeDamage : MonoBehaviour
{
    [SerializeField] private int damaged = 10;
    public GameObject damagePoint;

    [SerializeField] private TreeHealth _treeHealth;

    public void Active()
    {
        _treeHealth.TakeDamage(damaged);
        damagePoint.SetActive(false);
    }
}
