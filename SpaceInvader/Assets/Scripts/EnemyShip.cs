using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class EnemyShip : MonoBehaviour
{
    [SerializeField] private GameObject _missile;
    [SerializeField] private AudioClip _firingSound;
    [SerializeField] private AudioClip _explosionSound;
    [SerializeField] private int _scorePoints;
    [SerializeField] private float _minFireTime;
    [SerializeField] private float _maxFireTime;

    private AudioSource _audioSource;
    private System.Random _rand;
    private Animator _animator;
    private float _firingTime;
    private float _timer = 0;
    private string _currentState;

    private const string IDLE = "EnemyIdle";
    private const string SHOOT = "EnemyMove";
    private const string MOVE = "EnemyMove";


    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        _animator = GetComponent<Animator>();
        _rand = new System.Random();

        _firingTime = (float)_rand.NextDouble() * (_maxFireTime - _minFireTime) + _minFireTime;
        _currentState = IDLE;
    }

    void Update()
    {
        if (GameManager.Instance.Paused) {
            ChangeAnim(IDLE);
            return;
        };

        _timer += Time.deltaTime;
        if (_timer >= _firingTime)
        {
            Fire();

            _firingTime = _firingTime = (float)_rand.NextDouble() * (_maxFireTime - _minFireTime) + _minFireTime;
            _timer = 0;
        }
        if (_timer > 0.5) {
            ChangeAnim(MOVE);
        }
        
        EnemyManager.Instance.TryEndReached(transform.position.y);
    }


    private void Fire()
    {
        GameObject missile = Instantiate(_missile, transform.position + Vector3.down, Quaternion.identity);
        missile.GetComponent<MissileScript>().Setup(Vector2.down, true);
        _audioSource.PlayOneShot(_firingSound);
        ChangeAnim(SHOOT);
    }

    private void ChangeAnim(string newState)
    {
        if (newState != _currentState) {
            _currentState = newState;
            _animator.Play(newState);
        }
    }


    public void Destroyed()
    {
        GameManager.Instance.AddScore(_scorePoints);
        EnemyManager.Instance.ShipDestroyed();
        _audioSource.PlayOneShot(_explosionSound);

        GetComponent<SpriteRenderer>().enabled = false;
        GetComponent<BoxCollider2D>().enabled = false;
        Destroy(gameObject, 1);
        enabled = false;
    }
}
