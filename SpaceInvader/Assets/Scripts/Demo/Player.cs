using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public GameObject bullet;
    public Transform shottingOffset;
    public float fireRate = 0.05f;

    private float timer = 0f;


    void Update()
    {
        timer += Time.deltaTime;
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Fire();
        }
        else if (Input.GetKey(KeyCode.Space) && timer >= fireRate) {
            Fire();
            timer = 0;
        }
    }

    private void Fire() {
            GameObject shot = Instantiate(bullet, shottingOffset.position, Quaternion.identity);
            Debug.Log("Bang!");
            //Destroy(shot, 3f);
    }
}
