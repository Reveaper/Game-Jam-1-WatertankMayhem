using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunBarrelEnemy : MonoBehaviour
{
    private ObjectPooler _objectPooler;

    private void Start()
    {
        _objectPooler = ObjectPooler.Instance;
    }

    public void FireProjectile()
    {
        GameObject spawnedObject = _objectPooler.SpawnFromPool("ProjectileEnemy", this.transform.position, this.transform.rotation);
        ProjectileEnemy projectile = spawnedObject.GetComponent<ProjectileEnemy>();
        projectile.InitializeProjectile();
    }
}