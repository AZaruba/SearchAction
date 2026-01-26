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
    InitMessages();
    InitStateMachine();
    base._Ready();
  }

  public override void _PhysicsProcess(double delta)
  {
    base._PhysicsProcess(delta);

    SetCollisionMaskValue(9, ProgressTracker.GetEquippedItem(ItemCategory.Body) != ItemID.Swimsuit);

    // synchronize character
    Data.CurrentGroundNormal = GetFloorNormal();
    Data.CurrentDirection = Basis;
    Data.Position = Position;

    // update state
    UpdateState();
    
    // apply state actions
    ProcessState((float)delta);

    // synchronize engine

    // Need to "Redirect" movement with rotation, as the rotation acceleration gives the rotation weight, preventing "drifting" feeling
    Data.CurrentVelocity = Data.CurrentVelocity.Rotated(UpDirection, Data.CurrentRotationRate * (float)delta);
    Velocity = Data.CurrentVelocity + Data.SwimmingRate;
    Basis = Basis.Rotated(UpDirection, Data.CurrentRotationRate * (float)delta).Orthonormalized();
    MoveAndSlide();
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
    if (Data.Position.DistanceSquaredTo(Data.SlidingTarget) < 1.1f)
    {
      StateMachine.Execute(StateManagement.Command.END_SLIDE);
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

  public void OnIceVolumeEntered(Vector3 targetLocation)
  {
    if (ProgressTracker.GetEquippedItem(ItemCategory.Tool) == ItemID.Treads)
    {
      return;
    }

    // only if sliding is NOT already happening
    if (StateMachine.GetCurrentState().Key() != StateManagement.State.SLIDING)
    {
      // future note - what about multi step sliding?
      Data.SlidingTarget = targetLocation;
    }
    StateMachine.Execute(StateManagement.Command.SLIDE);
  }

  public void OnIceVolumeExited()
  {
    
  }

  public void OnColdVolumeEntered(Vector3 resetLocation)
  {
    
  }

  public void OnColdVolumeExited()
  {
    
  }

  private void OnEquipItem(ItemID item, ItemCategory _itemCategory)
  {
    if (item == ItemID.Fins)
    {
      Data.CurrentToolSwimModifier = 1.5f;
      Data.CurrentToolMoveModifier = 0.1f;
    }
    else if (item == ItemID.Treads)
    {
      Data.CurrentToolSwimModifier = 0.1f;
      Data.CurrentToolMoveModifier = 0.6f;
    }
    else
    {
      Data.CurrentToolSwimModifier = 1;
      Data.CurrentToolMoveModifier = 0.7f;
    }
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
    Data.CurrentTimeInCold = 0;
    Data.SlidingTarget = Vector3.Zero;
  }

  private void InitMessages()
  {
    EventBus.Instance.ChangeEquippedItem += OnEquipItem;
  }

  private void InitStateMachine()
  {
    StateMachine = new StateMachine<IPlayerState>();
    StateMachine.AddState(StateManagement.State.GROUNDED, new PlayerGroundedState(StateManagement.State.GROUNDED, Data));
    StateMachine.AddState(StateManagement.State.SWIMMING, new PlayerSwimmingState(StateManagement.State.SWIMMING, Data));
    StateMachine.AddState(StateManagement.State.FALLING, new PlayerFallingState(StateManagement.State.FALLING, Data));
    StateMachine.AddState(StateManagement.State.SLIDING, new PlayerSlidingState(StateManagement.State.SLIDING, Data));

    StateMachine.AddTransition(StateManagement.State.GROUNDED, StateManagement.Command.ENTER_WATER, StateManagement.State.SWIMMING);
    StateMachine.AddTransition(StateManagement.State.GROUNDED, StateManagement.Command.SLIDE, StateManagement.State.SLIDING);
    StateMachine.AddTransition(StateManagement.State.GROUNDED, StateManagement.Command.FALL, StateManagement.State.FALLING);

    StateMachine.AddTransition(StateManagement.State.FALLING, StateManagement.Command.LAND, StateManagement.State.GROUNDED);
    StateMachine.AddTransition(StateManagement.State.FALLING, StateManagement.Command.ENTER_WATER, StateManagement.State.SWIMMING);

    StateMachine.AddTransition(StateManagement.State.SWIMMING, StateManagement.Command.LEAVE_WATER, StateManagement.State.GROUNDED);

    StateMachine.AddTransition(StateManagement.State.SLIDING, StateManagement.Command.END_SLIDE, StateManagement.State.GROUNDED);

    StateMachine.Start(StateManagement.State.GROUNDED);
  }
  
  public override void _ExitTree()
  {
    EventBus.Instance.ChangeEquippedItem -= OnEquipItem;
  }
}
