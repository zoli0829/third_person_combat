using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpingState : PlayerBaseState
{
    private readonly int JumpHash = Animator.StringToHash("Jump_Up");
    private const float CrossFadeDuration = 0.1f;
    private Vector3 momentum;

    public PlayerJumpingState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        stateMachine.ForceReceiver.Jump(stateMachine.JumpForce);

        momentum = stateMachine.CharacterController.velocity;
        momentum.y = 0;

        stateMachine.Animator.CrossFadeInFixedTime(JumpHash, CrossFadeDuration);

        stateMachine.LedgeDetector.OnLedgeDetect += OnLedgeDetect;
    }

    public override void Tick(float deltaTime)
    {
        Move(momentum, deltaTime);

        if(stateMachine.CharacterController.velocity.y <= 0 )
        {
            stateMachine.SwitchState(new PlayerFallingState(stateMachine));
            return;
        }

        FaceTarget();
    }


    public override void Exit()
    {
        stateMachine.LedgeDetector.OnLedgeDetect -= OnLedgeDetect;
    }

    private void OnLedgeDetect(Vector3 ledgeForward, Vector3 closestPoint)
    {
        stateMachine.SwitchState(new PlayerHangingState(stateMachine, ledgeForward, closestPoint));
    }
}
