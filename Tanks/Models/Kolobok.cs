using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Tanks
{
	public class Kolobok: EntityModel, IMovable
	{
		public Kolobok()
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

			images[0] = Properties.Resources.kolobok_up;
			images[1] = Properties.Resources.kolobok_down;
			images[2] = Properties.Resources.kolobok_left;
			images[3] = Properties.Resources.kolobok_right;
		}
	}
}
