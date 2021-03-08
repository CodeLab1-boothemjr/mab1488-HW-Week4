using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Numerics;
using UnityEngine;
using UnityEngine.SceneManagement;
using Object = System.Object;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

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
        // todo: save the game state
        if (Input.GetKey(KeyCode.P))
        {
            SaveState();
        }

        // todo: load the game state
        if (Input.GetKey(KeyCode.L))
        {
            LoadState();
        }
        
        // remove saved level + meta files
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

    // bug: does not currently update to correct level
    private static void SaveState()
    {
        string text = "";
        
        // save player position
        GameObject player = GameObject.FindWithTag("Player");
        text += player.transform.position + "\n";

        // save enemy positions
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        for (int i = 0; i < enemies.Length; i++)
        {
            text += enemies[i].transform.position + "\n";
        }

        // todo: save scene index 
        
        // write to file
        File.WriteAllText(FILE_PATH_GAME_STATE, text);
        Debug.Log("Game State Saved!");
    }

    private static void LoadState()
    {
        // connect player (for moving)
        GameObject player = GameObject.FindWithTag("Player");
        
        // connect enemy object (for instantiating)
        GameObject enemy = GameObject.FindWithTag("Enemy");
        
        // destroy enemies
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        for (int i = 0; i < enemies.Length; i++)
        {
            Destroy(enemies[i]);
        }

        // load gamestate from file
        string text = File.ReadAllText(FILE_PATH_GAME_STATE);
        
        // load player position
        string[] textArr = text.Split('\n');
        player.transform.position = StringToVector3(textArr[0]);

        // load enemy positions
        for (int i = 1; i < textArr.Length-1; i++)
        {
            GameObject newEnemy =  Instantiate(enemy);
            newEnemy.transform.position = StringToVector3(textArr[i]);
        }

        // todo: load scene index

    }
    public static Vector3 StringToVector3(string textVector)
    {
        // substring to remove parentheses
        textVector = textVector.Substring(1, textVector.Length-2);

        // split the items
        string[] sArray = textVector.Split(',');
        
        // store in Vector3
        Vector3 result = new Vector3(
            float.Parse(sArray[0]),
            float.Parse(sArray[1]),
            float.Parse(sArray[2]));

        return result;
    }
    
}
