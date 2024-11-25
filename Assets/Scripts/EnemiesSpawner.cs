using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class EnemiesSpawner : MonoBehaviour
{
    [SerializeField]
    private Transform _centerTower;

    [SerializeField]
    private GameObject _enemyPrefab;

    private CircleCollider2D _circleCollider;

    private ComponentPool<Enemy> _enemyPool;

    public List<Enemy> EnemiesAlive = new();

    private void Start()
    {
        _circleCollider = GetComponent<CircleCollider2D>();
        _enemyPool = new ComponentPool<Enemy>(_enemyPrefab, 50, 10);
    }

    private void SpawnEnemy()
    {
        Enemy enemy = _enemyPool.Get();
        enemy.transform.position = Random.insideUnitCircle.normalized * _circleCollider.radius;
        enemy.Spawner = this;
        EnemiesAlive.Add(enemy);
    }

    private void Update()
    {
        foreach (Enemy enemy in EnemiesAlive)
        {
            enemy.transform.Translate((_centerTower.position - enemy.transform.position).normalized * (enemy.speed * Time.deltaTime));
        }

        if (EnemiesAlive.Count <= 7)
        {
            SpawnEnemy();
        }
    }
    
    public void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, GetComponent<CircleCollider2D>().radius);
    }
}
