using Authorship.src;
using Godot;
using System.Collections.Generic;

public partial class InventoryController : CanvasLayer
{
  [Export] RichTextLabel DescriptiveText;
  [Export] TextureButton EquipButton;

  private ItemID CurrentItemID;
  private ItemCategory CurrentCategory;
  public override void _Ready()
  {
	  EquipButton.Visible = false;
    DescriptiveText.Text = "";
    EventBus.Instance.SelectItem += OnSelect;
  }

  public override void _ExitTree()
  {
    base._ExitTree();
  }

  public override void _PhysicsProcess(double delta)
  {
	  base._PhysicsProcess(delta);
  }

  private void OnSelect(ItemID id, ItemCategory cat)
  {
    DescriptiveText.Text = DescriptiveTextLookup[id];
    CurrentItemID = id;
    CurrentCategory = cat;
    EquipButton.Visible = cat != ItemCategory.Key && cat != ItemCategory.Upgrade && cat != ItemCategory.Notes;
  }

  private void OnEquipButtonClicked()
  {
    EventBus.Emit(EventBus.SignalName.ChangeEquippedItem, CurrentItemID, CurrentCategory);
  }

  private static Dictionary<ItemID, string> DescriptiveTextLookup = new Dictionary<ItemID, string>()
  {
      { ItemID.RubyKey, "[center][u][font_size={32}]Ruby Key[/font_size][/u][/center][left][font_size={20}]\nA key with a red stone embedded in it. A small scuff on the stone indicates that it's probably fake.[/font_size][/left]" },

      { ItemID.SaphKey, "[center][u][font_size={32}]Saphhire Key[/font_size][/u][/center][left][font_size={20}]\nA key with a picture of a blue gemstone printed on it. Significantly less effort made to make this look real.[/font_size][/left]" },

      { ItemID.EmerKey, "[center][u][font_size={32}]Emerald Key[/font_size][/u][/center][left][font_size={20}]\nYet another key with a... hang on is this Emerald real? How much is this thing worth!? Should you just pocket it and go home?[/font_size][/left]" },

      { ItemID.ReadingGlasses, "[center][u][font_size={32}]Reading Glasses[/font_size][/u][/center][left][font_size={20}]\nYou turned thirty and your eyes aren't what they used to be.[/font_size][/left]" },

      { ItemID.Headlamp, "[center][u][font_size={32}]Head Lamp[/font_size][/u][/center][left][font_size={20}]\nIlluminate the space in front of you![/font_size][/left]" },

      { ItemID.DiveMask, "[center][u][font_size={32}]Dive Mask[/font_size][/u][/center][left][font_size={20}]\nSee underwater and dive down! Pinch your nose to equalize. Recreate a scene from Death in the Deep End where Detective Beauford cracks the case by finding a stray earring in a swimming pool.[/font_size][/left]" },

      { ItemID.TDGlasses, "[center][u][font_size={32}]3D Glasses[/font_size][/u][/center][left][font_size={20}]\n[/font_size][/left]" },

      { ItemID.Sweater, "[center][u][font_size={32}]Sweater and Slacks[/font_size][/u][/center][left][font_size={20}]\nA fashionable combo that's also comfortable! You figured you'd be outside for only a few seconds, so you left your coat at home. You certainly won't be warm in a surprise snowstorm![/font_size][/left]" },

      { ItemID.Swimsuit, "[center][u][font_size={32}]Swimsuit[/font_size][/u][/center][left][font_size={20}]\nA sporty suit appropriate for swimming laps at the gym. Makes you agile in water and you'll be able to change back into dry clothes.[/font_size][/left]" },

      { ItemID.WinterCoat, "[center][u][font_size={32}]Winter Coat[/font_size][/u][/center][left][font_size={20}]\nA thick winter coat that's sure to keep you warm. Matches the luxurious description of Detective Beauford's choice of attire in Cold Shoulder[/font_size][/left]" },

      { ItemID.Lighter, "[center][u][font_size={32}]Lighter[/font_size][/u][/center][left][font_size={20}]\nAn old-fashioned lighter. An excellent fidget toy even though you don't smoke. Should be able to melt a little ice but won't keep you warm.[/font_size][/left]" },

      { ItemID.BoltCutters, "[center][u][font_size={32}]Bolt Cutters[/font_size][/u][/center][left][font_size={20}]\n[/font_size][/left]" },

      { ItemID.Fins, "[center][u][font_size={32}]Fins[/font_size][/u][/center][left][font_size={20}]\nDive-appropriate fins that make you fast in the water. A crucial clue in Death in the Deep End.[/font_size][/left]" },
      
      { ItemID.WorkGloves, "[center][u][font_size={32}]Work Gloves[/font_size][/u][/center][left][font_size={20}]\n[/font_size][/left]" },

      { ItemID.Treads, "[center][u][font_size={32}]Snow Boots[/font_size][/u][/center][left][font_size={20}]\nSlip and slide on ice no more! These shoes will keep your feet underneath you and prevent sliding on icy patches.[/font_size][/left]" },

      { ItemID.SpeedOne, "[center][u][font_size={32}]Coffee (Half-caf)[/font_size][/u][/center][left][font_size={20}]\nIt's way too late for a cup of coffee, but this should give an extra spring to your step.[/font_size][/left]" },

      { ItemID.SpeedTwo, "[center][u][font_size={32}]Jelly Beans[/font_size][/u][/center][left][font_size={20}]\nGenuinely good fuel for endurance athletes. Quick energy and easy on the stomach. Also peps you up because it's literally candy![/font_size][/left]" },

      { ItemID.SpeedThree, "[center][u][font_size={32}]Athletic socks[/font_size][/u][/center][left][font_size={20}]\nFashionable wool socks are okay for walking around, but you should be able to move faster without fear of blisters with these.[/font_size][/left]" },
  };
}
