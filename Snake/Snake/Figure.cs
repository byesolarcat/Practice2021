using System.Collections.Generic;

namespace Snake
{
	class Figure
	{
		public List<Point> PointsList { get; set; }
		public void Draw()
		{
			foreach (var point in PointsList)
			{
				point.Draw();
			}
		}
	}
}
