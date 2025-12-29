using Authorship.src;
using Godot;
using System;

public partial class InventoryController : CanvasLayer
{
  public override void _Ready()
  {
    base._Ready();
  }

  public override void _PhysicsProcess(double delta)
  {
    if (Input.IsActionJustPressed(InputActions.OpenInventory))
    {
      Visible = !Visible;
    }
    base._PhysicsProcess(delta);
  }
}
