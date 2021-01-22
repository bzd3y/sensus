using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Sensus.UI.Inputs.MindTrials
{
	public class MindTrialsBanner : Input
	{
		public override object Value => null;

		public override bool Enabled { get; set; } = true;

		public override string DefaultName => "MTBanner";

		public string SessionTitle { get; set; }
		public string ScenarioTitle { get; set; }

		public override View GetView(int index)
		{
			if (base.GetView(index) == null)
			{
				Image logoImage = new Image
				{
					Source = "logo.png",
					WidthRequest = 20
				};

				Label sessionLabel = new Label
				{
					Text = SessionTitle,
					TextColor = (Color)Application.Current.Resources["NavigationBarTextColor"],
					FontSize = 30,
					Margin = new Thickness(20, 0, 0, 0)
				};

				StackLayout sessionLayout = new StackLayout()
				{
					Orientation = StackOrientation.Horizontal,
					HorizontalOptions = LayoutOptions.FillAndExpand,
					BackgroundColor = (Color)Application.Current.Resources["NavigationBarColor"],
					Children = { logoImage, sessionLabel },
					Padding = new Thickness(20)
				};

				Image pencilImage = new Image
				{
					Source = "pencil.png",
					WidthRequest = 15
				};

				Label scenarioLabel = new Label
				{
					Text = ScenarioTitle,
					FontSize = 20,
					Margin = new Thickness(15, 0, 0, 0)
				};

				StackLayout scenarioLayout = new StackLayout()
				{
					Orientation = StackOrientation.Horizontal,
					HorizontalOptions = LayoutOptions.CenterAndExpand,
					Children = { pencilImage, scenarioLabel },
					Padding = new Thickness(15)
				};

				StackLayout layout = new StackLayout()
				{
					Children = { sessionLayout, scenarioLayout },
					Padding = new Thickness(-10)
				};

				RelativeLayout bannerLayout = new RelativeLayout();

				bannerLayout.Children.Add(layout, Constraint.Constant(-10));

				Frame = false;

				base.SetView(bannerLayout);
			}

			return base.GetView(index);
		}
	}
}
