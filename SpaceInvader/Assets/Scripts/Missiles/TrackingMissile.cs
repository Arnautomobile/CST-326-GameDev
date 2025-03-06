using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackingMissile : BaseMissile
{
    public override void Setup(Vector2 direction, bool isEnemy)
    {
        _direction = direction;
        _isEnemy = isEnemy;
        _speed = _baseSpeed;
        transform.eulerAngles = new Vector3(0,0,90);
    }
}
