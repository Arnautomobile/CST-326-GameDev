using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private GameObject _missile;
    [SerializeField] private AudioClip _firingSound;
    [SerializeField] private AudioClip _explosionSound;

    
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
    private AudioSource _audioSource;
    private Animator _animator;
    private float _direction = 0;
    private float _firingTime = 0;
    private float _fireTimer = 0;
    private string _currentState;

    private const string IDLE = "PlayerIdle";
    private const string SHOOT = "PlayerShoot";
    private const string LEFT = "PlayerLeft";
    private const string RIGHT = "PlayerRight";


    void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _audioSource = GetComponent<AudioSource>();
        _animator = GetComponent<Animator>();

        transform.position = new Vector2(0,_height);
        _currentState = IDLE;
    }


    void Update()
    {
        if (GameManager.Instance.Paused) {
            ChangeAnim(IDLE);
            return;
        }

        _direction = Input.GetAxis("Horizontal");
        _fireTimer += Time.deltaTime;

        if (_fireTimer > 0.5)
        {
            if (_direction < 0) {
                ChangeAnim(LEFT);
            }
            else if (_direction > 0) {
                ChangeAnim(RIGHT);
            }
            else {
                ChangeAnim(IDLE);
            }
        }

        if (Input.GetKeyDown(KeyCode.A)) {
            _isAuto = !_isAuto;
        }

        if (_isAuto)
        {
            if ((Input.GetButtonDown("Fire") || Input.GetButton("Fire")) && _fireTimer > _fireRate) {
                Fire(0);
                _fireTimer = 0;
            }
        }
        else
        {
            if (Input.GetButtonDown("Fire")) {
                _firingTime = 0;
            }
            else if (Input.GetButton("Fire")) {
                _firingTime += Time.deltaTime;
            }
            else if (Input.GetButtonUp("Fire") && _fireTimer > _chargeTime/_resetSpeed) {
                if (_firingTime > _chargeTime)
                    _firingTime = _chargeTime;

                _fireTimer = 0;
                Fire(_firingTime/_chargeTime);
            }
        }
    }
    
    void FixedUpdate()
    {
        if (GameManager.Instance.Paused || _direction == 0) return;
        
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


    public void Hit()
    {
        _audioSource.PlayOneShot(_explosionSound);
    }
}
