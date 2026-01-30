using Godot;
using System;

public partial class PushButton : UnlockableEntity
{
	[Export] IEventTrigger Trigger;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

  public override void OnInteract()
  {
    Trigger.TriggerEvent();
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
