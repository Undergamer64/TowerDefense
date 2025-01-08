using System.Collections.Generic;
using UnityEngine;

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
    }
    
    private void SpawnBigEnemy()
    {
        //Generate 1 enemy with enhanced health but less speed
        
        Enemy enemy = GenerateEnemy();
        enemy.Life *= 2f;
        enemy.Speed *= 0.8f;
        enemy.transform.position = Random.insideUnitCircle.normalized * _circleCollider.radius;
        enemy.transform.localScale *= 1.5f;
        enemy.transform.position += _centerTower.position;
    }
    
    private void SpawnGroupeEnemy()
    {
        //Generate 5 enemies with reduced health but more speed

        for (int i = 0; i < 5; i++)
        {
            Enemy enemy = GenerateEnemy();
            enemy.Life *= 0.5f;
            enemy.Speed *= 1.2f;
            enemy.transform.position = Random.insideUnitCircle.normalized * _circleCollider.radius;
            enemy.transform.localScale *= 0.8f;
            enemy.transform.position += _centerTower.position;
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
        enemy.OnDestroy.RemoveAllListeners();
        enemy.OnDestroy.AddListener(RemoveEnemy);
        _enemiesAlive.Add(enemy);
        enemy.transform.localScale = _enemyPrefab.transform.localScale;
        enemy.Life = _enemyPrefab.GetComponent<Enemy>().Life;
        enemy.Speed = _enemyPrefab.GetComponent<Enemy>().Speed;
        return enemy;
    }
    
    private void Update()
    {
        foreach (Enemy enemy in _enemiesAlive)
        {
            enemy.transform.Translate((_centerTower.position - enemy.transform.position).normalized * (enemy.Speed * Time.deltaTime));
        }
    }

    private void RemoveEnemy(Enemy enemy)
    {
        _enemiesAlive.Remove(enemy);
        _centerTower.GetComponent<PlayerData>().Money++;
    }
    
    public void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, GetComponent<CircleCollider2D>().radius);
    }
}
