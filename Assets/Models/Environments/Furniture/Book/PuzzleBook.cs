using Godot;
using System;

public partial class PuzzleBook : Node3D
{
	[Signal] public delegate void BookPickupEventHandler(PuzzleBook book, int slotID = -1);
  public static readonly float INTERACT_DISTANCE = 5;
  [Export] int ID;

  public int CurrentSlot = -1;
  private void OnMouseInput(Node3D camera, InputEvent @event, Vector3 eventPosition, Vector3 normal, long shapeIdx)
  {
	if (@event is InputEventMouseButton eventMouseButton && eventMouseButton.IsPressed())
	{
	  DetectDistanceAndInteract(camera.GlobalPosition);
	}
  }


  private void DetectDistanceAndInteract(Vector3 CameraPosition)
  {
	GD.Print("DETECTED");
	if (GlobalPosition.DistanceTo(CameraPosition) < INTERACT_DISTANCE)
	{
	  // pickup book
	  OnBookPickup();
	}
  }

  public void OnBookPickup()
  {
	  GD.Print("EMITTING");
	  EmitSignal(SignalName.BookPickup, this, CurrentSlot);
	  this.Visible = false;
  }

  public void OnBookMovedToSlot(Vector3 SlotPosition)
  {
	  this.Position = SlotPosition;
	  this.Visible = true;
  }
}
