using UnityEngine;

public class MouseLook : MonoBehaviour
{
    private Controls _input;

    [SerializeField] private float _mouseSensivity = 100f;
    [SerializeField] private Transform _playerBody;

    private Vector2 _mouseLook;
    private float _xRotation = 0f;
   

    private float _mouseX;
    private float _mouseY;

    private void Update()
    {
        Look();
    }

    private void Awake()
    {
        _playerBody = transform.parent;
        _input = new Controls();
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void OnEnable()
    {
        _input.Enable();
    }

    private void OnDisable()
    {
        _input.Disable();
    }

    private void Look()
    {
        _mouseLook = _input.Player.Look.ReadValue<Vector2>();

        _mouseX = _mouseLook.x * Time.deltaTime * _mouseSensivity;
        _mouseY = _mouseLook.y * Time.deltaTime * _mouseSensivity;

        _xRotation -= _mouseY;
        _xRotation = Mathf.Clamp(_xRotation, -90f, 90f);

        transform.localRotation = Quaternion.Euler(_xRotation, 0, 0);
        _playerBody.Rotate(Vector3.up * _mouseX);
    }
}
