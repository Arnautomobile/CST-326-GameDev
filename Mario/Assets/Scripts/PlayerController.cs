using System;
using System.Collections;
using System.Collections.Generic;
using TreeEditor;
using UnityEngine;


public class PlayerController : MonoBehaviour
{
    [Header("Checking Collisions")]
    [SerializeField] private Vector3 _groundCheckOffset;
    [SerializeField] private Vector3 _groundCheckDimensions;
    [SerializeField] private Vector3 _blockCheckOffset;
    [SerializeField] private Vector3 _blockCheckDimensions;
    [SerializeField] private LayerMask _blocksLayer;

    [Header("Mouvement Parameters")]
    [SerializeField] private float _maxSpeed = 8f;
    [SerializeField] private float _acceleration = 30f;
    [SerializeField] private float _airAcceleration = 5f;
    [SerializeField] private float _jumpHoldTime = 0.5f;
    [SerializeField] private float _jumpForce = 15f;
    [SerializeField] private float _gravity = 40f;
    [SerializeField] private float _maxFallSpeed = 15f;
    [SerializeField] private float _powerLandingSpeed = 30f;

    private Rigidbody _rigidbody;
    private float _xSpeed = 0;
    private float _ySpeed = 0;
    private float _jumpHoldTimer = 0;


    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        _rigidbody.velocity = new Vector3(_xSpeed, _ySpeed, 0);
    }


    void Update()
    {
        float xDirection = Input.GetAxis("Horizontal");

        if (IsGrounded()) {
            _xSpeed = Mathf.Lerp(_xSpeed, _maxSpeed * xDirection, _acceleration * Time.deltaTime);

            if (_ySpeed < 0) {
                if (_ySpeed == -_powerLandingSpeed) {
                    CheckHitBlock(-1);
                }
                _ySpeed = 0;
            }
            if (Input.GetButtonDown("Jump")) {
                _jumpHoldTimer = 0;
                _ySpeed = _jumpForce;
            }
        }
        else {
            _xSpeed = Mathf.Lerp(_xSpeed, _maxSpeed * xDirection, _airAcceleration * Time.deltaTime);

            if (_jumpHoldTimer < _jumpHoldTime) {
                _jumpHoldTimer += Time.deltaTime;

                if (Input.GetButton("Jump")) {
                    _ySpeed += _jumpForce * Time.deltaTime;
                }
                else if (Input.GetButtonUp("Jump")) {
                    _jumpHoldTimer = _jumpHoldTime;
                }
            }

            if (-_ySpeed < _maxFallSpeed) {
                _ySpeed -= _gravity * Time.deltaTime;
            }
            if (_ySpeed > 0) {
                CheckHitBlock(1);
            }
            else if (Input.GetButtonDown("Land")) {
                _ySpeed = -_powerLandingSpeed;
            }
        }

        if (_xSpeed > 0) {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else if (_xSpeed < 0) {
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }
    }
    

    private bool IsGrounded()
    {
        return Physics.OverlapBox(_rigidbody.position + _groundCheckOffset, _groundCheckDimensions * 0.5f,
               Quaternion.identity, _blocksLayer).Length > 0;
    }

    private void CheckHitBlock(int direction)
    {
        Collider[] hits = Physics.OverlapBox(_rigidbody.position + _blockCheckOffset * direction, _blockCheckDimensions * 0.5f, Quaternion.identity, _blocksLayer);
        if (hits.Length == 0) {
            return;
        }
        foreach (Collider hit in hits) {
            GameManager.Instance.BlockHit(hit.gameObject);
        }
        _ySpeed = 0;
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position + _groundCheckOffset, _groundCheckDimensions);
        Gizmos.DrawWireCube(transform.position + _blockCheckOffset, _blockCheckDimensions);
    }
}
