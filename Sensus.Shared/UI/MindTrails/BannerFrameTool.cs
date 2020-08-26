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

namespace Sensus.UI.MindTrails
{
    public class BannerFrameTool : ContentPage
    {
        protected RelativeLayout _contentLayout;
        protected StackLayout _contentStack;
        protected StackLayout _whiteframeLayout;
        public static int scenarioCounter = 0; // counts scenarios in a given session 
        public static int sessionNumber = 1; // reflects the session number  
        public static int roundScore1 = 10; // score in round 1 in a given session 
        public static int roundScore2 = 10; // score in round 2 in a given session 
        public static int roundScore3 = 10; // round 3
        public static int roundScore4 = 10; // round 4
        public static int roundCounter = 1; // reflects round number in a given session 

        public BannerFrameTool()
        {
            NavigationPage.SetHasNavigationBar(this, false); // deletes top navigation bar that is default with a new navigation page 

            _contentLayout = new RelativeLayout
            {

                BackgroundColor = Color.FromHex("E5E7ED"),
                Padding = new Thickness(20, 15)
            };

            Frame bannerFrame = new Frame // frame that holds banner at top 
            {
                BackgroundColor = Color.FromHex("233367"),
                HeightRequest = 80,
                CornerRadius = 0
            };

            RelativeLayout bannerLayout = new RelativeLayout(); // layout within the bannerFrame 

            Image logoImage = new Image { Source = "Logo.png" }; // MT logo 
            Label sessionNum = new Label // "Session X" 
            {
                Text = "Session " + sessionNumber.ToString(),
                TextColor = Color.White,
                FontFamily = "Source Sans Pro",
                FontAttributes = FontAttributes.Bold,
                FontSize = 30
            };
            bannerLayout.Children.Add(logoImage, // add logo to banner 
                xConstraint: Constraint.RelativeToParent((parent) =>
                { return parent.Width * .01; }),
                yConstraint: Constraint.RelativeToParent((parent) =>
                { return parent.Height * .13; }), 
                widthConstraint: Constraint.RelativeToParent((parent) =>
                { return parent.Width * .2; }));

            bannerLayout.Children.Add(sessionNum, // add "Session X" 
                xConstraint: Constraint.RelativeToView(logoImage,
                    (parent, sibling) => { return sibling.Width * 1.2; }),
                yConstraint: Constraint.RelativeToParent((parent) =>
                { return parent.Height - 40; }));

            bannerFrame.Content = bannerLayout; // content of frame is bannerLayout 

            _contentLayout.Children.Add(bannerFrame,
                heightConstraint: Constraint.RelativeToParent((parent) =>
                { return parent.Height * 0.13; }),
                widthConstraint: Constraint.RelativeToParent((parent) =>
                { return parent.Width; }));

            _contentStack = new StackLayout // stack layout that you can easily add elements to in future screens 
            {

            };

            _contentLayout.Children.Add(_contentStack, // add _contentStack to the layout on screen 
                heightConstraint: Constraint.RelativeToParent((parent) =>
                { return parent.Height; }),
                yConstraint: Constraint.RelativeToParent((parent) =>
                { return parent.Height * .15; }),
                widthConstraint: Constraint.RelativeToParent((parent) =>
                { return parent.Width; }));

            Frame whiteFrame = new Frame // white frame that some screens have under the banner
            {
                BackgroundColor = Color.White, 
                HasShadow = true, 
                CornerRadius = 10,
                Padding = 0,
                Margin = new Thickness(20, 0, 20, 10), 
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center, 
                IsClippedToBounds = true,

            };

            _contentStack.Children.Add(whiteFrame); // add white frame to that content stack 

            _whiteframeLayout = new StackLayout // layout within the white frame that you can add to 
            {
                Padding = new Thickness(0), 
                Margin = new Thickness(0) 
            };

            whiteFrame.Content = _whiteframeLayout;




            Frame toolbarFrame = new Frame // tool bar at bottom
            {
                BorderColor = Color.FromHex("F0ECEC"),
                HeightRequest = 70,
                Padding = new Thickness(0, 0, 0, 50),
                Margin = new Thickness(0, 15, 0, 0),
                BackgroundColor = Color.FromHex("D9FFFFF")


            };
            Grid toolbarGrid = new Grid // grid layout within the tool bar 
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
            ImageButton journalButton = new ImageButton // journal button on tool bar
            {
                Source = "JournalGray.png",
                BackgroundColor = Color.Transparent,
                Margin = new Thickness(0, 0, 0, 10)
            };
            ImageButton homeButton = new ImageButton // home button on tool bar 
            {
                Source = "Home.png",
                BackgroundColor = Color.Transparent,
                Margin = new Thickness(0, 0, 0, 10)
            };

            homeButton.Clicked += onHome;

            async void onHome(object sender, EventArgs args)
            {
                await Navigation.PushAsync(new HomePage());

            };

            ImageButton profileButton = new ImageButton // profile button on tool bar
            {
                Source = "ProfileGray.png",
                BackgroundColor = Color.Transparent,
                Margin = new Thickness(0, 0, 0, 10)
            };
            // add all to toolbar 
            toolbarGrid.Children.Add(journalButton, 0, 0);
            toolbarGrid.Children.Add(homeButton, 1, 0);
            toolbarGrid.Children.Add(profileButton, 2, 0);

            toolbarFrame.Content = toolbarGrid;

            // add tool bar to the screen's layout 
            _contentLayout.Children.Add(toolbarFrame,
                widthConstraint: Constraint.RelativeToParent(
                    (parent) => { return parent.Width; }),
                yConstraint: Constraint.RelativeToParent(
                    (parent) => { return parent.Height - 85; }));
        }
    }
}
