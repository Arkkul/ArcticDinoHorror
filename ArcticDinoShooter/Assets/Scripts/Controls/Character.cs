using System.Collections;
using UnityEngine;

public class Character : MonoBehaviour, IControllable
{
    private float _speed;
    [SerializeField] private float _walkSpeed;
    [SerializeField] private float _runSpeed;
    [SerializeField] private float _energyToRun = 4;

    [SerializeField] private float _crouchHeight;
    [SerializeField] private float _crouchTime;
    
    private CharacterController _characterController;
    private InteractSystem _interactSystem;
    private Stamina _stamina;
    private bool _isCrouching = false;
    private float _standHeight;

    private float velocity;

    private bool _isMove; 

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
        _interactSystem = GetComponentInChildren<InteractSystem>();
        _stamina = GetComponent<Stamina>();

        _standHeight = _characterController.height;
        _speed = _walkSpeed;
    }

    private void Update()
    {
        GravityFall();
    }

    private void GravityFall()
    {
        if (!_characterController.isGrounded)
        {
            _characterController.Move(Vector3.up * velocity * Time.deltaTime);
            velocity += -10 * Time.deltaTime;
        }
    }

    public void Crouch()
    {
       if(_isCrouching )
        {
            RaycastHit hit;
            if (!Physics.Raycast(transform.position, Vector3.up, out hit, _crouchHeight + _crouchHeight/2))
            {
                Debug.Log("встаём");
                _isCrouching = false;
                StartCoroutine(SetHeight(_standHeight, _crouchTime));
            }
        }
        else
        {
            Debug.Log("крадёмся");
            _isCrouching = true;
            StartCoroutine(SetHeight(_crouchHeight, _crouchTime));
        }
    }

    private IEnumerator SetHeight(float targetHeight, float duration)
    {
        float time = 0f;
        float startHeight = _characterController.height;

        while (time < duration)
        {
            _characterController.Move(Vector3.zero);
            float newHeight = Mathf.Lerp(startHeight, targetHeight, time / duration);
            _characterController.height = newHeight;
            time += Time.deltaTime;
            yield return null;
        }

    }

    public void Interact()
    {
       _interactSystem.Interact();
    }

    public void Look()
    {
        throw new System.NotImplementedException();
    }

    public void Run()
    {
        if (_stamina.HasEnergy())
        {
            //Debug.Log(_stamina.GetEnergy());
            _speed = _runSpeed;
        }
    }

    public void StopRun()
    {
        _speed = _walkSpeed;
    }

    public void Move(Vector3 _direction)
    {
        if (_direction != Vector3.zero)
        {
            _isMove = true;
            _characterController.Move(_direction * _speed * Time.deltaTime);
        }
        else
        {
            _isMove = false;
        }

        if (_speed == _runSpeed)
        {
            _stamina.SpendEnergy(_energyToRun);
        }
        else
        {
            _stamina.RestoreEnergy();
        }

        if (!_stamina.HasEnergy())
         StopRun();
    }

    public bool IsMove()
    {
        return _isMove;
    }
}
