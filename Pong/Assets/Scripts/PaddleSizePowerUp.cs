using System.Collections;
using UnityEngine;

public class PaddleSizePowerUp : PowerUpScript
{
    [SerializeField] private float _bonusPaddleSize;

    protected override IEnumerator PowerUp(PaddleController paddleController)
    {
        Material paddleMaterial = paddleController.GetComponent<Renderer>().material;
        paddleMaterial.EnableKeyword("_EMISSION");
        paddleMaterial.SetColor("_EmissionColor", _color);
        paddleController.SetBonusSize(_bonusPaddleSize);
        paddleController.PowerUpActivated = true;

        yield return new WaitForSeconds(_durationTime);
        
        paddleController.PowerUpActivated = false;
        paddleController.SetBonusSize(0);
        paddleMaterial.SetColor("_EmissionColor", Color.black);
        paddleMaterial.DisableKeyword("_EMISSION");

        Destroy(gameObject);
    }
}
