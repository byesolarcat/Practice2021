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
		public List<PictureBox> pictureBoxes = new List<PictureBox>();
		public PacmanController controller = new PacmanController();

		Random rnd = new Random();

		public Form1()
		{
			InitializeComponent();
			InitEntities();
		}

		private void InitEntities()
		{
			this.Controls.Add(controller.kolobok.CreateEntityPictureBox
				(new Bitmap(controller.kolobok.images[0]), new Point(170, 420)));
			BuildWalls();
			//foreach (var entity in controller.entities)
			//{
			//	this.Controls.Add(entity.CreateEntityPictureBox(new Bitmap(entity.images[0]), new Point(300, 300)));
			//}

			
		}

		private void Form1_Load(object sender, EventArgs e)
		{

		}

		private void timer1_Tick(object sender, EventArgs e)
		{
			if (controller.tanks.Count < 3 && (rnd.Next(0, 1) < 1 - 0.9999))
			{
				int xCoord = rnd.Next(0, 477);
				int yCoord = rnd.Next(0, 420);

				Tank tank = new Tank();
				PictureBox newTankBox = new PictureBox();
				newTankBox.Visible = false;
				newTankBox = tank.CreateEntityPictureBox
					(new Bitmap(tank.images[0]), new Point(xCoord, yCoord));


				bool shouldSpawn = false;
				foreach(var wall in controller.walls)
				{
					if (controller.IsBoxColiding(tank, wall))
					{
						shouldSpawn = false;
						break;
					}
					else
					{
						shouldSpawn = true;
					}
				}
				if(shouldSpawn)
				{
					this.Controls.Add(newTankBox);
					tank.pictureBox = newTankBox;
					newTankBox.Visible = true;
					controller.tanks.Add(tank);
				}
			}
			UpdateEntities();
		}

		private void UpdateEntities()
		{
			foreach(var entity in controller.movableEntities)
			{
				controller.MoveEntity(entity);
			}
			controller.CheckCollisions(this.ClientSize);
		}

		

		private void Form1_KeyDown(object sender, KeyEventArgs e)
		{
			controller.KeyIsDown_Handler(sender, e);
		}

		private void BuildWalls()
		{
			Wall wall1 = new Wall(this.wallBox1);
			Wall wall2 = new Wall(this.wallBox2);
			Wall wall3 = new Wall(this.wallBox3);
			Wall wall4 = new Wall(this.wallBox4);
			Wall wall5 = new Wall(this.wallBox5);
			Wall wall6 = new Wall(this.wallBox6);
			Wall wall7 = new Wall(this.wallBox7);
			Wall wall8 = new Wall(this.wallBox8);
			Wall wall9 = new Wall(this.wallBox9);
			Wall wall10 = new Wall(this.wallBox10);
			Wall wall11 = new Wall(this.wallBox11);
			Wall wall12 = new Wall(this.wallBox12);
			Wall wall13 = new Wall(this.wallBox13);
			Wall wall14 = new Wall(this.wallBox14);
			Wall wall15 = new Wall(this.wallBox15);
			Wall wall16 = new Wall(this.wallBox16);
			Wall wall17 = new Wall(this.wallBox17);
			Wall wall18 = new Wall(this.wallBox18);
			Wall wall19 = new Wall(this.wallBox19);
			Wall wall20 = new Wall(this.wallBox20);
			Wall wall21 = new Wall(this.wallBox21);
			Wall wall22 = new Wall(this.wallBox22);

			controller.walls.Add(wall1);
			controller.walls.Add(wall2);
			controller.walls.Add(wall3);
			controller.walls.Add(wall4);
			controller.walls.Add(wall5);
			controller.walls.Add(wall6);
			controller.walls.Add(wall7);
			controller.walls.Add(wall8);
			controller.walls.Add(wall9);
			controller.walls.Add(wall10);
			controller.walls.Add(wall11);
			controller.walls.Add(wall12);
			controller.walls.Add(wall13);
			controller.walls.Add(wall14);
			controller.walls.Add(wall15);
			controller.walls.Add(wall16);
			controller.walls.Add(wall17);
			controller.walls.Add(wall18);
			controller.walls.Add(wall19);
			controller.walls.Add(wall20);
			controller.walls.Add(wall21);
			controller.walls.Add(wall22);
		}
	}
}
