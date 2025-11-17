using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField] private GameObject _curentItem;

    [SerializeField] private Camera _playerCamera;
    [SerializeField] private float _interactDistance;

    [SerializeField] private TMP_Text hint;
    [SerializeField] private GameObject _inventoryUI;
    private bool _isInventoryOpen = false;

    private GameObject _interactableObject;

    [SerializeField] private LayerMask _layerMask;

    [SerializeField] private List<ItemInfo> _items;

    private void Awake()
    {
        _playerCamera = Camera.main;
    }

    private void Update()
    {
        FindItem();
    }

    public void OpenCloseInventory()
    {
        if (_isInventoryOpen)
        {
            _inventoryUI.SetActive(false);
            _isInventoryOpen = false;
        }
        else
        {
            _inventoryUI.SetActive(true);
            _inventoryUI.GetComponentInChildren<InventoryUI>().ShowInventory();
            _isInventoryOpen = true;
        }
           
    }

    public List<ItemInfo> GetItemList()
    {
        return _items;
    }

    public void UseItem()
    {
        if (_curentItem != null)
            _curentItem.GetComponent<Item>().Use();
    }

    public void ChangeCurentItem(ItemInfo itemInfo)
    {
        Destroy(_curentItem);
        _curentItem = Instantiate(itemInfo.prefab, transform);
        _curentItem.transform.localPosition = Vector3.zero;
        _curentItem.transform.localRotation = Quaternion.identity;

       // _curentItem.GetComponent<Item>().Equip(transform);
    }

    public void TakeItem()
    {
        if (_interactableObject != null)
        {
            if(_curentItem != null)
            {
                Destroy( _curentItem );
            }
            _curentItem = _interactableObject;
            _curentItem.GetComponent<Item>().Equip(transform);
            _items.Add(_interactableObject.GetComponent<Item>().GetInfo());
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
        if (_curentItem != null)
        {
            _items.Remove(_curentItem.GetComponent<Item>().GetInfo());
            _curentItem.GetComponent<Item>().Drop();
            _curentItem = null;
            Debug.Log(_curentItem);
        }
    }

    public void ThrowItem()
    {
        if (_curentItem != null)
        {
            _items.Remove(_curentItem.GetComponent<Item>().GetInfo());
            _curentItem.GetComponent<Item>().Trow();
            _curentItem = null;
        }
    }

    public bool HasCurrentItem(string itemId)
    {
        if (_curentItem != null)
        {
            Item item = _curentItem.GetComponent<Item>();
            return item != null && item.ID == itemId;
        }
        return false;
    }

    public void DropCurrentItem()
    {
        if (_curentItem != null)
        {
            DropItem();
        }
    }
}
