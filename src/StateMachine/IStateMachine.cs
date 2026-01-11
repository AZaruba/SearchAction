using System.Collections.Generic;
using Godot;
using StateManagement;

public class StateMachine<T> where T : IState
{
	public StateMachine() {
		StateLookup = new Dictionary<State, T>();
	}
	private Dictionary<State, T> StateLookup;
	private T CurrentState;

	private bool Paused = false;

	public void Start(State initialState)
	{
    CurrentState = StateLookup[initialState];
	}
	
	public void AddState(State stateName, T state)
	{
		StateLookup.Add(stateName, state);
	}
	public void AddTransition(State current, Command command, State next)
	{
		if (!StateLookup.ContainsKey(current) || !StateLookup.ContainsKey(next))
		{
      throw new System.Exception();
		}
		StateLookup[current].AddTransition(command, next);
	}


  public void Execute(Command command)
	{
		State newState = CurrentState.ChangeState(command);
		if (newState == State.ERROR_STATE)
		{
			throw new System.Exception();
		}
		if (!StateLookup.ContainsKey(newState))
		{
			throw new System.Exception();
		}

		// short circuit if it is the current state
		if (StateLookup[newState] == CurrentState) return;

    // end current state, then move to the new one
    CurrentState.ExitState();
    CurrentState = StateLookup[newState];
		CurrentState.EnterState();
	}

	 public virtual void Pause() { Paused = true; }
	 public virtual void UnPause() { Paused = false; }
	 public virtual bool IsPaused() { return Paused; }

	 public virtual T GetCurrentState() { return CurrentState; }
	 public virtual void SetCurrentState(T nextState) { CurrentState = nextState; }

	 public void PrintCurrentState() { CurrentState.PrintCurrentState(); }
}

public abstract class IState {
	private Dictionary<Command, State> Transitions;
	private State thisState;

	public IState(State thisState) {
		Transitions = new Dictionary<Command, State>();
		this.thisState = thisState;
	}

	public virtual void AddTransition(Command command, State state)
	{
		if (Transitions.ContainsKey(command)) return;

		Transitions.Add(command, state);
	}

	public virtual State ChangeState(Command command)
	{
    if (!Transitions.ContainsKey(command)) return thisState;

		return Transitions[command];
	}
  public abstract void ExitState();

	public abstract void EnterState();

	public virtual void Act(float delta) { /* by default do nothing! */ }

	public void PrintCurrentState() { GD.Print(thisState); }
	public State Key() { return thisState; }
}