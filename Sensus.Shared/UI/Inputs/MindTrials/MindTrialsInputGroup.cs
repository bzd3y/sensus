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
using System.Collections.ObjectModel;
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

		[OnOffUiProperty("Update Automatically:", true, 1)]
		public bool UpdateAutomatically { get; set; } = true;
		
		public string Url { get; set; }
		public string Hash { get; set; }
		public List<InputGroup> InputGroups { get; set; }

		public override ObservableCollection<Input> Inputs => new ObservableCollection<Input>(InputGroups.SelectMany(x => x.Inputs));
		public override bool HasInputs => InputGroups.Any(x => x.HasInputs);

		private async Task<string> DownloadAsync()
		{
			using (HttpClient client = new HttpClient())
			{
				client.DefaultRequestHeaders.Add("Cache-Control", "no-cache");

				using (HttpResponseMessage response = await client.GetAsync(Url))
				{
					return await response.Content.ReadAsStringAsync();
				}
			}
		}

		private async Task BuildAsync(string json)
		{
			List<Domain> model = JsonConvert.DeserializeObject<List<Domain>>(json, SensusServiceHelper.JSON_SERIALIZER_SETTINGS);

			InputGroups.Clear();

			InputGroup domains = new InputGroup
			{
				HideTitle = true,
				ShowNavigationButtons = ShowNavigationOptions.Never
			};

			ButtonGridInput domainButtons = new ButtonGridInput()
			{
				NavigationOnCorrect = InputGroupPage.NavigationResult.Forward,
				Required = false
			};

			domains.Inputs.Add(domainButtons);

			domainButtons.Buttons = new List<string>();
			domainButtons.ColumnCount = 2;

			InputGroups.Add(domains);

			foreach (Domain domain in model)
			{
				List<InputGroup> domainInputGroups = new List<InputGroup>();
				Session lastSession = domain.Sessions.LastOrDefault();

				domainButtons.Buttons.Add(domain.Title);

				foreach (Session session in domain.Sessions)
				{
					List<InputGroup> sessionInputGroups = new List<InputGroup>();

					if (string.IsNullOrWhiteSpace(session.Name))
					{
						session.Name = $"Session{session.Number}";
					}

					if (string.IsNullOrWhiteSpace(session.Title))
					{
						session.Title = $"Session {session.Number}";
					}

					string scoreGroup = $"{domain.Name}.{session.Name}_Score";

					foreach (Scenario scenario in session.Scenarios)
					{
						if (string.IsNullOrWhiteSpace(scenario.Name))
						{
							scenario.Name = $"Scenario{scenario.Number}";
						}

						if (string.IsNullOrWhiteSpace(scenario.Title))
						{
							scenario.Title = $"Scenario {scenario.Number}";
						}

						// Introduction page...
						InputGroup introduction = new InputGroup
						{
							HideTitle = true
						};

						introduction.Inputs.Add(new LabelOnlyInput(session.Title));
						introduction.Inputs.Add(new LabelOnlyInput(scenario.Title));

						if (string.IsNullOrWhiteSpace(scenario.Image) == false)
						{
							MediaInput mediaInput = new MediaInput();

							if (scenario.ImageFromUrl)
							{
								string mimeType = scenario.ImageType;

								if (string.IsNullOrWhiteSpace(mimeType))
								{
									mimeType = SensusServiceHelper.Get().GetMimeType(scenario.Image);
								}

								mediaInput.Media = await MediaObject.FromUrlAsync(scenario.Image, mimeType, scenario.ImageEmbeded);
							}
							else
							{
								if (string.IsNullOrWhiteSpace(scenario.ImageType) == false)
								{
									mediaInput.Media = new MediaObject(scenario.Image, scenario.ImageType, true);
								}
							}

							introduction.Inputs.Add(mediaInput);
						}

						introduction.NextButtonText = "Next";
						introduction.ShowNavigationButtons = ShowNavigationOptions.Always;
						introduction.HidePreviousButton = true;

						// Puzzle page...
						InputGroup puzzle = new InputGroup
						{
							HideTitle = true
						};

						puzzle.Inputs.Add(new LabelOnlyInput(session.Title));
						puzzle.Inputs.Add(new LabelOnlyInput(scenario.Title));

						puzzle.Inputs.Add(new ReadOnlyTextInput() { Text = scenario.Description });

						puzzle.Inputs.Add(new WordPuzzleInput()
						{
							Words = scenario.Words,
							ScoreGroup = scoreGroup,
							CorrectScore = 0.5f,
							CorrectDelay = 1000,
							IncorrectDelay = 5000,
							CorrectFeedbackMessage = scenario.CorrectFeedback,
							IncorrectFeedbackMessage = scenario.IncorrectFeedback,
							NavigationOnCorrect = InputGroupPage.NavigationResult.Forward
						});

						puzzle.ShowNavigationButtons = ShowNavigationOptions.Never;
						puzzle.HidePreviousButton = true;

						// Question page...
						InputGroup question = new InputGroup
						{
							HideTitle = true
						};

						question.Inputs.Add(new LabelOnlyInput(session.Title));
						question.Inputs.Add(new LabelOnlyInput(scenario.Title));

						question.Inputs.Add(new ReadOnlyTextInput { Text = scenario.Question });

						question.Inputs.Add(new ButtonGridInput
						{
							Buttons = new List<string>() { "Yes", "No" },
							ColumnCount = 2,
							ScoreGroup = scoreGroup,
							CorrectValue = scenario.Answer,
							CorrectScore = 0.5f,
							IncorrectDelay = 5000,
							CorrectFeedbackMessage = scenario.CorrectFeedback,
							IncorrectFeedbackMessage = scenario.IncorrectFeedback
						});

						question.NextButtonText = "Next";
						question.ShowNavigationButtons = ShowNavigationOptions.WhenCorrect;
						question.HidePreviousButton = true;

						sessionInputGroups.AddRange(new[] { introduction, puzzle, question });
					}

					InputGroup score = new InputGroup
					{
						HideTitle = true
					};

					score.Inputs.Add(new LabelOnlyInput(session.Title));

					score.Inputs.Add(new ScoreInput
					{
						ScoreGroup = scoreGroup
					});

					score.ShowNavigationButtons = ShowNavigationOptions.Always;
					score.HidePreviousButton = true;

					if (session == lastSession)
					{
						score.NextButtonText = "Done";
						score.SubmitButtonText = "Done";
					}
					else
					{
						score.NextButtonText = $"Start Round {session.Number + 1}";
					}

					sessionInputGroups.Add(score);

					domainInputGroups.AddRange(sessionInputGroups);

					InputGroups.AddRange(sessionInputGroups);
				}

				foreach (Input input in domainInputGroups.SelectMany(x => x.Inputs))
				{
					InputDisplayCondition condition = new InputDisplayCondition(domainButtons, InputValueCondition.Equals, domain.Name, true);

					input.Required = false;

					input.DisplayConditions.Add(condition);
				}
			}
		}

		public async Task DownloadAndBuildAsync(string url)
		{
			Url = url;

			await DownloadAndBuildAsync();
		}

		public async Task DownloadAndBuildAsync()
		{
			string json = await DownloadAsync();

			Hash = SensusServiceHelper.Get().GetHash(json);

			await BuildAsync(json);
		}

		public async Task UpdateAsync()
		{
			string json = await DownloadAsync();

			string hash = SensusServiceHelper.Get().GetHash(json);

			if (Hash != hash)
			{
				Hash = hash;

				await BuildAsync(json);
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
