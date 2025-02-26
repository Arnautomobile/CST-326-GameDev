using System;
using UnityEngine;


public class PlayerController : MonoBehaviour
{
    [SerializeField] private float _movementSpeed;
    [SerializeField] private float _jumpPower;
    [SerializeField] private GameObject _pivot;
    [SerializeField] private GameObject _camera;
    [SerializeField] private Vector3 _cameraOffset;
    [SerializeField] private float _cameraDistance;
    [SerializeField] private float _lookSpeed;
    [SerializeField] private float _verticalLookLimit;
    [SerializeField] private float _minCameraDistance;
    [SerializeField] private float _maxCameraDistance;

    private Vector3 _moveDirection = Vector3.zero;
    private Rigidbody _rigidbody;
    private float _verticalAngle = 0f;


    void Start() {
        _cameraOffset.Normalize();
        _rigidbody = GetComponentInChildren<Rigidbody>();
    }


    void Update()
    {
        _pivot.transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * _lookSpeed, 0);
        _verticalAngle -= Input.GetAxis("Mouse Y") * _lookSpeed;
        _verticalAngle = Mathf.Clamp(_verticalAngle, -_verticalLookLimit, _verticalLookLimit);
        _cameraDistance -= Input.GetAxis("Mouse ScrollWheel") * _lookSpeed;
        _cameraDistance = Mathf.Clamp(_cameraDistance, _minCameraDistance, _maxCameraDistance);

        Quaternion rotation = Quaternion.Euler(_verticalAngle, _pivot.transform.eulerAngles.y, 0);
        _camera.transform.position = _rigidbody.transform.position + rotation * _cameraOffset * _cameraDistance;
        _camera.transform.LookAt(_rigidbody.transform.position);

        Vector3 cameraForward = new Vector3(_camera.transform.forward.x, 0, _camera.transform.forward.z);
        Vector3 cameraRight = new Vector3(_camera.transform.right.x, 0, _camera.transform.right.z);

        _moveDirection = (cameraForward * Input.GetAxis("Vertical")) + (cameraRight * Input.GetAxis("Horizontal"));

        if (Input.GetKeyDown(KeyCode.Space)) {
            _rigidbody.AddForce(Vector3.up * _jumpPower);
        }
    }


    void FixedUpdate()
    {
        _rigidbody.AddForce(_moveDirection * _movementSpeed);
    }
}