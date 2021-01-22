
using Xamarin.Forms;

namespace Sensus.UI.Inputs.MindTrials
{
	public class MindTrialsSessionBanner : Input
	{
		public override object Value => null;

		public override bool Enabled { get; set; } = true;

		public override string DefaultName => "Scenario Banner";

		public string SessionTitle { get; set; }

		public override View GetView(int index)
		{
			if (base.GetView(index) == null)
			{
				Image image = new Image
				{
					Source = "logo.png"
				};

				Label sessionLabel = new Label
				{
					Text = SessionTitle,
					TextColor = (Color)Application.Current.Resources["NavigationBarTextColor"],
					FontSize = 30,
					Margin = new Thickness(20, 0, 0, 0)
				};

				StackLayout layout = new StackLayout()
				{
					Orientation = StackOrientation.Horizontal,
					BackgroundColor = (Color)Application.Current.Resources["NavigationBarColor"],
					Children = { image, sessionLabel },
					Padding = new Thickness(20)
				};

				base.SetView(layout);
			}

			return base.GetView(index);
		}
	}
}
