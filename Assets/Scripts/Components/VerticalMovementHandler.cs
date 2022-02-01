using UnityEngine;
using System.Collections;

public class VerticalMovementHandler : MonoBehaviour
{
    public float ApplyGravity( float currentVelocity, float acceleration, float minVelocity )
    {
        return Mathf.Max(currentVelocity + acceleration * Time.deltaTime, minVelocity);
    }

    public float CalculateJumpVelocity( float targetHeight, float targetTime )
    {
        return 2 * targetHeight / targetTime;
    }

    public float CalculateJumpUpAcceleration( float targetHeight, float targetTime )
    {
        return -2 * targetHeight / (targetTime * targetTime);
    }

    public float CalculateFallAcceleration( float targetVelocity, float targetTime )
    {
        return targetVelocity / targetTime;
    }

    public bool GroundCheck( BoxCollider2D collider, Vector2 position, float currentVelocity, int layerMask, out RaycastHit2D hit )
    {
        hit = Physics2D.BoxCast(position + collider.offset, collider.size, 0, Vector2.down, currentVelocity * Time.deltaTime, layerMask);
        return hit.collider != null;
    }
}
