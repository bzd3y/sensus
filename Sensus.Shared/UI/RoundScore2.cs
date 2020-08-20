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

namespace Sensus.UI
{
    public class RoundScore2 : BannerFrameTool
    {
        List<Entry> entries = new List<Entry>
        {
            new Entry(roundScore)
            {
                Color = SKColor.Parse("#166DA3"),
                Label ="January",
                ValueLabel = "200"
            },
            new Entry(10-roundScore)
            {
                Color = SKColors.Transparent,
                Label = "March",
                ValueLabel = "400"
            }
        };
        public RoundScore2()
        {
            Content = _contentLayout;

            Label congrats = new Label
            {
                Text = "Congratulations!",
                FontAttributes = FontAttributes.Bold,
                FontSize = 30,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                VerticalOptions = LayoutOptions.Start,
                TextColor = Color.Black
            };
            _contentStack.Children.Add(congrats);



            ChartView Chart1 = new ChartView
            {
                HeightRequest = 150

            };

            //Frame fullFrame = new Frame
            //{
            //    VerticalOptions = LayoutOptions.FillAndExpand,
            //    HorizontalOptions = LayoutOptions.FillAndExpand,
            //    Padding = 0,
            //    BackgroundColor = Color.Transparent

            //};


            //fullFrame.Content = Chart1;

            //_contentStack.Children.Add(fullFrame);

            StackLayout layout = new StackLayout()
            {
            };

            _contentStack.Children.Add(layout);

            layout.Children.Add(Chart1);

            Chart1.Chart = new DonutChart() { Entries = entries, HoleRadius = 40,  };


            Button nextRound = new Button
            {
                Text = "Start Round " + roundCounter.ToString(),
                Margin = new Thickness(10, 20, 10, 10),
                TextColor = Color.Black, // CHANGE white
                BackgroundColor = Color.FromHex("48AADF"), // CHANGE 166DA3 
                FontFamily = "Source Sans Pro",
                FontSize = 20,
                FontAttributes = FontAttributes.Bold,
                CornerRadius = 8,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.StartAndExpand,
                WidthRequest = 180

            };
            _contentStack.Children.Add(nextRound);
        }
    }
}
