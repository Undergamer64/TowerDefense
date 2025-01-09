using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class WaveManager : MonoBehaviour
{
    [SerializeField] private Shop _shop;
    
    [SerializeField] private int _normalEnemyCost = 1;
    [SerializeField] private int _bigEnemyCost = 2;
    [SerializeField] private int _groupeEnemyCost = 2;
    
    [SerializeField] private EnemiesSpawner _spawner;
    
    [SerializeField] private int _maxEnemySlots = 5;
    private int _enemySlots;
    
    [SerializeField] private float _maxRoundCooldown = 2;
    private float _roundCooldown;
    private bool _isRoundFinished;
    
    private bool _spawning = true;
    
    [FormerlySerializedAs("_maxspawnCooldown")] [SerializeField] private float _maxSpawnCooldown = 0.5f;
    private float _spawnCooldown;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _roundCooldown = _maxRoundCooldown;
        _spawnCooldown = _maxSpawnCooldown;
        _enemySlots = _maxEnemySlots;
        _spawner.OnRoundEnd.AddListener(RoundFinished);
    }

    // Update is called once per frame
    void Update()
    {
        if (_isRoundFinished)
        {
            _roundCooldown -= Time.deltaTime;
            if (!(_roundCooldown <= 0)) return;
            _enemySlots = _maxEnemySlots;
            _isRoundFinished = false;
            _spawning = true;
            _roundCooldown = _maxRoundCooldown;
            _shop.OpenShop();
        }
        else if (_spawning)
        {
            _spawnCooldown -= Time.deltaTime;
            if (!(_spawnCooldown <= 0)) return;
            _spawnCooldown = _maxSpawnCooldown;
            TrySpawnEnemy();
        }
    }

    private void TrySpawnEnemy()
    {
        List<EnemyType> enemyTypes = Enum.GetValues(typeof(EnemyType)).Cast<EnemyType>().ToList();
        
        for (int i = 0; i < enemyTypes.Count; i++)
        {
            int typeIndex = Random.Range(0, enemyTypes.Count);

            int slotUsed = _enemySlots + 1;
            
            switch (enemyTypes[typeIndex])
            {
                case EnemyType.normal:
                    slotUsed = _normalEnemyCost;
                    break;
                case EnemyType.big:
                    slotUsed = _bigEnemyCost;
                    break;
                case EnemyType.groupe:
                    slotUsed = _groupeEnemyCost;
                    break;
                default:
                    break;
            }

            if (_enemySlots - slotUsed < 0)
            {
                enemyTypes.RemoveAt(typeIndex);
                continue;
            }
            
            _enemySlots -= slotUsed;
            _spawner.SpawnEnemy(enemyTypes[typeIndex]);
            _spawnCooldown = _maxSpawnCooldown;
            return;
        }
        _roundCooldown = _maxRoundCooldown;
        _maxEnemySlots += 5;
        _maxSpawnCooldown *= 0.95f;
        _spawning = false;
    }

    private void RoundFinished()
    {
        if (_spawning) return;
        _isRoundFinished = true;
    }
}
