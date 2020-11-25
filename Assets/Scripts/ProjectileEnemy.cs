using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileEnemy : MonoBehaviour
{
    [SerializeField] private Rigidbody _rigidBody;
    [SerializeField] private ParticleSystem _particle;
    private float _damage = 18f;
    private float _baseSpeed = 50f;

    [SerializeField] private LayerMask _obstacleMask;

    private void Awake()
    {
        _obstacleMask = LayerMask.NameToLayer("Obstacle");
    }

    public void InitializeProjectile()
    {
        _rigidBody.velocity = this.transform.forward * _baseSpeed;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer != _obstacleMask)
        {
            if (other.TryGetComponent<CarrierShield>(out CarrierShield shield))
            {
                //_particle?.Play();
                shield.Damage(_damage);
            }
            else if (other.TryGetComponent<CarrierBehaviour>(out CarrierBehaviour carrier))
            {
                //_particle?.Play();
                carrier.Damage(_damage);
            }

            if (other.TryGetComponent<PlayerMotion>(out PlayerMotion resultPlayer))
            {
                //_particle?.Play();
                resultPlayer.Damage(_damage);
            }
        }

        this.gameObject.SetActive(false);
    }
}