using UnityEngine;

public abstract class MissileScript : MonoBehaviour
{
    [SerializeField] protected float _baseSpeed;

    protected Rigidbody2D _rigidbody;
    protected Vector2 _direction = Vector2.zero;
    protected bool _isEnemy = false;
    protected float _speed = 0;


    void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        _rigidbody.velocity = _direction * _speed;
        if (_rigidbody.position.y > 15 || _rigidbody.position.y < -15) {
            Destroy(gameObject);
        }
    }

    public abstract void Setup(Vector2 direction, bool isEnemy);
    protected abstract void Explode();

    private void OnTriggerEnter2D(Collider2D collider)
    {
        GameObject hit = collider.gameObject;
        
        if (hit.CompareTag("Player") && _isEnemy) {
            GameManager.Instance.RemoveLife();
        }
        else if (hit.CompareTag("Enemy") && !_isEnemy) {
            hit.GetComponent<EnemyShip>().Destroyed();
        }
        else if (hit.CompareTag("Defense")) {
            Destroy(hit);
        }
        else {
            return;
        }

        Explode();
    }
}
