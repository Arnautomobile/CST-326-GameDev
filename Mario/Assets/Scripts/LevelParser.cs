using System.Collections.Generic;
using System.IO;
using UnityEngine;


public class LevelParser : MonoBehaviour
{
    [SerializeField] private string _filename;
    [SerializeField] private Transform _environmentRoot;

    [Header("Block Prefabs")]
    [SerializeField] private GameObject _rockPrefab;
    [SerializeField] private GameObject _brickPrefab;
    [SerializeField] private GameObject _questionBlockPrefab;
    [SerializeField] private GameObject _stonePrefab;
    [SerializeField] private GameObject _waterPrefab;
    [SerializeField] private GameObject _polePrefab;


    void Start()
    {
        LoadLevel();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R)) {
            ReloadLevel();
            GameManager.Instance.StartGame();
        }
    }

    void ReloadLevel()
    {
        foreach (Transform child in _environmentRoot)
           Destroy(child.gameObject);

        LoadLevel();
    }


    void LoadLevel()
    {
        string fileToParse = $"{Application.dataPath}/Resources/{_filename}.txt";
        Debug.Log($"Loading level file: {fileToParse}");

        Stack<string> levelRows = new();

        // Get each line of text representing blocks in our level
        using (StreamReader sr = new(fileToParse))
        {
            while (sr.ReadLine() is { } line)
                levelRows.Push(line);
            sr.Close();
        }

        // Use this variable in the todo code!!
        int row = 0;
        // Go through the rows from bottom to top
        while (levelRows.Count > 0)
        {
            string currentLine = levelRows.Pop();

            char[] letters = currentLine.ToCharArray();
            for (int col = 0; col < letters.Length; ++col)
            {
                char letter = letters[col];
                GameObject prefab = null;
                
                switch (letter) {
                    case 'x':
                        prefab = _rockPrefab;
                        break;
                    case 'b':
                        prefab = _brickPrefab;
                        break;
                    case 's':
                        prefab = _stonePrefab;
                        break;
                    case '?':
                        prefab = _questionBlockPrefab;
                        break;
                    case 'w':
                        prefab = _waterPrefab;
                        break;
                    case 'p':
                        prefab = _polePrefab;
                        break;
                }

                if (prefab != null) {
                    GameObject block = Instantiate(prefab, new Vector3(col, row, 0), Quaternion.identity);
                    block.transform.parent = _environmentRoot.transform;
                }
                // Todo - Instantiate a new GameObject that matches the type specified by letter
                // Todo - Position the new GameObject at the appropriate location by using row and column
                // Todo - Parent the new GameObject under levelRoot
            }
            row++;
        }
    }
}
