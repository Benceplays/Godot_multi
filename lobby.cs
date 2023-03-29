using Godot;
using System;

public partial class lobby : Node2D
{
	[Export]
	private PackedScene psCharacter;
	public override void _Ready()
	{
		Random rnd = new Random();
		Node2D new_player = (Node2D)psCharacter.Instantiate();
		new_player.Name = Convert.ToString(GetTree().GetInstanceId());
		GD.Print(GetTree().GetInstanceId());
		new_player.Position = new Vector2(rnd.Next(0, 100), rnd.Next(0, 100));
		AddChild(new_player);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
