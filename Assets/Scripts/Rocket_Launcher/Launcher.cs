using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Launcher : MonoBehaviour
{
    // Variable declaration start-------------------------------------------------------------------
    public GameObject missile;
    public GameObject target;
    public Transform missileInitiatePos;

    private float timer; //control missile frequency spawn

    AudioManager audioManager;

    // Variable declaration end-------------------------------------------------------------------


    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        float distance = Vector2.Distance(transform.position, target.transform.position);
        Debug.Log(distance);
        if (distance < 15)
        {
            timer += Time.deltaTime;
            if (timer > 5)//after 5 secs 
            {
                timer = 0;//reset time
                Shoot();
            }
        }
    }


    void Shoot()
    {
        Instantiate(missile, missileInitiatePos.position, Quaternion.identity);
        audioManager.PlaySFX(audioManager.laucher);
    }
}
