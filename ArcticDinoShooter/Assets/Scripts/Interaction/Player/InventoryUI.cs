using System.Collections.Generic;
using UnityEditor.Search;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] private GameObject _itemPanelPrefab;
    [SerializeField] private GameObject _inventoryPanel;
    private Inventory _inventory;
    [SerializeField] private List<ItemInfo> _items = new List<ItemInfo>();

    private void Start()
    {
        _inventory = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<Inventory>();
        ShowInventory();
    }

    public void ShowInventory()
    {
        _items = _inventory.GetItemList();
        foreach (var item in _items)
        {
            InventorySlot _inventorySlot = Instantiate(_itemPanelPrefab, Vector3.zero, Quaternion.identity, _inventoryPanel.transform).GetComponent<InventorySlot>(); ;
            print(item.name + "werf");
            _inventorySlot.SetImage(item.icon);
            _inventorySlot.SetName(item.name);
        }
    }
}
