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
using Xamarin.Forms;

namespace Sensus.UI
{
    public class ScenarioTestPage : BannerFrameTool
    {
        public ScenarioTestPage()
        {
            Content = _contentLayout;

            Grid headerGrid = new Grid
            {
                ColumnSpacing = 0,
                RowSpacing = 0,
                Padding = 0,
                Margin = new Thickness(10, 20),
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
                TextColor = Color.FromHex("166DA3"),
                FontSize = 22,
                FontFamily = "Source Sans Pro",
                HorizontalTextAlignment = TextAlignment.Start

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
                Margin = new Thickness(20, 30, 20, 0),
                VerticalOptions = LayoutOptions.CenterAndExpand,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                CornerRadius = 10
            };

            _whiteframeLayout.Children.Add(grayFrame);

            Label question = new Label
            {
                Text = "Do you expect your boss to have a positive opinion of you after reading your report?",
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                FontFamily = "Source Sans Pro",
                FontSize = 20,
                Margin = 20,
                TextColor = Color.Black
            };

            grayFrame.Content = question;

            Grid yesNo = new Grid
            {
                ColumnDefinitions =
                {
                    new ColumnDefinition {Width = new GridLength(.5, GridUnitType.Star)},
                    new ColumnDefinition {Width = new GridLength(1, GridUnitType.Star)},
                    new ColumnDefinition {Width = new GridLength(1, GridUnitType.Star)},
                    new ColumnDefinition {Width = new GridLength(.5, GridUnitType.Star)},

                },
                RowDefinitions =
                {
                    new RowDefinition {Height = new GridLength(1, GridUnitType.Star)},
                    new RowDefinition {Height = new GridLength(1, GridUnitType.Star)},
                    new RowDefinition {Height = new GridLength(1, GridUnitType.Star)},
                    new RowDefinition {Height = new GridLength(1, GridUnitType.Star)},

                },
                Margin = new Thickness(10,40,10,20)
            };

            _whiteframeLayout.Children.Add(yesNo);

            Button yes = new Button
            {
                Text = "Yes",
                FontFamily = "Source Sans Pro",
                BackgroundColor = Color.FromHex("B5E7FA"),
                TextColor = Color.Black,
                HeightRequest = 40,
                FontSize = 15,
                CornerRadius = 6,
            };

            Button yesGreen = new Button
            {
                Text = "Yes",
                FontFamily = "Source Sans Pro",
                BackgroundColor = Color.FromHex("6662A74C"),
                TextColor = Color.Black,
                HeightRequest = 40,
                FontSize = 15,
                CornerRadius = 6
            };

            Button yesRed = new Button
            {
                Text = "Yes",
                FontFamily = "Source Sans Pro",
                BackgroundColor = Color.FromHex("FAB9B5"),
                TextColor = Color.Black,
                HeightRequest = 40,
                FontSize = 15,
                CornerRadius = 6
            };

            Button no = new Button
            {
                Text = "No",
                FontFamily = "Source Sans Pro",
                BackgroundColor = Color.FromHex("B5E7FA"),
                TextColor = Color.Black,
                HeightRequest = 40,
                FontSize = 15,
                CornerRadius = 6,
            };

            Button noGreen = new Button
            {
                Text = "No",
                FontFamily = "Source Sans Pro",
                BackgroundColor = Color.FromHex("6662A74C"),
                TextColor = Color.Black,
                HeightRequest = 40,
                FontSize = 15,
                CornerRadius = 6
            };

            Button noRed = new Button
            {
                Text = "No",
                FontFamily = "Source Sans Pro",
                BackgroundColor = Color.FromHex("FAB9B5"),
                TextColor = Color.Black,
                HeightRequest = 40,
                FontSize = 15,
                CornerRadius = 6
            };
            Image correctIcon = new Image
            {
                Source = "CheckMark.png",
                HeightRequest = 40,
                Margin = 0
            };
            Image incorrectIcon = new Image
            {
                Source = "XMark.png",
                HeightRequest = 40,
                Margin = 0
            };
            Label blankLabel = new Label
            {
                Text = " ",
                BackgroundColor = Color.White,
                HorizontalOptions = LayoutOptions.End // andexpand
            };
            ProgressBar progressBarWhite = new ProgressBar
            {
                ProgressColor = Color.White
            };

            //Grid.SetColumnSpan(progressBarWhite, 3);
            yesNo.Children.Add(progressBarWhite, 1, 1);
            yesNo.Children.Add(progressBarWhite, 1, 2);
            yesNo.Children.Add(progressBarWhite, 1, 3);

            ProgressBar progressBar = new ProgressBar
            {
                ProgressColor = Color.FromHex("233367")
            };

            Label whoops = new Label
            {
                TextColor = Color.Black,
                Text = "Whoops! That doesn’t look right. Please wait a moment and try again.",
            };

            yesNo.Children.Add(yes, 1, 0);
            yesNo.Children.Add(no, 2, 0);

            yes.Clicked += onYes;

            void onYes(object sender, EventArgs args)
            {
                yesNo.Children.Add(yesGreen, 1, 0);
                yesNo.Children.Add(correctIcon, 3, 0);
                no.IsEnabled = false;
                // make it so you can't click no 
            };

            no.Clicked += onNo;

            void onNo(object sender, EventArgs args)
            {
                yesNo.Children.Add(noRed, 2, 0);
                yesNo.Children.Add(incorrectIcon, 3, 0);
                yes.IsEnabled = false;
                // span two columns
                // FIX the column spans! 
                //Grid.SetColumnSpan(whoops, 3);
                //Grid.SetColumnSpan(progressBar, 3);
                yesNo.Children.Add(whoops, 1, 2);
                yesNo.Children.Add(progressBar, 1, 1);

                var updateRate = 10000 / 1f; // 30Hz added a 0 
                double step = updateRate / (2 * 10 * 1000f); // from 30 * 1000f
                Device.StartTimer(TimeSpan.FromMilliseconds(updateRate), () =>
                {
                    if (progressBar.Progress < 100)
                    {
                        Device.BeginInvokeOnMainThread(() => progressBar.Progress += step);
                        return true;
                    }
                    return false;
                });
                yes.IsEnabled = true;
                yesNo.Children.Add(no, 2, 0);
                yesNo.Children.Add(blankLabel, 3, 0);

            };



            _whiteframeLayout.Children.Add(blankLabel);

            // timer bar --> show "Whoops! That doesn’t look right. Please wait a moment and try again."
        }
    }
}
