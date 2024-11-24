using UnityEngine;
using Random = UnityEngine.Random;

public class PoolSpawner : MonoBehaviour
{
    [SerializeField, Min(0.0f)] private float _spawnRate;

    [SerializeField] private GameObject _cubePrefab;
    [SerializeField] private GameObject _spherePrefab;
    /*
    private ComponentPool<Cube> _cubePool;
    private ComponentPool<Sphere> _spherePool;
    private float _timer;

    private void Awake()
    {
        _cubePool = new ComponentPool<Cube>(_cubePrefab, 500, 300);
        _spherePool = new ComponentPool<Sphere>(_spherePrefab, 500, 300);
        _timer = 0.0f;
    }

    public Cube CreateNewCube()
    {
        return Instantiate(_cubePrefab, gameObject.transform).GetComponent<Cube>();
    }

    public Sphere CreateNewSphere()
    {
        return Instantiate(_spherePrefab, gameObject.transform).GetComponent<Sphere>();
    }

    private void Update()
    {
        _timer += Time.deltaTime;
        if (_timer >= _spawnRate)
        {
            _timer = 0.0f;
            Sphere sphere = _spherePool.Get();
            sphere.transform.SetPositionAndRotation(transform.position, Random.rotation);

            Cube cube = _cubePool.Get();
            cube.transform.SetPositionAndRotation(transform.position, Random.rotation);
        }
    }
    */
}