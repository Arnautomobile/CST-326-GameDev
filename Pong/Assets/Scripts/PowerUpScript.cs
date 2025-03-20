using System;
using System.Collections;
using UnityEngine;

public abstract class PowerUpScript : MonoBehaviour
{
    [SerializeField] protected Color _color;
    [SerializeField] private AudioClip _audioClip;
    [SerializeField] protected float _durationTime = 20f;
    [SerializeField] private float _rotationSpeed = 200f;
    [SerializeField] private float _bounceRange = 0.5f;
    [SerializeField] private float _bouncingSpeed = 5f;
    [SerializeField] private float _baseHeight = 1f;


    private AudioSource _audioSource;
    private bool _isUsed = false;


    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (_isUsed) return;
        
        transform.eulerAngles = new Vector3(0, transform.eulerAngles.y + _rotationSpeed * Time.deltaTime, 0);
        transform.position = new Vector3(transform.position.x, _baseHeight + _bounceRange * (float)Math.Cos(Time.time * _bouncingSpeed), transform.position.z);
    }


    protected abstract IEnumerator PowerUp(PaddleController paddleController);


    void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("Ball") && !_isUsed) {
            
            PaddleController paddleController = GameManager.Instance.GetSenderPaddle();
            if (paddleController.PowerUpActivated) {
                return;
            }

            _isUsed = true;
            _audioSource.PlayOneShot(_audioClip);
            GetComponent<Collider>().enabled = false;
            GetComponent<Renderer>().enabled = false;
            StartCoroutine(PowerUp(paddleController));
        }
    }
}
