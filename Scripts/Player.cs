using Godot;
using System;

public partial class Player : CharacterBody2D
{
	public const float Speed = 300.0f;
	public const float JumpVelocity = -400.0f;

	// Ссылка на наш AnimatedSprite2D
	private AnimatedSprite2D _animatedSprite;

	public override void _Ready()
	{
		// Получаем доступ к узлу анимации
		_animatedSprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
	}

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

		// 3. Движение и логика анимаций
		Vector2 direction = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down");
		
		if (direction != Vector2.Zero)
		{
			velocity.X = direction.X * Speed;
			
			// Разворот спрайта (теперь через AnimatedSprite2D, а не Sprite2D)
			if (direction.X > 0)
				_animatedSprite.FlipH = false; 
			else if (direction.X < 0)
				_animatedSprite.FlipH = true;  
		}
		else
		{
			velocity.X = Mathf.MoveToward(Velocity.X, 0, Speed);
		}

		Velocity = velocity;
		MoveAndSlide();

		// --- ЛОГИКА АНИМАЦИИ ---
		UpdateAnimation(direction);
	}

	private void UpdateAnimation(Vector2 direction)
	{
		// Если мы в воздухе (прыгаем или падаем)
		if (!IsOnFloor())
		{
			_animatedSprite.Play("Jump");
		}
		// Если мы на земле и движемся
		else if (direction.X != 0)
		{
			_animatedSprite.Play("Run"); // Или "Walk", если хочешь медленную ходьбу
		}
		// Если мы на земле и стоим
		else
		{
			// Если анимации Idle нет, используй первый кадр Walk или Run
			// (если добавишь тег "Idle" в Aseprite, поменяй здесь на "Idle")
			_animatedSprite.Play("Walk"); 
			_animatedSprite.Pause(); // Останавливаем на первом кадре
		}
	}
}