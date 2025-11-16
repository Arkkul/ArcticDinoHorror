using UnityEngine;

public class HealthUpTrigger : MonoBehaviour
{
    [SerializeField] private int _healtPoints;

    private void OnTriggerEnter(Collider other)
    {
        HealthUp(other.gameObject, _healtPoints);
    }

    private void HealthUp(GameObject _Character, int _healthPoints)
    {
        _Character.GetComponent<IHealth>().HealthUp(_healthPoints);
    }
}
