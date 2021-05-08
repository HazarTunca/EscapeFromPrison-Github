using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWallRun : MonoBehaviour
{
    private Rigidbody rb;
    public Transform orientation;

    public float wallDistance = 0.5f;
    public float minimumJumpHeight = 1.5f;

    public float wallRunGravity = 5f;
    public float wallJumpForce = 550f;

    public float wallStickForce = 50f;
    public float wallStickTime = 5f;
    
    bool wallLeft = false;
    bool wallRight = false;

    RaycastHit leftWallHit;
    RaycastHit rightWallHit;

    [Header("Camera Tilt")]
    public Camera cam;
    public float fov;
    public float wallRunFov;
    public float wallRunFovTime;
    public float camTilt;
    public float camTiltTime;

    public float tilt { get; private set; }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        CheckWall();

        if (CanWallRun())
        {
            if (wallLeft)
            {
                StartWallRun();
            }
            else if (wallRight)
            {
                StartWallRun();
            }
            else
            {
                StopWallRun();
            }
        }
        else
        {
            StopWallRun();
        }
    }

    private bool CanWallRun()
    {
        return !Physics.Raycast(transform.position, Vector3.down, minimumJumpHeight);
    }

    private void CheckWall()
    {
        wallLeft = Physics.Raycast(transform.position, -orientation.right, out leftWallHit, wallDistance);
        wallRight = Physics.Raycast(transform.position, orientation.right, out rightWallHit, wallDistance);
    }

    private void StartWallRun()
    {
        rb.useGravity = false;

        rb.AddForce(Vector3.down * wallRunGravity, ForceMode.Force);

        cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, wallRunFov, wallRunFovTime * Time.deltaTime);

        rb.velocity = new Vector3(rb.velocity.x, Mathf.Lerp(rb.velocity.y, 0f, wallStickTime * Time.deltaTime), rb.velocity.z);
        
        if(wallLeft)
        {
            tilt = Mathf.Lerp(tilt, -camTilt, camTiltTime * Time.deltaTime);
            rb.AddForce(-orientation.right * wallStickForce, ForceMode.Force);
        }
        else if (wallRight)
        {
            tilt = Mathf.Lerp(tilt, camTilt, camTiltTime * Time.deltaTime);
            rb.AddForce(orientation.right * wallStickForce, ForceMode.Force);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (wallLeft)
            {
                Vector3 wallRunJumpDirection = transform.up + leftWallHit.normal;
                rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
                rb.AddForce(wallRunJumpDirection * wallJumpForce, ForceMode.Force);
            }
            else if (wallRight)
            {
                Vector3 wallRunJumpDirection = transform.up + rightWallHit.normal;
                rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
                rb.AddForce(wallRunJumpDirection * wallJumpForce, ForceMode.Force);
            }
        }
    }
    
    private void StopWallRun()
    {
        rb.useGravity = true;
        cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, fov, wallRunFovTime * Time.deltaTime);
        tilt = Mathf.Lerp(tilt, 0, camTiltTime * Time.deltaTime);
    }
}
