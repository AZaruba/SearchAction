using Godot;
using System;

public partial class IceBlock : UnlockableEntity
{
	[Export] ConditionalDoor AssociatedDoor;

	Tween MeltTween;
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
    if (ProgressTracker.GetEquippedItem(Authorship.src.ItemCategory.Tool) == requiredItem)
		{
			OnInteractWithItem();
		}
  }

  public override void OnInteractWithItem()
  {
		MeltTween = GetTree().CreateTween().BindNode(this).SetTrans(Tween.TransitionType.Expo).SetEase(Tween.EaseType.Out);
		MeltTween.TweenProperty(this, "scale", new Vector3(0.1f, 0.1f, 0.1f), 4f);
		MeltTween.Play();
		MeltTween.Finished += UnlockAssociatedDoor;
  }

  private void UnlockAssociatedDoor()
	{
		AssociatedDoor.ToggleLock();
		MeltTween.Finished -= UnlockAssociatedDoor;
		QueueFree();
	}
}
