using Godot;
using System;

public partial class ColdVolume : Area3D
{
	Node3D ResetDestination;
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
		player.OnColdVolumeEntered(ResetDestination.Position);
	}

	public void OnPlayerExited(PlayerCharacter player)
	{
		player.OnColdVolumeExited();
	}
}
