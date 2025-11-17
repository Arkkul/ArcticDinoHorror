using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _name;
    [SerializeField] private Image _icon;
    [SerializeField] ItemInfo _itemInfo;
    private Inventory _inventory;

    private void Start()
    {
        _inventory = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<Inventory>();
    }

    public void SetName(string name)
    {
        _name.text = name;
    }

    public void SetImage(Sprite icon)
    {
        _icon.sprite = icon;
    }

    public void SetItemInfo(ItemInfo itemInfo)
    {
        _itemInfo = itemInfo;
    }

    public void RemoveItemFormInvetory()
    {

    }

    public void EquipItem()
    {
        _inventory.ChangeCurentItem(_itemInfo);
    }
}
