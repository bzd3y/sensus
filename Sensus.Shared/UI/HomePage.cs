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
using Sensus.Android;
using Xamarin.Forms;

namespace Sensus.UI
{
    public class HomePage : ContentPage
    {
        protected RelativeLayout _contentLayout;
        protected StackLayout _contentStack;
        protected StackLayout _whiteframeLayout;

        public HomePage()
        {
            NavigationPage.SetHasNavigationBar(this, false);

            _contentLayout = new RelativeLayout
            {

                BackgroundColor = Color.FromHex("E5E7ED"),
                Padding = new Thickness(20, 15)
            };
            Content = _contentLayout;
            _contentLayout.Children.Add(new Image
            {
                Source = "Background.png",
                Aspect = Aspect.Fill,
            },
            widthConstraint: Constraint.RelativeToParent((parent) =>
            { return parent.Width * 1.1; })); // if this doesn't work, try getting rid of the background color for contentlayout and manually setting it 

            Frame bannerFrame = new Frame
            {
                BackgroundColor = Color.FromHex("233367"),
                HeightRequest = 80,
                CornerRadius = 0
            };

            RelativeLayout bannerLayout = new RelativeLayout();

            Image logoImage = new Image { Source = "Logo.png" };
            Label sessionNum = new Label
            {
                Text = "Session 1",
                TextColor = Color.White,
                FontFamily = "Source Sans Pro",
                FontAttributes = FontAttributes.Bold,
                FontSize = 30
            };
            bannerLayout.Children.Add(logoImage,
                xConstraint: Constraint.RelativeToParent((parent) =>
                { return parent.Width * .01; }),
                yConstraint: Constraint.RelativeToParent((parent) =>
                { return parent.Height * .13; }), // CHANGE from .15
                widthConstraint: Constraint.RelativeToParent((parent) =>
                { return parent.Width * .2; }));

            bannerLayout.Children.Add(sessionNum,
                xConstraint: Constraint.RelativeToView(logoImage,
                    (parent, sibling) => { return sibling.Width * 1.2; }),
                yConstraint: Constraint.RelativeToParent((parent) =>
                { return parent.Height - 40; }));

            bannerFrame.Content = bannerLayout;

            _contentLayout.Children.Add(bannerFrame,
                heightConstraint: Constraint.RelativeToParent((parent) =>
                { return parent.Height * 0.13; }),
                widthConstraint: Constraint.RelativeToParent((parent) =>
                { return parent.Width; }));

            _contentStack = new StackLayout
            {

            };

            _contentLayout.Children.Add(_contentStack,
                heightConstraint: Constraint.RelativeToParent((parent) =>
                { return parent.Height; }),
                yConstraint: Constraint.RelativeToParent((parent) =>
                { return parent.Height * .15; }),
                widthConstraint: Constraint.RelativeToParent((parent) =>
                { return parent.Width; }));

            Frame whiteFrame = new Frame
            {
                BackgroundColor = Color.White,
                HasShadow = true, // Change from true
                //BorderColor = Color.Gray, // maybe change back 
                CornerRadius = 10,
                Padding = 0,
                Margin = new Thickness(30, 0, 30, 40), // CHANGED from  0
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
                IsClippedToBounds = true,

            };

            _contentStack.Children.Add(whiteFrame);

            _whiteframeLayout = new StackLayout // Compare with BannerAndToolBarControl
            {
                Padding = new Thickness(0), // add this to other ^
                Margin = new Thickness(0)
            };

            whiteFrame.Content = _whiteframeLayout;

            Frame toolbarFrame = new Frame
            {
                //CornerRadius = 18, CHANGE
                BorderColor = Color.FromHex("F0ECEC"),
                HeightRequest = 70,
                Padding = new Thickness(0, 0, 0, 50),
                Margin = new Thickness(0, 15, 0, 0),
                BackgroundColor = Color.FromHex("D9FFFFF")


            };
            Grid toolbarGrid = new Grid
            {
                Margin = 0,
                Padding = new Thickness(0, 10, 0, 18),
                RowDefinitions = {
                    new RowDefinition {Height = new GridLength(1, GridUnitType.Star) }
                },
                ColumnDefinitions = {
                    new ColumnDefinition {Width = new GridLength(1, GridUnitType.Star) },
                    new ColumnDefinition {Width = new GridLength(1, GridUnitType.Star) },
                    new ColumnDefinition {Width = new GridLength(1, GridUnitType.Star) }
                }
            };
            ImageButton journalButton = new ImageButton
            {
                Source = "JournalGray.png",
                BackgroundColor = Color.Transparent,
                Margin = new Thickness(0, 0, 0, 10)
            };
            ImageButton homeButton = new ImageButton
            {
                Source = "Home.png",
                BackgroundColor = Color.Transparent,
                Margin = new Thickness(0, 0, 0, 10)
            };
            ImageButton profileButton = new ImageButton
            {
                Source = "ProfileGray.png",
                BackgroundColor = Color.Transparent,
                Margin = new Thickness(0, 0, 0, 10)
            };
            toolbarGrid.Children.Add(journalButton, 0, 0); // column, row 
            toolbarGrid.Children.Add(homeButton, 1, 0);
            toolbarGrid.Children.Add(profileButton, 2, 0);

            toolbarFrame.Content = toolbarGrid;
            _contentLayout.Children.Add(toolbarFrame,
                widthConstraint: Constraint.RelativeToParent(
                    (parent) => { return parent.Width; }),
                yConstraint: Constraint.RelativeToParent(
                    (parent) => { return parent.Height - 85; }));

            Frame dateFrame = new Frame
                {
                    BackgroundColor = Color.White,
                    HasShadow = false,
                    Padding = 0,
                    VerticalOptions = LayoutOptions.Start,
                    HorizontalOptions = LayoutOptions.Center,
                    WidthRequest = 250,
                    HeightRequest = 140,
                    CornerRadius = 10
                };

            _contentStack.Children.Add(dateFrame);

            Grid dateGrid = new Grid
            {
                ColumnDefinitions =
                {
                    new ColumnDefinition {
                        Width = new GridLength(1, GridUnitType.Star)
                    },
                    new ColumnDefinition {
                        Width = new GridLength(1.5, GridUnitType.Star)
                    },
                }
            };

            dateFrame.Content = dateGrid;

            Button date = new Button
            {
                Text = "0",
                Margin = new Thickness(20,30,20,50),
                TextColor = Color.Black,
                FontAttributes = FontAttributes.Bold,
                VerticalOptions = LayoutOptions.Center, // CHANGE 7/30
                FontSize = 35,
                BackgroundColor = Color.FromHex("CECECE")
            };

            dateGrid.Children.Add(date);

            StackLayout daysStack = new StackLayout
            {

            };
            dateGrid.Children.Add(daysStack, 1, 0);

            Label days = new Label
            {
                Text = "Days",
                HorizontalTextAlignment = TextAlignment.Start,
                Margin = new Thickness(0,30,0,0),
                FontSize = 25,
                TextColor = Color.Black
            };

            daysStack.Children.Add(days);

            Label untilNext = new Label
            {
                Text = "until next module",
                HorizontalTextAlignment = TextAlignment.Start,
                VerticalTextAlignment = TextAlignment.Center,
                FontSize = 15,
                TextColor = Color.Black
            };

            daysStack.Children.Add(untilNext);

            Button startSession = new Button
            {
                Text = "Start my session!",
                BackgroundColor = Color.FromHex("48AADF"),
                FontAttributes = FontAttributes.Bold,
                FontSize = 20,
                TextColor = Color.White,
                FontFamily = "Source Sans Pro",
                CornerRadius = 8,
                HeightRequest = 70,
                WidthRequest = 200,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center, // change

            };

            //startSession.Visual = new CustomVisual();

            startSession.Clicked += onSessionStart;

            async void onSessionStart(object sender, EventArgs args)
            {
                //await Navigation.PushAsync(new DomainsScreen());
                await Navigation.PushAsync(new IfThenJournalPage());


            };

            _contentStack.Children.Add(startSession);
        }
    }
}
