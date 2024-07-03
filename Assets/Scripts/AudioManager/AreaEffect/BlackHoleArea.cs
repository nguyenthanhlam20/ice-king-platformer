using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackHoleArea : MonoBehaviour
{
    AudioManager audioManager;
    private BlackHole blackHoleScript;
    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
        blackHoleScript = GetComponentInParent<BlackHole>();

    }
    // Start is called before the first frame update
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            audioManager.PlayBlackHoleSound();

        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))

        {
            audioManager.StopBlackHoleSound();

        }
    }
}
