using System.Collections;
using System.Collections.Generic;
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

    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        currentFriction = friction;
    }

    void Update()
    {
        if (isSliding)
        {
            if (Input.GetAxisRaw("Horizontal") != 0)
            {
                StopSliding();
            }
        }
    }

    void FixedUpdate()
    {
        if (isSliding)
        {
            Vector2 slideDirection = new Vector2(transform.localScale.x, 0).normalized;
            rb.velocity = new Vector2(slideDirection.x * slideSpeed, rb.velocity.y);

            // Kiểm tra nếu đang trượt và nhấn phím A (di chuyển ngược lại)
            if (Input.GetAxisRaw("Horizontal") < 0 && !Input.GetKey(KeyCode.Space))
            {
                StopSliding();
            }
        }
        else if (onIce)
        {
            if (rb.velocity.x != 0 && !Input.GetKey(KeyCode.Space))
            {
                // Giảm dần vận tốc khi trượt trên băng, bao gồm cả khi di chuyển ngược lại
                rb.velocity = new Vector2(rb.velocity.x * 0.95f, rb.velocity.y);
            }

            // Kiểm tra và dừng trượt khi vận tốc đủ nhỏ
            if (Mathf.Abs(rb.velocity.x) < 0.1f)
            {
                StopSliding();
            }
        }
    }


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
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Ice") && rb.velocity.y < 0)
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
        isSliding = false;
        animator.SetBool("IsSliding", false);
        rb.velocity = new Vector2(0f, rb.velocity.y);
        currentFriction = friction; // Reset friction khi ngừng trượt
    }
}