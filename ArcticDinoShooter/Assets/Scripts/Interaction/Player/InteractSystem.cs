using TMPro;
using UnityEngine;

public class InteractSystem: MonoBehaviour
{
    [SerializeField] private Camera _playerCamera;
    [SerializeField] private float _interactDistance;

    [SerializeField] private TMP_Text interactText;

    private Iinteractable interactableObject;

    [SerializeField] private LayerMask _layerMask;
    
    private void Awake()
    {
        _playerCamera = Camera.main;
    }

    void Update()
    {
        RayCastInteract();
    }

    private void RayCastInteract()
    {
        Ray ray = _playerCamera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, _interactDistance, _layerMask))
        {
            RaycastHit otherHit;
            Physics.Raycast(ray, out otherHit, _interactDistance);

            if (otherHit.collider == hit.collider)
            {
                interactableObject = hit.collider.GetComponent<Iinteractable>();

                interactText.gameObject.SetActive(true);
                interactText.text = interactableObject.GetInteractionHint();
            }
        }
        else
        {
            interactableObject = null;
            
            interactText.gameObject.SetActive(false);
        }
    }

    public void Interact()
    {
      if(interactableObject != null )
        interactableObject.Interact();
    }
}
