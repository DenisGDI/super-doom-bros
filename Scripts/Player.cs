using Godot;
using System;

public partial class Player : CharacterBody2D
{
	public const float Speed = 300.0f;
	public const float JumpVelocity = -400.0f;

	public override void _PhysicsProcess(double delta)
	{
		Vector2 velocity = Velocity;

		// 1. Гравитация
		if (!IsOnFloor())
		{
			velocity += GetGravity() * (float)delta;
		}

		// 2. Прыжок
		if (Input.IsActionJustPressed("ui_accept") && IsOnFloor())
		{
			velocity.Y = JumpVelocity;
		}

		// 3. Движение влево/вправо
		Vector2 direction = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down");
		if (direction != Vector2.Zero)
		{
			velocity.X = direction.X * Speed;
			
			// --- НОВАЯ ЛОГИКА: Разворот спрайта ---
			var sprite = GetNode<Sprite2D>("Sprite2D"); // Ищем дочерний узел Sprite2D
			if (direction.X > 0)
				sprite.FlipH = false; // Смотрит вправо
			else if (direction.X < 0)
				sprite.FlipH = true;  // Смотрит влево (отзеркален)
			// --------------------------------------
		}
		else
		{
			velocity.X = Mathf.MoveToward(Velocity.X, 0, Speed);
		}

		Velocity = velocity;
		MoveAndSlide();
	}
}