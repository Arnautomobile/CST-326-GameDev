using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;




// implement overheating
// fix text mesh pro shadow
// finish menus and info text
// add settings and option menu
// animate with scale
// create bomber



public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public GameObject Player { get; private set; }
    public bool Paused { get; private set; }
    public float EnemyMovePeriod { get; private set; }

    [SerializeField] private AudioClip _music;

    [Header("Scene names")]
    [SerializeField] private string _startScene;
    [SerializeField] private string _gameScene;
    [SerializeField] private string _creditScene;

    [Header("Game parameters")]
    [SerializeField] private GameObject _playerPrefab;
    [SerializeField] private GameObject _canvas;
    [SerializeField] private int _startingLives;
    [SerializeField] private float _baseEnemyMovePeriod;
    [SerializeField] private float _enemyAcceleration;
    
    private TextMeshProUGUI[] _textsUI;
    private AudioSource _musicSource;
    private int _livesLeft;
    private int _score;


    void Awake()
    {
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        _textsUI = _canvas.GetComponentsInChildren<TextMeshProUGUI>();
        EnemyMovePeriod = _baseEnemyMovePeriod;
        _livesLeft = _startingLives;
        _score = 0;

        _musicSource = GetComponent<AudioSource>();
        _musicSource.clip = _music;
        _musicSource.Play();
        
        Pause(true);
    }


    public void EndGame(bool won)
    {
        if (won) {
            EnemyMovePeriod -= _enemyAcceleration;
        }
        else {
            EnemyMovePeriod = _baseEnemyMovePeriod;
            _livesLeft = _startingLives;
            _score = 0;
            EnemyManager.Instance.EndReached = true;
            Destroy(Player);
        }
        StartCoroutine(Reload(won));
    }


    private void SetScore(TextMeshProUGUI scoreText, int score)
    {
        string scoreStr = "";
        for (int i = 1; i < 1000000; i *= 10) {
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
        Player.GetComponent<PlayerController>().Hit();

        if (_livesLeft <= 0) {
            EndGame(false);
        }
    }

    public void Pause(bool pause)
    {
        _textsUI[0].enabled = !pause;
        _textsUI[1].enabled = !pause;
        _textsUI[2].enabled = !pause;
        _textsUI[3].enabled = !pause;
        _textsUI[4].enabled = !pause;
        _textsUI[5].enabled = !pause;
        Paused = pause;
    }



    private IEnumerator Reload(bool won)
    {
        TextMeshProUGUI text = won ? _textsUI[6] : _textsUI[7];
        text.enabled = true;
        yield return new WaitForSeconds(5);
        text.enabled = false;

        Pause(true);
        if (won) {
            SwitchScene(1);
        }
        else {
            SwitchScene(2);
        }
    }

    private IEnumerator StartGame()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(_gameScene);
        yield return new WaitUntil(() => asyncLoad.isDone);

        Player = Instantiate(_playerPrefab);
        SetScore(_textsUI[0], _score);
        _textsUI[2].text = _livesLeft.ToString();
        if (!PlayerPrefs.HasKey("HighScore")) {
            PlayerPrefs.SetInt("HighScore", 0);
        }
        SetScore(_textsUI[1], PlayerPrefs.GetInt("HighScore"));

        Pause(false);
    }

    private IEnumerator ShowCredits()
    {
        MenuManager.Instance.MainOpened = true;
        SceneManager.LoadScene(_creditScene);
        yield return new WaitForSeconds(5);
        MenuManager.Instance.QuitGame();
    }



    public void SwitchScene(int scene)
    {
        if (scene == 0) {
            SceneManager.LoadScene(_startScene);
        }
        else if (scene == 1) {
            StartCoroutine(StartGame());
        }
        else if (scene == 2) {
            StartCoroutine(ShowCredits());
        }
    }
}