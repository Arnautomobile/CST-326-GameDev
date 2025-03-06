using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FastMissile : MissileScript
{
    private int _bounces = 1;

    public override void Setup(Vector2 direction, bool isEnemy)
    {
        _direction = direction;
        _isEnemy = isEnemy;
        _speed = _baseSpeed;
    }

    protected override void HitObject(GameObject hit)
    {
        Destroy(hit);

        if (_bounces > 0 && hit.CompareTag("Defense")) {
            _direction = new Vector2(0, -_direction.y);
            transform.localScale = new Vector3(transform.localScale.x, -transform.localScale.y, 1);
        }
        else {
            Destroy(gameObject);
        }
    }
}
