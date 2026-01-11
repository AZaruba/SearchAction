
using Godot;

public partial class PlayerData : Resource
{
  [Export] public float MoveSpeed;
  [Export] public float MoveAcceleration;
  [Export] public float TurnSpeed;
  [Export] public float TurnAcceleration;
  [Export] public float Gravity;

  public Vector3 Position;
  public Vector3 CurrentVelocity;
  public Basis CurrentDirection;
  public float CurrentRotationRate;
  public Vector3 CurrentGroundNormal;
  public Vector3 WaterVolumeSurface;
  public Vector3 SwimmingRate;
}