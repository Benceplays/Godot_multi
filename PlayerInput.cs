using Godot;
using System;

public partial class PlayerInput : MultiplayerSynchronizer
{
	[Export] public bool jumping {get; set;}
	[Export] public Vector2 direction {get; set;}
	[Export] public Vector2 relative {get; set;} 

	public override void _EnterTree() {

	}
	// Called when the node enters the scene tree for the first time.
	public override void _Ready() {
		GD.Print($"MultiplayerAuthority: {GetMultiplayerAuthority()}\n Network ID: {Multiplayer.GetUniqueId()}");
		SetProcess(GetMultiplayerAuthority() == Multiplayer.GetUniqueId());
		//SetProcessInput(GetMultiplayerAuthority() == Multiplayer.GetUniqueId());
	}

	[Rpc(CallLocal = true)]
	public void Jump() {
		jumping = true;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta) {
		direction = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down");

		if (Input.IsActionJustPressed("ui_accept")) {
			Rpc(MethodName.Jump);
		}
	}
	/*public override void _Input(InputEvent @event)
	{
		if (@event is InputEventMouseMotion eventMouseMotion)
		{
			relative = -eventMouseMotion.Relative;
		}
	}*/

}
