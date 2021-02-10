using System.Drawing;

namespace Tanks
{
	public class Apple : EntityModel, IDrawable
	{
		public Apple(Position coordinates, int width, int height) : base(coordinates, width, height)
		{
			InitImages();
			CurrentImage = images[0];
			speed = 0;

			Title = "Apple";
		}

		public void Draw(Graphics g)
		{
			g.DrawImageUnscaledAndClipped(CurrentImage,
						new Rectangle(new Point(Coordinates.X, Coordinates.Y), new Size(Width, Height)));
		}

		private void InitImages()
		{
			images = new Bitmap[1];

			images[0] = Properties.Resources.apple;
		}
	}
}
