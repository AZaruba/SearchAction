using Godot;
using System;

public partial class SingleDoor : UnlockableEntity
{
  public static readonly string OPEN_ANIM = "Open";
  public static readonly string LOCKED_ANIM = "OpenLocked";
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
      AnimPlayer.Play(OPEN_ANIM);
      unlocked = true;
    }

    base.OnInteractWithItem();
  }

  // TODO: Add animation for locked door
  public override void OnInteractWithoutItem()
  {
    AnimPlayer.Play(LOCKED_ANIM);
    base.OnInteractWithoutItem();
  }
}
