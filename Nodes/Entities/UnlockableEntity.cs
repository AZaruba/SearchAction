using Authorship.src;
using Godot;
using System;

public partial class UnlockableEntity : Node3D
{
  
  [Export] public ItemID requiredItem;

  public static readonly float INTERACT_DISTANCE = 5;

  public virtual void OnInteract()
  {
    if (ProgressTracker.IsItemCollected(requiredItem))
    {
      GD.Print("With");
      OnInteractWithItem();
    }
    else
    {
      GD.Print("Without");
      OnInteractWithoutItem();
    }
  }

  public virtual void OnInteractWithItem()
  {
    
  }

  public virtual void OnInteractWithoutItem()
  {
    
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
    if (GlobalPosition.DistanceTo(CameraPosition) < INTERACT_DISTANCE)
    {
      OnInteract();
    }
  }
}
