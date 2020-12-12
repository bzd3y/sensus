// Copyright 2014 The Rector & Visitors of the University of Virginia
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using System.Collections.Generic;

namespace Sensus.UI.Inputs.MindTrials
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
