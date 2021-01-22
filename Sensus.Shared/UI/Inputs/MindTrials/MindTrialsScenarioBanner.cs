using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Sensus.UI.Inputs.MindTrials
{
	public class MindTrialsScenarioBanner : Input
	{
		public override object Value => null;

		public override bool Enabled { get; set; } = true;

		public override string DefaultName => "Scenario Banner";

		public string ScenarioTitle { get; set; }

		public override View GetView(int index)
		{
			if (base.GetView(index) == null)
			{
				Image image = new Image
				{
					Source = "pencil.png"
				};

				Label sessionLabel = new Label
				{
					Text = ScenarioTitle,
					FontSize = 20,
					Margin = new Thickness(15, 0, 0, 0)
				};

				StackLayout layout = new StackLayout()
				{
					Orientation = StackOrientation.Horizontal,
					Children = { image, sessionLabel },
					Padding = new Thickness(15)
				};

				base.SetView(layout);
			}

			return base.GetView(index);
		}
	}
}
