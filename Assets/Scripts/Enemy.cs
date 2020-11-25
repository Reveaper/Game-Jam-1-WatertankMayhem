using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [SerializeField] protected NavMeshAgent _agent;
    [SerializeField] private ParticleEnemyDeath _particlesDeath;
    [SerializeField] private GunBarrelEnemy _barrel;
    [SerializeField] private EnemyLOS _LOS;
    [SerializeField] private GameObject _aggroMark;

    private EnemyManager _managerEnemy;
    public EnemyManager EnemyManager { set { _managerEnemy = value; } }


    private PlayerMotion _playerShip;
    public PlayerMotion PlayerShip { get { return _playerShip; } set { _playerShip = value; } }

    private CarrierBehaviour _carrier;
    public CarrierBehaviour Carrier { set { _carrier = value; } }

    protected Vector2 _health = new Vector2(100, 100);
    private Transform _target;

    public Transform CurrentTarget { get { return _target; } }


    private bool _isFiring;
    public bool IsFiring { set { _isFiring = value; } }
    private Vector2 _fireTimer = new Vector2(0, 1);

    private float _fireRange = 35f;

    public virtual void Update()
    {
        _agent.SetDestination(_target.position);

        if(_LOS.TargetInLOS && _fireTimer.x <= 0)
        {
            _fireTimer.x += _fireTimer.y;
            FireWeapon();
        }
        else
            _fireTimer.x = _fireTimer.x < 0 ? 0 : _fireTimer.x - Time.deltaTime;
    }

    private void FireWeapon()
    {
        _barrel.FireProjectile();
    }

    public void Damage(float amount)
    {
        _health.x -= amount;

        if (_health.x <= 0)
            Execute();
    }

    public virtual void Execute()
    {
        GameObject sound = ObjectPooler.Instance.SpawnFromPool("SoundEnemyDeath", this.transform.position, this.transform.rotation);
        sound.GetComponent<AudioSource>().Play();

        GameObject collectibleParticles = ObjectPooler.Instance.SpawnFromPool("CollectibleParticles", this.transform.position, Quaternion.LookRotation(Vector3.up, Vector3.right));
        ParticleEnemyDeath energy = collectibleParticles.GetComponent<ParticleEnemyDeath>();
        energy.Player = _playerShip;
        Player.Instance.EnemiesDestroyed++;
        Player.Instance.GameEndEnemiesKilled++;
        ObjectPooler.Instance.SpawnFromPool("DestructionParticles", this.transform.position, Quaternion.LookRotation(Vector3.up, Vector3.right)); ;
        _managerEnemy.EnemyDestroyed(this);
        this.gameObject.SetActive(false);
        
    }

    public void FocusCarrier()
    {
        if (_carrier != null)
        {
            _target = _carrier.transform;
            _aggroMark.SetActive(false);
        }
        else
            FocusPlayer();
    }

    public void FocusPlayer()
    {
        _target = _playerShip.transform;
        _aggroMark.SetActive(true);
    }

    public void ResetEnemy()
    {
        _health.x = _health.y;

        if(_LOS != null)
        {
            _isFiring = false;
            _LOS.TargetInLOS = false;
        }

    }
}