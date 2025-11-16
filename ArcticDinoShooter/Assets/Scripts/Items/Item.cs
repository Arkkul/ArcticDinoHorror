using UnityEngine;

public class Item : MonoBehaviour, Iinteractable
{
    [SerializeField] private string _id;
    [SerializeField] private float _throwForce;

    private Rigidbody _rigidbody;

    private Iusable _usableItem;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _usableItem = GetComponent<Iusable>();

        _rigidbody.isKinematic = true;
    }

    public string ID
    {
        get { return _id; }
        set { }
    }

    public void Equip(Transform _toolParent)
    {
        _rigidbody.isKinematic = true;

        transform.position = _toolParent.transform.position;
        transform.rotation = _toolParent.transform.rotation;

        transform.SetParent(_toolParent);
    }

    public void Use()
    {
        _usableItem.Use();
    }

    public void Drop()
    {
        transform.SetParent(null);

        _rigidbody.isKinematic = false;
    }

    public void Trow()
    {
        transform.SetParent(null);

        _rigidbody.isKinematic = false;

        _rigidbody.AddForce(transform.forward * _throwForce, ForceMode.Impulse);
    }

    public string GetInteractionHint()
    {
        return ("Нажмите F чтобы экипировать");
    }

    public void Interact()
    {
        throw new System.NotImplementedException();
    }

    public void InteractWith(Item item)
    {
        throw new System.NotImplementedException();
    }
}
