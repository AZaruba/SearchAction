using Godot;
using System;

public partial class WaterVolume : Area3D
{
  [Export] CollisionShape3D Volume;
  [Export] Vector3 PushDirection;
  [Export] bool IsDebug;

  public override void _Ready()
  {
    
  }

  public override void _PhysicsProcess(double delta)
  {
  }
  
  public void OnPlayerEntered(PlayerCharacter player)
  {
    player.OnWaterVolumeEntered(PushDirection);
  }

  public void OnPlayerExited(PlayerCharacter player)
  {
    // need to ensure when player leaves one volume and enters another, it's okay
    player.OnWaterVolumeExited(PushDirection);
  }
}
