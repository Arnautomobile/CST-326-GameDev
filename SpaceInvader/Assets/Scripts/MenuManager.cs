using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private GameObject _mainMenu;
    [SerializeField] private GameObject _inGameMenu;
    [SerializeField] private GameObject _optionsMenu;
    [SerializeField] private GameObject _background;

    public static MenuManager Instance { get; private set; } = null;
    public bool MainOpened { get; set; } = true;


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

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !MainOpened) {
            bool open = !_inGameMenu.activeSelf;
            _inGameMenu.SetActive(open);
            _background.SetActive(open);
            GameManager.Instance.Pause(open);
        }
    }


    public void PlayGame()
    {
        MainOpened = false;
        _mainMenu.SetActive(false);
        _background.SetActive(false);
        GameManager.Instance.SwitchScene(1);
    }

    public void QuitGame()
    {
        MainOpened = true;
        _mainMenu.SetActive(true);
        _background.SetActive(true);
        GameManager.Instance.SwitchScene(0);
    }

    public void CloseApp()
    {
        Debug.Log("Apllication Quit");
        Application.Quit();
    }

    public void OpenOptionsMenu()
    {
        _optionsMenu.SetActive(true);
        if (MainOpened) {
            _mainMenu.SetActive(false);
        }
        else {
            _inGameMenu.SetActive(false);
        }
    }

    public void GoBackToMenu()
    {
        _optionsMenu.SetActive(false);
        if (MainOpened) {
            _mainMenu.SetActive(true);
        }
        else {
            _inGameMenu.SetActive(true);
        }
    }
}
