using Godot;
using System;

public partial class SingleDoor : UnlockableEntity
{
  public static readonly string OPEN_ANIM = "Open";
  public static readonly string LOCKED_ANIM = "OpenLocked";

  public static readonly float INTERACT_DISTANCE = 5;
  private bool unlocked = false;

  [Export] AnimationPlayer AnimPlayer;

  private bool IsUnlocked = false;

  public override void _Ready()
  {
    // TODO: if door is open, make it open automatically?
    base._Ready();
  }

  public override void OnInteractWithItem()
  {
    if (!unlocked)
    {
      GD.Print("Unlocked the door!");
      AnimPlayer.Play(OPEN_ANIM);
      unlocked = true;
    }

    base.OnInteractWithItem();
  }

  // TODO: Add animation for locked door
  public override void OnInteractWithoutItem()
  {
    GD.Print("You can't open that!");
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
