using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhiteHole : MonoBehaviour
{
    [SerializeField]
    private GameObject player;
    public float intensity;
    public float distanceBtPlayer;
    Vector2 pullForce;
    // Start is called before the first frame update
   
    private void Update()
    {
        distanceBtPlayer = Vector2.Distance(player.transform.position, transform.position); ;
        if(distanceBtPlayer <= 5)
        {
            Rigidbody2D rb = player.GetComponent<Rigidbody2D>();
        /*    pullForce = (transform.position - player.transform.position).normalized / distanceBtPlayer * intensity;
            rb.AddForce(pullForce, ForceMode2D.Force);*/

            Vector2 direction = player.transform.position - transform.position;
            direction.Normalize();
            rb.AddForce(direction * 5f, ForceMode2D.Force);
        }
        
    }

    // Update is called once per frame
    
}
