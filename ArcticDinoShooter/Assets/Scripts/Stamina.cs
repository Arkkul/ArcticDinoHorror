using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stamina : MonoBehaviour
{
    [SerializeField] private float _maxEnergy = 100f;
    [SerializeField] private float _minEnergy = 0f;
    [SerializeField] private float _energyRestoreStep = 5f;
   
    private bool _isSpendingEnergy;

    [SerializeField] private float _energy;
    private float _Energy
    {
        get { return _energy; }
        set { _energy = Mathf.Clamp(value, _minEnergy, _maxEnergy); }
    }

    private void Awake()
    {
        _Energy = _maxEnergy;
    }

    public void SpendEnergy(float _energyToSpend)
    {       
            _Energy -= _energyToSpend * Time.deltaTime;  
    }

    public void RestoreEnergy()
    {   
             _Energy += _energyRestoreStep * Time.deltaTime;   
    }

    public float GetEnergy()
    {
        return _Energy;
    }

    public bool HasEnergy()
    {
        return _Energy > 10;
    }
}
