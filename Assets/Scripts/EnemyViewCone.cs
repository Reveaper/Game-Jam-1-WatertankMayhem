using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyViewCone : MonoBehaviour
{
    [SerializeField] private Enemy _enemy;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<PlayerMotion>(out PlayerMotion result))
            _enemy.FocusPlayer();
    }
}
