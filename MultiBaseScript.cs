using Godot;
using System;

public partial class MultiBaseScript : Node
{
	private ENetMultiplayerPeer multi;
	private LineEdit ip;
	private LineEdit port;

	public override void _Ready()
	{
		multi = new ENetMultiplayerPeer();
		ip = GetNode("ip") as LineEdit;
		port = GetNode("port") as LineEdit;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
	public void _on_join_pressed()
	{
		multi.CreateClient(ip.Text, Convert.ToInt32(port.Text));
		Multiplayer.MultiplayerPeer = multi;

		/*var id = Multiplayer.GetUniqueId();
		SetMultiplayerAuthority(id);
		IsMultiplayerAuthority();
		
		https://www.youtube.com/watch?v=3xgDU4gRLps
		https://godotengine.org/qa/143874/help-migration-c%23-code-from-3-5-to-4-0*/
		

		if (multi.GetConnectionStatus() == MultiplayerPeer.ConnectionStatus.Disconnected)
		{
			GD.Print("Nem sikerult");
			return;
		}
		StartGame();
	} 
	public void _on_host_pressed()
	{
		multi.CreateServer(Convert.ToInt32(port.Text));
		Multiplayer.MultiplayerPeer = multi;
		if (multi.GetConnectionStatus() == MultiplayerPeer.ConnectionStatus.Disconnected)
		{
			GD.Print("Nem sikerult");
			return;
		}
		StartGame();
	}
	public void StartGame()
	{
		GetTree().ChangeSceneToFile("res://lobby.tscn");
		GD.Print("Sikerult a kapcsolat");
	}
}
