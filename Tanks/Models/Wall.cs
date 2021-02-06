using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Tanks
{
	public class Wall : EntityModel
	{
		public Wall(Position coordinates, int width, int height) : base(coordinates, width, height)
		{
			InitImages();
			CurrentImage = images[0];

			isBuletCollisable = true;
		}

		private void InitImages()
		{
			images = new Bitmap[1];

			images[0] = Properties.Resources.wall;
		}
	}
}
