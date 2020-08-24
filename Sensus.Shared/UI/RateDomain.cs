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
    public class RateDomain : BannerFrameTool
    {
        public RateDomain(string domain)
        {
            Content = _contentLayout;

            Label youChose = new Label
            {
                Text = "You chose:",
                TextColor = Color.Black,
                FontFamily = "Source Sans Pro",
                //FontAttributes = FontAttributes.Bold,
                FontSize = 23,
                Margin = new Thickness(20,20,20,10)
            };

            _whiteframeLayout.Children.Add(youChose);

            Frame grayBox = new Frame
            {
                BackgroundColor = Color.FromHex("F0ECEC"),
                HasShadow = false,
                CornerRadius = 8,
                VerticalOptions = LayoutOptions.CenterAndExpand, // CHANGE
                Margin = new Thickness(40,0,40,0)
                
            };

            _whiteframeLayout.Children.Add(grayBox);

            StackLayout grayLayout = new StackLayout
            {
                VerticalOptions = LayoutOptions.CenterAndExpand
            };

            grayBox.Content = grayLayout;

            Label domainArea = new Label
            {
                Text = domain,
                TextColor = Color.Black,
                FontFamily = "Source Sans Pro",
                FontAttributes = FontAttributes.Bold,
                FontSize = 30,
                VerticalTextAlignment = TextAlignment.Center, // CHANGE  
                HorizontalTextAlignment = TextAlignment.Center
            };

            grayLayout.Children.Add(domainArea);

            Button selectDiff = new Button
            {
                Text = "Select a different module",
                HorizontalOptions = LayoutOptions.Center,
                Margin = new Thickness(0,10,0,0),
                WidthRequest = 180,
                TextColor = Color.Black,
                BackgroundColor = Color.Transparent,
                //BackgroundColor = Color.FromHex("B5E7FA"), // CHANGED
                FontSize = 14,
                CornerRadius = 8
            };
            grayLayout.Children.Add(selectDiff);

            Label feeling = new Label
            {
                Text = "How satisfied are you feeling about this area in your life?",
                HorizontalTextAlignment = TextAlignment.Center,
                TextColor = Color.Black,
                FontSize = 20,
                Margin = new Thickness(15, 10, 15, 20)
            };

            _whiteframeLayout.Children.Add(feeling);

            Frame likertBox = new Frame
            {
                CornerRadius = 30, 
                HasShadow = true, // CHANGED FROM False
                HeightRequest = 65, // from 70 
                Padding = new Thickness(0),
                BorderColor = Color.Black, // CHANGED FROM GRAY
                Margin = new Thickness(20, 0, 20, 0),
                HorizontalOptions = LayoutOptions.Center
            };

            Grid likertGrid = new Grid
            {
                Margin = new Thickness(0),
                Padding = new Thickness(0),
                ColumnSpacing = -10,
                RowDefinitions =
                {
                    new RowDefinition {Height = new GridLength(1, GridUnitType.Star)}
                },
                ColumnDefinitions =
                {
                    new ColumnDefinition {Width = new GridLength(1, GridUnitType.Star)},
                    new ColumnDefinition {Width = new GridLength(1, GridUnitType.Star)},
                    new ColumnDefinition {Width = new GridLength(1, GridUnitType.Star)},
                    new ColumnDefinition {Width = new GridLength(1, GridUnitType.Star)},
                    new ColumnDefinition {Width = new GridLength(1, GridUnitType.Star)},
                }
            };

            _whiteframeLayout.Children.Add(likertBox);

            likertBox.Content = likertGrid;

            ImageButton L1 = new ImageButton
            {
                BackgroundColor = Color.Transparent,
                Source = "L1.png"
            };
            ImageButton L2 = new ImageButton
            {
                BackgroundColor = Color.Transparent,
                Source = "L2.png"
            };
            ImageButton L3 = new ImageButton
            {
                BackgroundColor = Color.Transparent,
                Source = "L3.png"
            };
            ImageButton L4 = new ImageButton
            {
                BackgroundColor = Color.Transparent,
                Source = "L4.png"
            };
            ImageButton L5 = new ImageButton
            {
                BackgroundColor = Color.Transparent,
                Source = "L5.png"
            };

            likertGrid.Children.Add(L1, 0, 0);
            likertGrid.Children.Add(L2, 1, 0);
            likertGrid.Children.Add(L3, 2, 0);
            likertGrid.Children.Add(L4, 3, 0);
            likertGrid.Children.Add(L5, 4, 0);

            Button next = new Button
            {
                Text = "Next",
                Margin = new Thickness(10, 20, 10, 35),
                TextColor = Color.Black, 
                BackgroundColor = Color.FromHex("48AADF"), 
                FontFamily = "Source Sans Pro",
                FontSize = 20,
                FontAttributes = FontAttributes.Bold,
                CornerRadius = 8,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.EndAndExpand, 
                WidthRequest = 150

            };

            next.Clicked += onNextClicked;

            async void onNextClicked(object sender, EventArgs args)
            {
                await Navigation.PushAsync(new ScenarioPage());

            };
            _whiteframeLayout.Children.Add(next);

        }
    }
}
