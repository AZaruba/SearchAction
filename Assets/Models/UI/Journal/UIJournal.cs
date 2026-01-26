using Authorship.src;
using Godot;

public partial class UIJournal : Node3D
{
  public static readonly string OPEN_ANIM = "Open";
  public static readonly string CLOSE_ANIM = "Close";

  [Export] AnimationPlayer AnimPlayer;
	[Export] Vector3 OpenPosition;
	[Export] Vector3 ClosePosition;

	[Export] SubViewport RenderedTexture;

	[Export] QuadMesh Quad;

	Tween PositionTween;

	private bool IsOpen = false;
	private bool Locked = false;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}
	
	private void OnMouseInput(Node3D camera, InputEvent @event, Vector3 eventPosition, Vector3 normal, long shapeIdx)
	{
		if (@event is InputEventMouseButton eventMouseButton && eventMouseButton.IsPressed())
		{
			Vector2 unprojected = GetViewport().GetCamera3D().UnprojectPosition(eventPosition);
			Vector3 affineTransformed = GlobalTransform.AffineInverse() * eventPosition;

			float TranslatedX = affineTransformed.Z / Quad.Size.Y + 0.5f;
			float TranslatedY = affineTransformed.X * - 1 / Quad.Size.X + 1f;

			InputEventMouse asMouse = @event as InputEventMouse;
		  asMouse.Position = new Vector2(TranslatedX * 600, TranslatedY * 600 * (2.8f/4f));
		  asMouse.GlobalPosition = new Vector2(TranslatedX * 600, TranslatedY * 600 * (2.8f/4f));
			DebugLog.LogToScreen(asMouse.GlobalPosition.ToString(), 2);
			DebugLog.LogToScreen(asMouse.Position.ToString(), 3);
			RenderedTexture.PushInput(asMouse);
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (Locked)
		{
			return;
		}

    if (Input.IsActionJustPressed(InputActions.OpenInventory))
		{
      Locked = true;
		  if (IsOpen)
			{
				IsOpen = false;
				Close();
			}
			else
			{
				IsOpen = true;
				Open();
			}
		}
	}

  private void Unlock(StringName _animName)
  {
		Locked = false;
		AnimPlayer.AnimationFinished -= Unlock;
  }
  private void Unlock()
  {
		Locked = false;
		PositionTween.Finished -= Unlock;
  }

	public void Close()
	{
		AnimPlayer.Play(CLOSE_ANIM);
		AnimPlayer.AnimationFinished += TweenClose;
	}

	public void TweenClose(StringName _animName)
	{
		AnimPlayer.AnimationFinished -= TweenClose;
		PositionTween = GetTree().CreateTween().BindNode(this).SetTrans(Tween.TransitionType.Expo).SetEase(Tween.EaseType.InOut);
		PositionTween.Parallel().TweenProperty(this, "position", ClosePosition, 1f);
		PositionTween.Play();
		PositionTween.Finished += Unlock;
	}


  public void Open()
	{
		PositionTween = GetTree().CreateTween().BindNode(this).SetTrans(Tween.TransitionType.Expo).SetEase(Tween.EaseType.InOut);
		PositionTween.Parallel().TweenProperty(this, "position", OpenPosition, 1f);
		PositionTween.Play();
		PositionTween.Finished += AnimOpen;
	}

	public void AnimOpen()
	{
		AnimPlayer.AnimationFinished += Unlock;
		AnimPlayer.Play(OPEN_ANIM);
		PositionTween.Finished -= AnimOpen;
	}
}
