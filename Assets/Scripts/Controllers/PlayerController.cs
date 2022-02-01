using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    /// <summary>
    /// Your solution must make use of the following fields. If these values are changed, even at runtime,
    /// the character controller should respect the new values and act as detailed in the Unity inspector.
    /// </summary>

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
    FacingDirection facing;

    public bool IsWalking()
    {
        if (ShovelKnightInput.GetDirectionalInput().x != 0)
        {
            return true;
        }
        else {
            return false;
        }
    }

    public bool IsGrounded()
    {
        isGrounded = Physics2D.CircleCast(new Vector2(transform.position.x, transform.position.y - GetComponent<Collider2D>().bounds.extents.y), 0.2f, Vector2.down, 0, LayerMask.GetMask("Ground"));
        return isGrounded;
    }

    public FacingDirection GetFacingDirection()
    {
        if (ShovelKnightInput.GetDirectionalInput().x > 0)
        {
            
            facing = FacingDirection.Right;
        }
        else if (ShovelKnightInput.GetDirectionalInput().x < 0) {
            facing = FacingDirection.Left; ;
        }
        return facing;
    }

    // Add additional methods such as Start, Update, FixedUpdate, or whatever else you think is necessary, below.
    Rigidbody2D rigidbody;

    bool isGrounded;
    bool canJump = true;
    int jumpCount = 0;

    float currentCoyoteTime;
    float currentJumpBuffer;

    void Start() 
    {
        rigidbody = GetComponent<Rigidbody2D>();
        m_jumpApexHeight = rigidbody.gravityScale;
    }

    void Update()
    {
        //coyoteTime
        currentCoyoteTime -= Time.deltaTime;
        if (IsGrounded())
        {
            currentCoyoteTime = m_coyoteTime;
            
            if (canJump)
            {
                jumpCount = 0;
            }
        }
        else
        {
            canJump = true;
        }

        if (currentCoyoteTime > 0)
        {
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }
        //jump buffer 
        if (ShovelKnightInput.WasJumpPressed())
        {
            currentJumpBuffer = m_jumpBufferTime;
        }
        else
        {
            currentJumpBuffer -= Time.deltaTime;

        }

        //right
        if (ShovelKnightInput.GetDirectionalInput().x > 0) {
            //quick turn
            if (rigidbody.velocity.x < 0)
            {
                rigidbody.AddForce(Vector2.right * m_accelerationTimeFromRest * m_accelerationTimeFromQuickturn * Time.deltaTime, ForceMode2D.Impulse);
            }
            //go right
            rigidbody.AddForce(Vector2.right * m_accelerationTimeFromRest * Time.deltaTime, ForceMode2D.Impulse);
        }
        //left
        else if (ShovelKnightInput.GetDirectionalInput().x < 0)
        {
            //quick turn
            if (rigidbody.velocity.x > 0)
            {
                rigidbody.AddForce(Vector2.left * m_accelerationTimeFromRest * m_accelerationTimeFromQuickturn * Time.deltaTime, ForceMode2D.Impulse);
            }
            //go left
            rigidbody.AddForce(Vector2.left * m_accelerationTimeFromRest * Time.deltaTime, ForceMode2D.Impulse);
        }
        //deceleration 
        else if (ShovelKnightInput.GetDirectionalInput().x == 0)
        {
            rigidbody.AddForce(new Vector2(-rigidbody.velocity.x, 0) * m_decelerationTimeToRest * Time.deltaTime, ForceMode2D.Impulse);
        }
        //jump
        if (currentJumpBuffer > 0 && IsGrounded() && canJump && jumpCount < 1) {
            currentJumpBuffer = 0;
            jumpCount++;
            canJump = false;
            rigidbody.gravityScale = rigidbody.gravityScale - 2;
            rigidbody.velocity = new Vector2(rigidbody.velocity.x, 0);
            rigidbody.AddForce(Vector2.up * m_jumpApexTime, ForceMode2D.Impulse);
        }
        else if (!ShovelKnightInput.IsJumpPressed()) {
            rigidbody.gravityScale = m_jumpApexHeight;
        }
        //velocity
        rigidbody.velocity = new Vector2(Mathf.Clamp(rigidbody.velocity.x, -m_maxHorizontalSpeed, m_maxHorizontalSpeed), rigidbody.velocity.y);
    }
}
