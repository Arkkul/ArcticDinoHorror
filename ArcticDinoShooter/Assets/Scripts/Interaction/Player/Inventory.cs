using TMPro;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField] private GameObject _currentItem;

    [SerializeField] private Camera _playerCamera;
    [SerializeField] private float _interactDistance;

    [SerializeField] private TMP_Text hint;

    private GameObject _interactableObject;

    [SerializeField] private LayerMask _layerMask;

    private void Awake()
    {
        _playerCamera = Camera.main;
    }

    private void Update()
    {
        FindItem();
    }

    public void UseItem()
    {
        if (_currentItem != null)
            _currentItem.GetComponent<Item>().Use();
    }

    public void TakeItem()
    {
        if (_interactableObject != null && _currentItem == null)
        {
            _currentItem = _interactableObject;
            _currentItem.GetComponent<Item>().Equip(transform);
            Debug.Log("взял");
        }
       
    }

    private void FindItem()
    {
        Ray ray = _playerCamera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, _interactDistance, _layerMask))
        {
            RaycastHit otherHit;
            Physics.Raycast(ray, out otherHit, _interactDistance);

                if (otherHit.collider == hit.collider)
                {
                    //Debug.Log(hit.collider.transform.name);
                    _interactableObject = hit.collider.gameObject;


                    hint.gameObject.SetActive(true);

                    hint.text = _interactableObject.GetComponent<Iinteractable>().GetInteractionHint();
                } 
        }
        else
        {
            _interactableObject = null;
            hint.gameObject.SetActive(false);
        }
    }

    public void DropItem()
    {
        if (_currentItem != null)
        {
            _currentItem.GetComponent<Item>().Drop();
            _currentItem = null;
            Debug.Log(_currentItem);
        }
    }

    public void ThrowItem()
    {
        if (_currentItem != null)
        {
            _currentItem.GetComponent<Item>().Trow();
            _currentItem = null;
        }
    }

    public bool HasCurrentItem(string itemId)
    {
        if (_currentItem != null)
        {
            Item item = _currentItem.GetComponent<Item>();
            return item != null && item.ID == itemId;
        }
        return false;
    }

    public void DropCurrentItem()
    {
        if (_currentItem != null)
        {
            DropItem();
        }
    }
}
