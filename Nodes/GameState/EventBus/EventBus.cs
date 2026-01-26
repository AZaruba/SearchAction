using Authorship.src;
using Godot;
using System;

public partial class EventBus : Node
{
  [Signal] public delegate void ItemPickupEventHandler(ItemID id);
  [Signal] public delegate void SelectItemEventHandler(ItemID id, ItemCategory cat);
  [Signal] public delegate void InteractWithItemEventHandler(ItemID id);
  [Signal] public delegate void ChangeEquippedItemEventHandler(ItemID id, ItemCategory cat);

  public static EventBus Instance;

  public EventBus()
  {
    Instance = this;
  }

  public static void Emit(string SignalName)
  {
    Instance.EmitSignal(SignalName);
  }

  public static void Emit(string SignalName, ItemID itemId)
  {
    Instance.EmitSignal(SignalName, (int)itemId);
  }

  public static void Emit(string SignalName, ItemID itemId, ItemCategory cat)
  {
    Instance.EmitSignal(SignalName, (int)itemId, (int)cat);
  }
}
