using UnityEngine;

public class Door : MonoBehaviour, Iinteractable
{
    [SerializeField] private bool _closedOnKey;
    [SerializeField] private bool _closed;
    private Animator _animator;

    [SerializeField] private string _requiredKeyID;

    private void Start()
    {
        _animator = GetComponent<Animator>();
    }

    private void InsertKey(string _key)
    {
        if (_key == _requiredKeyID)
        {
           /* _closedOnKey = !_closedOnKey;
            Debug.Log(_closedOnKey);*/
            if (_closedOnKey)
            {
                Debug.Log("дверь открыта ключом");
                _closedOnKey = false;
                Open();
            }
            else
            {
                Debug.Log("дверь закрыта на ключ");
                Close();
                _closedOnKey = true;
            }
        }
    }

    private void TrySwitchState()
    {
        if (!_closedOnKey)
        {
            if (_closed)
            {
                Open();
            }
            else
            {
                Close();
            }
        }
        else
        {
            Debug.Log("закрыто на ключ");
        }
    }

    private void Open()
    {
        _animator.SetTrigger("Open");
        Debug.Log("Открыл");
        _closed = false;
    }

    private void Close()
    {
        _animator.SetTrigger("Close");
        Debug.Log("Закрыл");
        _closed = true;
    }

    public string GetInteractionHint()
    {
        return _closedOnKey ? "закрыто на ключ" : _closed? "нажмите F, чтобы открыть": "нажмите F, чтобы закрыть";
    }

    public void Interact()
    {
       TrySwitchState();
    }

    public void InteractWith(Item item)
    {
        InsertKey(item.ID);
    }
}
