using System.Collections.Generic;

namespace Sensus.UI.Inputs.MindTrails
{
	public class Domain
	{
		public string Name { get; set; }
		public string Title { get; set; }

		public List<Session> Sessions { get; set; }
	}

	public class Session
	{
		public int Number { get; set; }
		public string Name { get; set; }
		public string Title { get; set; }

		public List<Scenario> Scenarios { get; set; }
	}

	public class Scenario
	{
		public int Number { get; set; }
		public string Name { get; set; }
		public string Title { get; set; }
		public string Caption { get; set; }
		public string Image { get; set; }
		public string ImageType { get; set; }
		public bool ImageEmbeded { get; set; }
		public bool ImageFromUrl { get; set; }
		public string Description { get; set; }
		public string CorrectFeedback { get; set; }
		public string IncorrectFeedback { get; set; }
		public List<string> Words { get; set; }
		public string Question { get; set; }
		public string Answer { get; set; }
	}
}
