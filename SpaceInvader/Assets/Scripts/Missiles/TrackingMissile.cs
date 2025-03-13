using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackingMissile : BaseMissile
{
    [SerializeField] private float _rotateSpeed;

    public override void Setup(Vector2 direction, bool isEnemy)
    {
        base.Setup(direction, isEnemy);
        transform.eulerAngles = new Vector3(0,0,90);
    }

    private new void FixedUpdate()
    {
        base.FixedUpdate();

        if (GameManager.Instance.Player == null || GameManager.Instance.Paused) return;

        Vector3 heading = (GameManager.Instance.Player.transform.position - transform.position).normalized;
        float angle = Vector3.Cross(_direction, heading).z * 100;

        if (angle > _rotateSpeed)
            angle = _rotateSpeed;
        else if (-angle > _rotateSpeed)
            angle = -_rotateSpeed;

        _direction = Quaternion.Euler(0, 0, angle) * _direction;
        transform.eulerAngles = new Vector3(0, 0, transform.eulerAngles.z + angle);
    }
}
