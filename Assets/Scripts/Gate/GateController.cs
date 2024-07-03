using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GateController : MonoBehaviour
{

    AudioManager audioManager;
    GameObject player;
    Animation anim;
    Rigidbody2D playerRb;


    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
        anim = player.GetComponent<Animation>();
        playerRb = player.GetComponent<Rigidbody2D>();

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Key"))
        {
            audioManager.PlaySFX(audioManager.changemapgate);
            StartCoroutine(PortalIn());
            CoinController.instance.ResetCoin();
            SceneController.instance.LoadNextScene();
        }

    }

    private IEnumerator PortalIn()
    {
        Debug.Log("In");
        playerRb.simulated = false;
        StartCoroutine(Shrink());
        yield return new WaitForSeconds(0.5f);
    }



    private IEnumerator Shrink()
    {
        float timeElapsed = 0;
        Vector3 targetScale = new Vector3(0f, 0f, 0f);
        Vector3 initialScale = playerRb.transform.localScale;

        while (timeElapsed < 0.5f)
        {
            player.transform.position = Vector2.MoveTowards(player.transform.position, transform.position, 3 * Time.deltaTime);
            player.transform.localScale = Vector3.Lerp(initialScale, targetScale, timeElapsed / 0.5f);
            yield return new WaitForEndOfFrame();
            timeElapsed += Time.deltaTime;
        }

    }

}