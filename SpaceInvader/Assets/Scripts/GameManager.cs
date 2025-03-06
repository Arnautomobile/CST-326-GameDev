using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public GameObject Player { get; private set; }

    [SerializeField] private GameObject _playerPrefab;
    [SerializeField] private GameObject _canvas;
    [SerializeField] private int _startingLives;
    
    private TextMeshProUGUI[] _textsUI;
    private int _livesLeft;
    private int _score = 0;


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
        _textsUI = _canvas.GetComponentsInChildren<TextMeshProUGUI>();
        _livesLeft = _startingLives;
        StartGame();
    }


    public void StartGame()
    {
        Player = Instantiate(_playerPrefab);
        SetScore(_textsUI[0], 0);
        _textsUI[2].text = _livesLeft.ToString();

        if (!PlayerPrefs.HasKey("HighScore")) {
            PlayerPrefs.SetInt("HighScore", 0);
        }
        SetScore(_textsUI[1], PlayerPrefs.GetInt("HighScore"));
    }

    public void EndGame(bool won)
    {
        if (won) {
            _textsUI[3].enabled = true;
        }
        else {
            _textsUI[4].enabled = true;
            Destroy(Player);
        }
    }


    private void SetScore(TextMeshProUGUI scoreText, int score)
    {
        string scoreStr = "";
        for (int i = 1; i < 10000; i *= 10) {
            scoreStr = score / i % 10 + scoreStr;
        }
        scoreText.text = scoreStr;
    }


    public void AddScore(int points) 
    {
        _score += points;
        SetScore(_textsUI[0], _score);

        if (_score > PlayerPrefs.GetInt("HighScore")) {
            PlayerPrefs.SetInt("HighScore", _score);
            SetScore(_textsUI[1], _score);
        }
    }

    public void RemoveLife()
    {
        _livesLeft--;
        _textsUI[2].text = _livesLeft.ToString();

        if (_livesLeft <= 0) {
            EndGame(false);
        }
    }
}