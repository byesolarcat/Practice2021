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
		protected Bitmap[] images;
		public Bitmap CurrentImage;

		public Position Coordinates { get; set; } //Top-left corner

		public int Left => Coordinates.X;
		public int Right => Coordinates.X + Width;
		public int Top => Coordinates.Y;
		public int Bottom => Coordinates.Y + Height;

		public int Width { get; }
		public int Height { get; }

		public Direction direction;
		public int speed;
		
		public bool isBuletCollisable;

		public EntityModel(Position coordinates, int width, int height)
		{
			this.Coordinates = coordinates;
			this.Width = width;
			this.Height = height;
		}
	}
}
