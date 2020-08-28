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
using System;
using System.Collections.Generic;
using Microcharts;
using Microcharts.Forms;
using SkiaSharp;
using Xamarin.Forms;
using Entry = Microcharts.ChartEntry; 


namespace Sensus.UI.MindTrails
{
    public class SessionScore : BannerFrameTool
    {
        public int roundScore;

        public SessionScore()
        {
            Content = _contentLayout;

            _contentLayout.BackgroundColor = Color.White;

            List<Entry> entries = new List<Entry>
            {
                new Entry((float)roundScore1)
                {
                    Color = SKColor.Parse("#166DA3"),
                },
                new Entry((float)roundScore2)
                {
                    Color = SKColor.Parse("#48AADF"),

                },
                new Entry((float)roundScore3)
                {
                    Color = SKColor.Parse("#B5E7FA"),
                },
                new Entry((float)roundScore4)
                {
                    Color = SKColor.Parse("#BED3DF"),

                },
                new Entry(10-((float)roundScore1 + (float)roundScore2 +
                (float)roundScore3 + (float)roundScore4))
                {
                    Color = SKColors.Transparent
                }
            };

            _contentStack.BackgroundColor = Color.Transparent;

            Label congrats = new Label
            {
                Text = "Training Session " + sessionNumber.ToString() + "\nComplete!",
                FontAttributes = FontAttributes.Bold,
                FontSize = 30,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                VerticalOptions = LayoutOptions.Start,
                TextColor = Color.Black,
                HorizontalTextAlignment = TextAlignment.Center,
                Margin = new Thickness(-20, 0, 0, 10)
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

            scoreString.Spans.Add(new Span { Text = (roundScore1+roundScore2+roundScore3+roundScore4).ToString(), FontSize = 50, FontAttributes = FontAttributes.Bold });
            scoreString.Spans.Add(new Span { Text = "\n/40", FontSize = 25 });
            score.FormattedText = scoreString;

            grid.Children.Add(Chart1);
            grid.Children.Add(score);
            _contentStack.Children.Add(grid);
            Chart1.Chart = new DonutChart() { Entries = entries, HoleRadius = .6f, BackgroundColor = SKColor.Empty};

            Button finish = new Button
            {
                Text = "Finish!",
                Margin = new Thickness(10, 15, 10, 10),
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
            _contentStack.Children.Add(finish);
            finish.Clicked += Finish_Clicked;

            async void Finish_Clicked(object sender, EventArgs args)
            {
                await Navigation.PushModalAsync(new NavigationPage(new HomePage()));
                sessionNumber++;
            };

        }
    }
}
