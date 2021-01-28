using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Snake
{
	class Snake : Figure
	{
		Direction MovementDirection { get; set; }

		public Snake(Point tail, int length, Direction direction )
		{
			this.MovementDirection = direction;
			PointsList = new List<Point>();
			for (int i = 0; i < length; i++)
			{
				Point p = new Point(tail);
				p.Move(i, direction);
				this.PointsList.Add(p);
			}
			
		}

		public void Move()
		{
			Point tail = PointsList.First();
			PointsList.Remove(tail);
			Point head = GetNextPoint();
			PointsList.Add(head);

			tail.Clear();
			head.Draw();
		}

		private Point GetNextPoint()
		{
			Point head = PointsList.Last();
			Point nextPoint = new Point(head);
			nextPoint.Move(1, MovementDirection);
			return nextPoint;
		}
	}
}
