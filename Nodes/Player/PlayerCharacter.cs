using System;
using Authorship.src;
using Godot;
using Godot.Collections;

public partial class PlayerCharacter : CharacterBody3D
{
  [Export] PlayerData Data;
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

    // apply state actions
    ProcessState((float)delta);
    DebugLog.LogToScreen("Current Move Direction: " + Data.CurrentVelocity.ToString());

    // synchronize engine
    Velocity = Data.CurrentVelocity;
    Basis = Basis.Rotated(UpDirection, Data.CurrentRotationRate * (float)delta).Orthonormalized();
    MoveAndSlide();
  }

  private void InitData()
  {
    Data.CurrentVelocity = Vector3.Zero;
    Data.CurrentRotationRate = 0;
    Data.CurrentGroundNormal = GetFloorNormal();
    Data.CurrentDirection = Basis;
  }

  private void InitStateMachine()
  {
    StateMachine = new StateMachine<IPlayerState>();
    StateMachine.AddState(StateManagement.State.GROUNDED, new PlayerGroundedState(StateManagement.State.GROUNDED, Data));
    StateMachine.Start(StateManagement.State.GROUNDED);
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
  }

  private void CastInteractionRay()
  {
    PhysicsDirectSpaceState3D spaceState = GetWorld3D().DirectSpaceState;
    PhysicsRayQueryParameters3D rayParams = PhysicsRayQueryParameters3D.Create(Position, Position + Basis.Z * 2, 4);
    Dictionary result = spaceState.IntersectRay(rayParams);
    DebugLog.LogToScreen("Casting Ray From: " + rayParams.From.ToString() + " To " + rayParams.To.ToString(), 2);
    if (result.TryGetValue("collider", out Variant collider))
    {
      DebugLog.LogToScreen("Detected " + collider, 3);
      // better way to find our UnlockableEntity?
      UnlockableEntity attachedEntity = collider.As<StaticBody3D>().GetParent().GetParent().GetParent<UnlockableEntity>();
      attachedEntity.OnInteract();
    }
    else
    {
      DebugLog.LogToScreen("No object detected", 3);
    }
  }
}
