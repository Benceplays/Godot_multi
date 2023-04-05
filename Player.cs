using Godot;
using System;

public partial class Player : CharacterBody3D {
    public const float SPEED = 5.0f;
    public const float JUMP_VELOCITY = 4.5f;

    //public float gravity = (float) ProjectSettings.GetSetting("physics/3d/default_gravity");

    public PlayerInput input;
    public string Nev {get; set;}
	private Camera3D kamera;
    private Label3D NevLabel;
    private int _player = 1;
    [Export]
    public int player {
        get {
            return _player;
        }
        set {
            _player = value;
            GetNode<PlayerInput>("PlayerInput").SetMultiplayerAuthority(_player);
        }
    }

    public override void _EnterTree()
    {
        input = GetNode<PlayerInput>("PlayerInput");
    }

    public override void _Ready() {
        NevLabel = GetNode<Label3D>("NameLabel");
        NevLabel.Text = Nev;
        GD.Print("Player._Ready, _player = ", player);

        if (player == Multiplayer.GetUniqueId()) {
            GetNode<Camera3D>("Camera3D").Current = true;
        }

        GD.Print(input, input.jumping);
		kamera = GetNode("Camera3D") as Camera3D;
		Input.MouseMode = Input.MouseModeEnum.Captured;
    }

    public override void _Process(double delta)
    {
        
		RotateY(input.relative.X * lookSensitivity);
		kamera.RotateX(input.relative.Y * lookSensitivity);
		kamera.Rotation = new Vector3((float) Math.Clamp(kamera.Rotation.X,  -Math.PI/2, Math.PI/2), 0, 0);
    }
    public override void _PhysicsProcess(double delta) {
        /*if (!IsOnFloor()) {
            tempVelocity.Y -= gravity * (float) delta;
        }

        if (input.jumping && IsOnFloor()) {
            tempVelocity.Y = JUMP_VELOCITY;
        }

        input.jumping = false;*/

        Vector3 direction = (Transform.Basis * new Vector3(input.direction.X, 0, input.direction.Y)).Normalized();
        UpdatePosition(direction);
    }

	private void UpdatePosition(Vector3 direction)
	{
        Vector3 tempVelocity = Velocity;
        if (direction != Vector3.Zero) {
            tempVelocity.X = direction.X * SPEED;
            tempVelocity.Z = direction.Z * SPEED;
        }

        else {
            tempVelocity.X = Mathf.MoveToward(tempVelocity.X, 0, SPEED);
            tempVelocity.Z = Mathf.MoveToward(tempVelocity.Z, 0, SPEED);
        }

        Velocity = tempVelocity;
        MoveAndSlide();
	}
    float lookSensitivity = 0.005f; // 
    // public void HandoffInputAuthority(int id)
    // {
    //     player = id;
    //     GetNode<PlayerInput>("PlayerInput").SetMultiplayerAuthority(id);
    // }

}
