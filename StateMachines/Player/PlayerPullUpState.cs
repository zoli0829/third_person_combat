using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPullUpState : PlayerBaseState
{
    private readonly int PullUpHash = Animator.StringToHash("Pull_Up");
    private const float CrossFadeDuration = 0.1f;
    // workaround for the pull up animation
    private readonly Vector3 Offset = new Vector3(0f, 2.325f, 0.65f);

    public PlayerPullUpState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        stateMachine.Animator.CrossFadeInFixedTime(PullUpHash, CrossFadeDuration);
    }

    public override void Tick(float deltaTime)
    {
        // wait until the animation finishes
        if (GetNormalizedTime(stateMachine.Animator, "Climbing") < 1f)
            return;

        // this is a workaround for the pull up animation
        stateMachine.CharacterController.enabled = false;
        stateMachine.transform.Translate(Offset, Space.Self);
        stateMachine.CharacterController.enabled = true;

        // then switch state
        stateMachine.SwitchState(new PlayerFreeLookState(stateMachine, false));
    }

    public override void Exit()
    {
        stateMachine.CharacterController.Move(Vector3.zero);
        stateMachine.ForceReceiver.Reset();
    }
}
