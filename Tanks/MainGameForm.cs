using System;
using System.Drawing;
using System.Windows.Forms;

namespace Tanks
{
	public partial class MainGameForm : Form
	{
		public PacmanController controller = new PacmanController(5, 5);

		EntitiesInfo infoForm;

		public MainGameForm()
		{
			InitializeComponent();
			this.FormBorderStyle = FormBorderStyle.FixedSingle;
			mainGameTimer.Enabled = false;
			Bitmap bmp = new Bitmap(gameFieldPictureBox.Width, gameFieldPictureBox.Height);
			this.gameFieldPictureBox.Image = bmp;
		}

		private void UpdateEntities()
		{
			foreach (var entity in controller.MovableEntities)
			{
				controller.MoveEntity(entity);
			}
			controller.LookForCollisions(this.gameFieldPictureBox.Size);
			RenderEntitites();
		}

		private void InitGameOver()
		{
			if (infoForm != null)
			{
				infoForm.Dispose();
				infoForm = null;
			}
			startGameButton.Enabled = true;
			gameFieldPictureBox.Image = Properties.Resources.gameOver;
			Bitmap bmp = new Bitmap(this.gameFieldPictureBox.Image);
			using (Graphics g = Graphics.FromImage(bmp))
			{
				g.DrawImage(Properties.Resources.gameOver,
						new Rectangle(new Point(0, 0), this.gameFieldPictureBox.Size));

			}
		}

		private void RenderEntitites()
		{
			Bitmap bmp = new Bitmap(this.gameFieldPictureBox.Width, this.gameFieldPictureBox.Height);
			using (Graphics g = Graphics.FromImage(bmp))
			{
				foreach (var entity in controller.DrawableEntities)
				{
					entity.Draw(g);
				}
			}

			gameFieldPictureBox.Image = bmp;
		}

		private void timer1_Tick(object sender, EventArgs e)
		{
			if (!controller.GameOver)
			{
				controller.SpawnTank(this.gameFieldPictureBox.Size);
				controller.SpawnApple(this.gameFieldPictureBox.Size);
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
			controller = new PacmanController(5, 5);
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
			if (e.KeyCode == Keys.P && infoForm == null)
			{
				infoForm = new EntitiesInfo(controller);
				infoForm.StartPosition = FormStartPosition.Manual;
				infoForm.Location = new Point(this.Location.X + this.Width, this.Location.Y);
				infoForm.Show(this);
			}
			else if (e.KeyCode == Keys.P && infoForm.Visible)
			{
				infoForm.Hide();
			}
			else if (e.KeyCode == Keys.P && !infoForm.Visible)
			{
				infoForm.Show(this);
			}
			else if (e.KeyCode == Keys.Y && controller.GameOver)
			{
				InitGameOver();
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
