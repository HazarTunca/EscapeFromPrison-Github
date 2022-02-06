using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OvenMachineController : MonoBehaviour
{
    [SerializeField] private Animator animator;

    private void OnTriggerEnter(Collider other)
    {
        animator.SetTrigger("OvenOpen");
    }
}
