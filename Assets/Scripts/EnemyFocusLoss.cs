using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFocusLoss : MonoBehaviour
{
    [SerializeField] private Enemy _enemy;

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out PlayerMotion result))
            _enemy.FocusCarrier();
    }
}
