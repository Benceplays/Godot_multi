using Godot;
using System;

public partial class Level : Node3D {

	public const float SPAWN_RANDOM = 5.0f;

	private RandomNumberGenerator rng = new RandomNumberGenerator();

	// Called when the node enters the scene tree for the first time.
	public override void _Ready() {
		if (!Multiplayer.IsServer()) {
			return;
		}

		rng.Randomize();

		Multiplayer.Connect(MultiplayerApi.SignalName.PeerConnected, new Callable(this, MethodName._AddPlayer));
		Multiplayer.Connect(MultiplayerApi.SignalName.PeerDisconnected, new Callable(this, MethodName._DeletePlayer));

		foreach(int id in Multiplayer.GetPeers()) {
			GD.Print(id);
			_AddPlayer(id);
		}

		if (!OS.HasFeature("dedicated_server")) {
			_AddPlayer(1);
		}
	}

    public override void _ExitTree() {
		if (!Multiplayer.IsServer()) {
			return;
		}

		Multiplayer.Disconnect(MultiplayerApi.SignalName.PeerConnected, new Callable(this, MethodName._AddPlayer));
		Multiplayer.Disconnect(MultiplayerApi.SignalName.PeerDisconnected, new Callable(this, MethodName._DeletePlayer));
    }

	private void _AddPlayer(int id) {
		GD.Print("Calling _AddPlayer");

		var playerScene = (PackedScene) ResourceLoader.Load("res://Player.tscn");
		Player newPlayer = playerScene.Instantiate<Player>();
		// newPlayer.input = newPlayer.GetNode<PlayerInput>("PlayerInput");
		newPlayer.player = id;
		// newPlayer.HandoffInputAuthority(id);

		Vector2 position = Vector2.FromAngle(rng.Randf() * 2 * (float) Math.PI);

		newPlayer.Position = new Vector3(position.X * SPAWN_RANDOM * rng.Randf(), 0, position.Y * SPAWN_RANDOM * rng.Randf());

		newPlayer.Name = id.ToString();

		GD.Print($"Adding {id} to scene tree");
		GetNode<Node3D>("Players").AddChild(newPlayer, true);
	}

	private void _DeletePlayer(int id) {
		Node3D players = GetNode<Node3D>("Players");

		if (!players.HasNode(id.ToString())) {
			return;
		}

		players.GetNode(id.ToString()).QueueFree();
	}
}
