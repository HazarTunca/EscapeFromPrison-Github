using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpPadController : MonoBehaviour
{
    #region vars

    [SerializeField] private Animator animator;
    [SerializeField] private float jumpForce = 45f;
    [SerializeField] private float occurCollisionTimeTreshold = 2.5f;

    private Transform target;
    private bool debug;
    [SerializeField] private float occurCollisionTime = 0f;

    #endregion

    private void Update()
    {
        if (debug)
        {
            occurCollisionTime += Time.deltaTime;
            if (occurCollisionTime >= occurCollisionTimeTreshold)
            {
                Physics.IgnoreCollision(transform.GetComponent<Collider>(), target.GetComponent<Collider>(), false);
                debug = false;
                target = null;
                occurCollisionTime = 0f;
            }
        }
        else
        {
            occurCollisionTime = 0f;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        animator.SetTrigger("Jump");

        target = collision.transform;

        if (target.GetComponent<Rigidbody>() != null)
        {
            Rigidbody rb = target.GetComponent<Rigidbody>();
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);

            Physics.IgnoreCollision(transform.GetComponent<Collider>(), target.GetComponent<Collider>(), true);
            debug = true;
        }
    }
}
