using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretController : MonoBehaviour
{
    [Header("Properties")]
    [SerializeField] private float _fireRate = 0.05f;
    [SerializeField] private int _damageAmount = 20;
    [SerializeField] private float _rotationSpeed = 5f;
    [SerializeField] private float _maxShootDistance = 10f;

    [SerializeField] private Transform _turretPivot;
    [SerializeField] private Transform _firePoint;
    [SerializeField] private ParticleSystem _shootEffect;

    [Header("Targets")]
    [SerializeField] private List<GameObject> _targets;

    private float _timeTillNextShot;
    private GameObject _activeTarget;

    [SerializeField]private bool _isFollowing = false;

    void Start()
    {
        _timeTillNextShot = _fireRate;
        _targets.Add(GameObject.FindGameObjectWithTag("Player"));
    }

    public void StartFollow()
    {
        _isFollowing = true;
    }

    public void StopFollow()
    {
        _isFollowing = false;
    }

    void Update()
    {
        if (_isFollowing)
        {
            LookAtTarget();

            if (_timeTillNextShot <= 0)
            {
                _timeTillNextShot = _fireRate;
                if (_activeTarget != null)
                {
                    Shoot();
                }
            }
            else
            {
                _timeTillNextShot -= Time.deltaTime;
            }
        }
    }

    private GameObject GetNextTarget()
    {
        if (_targets.Count > 0)
        {
            GameObject target = _targets[0];
          //  _targets.Remove(target);
            return target;
        }

        return null;
    }

    private void LookAtTarget()
    {
        //Find the next target
        if (_activeTarget == null || !_activeTarget.activeSelf)
        {
            _activeTarget = GetNextTarget();
        }

        //If there are no more targets we can bail
        if (_activeTarget == null)
        {
            return;
        }

        //Get the direction
        Vector3 direction = _activeTarget.transform.position - _turretPivot.position;
        //Get the look rotation
        Quaternion lookRotation = Quaternion.LookRotation(direction);

        //Smoothly rotate towards the target using all axes
        _turretPivot.rotation = Quaternion.Slerp(_turretPivot.rotation, lookRotation, _rotationSpeed * Time.deltaTime);
    }

    private void Shoot()
    {
       // Debug.Log("shoot");
       // _shootEffect.Play();

        Ray shootRay = new Ray(_firePoint.position, _firePoint.forward);

        if (Physics.Raycast(shootRay, out RaycastHit hitInfo, _maxShootDistance))
        {
            PlayerHealth health = hitInfo.transform.gameObject.GetComponent<PlayerHealth>();
            //Debug.Log(hitInfo.transform.gameObject.name);
            if (health == null)
            {
                Debug.LogWarning("We hit something that doesn't have health........."  );

            }
            else
            {
                health.TakeDamage(_damageAmount);
            }
        }
    }
}
