using System;
using System.Drawing;
using System.Threading;
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
			mainGameTimer.Enabled = false;
			InitGameField();
		}

		private void InitGameField()
		{
			if (gameFieldPictureBox.Image == null)
			{
				Bitmap bmp = new Bitmap(gameFieldPictureBox.Width, gameFieldPictureBox.Height);
				this.gameFieldPictureBox.Image = bmp;
			}
			RenderWalls();
		}

		private void UpdateEntities()
		{
			foreach (var entity in controller.MovableEntities)
			{
				controller.MoveEntity(entity);
			}
			controller.CheckCollisions(this.gameFieldPictureBox.Size);
			RenderEntitites();
		}

		private void InitGameOver()
		{
			gameFieldPictureBox.Image = Properties.Resources.gameOver;
			Bitmap bmp = new Bitmap(this.gameFieldPictureBox.Image);
			using (Graphics g = Graphics.FromImage(bmp))
			{
				g.DrawImageUnscaledAndClipped(Properties.Resources.gameOver,
						new Rectangle(new Point(0, 0), gameFieldPictureBox.Size));

			}
		}

		private void RenderEntitites()
		{
			gameFieldPictureBox.Image = wallsBitmap;
			Bitmap bmp = new Bitmap(this.gameFieldPictureBox.Image);
			using (Graphics g = Graphics.FromImage(bmp))
			{
				foreach (var entity in controller.DrawableEntities)
				{
					g.DrawImageUnscaledAndClipped(entity.CurrentImage,
						new Rectangle(new Point(entity.Coordinates.X, entity.Coordinates.Y), new Size(entity.Width, entity.Height)));
				}
				
			}
			gameFieldPictureBox.Image = bmp;
		}

		private void RenderWalls()
		{
			wallsBitmap = new Bitmap(this.gameFieldPictureBox.Image);
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
			gameFieldPictureBox.Image = wallsBitmap;
		}

		private void timer1_Tick(object sender, EventArgs e)
		{
			if (!controller.GameOver)
			{
				controller.SpawnTank();
				controller.SpawnApple();
				UpdateEntities();
				scoreLabel.Text = "Score: " + controller.Score;
			}
			else
			{
				InitGameOver();
			}
			
		}

		private void Form1_KeyDown(object sender, KeyEventArgs e)
		{
			controller.KeyIsDown_Handler(sender, e);
		}

		private void startGameButton_Click(object sender, EventArgs e)
		{
			mainGameTimer.Enabled = true;
			this.ActiveControl = null;
		}

		private void shootingTimer_Tick(object sender, EventArgs e)
		{
			foreach (var tank in controller.Tanks)
			{
				controller.CheckEnemyAhead(tank);
			}
		}
	}
}
