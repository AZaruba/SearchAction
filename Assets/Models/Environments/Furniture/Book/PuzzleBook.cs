using Godot;
using System;

public partial class PuzzleBook : StaticBody3D
{
  private void OnMouseInput(Node camera, InputEvent @event, Vector3 eventPosition, Vector3 normal, long shapeIdx)
  {
    if (@event is InputEventMouseButton eventMouseButton)
    {
      GD.Print("Click The book!");
    }
  }
}
