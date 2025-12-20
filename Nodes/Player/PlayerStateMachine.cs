using System;
using Godot;
using StateManagement;

public abstract class IPlayerState : IState
{
  protected PlayerData DataRef;
  protected IPlayerState(State thisState, PlayerData Data) : base(thisState)
  {
    DataRef = Data;
  }

  public override void Act(float delta)
  {
    
  }

  public virtual void ProcessMove(float delta, float fbAxis, float lrAxis)
  {
  }

  public virtual void ProcessTurn(float delta, float lrAxis)
  {
    
  }

  public virtual void ProcessLook(float delta, float TurnInput)
  {
    
  }

  public virtual void ProcessInventory(float delta)
  {
    
  }

  public virtual void ProcessInteract(float delta)
  {
    
  }  
}

public class PlayerGroundedState : IPlayerState
{
  public PlayerGroundedState(State thisState, PlayerData Data) : base(thisState, Data)
  {
  }

  public override void EnterState()
  {
    
  }

  public override void ExitState()
  {
    
  }


  public override void Act(float delta)
  {
    
  }

  public override void ProcessMove(float delta, float fbAxis, float lrAxis)
  {
    DataRef.CurrentVelocity = DataRef.MoveSpeed * (DataRef.CurrentDirection.Z * fbAxis + DataRef.CurrentDirection.X * lrAxis);
  }

  public override void ProcessTurn(float delta, float lrAxis)
  {
    DataRef.CurrentRotationRate = DataRef.LookSpeed * lrAxis;
  }

  public override void ProcessLook(float delta, float TurnInput)
  {
    
  }

  public override void ProcessInventory(float delta)
  {
    
  }

  public override void ProcessInteract(float delta)
  {
    
  }  
}
