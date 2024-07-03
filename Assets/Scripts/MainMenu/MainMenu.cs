using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    AudioManager audioManager;
    [SerializeField] private Button continueButton;
    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    private void Start()
    {
        if (!DataActionManager.instance.HasSavedGame())
        {
            continueButton.gameObject.SetActive(false);
        }
    }
    public void PlayGame()
    {
        audioManager.PlaySFX(audioManager.buttonclick);
        SceneController.instance.LoadSavedScene();
    }

    public void NewGame()
    {
        audioManager.PlaySFX(audioManager.buttonclick);
        DataActionManager.instance.NewGame();
        SceneController.instance.LoadFirstScene();
    }
    public void QuitGame()
    {
        audioManager.PlaySFX(audioManager.buttonclick);
        Application.Quit();
    }
}
