using UnityEngine;

public class WaterScript : MonoBehaviour
{
    void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("Player")) {
            Debug.Log("Lost");
            GameManager.Instance.GameOver(false);
        }
    }
}
