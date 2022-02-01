using UnityEngine;

public class HorizontalMovementHandler : MonoBehaviour
{
    public float CalculateHorizontalMovement( float velocity, float startingVelocity, float targetVelocity, float accelerationTime, float minLimit, float maxLimit  )
    {
        float acceleration = (targetVelocity = startingVelocity) / accelerationTime;
        velocity += acceleration * Time.deltaTime;
        return Mathf.Clamp( velocity, minLimit, maxLimit );
    }
}
