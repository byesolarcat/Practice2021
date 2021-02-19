using System;
using System.ComponentModel;
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
			foreach (var entity in controller.DrawableEntities)
			{
				if (entity is Tank || entity is Apple)
					entities.Add((EntityModel)entity);
			}
			dataGridView1.DataSource = entities;
			this.Focus();
		}

		private void propertiesUpdateTimer_Tick(object sender, EventArgs e)
		{
			entities.Clear();
			entities.Add(controller.Kolobok);
			foreach (var entity in controller.DrawableEntities)
			{
				if (entity is Tank || entity is Apple)
					entities.Add((EntityModel)entity);
			}
			dataGridView1.DataSource = entities;
		}

		private void EntitiesInfo_KeyDown(object sender, KeyEventArgs e)
		{
			controller.KeyIsDown_Handler(sender, e);

		}
	}
}
