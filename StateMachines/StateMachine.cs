using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StateMachine : MonoBehaviour
{
    private State currentState;

    // Update is called once per frame
    private void Update()
    {
            currentState?.Tick(Time.deltaTime);
    }

    public void SwitchState(State newState)
    {
        if (currentState != null)
        { //Only do this if there is an existing state
            currentState.Exit();
        }
        //Always do the rest
        currentState = newState;
        currentState.Enter();
    }
}
