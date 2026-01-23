using Godot;
using System;

public partial class ConditionalDoor : UnlockableEntity
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

  private void DetectDistanceAndInteract(Vector3 CameraPosition)
  {
    if (Position.DistanceTo(CameraPosition) < INTERACT_DISTANCE)
    {
      OnInteract();
    }
  }

  public override void OnInteract()
  {
    if (IsUnlocked)
    {
      AnimPlayer.Play(OPEN_ANIM);
    }
  }

  public void ToggleLock()
  {
    IsUnlocked = true;
  }
}
