using Authorship.src;
using Godot;
using System;
using System.Linq;

public partial class InventoryCategory : Control
{
  [Export] ItemCategory Category;
  public void OnItemSelected(ItemID id)
  {
    InventoryItem[] items = GetChildren().Where(child => child is InventoryItem).Cast<InventoryItem>().ToArray();
    foreach(InventoryItem item in items)
    {
      item.ValidateSelection(id);
    }
  }
}
