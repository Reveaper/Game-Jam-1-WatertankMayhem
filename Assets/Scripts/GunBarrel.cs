using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunBarrel : MonoBehaviour
{
    private ObjectPooler _objectPooler;

    private void Start()
    {
        _objectPooler = ObjectPooler.Instance;
    }

    public void FireProjectile(float bonusDamage)
    {
        GameObject spawnedObject = _objectPooler.SpawnFromPool("ProjectilePlayer", this.transform.position, this.transform.rotation);
        Projectile projectile = spawnedObject.GetComponent<Projectile>();
        projectile.InitializeProjectile(bonusDamage);
    }
}