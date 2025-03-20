using UnityEngine;

public class PaddleController : MonoBehaviour
{
    [SerializeField] private AudioClip _audioClip;
    [SerializeField] private int _playerNumber;
    [SerializeField] private float _minSpeed = 10;
    [SerializeField] private float _additionalSpeed = 0.1f;
    [SerializeField] private float _minSize = 3f;
    [SerializeField] private float _maxAdditionalSize = 2f;

    private AudioSource _audioSource;
    private Vector3 _direction;
    private float _currentSpeed;
    private float _bonusSize;

    public bool IsAI { get; set; }
    public bool PowerUpActivated { get; set; } = false;
    public float BonusBallSpeed { get; set; } = 0;


    void Start()
    {
        if (_playerNumber != 1 && _playerNumber != 2) {
            Debug.Log("Wrong player number assigned for this paddle.");
        }

        _audioSource = GetComponent<AudioSource>();
        _currentSpeed = _minSpeed;
        _direction = Vector3.zero;
    }


    void Update()
    {
        _direction = Vector3.zero;

        if (IsAI) {
            AIMovement();
        }
        else {
            PlayerMovement();
        }

        transform.position += _currentSpeed * Time.deltaTime * _direction;
        float positionLimit = 6f - (transform.localScale.z - 1f) / 2f;

        if (transform.position.z > positionLimit) {
            transform.position = new Vector3(transform.position.x, 0.5f, positionLimit);
        }
        else if (transform.position.z < -positionLimit) {
            transform.position = new Vector3(transform.position.x, 0.5f, -positionLimit);
        }
    }


    private void PlayerMovement()
    {
        if (_playerNumber == 1) {
            _direction = new Vector3(0, 0, Input.GetAxis("Paddle Left"));
        }
        else if (_playerNumber == 2) {
            _direction = new Vector3(0, 0, Input.GetAxis("Paddle Right"));
        }
    }

    private void AIMovement()
    {
        float predictedZ = 0; 
        BallMovement ballMovement;

        if ((ballMovement = GameManager.Instance.BallMovement) != null) {

            predictedZ = ballMovement.transform.position.z;
            if (_playerNumber == 1 && ballMovement.Direction.x < 0) {
                predictedZ += ballMovement.Direction.z * (-12 - ballMovement.transform.position.x) / ballMovement.Direction.x;
            }
            else if (_playerNumber == 2 && ballMovement.Direction.x > 0) {
                predictedZ += ballMovement.Direction.z * (12 - ballMovement.transform.position.x) / ballMovement.Direction.x;
            }
        }

        if (transform.position.z < predictedZ - 0.5f) {
            _direction = Vector3.forward;
        }
        else if (transform.position.z > predictedZ + 0.5f) {
            _direction = Vector3.back;
        }
    }


    public void ResetSpeed()
    {
        _currentSpeed = _minSpeed;
    }

    public void ChangeSize(float factor)
    {
        transform.localScale = new Vector3(1, 1, _minSize + _maxAdditionalSize * factor + _bonusSize);
    }

    public void SetBonusSize(float newBonusSize) {
        transform.localScale = new Vector3(1, 1, transform.localScale.z - _bonusSize + newBonusSize);
        _bonusSize = newBonusSize;
    }


    void OnCollisionEnter(Collision collision)
    {
        GameObject ball = collision.collider.gameObject;
        BallMovement ballMovement;

        if ((ballMovement = ball.GetComponent<BallMovement>()) != null)
        {
            _audioSource.PlayOneShot(_audioClip);

            Vector3 resultDirection = ball.transform.position - transform.position;
            resultDirection.x += _playerNumber == 1 ? 1 : -1;
            ballMovement.Direction = resultDirection.normalized;
            ballMovement.BonusSpeed = BonusBallSpeed;
            ballMovement.IncreaseSpeed(GameManager.Instance.BallAcceleration);

            if (!IsAI) {
                _currentSpeed += _additionalSpeed;
            }
            GameManager.Instance.SetNextPlayer(_playerNumber % 2 + 1);
        }
    }
}
