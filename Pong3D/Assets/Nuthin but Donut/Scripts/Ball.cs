using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameTag
{
  Rim,
  Trampoline,
  Backboard,
  Score
}

[RequireComponent(typeof(AudioSource))]
public class Ball : MonoBehaviour
{
  public int amplify = 1000;
  private bool groundHit = false;
  private Vector3 impactValue = Vector3.zero;
  private AudioSource audioSource;
  public AudioClip trampolineCollision;
  public AudioClip rimCollision;
  public AudioClip backboardCollision;
  public AudioClip scoringClip;


  void Awake()
  {
    audioSource = GetComponent<AudioSource>();
  }

  void OnCollisionEnter(Collision collision)
  {
    //Debug.Log($"I hit the {collision.collider.name}");
    if (collision.gameObject.tag == (GameTag.Trampoline.ToString()))
    {
      audioSource.PlayOneShot(trampolineCollision);
    }

    if (collision.gameObject.tag == GameTag.Rim.ToString())
    {
      audioSource.PlayOneShot(rimCollision);
    }
  }

  void FixedUpdate()
  {
    //if (groundHit) HitGround();
    //impactValue = GetComponent<Rigidbody>().velocity;
  }


}
