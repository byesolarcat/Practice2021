using System;
using System.Collections.Generic;
using System.Text;

namespace Snake
{
	class Snake : Figure
	{
		public Snake(Point tail, int length, Direction direction )
		{
			PointsList = new List<Point>();
			for (int i = 0; i < length; i++)
			{
				Point p = new Point(tail);
				p.Move(i, direction);
				this.PointsList.Add(p);
				
			}
			
		}
	}
}
