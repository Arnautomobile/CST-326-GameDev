using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager Instance { get; private set; }

    [SerializeField] private int _startingShips;
    [SerializeField] private float _movementsNumber;
    [SerializeField] private float _movementPeriod;
    [SerializeField] private float _edgeLeft;
    [SerializeField] private float _edgeRight;
    [SerializeField] private float _heightDecrease;
    
    private Vector3 _direction = Vector3.right;
    private bool _goDown = false;
    private float _timer = 0;
    private float _moveDistance;
    public int _shipsLeft;


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
        _moveDistance = (_edgeRight - _edgeLeft) / _movementsNumber;
        _shipsLeft = _startingShips;
    }

    void Update()
    {
        _timer += Time.deltaTime;
        if (_timer >= _movementPeriod) {
            _timer = 0;

            if (_goDown) {
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


    private void ShipDestroyed() {
        _shipsLeft--;
    }
}
