﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tanks
{
	class Explosion: EntityModel, IDrawable
	{
		private int currentImageNum;
		public Explosion(Position coordinates, int width, int height, Direction direction) : base(coordinates, width, height)
		{
			InitImages();
			currentImageNum = 0;
		}

		public void Draw(Graphics g)
		{
			//for (int i = 0; i < images.Length; i++)
			//{
			//	g.DrawImageUnscaledAndClipped(images[i],
			//			new Rectangle(new Point(Coordinates.X, Coordinates.Y), new Size(Width, Height)));
			//}
			CurrentImage = images[currentImageNum];
			g.DrawImageUnscaledAndClipped(CurrentImage,
						new Rectangle(new Point(Coordinates.X, Coordinates.Y), new Size(Width, Height)));
			if (currentImageNum != 12) currentImageNum++;
			
		}

		private void InitImages()
		{
			images = new Bitmap[13];
			images[0] = Properties.Resources.explosion_start;
			images[1] = Properties.Resources.explosion_start;
			images[2] = Properties.Resources.explosion_start;
			images[3] = Properties.Resources.explosion_start;
			images[4] = Properties.Resources.explosion_middle;
			images[5] = Properties.Resources.explosion_middle;
			images[6] = Properties.Resources.explosion_middle;
			images[7] = Properties.Resources.explosion_middle;
			images[8] = Properties.Resources.explosion_end;
			images[9] = Properties.Resources.explosion_end;
			images[10] = Properties.Resources.explosion_end;
			images[11] = Properties.Resources.explosion_end;
			images[12] = new Bitmap(Width, Height);
		}
	}
}
