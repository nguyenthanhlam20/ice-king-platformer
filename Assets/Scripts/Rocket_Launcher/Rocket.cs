using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Rocket : MonoBehaviour
{
    // Variable declaration start-------------------------------------------------------------------
    public Transform target; //indicate the target which missile will hit
    private Rigidbody2D rb; //apply physic to the object 
    public GameObject explosionEffect; //when hit target, instantiate the explode effect

    private float speed = 3f; //speed of the missile
    public float rotateSpeed = 280f; //speed rotate of missile

    public float detonateTime = 7f;

    Collider2D[] inExplosionRadius = null; //indicate any collider within explosion radius
    [SerializeField] private float ExplosionForceMulti = 300; //indicate how strong the force when hit other objects
    [SerializeField] private float ExplosionRadius = 4; //explode radius can affect
    AudioManager audioManager;

    // Variable declaration end-------------------------------------------------------------------


    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }


    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
        rb = GetComponent<Rigidbody2D>();
        StartCoroutine(DetonateAfterTime(detonateTime));
    }

    // Update is called once per frame
    void Update()
    {
        //direction form current position of rocket toward target
        Vector2 direction = (Vector2)target.position - rb.position;

        direction.Normalize();
        //amount of rotation needed to align the missile's forward direction
        float rotateAmount = Vector3.Cross(direction, transform.right).z;
        //Sets the angular velocity of the missile's Rigidbody to rotate it towards the target
        rb.angularVelocity = -rotateAmount * rotateSpeed;

        rb.velocity = transform.right * speed;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        // var hit = GameObject.FindGameObjectWithTag("Player");
        if (collision.CompareTag("Player"))
        {
            Instantiate(explosionEffect, transform.position, transform.rotation);
            audioManager.PlaySFX(audioManager.rocketExplosion);
            Explode();
            Destroy(gameObject);

        }
        else if (collision.CompareTag("Shield"))
        {
            Instantiate(explosionEffect, transform.position, transform.rotation);
            audioManager.PlaySFX(audioManager.rocketExplosion);
            Destroy(gameObject);
        }
    }

    // explosion method 
    public void Explode()
    {
        //determine if any collider intersect of fall within 
        inExplosionRadius = Physics2D.OverlapCircleAll(transform.position, ExplosionRadius);
        foreach (Collider2D anyCollider in inExplosionRadius)
        {
            Rigidbody2D anyCollider_Rigidbody = anyCollider.GetComponent<Rigidbody2D>();
            if (anyCollider_Rigidbody != null)
            {
                //indicate the distance a target Object can be affect
                Vector2 distanceVetor = anyCollider.transform.position - transform.position;
                if (distanceVetor.magnitude > 0) //represents the distance between the explosion center and the object
                {
                    //objects closer to the explosion center receive a stronger force
                    float actualExplosion = ExplosionForceMulti * distanceVetor.magnitude;
                    //applies the calculated force to the Rigidbody2D component of target objects
                    anyCollider_Rigidbody.AddForce(distanceVetor.normalized * actualExplosion);
                }
            }
        }
    }


    // Coroutine to handle timed detonation
    private IEnumerator DetonateAfterTime(float time)
    {
        yield return new WaitForSeconds(time);
        Debug.Log("time to detonate is:" + time);
        Instantiate(explosionEffect, transform.position, transform.rotation);
        audioManager.PlaySFX(audioManager.rocketExplosion);
        Explode();
        Destroy(gameObject);
    }
}
