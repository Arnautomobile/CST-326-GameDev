using UnityEngine;

public class InputController : MonoBehaviour
{
    public Transform leftPaddle;
    public Transform rightPaddle;
    public float paddleSpeed = 1f;

    private float leftInput = 0f;
    private float rightInput = 0f;

    //-----------------------------------------------------------------------------
    // Update is called once per frame
    void Update()
    {
        leftInput = Input.GetAxis("LeftPaddle");
        rightInput = Input.GetAxis("RightPaddle");
    }

    //-----------------------------------------------------------------------------
    void FixedUpdate() {
        Vector3 leftPaddleMovement = new Vector3(0, 0, paddleSpeed * leftInput * Time.deltaTime);
        Vector3 rightPaddleMovement = new Vector3(0, 0, paddleSpeed * rightInput * Time.deltaTime);

        leftPaddle.Translate(leftPaddleMovement);
        rightPaddle.Translate(rightPaddleMovement);
    }
}
