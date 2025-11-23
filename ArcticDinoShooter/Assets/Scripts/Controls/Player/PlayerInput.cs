using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    private Controls _input;
    private IControllable _controllable;
    private Inventory _inventory;

    private void Awake()
    {
        _input = new Controls();
        _controllable = GetComponent<IControllable>();
        _inventory = GetComponentInChildren<Inventory>();

        _input.Player.Crouch.performed += Crouch_performed;
        _input.Player.Interacte.performed += Interacte_performed;
        _input.Player.EquipItem.performed += EquipItem_performed;
        _input.Player.DropItem.performed += DropItem_performed;
        _input.Player.ThrowItem.performed += ThrowItem_performed;
        _input.Player.UseItem.performed += UseItem_performed;
        _input.Player.Run.canceled += Run_canceled;
        _input.Player.Run.performed += Run_performed;
        _input.Player.Aim.performed += Aim_performed;
        _input.Player.Aim.canceled += Aim_canceled;
        _input.UI.Pause.performed += Pause_performed;
        _input.UI.OpenCloseInventory.performed += OpenCloseInventory_performed;
        _input.Player.Jump.performed += Jump_performed;
        
    }

    private void Jump_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        _controllable.Jump();
    }

    private void Aim_canceled(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        _inventory.StopAim();
    }

    private void Aim_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        _inventory.Aim();
    }

    private void OpenCloseInventory_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        _inventory.OpenCloseInventory();
    }

    private void Pause_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        throw new System.NotImplementedException();
    }

    private void Run_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        _controllable.Run();
    }

    private void Run_canceled(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
       _controllable.StopRun();
    }

    private void UseItem_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        _inventory.UseItem();
    }

    private void ThrowItem_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        _inventory.ThrowItem();
    }

    private void DropItem_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        _inventory.DropItem();
    }

    private void EquipItem_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        _inventory.TakeItem();
    }

    private void Interacte_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        _controllable.Interact();
    }

    private void Crouch_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        _controllable.Crouch();
    }

    private void Update()
    {
        ReadMovement();
    }

    private void ReadMovement()
    {
        var _movement = _input.Player.Move.ReadValue<Vector2>();
        Vector3 _moveDirection = (_movement.y * transform.forward + _movement.x * transform.right);

        _controllable.Move(_moveDirection);
    }

    private void OnEnable()
    {
        _input.Enable();
    }

    private void OnDisable()
    {
        _input.Disable();
    }
}
