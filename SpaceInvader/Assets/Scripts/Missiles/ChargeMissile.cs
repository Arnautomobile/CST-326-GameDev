using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;

public class ChargeMissile : MissileScript
{
    [SerializeField] private float _maxSpeed;
    [SerializeField] private int _maxPower;

    private int _power = 0;


    public override void Setup(Vector2 direction, bool isEnemy)
    {
        _direction = direction;
        _isEnemy = isEnemy;
    }

    public void Setup(Vector2 direction, bool isEnemy, float power) {
        Setup(direction, isEnemy);
        _power += (int)(_maxPower * power);
        _speed = _baseSpeed + (_maxSpeed - _baseSpeed) * power;
        Debug.Log($"Power : {power}  |  new speed : {_speed}");
    }

    protected override void HitObject(GameObject hit)
    {
        Destroy(hit);
        if (_power == 0) {
            Destroy(gameObject);
            return;
        }
        _power--;
        _speed *= 0.8f;
    }
}
