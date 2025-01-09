using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum EnemyType
{
    normal,
    big,
    groupe
}

public class EnemiesSpawner : MonoBehaviour
{
    [SerializeField] private Transform _centerTower;

    [SerializeField] private GameObject _enemyPrefab;

    public UnityEvent OnRoundEnd = new UnityEvent();
    
    private CircleCollider2D _circleCollider;

    private ComponentPool<Enemy> _enemyPool;
    
    private List<Enemy> _enemiesAlive = new();
    
    private void Start()
    {
        _circleCollider = GetComponent<CircleCollider2D>();
        _enemyPool = new ComponentPool<Enemy>(_enemyPrefab, 50, 10);
    }

    private void SpawnNormalEnemy()
    {
        //Generate 1 normal enemy
        
        Enemy enemy = GenerateEnemy();
        enemy.transform.position = Random.insideUnitCircle.normalized * _circleCollider.radius;
        enemy.transform.position += _centerTower.position;
        enemy._Type = EnemyType.normal;
    }
    
    private void SpawnBigEnemy()
    {
        //Generate 1 enemy with enhanced health but less speed
        
        Enemy enemy = GenerateEnemy();
        enemy._Life *= 3f;
        enemy._Speed *= 0.7f;
        enemy.transform.position = Random.insideUnitCircle.normalized * _circleCollider.radius;
        enemy.transform.localScale *= 1.5f;
        enemy.transform.position += _centerTower.position;
        enemy._Type = EnemyType.big;
    }
    
    private void SpawnGroupeEnemy()
    {
        //Generate 5 enemies with reduced health but more speed

        for (int i = 0; i < 3; i++)
        {
            Enemy enemy = GenerateEnemy();
            enemy._Life *= 0.5f;
            enemy._Speed *= 1.5f;
            enemy.transform.position = Random.insideUnitCircle.normalized * _circleCollider.radius;
            enemy.transform.localScale *= 0.5f;
            enemy.transform.position += _centerTower.position;
            enemy._Type = EnemyType.groupe;
        }
    }
    
    public void SpawnEnemy(EnemyType type)
    {
        switch (type)
        {
            case EnemyType.normal:
                SpawnNormalEnemy();
                break;
            case EnemyType.big:
                SpawnBigEnemy();
                break;
            case EnemyType.groupe:
                SpawnGroupeEnemy();
                break;
        }
    }

    private Enemy GenerateEnemy()
    {
        Enemy enemy = _enemyPool.Get();
        enemy._OnDestroy.RemoveAllListeners();
        enemy._OnDestroy.AddListener(RemoveEnemy);
        _enemiesAlive.Add(enemy);
        enemy.transform.localScale = _enemyPrefab.transform.localScale;
        enemy._Life = _enemyPrefab.GetComponent<Enemy>()._Life;
        enemy._Speed = _enemyPrefab.GetComponent<Enemy>()._Speed;
        enemy._Type = EnemyType.normal;
        return enemy;
    }
    
    private void Update()
    {
        foreach (Enemy enemy in _enemiesAlive)
        {
            enemy.transform.Translate((_centerTower.position - enemy.transform.position).normalized * (enemy._Speed * Time.deltaTime));
        }
    }

    private void RemoveEnemy(Enemy enemy)
    {
        switch (enemy._Type)
        {
            case EnemyType.big:
                _centerTower.GetComponent<PlayerData>()._Money += 3;
                break;
            case EnemyType.normal:
                _centerTower.GetComponent<PlayerData>()._Money += 2;
                break;
            case EnemyType.groupe:
                _centerTower.GetComponent<PlayerData>()._Money += 1;
                break;
            default:
                break;
        }
        
        _enemiesAlive.Remove(enemy);
        if (_enemiesAlive.Count == 0) OnRoundEnd.Invoke();
    }
    
    public void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, GetComponent<CircleCollider2D>().radius);
    }
}
