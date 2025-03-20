using System.Collections;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    
    [SerializeField] private GameObject _ball;
    [SerializeField] private GameObject _camera;
    [SerializeField] private GameObject _paddleLeft;
    [SerializeField] private GameObject _paddleRight;
    [SerializeField] private GameObject _soloUI;
    [SerializeField] private GameObject _duelUI;
    
    [SerializeField] private string _gamemode = "Solo"; // todo - make that a public property to access via menu
    [SerializeField] private int _soloLives = 5;
    [SerializeField] private int _scoreToWin = 11;
    [SerializeField] private float _ballAcceleration = 0.05f;
    [SerializeField] private float _waitTime = 3f;

    private TextMeshProUGUI[] _UItexts = null;
    private PaddleController _controllerLeft;
    private PaddleController _controllerRight;
    private readonly int[] _score = {0, 0};
    private int _topScore = 0;
    
    public int NextPlayer { get; private set; } = 1;
    public float BallAcceleration { get => _ballAcceleration; }
    public BallMovement BallMovement { get; set; } = null;



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
        _controllerLeft = _paddleLeft.GetComponent<PaddleController>();
        _controllerRight = _paddleRight.GetComponent<PaddleController>();

        StartGame();
    }


    private void StartGame()
    {
        switch (_gamemode)
        {
            case "Solo":
                _soloUI.SetActive(true);
                _duelUI.SetActive(false);
                _UItexts = _soloUI.GetComponentsInChildren<TextMeshProUGUI>();
                _UItexts[0].text = $"Score : 0";
                _controllerLeft.IsAI = false;
                _controllerRight.IsAI = true;
                break;

            case "Duel":
                _soloUI.SetActive(false);
                _duelUI.SetActive(true);
                _UItexts = _duelUI.GetComponentsInChildren<TextMeshProUGUI>();
                _UItexts[0].text = $"Score : 0 - 0";
                _controllerLeft.IsAI = false;
                _controllerLeft.IsAI = false;
                break;

            case "Spectator":
                _soloUI.SetActive(false);
                _duelUI.SetActive(false);
                _controllerLeft.IsAI = true;
                _controllerRight.IsAI = true;
                _UItexts = null;
                break;

            default:
                Debug.Log("Gamemode is incorrect, game launch failed.");
                return;
        }

        _score[0] = 0;
        _score[1] = 0;
        ChangeScore(0);
    }


    public void Scored(int playerNumber)
    {
        BallMovement = null;
        _controllerLeft.ResetSpeed();
        _controllerRight.ResetSpeed();

        if (playerNumber == 1) {
            SetNextPlayer(2);
        }
        else if (playerNumber == 2) {
            SetNextPlayer(1);
        }
        else {
            StartCoroutine(CreateNewBall());
            return;
        }

        _score[playerNumber - 1]++;
        ChangeScore(playerNumber);
    }



    private void ChangeScore(int winningPlayer)
    {
        if (_gamemode == "Solo")
        {
            if (winningPlayer == 1 && _score[0] > _topScore) {
                _topScore = _score[winningPlayer - 1];
            }

            _UItexts[0].text = $"Score : {_score[0]}";
            _UItexts[1].text = $"Lives Left : {_soloLives - _score[1]}";
            _UItexts[2].text = $"Top Score : {_topScore}";

            if (winningPlayer == 2 && _score[1] >= _soloLives) {
                StartGame();
                return;
            }
        }

        else if (_gamemode == "Duel")
        {
            _UItexts[0].text = $"Score : {_score[0]} - {_score[1]}";

            if ((winningPlayer == 1 || winningPlayer == 2) && _score[winningPlayer - 1] >= _scoreToWin) {
                StartGame();
                return;
            }

            _controllerLeft.ChangeSize((float)(_scoreToWin - _score[0]) / (float)_scoreToWin);
            _controllerRight.ChangeSize((float)(_scoreToWin - _score[1]) / (float)_scoreToWin);
        }

        StartCoroutine(CreateNewBall());
    }


    public void SetNextPlayer(int player)
    {
        NextPlayer = player;

        if (_gamemode != "Duel") return;

        if (player == 2) {
            _UItexts[1].text = "RIGHT PLAYER";
            _UItexts[1].color = Color.red;
        }
        else if (player == 1) {
            _UItexts[1].text = "LEFT PLAYER";
            _UItexts[1].color = Color.blue;
        }
    }



    public PaddleController GetSenderPaddle()
    {
        if (NextPlayer == 1)
            return _controllerRight;
        if (NextPlayer == 2)
            return _controllerLeft;
        return null;
    }


    private IEnumerator CreateNewBall()
    {
        yield return new WaitForSeconds(_waitTime);
        BallMovement = Instantiate(_ball).GetComponent<BallMovement>();
    }
}
