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
using CsvHelper;
using Newtonsoft.Json;
using Sensus.MindTrailsBehind;
using Sensus.Notifications;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Xamarin.Forms;
using static Sensus.MindTrailsBehind.CsvFileReader;

namespace Sensus.UI
{
    public class ScenarioPage : BannerFrameTool
    {
        private Label scenarioName;
        public int scenarioCounter = 0;

        public ScenarioPage()
        { 

            Content = _contentLayout;
            // WorkingWithFiles --> Sensus
            // LibTextResource.txt --> firstSession.csv
            // resourceid = Sensus.firstSession.csv
            // trying to read from file
            //var assembly = IntrospectionExtensions.GetTypeInfo(typeof(MindTrailsBehind.ScenarioViewModel)).Assembly;
            //Stream stream = assembly.GetManifestResourceStream("Sensus.firstSession.csv");
            //string text = "";
            //using (var reader = new System.IO.StreamReader(stream))
            //{
            //    text = reader.ReadToEnd();
            //}


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

            scenarioName = new Label
            {
                //Text = "Writing a report",
                // spotting a neighbor
                //Text = MindTrailsBehind.CsvFileReader.ReadRow(1),
                VerticalTextAlignment = TextAlignment.Center,
                HorizontalTextAlignment = TextAlignment.Center,
                FontSize = 25,
                TextColor = Color.Black,
                HeightRequest = 80, // CHANGED FROM 100
            };

            //string jsonFileName = "firstSession.json";
            //SessionModel sessionsList = new SessionModel();
            //var assembly = typeof(ScenarioPage).GetTypeInfo().Assembly;
            //Stream stream = assembly.GetManifestResourceStream($"{assembly.GetName().Name}.{jsonFileName}");
            //using (var reader = new System.IO.StreamReader(stream))
            //{
            //    var jsonString = reader.ReadToEnd();

            //    //Converting JSON Array Objects into generic list    
            //    sessionsList = JsonConvert.DeserializeObject<SessionModel>(jsonString);
            //}

            //scenarioName.Text = sessionsList.title;
            var assembly = typeof(ScenarioPage).GetTypeInfo().Assembly;
            //string jsonFileName = "firstSession.json";
            //Stream stream = assembly.GetManifestResourceStream($"{assembly.GetName().Name}.{jsonFileName}");
            string fileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "firstSession.json");
            Console.WriteLine("Stream is");
            Console.WriteLine(fileName);
            // NEXT: Sensus.Android.Resources.firstSession.json
            Stream stream = assembly.GetManifestResourceStream("Sensus.Android.Resources.firstSession.json");
            //Console.WriteLine(assembly.GetName().Name.ToString()); // SensusAndroid 
            //Console.WriteLine(Directory.GetCurrentDirectory().ToString()); // "/" 

            using (var reader = new StreamReader(stream)) // System.ArgumentNullException
            {
                var json = reader.ReadToEnd();
                var data = JsonConvert.DeserializeObject<SessionModel>(json); // take out the lists
                scenarioName.Text = data.title;
            }
            // try creating a string of the json https://www.newtonsoft.com/json/help/html/DeserializeObject.htm

            //if (scenarioCounter < 5) // less than length of block
            //{
            // maybe data[0]
            //    scenarioCounter++; // add this to where the scenario is over
            //}


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
                VerticalOptions = LayoutOptions.EndAndExpand, 
                WidthRequest = 150

            };


            next.Clicked += onNextClicked; // CHANGE BACK 8/6
            //next.Clicked += onNextClicked;

            async void onNextClicked(object sender, EventArgs args)
            {
                //GetJsonData();
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


        //private void Button_Clicked(object sender, EventArgs e)
        //{

        //    var list = new List<MindTrailsBehind.session>();
        //    var assembly = Assembly.GetExecutingAssembly();
        //    var resourceName = "MindTrailsBehind.firstSession.csv"; // try just MindTrailsBehind.test.csv
        //                                                            // cannot find firstSession.csv
        //                                                            // tried Sensus.Shared.UI.firstSession.csv
        //                                                            // Sensus.Shared.firstSession.csv
        //                                                            // Sensus.Shared.MindTrailsBehind.firstSession.csv

        //    using (Stream stream = assembly.GetManifestResourceStream(resourceName))
        //    using (StreamReader reader = new StreamReader(stream)) // getting null object here
        //    {
        //        //string result = reader.ReadToEnd();

        //        if (reader != null)
        //        {
        //            using (var csv = new CsvReader(reader, CultureInfo.CurrentCulture))
        //            {
        //                while (csv.Read())
        //                {
        //                    list.Add(new MindTrailsBehind.session
        //                    {
        //                        Block = csv.GetField<int>(0),
        //                        Name = csv.GetField<string>(1),
        //                        Title = csv.GetField<string>(2),
        //                        Word1 = csv.GetField<string>(3),
        //                        Word2 = csv.GetField<string>(4),
        //                        Statement1 = csv.GetField<string>(5),
        //                        Statement2 = csv.GetField<string>(6),
        //                        Question = csv.GetField<string>(7),
        //                        Positive = csv.GetField<string>(8),
        //                        Negative = csv.GetField<string>(9),
        //                        Answer = csv.GetField<string>(10),
        //                        Type = csv.GetField<string>(11),

        //                    });
        //                }
        //            }
        //        }

        //    }
        //    scenarioName.Text = (list.ToArray())[1].Title;  //used to get the value of ID column,6th row(the row include the column[ID, Name, Age] row.).        

        //}
    }
}
