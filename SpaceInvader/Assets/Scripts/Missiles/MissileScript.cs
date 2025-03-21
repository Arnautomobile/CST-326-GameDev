using UnityEngine;

public abstract class MissileScript : MonoBehaviour
{
    [SerializeField] protected float _baseSpeed;
    [SerializeField] private GameObject _shootParticle;
    [SerializeField] private GameObject _explosionParticle;

    protected Rigidbody2D _rigidbody;
    protected Vector2 _direction = Vector2.zero;
    protected bool _isEnemy = false;
    protected float _speed = 0;


    void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        GameObject particle = Instantiate(_shootParticle, transform.position, Quaternion.identity);
        Destroy(particle, 5);
    }

    protected void FixedUpdate()
    {
        _rigidbody.velocity = _direction * _speed;
        if (_rigidbody.position.y > 11 || _rigidbody.position.y < -11 || _rigidbody.position.x > 20 || _rigidbody.position.x < -20) {
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

        GameObject particle = Instantiate(_explosionParticle, transform.position, Quaternion.identity);
        Destroy(particle, 5);

        Explode();
    }
}
