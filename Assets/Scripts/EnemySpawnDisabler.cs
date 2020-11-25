using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnDisabler : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<EnemySpawn>(out EnemySpawn enemySpawn))
            enemySpawn.CanSpawn = false;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<EnemySpawn>(out EnemySpawn enemySpawn))
            enemySpawn.CanSpawn = true;
    }
}
