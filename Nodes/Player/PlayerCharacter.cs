using System;
using System.Diagnostics;
using Authorship.src;
using Godot;
using Godot.Collections;

public partial class PlayerCharacter : CharacterBody3D
{
  [Export] PlayerData Data;
  [Export] CanvasLayer WaterOverlay;
  StateMachine<IPlayerState> StateMachine;

  public override void _Ready()
  {
    InitData();
    InitStateMachine();
    base._Ready();
  }

  public override void _PhysicsProcess(double delta)
  {
    base._PhysicsProcess(delta);

    // synchronize character
    Data.CurrentGroundNormal = GetFloorNormal();
    Data.CurrentDirection = Basis;
    Data.Position = Position;

    // update state
    UpdateState();

    // apply state actions
    ProcessState((float)delta);
    DebugLog.LogToScreen("Current Float Rate: " + Data.SwimmingRate, 2);
    DebugLog.LogToScreen("Current State: " + StateMachine.GetCurrentState(), 3);

    // synchronize engine

    // Need to "Redirect" movement with rotation, as the rotation acceleration gives the rotation weight, preventing "drifting" feeling
    Data.CurrentVelocity = Data.CurrentVelocity.Rotated(UpDirection, Data.CurrentRotationRate * (float)delta);
    Velocity = Data.CurrentVelocity + Data.SwimmingRate;
    Basis = Basis.Rotated(UpDirection, Data.CurrentRotationRate * (float)delta).Orthonormalized();
    MoveAndSlide();
  }

  private void InitData()
  {
    Data.CurrentVelocity = Vector3.Zero;
    Data.Position = Position;
    Data.CurrentRotationRate = 0;
    Data.CurrentGroundNormal = GetFloorNormal();
    Data.CurrentDirection = Basis;
    Data.SwimmingRate = Vector3.Zero;
    Data.WaterVolumeSurface = Vector3.Zero;
  }

  private void InitStateMachine()
  {
    StateMachine = new StateMachine<IPlayerState>();
    StateMachine.AddState(StateManagement.State.GROUNDED, new PlayerGroundedState(StateManagement.State.GROUNDED, Data));
    StateMachine.AddState(StateManagement.State.SWIMMING,new PlayerSwimmingState(StateManagement.State.SWIMMING, Data));
    StateMachine.AddState(StateManagement.State.FALLING,new PlayerFallingState(StateManagement.State.FALLING, Data));
    StateMachine.AddTransition(StateManagement.State.GROUNDED, StateManagement.Command.ENTER_WATER, StateManagement.State.SWIMMING);
    StateMachine.AddTransition(StateManagement.State.FALLING, StateManagement.Command.ENTER_WATER, StateManagement.State.SWIMMING);
    StateMachine.AddTransition(StateManagement.State.SWIMMING, StateManagement.Command.LEAVE_WATER, StateManagement.State.GROUNDED);
    StateMachine.AddTransition(StateManagement.State.GROUNDED, StateManagement.Command.FALL, StateManagement.State.FALLING);
    StateMachine.AddTransition(StateManagement.State.FALLING, StateManagement.Command.LAND, StateManagement.State.GROUNDED);
    StateMachine.Start(StateManagement.State.GROUNDED);
  }

  private void UpdateState()
  {
    if (MotionMode == MotionModeEnum.Grounded)
    {
      if (!IsOnFloor())
      {
        StateMachine.Execute(StateManagement.Command.FALL);
      }
      else if (MotionMode == MotionModeEnum.Grounded)
      {
        StateMachine.Execute(StateManagement.Command.LAND);
      }
    }
  }

  private void ProcessState(float delta)
  {
    StateMachine.GetCurrentState().Act(delta);

    float fbAxis = Input.GetAxis(InputActions.MoveBack, InputActions.MoveForward);
    float lrAxis = Input.GetAxis(InputActions.MoveRight, InputActions.MoveLeft);
    float lookAxis = Input.GetAxis(InputActions.LookRight, InputActions.LookLeft);

    StateMachine.GetCurrentState().ProcessMove(delta, fbAxis, lrAxis);
    StateMachine.GetCurrentState().ProcessTurn(delta, lookAxis);

    if (Input.IsActionJustPressed(InputActions.Interact))
    {
      CastInteractionRay();
    }
    if (StateMachine.GetCurrentState().Key() == StateManagement.State.SWIMMING)
    {
      WaterOverlay.Visible = Position.Y < Data.WaterVolumeSurface.Y;
    }
  }

  private void CastInteractionRay()
  {
    PhysicsDirectSpaceState3D spaceState = GetWorld3D().DirectSpaceState;
    PhysicsRayQueryParameters3D rayParams = PhysicsRayQueryParameters3D.Create(Position, Position + Basis.Z * 2, 4);
    Dictionary result = spaceState.IntersectRay(rayParams);
    if (result.TryGetValue("collider", out Variant collider))
    {
      // better way to find our UnlockableEntity?
      UnlockableEntity attachedEntity = collider.As<StaticBody3D>().GetParent().GetParent().GetParent<UnlockableEntity>();
      attachedEntity.OnInteract();
    }
    else
    {
    }
  }

  public void OnWaterVolumeEntered(Vector3 desiredHeight)
  {
    Data.WaterVolumeSurface = desiredHeight;
    MotionMode = MotionModeEnum.Floating;
    StateMachine.Execute(StateManagement.Command.ENTER_WATER);
  }

  public void OnWaterVolumeExited()
  {
    MotionMode = MotionModeEnum.Grounded;
    Data.WaterVolumeSurface = Vector3.Zero;
    StateMachine.Execute(StateManagement.Command.LEAVE_WATER);
  }
}
