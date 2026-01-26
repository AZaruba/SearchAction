using Authorship.src;
using Godot;
using System;
using System.Linq;

public partial class InventoryCategory : Control
{
  [Export] ItemCategory Category;

  public override void _Ready()
  {
    EventBus.Instance.ChangeEquippedItem += UpdateItems;
  }

  public override void _ExitTree()
  {
    EventBus.Instance.ChangeEquippedItem -= UpdateItems;
  }

  public void UpdateItems(ItemID id, ItemCategory cat)
  {
    if (cat == Category)
    {
      InventoryItem[] items = GetChildren().Where(child => child is InventoryItem).Cast<InventoryItem>().ToArray();
       foreach(InventoryItem item in items)
        {
          item.ChangeEquipStatus(id);
        }
    }
  }
  public void OnItemSelected(ItemID id)
  {
    InventoryItem[] items = GetChildren().Where(child => child is InventoryItem).Cast<InventoryItem>().ToArray();
    foreach(InventoryItem item in items)
    {
      item.ValidateSelection(id);
    }
  }
}
