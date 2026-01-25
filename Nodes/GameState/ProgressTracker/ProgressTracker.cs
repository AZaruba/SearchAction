using Authorship.src;
using Godot;
using System;
using System.Collections.Generic;

public class ProgressInfo
{
  public Dictionary<ItemID, bool> CollectedItems;
  
  public ItemID EquippedHat;
  public ItemID EquippedBody;
  public ItemID EquippedTool;

  public ProgressInfo()
  {
    CollectedItems  = new Dictionary<ItemID, bool>()
    {
      { ItemID.RubyKey, false },
      { ItemID.SaphKey, false },
      { ItemID.EmerKey, false },
      { ItemID.ReadingGlasses, true },
      { ItemID.Headlamp, false },
      { ItemID.DiveMask, true },
      { ItemID.TDGlasses, false },
      { ItemID.Sweater, true },
      { ItemID.Swimsuit, true },
      { ItemID.WinterCoat, false },
      { ItemID.Lighter, true },
      { ItemID.BoltCutters, false },
      { ItemID.Fins, true },
      { ItemID.WorkGloves, false },
      { ItemID.Treads, false },
      { ItemID.SpeedOne, false },
      { ItemID.SpeedTwo, false },
      { ItemID.SpeedThree, false },
    };

    EquippedHat = ItemID.ReadingGlasses;
    EquippedBody = ItemID.Sweater;
    EquippedTool = ItemID.None;
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

    EventBus.Instance.ItemPickup += CollectItem;
    EventBus.Instance.SelectItem += EquipItem;

    base._Ready();
  }

    public override void _ExitTree()
    {
    EventBus.Instance.ItemPickup -= CollectItem;
    EventBus.Instance.SelectItem -= EquipItem;
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

  public static void EquipItem(ItemID id, ItemCategory cat)
  {
    GD.Print(id);
    if (cat == ItemCategory.Hat)
    {
      Instance.Progress.EquippedHat = id;
    }
    else if (cat == ItemCategory.Body)
    {
      Instance.Progress.EquippedBody = id;
    }
    else if (cat == ItemCategory.Tool)
    {
      Instance.Progress.EquippedTool = id;
      EventBus.Emit(EventBus.SignalName.ChangeEquippedItem, id);
    }
  }

  public static ItemID GetEquippedItem(ItemCategory cat)
  {
    if (cat == ItemCategory.Hat)
    {
      return Instance.Progress.EquippedHat;
    }
    else if (cat == ItemCategory.Body)
    { 
      return Instance.Progress.EquippedBody;
    }
    else 
    { 
      return Instance.Progress.EquippedTool;
    }
  }
}
