using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopDetector : MonoBehaviour
{
    [SerializeField] private CarrierBehaviour _Carrier;
    [SerializeField] private GameObject _marker;

    public bool IsPlayerInRange { get { return _marker.activeSelf; } }

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent<PlayerMotion>(out PlayerMotion resultPlayer))
            _marker.SetActive(true);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<PlayerMotion>(out PlayerMotion resultPlayer))
            _marker.SetActive(false);
    }

    public void Close()
    {
        _marker.SetActive(false);
        this.gameObject.SetActive(false);
    }
}
