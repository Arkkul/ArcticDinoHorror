using UnityEngine;
using UnityEngine.Events;


public class ItemRequiredInteractable : MonoBehaviour, Iinteractable
{
    [SerializeField] private string _interactableText = "Press E to Interact";
    [SerializeField] private UnityEvent _onInteract;
    [SerializeField] private UnityEvent _onInteractWith;

    [SerializeField] private string _keyId;

    public string GetInteractionHint()
    {
        return _interactableText;
    }

    public void Interact()
    {
        Debug.Log("нужен ключ");
        _onInteract.Invoke();
    }

    public void InteractWith(Item item)
    {
        print(item.ID);
        if(item.ID == _keyId)
        {
            _onInteractWith.Invoke();
        }
        else
        {
            Interact();
        }
       
    }
}
