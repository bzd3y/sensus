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

using Newtonsoft.Json;
using Sensus.UI.UiProperties;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Sensus.UI.Inputs.MindTrials
{
	public class MindTrialsInputGroup : InputGroup, INotifyPropertyChanged
	{
		public MindTrialsInputGroup()
		{
			InputGroups = new List<InputGroup>();
		}

		public string Url { get; set; }
		public List<InputGroup> InputGroups { get; set; }

		public override bool HasInputs => InputGroups.Any(x => x.HasInputs);

		public async Task DownloadAndBuildAsync(string url)
		{
			Url = url;

			await DownloadAndBuildAsync();
		}

		public async Task DownloadAndBuildAsync()
		{
			using (HttpClient client = new HttpClient())
			{
				using (HttpResponseMessage response = await client.GetAsync(Url))
				{
					await BuildAsync(await response.Content.ReadAsStringAsync());
				}
			}
		}

		public async Task BuildAsync(string json)
		{
			List<Domain> domains = JsonConvert.DeserializeObject<List<Domain>>(json, SensusServiceHelper.JSON_SERIALIZER_SETTINGS);

			InputGroups.Clear();

			InputGroup domainPage = new InputGroup();

			ButtonGridInput domainButtons = new ButtonGridInput();

			domainPage.Inputs.Add(domainButtons);

			domainButtons.Buttons = new List<string>();

			foreach (Domain domain in domains)
			{
				domainButtons.Buttons.Add(domain.Title);

				foreach (Session session in domain.Sessions)
				{
					string scoreGroup = $"{domain.Name}.{session.Name}_Score";

					foreach (Scenario scenario in session.Scenarios)
					{
						// Introduction page...
						InputGroup introduction = new InputGroup();

						introduction.Inputs.Add(new LabelOnlyInput(scenario.Title));

						if (string.IsNullOrWhiteSpace(scenario.Image) == false)
						{
							MediaInput mediaInput = new MediaInput();

							if (scenario.ImageEmbeded)
							{
								if (string.IsNullOrWhiteSpace(scenario.ImageType) == false)
								{
									mediaInput.Media = new MediaObject(scenario.Image, scenario.ImageType, true);
								}
							}
							else
							{
								string mimeType = SensusServiceHelper.Get().GetMimeType(scenario.Image);

								mediaInput.Media = await MediaObject.FromUrlAsync(scenario.Image, mimeType, true);
							}

							introduction.Inputs.Add(mediaInput);
						}

						introduction.ShowNavigationButtons = ShowNavigationOptions.Always;
						introduction.HidePreviousButton = true;

						// Puzzle page...
						InputGroup puzzle = new InputGroup();

						puzzle.Inputs.Add(new LabelOnlyInput(scenario.Title));

						puzzle.Inputs.Add(new ReadOnlyTextInput() { Text = scenario.Description });

						puzzle.Inputs.Add(new WordPuzzleInput()
						{
							Words = scenario.Words,
							ScoreGroup = scoreGroup,
							CorrectScore = 0.5f,
							CorrectDelay = 1000,
							IncorrectDelay = 5000,
							IncorrectFeedbackMessage = scenario.IncorrectFeedback,
							NavigationOnCorrect = InputGroupPage.NavigationResult.Forward
						});

						puzzle.ShowNavigationButtons = ShowNavigationOptions.Never;
						puzzle.HidePreviousButton = true;

						// Question page...
						InputGroup question = new InputGroup();

						question.Inputs.Add(new ReadOnlyTextInput { Text = scenario.Question });

						question.Inputs.Add(new ButtonGridInput
						{
							Buttons = new List<string> () { "Yes", "No" },
							ColumnCount = 2,
							ScoreGroup = scoreGroup,
							CorrectValue = scenario.Answer,
							CorrectScore = 0.5f,
							IncorrectDelay = 5000,
							IncorrectFeedbackMessage = scenario.IncorrectFeedback
						});

						question.ShowNavigationButtons = ShowNavigationOptions.WhenCorrect;
						question.HidePreviousButton = true;

						InputGroups.AddRange(new[] { introduction, puzzle, question });
					}

					InputGroup score = new InputGroup();

					score.Inputs.Add(new ScoreInput
					{
						ScoreGroup = scoreGroup
					});

					InputGroups.Add(score);
				}
			}
		}

		/// <summary>
		/// Whether or not to display the built in navigation buttons. If set to true, 
		/// the input group page is responsible for providing a mechanism to navigate
		/// to the next and/or previous page.
		/// </summary>
		/// <value><c>true</c> to hide the navigation buttons; otherwise, <c>false</c>.</value>
		[HiddenUiProperty]
		public override ShowNavigationOptions ShowNavigationButtons { get; set; }

		/// <summary>
		/// Whether or not to tag inputs in this group with the device's current GPS location.
		/// </summary>
		/// <value><c>true</c> if geotag; otherwise, <c>false</c>.</value>
		[HiddenUiProperty]
		public override bool Geotag { get; set; }

		/// <summary>
		/// Whether or not to force valid input values (e.g., all required fields completed, etc.)
		/// before allowing the user to move to the next input group.
		/// </summary>
		/// <value><c>true</c> if force valid inputs; otherwise, <c>false</c>.</value>
		[HiddenUiProperty]
		public override bool ForceValidInputs { get; set; }

		/// <summary>
		/// Whether or not to randomly shuffle the inputs in this group when displaying them to the user.
		/// </summary>
		/// <value><c>true</c> if shuffle inputs; otherwise, <c>false</c>.</value>
		[HiddenUiProperty]
		public override bool ShuffleInputs { get; set; }
	}
}
