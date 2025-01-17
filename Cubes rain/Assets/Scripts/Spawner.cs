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
            actionOnGet: (obj) => ActionOnGet(obj),
            actionOnRelease: (obj) => obj.SetActive(false),
            actionOnDestroy: (obj) => Destroy(obj),
            collectionCheck: true,
            defaultCapacity: _poolCapacity,
            maxSize: _poolMaxSize);
    }

    private void ActionOnGet(GameObject obj)
    {
        float spawnX = _platformPosition.x + Random.Range(
            -_platformSize.x/2 + _spawnMargin, 
            _platformSize.x/2 - _spawnMargin
        );
        
        float spawnHeight = _platformPosition.y + _heightAbovePlatform;
        
        float spawnZ = _platformPosition.z + Random.Range(
            -_platformSize.z/2 + _spawnMargin, 
            _platformSize.z/2 - _spawnMargin
        );

        Vector3 spawnPosition = new Vector3(spawnX, spawnHeight, spawnZ);
        
        obj.transform.position = spawnPosition;
        obj.GetComponent<Rigidbody>().velocity = Vector3.zero;
        obj.SetActive(true);
    }

    private void Start()
    {
        InvokeRepeating(nameof(GetObject), 0.0f, _spawnInterval);
    }

    private void GetObject()
    {
        _pool.Get();
    }
}