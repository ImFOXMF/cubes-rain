using UnityEngine;
using UnityEngine.Pool;
using Random = UnityEngine.Random;

public class Spawner : MonoBehaviour
{
    [SerializeField] private GameObject _cubePrefab;
    [SerializeField] private float _spawnInterval = 1f;
    [SerializeField] private int _poolCapacity = 5;
    [SerializeField] private int _poolMaxSize = 5;

    [SerializeField] private Transform _platform;
    [SerializeField] private float _heightAbovePlatform = 20f;
    [SerializeField] private float _spawnMargin = 0.5f;

    private ObjectPool<GameObject> _pool;
    private Vector3 _platformSize;
    private Vector3 _platformPosition;
    private int _deviderForPlatform = 2;

    private void Awake()
    {
        if (_platform != null)
        {
            _platformSize = _platform.GetComponent<Renderer>().bounds.size;
            Debug.Log(_platformSize);
            _platformPosition = _platform.position;
        }

        _pool = new ObjectPool<GameObject>(
            createFunc: () => Instantiate(_cubePrefab),
            actionOnGet: (obj) => GetAction(obj),
            actionOnRelease: (obj) => obj.SetActive(false),
            actionOnDestroy: (obj) => Destroy(obj),
            collectionCheck: true,
            defaultCapacity: _poolCapacity,
            maxSize: _poolMaxSize);
    }
    
    private void Start()
    {
        InvokeRepeating(nameof(GetObject), 0.0f, _spawnInterval);
    }

    private void GetAction(GameObject obj)
    {
        float spawnX = FindSpawnPoint(_platformPosition.x, _platformSize.x);

        float spawnHeight = _platformPosition.y + _heightAbovePlatform;
        
        float spawnZ = FindSpawnPoint(_platformPosition.z, _platformSize.z);

        Vector3 spawnPosition = new Vector3(spawnX, spawnHeight, spawnZ);

        obj.transform.position = spawnPosition;
        obj.GetComponent<Rigidbody>().velocity = Vector3.zero;
        obj.SetActive(true);
    }

    private void GetObject()
    {
        _pool.Get();
    }

    private float FindSpawnPoint(float _platformPosition, float _platformSize)
    {
        float spawnPoint = _platformPosition + Random.Range(
            -_platformSize / _deviderForPlatform + _spawnMargin,
            _platformSize / _deviderForPlatform - _spawnMargin);
        
        return spawnPoint;
    }
}