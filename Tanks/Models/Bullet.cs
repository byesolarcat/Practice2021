using System;
using System.Drawing;
using System.Windows.Forms;

namespace Tanks
{
	public class Bullet : EntityModel, IMovable
	{
		public Tank shooter;
		public Bullet(Position coordinates, int width, int height, Direction direction, Bitmap image, Tank shooter) : base(coordinates, width, height)
		{
			images = new Bitmap[1];

			images[0] = image;
			CurrentImage = images[0];

			this.direction = direction;
			speed = 5;

			this.shooter = shooter;
		}

		public Bullet(Position coordinates, int width, int height, Direction direction, Bitmap image) : base(coordinates, width, height)
		{
			images = new Bitmap[1];

			images[0] = image;
			CurrentImage = images[0];

			this.direction = direction;
			speed = 15;

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
		}


		public void SwitchDirection()
		{
			return;
		}
	}
}
