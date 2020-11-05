﻿using Newtonsoft.Json;
using Sensus.UI.UiProperties;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Xamarin.Forms;

namespace Sensus.UI.Inputs
{
	public class ScoreInput : Input
	{
		private IEnumerable<Input> _inputs = new List<Input>();
		private float _maxScore;
		private Label _scoreLabel;

		public override object Value
		{
			get
			{
				return _score;
			}
		}

		public override bool Enabled { get; set; }

		public override string DefaultName => "Score";

		[EntryFloatUiProperty("Score Value:", false, 3, false)]
		public override float ScoreValue => 0;

		[ListUiProperty("Score Method:", true, 4, new object[] { ScoreMethods.Total, ScoreMethods.Average }, false)]
		public override ScoreMethods ScoreMethod { get; set; } = ScoreMethods.Total;

		[EntryIntegerUiProperty("Allowed Retries:", false, 5, false)]
		public override int? Retries => 0;

		[JsonIgnore] // TODO: determine if this needs to be serialized or not
		public IEnumerable<Input> Inputs
		{
			get
			{
				return _inputs;
			}
			set
			{
				// remove the ScoreChanged event from each of the original inputs.
				foreach (Input input in _inputs)
				{
					input.PropertyChanged -= ScoreChanged;
				}

				if (string.IsNullOrWhiteSpace(ScoreGroup))
				{
					_inputs = value.OfType<ScoreInput>().Where(x => string.IsNullOrWhiteSpace(x.ScoreGroup) == false);
				}
				else
				{
					_inputs = value.Where(x => x is ScoreInput == false);
				}

				foreach (Input input in _inputs)
				{
					input.PropertyChanged += ScoreChanged;
				}

				SetScore();
			}
		}

		public void SetScore()
		{
			_maxScore = 0;

			if (ScoreMethod == ScoreMethods.Total)
			{
				Score = _inputs.Sum(x => x.ScoreValue);
				_maxScore = _inputs.Sum(x => x.ScoreValue);
			}
			else if (ScoreMethod == ScoreMethods.Average)
			{
				Score = _inputs.Sum(x => x.ScoreValue) / _inputs.Count();
			}

			// if the label has been created, update its text
			UpdateScoreText();
		}

		private void ScoreChanged(object sender, PropertyChangedEventArgs e)
		{
			if (e.PropertyName == nameof(Score))
			{
				SetScore();
			}
		}

		private void UpdateScoreText()
		{
			if (_scoreLabel != null)
			{			
				_scoreLabel.Text = $"{_score}/{_maxScore}";
			}
		}

		public override View GetView(int index)
		{
			if (base.GetView(index) == null)
			{
				_scoreLabel = CreateLabel(-1);

				UpdateScoreText();

				base.SetView(_scoreLabel);
			}

			return base.GetView(index);
		}
	}
}
