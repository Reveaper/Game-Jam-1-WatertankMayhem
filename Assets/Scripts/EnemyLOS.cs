using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLOS : MonoBehaviour
{
    [SerializeField] private Enemy _enemy;

    private bool _targetInLOS;
    public bool TargetInLOS { get { return _targetInLOS; } set { _targetInLOS = value; } }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == _enemy.CurrentTarget.gameObject)
            _targetInLOS = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == _enemy.CurrentTarget.gameObject)
            _targetInLOS = false;
    }
}
