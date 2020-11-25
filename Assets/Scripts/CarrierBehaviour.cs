using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CarrierBehaviour : MonoBehaviour
{
    [SerializeField] private CarrierShield _shieldObject;
    [SerializeField] private ShopDetector _shopDetector;
    [SerializeField] private GameObject _shopCanvas;
    public bool IsShopOpen { get { return _shopCanvas.gameObject.activeSelf; } }

    private EnemyManager _managerEnemy;
    public EnemyManager EnemyManager { set { _managerEnemy = value; } }

    private PlayerMotion _playerShip;
    public PlayerMotion PlayerShip { set { _playerShip = value; } }

    [SerializeField] private List<Transform> _waypoints = new List<Transform>();
    private NavMeshAgent _agent;
    private float _agentSpeedDefault;
    private int _currentWaypoint;

    private Vector2 _health;

    private Vector2 _shield;
    private float _regenerationPerSec = 10f;
    private float _transferStrengthMultiplier = 1.5f;

    private Vector2 _shieldReviveTimer = new Vector2(0, 3);

    private float _energyHarvested;
    private float _spendableEnergy = 1000;
    private float _energyHarvestPerSec = 20f;  

    public float EnergyHarvested { get { return _energyHarvested; } }
    public float SpendableEnergy { get { return _spendableEnergy; } set { _spendableEnergy = value; } }
    public Vector2 Health { get { return _health; } set { _health = value; } }
    public Vector2 Shield { get { return _shield; } set { _shield = value; } }

    //Bonus stuff
    public float ShieldBaseValue { get { return 1000; } }
    public float BonusRegenerationPerSec { get; set; }
    public float HealthBaseValue { get { return 750; } }
    public float BonusEnergyHarvestPerSec { get; set; }


    private void Start()
    {
        _health = new Vector2(HealthBaseValue, HealthBaseValue);
        _shield = new Vector2(ShieldBaseValue, ShieldBaseValue);

        _agent = this.GetComponent<NavMeshAgent>();
        _currentWaypoint = UnityEngine.Random.Range(0, _waypoints.Count);
        _agent.SetDestination(_waypoints[_currentWaypoint].position);
        _agentSpeedDefault = _agent.speed;
    }

    private void Update()
    {
        if(_shield.x > 0)
        {
            _shield.x = Mathf.Clamp(_shield.x + (_regenerationPerSec + BonusRegenerationPerSec) * Time.deltaTime, 0, _shield.y);
        }

        AddScore((_energyHarvestPerSec + BonusEnergyHarvestPerSec) * Time.deltaTime);

        Vector3 vectorToWaypoint = (_waypoints[_currentWaypoint].position - this.transform.position);
        vectorToWaypoint = Vector3.Scale(vectorToWaypoint, new Vector3(1, 0, 1));

        _agent.speed = _shopDetector.IsPlayerInRange ? 0 : _agentSpeedDefault;

        if (vectorToWaypoint.sqrMagnitude <= 2 * 2)
        {
            _currentWaypoint = (_currentWaypoint + 1) % _waypoints.Count;
            _agent.SetDestination(_waypoints[_currentWaypoint].position);
        }
    }

    public void Damage(float amount)
    {
        if (_shield.x > 0)
        {
            _shield.x -= amount;

            if (_shield.x <= 0)
            {
                _shieldReviveTimer.x = 0;
                _shieldObject.gameObject.SetActive(false);
            }
        }
        else
        {
            _health.x -= amount;

            if(_health.x <= 0)
            {
                this.gameObject.SetActive(false);
                Player.Instance.GameEndTotalScore = _energyHarvested;
                _managerEnemy.ForceFocusPlayer();
                _playerShip.ReleaseTether();
            }
        }
    }

    public void TransferEnergy(ref Vector2 amount)
    {
        if (_shield.x > 0)
        {
            _shield.x += amount.x * _transferStrengthMultiplier;
            amount.x = 0;
        }
        else if (((float)amount.x / (float)amount.y) >= 0.5f)
        {
            _shieldReviveTimer.x = 0;
            _shield.x = Mathf.Clamp(_shield.y * 0.5f + (amount.x - (amount.y * 0.75f)) * 5, _shield.y * 0.5f, _shield.y);
            amount.x = 0;
            _shieldObject.gameObject.SetActive(true);
        }
    }

    public void AddScore(float amount)
    {
        _energyHarvested += amount;
        _spendableEnergy += amount;
        Player.Instance.CurrentEnergyHarvested += amount;
    }


    public void EnableShop()
    {
        _shopDetector.gameObject.SetActive(true);
        _shopCanvas.SetActive(false);
        _shieldObject.gameObject.SetActive(false);
    }

    public void DisableShop()
    {
        _shieldObject.gameObject.SetActive(_shield.x > 0);
        _shopDetector.Close();
        CloseShop();
    }

    public void OpenShop()
    {
        if (_shopDetector.gameObject.activeSelf && _shopDetector.IsPlayerInRange)
        {
            _shopCanvas.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    public void CloseShop()
    {
        _shopCanvas.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}
