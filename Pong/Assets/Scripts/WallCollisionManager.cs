using System;
using UnityEngine;

public class WallCollisionManager : MonoBehaviour
{
    [SerializeField] private AudioClip _audioClip;
    private AudioSource _audioSource;
    

    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
    }


    void OnCollisionEnter(Collision collision)
    {
        GameObject gameObject = collision.collider.gameObject;
        if (gameObject.CompareTag("Ball"))
        {
            _audioSource.PlayOneShot(_audioClip);
        }
        BounceBall(gameObject);
    }

    void OnCollisionStay(Collision collision)
    {
        BounceBall(collision.collider.gameObject);
    }

    void BounceBall(GameObject ball)
    {
        BallMovement ballMovement;
        if ((ballMovement = ball.GetComponent<BallMovement>()) != null) {
            float newDirection = ball.transform.position.z < 0 ? 1 : -1;
            ballMovement.Direction = new Vector3(ballMovement.Direction.x, 0, newDirection * Math.Abs(ballMovement.Direction.z));
        }
    }
}
