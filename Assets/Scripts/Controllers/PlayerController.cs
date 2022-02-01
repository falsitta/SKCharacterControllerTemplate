using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    /// <summary>
    /// Your solution must make use of the following fields. If these values are changed, even at runtime,
    /// the character controller should respect the new values and act as detailed in the Unity inspector.
    /// </summary>

    Rigidbody2D rb;

    public FacingDirection direction;

    float XSpeed;
    LayerMask groundMask;


    [SerializeField]
    private float m_jumpApexHeight;

    [SerializeField]
    private float m_jumpApexTime;

    [SerializeField]
    private float m_terminalVelocity;

    [SerializeField]
    private float m_coyoteTime;

    [SerializeField]
    private float m_jumpBufferTime;

    [SerializeField]
    private float m_accelerationTimeFromRest;

    [SerializeField]
    private float m_decelerationTimeToRest;

    [SerializeField]
    private float m_maxHorizontalSpeed;

    [SerializeField]
    private float m_accelerationTimeFromQuickturn;

    [SerializeField]
    private float m_decelerationTimeFromQuickturn;

    public enum FacingDirection { Left, Right }

    public bool IsWalking()
    {
        if (rb.velocity.x > 0.1 || rb.velocity.x < -0.1)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool IsGrounded()
    {
        RaycastHit2D leftRay = Physics2D.Raycast(new Vector2(transform.position.x - 0.5f, transform.position.y), -Vector2.up, 0.1f, groundMask);
        RaycastHit2D rightRay = Physics2D.Raycast(new Vector2(transform.position.x + 0.5f, transform.position.y), -Vector2.up, 0.1f, groundMask);
        if(leftRay.collider != null || rightRay.collider != null)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public FacingDirection GetFacingDirection()
    {
        return direction;
    }

    // Add additional methods such as Start, Update, FixedUpdate, or whatever else you think is necessary, below.

    void Start()
    {
        groundMask = LayerMask.GetMask("Ground");
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {

        m_coyoteTime -= Time.deltaTime;

        if (ShovelKnightInput.GetDirectionalInput().x > 0)
        {
            direction = FacingDirection.Right;
        }
        else if (ShovelKnightInput.GetDirectionalInput().x < 0)
        {
            direction = FacingDirection.Left;
        }
        else
        {
            direction = FacingDirection.Right;
        }

        //Jumping / Vertical Movements
        //Could use ShovelKnightInput.GetDirectionalInput().y == 1 (If I want the jump input as W)
        if (ShovelKnightInput.IsJumpPressed() && IsGrounded())
        {
            rb.velocity = new Vector2(0, 2 * m_jumpApexHeight / m_jumpApexTime);

            m_coyoteTime = 0.2f;
        }

        if (ShovelKnightInput.IsJumpPressed() && rb.velocity.y > 0)
        {
            rb.gravityScale = 10f;

            m_coyoteTime = 0.2f;
        }
        else if (rb.velocity.y < 0)
        {
            rb.gravityScale = 10;
        }
        else
        {
            rb.gravityScale = 10;
        }

        //coyoteTime
        if (ShovelKnightInput.WasJumpPressed() && IsGrounded() && m_coyoteTime > 0)
        {
            rb.velocity = new Vector2(0, 2 * m_jumpApexHeight / m_jumpApexTime);
        }

        if (rb.velocity.y < -m_terminalVelocity)
        {
            rb.velocity = new Vector2(rb.velocity.x, -m_terminalVelocity);
        }

        if (WillCollide() && rb.velocity.y < -m_terminalVelocity + 1)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y / 2);
        }

        //Horizontal Movement
        XSpeed = Mathf.Clamp(XSpeed + ShovelKnightInput.GetDirectionalInput().x * m_maxHorizontalSpeed / m_accelerationTimeFromRest * Time.deltaTime, -m_maxHorizontalSpeed, m_maxHorizontalSpeed);

        if (ShovelKnightInput.GetDirectionalInput().x == 0)
        {
            if (XSpeed < 0 + (m_maxHorizontalSpeed / m_decelerationTimeToRest * Time.deltaTime) && XSpeed > 0)
            {
                XSpeed = 0;
            }
            else if (XSpeed > 0 - (m_maxHorizontalSpeed / m_decelerationTimeToRest * Time.deltaTime) && XSpeed < 0)
            {
                XSpeed = 0;
            }
            else if (XSpeed > 0)
            {
                XSpeed = XSpeed - (m_maxHorizontalSpeed / m_decelerationTimeToRest * Time.deltaTime);
            }
            else if (XSpeed < 0)
            {
                XSpeed = XSpeed + (m_maxHorizontalSpeed / m_decelerationTimeToRest * Time.deltaTime);
            }
        }
        rb.velocity = new Vector2(XSpeed, rb.velocity.y);
    }

    public bool WillCollide()
    {
        RaycastHit2D leftRay = Physics2D.Raycast(new Vector2(transform.position.x - 0.12f, transform.position.y - 0.9f), -Vector2.up, Mathf.Infinity, groundMask);
        RaycastHit2D rightRay = Physics2D.Raycast(new Vector2(transform.position.x + 0.12f, transform.position.y - 0.9f), -Vector2.up, Mathf.Infinity, groundMask);
        if (leftRay.collider != null || rightRay.collider != null)
        {
            if(leftRay.collider.Distance(gameObject.GetComponent<CapsuleCollider2D>()).distance < rb.velocity.y || rightRay.collider.Distance(gameObject.GetComponent<CapsuleCollider2D>()).distance < rb.velocity.y)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }
    }
}
