using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private Rigidbody _rigidBody;
    [SerializeField] private ParticleSystem _particle;
    private float _baseDamage = 25f;
    private float _bonusDamage = 0;
    private float _baseSpeed = 50f;

    public void InitializeProjectile(float bonusDamage)
    {
        _bonusDamage = bonusDamage;
        _rigidBody.velocity = this.transform.forward * _baseSpeed;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent<Enemy>(out Enemy result))
        {
            //_particle?.Play();
            result.Damage(_baseDamage + _bonusDamage);
        }

        this.gameObject.SetActive(false);
    }
}