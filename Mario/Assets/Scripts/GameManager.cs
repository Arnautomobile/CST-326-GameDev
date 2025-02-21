using System;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField] private Camera _camera;
    [SerializeField] private GameObject _canvas;
    [SerializeField] private float _timeSpeed = 2;
    [SerializeField] private int _playTime = 400;
    [SerializeField] private int _score = 0;
    [SerializeField] private int _coins = 0;
    
    private TextMeshProUGUI[] _texts = null;
    private int _timeLeft; 


    void Awake()
    {
        if (Instance == null) {
            Instance = this;
        }
        else {
            Destroy(gameObject);
        }
    }

    void Start()
    {        
        _texts = _canvas.GetComponentsInChildren<TextMeshProUGUI>();
        _timeLeft = _playTime;
    }


    void Update()
    {
        _timeLeft = _playTime - (int)(Time.time * _timeSpeed);

        string scoreStr = "";
        for (int i = 1; i < 1000000; i *= 10) {
            scoreStr = (_score / i) % 10 + scoreStr;
        }
        _texts[0].text = "Mario\n" + scoreStr;

        if (_coins < 10)
            _texts[1].text = "x0" + _coins;
        else
            _texts[1].text = "x" + _coins;
        
        _texts[3].text = "Time\n" + _timeLeft;

        CheckMouseClicks();
    }


    void CheckMouseClicks()
    {
        if (!Input.GetMouseButtonUp(0)) return;

        Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, 1000))
        {
            GameObject hitObject = hit.transform.gameObject;
            if (hitObject.name == "Question(Clone)") _coins++;
            Destroy(hitObject);
        }
    }
}
