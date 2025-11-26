using UnityEngine;

public class ShootingWeapon : MonoBehaviour, Iusable
{
    [SerializeField] private float _shootDistance;
    [SerializeField] private int _numberOfAmmo;
    [SerializeField] private int _loadedAmmo;
    [SerializeField] private int _magazineCapacity;
    [SerializeField] private int _damage;
    [SerializeField] private AudioSource _shootSound;
    [SerializeField] private AudioSource _reloadSound;
    [SerializeField] private LayerMask _layerMask;

    private Camera _playerCamera;
    private Controls _input;

    private void Awake()
    {
        _input = new Controls();
        _input.Player.ReloadWeapon.performed += ReloadWeapon_performed;
    }

    private void ReloadWeapon_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        if (_loadedAmmo < _magazineCapacity && _numberOfAmmo > 0)
        {
            int ammoNeeded = _magazineCapacity - _loadedAmmo;
            int ammoToTransfer = Mathf.Min(ammoNeeded, _numberOfAmmo);

            _loadedAmmo += ammoToTransfer;
            _numberOfAmmo -= ammoToTransfer;
            _reloadSound.Play();
        }
    }

    private void OnEnable()
    {
        _input.Enable();
    }

    private void OnDisable()
    {
        _input.Disable();
    }

    private void Start()
    {
        _playerCamera = Camera.main;
    }

    public void Use()
    {
        if(_loadedAmmo > 0)
        {
            Shoot();
        }
    }

    private void Shoot()
    {
        _loadedAmmo--;
        _shootSound.Play();

        Ray ray = _playerCamera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
        RaycastHit hit;
        RaycastHit otherHit;
        if (Physics.Raycast(ray, out otherHit, _shootDistance))
        {
            print(otherHit.collider.gameObject.name);
            if (Physics.Raycast(ray, out hit, _shootDistance, _layerMask))
            {
                if (hit.collider == otherHit.collider)
                {
                    print(hit.collider.gameObject.name);
                    hit.transform.gameObject.GetComponent<IHealth>().TakeDamage(_damage);
                }
            }
        }
    }
}
