
using Godot;

public partial class PlayerData : Resource
{
  [Export] public float MoveSpeed;
  [Export] public float MoveAcceleration;
  [Export] public float TurnSpeed;
  [Export] public float TurnAcceleration;
  [Export] public float Gravity;
  [Export] public float SlideVelocity;
  [Export] public float ColdResetTime;

  public Vector3 Position;
  public Vector3 CurrentVelocity;
  public Basis CurrentDirection;
  public float CurrentRotationRate;
  public Vector3 CurrentGroundNormal;
  public Vector3 SwimmingRate;

  public Vector3 SlidingTarget;

  public float CurrentTimeInCold;
  public float CurrentToolMoveModifier = 1;
  public float CurrentToolSwimModifier = 0.7f;

  public float CurrentBuoyancy;
  public Vector3 CurrentBuoyancySurface;

  public bool IsAtWaterSurface;
}