using System;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] private GameObject _marioPrefab;
    [SerializeField] private GameObject _canvas;
    [SerializeField] private Camera _camera;

    [Header("Game Parameters")]
    [SerializeField] private int _playTime = 400;
    [SerializeField] private float _timeSpeed = 2;
    [SerializeField] private Vector3 _cameraOffset;
    
    private GameObject _mario;
    private TextMeshProUGUI[] _texts;
    private float _timeLeft;
    private bool _won = false;
    private int _score = 0;
    private int _coins = 0;


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
        StartGame();
    }


    void Update()
    {
        if (_mario == null) return;

        _timeLeft -= Time.deltaTime * _timeSpeed;

        if (_timeLeft <= 0) {
            GameOver(false);
        }

        UpdateTexts();
        FollowPlayer();
    }


    private void UpdateTexts()
    {
        string scoreStr = "";
        for (int i = 1; i < 1000000; i *= 10) {
            scoreStr = (_score / i) % 10 + scoreStr;
        }
        _texts[0].text = "Mario\n" + scoreStr;

        if (_coins < 10)
            _texts[1].text = "x0" + _coins;
        else
            _texts[1].text = "x" + _coins;
        
        _texts[3].text = "Time\n" + (int)_timeLeft;
    }

    private void FollowPlayer()
    {
        _camera.gameObject.transform.position = new Vector3(_mario.transform.position.x, 0, 0) + _cameraOffset;
    }


    public void BlockHit(GameObject block)
    {
        if (block.CompareTag("QuestionBlock")) {
            _coins++;
            _score += 100;
            Destroy(block);
        }
        else if (block.CompareTag("Brick")) {
            _score += 100;
            Destroy(block);
        }
    }

    public void StartGame()
    {
        _timeLeft = _playTime;
        _mario = Instantiate(_marioPrefab);

        if (!_won) {
            _score = 0;
            _coins = 0;
        }
        else {
            _won = false;
        }

        _texts[4].enabled = false;
        _texts[5].enabled = false;
        _texts[6].enabled = false;
    }

    public void GameOver(bool won)
    {
        Destroy(_mario);

        if (won) {
            _texts[4].enabled = true;
        }
        else {
            _texts[5].enabled = true;
        }
        _texts[6].enabled = true;
        _won = won;
    }
}
