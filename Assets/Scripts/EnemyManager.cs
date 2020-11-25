using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DiscordPresence;

public enum FocusType
{
    Player, Carrier
}

public class EnemyManager : MonoBehaviour
{
    [SerializeField] private PlayerMotion _playerShip;
    [SerializeField] private CarrierBehaviour _carrier;

    [Header("Enemy")]
    [SerializeField] private Enemy _enemyFighter;
    [SerializeField] private Enemy _enemyBomber;

    [Header("Temp")]
    [SerializeField] private List<EnemySpawn> _enemySpawnPoints = new List<EnemySpawn>();

    private List<Enemy> _currentEnemies = new List<Enemy>();

    private FocusType _enemySpawnFocus = FocusType.Carrier;

    public int EnemyCount { get { return _currentEnemies.Count; } }

    private int _currentWave = 0;

    public int Wave { get { return _currentWave; } }

    private int _fighterGrowthRate = 3;
    private int _bomberGrowthRate = 1;

    private Vector2 _wavePauseTimer = new Vector2(0, 30);
    public float WavePauseTime { get { return 1 - (_wavePauseTimer.x / _wavePauseTimer.y); } }

    private ObjectPooler _objectPooler;

    private bool _firstRound = true;

    private void Start()
    {
        Player.Instance.ResetGameScore();
        _objectPooler = ObjectPooler.Instance;
        _carrier.EnemyManager = this;
        _carrier.PlayerShip = _playerShip;
        StartCoroutine(Wave1(10, 3, 1, 0));
    }

    private IEnumerator Wave1(int enemyCount, int spawnRate, int fighterRate, int bomberRate)
    {
        _carrier.EnableShop();
        
        _wavePauseTimer.x = 0;
        while(_wavePauseTimer.x < _wavePauseTimer.y)
        {
            _wavePauseTimer.x += Time.deltaTime;
            yield return null;
        }

        _currentWave++;
        _carrier.DisableShop();

        Player.Instance.CurrentWave = _currentWave;
        PresenceManager.UpdatePresence(detail: "Wave " + _currentWave, state: "Carrier escort");

        int enemySpawnCount = 0;
        _wavePauseTimer.x = 0;

        while (enemySpawnCount < enemyCount)
        {
            int spawnCountTrimmed = Mathf.Clamp(enemyCount - _currentEnemies.Count, 0, spawnRate);
            yield return new WaitForSeconds(3f);

            for (int i = 0; i < spawnCountTrimmed; i++)
            {
                int randomRoll = UnityEngine.Random.Range(1, fighterRate + (bomberRate + 1));

                if (randomRoll >= 1 && randomRoll <= fighterRate)
                    SpawnEnemy("EnemyFighter");
                else if (randomRoll > fighterRate && randomRoll <= fighterRate + bomberRate)
                    SpawnEnemy("EnemyBomber");
                else
                    Debug.LogWarning($"Unknown enemy attempted to spawn.");

                enemySpawnCount++;
            }
        }

        yield return new WaitUntil(() => _currentEnemies.Count <= 0);
        _wavePauseTimer.y = 30;
        StartNewWave(enemyCount, spawnRate, fighterRate, bomberRate);
    }

    private void StartNewWave(int enemyCount, int spawnRate, int fighterRate, int bomberRate)
    {
        _firstRound = false;
        StartCoroutine(Wave1((int)(enemyCount + (5 * (_currentWave + 1))), spawnRate + (int)(_currentWave / 3f), fighterRate + _fighterGrowthRate, bomberRate + _bomberGrowthRate));
    }

    private void SpawnEnemy(string enemyTag)
    {
        int randomIndex = UnityEngine.Random.Range(0, _enemySpawnPoints.Count);
        EnemySpawn enemySpawn = _enemySpawnPoints[randomIndex];
        
        while(!enemySpawn.CanSpawn)
        {
            randomIndex = UnityEngine.Random.Range(0, _enemySpawnPoints.Count);
            enemySpawn = _enemySpawnPoints[randomIndex];
        }

        GameObject enemyFromPool = _objectPooler.SpawnFromPool(enemyTag, enemySpawn.transform.position, enemySpawn.transform.rotation);
        Enemy spawnedEnemy = enemyFromPool.GetComponent<Enemy>();

        spawnedEnemy.PlayerShip = _playerShip;
        spawnedEnemy.Carrier = _carrier;
        spawnedEnemy.EnemyManager = this;
        spawnedEnemy.ResetEnemy();

        switch (_enemySpawnFocus)
        {
            case FocusType.Player:
                spawnedEnemy.FocusPlayer();
                break;
            case FocusType.Carrier:
                spawnedEnemy.FocusCarrier();
                break;
        }

        _currentEnemies.Add(spawnedEnemy);
    }

    /*
    private void SpawnEnemy(Enemy enemyPrefab)
    {
        if(enemyPrefab == null)
        {
            Debug.LogWarning("Attempted to spawn an Null enemy, spawning a simple fighter instead...");
            enemyPrefab = _enemyFighter;
        }

        int randomPoint = UnityEngine.Random.Range(0, _enemySpawnPoints.Count);

        Enemy newEnemy = Instantiate(enemyPrefab, _enemySpawnPoints[randomPoint].position, _enemySpawnPoints[randomPoint].rotation);
        newEnemy.Player = _player;
        newEnemy.Carrier = _carrier;
        newEnemy.EnemyManager = this;

        switch (_enemySpawnFocus)
        {
            case FocusType.Player:
                newEnemy.FocusPlayer();
                break;
            case FocusType.Carrier:
                newEnemy.FocusCarrier();
                break;
        }

        _currentEnemies.Add(newEnemy);
    }*/

    public void EnemyDestroyed(Enemy enemy)
    {
        _currentEnemies.Remove(enemy);
    }

    public void ForceFocusPlayer()
    {
        for (int i = 0; i < _currentEnemies.Count; i++)
            _currentEnemies[i].FocusPlayer();

        _enemySpawnFocus = FocusType.Player;
    }
}
