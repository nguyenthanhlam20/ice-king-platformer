using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class doublejump : MonoBehaviour, IItemCollection
{
    private Rigidbody2D rb;
    [SerializeField] private Move moveScript;
    private ParticleSystem doubleJumpParticle; 
    public float boostJumpSpeed = 18f; 
    private bool canDoubleJump;
    public bool canActivateDoubleJump = false;
    AudioManager audioManager;
    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }


    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        moveScript = gameObject.GetComponent<Move>();
        doubleJumpParticle = gameObject.GetComponentInChildren<ParticleSystem>();
        canDoubleJump = false; // Ban đầu không cho phép nhảy đôi

        // Điều chỉnh số lượng hạt phát ra
        var emission = doubleJumpParticle.emission;
        emission.rateOverTime = 50f; // Đặt số lượng hạt phát ra mỗi giây
    }
    
    // Update is called once per frame
    void Update()
    {
        if (canActivateDoubleJump)
        {
            if (Input.GetKeyDown(KeyCode.Space) && !moveScript.isGrounded && canDoubleJump && canActivateDoubleJump)
            {
                // Thực hiện nhảy boost với vận tốc boostJumpSpeed
                rb.velocity = new Vector2(rb.velocity.x, boostJumpSpeed);

                doubleJumpParticle.Play();
                audioManager.PlaySFX(audioManager.doublejump);
                canDoubleJump = false;
            }

            // Reset lại khi nhân vật chạm đất
            if (moveScript.isGrounded)
            {
                canDoubleJump = true;

                // Dừng Particle System ngay lập tức khi chạm đất
                StopAndClearParticleSystem(doubleJumpParticle);
            }
        }


    }

    // Method to enable shield functionality
    private void StopAndClearParticleSystem(ParticleSystem ps)
    {
        ps.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
    }

    public void activeItem()
    {
        canActivateDoubleJump = true;
    }
}
