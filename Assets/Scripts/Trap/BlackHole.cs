using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackHole : MonoBehaviour
{
    [Header("Scripts Ref:")]
    public GrapplingHook grapplingGun;
    public GameObject whiteHole;
    private GameObject player;
    public float intensity;
    public float range;
    public float distanceBtPlayer;
    float pullForce = 4f;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void Update()
    {
        distanceBtPlayer = Vector2.Distance(player.transform.position, transform.position); ;
        if (distanceBtPlayer <= 5)
        {

            if (grapplingGun.grappleRope)
            {
                grapplingGun.StopGrappling();
            }
            Rigidbody2D rb = player.GetComponent<Rigidbody2D>();
            Vector2 direction = transform.position - player.transform.position;
            direction.Normalize();
            rb.AddForce(pullForce * direction, ForceMode2D.Force);
        }
    }

    // Update is called once per frame
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (player != null && collision.CompareTag("Player"))
        {
            player.transform.position = whiteHole.transform.position;
        }
    }
}
