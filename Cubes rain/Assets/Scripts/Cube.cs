using System.Collections;
using UnityEngine;
using UnityEngine.Pool;
using Random = System.Random;

public class Cube : MonoBehaviour
{
    [SerializeField] private int _minLifetime = 2;
    [SerializeField] private int _maxLifetime = 6;

    private float _lifetime;
    private bool _isChangedColor = false;
    private bool _isTimerStarted = false;
    private ObjectPool<Cube> _pool;

    private void Start()
    {
        Random random = new Random();
        _lifetime = random.Next(_minLifetime, _maxLifetime);
    }
    
    public bool GetIsChangedColor() => _isChangedColor;

    public bool ChangeColor() => _isChangedColor = true;

    public void SetPool(ObjectPool<Cube> pool)
    {
        _pool = pool;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!_isTimerStarted && collision.gameObject.GetComponent<Platform>() != null)
        {
            _isTimerStarted = true;
            StartCoroutine(ReturnToPoolAfterDelay());
        }
    }

    private IEnumerator ReturnToPoolAfterDelay()
    {
        yield return new WaitForSeconds(_lifetime);
        
        if (_pool != null)
        {
            _pool.Release(this);
            _isChangedColor = false;
            _isTimerStarted = false;
        }
    }
}