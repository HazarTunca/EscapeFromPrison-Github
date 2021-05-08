using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollisionDetections : MonoBehaviour
{
    #region vars

    [Header("IsGrounded")]
    public bool grounded;
    public LayerMask whatIsGround;

    public float maxSlopeAngle = 35f;
    public Vector3 normalVector = Vector3.up;

    private bool cancellingGrounded;

    [Header("WallRun")]
    public bool canWallRun;
    public LayerMask whatIsWall;

    private bool cancelWallRun;

    #endregion

    private void OnCollisionStay(Collision other)
    {
        CheckGround(other);
    }

    #region Checking Ground

    private void CheckGround(Collision other)
    {
        //Make sure we are only checking for walkable layers
        int layer = other.gameObject.layer;
        if (whatIsGround != (whatIsGround | (1 << layer))) return;

        //Iterate through every collision in a physics update
        for (int i = 0; i < other.contactCount; i++)
        {
            Vector3 normal = other.contacts[i].normal;
            //FLOOR
            if (IsFloor(normal))
            {
                grounded = true;
                cancellingGrounded = false;
                normalVector = normal;
                CancelInvoke(nameof(StopGrounded));
            }
        }

        //Invoke ground/wall cancel, since we can't check normals with CollisionExit
        float delay = 3f;
        if (!cancellingGrounded)
        {
            cancellingGrounded = true;
            Invoke(nameof(StopGrounded), Time.deltaTime * delay);
        }
    }

    public bool IsFloor(Vector3 v)
    {
        float angle = Vector3.Angle(Vector3.up, v);
        return angle < maxSlopeAngle;
    }

    private void StopGrounded() => grounded = false;

    #endregion
}
