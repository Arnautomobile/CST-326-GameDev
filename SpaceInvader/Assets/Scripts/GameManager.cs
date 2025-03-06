using System;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public GameObject Player { get; private set; }

    [SerializeField] private GameObject _playerPrefab;
    [SerializeField] private GameObject _canvas;
    [SerializeField] private int _lives;
    
    private TextMeshProUGUI[] _texts;
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
        _texts = _canvas.GetComponentsInChildren<TextMeshProUGUI>();
        StartGame();
    }


    void Update()
    {
        
    }

    public void StartGame()
    {
        Player = Instantiate(_playerPrefab);
    }

    public void EndGame()
    {
        
    }
}
