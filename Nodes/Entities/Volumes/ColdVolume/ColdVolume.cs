using Godot;
using System;
using System.ComponentModel;

public partial class ColdVolume : Area3D
{
	[Export] Node3D ResetDestination;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		
	}
	public void OnPlayerEntered(PlayerCharacter player)
	{
		player.OnColdVolumeEntered(ResetDestination.GlobalPosition);
	}

	public void OnPlayerExited(PlayerCharacter player)
	{
		player.OnColdVolumeExited();
	}
}
