using Godot;
using System;

public partial class PlayerInput : MultiplayerSynchronizer
{
	private bool _jumping = false;
	[Export]
	public bool jumping {get; set;}

	private Vector2 _direction = Vector2.Zero;
	[Export]
	public Vector2 direction {get; set;}

	public override void _EnterTree() {

	}
	// Called when the node enters the scene tree for the first time.
	public override void _Ready() {
		GD.Print($"MultiplayerAuthority: {GetMultiplayerAuthority()}\n Network ID: {Multiplayer.GetUniqueId()}");


		SetProcess(GetMultiplayerAuthority() == Multiplayer.GetUniqueId());
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta) {
		direction = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down");

		if (Input.IsActionJustPressed("ui_accept")) {
			Rpc(MethodName.Jump);
		}
	}

	[Rpc] // this may need further specification
	public void Jump() {
		jumping = true;
	}

}
