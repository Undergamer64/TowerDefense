using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

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

    private void SpawnEnemy()
    {
        Enemy enemy = _enemyPool.Get();
        enemy.OnDestroy.AddListener(RemoveEnemy);
        enemy.transform.position = Random.insideUnitCircle.normalized * _circleCollider.radius;
        _enemiesAlive.Add(enemy);
    }

    private void Update()
    {
        foreach (Enemy enemy in _enemiesAlive)
        {
            enemy.transform.Translate((_centerTower.position - enemy.transform.position).normalized * (enemy.Speed * Time.deltaTime));
        }

        if (_enemiesAlive.Count <= 7)
        {
            SpawnEnemy();
        }
    }

    private void RemoveEnemy(Enemy enemy)
    {
        _enemiesAlive.Remove(enemy);
    }
    
    public void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, GetComponent<CircleCollider2D>().radius);
    }
}
