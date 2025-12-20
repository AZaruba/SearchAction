using Godot;
using System;

public partial class TestDoor : UnlockableEntity
{
  private bool IsUnlocked = false;
  public override void _Ready()
  {
    base._Ready();
  }

  public override void _PhysicsProcess(double delta)
  {
    if (IsUnlocked)
    {
      Position -= (float)delta * Vector3.Down;
    }
  }

  public override void OnInteractWithItem()
  {
    IsUnlocked = true;
  }

  public override void OnInteractWithoutItem()
  {
    
  }

  public void InteractSignal()
  {
    
  }
}
