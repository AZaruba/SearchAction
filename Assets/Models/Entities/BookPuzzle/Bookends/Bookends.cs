using Godot;
using System;

public partial class Bookends : Node3D
{	
    [Signal] public delegate void OnBookendClickedEventHandler(Bookends slot);
	[Export] public Vector3 SlotPosition;

	public PuzzleBook CurrentBook = null;
    public static readonly float INTERACT_DISTANCE = 5;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void OnBookMovedToSlot(PuzzleBook newBook)
	{
		CurrentBook = newBook;
		CurrentBook.GlobalPosition = SlotPosition + this.GlobalPosition;
		CurrentBook.Visible = true;
	}

	public void OnBookPickup()
	{
		CurrentBook = null;
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
    GD.Print("BOOKEND DETECTED");
    if (GlobalPosition.DistanceTo(CameraPosition) < INTERACT_DISTANCE)
	{
		EmitSignal(SignalName.OnBookendClicked, this);
	}
  }

  public bool HasBook()
  {
	return CurrentBook != null;
  }
}
