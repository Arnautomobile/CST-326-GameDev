using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager Instance { get; private set; }
    public float WinningHeight { get => _winningHeight; }

    [SerializeField] private int _startingShips;
    [SerializeField] private float _movementsNumber;
    [SerializeField] private float _minMovementPeriod;
    [SerializeField] private float _maxMovementPeriod;
    [SerializeField] private float _heightDecrease;
    [SerializeField] private float _winningHeight;
    [SerializeField] private float _edgeLeft;
    [SerializeField] private float _edgeRight;
    
    private Vector3 _direction = Vector3.right;
    private bool _endReached = false;
    private bool _goDown = false;
    private float _timer = 0;
    private float _movementPeriod;
    private float _moveDistance;
    private int _shipsLeft;


    void Awake()
    {
        if (Instance == null) {
            Instance = this;
        }
        else {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        _movementPeriod = _maxMovementPeriod;
        _moveDistance = (_edgeRight - _edgeLeft) / _movementsNumber;
        _shipsLeft = _startingShips;
    }

    void Update()
    {
        _timer += Time.deltaTime;
        if (_timer >= _movementPeriod) {
            _timer = 0;

            if (_goDown || _endReached) {
                transform.position += Vector3.down * _heightDecrease;
                _direction = new Vector3(-_direction.x, 0, 0);
                _goDown = false;
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
        _movementPeriod = _minMovementPeriod + (_maxMovementPeriod - _minMovementPeriod) * _shipsLeft/_startingShips;
    }

    public void TryEndReached(float pos)
    {
        if (pos > _winningHeight || _endReached) return;

        _endReached = true;
        GameManager.Instance.EndGame(false);
    }
}
