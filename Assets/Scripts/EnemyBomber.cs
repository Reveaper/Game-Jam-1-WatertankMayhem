using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBomber : Enemy
{
    

    private void Start()
    {
        _health = new Vector2(225, 225);
    }

    public override void Update()
    {
        _agent.SetDestination(CurrentTarget.position);
    }

    public override void Execute()
    {
        GameObject sound = ObjectPooler.Instance.SpawnFromPool("SoundEnemyDeathBomber", this.transform.position, this.transform.rotation);
        sound.GetComponent<AudioSource>().Play();
        ObjectPooler.Instance.SpawnFromPool("BomberExplosionParticles", this.transform.position, Quaternion.LookRotation(Vector3.up, Vector3.right)); ;
        base.Execute();
    }


}
