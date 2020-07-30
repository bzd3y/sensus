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
using Sensus.Notifications;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Sensus.UI
{
    public class ScenarioPage : BannerFrameTool
    {

        public ScenarioPage()
        {

            Content = _contentLayout;

            Grid headerGrid = new Grid
            {
                ColumnSpacing = 0,
                RowSpacing = 0,
                Padding = 0,
                Margin = new Thickness(10,20),
                ColumnDefinitions = {
                    new ColumnDefinition {
                        Width = new GridLength(1, GridUnitType.Star)
                    },
                    new ColumnDefinition
                    {
                        Width = new GridLength(3, GridUnitType.Star)
                    }
                },

            };
            Label scenarioNum = new Label
            {
                Text = "Scenario 1",
                //Margin = new Thickness(10), CHANGED 
                TextColor = Color.FromHex("166DA3"),
                FontSize = 22,
                FontFamily = "Source Sans Pro",
                HorizontalTextAlignment = TextAlignment.Start
                // WidthRequest =1000

            };
            Image scenarioIcon = new Image { Source = "pencil.png", HeightRequest = 10 };

            headerGrid.Children.Add(scenarioIcon, 0, 0);

            headerGrid.Children.Add(scenarioNum, 1, 0); // column, row 

            _whiteframeLayout.Children.Add(headerGrid);

            Frame grayFrame = new Frame
            {
                BackgroundColor = Color.FromHex("F0ECEC"),
                HasShadow = false,
                Padding = 0,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                HorizontalOptions = LayoutOptions.Center,
                WidthRequest = 250,
                CornerRadius = 10
            };

            Label scenarioName = new Label
            {
                Text = "Writing a report",
                VerticalTextAlignment = TextAlignment.Center,
                HorizontalTextAlignment = TextAlignment.Center,
                FontSize = 25,
                TextColor = Color.Black,
                HeightRequest = 80 // CHANGED FROM 100 
            };
            Image scenarioImage = new Image
            {
                Source = "Report.png",
                HeightRequest = 200,
                Margin = new Thickness(0, 0, 0, 0)
            }; // CHANGED from 0, 20, 0, 0 

            grayFrame.Content = scenarioName;
            _whiteframeLayout.Children.Add(grayFrame);
            _whiteframeLayout.Children.Add(scenarioImage);

            Button next = new Button
            {
                Text = "Next",
                Margin = new Thickness(10, 20, 10, 10),
                TextColor = Color.Black, // CHANGE white
                BackgroundColor = Color.FromHex("48AADF"), // CHANGE 166DA3 
                FontFamily = "Source Sans Pro",
                FontSize = 20,
                FontAttributes = FontAttributes.Bold,
                CornerRadius = 8,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.EndAndExpand, // CHANGE
                WidthRequest = 150

            };

            

            next.Clicked += onNextClicked;

            async void onNextClicked(object sender, EventArgs args)
            {
                await Navigation.PushAsync(new ScenarioDetailPage());
            };
            _whiteframeLayout.Children.Add(next);

            ProgressBar progress = new ProgressBar
            {
                ProgressColor = Color.FromHex("166DA3"),
                Progress = .2,
                Margin = new Thickness(20, 0, 20, 10),
                VerticalOptions = LayoutOptions.EndAndExpand
            };

            _whiteframeLayout.Children.Add(progress);
        }
    }
}
