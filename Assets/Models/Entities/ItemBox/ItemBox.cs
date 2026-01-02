using Authorship.src;
using Godot;
using System;

public partial class ItemBox : Node3D
{
  [Export] private Timer timer;
  [Export] private Curve curve;
  [Export] private Area3D CollisionArea;
  [Export] private float BounceDistance;
  [Export] private float RotationRate;

  [Export] public ItemID Item;
  [Export] public Texture2D Icon;
  private Vector3 BasePosition;
  [Export] private Sprite3D ItemIconSprite;

  public override void _Ready()
  {
    BasePosition = Position;
    this.CollisionArea.BodyEntered += OnCollision;
    ItemIconSprite.Texture = Icon;
    base._Ready();
  }
  public override void _PhysicsProcess(double delta)
  {
    float LerpTime = (float)Mathf.Lerp(0,1,timer.TimeLeft/timer.WaitTime);
    float bounceValue = curve.Sample(LerpTime);
    Position = BasePosition + Vector3.Up * bounceValue * BounceDistance;
    Basis = Basis.Rotated(Vector3.Up, (float)delta * RotationRate);
  }

  private void OnCollision(Node3D body)
  {
    GD.Print("Got it");
    QueueFree();
    EventBus.Emit(EventBus.SignalName.ItemPickup, Item);
  }
}
