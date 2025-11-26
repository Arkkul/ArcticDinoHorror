using UnityEngine;
using UnityEngine.Events;

public class InteractTrigger : MonoBehaviour
{
    [SerializeField] UnityEvent _onEnter;
    [SerializeField] UnityEvent _onExit;

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerHealth>() != null)
        {
            if (_onEnter != null)
                _onEnter.Invoke();
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<PlayerHealth>() != null)
        {
            if (_onExit != null)
                _onExit.Invoke();
        }
    }
}
