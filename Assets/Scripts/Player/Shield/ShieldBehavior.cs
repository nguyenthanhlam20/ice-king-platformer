using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldBehavior : MonoBehaviour, IItemCollection
{


    //variable to reference to the shiled in game
    [SerializeField] private GameObject shield;
    //check if character is shielded or not
    private bool isShielded;
    private bool canActivateShield;
    private bool isShieldBrokenSoundPlayed;
    AudioManager audioManager;
    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }



    void Start()
    {
        isShielded = false;
        shield.SetActive(false);
        canActivateShield = false; //cannot active shield when player does not collect item
        isShieldBrokenSoundPlayed = false;
    }

    void Update()
    {
        if (canActivateShield)
        {
            CheckShield();
        }

    }

    public void CheckShield() //check if shield is actived
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) && !isShielded)
        {
            shield.SetActive(true);
            isShielded = true;
            audioManager.PlaySFX(audioManager.shieldactived);
            Debug.Log("Shield Actived");
            isShieldBrokenSoundPlayed = false;
        }
    }

    void NoShield()
    {
        shield.SetActive(false);
        isShielded = false;
        // Only play the shield broken sound if it hasn't been played yet
        if (!isShieldBrokenSoundPlayed)
        {
            audioManager.PlaySFX(audioManager.shieldbroken);
            isShieldBrokenSoundPlayed = true;
        }

    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Rocket"))
        {
            Debug.Log("missile collision detect");
            //turning of the shield
            Invoke("NoShield", 0f);
        }
    }

    public void activeItem()
    {
        canActivateShield = true;
    }
}
