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
	public partial class EntitiesInfo : Form
	{
		private BindingList<EntityModel> entities = new BindingList<EntityModel>();
		PacmanController controller;
		public EntitiesInfo(PacmanController controller)
		{
			InitializeComponent();
			this.controller = controller;
			entities.Add(controller.Kolobok);
			foreach (var tank in controller.Tanks)
			{
				entities.Add(tank);
			}
			dataGridView1.DataSource = entities;
			this.Focus();
		}

		private void propertiesUpdateTimer_Tick(object sender, EventArgs e)
		{
			entities.Clear();
			entities.Add(controller.Kolobok);
			foreach (var tank in controller.Tanks)
			{
				entities.Add(tank);
			}
			dataGridView1.DataSource = entities;
		}

		private void EntitiesInfo_KeyDown(object sender, KeyEventArgs e)
		{
			controller.KeyIsDown_Handler(sender, e);
			
		}
	}
}
