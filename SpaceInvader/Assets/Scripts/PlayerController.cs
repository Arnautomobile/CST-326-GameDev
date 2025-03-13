using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private GameObject _missile;

    [Header("Movement settings")]
    [SerializeField] private float _speed;
    [SerializeField] private float _height;
    [SerializeField] private float _edgeLeft;
    [SerializeField] private float _edgeRight;

    [Header("Shooting settings")]
    [SerializeField] private bool _isAuto;
    [SerializeField] private float _fireRate;
    [SerializeField] private float _chargeTime;
    [SerializeField] private float _overheatTime;
    [SerializeField] private float _resetSpeed;

    private Rigidbody2D _rigidbody;
    private float _direction = 0;
    private float _firingTime = 0;
    private float _timer = 0;


    void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        transform.position = new Vector2(0,_height);
    }


    void Update()
    {
        if (GameManager.Instance.Paused) return;

        _direction = Input.GetAxis("Horizontal");
        _timer += Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.LeftAlt)) {
            _isAuto = !_isAuto;
        }

        if (_isAuto) {
            if ((Input.GetButtonDown("Fire") || Input.GetButton("Fire")) && _timer > _fireRate) {
                Fire(0);
                _timer = 0;
            }
        }
        else {
            if (Input.GetButtonDown("Fire")) {
                _firingTime = 0;
            }
            else if (Input.GetButton("Fire")) {
                _firingTime += Time.deltaTime;
            }
            else if (Input.GetButtonUp("Fire") && _timer > _chargeTime/_resetSpeed) {
                if (_firingTime > _chargeTime)
                    _firingTime = _chargeTime;

                _timer = 0;
                Fire(_firingTime/_chargeTime);
            }
        }
    }
    

    void FixedUpdate()
    {
        if (GameManager.Instance.Paused) return;
        
        float x = _rigidbody.position.x + _direction * _speed;

        if (x < _edgeLeft) {
            _rigidbody.MovePosition(new Vector2(_edgeLeft, _height));
        }
        else if (x > _edgeRight) {
            _rigidbody.MovePosition(new Vector2(_edgeRight, _height));
        }
        else {
            _rigidbody.MovePosition(new Vector2(x, _height));
        }
    }


    private void Fire(float chargeFactor)
    {
        GameObject missile = Instantiate(_missile, transform.position + Vector3.up, Quaternion.identity);
        missile.GetComponent<ChargeMissile>().Setup(Vector2.up, false, chargeFactor);
    }
}
