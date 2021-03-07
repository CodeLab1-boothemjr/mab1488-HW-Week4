using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;

    private static int _currentLevel;
    private const int MAX_LEVEL = 2;
    
    const string DIR_LOGS = "/Logs";
    private const string FILE_LEVEL = DIR_LOGS + "/level.txt";
    private const string GAME_STATE = DIR_LOGS + "/state.txt";
    private static string FILE_PATH_LEVEL;
    private static string FILE_PATH_GAME_STATE;

    void Awake()
    {
        if (_instance == null)
        {
            DontDestroyOnLoad(gameObject);
            _instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        FILE_PATH_GAME_STATE = Application.dataPath + GAME_STATE;
        FILE_PATH_LEVEL = Application.dataPath + FILE_LEVEL;
        
        if (File.Exists(FILE_PATH_LEVEL))
        {
            //parse level from file if there's a file available
            string fileContents = File.ReadAllText(FILE_PATH_LEVEL);
            //set the level to what's in the file
            _currentLevel = Int32.Parse(fileContents);
            Debug.Log("LEVEL LOADED FROM FILE");
        }
        else
        {
            //create the directory if it doesn't exist
            Directory.CreateDirectory(Application.dataPath + DIR_LOGS);
            Debug.Log("DIRECTORY CREATED");
            //start at level 0
            _currentLevel = 0;
        }
                
        //load the correct level
        SceneManager.LoadScene(_currentLevel);
        Debug.Log("LEVEL LOADED");

        
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetKey(KeyCode.P))
        {
            SaveState();
        }

        if (Input.GetKey(KeyCode.L))
        {
            LoadState();
        }
        
        // remove saved level + meta files
        //todo - add method for removing saved level file
        if (Input.GetKey(KeyCode.N))
        {
            if (File.Exists(FILE_PATH_LEVEL))
            {
                File.Delete(FILE_PATH_LEVEL);
                File.Delete(FILE_PATH_LEVEL + ".meta");
                Debug.Log("FILE REMOVED");
            }
        }
    }

    public static void AdvanceCurrentLevel()
    {
        //advance only if not on the last level
        if (_currentLevel < MAX_LEVEL)
        {
            //load next scene
            SceneManager.LoadScene(++_currentLevel);
        
            //update save file to next level
            File.WriteAllText(FILE_PATH_LEVEL, _currentLevel + "");
            Debug.Log("FILE UPDATED");
        }
        else
        {
            Debug.Log("YOU WIN!");
        }
        
    }

    private static void SaveState()
    {
        
    }

    private static void LoadState()
    {
        
    }
}
