using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackingState : PlayerBaseState
{
    private float previousFrameTime;
    private Attack attack;
    private bool alreadyAppliedForce;
    public PlayerAttackingState(PlayerStateMachine stateMachine, int attackIndex) : base(stateMachine)
    {
        attack = stateMachine.Attacks[attackIndex];
    }

    public override void Enter()
    {
        stateMachine.Weapon.SetAttack(attack.Damage, attack.Knockback);
        stateMachine.Animator.CrossFadeInFixedTime(attack.AnimationName, attack.TransitionDuration);
    }

    public override void Tick(float deltaTime)
    {
        Move(deltaTime);

        FaceTarget();

        float normalizedTime = GetNormalizedTime(stateMachine.Animator);

        if(normalizedTime < 1f)

        {
            if(normalizedTime >= attack.ForceTime)
            {
                TryApplyForce();
            }

            if(stateMachine.InputReader.IsAttacking)
            {
                TryComboAttack(normalizedTime);
            }
        }
        else
        {
            // GO BACK TO LOCOMOTION
            if(stateMachine.Targeter.CurrentTarget != null)
            {
                stateMachine.SwitchState(new PlayerTargetingState(stateMachine));
            }
            else
            {
                stateMachine.SwitchState(new PlayerFreeLookState(stateMachine));
            }
        }
        previousFrameTime = normalizedTime;
    }

    public override void Exit()
    {
        
    }

    private void TryComboAttack(float normalizedTime)
    {
        // make sure we have a combo attack
        if (attack.ComboStateIndex == -1) { return; }

        // make sure we are far enough in the animation to do it
        if(normalizedTime < attack.ComboAttackTime) { return; }

        // if we are then switch state to the attack
        stateMachine.SwitchState(new PlayerAttackingState(stateMachine, attack.ComboStateIndex));
    }

    private void TryApplyForce()
    {
        if(alreadyAppliedForce) { return; }

        stateMachine.ForceReceiver.AddForce(stateMachine.transform.forward * attack.Force);
        alreadyAppliedForce = true;
    }
}
