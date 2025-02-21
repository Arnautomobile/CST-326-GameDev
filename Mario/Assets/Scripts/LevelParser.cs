using System.Collections.Generic;
using System.IO;
using UnityEngine;

/*
 * This script is responsible for reading a level layout from a text file and constructing the level
 * in a Unity scene by instantiating block GameObjects. The level file should be placed in the
 * Resources folder, and each line in the file represents a row of blocks (with the bottom row processed first).
 *
 * WHAT YOU NEED TO DO:
 * 1. In the foreach loop that iterates over each character (letter) in the current row, determine
 *    which type of block to create based on the letter (e.g., use 'R' for rock, 'B' for brick, etc.).
 *
 * 2. Instantiate the correct prefab (rockPrefab, brickPrefab, questionBoxPrefab, stonePrefab) corresponding
 *    to the letter.
 *
 * 3. Calculate the position for the new block GameObject using the current 'row' value and its column index.
 *    - You will likely need to maintain a separate column counter as you iterate through the characters.
 *
 * 4. Set the instantiated block’s parent to 'environmentRoot' to keep the hierarchy organized.
 *
 * ADDITIONAL NOTES:
 * - The level reloads when the player presses the 'R' key, which clears all blocks under environmentRoot
 *   and then re-parses the level file.
 * - Ensure that the level file's name (without the extension) matches the 'filename' variable.
 *
 * By completing these TODOs, you will enable the level parser to dynamically create and position
 * the blocks based on the level file data.
 */

public class LevelParser : MonoBehaviour
{
    [SerializeField] private string _filename;
    [SerializeField] private Transform _environmentRoot;

    [Header("Block Prefabs")]
    [SerializeField] private GameObject _rockPrefab;
    [SerializeField] private GameObject _brickPrefab;
    [SerializeField] private GameObject _questionBoxPrefab;
    [SerializeField] private GameObject _stonePrefab;

    // --------------------------------------------------------------------------
    void Start()
    {
        LoadLevel();
    }

    // --------------------------------------------------------------------------
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
            ReloadLevel();
    }

    // --------------------------------------------------------------------------
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
                GameObject block = null;
                
                if (letter == 'x') {
                    block = Instantiate(_rockPrefab, new Vector3(col, row, 0), Quaternion.identity);
                }
                else if (letter == 'b') {
                    block = Instantiate(_brickPrefab, new Vector3(col, row, 0), Quaternion.identity);
                }
                else if (letter == 's') {
                    block = Instantiate(_stonePrefab, new Vector3(col, row, 0), Quaternion.identity);
                }
                else if (letter == '?') {
                    block = Instantiate(_questionBoxPrefab, new Vector3(col, row, 0), Quaternion.identity);
                }

                if (block != null) {
                    block.transform.parent = _environmentRoot.transform;
                }
                // Todo - Instantiate a new GameObject that matches the type specified by letter
                // Todo - Position the new GameObject at the appropriate location by using row and column
                // Todo - Parent the new GameObject under levelRoot
            }
            row++;
        }
    }

    // --------------------------------------------------------------------------
    void ReloadLevel()
    {
        foreach (Transform child in _environmentRoot)
           Destroy(child.gameObject);

        LoadLevel();
    }
}
