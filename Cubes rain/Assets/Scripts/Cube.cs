using System.Collections;
using UnityEngine;
using Random = System.Random;

public class Cube : MonoBehaviour
{
    [SerializeField] private int _minLifetime = 2;
    [SerializeField] private int _maxLifetime = 6;

    private float _lifetime;
    private bool _isChangedColor = false;
    private bool _isTimerStarted = false;

    private void Start()
    {
        Random random = new Random();
        _lifetime = random.Next(_minLifetime, _maxLifetime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!_isTimerStarted && collision.gameObject.GetComponent<Platform>() != null)
        {
            _isTimerStarted = true;
            StartCoroutine(DestroyAfterDelay());
        }
    }

    private IEnumerator DestroyAfterDelay()
    {
        yield return new WaitForSeconds(_lifetime);
        Destroy(gameObject);
    }

    public bool GetIsChangedColor() => _isChangedColor;

    public bool ChangeColor() => _isChangedColor = true;
}