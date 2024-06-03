using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ragdoll : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private CharacterController characterController;

    private Collider[] allColliders;
    private Rigidbody[] allRigidbodies;

    private void Start()
    {
        // true means it will get all enabled and disabled colliders
        allColliders = GetComponentsInChildren<Collider>(true);
        allRigidbodies = GetComponentsInChildren<Rigidbody>(true);

        ToggleRagdoll(false);
    }

    public void ToggleRagdoll(bool isRagdoll)
    {
        foreach(Collider collider in allColliders)
        {
            if(collider.gameObject.CompareTag("Ragdoll"))
            {
                collider.enabled = isRagdoll;
            }
        }

        foreach (Rigidbody rb in allRigidbodies)
        {
            if (rb.gameObject.CompareTag("Ragdoll"))
            {
                // isKinematic needs to be the opposite of the passed in bool
                rb.isKinematic = !isRagdoll;
                rb.useGravity = isRagdoll;
            }
        }

        characterController.enabled = !isRagdoll;
        animator.enabled = !isRagdoll;
    }
}
