using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Tanks
{
	public class Kolobok : EntityModel, IMovable
	{
		public Kolobok(Position coordinates, int width, int height) : base(coordinates, width, height)
		{
			InitImages();

			isBuletCollisable = true;

			direction = Direction.Right;
			speed = 1;
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

		public Bullet Shoot()
		{
			switch (direction)
			{
				case Direction.Right:
					{
						return new Bullet(
							new Position(Right, Top + Width / 2),
							4, 4, direction, Properties.Resources.Bullet);
					}
				case Direction.Left:
					{
						return new Bullet(
							new Position(Left, Top + Width / 2),
							4, 4, direction, Properties.Resources.Bullet);
					}
				case Direction.Up:
					{
						return new Bullet(
							new Position(Left + Width / 2, Top),
							4, 4, direction, Properties.Resources.Bullet);
					}
				case Direction.Down:
					{
						return new Bullet(
							new Position(Left + Width / 2, Bottom),
							4, 4, direction, Properties.Resources.Bullet);
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

			images[0] = Properties.Resources.kolobok_up;
			images[1] = Properties.Resources.kolobok_down;
			images[2] = Properties.Resources.kolobok_left;
			images[3] = Properties.Resources.kolobok_right;
		}
	}
}
