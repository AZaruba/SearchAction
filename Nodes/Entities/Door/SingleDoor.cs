using Godot;
using System;

public partial class SingleDoor : UnlockableEntity
{
  public static readonly string OPEN_ANIM = "Open";
  public static readonly string LOCKED_ANIM = "OpenLocked";

  [Export] AnimationPlayer AnimPlayer;

  private bool IsUnlocked = false;

  public override void _Ready()
  {
    // TODO: if door is open, make it open automatically?
    base._Ready();
  }

  public override void OnInteractWithItem()
  {
    if (!IsUnlocked)
    {
      AnimPlayer.Play(OPEN_ANIM);
      IsUnlocked = true;
    }

    base.OnInteractWithItem();
  }

  // TODO: Add animation for locked door
  public override void OnInteractWithoutItem()
  {

    // potential edge case: interacting without item set
    AnimPlayer.Play(LOCKED_ANIM);
    base.OnInteractWithoutItem();
  }
  private void OnMouseInput(Node3D camera, InputEvent @event, Vector3 eventPosition, Vector3 normal, long shapeIdx)
  {
    if (@event is InputEventMouseButton eventMouseButton && eventMouseButton.IsPressed())
    {
      DetectDistanceAndInteract(camera.GlobalPosition);
    }
  }

  private void DetectDistanceAndInteract(Vector3 CameraPosition)
  {
    if (Position.DistanceTo(CameraPosition) < INTERACT_DISTANCE)
    {
      OnInteract();
    }
  }
}
