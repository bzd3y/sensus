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
using Microcharts;
using SkiaSharp;
using Microcharts.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Entry = Microcharts.ChartEntry; // changed to ChartEntry -- worked!

namespace Sensus.UI.MindTrails
{
    public class RoundScore2 : BannerFrameTool
    {
        public double roundScore;

        public RoundScore2()
        {
            Content = _contentLayout;

            _contentLayout.BackgroundColor = Color.White;

            if(roundCounter == 1)
            {
                roundScore = roundScore1;
            }
            else if (roundCounter == 2)
            {
                roundScore = roundScore2;
            }
            else if (roundCounter == 3)
            {
                roundScore = roundScore3;
            }
            else if (roundCounter == 4)
            {
                roundScore = roundScore4;
            }
            List<Entry> entries = new List<Entry>
            {
                new Entry((float)roundScore)
                {
                    Color = SKColor.Parse("#166DA3")
                },
                new Entry(10-(float)roundScore)
                {
                    Color = SKColors.Transparent
                }
            };
            //_contentStack.HorizontalOptions = LayoutOptions.FillAndExpand;
            //_contentStack.VerticalOptions = LayoutOptions.FillAndExpand;
            _contentStack.BackgroundColor = Color.Transparent;

            Label congrats = new Label
            {
                Text = "Congratulations!",
                FontAttributes = FontAttributes.Bold,
                FontSize = 30,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                VerticalOptions = LayoutOptions.Start,
                TextColor = Color.Black,
                Margin = new Thickness(0,0,0,25)
            };
            _contentStack.Children.Add(congrats);

            Grid grid = new Grid
            {

            };

            ChartView Chart1 = new ChartView
            {
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.Fill, // from CenterAndExpand
                HeightRequest = 270, // back to 200
                BackgroundColor = Color.Transparent,
            };

            Label score = new Label
            {
                FontSize = 20,
                TextColor = Color.Black,
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.Center,
                HorizontalTextAlignment = TextAlignment.Center
            };
            var scoreString = new FormattedString();

            scoreString.Spans.Add(new Span { Text = roundScore.ToString(), FontSize = 50, FontAttributes = FontAttributes.Bold });
            scoreString.Spans.Add(new Span { Text = "\n/10", FontSize = 25 });
            score.FormattedText = scoreString;

            grid.Children.Add(Chart1);
            grid.Children.Add(score);
            _contentStack.Children.Add(grid);
            // .5f
            Chart1.Chart = new DonutChart() { Entries = entries, HoleRadius = .6f, BackgroundColor = SKColor.Empty  }; 

            Button nextRound = new Button
            {
                Text = "Start Round " + (roundCounter + 1).ToString(),
                Margin = new Thickness(10, 25, 10, 10),
                TextColor = Color.Black, // CHANGE white
                BackgroundColor = Color.FromHex("48AADF"), // CHANGE 166DA3 
                FontFamily = "Source Sans Pro",
                FontSize = 20,
                FontAttributes = FontAttributes.Bold,
                CornerRadius = 8,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.End, // Start and Expand 
                WidthRequest = 180

            };
            _contentStack.Children.Add(nextRound);

            nextRound.Clicked += nextClicked;
            async void nextClicked(object sender, EventArgs args)
            {
                roundCounter++;
                if (scenarioCounter == 39)
                {
                    await Navigation.PushModalAsync(new NavigationPage(new SessionScore()));

                }
                else
                {
                    await Navigation.PushModalAsync(new NavigationPage(new ScenarioPage()));

                }
            };

        }


    }
}
