using Godot;
using System;
using Godot.Collections;

public partial class PuzzleBookshelf : Node3D
{
	
	[Signal] public delegate void BookDropEventHandler(PuzzleBook book, int slotID);
	
	[Signal] public delegate void BookResetEventHandler(PuzzleBook book, int slotID);
	[Signal] public delegate void AllResetEventHandler();
	
	[Export] Vector3 CameraPositionWhileInPuzzle;

	[Export] Array<PuzzleBook> Books;
	[Export] Array<Bookends> BookSlots;

	[Export] IEventTrigger SolveTrigger;

	[Export] Array<int> SolutionIDs;

	private PuzzleBook HeldBook = null;

	private Vector3 ReturnCamera;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		foreach(PuzzleBook book in Books)
		{
			book.BookPickup += OnBookPickup;
		}

		foreach(Bookends slot in BookSlots)
		{
			slot.OnBookendClicked += OnBookendClicked;
		}
	}

    public override void _ExitTree()
    {
		foreach(PuzzleBook book in Books)
		{
			book.BookPickup -= OnBookPickup;
		}
		foreach(Bookends slot in BookSlots)
		{
			slot.OnBookendClicked -= OnBookendClicked;
		}
        base._ExitTree();
    }


	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	private void OnBookPickup(PuzzleBook book, Node3D camera, int currentSlot)
	{
		if (HeldBook != null || book.IsBookLocked())
		{
			return;
		}

		if (Books.Contains(book))
		{
			if (currentSlot > -1)
			{
				BookSlots[currentSlot].OnBookPickup();
			}
			book.MoveBookToPlayer(camera);
			HeldBook = book;
		}
	}

	private void OnBookendClicked(Bookends bookend, Node3D camera)
	{
		int idx = BookSlots.IndexOf(bookend);
		if(bookend.HasBook() && HeldBook == null)
		{
			// pickup?
			OnBookPickup(bookend.CurrentBook, camera, idx);
		}
		else if (HeldBook != null && !HeldBook.IsBookLocked())
		{
			HeldBook.Reparent(bookend);
			PuzzleBook BookFromSlot = bookend.OnBookMovedToSlot(HeldBook, idx);
			if (BookFromSlot != null)
			{
				HeldBook = null;
				OnBookPickup(BookFromSlot, camera, BookSlots.IndexOf(bookend));
			}
			HeldBook = BookFromSlot;

			CheckAndConfirmSolution();
		}
	}

	private void CheckAndConfirmSolution()
	{
		bool Solved = true;
		for(int i = 0; i < BookSlots.Count; i++)
		{
			Bookends slot = BookSlots[i];
			if (!slot.HasBook() || slot.HasBook() && slot.CurrentBook.GetID() != SolutionIDs[i])
			{
				Solved = false;
			}
		}

		if (Solved)
		{
			GD.Print("Solved!");
			SolveTrigger.TriggerEvent();
		}
		else
		{
			GD.Print("Not solved!");
		}
	}
}
