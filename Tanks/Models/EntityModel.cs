using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Tanks
{
	public abstract class EntityModel
	{
		public PictureBox pictureBox;
		public Bitmap[] images;

		public Position Coordinates { get; set; }
		public Direction direction;
		public int speed;
		
		public bool isBuletCollisable;

		public PictureBox CreateEntityPictureBox(Bitmap image, Point location)
		{
			pictureBox = new PictureBox();
			pictureBox.Image = image;
			pictureBox.Size = new Size(36, 36);
			pictureBox.BackColor = Color.Transparent;
			pictureBox.Location = location;

			return pictureBox;
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
	}
}
