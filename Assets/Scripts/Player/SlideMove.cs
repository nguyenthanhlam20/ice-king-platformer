using System;
using System.Collections;
using UnityEngine;

public class SlideMove : MonoBehaviour
{
    public float slideSpeed = 10f;
    public float friction = 0.5f;
    public bool isSliding = false;

    private Rigidbody2D rb;
    private Animator animator;
    private bool onIce = false;
    private float currentFriction;

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
    public bool canDoubleJump; // kiểm tra trạng thái nhảy

    AudioManager audioManager;
    private bool isMoving;
    private float airTime = 0f; // Thời gian ở trên không
    private float minAirTime = 0.5f; // Thời gian tối thiểu trên không để phát âm thanh tiếp đất

    private bool isStop = true;
    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    // Start is called before the first frame update
    void Start()
    {
        Footcollider.sharedMaterial = normalMa;
        bodycollider.sharedMaterial = bounceMa;
        rb.gravityScale = 6f;
        currentFriction = friction;
    }

    // Update is called once per frame
    void Update()
    {
        // Stop sliding when click arrow opposite site
        if (isSliding && IsOppositeSite()) StopSliding();
        if (Input.GetKey(KeyCode.Space))
        {
            isSliding = false;
            animator.SetBool("IsSliding", false);
        }

        moveInput = Input.GetAxisRaw("Horizontal");
        animator.SetBool("IsJumping", !isGrounded);
        //if (!isGrounded) animator.SetFloat("yVelocity", rb.velocity.y);

        if (moveInput != 0 && !isMoving && jumpSpeed == 0f && isGrounded)
        {
            if (!isSliding && onIce) StartSliding();
            StopAllCoroutines();
            StartCoroutine(CanMoving());
            isMoving = true;
            audioManager.PlayWalk(); // Phát âm thanh walk khi bắt đầu di chuyển
        }
        else if (isMoving && (moveInput == 0 || jumpSpeed > 0.0f || !isGrounded))
        {
            isMoving = false;
            audioManager.StopWalk(); // Dừng âm thanh walk khi dừng di chuyển
            rb.velocity = new Vector2(0f, rb.velocity.y);
            animator.SetFloat("Movement", 0);
        }

        if (jumpSpeed == 0f && isGrounded && !isStop)
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

        isGrounded = Physics2D.OverlapBox(new Vector2(transform.position.x, transform.position.y - 0.64f)
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
            var direction = facingRight ? 1 : -1;
            if (moveInput != 0)
            {
                Flip();
                direction = (int)Mathf.Sign(Input.GetAxisRaw("Horizontal"));
            }
            float tempx = direction * walkSpeed * jumpDistance;
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

        //if (rb.velocity.y < 0) animator.SetBool("IsRecharge", false);
    }

    private IEnumerator CanMoving()
    {
        yield return new WaitForSeconds(0.15f);
        isStop = false;
    }
    void FixedUpdate()
    {
        if (isSliding && !isMoving)
        {
            Vector2 slideDirection = new Vector2(transform.localScale.x, 0).normalized;
            rb.velocity = new Vector2(slideDirection.x * slideSpeed, rb.velocity.y);
        }
        else if (onIce)
        {
            if (rb.velocity.x != 0 && !Input.GetKey(KeyCode.Space) && !isStop)
            {
                // Giảm dần vận tốc khi trượt trên băng, bao gồm cả khi di chuyển ngược lại
                rb.velocity = new Vector2(rb.velocity.x * 0.95f, rb.velocity.y);
            }

            // Kiểm tra và dừng trượt khi vận tốc đủ nhỏ
            if (Mathf.Abs(rb.velocity.x) < 0.1f && IsOppositeSite())
            {
                StopSliding();
            }
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
        if (isStop) return;
        Flip();
    }

    private void Flip()
    {
        if ((facingRight && moveInput < 0f) || (!facingRight && moveInput > 0f))
        {
            var playerScale = transform.localScale;
            transform.localScale = new Vector3(playerScale.x * -1, playerScale.y, playerScale.z);
            facingRight = !facingRight;
        }
            
    }

    private bool IsOppositeSite()
     => Mathf.Sign(transform.localScale.x) != Mathf.Sign(Input.GetAxisRaw("Horizontal"))
        && Input.GetAxisRaw("Horizontal") != 0;

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Ice"))
        {
            onIce = true;
            if (!isSliding && rb.velocity.y < 0)
            {
                StartSliding();
            }
        }

        // Uncomment this 
        //if(collision.collider.CompareTag("Ground"))
        //{
        //    onIce = false;
        //    StopSliding();
        //}
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Ice") && rb.velocity.y < 0 && !onIce)
        {
            // Nhân vật đang rơi xuống và chạm vào băng, bắt đầu trượt về đúng hướng nhảy lên
            if (!isSliding)
            {
                StartSliding();
            }
            else
            {
                // Đảm bảo vận tốc trượt theo hướng nhảy lên
                Vector2 slideDirection = new Vector2(transform.localScale.x, 0).normalized;
                rb.velocity = new Vector2(slideDirection.x * slideSpeed, rb.velocity.y);
            }
        }

    }
    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Ice"))
        {
            onIce = false;
        }
    }

    void StartSliding()
    {
        isSliding = true;
        animator.SetBool("IsSliding", true);
        // Đặt vận tốc trượt theo hướng nhảy lên
        Vector2 slideDirection = new Vector2(transform.localScale.x, 0).normalized;
        rb.velocity = new Vector2(slideDirection.x * slideSpeed, rb.velocity.y);
        currentFriction = friction; // Reset current friction
    }

    void StopSliding()
    {
        isStop = true;
        isSliding = false;
        animator.SetBool("IsSliding", false);
        rb.velocity = new Vector2(0f, rb.velocity.y);
        currentFriction = friction; // Reset friction khi ngừng trượt
    }

    public void LoadData(GameData gameData)
    {
        if (gameData.playerPosition == new Vector3()) return;
        gameObject.transform.position = gameData.playerPosition;
    }

    public void SaveData(ref GameData gameData)
    {
        if (this != null) gameData.playerPosition = this.transform.position;
    }
}