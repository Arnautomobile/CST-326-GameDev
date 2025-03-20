using UnityEngine;

public class Trampoline : MonoBehaviour
{
    public AudioSource audioSource;
    public float impulseStrength = 1f;

    void OnCollisionEnter(Collision collision)
    {
        Debug.Log($"{this.name} collided with the {collision.gameObject.name}");
        Rigidbody rb = collision.gameObject.GetComponent<Rigidbody>();
        rb.AddForce(transform.up * impulseStrength, ForceMode.Impulse);
        audioSource.Play();
    }
}
