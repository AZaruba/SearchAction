using System;
using System.Diagnostics;
using Authorship.src;
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

public class PlayerSlidingState : IPlayerState
{
    public PlayerSlidingState(State thisState, PlayerData Data) : base(thisState, Data)
    {
    }


    public override void EnterState()
    {
      DataRef.CurrentRotationRate = 0;
      DataRef.CurrentDirection = Basis.LookingAt(DataRef.Position.DirectionTo(DataRef.SlidingTarget), Vector3.Up);
      DataRef.CurrentVelocity = DataRef.SlideVelocity * -DataRef.CurrentDirection.Z;
    }

    public override void ExitState()
    {
        DataRef.SlidingTarget = Vector3.Zero;
    }

    public override void Act(float delta)
    {
      
    }
    public override void ProcessTurn(float delta, float lrAxis)
    {
      
    }
}

public class PlayerSwimmingState : IPlayerState
{
  public PlayerSwimmingState(State thisState, PlayerData Data) : base(thisState, Data)
  {
  }

  public override void EnterState()
  {
    GD.Print("Enter swimming");
  }

  public override void ExitState()
  {
    DataRef.SwimmingRate = Vector3.Zero;
  }


  public override void Act(float delta)
  {

    if (ProgressTracker.GetEquippedItem(ItemCategory.Hat) == ItemID.DiveMask)
    {
      if (Input.IsActionPressed(InputActions.SwimUp))
      {
        DataRef.SwimmingRate = Vector3.Up * Mathf.Min(6 * DataRef.CurrentToolSwimModifier, DataRef.SwimmingRate.Y + delta * 3);
        if (DataRef.Position.Y >= DataRef.WaterVolumeSurface.Y)
        {
          DataRef.SwimmingRate = Vector3.Down * (DataRef.Position.Y - DataRef.WaterVolumeSurface.Y);
        }
      }
      else if (Input.IsActionPressed(InputActions.SwimDown))
      {
        DataRef.SwimmingRate = Vector3.Up * Mathf.Min(6 * DataRef.CurrentToolSwimModifier, DataRef.SwimmingRate.Y - delta * 3);
      }
      else
      {
        DataRef.SwimmingRate =  Vector3.Up * Mathf.Clamp(Mathf.Abs(DataRef.SwimmingRate.Y) - 5 * delta, 0, 6) * Mathf.Sign(DataRef.SwimmingRate.Y);
      }
    }
    else
    {
      if (DataRef.Position.Y < DataRef.WaterVolumeSurface.Y)
      {
        DataRef.SwimmingRate = Vector3.Up * Mathf.Min(6 * DataRef.CurrentToolSwimModifier, DataRef.SwimmingRate.Y + delta * 5);
      } 
      else
      {
          DataRef.SwimmingRate = Vector3.Down * Mathf.Round(DataRef.Position.Y - DataRef.WaterVolumeSurface.Y);
      }
    }
  }

  public override void ProcessMove(float delta, float fbAxis, float lrAxis)
  {
    
    Vector3 Direction = DataRef.CurrentDirection.Z * fbAxis + DataRef.CurrentDirection.X * lrAxis;
    float CurrentMoveSpeed = DataRef.CurrentVelocity.Length();
    if (Direction.Length() != 0)
    {
      DataRef.CurrentVelocity += DataRef.MoveAcceleration * Direction * delta * 0.1f;
      DataRef.CurrentVelocity = DataRef.CurrentVelocity.LimitLength(DataRef.MoveSpeed * DataRef.CurrentToolSwimModifier);
    }
    else
    {
      float NewSpeed = Mathf.Max(CurrentMoveSpeed - DataRef.MoveAcceleration * delta * 0.2f, 0);
      DataRef.CurrentVelocity = DataRef.CurrentVelocity.Normalized() * NewSpeed;
    }
  }

  public override void ProcessTurn(float delta, float lrAxis)
  {
    if (lrAxis != 0)
    {
      DataRef.CurrentRotationRate += DataRef.TurnAcceleration * delta * lrAxis;
      DataRef.CurrentRotationRate = Mathf.Clamp(Mathf.Abs(DataRef.CurrentRotationRate), 0, DataRef.TurnSpeed) * Mathf.Sign(DataRef.CurrentRotationRate);
    } else
    {
      DataRef.CurrentRotationRate = Mathf.Clamp(Mathf.Abs(DataRef.CurrentRotationRate) - DataRef.TurnAcceleration * delta, 0, DataRef.TurnSpeed) * Mathf.Sign(DataRef.CurrentRotationRate);
    }
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

public class PlayerGroundedState : IPlayerState
{
  public PlayerGroundedState(State thisState, PlayerData Data) : base(thisState, Data)
  {
  }

  public override void EnterState()
  {
    DataRef.SwimmingRate = Vector3.Zero;
  }

  public override void ExitState()
  {
    
  }


  public override void Act(float delta)
  {
    
  }

  public override void ProcessMove(float delta, float fbAxis, float lrAxis)
  {
    
    Vector3 Direction = DataRef.CurrentDirection.Z * fbAxis + DataRef.CurrentDirection.X * lrAxis;
    float CurrentMoveSpeed = DataRef.CurrentVelocity.Length();
    if (Direction.Length() != 0)
    {
      DataRef.CurrentVelocity += DataRef.MoveAcceleration * Direction * delta;
      DataRef.CurrentVelocity = DataRef.CurrentVelocity.LimitLength(DataRef.MoveSpeed * DataRef.CurrentToolMoveModifier);
    }
    else
    {
      float NewSpeed = Mathf.Max(CurrentMoveSpeed - DataRef.MoveAcceleration * delta, 0);
      DataRef.CurrentVelocity = DataRef.CurrentVelocity.Normalized() * NewSpeed;
    }
  }

  public override void ProcessTurn(float delta, float lrAxis)
  {
    if (lrAxis != 0)
    {
      DataRef.CurrentRotationRate += DataRef.TurnAcceleration * delta * lrAxis;
      DataRef.CurrentRotationRate = Mathf.Clamp(Mathf.Abs(DataRef.CurrentRotationRate), 0, DataRef.TurnSpeed) * Mathf.Sign(DataRef.CurrentRotationRate);
    } else
    {
      DataRef.CurrentRotationRate = Mathf.Clamp(Mathf.Abs(DataRef.CurrentRotationRate) - DataRef.TurnAcceleration * delta, 0, DataRef.TurnSpeed) * Mathf.Sign(DataRef.CurrentRotationRate);
    }
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

public class PlayerFallingState : IPlayerState
{
  public PlayerFallingState(State thisState, PlayerData Data) : base(thisState, Data)
  {
  }

  public override void EnterState()
  {
    GD.Print("Enter falling");
    DataRef.SwimmingRate = Vector3.Zero;
  }

  public override void ExitState()
  {
    
  }


  public override void Act(float delta)
  {
    DataRef.SwimmingRate = Vector3.Up * Mathf.Max(-10, DataRef.SwimmingRate.Y - delta * 9.8f);
  }

  public override void ProcessMove(float delta, float fbAxis, float lrAxis)
  {
    
  }

  public override void ProcessTurn(float delta, float lrAxis)
  {
    if (lrAxis != 0)
    {
      DataRef.CurrentRotationRate += DataRef.TurnAcceleration * delta * lrAxis;
      DataRef.CurrentRotationRate = Mathf.Clamp(Mathf.Abs(DataRef.CurrentRotationRate), 0, DataRef.TurnSpeed) * Mathf.Sign(DataRef.CurrentRotationRate);
    } else
    {
      DataRef.CurrentRotationRate = Mathf.Clamp(Mathf.Abs(DataRef.CurrentRotationRate) - DataRef.TurnAcceleration * delta, 0, DataRef.TurnSpeed) * Mathf.Sign(DataRef.CurrentRotationRate);
    }
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
