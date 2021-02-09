using System;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace Tanks
{
	public partial class MainGameForm : Form
	{
		public PacmanController controller = new PacmanController();

		private Bitmap wallsBitmap;

		EntitiesInfo infoForm;

		public MainGameForm()
		{
			InitializeComponent();
			mainGameTimer.Enabled = false;
			Bitmap bmp = new Bitmap(gameFieldPictureBox.Width, gameFieldPictureBox.Height);
			this.gameFieldPictureBox.Image = bmp;
		}

		private void InitGameField()
		{
			controller = new PacmanController();
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
			if (infoForm  != null )infoForm.Dispose();
			startGameButton.Enabled = true;
			gameFieldPictureBox.Image = Properties.Resources.gameOver;
			Bitmap bmp = new Bitmap(this.gameFieldPictureBox.Image);
			using (Graphics g = Graphics.FromImage(bmp))
			{
				g.DrawImageUnscaledAndClipped(Properties.Resources.gameOver,
						new Rectangle(new Point(0, 0), new Size(100, 100)));

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

		private void startGameButton_Click(object sender, EventArgs e)
		{
			InitGameField();
			mainGameTimer.Enabled = true;
			this.ActiveControl = null;
			startGameButton.Enabled = false;
		}

		private void shootingTimer_Tick(object sender, EventArgs e)
		{
			foreach (var tank in controller.Tanks)
			{
				controller.CheckEnemyAhead(tank);
			}
		}

		private void MainGameForm_KeyDown(object sender, KeyEventArgs e)
		{
			controller.KeyIsDown_Handler(sender, e);
			if (e.KeyCode == Keys.P)
			{
				infoForm = new EntitiesInfo(controller);
				infoForm.StartPosition = FormStartPosition.Manual;
				infoForm.Location = new Point(this.Location.X + this.Width, this.Location.Y);
				infoForm.Show(this);
			} 
			else if (e.KeyCode == Keys.Y && controller.GameOver)
			{
				this.gameFieldPictureBox.Image = wallsBitmap;
				InitGameField();
			}
		}

		protected override void OnLostFocus(EventArgs e)
		{
			base.OnLostFocus(e);
			this.Focus();
		}

		protected override void OnDeactivate(EventArgs e)
		{
			base.OnDeactivate(e);
			this.Focus();
		}

		private void label1_Click(object sender, EventArgs e)
		{

		}
	}
}
