using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FastMissile : BaseMissile
{
    [SerializeField] private int _bounces;

    protected override void Explode()
    {
        if (_bounces > 0) {
            _direction = new Vector2(0, -_direction.y);
            transform.localScale = new Vector3(transform.localScale.x, -transform.localScale.y, 1);
            _speed *= 0.8f;
            _bounces--;
        }
        else {
            base.Explode();
        }
    }
}
