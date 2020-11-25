using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBomberTrigger : MonoBehaviour
{
    [SerializeField] private Enemy _enemy;
    [SerializeField] private GameObject _detonationMark;
    private float _detonationDamage = 125f;
    private float _pushForce = 35f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<PlayerMotion>(out PlayerMotion resultPlayer))
        {
            resultPlayer.Damage(_detonationDamage);

            Vector3 push = (other.transform.position - this.transform.position).normalized * _pushForce;
            resultPlayer.Push(Vector3.Scale(push, new Vector3(1, 0, 1)), 1f);
            _enemy.Execute();
            EZCameraShake.CameraShaker.Instance.ShakeOnce(6f, 4f, 0.05f, 1f);
        }
        else if (other.TryGetComponent<CarrierShield>(out CarrierShield shield))
        {
            shield.Damage(_detonationDamage);
            DetonateOnCarrier();
        }
        else if (other.TryGetComponent<CarrierBehaviour>(out CarrierBehaviour resultCarrier))
        {
            resultCarrier.Damage(_detonationDamage);
            DetonateOnCarrier();
        }
    }

    private void DetonateOnCarrier()
    {
        PlayerMotion player = _enemy.PlayerShip;
        Vector3 direction = player.transform.position - this.transform.position;
        direction = (Mathf.Max(5 - direction.magnitude, 0) * direction.normalized);
        float directionPercentage = direction.magnitude / 5f;
        player.Push(direction, directionPercentage);

        _enemy.Execute();
        EZCameraShake.CameraShaker.Instance.ShakeOnce(directionPercentage * 2f, 4f, 0.05f, directionPercentage);
    }
}
