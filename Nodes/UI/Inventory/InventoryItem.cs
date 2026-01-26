using Authorship.src;
using Godot;
using System;

public partial class InventoryItem : TextureButton
{
	
	[Export] Texture2D ItemIcon;
	[Export] TextureRect ItemTextureRect;
	[Export] TextureRect BackgroundTextureRect;

	[Export] ItemID ID;
	[Export] ItemCategory Category;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		ItemTextureRect.Texture = ItemIcon;
		EventBus.Instance.ItemPickup += OnItemPickup;
		if (ProgressTracker.IsItemCollected(ID))
		{
			// collecte default items
			ItemTextureRect.Visible = true;
		}
		if (ProgressTracker.GetEquippedItem(Category) == ID)
		{
			BackgroundTextureRect.Visible = false;
			GetParent<InventoryCategory>().OnItemSelected(ID);
		}
	}

    public override void _ExitTree()
    {
		  EventBus.Instance.ItemPickup -= OnItemPickup;
    }


	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void ValidateSelection(ItemID id)
	{
		if (id != ID)
		{
			BackgroundTextureRect.Visible = true;
		}
	}

	public void ChangeEquipStatus(ItemID id)
	{
		BackgroundTextureRect.Visible = id != ID;
	}

	private void OnClick()
	{
    if (ProgressTracker.IsItemCollected(ID))
		{
			if (Category == ItemCategory.Key || Category == ItemCategory.Upgrade)
			{
				return;
			}
			
			EventBus.Emit(EventBus.SignalName.SelectItem, ID, Category);
		}
	}

	private void OnItemPickup(ItemID id)
	{
		if (this.ID == id)
		{
			 ItemTextureRect.Visible = true;
		}
	}
}
