using System;
using System.Drawing;

namespace Tanks
{
	public class Tank : EntityModel, IMovable, IDrawable
	{
		private static int id;
		Random rnd = new Random();

		public Tank(Position coordinates, int width, int height) : base(coordinates, width, height)
		{
			direction = Direction.Right;
			speed = 1;

			Title = "Tank #" + id++;

			InitImages();
		}

		public void Move()
		{
			switch (direction)
			{
				case Direction.Right:
					{
						Coordinates.X += speed;
						break;
					}
				case Direction.Left:
					{
						Coordinates.X -= speed;
						break;
					}
				case Direction.Up:
					{
						Coordinates.Y -= speed;
						break;
					}
				case Direction.Down:
					{
						Coordinates.Y += speed;
						break;
					}
			}
			int index = (int)direction;
			CurrentImage = images[index];
		}

		public void SwitchDirection()
		{
			Random rnd = new Random();
			if ((rnd.NextDouble() < 1 - 0.1))
			{
				direction = (Direction)rnd.Next(1, 4);
			}
		}

		public void Turn180()
		{
			switch (direction)
			{
				case Direction.Right:
					{
						direction = Direction.Left;
						break;
					}
				case Direction.Left:
					{
						direction = Direction.Right;
						break;
					}
				case Direction.Up:
					{
						direction = Direction.Down;
						break;
					}
				case Direction.Down:
					{
						direction = Direction.Up;
						break;
					}
			}
		}

		public void Draw(Graphics g)
		{
			g.DrawImageUnscaledAndClipped(CurrentImage,
						new Rectangle(new Point(Coordinates.X, Coordinates.Y), new Size(Width, Height)));
		}
		public Bullet Shoot()
		{
			switch (direction)
			{
				case Direction.Right:
					{
						return new Bullet(
							new Position(Right, Top + Width / 2),
							4, 4, direction, Properties.Resources.Bullet, this);
					}
				case Direction.Left:
					{
						return new Bullet(
							new Position(Left, Top + Width / 2),
							4, 4, direction, Properties.Resources.Bullet, this);
					}
				case Direction.Up:
					{
						return new Bullet(
							new Position(Left + Width / 2, Top),
							4, 4, direction, Properties.Resources.Bullet, this);
					}
				case Direction.Down:
					{
						return new Bullet(
							new Position(Left + Width / 2, Bottom),
							4, 4, direction, Properties.Resources.Bullet, this);
					}
				default:
					{
						return null;
					}
			}
		}

		public Bullet ShootInvisibleBullet()
		{
			switch (direction)
			{
				case Direction.Right:
					{
						return new Bullet(
							new Position(Right, Top + Width / 2),
							4, 4, direction, new Bitmap(4, 4), this);
					}
				case Direction.Left:
					{
						return new Bullet(
							new Position(Left, Top + Width / 2),
							4, 4, direction, new Bitmap(4, 4), this);
					}
				case Direction.Up:
					{
						return new Bullet(
							new Position(Left + Width / 2, Top),
							4, 4, direction, new Bitmap(4, 4), this);
					}
				case Direction.Down:
					{
						return new Bullet(
							new Position(Left + Width / 2, Bottom),
							4, 4, direction, new Bitmap(4, 4), this);
					}
				default:
					{
						return null;
					}
			}
		}

		private void InitImages()
		{
			images = new Bitmap[4];

			images[0] = Properties.Resources.tank_up;
			images[1] = Properties.Resources.tank_down;
			images[2] = Properties.Resources.tank_left;
			images[3] = Properties.Resources.tank_right;
		}
	}
}
