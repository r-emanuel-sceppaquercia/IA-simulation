using UnityEngine;

public class Wander : ISteering
{
    private float _speed;
    private Vector3 _currentDirection;
    private float _wanderTimer = 0f;
    private float _timeToChangeDir = 2f;

    public Wander(float speed)
    {
        _speed = speed;
        GoToDirection();
    }

    public Vector3 GetDir()
    {
        _wanderTimer -= Time.deltaTime;

        if (_wanderTimer <= 0f)
        {
            GoToDirection();
            _wanderTimer = _timeToChangeDir;
        }

        return _currentDirection * _speed;
    }

    private void GoToDirection()
    {
        float randomX = Random.Range(-1f, 1f);
        float randomZ = Random.Range(-1f, 1f);
        _currentDirection = new Vector3(randomX, 0f, randomZ).normalized;
    }
}