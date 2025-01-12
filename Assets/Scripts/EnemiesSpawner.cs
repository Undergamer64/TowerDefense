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
    [SerializeField] private List<Sprite> _enemySprites;
    
    [SerializeField] private Transform _centerTower;

    [SerializeField] private GameObject _enemyPrefab;

    public UnityEvent OnRoundEnd = new UnityEvent();
    
    private CircleCollider2D _circleCollider;

    private ComponentPool<Enemy> _enemyPool;
    
    private List<Enemy> _enemiesAlive = new();
    
    private void Start()
    {
        _circleCollider = GetComponent<CircleCollider2D>();
        _enemyPool = new ComponentPool<Enemy>(_enemyPrefab, 50, 10, transform);
    }

    private void SpawnNormalEnemy()
    {
        //Generate 1 normal enemy
        
        Enemy enemy = GenerateEnemy();
        enemy.transform.position = Random.insideUnitCircle.normalized * _circleCollider.radius;
        enemy.transform.position += _centerTower.position;
        enemy._Type = EnemyType.normal;
        ChangeEnemySprite(enemy, enemy.transform.position);
    }
    
    private void SpawnBigEnemy()
    {
        //Generate 1 enemy with enhanced health but less speed
        
        Enemy enemy = GenerateEnemy();
        enemy._Life *= 2.75f;
        enemy._Speed *= 0.7f;
        enemy.transform.position = Random.insideUnitCircle.normalized * _circleCollider.radius;
        enemy.transform.localScale *= 1.5f;
        enemy.transform.position += _centerTower.position;
        enemy._Type = EnemyType.big;
        ChangeEnemySprite(enemy, enemy.transform.position);
    }
    
    private void SpawnGroupeEnemy()
    {
        //Generate 3 enemies with reduced health but more speed

        Vector2 baseDirection = Random.insideUnitCircle.normalized; //Generate the general direction of spawn
        
        for (int i = 0; i < 3; i++)
        {
            Enemy enemy = GenerateEnemy();
            enemy._Life *= 0.5f;
            enemy._Speed *= 1.35f;

            float angle = Random.Range(0, 30f) * Mathf.Sign(Random.Range(-1,0)); //Generate random angle offset
            
            Vector2 newDirection = new Vector2(
                baseDirection.x * Mathf.Cos(angle * Mathf.Deg2Rad) - baseDirection.y * Mathf.Sin(angle * Mathf.Deg2Rad), 
                baseDirection.x * Mathf.Sin(angle * Mathf.Deg2Rad) + baseDirection.y * Mathf.Cos(angle * Mathf.Deg2Rad)); //Rotate the direction by angle
            
            enemy.transform.position = newDirection.normalized * _circleCollider.radius;
            enemy.transform.localScale *= 0.5f;
            enemy.transform.position += _centerTower.position;
            enemy._Type = EnemyType.groupe;
            ChangeEnemySprite(enemy, enemy.transform.position);
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

    private void ChangeEnemySprite(Enemy enemy, Vector3 pos)
    {
        float angle = ((Mathf.Atan2(pos.y, pos.x) - Mathf.PI/2) * Mathf.Rad2Deg) + 45f/2f;

        if (angle < 0)
        {
            angle += 360;
        }
        
        enemy.GetComponent<SpriteRenderer>().sprite = _enemySprites[Mathf.FloorToInt(angle/45f)];

        enemy.GetComponent<SpriteRenderer>().flipX = (angle - 45f/2f <= 180);
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
