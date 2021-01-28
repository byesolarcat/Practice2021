using System;
using System.Collections.Generic;

namespace Snake
{
	class Figure
	{
		public List<Point> PointsList = new List<Point>();
		public virtual void Draw()
		{
			foreach (var point in PointsList)
			{
				point.Draw();
			}
		}

		public bool IsHit(Figure figure)
		{
			foreach (var p in PointsList)
			{
				if (figure.IsHit(p))
					return true;
			}
			return false;
		}

		private bool IsHit(Point point)
		{
			foreach (var p in PointsList)
			{
				if (p.IsHit(point))
					return true;
			}
			return false;
		}
	}
}
