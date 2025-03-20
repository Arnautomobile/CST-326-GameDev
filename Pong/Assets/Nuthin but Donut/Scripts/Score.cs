using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Score : MonoBehaviour
{
  public ScoreManager scoreManger;
  void OnTriggerExit()
  {
    scoreManger.AddScore();
  }
}
