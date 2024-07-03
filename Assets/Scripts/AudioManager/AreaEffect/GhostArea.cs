using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostArea : MonoBehaviour
{
    AudioManager audioManager;
    private GhostController ghostController;
    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
        ghostController = GetComponentInParent<GhostController>();

    }
    // Start is called before the first frame update
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            audioManager.PlayGhostSound();
         
        }
        if (ghostController != null && ghostController.stand.enabled)
        {
            audioManager.PlayGhostSound();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))

        {
            audioManager.StopGhostSound();
                      
        }
        if (ghostController != null && !ghostController.stand.enabled)
        {
            audioManager.StopGhostSound();
        }
    }
}
