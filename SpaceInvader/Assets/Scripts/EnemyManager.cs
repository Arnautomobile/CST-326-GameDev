using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager Instance { get; private set; }
    public bool EndReached { get; set; } = false;

    [SerializeField] private int _startingShips;
    [SerializeField] private GameObject _bomberPrefab;
    [SerializeField] private float _minBomberSpawnTime;
    [SerializeField] private float _maxBomberSpawnTime;

    [Header("Movement settings")]
    [SerializeField] private float _movementsNumber;
    [SerializeField] private float _movementAcceleration;
    [SerializeField] private float _minMovePeriod;
    [SerializeField] private float _heightDecrease;
    [SerializeField] private float _winningHeight;
    [SerializeField] private float _edgeLeft;
    [SerializeField] private float _edgeRight;
    
    private Vector3 _direction = Vector3.right;
    private bool _goDown = false;
    private float _timer = 0;
    private float _movementPeriod;
    private float _moveDistance;
    private int _shipsLeft;


    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        _movementPeriod = GameManager.Instance.EnemyMovePeriod < _minMovePeriod ?
                          _minMovePeriod : GameManager.Instance.EnemyMovePeriod;
        _moveDistance = (_edgeRight - _edgeLeft) / _movementsNumber;
        _shipsLeft = _startingShips;
    }

    void Update()
    {
        if (GameManager.Instance.Paused) return;

        _timer += Time.deltaTime;
        if (_timer >= _movementPeriod) {
            _timer = 0;

            if (_goDown || EndReached) {
                transform.position += Vector3.down * _heightDecrease;
                _direction = new Vector3(-_direction.x, 0, 0);
                _goDown = false;

                if (transform.position.y < -13) {
                    Destroy(gameObject);
                }
            }
            else {
                transform.position += _direction * _moveDistance;
                if (transform.position.x < _edgeLeft || transform.position.x > _edgeRight) {
                    _goDown = true;
                }
            }
        }
    }


    public void ShipDestroyed()
    {
        _shipsLeft--;
        if (_shipsLeft <= 0) {
            GameManager.Instance.EndGame(true);
            Destroy(gameObject);
        }
        _movementPeriod -= _movementAcceleration;
        if (_movementPeriod < _minMovePeriod) {
            _movementPeriod = _minMovePeriod;
        }
    }

    public void TryEndReached(float pos)
    {
        if (pos > _winningHeight || EndReached) return;

        EndReached = true;
        GameManager.Instance.EndGame(false);
    }
}
