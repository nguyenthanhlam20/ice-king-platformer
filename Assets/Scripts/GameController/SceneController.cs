using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public static SceneController instance { get; private set; }
    [SerializeField] Animator transAimt;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            Debug.Log("SceneController instance set and marked as DontDestroyOnLoad.");
        }
        else
        {
            Destroy(gameObject);
            Debug.LogWarning("Attempting to create a second instance of SceneController. Destroying the new instance.");
        }

    }

    public void LoadNextScene()
    {
        StartCoroutine(LoadNewScene());
    } 


    public void LoadMainMenu() {
        SceneManager.LoadSceneAsync(0);    
    }

    public void LoadFirstScene()
    {
        SceneManager.LoadSceneAsync(1);

    }

    public void LoadSavedScene()
    {
        SceneManager.LoadSceneAsync(DataActionManager.instance.GetSavedSceneIndex());
    }

    public void NewGame()
    {
        DataActionManager.instance.NewGame();
        SceneManager.LoadSceneAsync(1);
    }

    IEnumerator LoadNewScene()
    {
        transAimt.SetTrigger("End");
        yield return new WaitForSeconds(1);
        // Check if the next scene index is within the valid range
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        transAimt.SetTrigger("Start");

    }
}
