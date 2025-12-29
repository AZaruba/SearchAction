using Authorship.src;
using Godot;
using System;
using System.Collections.Generic;

public class ProgressInfo
{
  public Dictionary<ItemID, bool> CollectedItems = new Dictionary<ItemID, bool>()
  {
    { ItemID.TestKey, false }
  };
  
  public ItemID EquippedHat;
  public ItemID EquippedClothes;
  public ItemID EquippedItem;

  public ProgressInfo()
  {
    CollectedItems  = new Dictionary<ItemID, bool>()
    {
      { ItemID.TestKey, false },
      { ItemID.Sweater, true }
    };

    EquippedHat = ItemID.None;
    EquippedClothes = ItemID.None;
    EquippedItem = ItemID.None;
  }
}

public partial class ProgressTracker : Node
{
  public static ProgressTracker Instance;

  private ProgressInfo Progress;

  public override void _Ready()
  {
    Progress = new ProgressInfo();
    Instance = this;
    base._Ready();
  }

  public static void CollectItem(ItemID collectedItem)
  {
    if (!Instance.Progress.CollectedItems.ContainsKey(collectedItem))
    {
      throw new Exception("COULD NOT FIND ITEM " + collectedItem.ToString());
    }
    else
    {
      Instance.Progress.CollectedItems[collectedItem] = true;
    }
  }

  public static bool IsItemCollected(ItemID itemID)
  {
    if (itemID == ItemID.None)
    {
      return true;
    }
    
    if (!Instance.Progress.CollectedItems.ContainsKey(itemID))
    {
      throw new Exception("COULD NOT FIND ITEM " + itemID.ToString());
    }
    else
    {
      return Instance.Progress.CollectedItems[itemID];
    }
  }
}
