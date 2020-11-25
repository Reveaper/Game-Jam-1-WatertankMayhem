using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLineOfSight : MonoBehaviour
{
    [SerializeField] private Enemy _enemy;

    private bool _targetInViewcone;
    public bool TargetInViewcone { get { return _targetInViewcone; } set { _targetInViewcone = value; } }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == _enemy.CurrentTarget.gameObject)
            _targetInViewcone = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == _enemy.CurrentTarget.gameObject)
            _targetInViewcone = false;
    }
}
