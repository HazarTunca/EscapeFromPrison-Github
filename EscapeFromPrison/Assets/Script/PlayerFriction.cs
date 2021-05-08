using System;
using UnityEngine;

public class PlayerFriction : MonoBehaviour
{
    #region vars

    // Requirements
    private PlayerInput inputs;
    private PlayerCamera cam;
    private PlayerMovement movement;
    private PlayerCollisionDetections col;

    [Header("Friction")]
    public float counterMovement = 0.175f;
    public float slideCounterMovement = 0.05f;
    private float threshold = 0.01f;

    #endregion

    private void Awake()
    {
        inputs = GetComponent<PlayerInput>();
        cam = GetComponent<PlayerCamera>();
        movement = GetComponent<PlayerMovement>();
        col = GetComponent<PlayerCollisionDetections>();
    }

    public void CounterMovement(float x, float y, Vector2 mag)
    {
        if (!col.grounded || inputs.jump) return;

        //Slow down sliding
        if (inputs.crouch)
        {
            movement.rb.AddForce(movement.moveSpeed * Time.deltaTime * -movement.rb.velocity.normalized * slideCounterMovement);
            return;
        }

        //Counter movement
        if (Math.Abs(mag.x) > threshold && Math.Abs(x) < 0.05f || (mag.x < -threshold && x > 0) || (mag.x > threshold && x < 0))
        {
            movement.rb.AddForce(movement.moveSpeed * cam.orientation.transform.right * Time.deltaTime * -mag.x * counterMovement);
        }
        if (Math.Abs(mag.y) > threshold && Math.Abs(y) < 0.05f || (mag.y < -threshold && y > 0) || (mag.y > threshold && y < 0))
        {
            movement.rb.AddForce(movement.moveSpeed * cam.orientation.transform.forward * Time.deltaTime * -mag.y * counterMovement);
        }

        //Limit diagonal running. This will also cause a full stop if sliding fast and un-crouching, so not optimal.
        if (Math.Sqrt((Math.Pow(movement.rb.velocity.x, 2) + Math.Pow(movement.rb.velocity.z, 2))) > movement.maxSpeed)
        {
            float fallspeed = movement.rb.velocity.y;
            Vector3 n = movement.rb.velocity.normalized * movement.maxSpeed;
            movement.rb.velocity = new Vector3(n.x, fallspeed, n.z);
        }
    }

    /// <summary>
    /// Find the velocity relative to where the player is looking
    /// Useful for vectors calculations regarding movement and limiting movement
    /// </summary>
    /// <returns></returns>
    public Vector2 FindVelRelativeToLook()
    {
        float lookAngle = cam.orientation.transform.eulerAngles.y;
        float moveAngle = Mathf.Atan2(movement.rb.velocity.x, movement.rb.velocity.z) * Mathf.Rad2Deg;

        float u = Mathf.DeltaAngle(lookAngle, moveAngle);
        float v = 90 - u;

        float magnitue = movement.rb.velocity.magnitude;
        float yMag = magnitue * Mathf.Cos(u * Mathf.Deg2Rad);
        float xMag = magnitue * Mathf.Cos(v * Mathf.Deg2Rad);

        return new Vector2(xMag, yMag);
    }
}
