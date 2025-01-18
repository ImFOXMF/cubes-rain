using UnityEngine;
using UnityEngine.Pool;
using System.Collections;
using Random = UnityEngine.Random;

public class Spawner : MonoBehaviour
{
    [SerializeField] private Cube _cubePrefab;
    [SerializeField] private float _spawnInterval = 1f;
    [SerializeField] private int _poolCapacity = 5;
    [SerializeField] private int _poolMaxSize = 5;

    [SerializeField] private Transform _platform;
    [SerializeField] private float _heightAbovePlatform = 20f;
    [SerializeField] private float _spawnMargin = 0.5f;

    private ObjectPool<Cube> _pool;
    private Vector3 _platformSize;
    private Vector3 _platformPosition;
    private int _deviderForPlatform = 2;

    private void Awake()
    {
        if (_platform != null)
        {
            Renderer platformRenderer = _platform.GetComponent<Renderer>();

            if (platformRenderer != null)
            {
                _platformSize = platformRenderer.bounds.size;
                _platformPosition = _platform.position;
            }
        }

        CreatePool();
    }

    private void Start()
    {
        StartCoroutine(SpawnRoutine());
    }

    private void GetAction(Cube cube)
    {
        float spawnX = FindSpawnPoint(_platformPosition.x, _platformSize.x);
        float spawnHeight = _platformPosition.y + _heightAbovePlatform;
        float spawnZ = FindSpawnPoint(_platformPosition.z, _platformSize.z);

        Vector3 spawnPosition = new Vector3(spawnX, spawnHeight, spawnZ);

        cube.transform.position = spawnPosition;
        cube.GetComponent<Rigidbody>().velocity = Vector3.zero;
        cube.gameObject.SetActive(true);
    }

    private float FindSpawnPoint(float _platformPosition, float _platformSize)
    {
        float spawnPoint = _platformPosition + Random.Range(
            -_platformSize / _deviderForPlatform + _spawnMargin,
            _platformSize / _deviderForPlatform - _spawnMargin);

        return spawnPoint;
    }

    private void CreatePool()
    {
        _pool = new ObjectPool<Cube>(
            createFunc: () =>
            {
                var cube = Instantiate(_cubePrefab);
                cube.SetPool(_pool);
                return cube;
            },
            actionOnGet: (cube) => GetAction(cube),
            actionOnRelease: (cube) => cube.gameObject.SetActive(false),
            actionOnDestroy: (cube) => Destroy(cube.gameObject),
            collectionCheck: true,
            defaultCapacity: _poolCapacity,
            maxSize: _poolMaxSize);
    }
    
    private IEnumerator SpawnRoutine()
    {
        while (true)
        {
            _pool.Get();
            yield return new WaitForSeconds(_spawnInterval);
        }
    }
}