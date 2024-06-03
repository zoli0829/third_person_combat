using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHangingState : PlayerBaseState
{
    private readonly int HangingHash = Animator.StringToHash("Hanging_Idle");
    private const float CrossFadeDuration = 0.1f;

    private Vector3 closestPoint;
    private Vector3 ledgeForward;

    public PlayerHangingState(PlayerStateMachine stateMachine, Vector3 ledgeForward, Vector3 closestPoint) : base(stateMachine)
    {
        this.closestPoint = closestPoint;
        this.ledgeForward = ledgeForward;
    }

    public override void Enter()
    {
        stateMachine.transform.rotation = Quaternion.LookRotation(ledgeForward, Vector3.up);
        // workaround to move the character
        stateMachine.CharacterController.enabled = false;
        stateMachine.transform.position = closestPoint - (stateMachine.LedgeDetector.transform.position - stateMachine.transform.position);
        stateMachine.CharacterController.enabled = true;

        stateMachine.Animator.CrossFadeInFixedTime(HangingHash, CrossFadeDuration);
    }

    public override void Tick(float deltaTime)
    {
        if(stateMachine.InputReader.MovementValue.y < 0)
        {
            stateMachine.CharacterController.Move(Vector3.zero);
            stateMachine.ForceReceiver.Reset();
            stateMachine.SwitchState(new PlayerFallingState(stateMachine));
        }
        else if(stateMachine.InputReader.MovementValue.y > 0) 
        {
            stateMachine.SwitchState(new PlayerPullUpState(stateMachine));
        }
    }

    public override void Exit()
    {
        
    }
}
