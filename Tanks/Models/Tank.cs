using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tanks
{
	public class Tank : EntityModel, IMovable
	{
		Random rnd = new Random();

		public Tank(Position coordinates, int width, int height) : base(coordinates, width, height)
		{
			direction = Direction.Right;
			speed = 1;

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
