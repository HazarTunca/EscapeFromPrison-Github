using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    #region vars

    [Header("Requiements")]
    [SerializeField] private Transform playerCam;
    [SerializeField] private Transform orientation;
    [SerializeField] private PlayerWallRun wallRun;

    [Header("Sensitivity")]
    public float sensitivity = 50f;

    [SerializeField] private float xRotation;
    [SerializeField] private float sensMultiplier = 1f;
    [SerializeField] private float desiredX;

    #endregion

    private void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * sensitivity * Time.fixedDeltaTime * sensMultiplier;
        float mouseY = Input.GetAxis("Mouse Y") * sensitivity * Time.fixedDeltaTime * sensMultiplier;

        //Find current look rotation
        Vector3 rot = playerCam.transform.localRotation.eulerAngles;
        desiredX = rot.y + mouseX;

        //Rotate, and also make sure we dont over- or under-rotate.
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        //Perform the rotations
        playerCam.transform.localRotation = Quaternion.Euler(xRotation, desiredX, wallRun.tilt);
        orientation.transform.localRotation = Quaternion.Euler(0, desiredX, 0);
    }
}
