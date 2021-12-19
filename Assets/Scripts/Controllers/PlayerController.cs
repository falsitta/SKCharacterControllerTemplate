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

    public bool IsWalking()
    {
        throw new System.NotImplementedException( "IsWalking in PlayerController has not been implemented." );

    }

    public bool IsGrounded()
    {
        throw new System.NotImplementedException( "IsGrounded in PlayerController has not been implemented." );
        if (ShovelKnightInput.IsJumpPressed())
        { 
        
        }
    }

    public FacingDirection GetFacingDirection()
    {
        throw new System.NotImplementedException( "GetFacingDirection in PlayerController has not been implemented." );
    }

    // Add additional methods such as Start, Update, FixedUpdate, or whatever else you think is necessary, below.
}
