using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseMissile : MissileScript
{
    public override void Setup(Vector2 direction, bool isEnemy)
    {
        _direction = direction;
        _isEnemy = isEnemy;
        _speed = _baseSpeed;
    }

    protected override void Explode()
    {
        Destroy(gameObject);
    }
}
