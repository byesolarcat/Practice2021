
namespace Tanks
{
	partial class Form1
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			this.mainGameTimer = new System.Windows.Forms.Timer(this.components);
			this.gameFieldPictureBox = new System.Windows.Forms.PictureBox();
			this.scoreLabel = new System.Windows.Forms.Label();
			this.startGameButton = new System.Windows.Forms.Button();
			this.shootingTimer = new System.Windows.Forms.Timer(this.components);
			((System.ComponentModel.ISupportInitialize)(this.gameFieldPictureBox)).BeginInit();
			this.SuspendLayout();
			// 
			// mainGameTimer
			// 
			this.mainGameTimer.Enabled = true;
			this.mainGameTimer.Interval = 25;
			this.mainGameTimer.Tick += new System.EventHandler(this.timer1_Tick);
			// 
			// gameFieldPictureBox
			// 
			this.gameFieldPictureBox.BackColor = System.Drawing.Color.Black;
			this.gameFieldPictureBox.Location = new System.Drawing.Point(0, 107);
			this.gameFieldPictureBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this.gameFieldPictureBox.Name = "gameFieldPictureBox";
			this.gameFieldPictureBox.Size = new System.Drawing.Size(683, 559);
			this.gameFieldPictureBox.TabIndex = 1;
			this.gameFieldPictureBox.TabStop = false;
			// 
			// scoreLabel
			// 
			this.scoreLabel.AutoSize = true;
			this.scoreLabel.Font = new System.Drawing.Font("Segoe UI", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.scoreLabel.Location = new System.Drawing.Point(16, 11);
			this.scoreLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.scoreLabel.Name = "scoreLabel";
			this.scoreLabel.Size = new System.Drawing.Size(110, 37);
			this.scoreLabel.TabIndex = 2;
			this.scoreLabel.Text = "Score: 0";
			// 
			// startGameButton
			// 
			this.startGameButton.Location = new System.Drawing.Point(289, 49);
			this.startGameButton.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this.startGameButton.Name = "startGameButton";
			this.startGameButton.Size = new System.Drawing.Size(100, 28);
			this.startGameButton.TabIndex = 3;
			this.startGameButton.Text = "Start Game";
			this.startGameButton.UseVisualStyleBackColor = true;
			this.startGameButton.Click += new System.EventHandler(this.startGameButton_Click);
			// 
			// shootingTimer
			// 
			this.shootingTimer.Enabled = true;
			this.shootingTimer.Interval = 1000;
			this.shootingTimer.Tick += new System.EventHandler(this.shootingTimer_Tick);
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.White;
			this.ClientSize = new System.Drawing.Size(680, 663);
			this.Controls.Add(this.startGameButton);
			this.Controls.Add(this.gameFieldPictureBox);
			this.Controls.Add(this.scoreLabel);
			this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this.Name = "Form1";
			this.Text = "Form1";
			this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyDown);
			((System.ComponentModel.ISupportInitialize)(this.gameFieldPictureBox)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion
		private System.Windows.Forms.Timer mainGameTimer;
		private System.Windows.Forms.PictureBox gameFieldPictureBox;
		private System.Windows.Forms.Label scoreLabel;
		private System.Windows.Forms.Button startGameButton;
		private System.Windows.Forms.Timer shootingTimer;
	}
}

