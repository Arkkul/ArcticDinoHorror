using UnityEngine;

public class Key : MonoBehaviour, Iusable
{
  //  [SerializeField] private string _id;

    [SerializeField] private float _distanceToDoor;

    private Camera _playerCamera;

    private int _layerNumber = 9;
    private int _layerMask;

    private void Awake()
    {
        _layerMask = 1 << _layerNumber;

        _playerCamera = Camera.main;
    }

    public void Use()
    {
       TryOpenDoor();
    }

    private void TryOpenDoor()
    {
        Ray ray = _playerCamera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, _distanceToDoor, _layerMask))
        {
            if (hit.collider.GetComponent<Door>() != null)
            {
                Item _item = transform.GetComponent<Item>();
                hit.collider.GetComponent<Iinteractable>().InteractWith(_item);
            }
        }
    }
}
