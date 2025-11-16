using UnityEngine;
using System.Collections;

public class Flashlight : MonoBehaviour, Iusable
{
    [SerializeField] private float _currentCharge;
    [SerializeField] private float rayDistance = 10f; // ƒистанци€ луча
    private float _maxCharge = 100f;
    private bool switchValue;

    [SerializeField] private LayerMask _targetMask;

    private Light _light;
    public Transform rayOrigin; // “очка, из которой будет выпускатьс€ луч


    private void Start()
    {
        _currentCharge = _maxCharge;

        _light = GetComponentInChildren<Light>();

        switchValue = _light.enabled;
    }

    public void Use()
    {
        if (_currentCharge > 0)
        {
            switchValue = !switchValue;

            if (switchValue == true)
            {
                SwitchOn();
                ShootRay();
            }
            else
            {
                SwitchOff();
            }
        }
    }

    private void SwitchOn()
    {
        _light.enabled = true;
        StartCoroutine(DecreaseChargeOverTime());
    }

    private void SwitchOff()
    {
        _light.enabled = false;
        StopAllCoroutines();
    }

    private IEnumerator DecreaseChargeOverTime()
    {
        while (_currentCharge > 0)
        {
            // ”меньшаем зар€д каждые несколько секунд
            yield return new WaitForSeconds(2f);
            _currentCharge -= 1f; // »змените шаг уменьшени€ зар€да по своему усмотрению
            Debug.Log("Current charge: " + _currentCharge);
        }

        if (_currentCharge == 0)
        {
            _light.enabled = false;
        }
    }

    void ShootRay()
    {
        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.transform.CompareTag("Enemy"))
            {
                IRayCastHit rayCastHit = hit.transform.GetComponent<IRayCastHit>();
                if (rayCastHit != null)
                {
                    rayCastHit.OnRayHit();
                }
            }
        }
    }

}
