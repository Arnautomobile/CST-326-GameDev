using System.Collections;
using UnityEngine;

public class BallSpeedPowerUp : PowerUpScript
{
    [SerializeField] private float _bonusBallSpeed;

    protected override IEnumerator PowerUp(PaddleController paddleController)
    {
        Material paddleMaterial = paddleController.GetComponent<Renderer>().material;
        paddleMaterial.EnableKeyword("_EMISSION");
        paddleMaterial.SetColor("_EmissionColor", _color);
        paddleController.BonusBallSpeed = _bonusBallSpeed;
        paddleController.PowerUpActivated = true;

        yield return new WaitForSeconds(_durationTime);

        paddleController.PowerUpActivated = false;
        paddleController.BonusBallSpeed = 0;
        paddleMaterial.SetColor("_EmissionColor", Color.black);
        paddleMaterial.DisableKeyword("_EMISSION");

        Destroy(gameObject);
    }
}
