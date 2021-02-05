using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace Tanks
{
	public class PacmanController
	{
		public List<IMovable> movableEntities;
		public List<Tank> tanks;
		public Kolobok kolobok;

		public List<Wall> walls;

		public PacmanController()
		{
			kolobok = new Kolobok();
			movableEntities = new List<IMovable>();
			walls = new List<Wall>();
			tanks = new List<Tank>();

			movableEntities.Add(kolobok);
		}

		public void KeyIsDown_Handler(object sender, KeyEventArgs e)
		{
			switch (e.KeyCode)
			{
				case Keys.Right:
					{
						kolobok.direction = Direction.Right;
						break;
					}
				case Keys.Left:
					{
						kolobok.direction = Direction.Left;
						break;
					}
				case Keys.Up:
					{
						kolobok.direction = Direction.Up;
						break;
					}
				case Keys.Down:
					{
						kolobok.direction = Direction.Down;
						break;
					}
			}
		}

		public void CheckCollisions(Size clientSize)
		{
			CheckBounds(clientSize);

			foreach(var wall in walls)
			{
				if (IsBoxColiding(wall, kolobok))
				{
					switch (kolobok.direction)
					{
						case Direction.Right:
							{
								kolobok.pictureBox.Left -= kolobok.speed;
								break;
							}
						case Direction.Left:
							{
								kolobok.pictureBox.Left += kolobok.speed;
								break;
							}
						case Direction.Up:
							{
								kolobok.pictureBox.Top += kolobok.speed;
								break;
							}
						case Direction.Down:
							{
								kolobok.pictureBox.Top -= kolobok.speed;
								break;
							}
					}
				}
			}
		}

		public bool IsBoxColiding(EntityModel entity1, EntityModel entity2)
		{
			return !(entity1.pictureBox.Right <= entity2.pictureBox.Left ||
				entity1.pictureBox.Left > entity2.pictureBox.Right ||
				entity1.pictureBox.Bottom <= entity2.pictureBox.Top ||
				entity1.pictureBox.Top > entity2.pictureBox.Bottom);
		}

		private void CheckBounds(Size clientSize)
		{
			if(kolobok.pictureBox.Bottom > clientSize.Height)
			{
				kolobok.pictureBox.Top = clientSize.Height - kolobok.pictureBox.Height;
			}
			else if (kolobok.pictureBox.Top < 0)
			{
				kolobok.pictureBox.Top = 0;
			}
			else if (kolobok.pictureBox.Left < 0)
			{
				kolobok.pictureBox.Left = 0;
			}
			else if (kolobok.pictureBox.Right > clientSize.Width)
			{
				kolobok.pictureBox.Left = clientSize.Width - kolobok.pictureBox.Height;
			}
		}

		public void MoveEntity(IMovable entity)
		{
			entity.Move();
		}
	}
}
