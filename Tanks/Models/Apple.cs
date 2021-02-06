using System.Drawing;

namespace Tanks
{
	public class Apple : EntityModel
	{
		public Apple(Position coordinates, int width, int height) : base(coordinates, width, height)
		{
			InitImages();
			CurrentImage = images[0];
			speed = 0;

			isBuletCollisable = true;
		}

		private void InitImages()
		{
			images = new Bitmap[1];

			images[0] = Properties.Resources.apple;
		}
	}
}
