using Godot;
using System;

public partial class PuzzleBook : Node3D
{
	[Signal] public delegate void BookPickupEventHandler(PuzzleBook book, Node3D camera, int slotID = -1);
	public static readonly float INTERACT_DISTANCE = 5;
	[Export] int ID;

	[Export] Vector3 HoldingOffset;
	[Export] Material BookCover;

	private bool locked = false;

	Tween PositionTween;

	public int CurrentSlot = -1;

	public override void _Ready()
	{
		GetChild<MeshInstance3D>(0).SetSurfaceOverrideMaterial(0, BookCover);
	}
	private void OnMouseInput(Node3D camera, InputEvent @event, Vector3 eventPosition, Vector3 normal, long shapeIdx)
	{
		if (locked)
		{
			return;
		}
		if (@event is InputEventMouseButton eventMouseButton && eventMouseButton.IsPressed())
		{
			DetectDistanceAndInteract(camera);
		}
	}


	private void DetectDistanceAndInteract(Node3D camera)
	{
		GD.Print("DETECTED");
		if (GlobalPosition.DistanceTo(camera.GlobalPosition) < INTERACT_DISTANCE)
		{
			// pickup book
			OnBookPickup(camera);
		}
	}

	public void OnBookPickup(Node3D camera)
	{
		GD.Print("EMITTING");
		EmitSignal(SignalName.BookPickup, this, camera, CurrentSlot);
	}

	public void MoveBookToPlayer(Node3D camera)
	{
		Reparent(camera);

		// Tween to player
		PositionTween = GetTree().CreateTween().BindNode(this).SetTrans(Tween.TransitionType.Expo).SetEase(Tween.EaseType.Out);
		PositionTween.Parallel().TweenProperty(this, "position", HoldingOffset, 1f);
		PositionTween.Parallel().TweenProperty(this, "rotation", new Vector3(Mathf.DegToRad(20), Mathf.Pi, 0), 1f);
		PositionTween.Play();
		locked = true;
		CurrentSlot = -1;
		PositionTween.Finished += UnlockBook;
	}

	public void OnBookMovedToSlot(Vector3 SlotPosition, int idx)
	{
		PositionTween = GetTree().CreateTween().BindNode(this).SetTrans(Tween.TransitionType.Expo).SetEase(Tween.EaseType.Out);
		PositionTween.Parallel().TweenProperty(this, "position", SlotPosition + Vector3.Right * 0.4f, 0.6f);
		PositionTween.Parallel().TweenProperty(this, "rotation", new Vector3(0, -Mathf.Pi, 0), 0.6f);
		PositionTween.TweenProperty(this, "position", SlotPosition, 0.4f).SetEase(Tween.EaseType.In);

		PositionTween.Play();
		locked = true;
		CurrentSlot = idx;
		PositionTween.Finished += UnlockBook;
	}

	private void UnlockBook()
	{
		locked = false;
		PositionTween.Finished -= UnlockBook;
	}

	public bool IsBookLocked()
	{
		return locked;
	}

	public int GetID()
	{
		return ID;
	}
}
