using Godot;
using System;

public partial class ColdEffect : CanvasLayer
{
	[Export] ShaderMaterial Mat;
	[Signal] public delegate void PlayerFreezeResetEventHandler();
	private float Alpha = 1f;

	private readonly float ALPHA_MIN = -1f;

	private readonly float ALPHA_MAX = 1f;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Alpha = 1f;
	}

	private bool Freezing = false;

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _PhysicsProcess(double delta)
	{
		if (Freezing)
		{
		  Alpha -= (float)delta;
			if (Alpha < ALPHA_MIN)
			{
			  OnPlayerFreezeReset();
			}
		  Mat.SetShaderParameter("AlphaValue", Alpha);
		}
		else if (Alpha < ALPHA_MAX)
		{
			Alpha+= (float)delta*3;
		  Mat.SetShaderParameter("AlphaValue", Alpha);
		}
	}

	public void OnPlayerEnteredColdVolume()
	{
		GD.Print("Player entered cold");
		Freezing = true;
	}

	public void OnPlayerExitedColdVolume()
	{
		GD.Print("Player left cold");
		Freezing = false;
	}

	private void OnPlayerFreezeReset()
	{
		Freezing = false;
		GD.Print("Emit Freeze Reset");
		EmitSignal(SignalName.PlayerFreezeReset);
	}
}
