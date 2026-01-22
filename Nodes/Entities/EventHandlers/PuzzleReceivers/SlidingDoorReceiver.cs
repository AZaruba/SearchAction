using Godot;
using System;

public partial class SlidingDoorReceiver : IEventReceiver
{
	[Export] Vector3 Destination;
	Tween MotionTween;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

  public override void OnTrigger()
  {
		MotionTween = GetTree().CreateTween().BindNode(this).SetTrans(Tween.TransitionType.Expo).SetEase(Tween.EaseType.Out);
		MotionTween.TweenProperty(this, "position", Destination, 4f);
		MotionTween.Play();
    base.OnTrigger();
  }

}
