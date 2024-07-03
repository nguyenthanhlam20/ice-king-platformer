using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DataActionManager : MonoBehaviour
{

    [Header("File Save Config")]
    [SerializeField] private string fileName;


    private GameData gameData;
    private List<IDataAction> dataActions;
    private FileHandler fileHandler;

    public static DataActionManager instance {  get; private set; }
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        this.fileHandler = new FileHandler(Application.persistentDataPath, fileName);
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.sceneUnloaded += OnSceneUnloaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        SceneManager.sceneUnloaded -= OnSceneUnloaded;
    }

    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        this.dataActions = FindAllDataActionsObjects();
        LoadGame();
    }

    public void OnSceneUnloaded(Scene scene)
    {

        SaveGame();
    }

    private List<IDataAction> FindAllDataActionsObjects()
    {
        IEnumerable<IDataAction> dataActionsObject = FindObjectsOfType<MonoBehaviour>()
            .OfType<IDataAction>();
        return dataActionsObject.ToList();
    }

    public void NewGame()
    {
        //Create new blank GameData 
        this.gameData = new GameData();
    }

    public void LoadGame()
    {
        //Load saved GameData from file using DataHandler
        this.gameData = fileHandler.LoadFromFile();

        //No data loaded == Nothing saved => New game
        if(this.gameData == null)
        {
            return;
        }

        //Push the Loaded data to other IDataAction Script
        foreach (IDataAction action in dataActions)
        {
            action.LoadData(gameData);
        }
    }

    public void SaveGame()
    {
        if(this.gameData == null)
        {
            return;
        }

        // update the current scene in our data
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        // DON'T save this for certain scenes, like our main menu scene
        if (currentSceneIndex != 0)
        {
            gameData.sceneIndex = currentSceneIndex;
        }

        Debug.Log("test index: " + currentSceneIndex);

        foreach (IDataAction action in dataActions)
        {
            action.SaveData(ref gameData);
        }

        fileHandler.SaveToFile(gameData);

        Debug.Log("Saved Position: " + gameData.playerPosition.ToString());
    }

    public int GetSavedSceneIndex()
    {
        // error out and return null if we don't have any game data yet
        if (gameData == null)
        {
            Debug.LogError("Tried to get scene name but data was null.");
        }

        // otherwise, return that value from our data
        return gameData.sceneIndex;
    }

    public bool HasSavedGame()
    {
        return gameData != null;
    }


    private void OnApplicationQuit()
    {
        SaveGame();
    }
}
