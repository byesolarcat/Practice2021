using System.ComponentModel;
using System.Drawing;

namespace Tanks
{
	public abstract class EntityModel
	{
		protected Bitmap[] images;
		[Browsable(false)]
		public Bitmap CurrentImage;
		[Browsable(false)]
		public Position Coordinates { get; set; } //Top-left corner

		public int Left => Coordinates.X;
		[Browsable(false)]
		public int Right => Coordinates.X + Width;
		public int Top => Coordinates.Y;
		[Browsable(false)]
		public int Bottom => Coordinates.Y + Height;

		[Browsable(false)]
		public int Width { get; }
		[Browsable(false)]
		public int Height { get; }

		[Browsable(false)]
		public Direction direction;
		[Browsable(false)]
		public int speed;
		
		public string Title { get; protected set; }

		public EntityModel(Position coordinates, int width, int height)
		{
			this.Coordinates = coordinates;
			this.Width = width;
			this.Height = height;
		}
	}
}
