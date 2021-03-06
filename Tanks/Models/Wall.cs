﻿using System.Drawing;

namespace Tanks
{
	public class Wall : EntityModel, IDrawable
	{
		public int? Health { get; set; }
		public Wall(Position coordinates, int width, int height) : base(coordinates, width, height)
		{
			InitImages();
			CurrentImage = images[0];

			Health = 1;
		}

		public void Draw(Graphics g)
		{
			g.DrawImage(CurrentImage,
						new Rectangle(new Point(Coordinates.X, Coordinates.Y), new Size(Width, Height)));
		}

		private void InitImages()
		{
			images = new Bitmap[1];

			images[0] = Properties.Resources.wall;
		}
	}
}
