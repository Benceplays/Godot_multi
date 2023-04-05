using Godot;
using System;

public partial class Multiplayer : Node {
    private const int PORT = 4433;
    public ENetMultiplayerPeer network;
    public Control ui;
    public static LineEdit NameLabel;

    public override void _Ready() {
        NameLabel = GetNode<LineEdit>("UI/Net/Options/Name");
        ui = GetNode<Control>("UI");
        // GetTree().Paused = true;

        if (DisplayServer.GetName() == "headless") {
            GD.Print("Automatically starting dedicated server.");
            CallDeferred(MethodName._OnHostPressed);
        }
    }

    private void _OnHostPressed() {
        GD.Print("OnHostPressed");
        network = new ENetMultiplayerPeer();
        network.CreateServer(PORT);

        if (network.GetConnectionStatus() == MultiplayerPeer.ConnectionStatus.Disconnected) {
            OS.Alert("Failed to start multiplayer server.");
            return;
        }

        Multiplayer.MultiplayerPeer = network;
        StartGame();
    }

    private void _OnConnectPressed() {
        GD.Print("OnConnectPressed");
        String text = GetNode<LineEdit>("UI/Net/Options/Remote").Text;

        if (text == "") {
            OS.Alert("Need a remote to connect to.");
            return;
        }

        network = new ENetMultiplayerPeer();
        network.CreateClient(text, PORT);

        if (network.GetConnectionStatus() == MultiplayerPeer.ConnectionStatus.Disconnected) {
            OS.Alert("Failed to start multiplayer client");
            return;
        }

        Multiplayer.MultiplayerPeer = network;
        StartGame();
    }

    public void StartGame() {
        GD.Print("StartGame");
        ui.Hide();
        GetTree().Paused = false;
        if (Multiplayer.IsServer()) {
            CallDeferred(MethodName._ChangeLevel, ((PackedScene) ResourceLoader.Load("res://Level.tscn")));
        }
    }

    private void _ChangeLevel(PackedScene scene) {
        Node level = GetNode<Node>("Level");
        foreach (Node3D child in level.GetChildren()) {
            level.RemoveChild(child);
            child.QueueFree();
        }
        level.AddChild(scene.Instantiate());
    }

    public override void _Input(InputEvent inputEvent) {
        if (!Multiplayer.IsServer()) {
            return;
        }

        if (inputEvent.IsAction("ui_home") && Input.IsActionJustPressed("ui_home")) {
            CallDeferred(MethodName._ChangeLevel, (PackedScene)ResourceLoader.Load("res://Level.tscn"));
        }
    }
}
