using Xamarin.Forms;

namespace Sensus.UI.Inputs.MindTrails
{
	public class MindTrailsHeader : Input
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
				StackLayout layout = new StackLayout()
				{
					HorizontalOptions = LayoutOptions.FillAndExpand,
					Orientation = StackOrientation.Vertical
				};

				if (string.IsNullOrWhiteSpace(SessionTitle) == false)
				{
					Image logoImage = new Image
					{
						Source = "logo.png",
						WidthRequest = 48
					};

					Label sessionLabel = new Label
					{
						Text = SessionTitle,
						TextColor = (Color)Application.Current.Resources["NavigationBarTextColor"],
						FontSize = 30,
						Margin = new Thickness(8, 0, 0, 0)
					};

					StackLayout sessionLayout = new StackLayout()
					{
						Orientation = StackOrientation.Horizontal,
						HorizontalOptions = LayoutOptions.FillAndExpand,
						BackgroundColor = (Color)Application.Current.Resources["NavigationBarColor"],
						Children = { logoImage, sessionLabel },
						Padding = new Thickness(12)
					};

					layout.Children.Add(sessionLayout);
				}

				if (string.IsNullOrWhiteSpace(ScenarioTitle) == false)
				{
					Image scenarioImage = new Image
					{
						Source = "pencil.png",
						HeightRequest = 24
					};

					Label scenarioLabel = new Label
					{
						Text = ScenarioTitle,
						FontSize = 20,
						Margin = new Thickness(6, 0, 0, 0)
					};

					StackLayout scenarioLayout = new StackLayout()
					{
						Orientation = StackOrientation.Horizontal,
						HorizontalOptions = LayoutOptions.FillAndExpand,
						Children = { scenarioImage, scenarioLabel },
						Padding = new Thickness(14, 6, 14, 6)
					};

					layout.Children.Add(scenarioLayout);
				}

				Frame = false;

				base.SetView(layout);
			}

			return base.GetView(index);
		}
	}
}
