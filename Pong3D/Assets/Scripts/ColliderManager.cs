using UnityEngine;

public class ColliderManager : MonoBehaviour
{
    [SerializeField] private AudioClip _audioClip;
    private AudioSource _audioSource;

    [SerializeField] private int _playerNumber;


    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("Ball")) { 
            _audioSource.PlayOneShot(_audioClip);
            Destroy(collider.gameObject);
            GameManager.Instance.Scored(_playerNumber);
        }
    }
}
