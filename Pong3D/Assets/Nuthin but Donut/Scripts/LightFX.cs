using UnityEngine;

public class LightFX : MonoBehaviour {
    public Light spotLight;
    public Transform minTransform;
    public Transform maxTransform;

    private float startTime;

    void Start() {
        startTime = Random.Range(100f, 1000000f);
    }

    // Update is called once per frame
    void Update() {
        Vector3 minPos = minTransform.position;
        Vector3 maxPos = maxTransform.position;

        float t = startTime + Time.timeSinceLevelLoad;
        float n1 = Mathf.PerlinNoise(t, 0f);
        float n2 = Mathf.PerlinNoise(0f, t);

        Debug.Log(n1);

        float x = minPos.x + (maxPos.x - minPos.x) * n1;
        float z = minPos.z + (maxPos.z - minPos.z) * n2;

        spotLight.transform.position = new Vector3(x, spotLight.transform.position.y, z);
    }
}
