using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class ForceReceiver : MonoBehaviour
{
    [SerializeField] private CharacterController controller;
    [SerializeField] private NavMeshAgent agent;

    private float verticalVelocity;
    private Vector3 impact;
    private Vector3 dampingVelocity;
    [SerializeField] private float drag = 0.01f;

    public Vector3 Movement => impact + Vector3.up * verticalVelocity;

    private void Update()
    {
        // handling gravity
        if (verticalVelocity < 0f && controller.isGrounded)
        {
            verticalVelocity = Physics.gravity.y * Time.deltaTime;
        }
        else
        {
            verticalVelocity += Physics.gravity.y * Time.deltaTime;
        }

        impact = Vector3.SmoothDamp(impact, Vector3.zero, ref dampingVelocity, drag);

        if (agent != null)
        {
            if (impact.sqrMagnitude < 0.2f * 0.2f)
            {
                impact = Vector3.zero;
                agent.enabled = true;
            }
        }
    }

    public void AddForce(Vector3 force)
    {
        impact += force;
        // disable the navmeshagent when we add knockback force to the enemy
        if(agent != null)
        {
            agent.enabled = false;
        }
    }

    public void Jump(float jumpForce)
    {
        verticalVelocity += jumpForce;
    }

    public void Reset()
    {
        verticalVelocity = 0;
        impact = Vector3.zero;
    }
}
