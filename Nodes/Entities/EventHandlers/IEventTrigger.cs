using Godot;
using System;

public partial class IEventTrigger : Node
{
	[Export] IEventReceiver ConnectedEntity;

	private bool Triggered = false;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public virtual void TriggerEvent()
	{
		if (!Triggered)
		{
		  ConnectedEntity.OnTrigger();
		}
		Triggered = true;
	}
}
