using Godot;
using System;

public partial class WaterVolume : Area3D
{
  [Export] CollisionShape3D Volume;
  [Export] Vector3 DesiredHeight;
  [Export] bool IsDebug;

  public override void _Ready()
  {
    GD.Print(CollisionMask.ToString());
    GD.Print(CollisionLayer.ToString());
  }

  public override void _PhysicsProcess(double delta)
  {
    if (GetOverlappingBodies().Count > 0 && IsDebug)
    {
      DebugLog.LogToScreen("Player in a volume", 3);
    }
    else if (IsDebug)
    {
      DebugLog.LogToScreen("Player NOT in a volume", 3);
    }
  }
  
  public void OnPlayerEntered(PlayerCharacter player)
  {
    GD.Print("Entering");
    player.OnWaterVolumeEntered(DesiredHeight);
  }

  public void OnPlayerExited(PlayerCharacter player)
  {
    GD.Print("Exiting");
    // need to ensure when player leaves one volume and enters another, it's okay
    player.OnWaterVolumeExited();
  }
}
