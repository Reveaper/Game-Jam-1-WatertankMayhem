using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarrierShield : MonoBehaviour
{
    [SerializeField] private CarrierBehaviour _carrier;

    public void Damage(float amount)
    {
        _carrier.Damage(amount);
    }
}
