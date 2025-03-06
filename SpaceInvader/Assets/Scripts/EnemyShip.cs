using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class EnemyShip : MonoBehaviour
{
    [SerializeField] private GameObject _missile;
    [SerializeField] private int _scorePoints;
    [SerializeField] private float _minFireTime;
    [SerializeField] private float _maxFireTime;

    private System.Random _rand;
    private float _firingTime;
    private float _timer;


    void Start()
    {
        _rand = new System.Random();
        _firingTime = (float)_rand.NextDouble() * (_maxFireTime - _minFireTime) + _minFireTime;
        _timer = 0;
    }

    void Update()
    {
        _timer += Time.deltaTime;
        if (_timer >= _firingTime)
        {
            GameObject missile = Instantiate(_missile, transform.position + Vector3.down, Quaternion.identity);
            missile.GetComponent<MissileScript>().Setup(Vector2.down, true);

            _firingTime = _firingTime = (float)_rand.NextDouble() * (_maxFireTime - _minFireTime) + _minFireTime;
            _timer = 0;
        }

        EnemyManager.Instance.TryEndReached(transform.position.y);
    }


    public void Destroyed()
    {
        GameManager.Instance.AddScore(_scorePoints);
        EnemyManager.Instance.ShipDestroyed();
        Destroy(gameObject);
    }
}
