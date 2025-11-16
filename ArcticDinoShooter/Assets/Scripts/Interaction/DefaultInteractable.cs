using UnityEngine;
using UnityEngine.Events;


public class DefaultInteractable : MonoBehaviour, Iinteractable
{
    [SerializeField] private string _interactableText = "Press E to Interact";
    [SerializeField] private UnityEvent _onInteract;

    public string GetInteractionHint()
    {
        return _interactableText;
    }

    public void Interact()
    { 
        _onInteract.Invoke(); 
    }

    public void InteractWith(Item item)
    {
        _onInteract.Invoke();
    }
}
