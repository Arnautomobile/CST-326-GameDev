using UnityEngine;

public class PoleScript : MonoBehaviour
{
    void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("Player")) {
            Debug.Log("Won");
            GameManager.Instance.GameOver(true);
        }
    }
}
