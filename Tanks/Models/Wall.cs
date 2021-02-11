using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Tanks
{
	public class Wall : EntityModel, IDrawable
	{

		public int Health { get; set; }
		public Wall(Position coordinates, int width, int height) : base(coordinates, width, height)
		{
			InitImages();
			CurrentImage = images[0];

			Health = 40;
		}

		public void Draw(Graphics g)
		{
			for (int i = 0; i < Width; i += 36)
			{
				Point point = new Point(Coordinates.X + i, Coordinates.Y);
				g.DrawImageUnscaledAndClipped(CurrentImage,
					new Rectangle(point, new Size(36, 36)));
			}
			for (int i = 0; i < Height; i += 36)
			{
				Point point = new Point(Coordinates.X, Coordinates.Y + i);
				g.DrawImageUnscaledAndClipped(CurrentImage,
					new Rectangle(point, new Size(36, 36)));
			}
		}

		private void InitImages()
		{
			images = new Bitmap[1];

			images[0] = Properties.Resources.wall;
		}
	}
}
