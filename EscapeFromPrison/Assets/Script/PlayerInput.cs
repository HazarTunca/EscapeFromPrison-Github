using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    [HideInInspector] public bool jump, sprint, crouch, crouchDown, crouchUp;
    [HideInInspector] public float x, y;

    private void Update()
    {
        x = Input.GetAxisRaw("Horizontal");
        y = Input.GetAxisRaw("Vertical");
        jump = Input.GetButton("Jump");
        crouch = Input.GetKey(KeyCode.LeftControl);
        crouchDown = Input.GetKeyDown(KeyCode.LeftControl);
        crouchUp = Input.GetKeyUp(KeyCode.LeftControl);
    }
}
