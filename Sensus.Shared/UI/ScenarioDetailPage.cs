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
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Sensus.MindTrailsBehind;
using Xamarin.Forms;

namespace Sensus.UI
{
    public class ScenarioDetailPage : BannerFrameTool
    {
        protected Label scenarioDescription;

        public ScenarioDetailPage()
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
                Margin = new Thickness(15,0,15,0),
                VerticalOptions = LayoutOptions.CenterAndExpand,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                CornerRadius = 10
            };

            _whiteframeLayout.Children.Add(grayFrame);

            scenarioDescription = new Label
            {
                //Text = "Your boss asks you to write a report.\n\nThe finished document is quite brief but took a lot of time and effort." +
                //"\n\nBased on your writing, you expect your boss' opinion of you will be...",
                TextColor = Color.Black,
                FontFamily = "Source Sans Pro",
                Margin = 20,
                FontSize = 20
            };

            grayFrame.Content = scenarioDescription;

            Grid wordGrid = new Grid
            {
                ColumnDefinitions = {
                    new ColumnDefinition {Width = new GridLength(1, GridUnitType.Star)},
                    new ColumnDefinition {Width = new GridLength(1, GridUnitType.Star)},
                    new ColumnDefinition {Width = new GridLength(1, GridUnitType.Star)},
                    new ColumnDefinition {Width = new GridLength(1, GridUnitType.Star)},
                    new ColumnDefinition {Width = new GridLength(1, GridUnitType.Star)},
                    new ColumnDefinition {Width = new GridLength(1, GridUnitType.Star)},
                    new ColumnDefinition {Width = new GridLength(1, GridUnitType.Star)},
                    new ColumnDefinition {Width = new GridLength(1, GridUnitType.Star)}

                },
                Margin = new Thickness(10,15,10,10),
                HorizontalOptions = LayoutOptions.CenterAndExpand
            };

            _whiteframeLayout.Children.Add(wordGrid);

            Button letter1 = new Button
            {
                Text = "P",
                FontFamily = "Source Sans Pro",
                FontAttributes = FontAttributes.Bold,
                BackgroundColor = Color.FromHex("B5E7FA"),
                TextColor = Color.Black,
                HeightRequest = 40,
                FontSize = 15,
                CornerRadius = 6
            };
            Button letter2 = new Button
            {
                Text = "O",
                FontFamily = "Source Sans Pro",
                FontAttributes = FontAttributes.Bold,
                BackgroundColor = Color.FromHex("B5E7FA"),
                TextColor = Color.Black,
                HeightRequest = 40,
                FontSize = 15,
                CornerRadius = 6
            };
            Button letter3 = new Button
            {
                Text = "S",
                FontFamily = "Source Sans Pro",
                FontAttributes = FontAttributes.Bold,
                BackgroundColor = Color.FromHex("B5E7FA"),
                TextColor = Color.Black,
                HeightRequest = 40,
                FontSize = 15,
                CornerRadius = 6
            };

            Button letter4 = new Button
            {
                Text = "I",
                FontFamily = "Source Sans Pro",
                FontAttributes = FontAttributes.Bold,
                BackgroundColor = Color.FromHex("B5E7FA"),
                TextColor = Color.Black,
                HeightRequest = 40,
                FontSize = 15,
                CornerRadius = 6
            };

            Button letter5 = new Button
            {
                BackgroundColor = Color.FromHex("DADADA"),
                HeightRequest = 40,
                FontSize = 15,
                CornerRadius = 6
            };
            Button correctLetterGreen = new Button
            {
                Text = "T",
                FontFamily = "Source Sans Pro",
                FontAttributes = FontAttributes.Bold,
                TextColor = Color.Black,
                BackgroundColor = Color.FromHex("6662A74C"),
                HeightRequest = 40,
                FontSize = 15,
                CornerRadius = 6
            };

            Button letter6 = new Button
            {
                Text = "I",
                FontFamily = "Source Sans Pro",
                FontAttributes = FontAttributes.Bold,
                BackgroundColor = Color.FromHex("B5E7FA"),
                TextColor = Color.Black,
                HeightRequest = 40,
                FontSize = 15,
                CornerRadius = 6
            };
            Button letter7 = new Button
            {
                Text = "V",
                FontFamily = "Source Sans Pro",
                FontAttributes = FontAttributes.Bold,
                BackgroundColor = Color.FromHex("B5E7FA"),
                TextColor = Color.Black,
                HeightRequest = 40,
                FontSize = 15,
                CornerRadius = 6
            };
            Button letter8 = new Button
            {
                Text = "E",
                FontFamily = "Source Sans Pro",
                FontAttributes = FontAttributes.Bold,
                BackgroundColor = Color.FromHex("B5E7FA"),
                TextColor = Color.Black,
                HeightRequest = 40,
                FontSize = 15,
                CornerRadius = 6
            };

            wordGrid.Children.Add(letter1, 0, 0);
            wordGrid.Children.Add(letter2, 1, 0);
            wordGrid.Children.Add(letter3, 2, 0);
            wordGrid.Children.Add(letter4, 3, 0);
            wordGrid.Children.Add(letter5, 4, 0);
            wordGrid.Children.Add(letter6, 5, 0);
            wordGrid.Children.Add(letter7, 6, 0);
            wordGrid.Children.Add(letter8, 7, 0);

            Label selectTile = new Label
            {
                Text = "SELECT A TILE:",
                FontSize = 14,
                TextColor = Color.Black,
                HorizontalOptions = LayoutOptions.CenterAndExpand

            };

            _whiteframeLayout.Children.Add(selectTile);

            Grid lettersGrid = new Grid
            {
                ColumnDefinitions = {
                    new ColumnDefinition {Width = new GridLength(1, GridUnitType.Star)},
                    new ColumnDefinition {Width = new GridLength(1, GridUnitType.Star)},
                    new ColumnDefinition {Width = new GridLength(1, GridUnitType.Star)},
                    new ColumnDefinition {Width = new GridLength(1, GridUnitType.Star)},
                    new ColumnDefinition {Width = new GridLength(1, GridUnitType.Star)},
                    new ColumnDefinition {Width = new GridLength(1, GridUnitType.Star)},

                },
                Padding = new Thickness(30,0,30,5),
                HorizontalOptions = LayoutOptions.CenterAndExpand

            };

            _whiteframeLayout.Children.Add(lettersGrid);

            Button letterOption1 = new Button
            {
                Text = "Y",
                FontFamily = "Source Sans Pro",
                FontAttributes = FontAttributes.Bold,
                BackgroundColor = Color.FromHex("B5E7FA"),
                TextColor = Color.Black,
                HeightRequest = 40,
                FontSize = 15,
                CornerRadius = 6
                // center text 
            };

            Button correctLetter = new Button
            {
                Text = "T",
                FontFamily = "Source Sans Pro",
                FontAttributes = FontAttributes.Bold,
                BackgroundColor = Color.FromHex("B5E7FA"),
                TextColor = Color.Black,
                HeightRequest = 40,
                FontSize = 15,
                CornerRadius = 6
            };
            Button whiteButton = new Button
            {
                BackgroundColor = Color.White,
                HeightRequest = 40,
                CornerRadius = 6
            };

            Image correctIcon = new Image
            {
                Source = "CheckMark.png",
                HeightRequest = 40,
                Margin = 0
            };

            correctLetter.Clicked += onCorrectLetter;

            async void onCorrectLetter(object sender, EventArgs args) // removed async 
            {
                wordGrid.Children.Add(correctLetterGreen, 4, 0);
                lettersGrid.Children.Add(correctIcon, 5, 0);
                lettersGrid.Children.Add(whiteButton, 2, 0);
                //// timer to next page
                await Task.Delay(500); // Task.Delay(500).Wait()
                //await Navigation.PushAsync(new ScenarioTestPage());
                await Navigation.PushAsync(new ReflectionsPage());
            };

            Button letterOption3 = new Button
            {
                Text = "O",
                FontFamily = "Source Sans Pro",
                FontAttributes = FontAttributes.Bold,
                BackgroundColor = Color.FromHex("B5E7FA"),
                TextColor = Color.Black,
                HeightRequest = 40,
                FontSize = 15,
                CornerRadius = 6
            };
            Button letterOption4 = new Button
            {
                Text = "S",
                FontFamily = "Source Sans Pro",
                FontAttributes = FontAttributes.Bold,
                BackgroundColor = Color.FromHex("B5E7FA"),
                TextColor = Color.Black,
                HeightRequest = 40,
                FontSize = 15,
                CornerRadius = 6
            };

            lettersGrid.Children.Add(letterOption1, 1, 0);
            lettersGrid.Children.Add(correctLetter, 2, 0 );
            lettersGrid.Children.Add(letterOption3, 3, 0);
            lettersGrid.Children.Add(letterOption4, 4, 0 );

            var assembly = typeof(ScenarioDetailPage).GetTypeInfo().Assembly;
            string fileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "firstSession.json");
            Stream stream = assembly.GetManifestResourceStream("Sensus.Android.Resources.firstSession.json");


            using (var reader = new StreamReader(stream))
            {
                var json = reader.ReadToEnd();
                var data = JsonConvert.DeserializeObject<Root>(json);

                if (scenarioCounter < 39) // length of json 
                {
                    string input = data.firstSession[scenarioCounter].statement1;
                    string[] sentences = Regex.Split(input, @"(?<=[\.!\?])\s+");
                    string description = "";
                    foreach(string sentence in sentences)
                    {
                        description += sentence + "\n\n";
                    }
                    scenarioDescription.Text = description.Substring(0, description.Length-2);
                    // delete last two \n\n

                }

            }
        }
    }
}
