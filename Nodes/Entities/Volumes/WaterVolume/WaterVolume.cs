using Godot;
using System;

public partial class WaterVolume : Area3D
{
  [Export] CollisionShape3D Volume;
  [Export] Vector3 DesiredHeight;

  public override void _Ready()
  {
  }
  
  public void OnPlayerEntered(PlayerCharacter player)
  {
    player.OnWaterVolumeEntered(DesiredHeight);
  }

  public void OnPlayerExited(PlayerCharacter player)
  {
    player.OnWaterVolumeExited();
  }
}
