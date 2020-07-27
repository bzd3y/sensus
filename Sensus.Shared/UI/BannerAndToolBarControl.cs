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
// does not do anything
namespace Sensus.UI
{
    public class BannerAndToolBarControl : ContentPage
    {
        protected RelativeLayout _contentLayout;

        public BannerAndToolBarControl()
        {
            NavigationPage.SetHasNavigationBar(this, false);
            _contentLayout = new RelativeLayout
            {

                BackgroundColor = Color.FromHex("E5E7ED"),
                Padding = new Thickness(20, 15)
            };

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
                Text = "Session 2",
                TextColor = Color.White,
                FontFamily = "Source Sans Pro",
                FontAttributes = FontAttributes.Bold,
                FontSize = 30
            };
            bannerLayout.Children.Add(logoImage,
                xConstraint: Constraint.RelativeToParent((parent) =>
                { return parent.Width * .01; }),
                yConstraint: Constraint.RelativeToParent((parent) =>
                { return parent.Height * .3; }),
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

            Frame toolbarFrame = new Frame
            {
                CornerRadius = 18,
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
                BackgroundColor = Color.Transparent
            };
            ImageButton homeButton = new ImageButton
            {
                Source = "Home.png",
                BackgroundColor = Color.Transparent
            };
            ImageButton profileButton = new ImageButton
            {
                Source = "ProfileGray.png",
                BackgroundColor = Color.Transparent
            };
            toolbarGrid.Children.Add(journalButton, 0, 0); // column, row 
            toolbarGrid.Children.Add(homeButton, 1, 0);
            toolbarGrid.Children.Add(profileButton, 2, 0);

            toolbarFrame.Content = toolbarGrid;
            _contentLayout.Children.Add(toolbarFrame,
                widthConstraint: Constraint.RelativeToParent(
                    (parent) => { return parent.Width; }),
                yConstraint: Constraint.RelativeToParent(
                    (parent) => { return parent.Height - 85; })); // CHANGED FROM 90
        }
    }
}
