using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class SettingsMenuManager : MonoBehaviour
{
    public static SettingsMenuManager instance { get; private set; }
    [SerializeField] GameObject settingsMenu;
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private Slider masterSlider, musicSlider, sfxSlider;
    AudioManager audioManager;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    void Start()
    {
        //hide setting menu when gaem load
        settingsMenu.SetActive(false);

        if (PlayerPrefs.HasKey("masterSliderKeyName")) //load saved volume if any adjust being made(game be open for second time and so on)
        {
            LoadVolume();
        }
        else //if game being open for the first time, volume will be default to highest
        {
            SetMasterVolumeSlider();
            SetMusicVolumeSlider();
            SetSFXVolumeSlider();
        }
    }

    public void OpenSettingsMenu() //call by Settings() in PauseMenu.cs 
    {
        settingsMenu.SetActive(true);
    }

    public void CloseSettingsMenu()
    {
        audioManager.PlaySFX(audioManager.buttonclick);
        settingsMenu.SetActive(false);
        Time.timeScale = 1f; //return the real-time game speed 
        // PauseMenu.PauseMenuInstance.Pause(); //still pause when close setting
    }

    public void SetMasterVolumeSlider()
    {
        float masterVol = masterSlider.value;
        // Debug.Log("value of master volume: " + masterVol);
        //since audio mixer change logarithmic (dB), volume slider change linear. To adjust volume properly, convert linear to logarith 
        audioMixer.SetFloat("MasterVolParam", Mathf.Log10(masterVol) * 20); //converts a linear volume value (masterVol) to decibels
        PlayerPrefs.SetFloat("masterSliderKeyName", masterVol); //save the value to PlayerPrefs of SettingsMenu prefabs
    }

    public void SetMusicVolumeSlider()
    {
        Debug.Log("function run");
        float musicVol = musicSlider.value;
        // Debug.Log("value of music volume: " + musicVol);
        audioMixer.SetFloat("MusicVolParam", Mathf.Log10(musicVol) * 20);
        PlayerPrefs.SetFloat("musicSliderKeyName", musicVol);
    }

    public void SetSFXVolumeSlider()
    {
        float sfxVol = sfxSlider.value;
        // Debug.Log("value of sfx volume: " + sfxVol);
        audioMixer.SetFloat("SfxVolParam", Mathf.Log10(sfxVol) * 20);
        PlayerPrefs.SetFloat("sfxSliderKeyName", sfxVol);
    }

    private void LoadVolume() //load the save value when game be open from the second time
    {
        Debug.Log(PlayerPrefs.GetFloat("masterSliderKeyName"));
        masterSlider.value = PlayerPrefs.GetFloat("masterSliderKeyName");
        musicSlider.value = PlayerPrefs.GetFloat("musicSliderKeyName");
        sfxSlider.value = PlayerPrefs.GetFloat("sfxSliderKeyName");
        //any change for volume value will be save by the methods below
        SetMasterVolumeSlider();
        SetMusicVolumeSlider();
        SetSFXVolumeSlider(); 
    }
}
