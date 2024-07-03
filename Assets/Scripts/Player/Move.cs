using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour, IDataAction
{

    public float jumpSpeed = 0.0f;
    public bool isGrounded;
    public LayerMask groundMask;
    public bool facingRight = true;



    public Collider2D bodycollider;
    public Collider2D Footcollider;
    public PhysicsMaterial2D normalMa;
    public PhysicsMaterial2D bounceMa;

    private bool coinCollected = false;
    public bool canJump = true;
    private float jumpDistance = 2f;
    private float walkSpeed = 8f;
    private float moveInput;
    private Rigidbody2D rb;
    private Animator animator;
    public bool canDoubleJump; // kiểm tra trạng thái nhảy


    AudioManager audioManager;
    private bool isMoving;
    private float airTime = 0f; // Thời gian ở trên không
    private float minAirTime = 0.5f; // Thời gian tối thiểu trên không để phát âm thanh tiếp đất
    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }


    // Start is called before the first frame update
    void Start()
    {
        animator = gameObject.GetComponent<Animator>();
        rb = gameObject.GetComponent<Rigidbody2D>();
        Footcollider.sharedMaterial = normalMa;
        bodycollider.sharedMaterial = bounceMa;
        rb.gravityScale = 6f;
    }

    // Update is called once per frame
    void Update()
    {
        moveInput = Input.GetAxisRaw("Horizontal");
        animator.SetBool("IsJumping", !isGrounded);
        animator.SetFloat("yVelocity", rb.velocity.y);


        if (moveInput != 0 && !isMoving && jumpSpeed == 0.0f && isGrounded)
        {
            isMoving = true;
            audioManager.PlayWalk(); // Phát âm thanh walk khi bắt đầu di chuyển
        }
        else if (isMoving && (moveInput == 0 || jumpSpeed > 0.0f || !isGrounded))
        {
            isMoving = false;
            audioManager.StopWalk(); // Dừng âm thanh walk khi dừng di chuyển
        }
        

        if (jumpSpeed == 0.0f && isGrounded)
        {
            CheckFacingDirection();
            animator.SetFloat("Movement", Mathf.Abs(rb.velocity.x));
            rb.velocity = new Vector2(moveInput * walkSpeed, rb.velocity.y);

        }

        if (isGrounded)
        {
            
            canDoubleJump = false; // Đặt lại trạng thái nhảy khi tiếp đất
            if (airTime >= minAirTime)
            {
                audioManager.PlaySFX(audioManager.fall);
            }

            // Đặt lại thời gian trên không
            airTime = 0f;
        }
        else
        {
            bodycollider.sharedMaterial = bounceMa;
            

            // Tăng thời gian ở trên không
            airTime += Time.deltaTime;
        }

        isGrounded = Physics2D.OverlapBox(new Vector2(gameObject.transform.position.x, gameObject.transform.position.y - 0.64f)
            , new Vector2(1.163f, 0.03f), 0f, groundMask);
        if (Input.GetKey(KeyCode.Space) && isGrounded && canJump)
        {
            CheckFacingDirection();
            jumpSpeed += Time.deltaTime * 60f;
            animator.SetBool("IsRecharge", true);
            canDoubleJump = true; // Đặt lại trạng thái nhảy khi đang ở trên mặt đất

        }

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded && canJump)
        {
            rb.velocity = new Vector2(0.0f, rb.velocity.y);
            animator.SetFloat("Movement", Mathf.Abs(rb.velocity.x));
           
        }

        if (jumpSpeed >= 30f && isGrounded)
        {
            float tempx = moveInput * walkSpeed * jumpDistance;
            float tempy = jumpSpeed;

            animator.SetBool("IsRecharge", false);
            rb.velocity = new Vector2(tempx, tempy);
            canJump = false;
            canDoubleJump = true; // Đặt trạng thái nhảy khi nhảy lên
            audioManager.PlaySFX(audioManager.jump);

        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            if (isGrounded)
            {
                rb.velocity = new Vector2(moveInput * walkSpeed * jumpDistance, jumpSpeed);
                animator.SetBool("IsRecharge", false);
                canDoubleJump = true; // Đặt lại trạng thái nhảy khi đang ở trên mặt đất
                audioManager.PlaySFX(audioManager.jump);
            }
            canJump = true;
           
        }

        if(rb.velocity.y < 0)
        {
            animator.SetBool("IsRecharge", false);
        }
    }




    private void LateUpdate()
    {
        if (!isGrounded)
        {
            jumpSpeed = 0f;             
            canDoubleJump = true; // Đặt lại trạng thái nhảy khi đang ở trên mặt đất

        }
        else
        {
            canDoubleJump = false; // Đặt lại trạng thái nhảy khi đang ở trên mặt đất
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            canJump = true;

        }
    }







    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawCube(new Vector2(gameObject.transform.position.x, gameObject.transform.position.y - 0.64f), new Vector2(1.163f, 0.03f));

    }

    public void CheckFacingDirection()
    {
        if (facingRight && moveInput < 0f)
        {
            Flip();
        }
        else if (!facingRight && moveInput > 0f)
        {
            Flip();
        }
    }
    private void Flip()
    {
        var playerScale = transform.localScale;
        transform.localScale = new Vector3(playerScale.x * -1, playerScale.y, playerScale.z);
        facingRight = !facingRight;
    }

    public void LoadData(GameData gameData)
    {
        if(gameData.playerPosition == new Vector3())
        {
            return;
        }
        gameObject.transform.position = gameData.playerPosition;
    }

    public void SaveData(ref GameData gameData)
    {
        if(this != null)
        {
            gameData.playerPosition = this.transform.position;
        }
    }

    // OnCollisionEnter2D is called when this object collides with another object
    /*    private void OnCollisionEnter2D(Collision2D collision)
        {
            // Check if the player collides with a ground object (e.g., platform, floor)
            if (collision.gameObject.CompareTag("Ground"))
            {
                jumpSpeed = 0.0f;
            }
        }*/
}