using Godot;
using System;

public partial class BuoyancyComponent : Node3D
{
  [Export] float Buoyancy;
  [Export] Area3D SurfaceDetector;

  private float CurrentBuoyancy;
  private bool AtSurface;
  private Vector3 SurfacePosition;

  public override void _Ready()
  {
    CurrentBuoyancy = 0;
    AtSurface = true;
  }

  public override void _PhysicsProcess(double delta)
  {
    if (SurfaceDetector.HasOverlappingAreas())
    {
      if (AtSurface)
      {
        SurfacePosition = GlobalPosition;
      }
      DebugLog.LogToScreen("Buoyancy detects below surface");
      Descend();
      AtSurface = false;
    }
    else
    {
      if (!AtSurface)
      {
        SurfacePosition = GlobalPosition;
      }
      DebugLog.LogToScreen("Buoyancy detects surface");
      HitSurface();
      AtSurface = true;
    }
  }

  public void HitSurface()
  {
    CurrentBuoyancy = 0;
  }

  public void Descend()
  {
    CurrentBuoyancy = Buoyancy;
  }
  public float GetBuoyancy()
  {
    return CurrentBuoyancy;
  }

  public bool IsAtSurface()
  {
    return AtSurface;
  }

  public Vector3 GetSurface()
  {
    return SurfacePosition;
  }
}
