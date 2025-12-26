using Authorship.src;
using Godot;
using System;

public partial class UnlockableEntity : Node3D
{
  
  [Export] public ItemID requiredItem;

  public virtual void OnInteract()
  {
    if (requiredItem == ItemID.None)
    {
      OnInteractWithItem();
    }
    else
    {
      OnInteractWithoutItem();
    }
  }

  public virtual void OnInteractWithItem()
  {
    
  }

  public virtual void OnInteractWithoutItem()
  {
    
  }
}
