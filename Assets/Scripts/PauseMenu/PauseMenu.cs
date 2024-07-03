using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] GameObject pauseMenu;
    [SerializeField] GameObject restartConfirm;

    // Start is called before the first frame update
    private static bool isPaused = false;
    AudioManager audioManager;
    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log(isPaused);
            if (isPaused)
            {
                audioManager.PlaySFX(audioManager.menuopen);
                Resume();
            }
            else
            {
                audioManager.PlaySFX(audioManager.menuopen);
                Pause();
            }
        }
    }
    public void Pause()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
    }

    public void Home()
    {
        audioManager.PlaySFX(audioManager.buttonclick);
        StartCoroutine(LoadScene());
    }

    public void Resume()
    {
        audioManager.PlaySFX(audioManager.buttonclick);
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }

    public void ConfirmRestart()
    {
        audioManager.PlaySFX(audioManager.buttonclick);
        restartConfirm.SetActive(true);
    }

    public void CancelRestart()
    {
        audioManager.PlaySFX(audioManager.buttonclick);
        restartConfirm.SetActive(false);
    }

    public void Restart()
    {
        audioManager.PlaySFX(audioManager.buttonclick);
        restartConfirm.SetActive(false);
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
        SceneController.instance.NewGame();
    }

    public void Settings()
    {
        audioManager.PlaySFX(audioManager.buttonclick);
        pauseMenu.SetActive(false);
        Time.timeScale = 0f; //pause game speed
        SettingsMenuManager.instance.OpenSettingsMenu();
    }

    private void LoadMainMenu()
    {
        SceneController.instance.LoadMainMenu();
        isPaused = false;
        Time.timeScale = 1f;
    }

    private IEnumerator LoadScene()
    {
        yield return new WaitForSecondsRealtime(0.5f); 
        SceneController.instance.LoadMainMenu();
        isPaused = false;
        Time.timeScale = 1f;
    }
}
