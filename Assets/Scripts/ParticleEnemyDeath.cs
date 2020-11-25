using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleEnemyDeath : MonoBehaviour
{
    [SerializeField] private ParticleSystem _particleSystem;
    private List<ParticleCollisionEvent> _collisionEvents;

    private PlayerMotion _player;
    public PlayerMotion Player { set { _player = value; } }

    private void Start()
    {
        _collisionEvents = new List<ParticleCollisionEvent>();
        _player = FindObjectOfType<PlayerMotion>();
    }
    
    private void OnParticleCollision(GameObject other)
    {
        int numCollisionEvents = _particleSystem.GetCollisionEvents(other, _collisionEvents);
        _player.CollectEnergy(numCollisionEvents);
    }
}
