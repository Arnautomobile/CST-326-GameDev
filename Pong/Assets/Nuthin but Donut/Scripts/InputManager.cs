using UnityEngine;

public class InputManager : MonoBehaviour
{
    public Hand hand;
    public Trampoline trampoline;


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            hand.Release();

        if (Input.GetKeyDown(KeyCode.R))
            hand.Reset();

        //if (Input.GetAxis("Trampoline") != 0)
        //    trampoline.transform.Translate(Vector3.forward * Input.GetAxis("Trampoline"), Space.World);
    }
}
