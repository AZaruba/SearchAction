using Authorship.src;
using Godot;
using System;

public partial class ThankYou : Node3D
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		EventBus.Instance.ChangeEquippedItem += Reveal;
	}

  private void Reveal(ItemID id, ItemCategory cat)
  {
		if (id == ItemID.TDGlasses)
		{
      Visible = true;
		}
		else if (cat == ItemCategory.Hat)
		{
			Visible = false;
		}
  }


  public override void _ExitTree()
  {
		EventBus.Instance.ChangeEquippedItem -= Reveal;
  }


	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
