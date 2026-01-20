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
        base._ExitTree();
    }


	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	private void OnBookPickup(PuzzleBook book, int currentSlot)
	{
		if (Books.Contains(book))
		{
			GD.Print("Found it, picked it up!");
			if (currentSlot > -1)
			{
				BookSlots[currentSlot].OnBookPickup();
			}
			HeldBook = book;
		}
	}

	private void OnBookendClicked(Bookends bookend)
	{
		GD.Print("bookend clicked");
		if(bookend.HasBook() && HeldBook == null)
		{
			// pickup?			
			bookend.CurrentBook.Visible = false;
			OnBookPickup(bookend.CurrentBook, BookSlots.IndexOf(bookend));
		}
		else if (HeldBook != null)
		{
			GD.Print("and we have the book");
			//HeldBook.OnBookMovedToSlot(bookend.SlotPosition);
			bookend.OnBookMovedToSlot(HeldBook);
			HeldBook = null;
		}
	}
}
