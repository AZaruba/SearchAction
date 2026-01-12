using Authorship.src;
using Godot;
using System;

public partial class DebugLog : CanvasLayer
{
  public static DebugLog Instance;
  [Export] RichTextLabel LogLine1;
  [Export] RichTextLabel LogLine2;
  [Export] RichTextLabel LogLine3;

  public override void _Ready()
  {
	  base._Ready();
	  Instance = this;
  }

  public override void _Process(double delta)
  {
	base._Process(delta);

  #if DEBUG
	if (Input.IsActionJustPressed(InputActions.DEBUG_Toggle_Log))
	{
    GD.Print("Dropping?");
	  LogLine1.Visible = !LogLine1.Visible;
	  LogLine2.Visible = !LogLine2.Visible;
	  LogLine3.Visible = !LogLine3.Visible;
	}
	if (Input.IsActionJustPressed(InputActions.DEBUG_Exit))
	{
	  GetTree().Root.PropagateNotification((int)NotificationWMCloseRequest);
	  GetTree().Quit();
	}
	if (Input.IsActionJustPressed(InputActions.DEBUG_Reset))
	{
	  // how to reload appropriately?
	  GetTree().ReloadCurrentScene();
	}
#endif
  }

  public static void LogToScreen(string textIn, int line = 1)
  {
	if (line == 1)
	{
	  Instance.LogLine1.Text = textIn;
	}
	if (line == 2)
	{
	  Instance.LogLine2.Text = textIn;
	}
	if (line == 3)
	{
	  Instance.LogLine3.Text = textIn;
	}
  }
}
