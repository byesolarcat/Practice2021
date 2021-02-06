using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Tanks
{
	public partial class Form1 : Form
	{
		public PacmanController controller = new PacmanController();

		private Bitmap wallsBitmap;

		public Form1()
		{
			InitializeComponent();
			timer1.Enabled = false;
			InitEntities();
		}

		private void InitEntities()
		{
			if (pictureBox1.Image == null)
			{
				Bitmap bmp = new Bitmap(gameField.Width, gameField.Height);
				this.pictureBox1.Image = bmp;
			}
			RenderWalls();
		}

		private void timer1_Tick(object sender, EventArgs e)
		{
			controller.SpawnTank();
			controller.SpawnApple();
			UpdateEntities();
			scoreLabel.Text = "Score: " + controller.Score;
		}

		private void UpdateEntities()
		{
			controller.MoveEntity(controller.Kolobok);
			foreach (var tank in controller.MovableEntities)
			{
				controller.MoveEntity(tank);
			}
			controller.CheckCollisions(this.ClientSize);
			
			RenderEntitites();
		}

		private void RenderEntitites()
		{
			pictureBox1.Image = wallsBitmap;
			Bitmap bmp = new Bitmap(this.pictureBox1.Image);
			using (Graphics g = Graphics.FromImage(bmp))
			{
				foreach (var entity in controller.DynamicEntities)
				{
					g.DrawImageUnscaledAndClipped(entity.CurrentImage,
						new Rectangle(new Point(entity.Coordinates.X, entity.Coordinates.Y), new Size(entity.Width, entity.Height)));
				}
				
			}
			pictureBox1.Image = bmp;
		}

		private void RenderEntity(EntityModel entity)
		{
			Bitmap bmp = new Bitmap(this.pictureBox1.Image);
			using (Graphics g = Graphics.FromImage(bmp))
			{
				g.DrawImageUnscaledAndClipped(entity.CurrentImage,
					new Rectangle(new Point(entity.Coordinates.X, entity.Coordinates.Y), new Size(36,36)));
			}
			pictureBox1.Image = bmp;
		}

		private void Form1_KeyDown(object sender, KeyEventArgs e)
		{
			controller.KeyIsDown_Handler(sender, e);
		}

		private void RenderWalls()
		{
			wallsBitmap = new Bitmap(this.pictureBox1.Image);
			using (Graphics g = Graphics.FromImage(wallsBitmap))
			{
				foreach (var wall in controller.Walls)
				{
					for(int i = 0; i < wall.Width; i += 36)
					{
						Point point = new Point(wall.Coordinates.X + i, wall.Coordinates.Y);
						g.DrawImageUnscaledAndClipped(wall.CurrentImage,
							new Rectangle(point, new Size(36, 36)));
					}
					for (int i = 0; i < wall.Height; i += 36)
					{
						Point point = new Point(wall.Coordinates.X, wall.Coordinates.Y + i);
						g.DrawImageUnscaledAndClipped(wall.CurrentImage,
							new Rectangle(point, new Size(36, 36)));
					}

				}
			}
			pictureBox1.Image = wallsBitmap;
		}

		private void startGameButton_Click(object sender, EventArgs e)
		{
			timer1.Enabled = true;
			this.ActiveControl = null;
		}
	}
}
