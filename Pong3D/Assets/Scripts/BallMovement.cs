using UnityEngine;

public class BallMovement : MonoBehaviour
{
    [SerializeField] private float _speed = 15f;
    private Rigidbody _rigidbody;

    public Vector3 Direction { get; set; }
    public float BonusSpeed { get; set; }


    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();

        if (GameManager.Instance.NextPlayer == 1) {
            Direction = Vector3.left;
        }
        else if (GameManager.Instance.NextPlayer == 2) {
            Direction = Vector3.right;
        }
        else {
            Debug.Log("Starting player is undefined.");
            Direction = Vector3.zero;
        }
    }


    void FixedUpdate()
    {
        if (Direction == Vector3.zero || _rigidbody.position.x < -20 || _rigidbody.position.x > 20
            || _rigidbody.position.z > 15 || _rigidbody.position.z < -15)
        {
            Debug.Log("Ball went out of the map.");
            GameManager.Instance.Scored(0);
            Destroy(gameObject);
        }
        else {
            _rigidbody.velocity = Direction * (_speed + BonusSpeed);
        }
    }


    public void IncreaseSpeed(float factor) {
        _speed += _speed * factor;
    }
}
