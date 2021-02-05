using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tanks
{
	public class Tank: EntityModel, IMovable
	{
		public Tank()
		{
			direction = Direction.Right;
			speed = 1;

			InitImages();
		}

		public void Move()
		{
			Random rnd = new Random();
			direction = (Direction) rnd.Next(1, 4);
			switch (direction)
			{
				case Direction.Right:
					{
						pictureBox.Left += speed;
						break;
					}
				case Direction.Left:
					{
						pictureBox.Left -= speed;
						break;
					}
				case Direction.Up:
					{
						pictureBox.Top -= speed;
						break;
					}
				case Direction.Down:
					{
						pictureBox.Top += speed;
						break;
					}
			}
			int index = (int)direction;
			pictureBox.Image = images[index];
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
