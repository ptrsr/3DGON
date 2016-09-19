using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum State
{
    Opening,
    Playing,
    Menu,
    Stopping,
    terminated
}

public enum Command
{
    Stop,
    Play,
    Continue,
    idle
}

public class StateMachine
{
    class StateTransition
    {
        readonly State CurrentState;
        readonly Command Command;

        public StateTransition(State currentState, Command command)
        {
            CurrentState = currentState;
            Command = command;
        }

        public override int GetHashCode()
        {
            return 17 + 31 * CurrentState.GetHashCode() + 31 * Command.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            StateTransition other = obj as StateTransition;
            return other != null && this.CurrentState == other.CurrentState && this.Command == other.Command;
        }
    }

    Dictionary<StateTransition, State> transitions;
    public State CurrentState { get; private set; }
    private State lastState;

    public StateMachine()
    {
        CurrentState = State.Opening;      //Starting state
        lastState = CurrentState;

        transitions = new Dictionary<StateTransition, State> 
        {
            { new StateTransition(State.Opening, Command.Continue), State.Menu },
            { new StateTransition(State.Menu, Command.Play), State.Playing },
            { new StateTransition(State.Menu, Command.Stop), State.terminated},
            { new StateTransition(State.Playing, Command.Stop), State.Stopping},
            { new StateTransition(State.Stopping, Command.Continue), State.Menu}
        };
    }

    private State GetNext(Command command)
    {
        StateTransition transition = new StateTransition(CurrentState, command);
        State NextState;

        if (!transitions.TryGetValue(transition, out NextState))
            Debug.LogWarning("Invalid Transition: " + CurrentState + " -> " + command);
        return NextState;
    }

    public State MoveNext(Command command)
    {
        CurrentState = GetNext(command);
        return CurrentState;
    }

    public bool StateChanged()
    {
        if (CurrentState == lastState)
            return false;

        lastState = CurrentState;
        return true;
    }
}
