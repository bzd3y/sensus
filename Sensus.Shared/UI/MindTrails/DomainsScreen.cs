﻿// Copyright 2014 The Rector & Visitors of the University of Virginia
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
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using Rg.Plugins.Popup.Animations;

namespace Sensus.UI.MindTrails
{
    public class DomainsScreen : BannerFrameTool // inherit from BannerFrameTool because this page has a banner + toolbar
    {
        public DomainsScreen()
        {
            NavigationPage.SetHasNavigationBar(this, false); // probably can delete this 

            Content = _contentLayout; // set content of page equal to the content layout of BannerFrameTool


            StackLayout domainLayout = new StackLayout // layout to hold each domain 
            {
                // could have used _contentStack for this. This was created before I added _contentStack 
            };

            _contentLayout.Children.Add(domainLayout, // add to BannerFrameTool's content 
                heightConstraint: Constraint.RelativeToParent((parent) =>
                { return parent.Height; }),
                yConstraint: Constraint.RelativeToParent((parent) =>
                { return parent.Height * .15 ; }),
                widthConstraint: Constraint.RelativeToParent((parent) =>
                { return parent.Width; }));

            Label chooseFocus = new Label
            {
                Text = "Choose a focus for today",
                FontAttributes = FontAttributes.Bold,
                FontSize = 28,
                TextColor = Color.Black,
                HorizontalTextAlignment = TextAlignment.Center
            };

            domainLayout.Children.Add(chooseFocus);

            Label clickTile = new Label
            {
                Text = "Click tile for more details",
                TextColor = Color.Black,
                FontSize = 13,
                HorizontalTextAlignment = TextAlignment.Center
            };

            domainLayout.Children.Add(clickTile);

            Grid tileGrid = new Grid // grid with all domain tiles  
            {
                BackgroundColor = Color.FromHex("E5E7ED"),
                Margin = 15,
                RowDefinitions = // three rows
                {
                    new RowDefinition {Height = new GridLength(1, GridUnitType.Star) },
                    new RowDefinition {Height = new GridLength(1, GridUnitType.Star) },
                    new RowDefinition {Height = new GridLength(1, GridUnitType.Star) }
                },
                ColumnDefinitions = // two columns 
                {
                    new ColumnDefinition {Width = new GridLength(1, GridUnitType.Star)},
                    new ColumnDefinition {Width = new GridLength(1, GridUnitType.Star)}
                }
            };

            // creating all tiles

            Button relationships = new Button 
            {
                Text = "Relationships",
                TextColor = Color.White,
                BackgroundColor = Color.FromHex("166DA3"),
                FontFamily = "Source Sans Pro",
                FontSize = 19,
                FontAttributes = FontAttributes.Bold,
                CornerRadius = 8,
                Margin = new Thickness(3),
                HeightRequest = 125
            };

            relationships.Clicked += onRelationshipsClicked; // when relationships is clicked

            async void onRelationshipsClicked(object sender, EventArgs args)
            {
                // animation for popup
                var popupProperties = new PopupPage();
                var scaleAnimation = new ScaleAnimation
                {
                    PositionIn = Rg.Plugins.Popup.Enums.MoveAnimationOptions.Right,
                    PositionOut = Rg.Plugins.Popup.Enums.MoveAnimationOptions.Left
                };
                // new pop up page with relationships 
                await PopupNavigation.Instance.PushAsync(new DomainPopUp("Relationships"));
            };

            Button resilience = new Button
            {
                Text = "My Resilience",
                TextColor = Color.White,
                BackgroundColor = Color.FromHex("166DA3"),
                FontFamily = "Source Sans Pro",
                FontSize = 19,
                FontAttributes = FontAttributes.Bold,
                CornerRadius = 8,
                Margin = new Thickness(3),
                HeightRequest = 125
            };
            resilience.Clicked += onResilienceClicked;

            async void onResilienceClicked(object sender, EventArgs args)
            {
                var popupProperties = new PopupPage();
                var scaleAnimation = new ScaleAnimation
                {
                    PositionIn = Rg.Plugins.Popup.Enums.MoveAnimationOptions.Right,
                    PositionOut = Rg.Plugins.Popup.Enums.MoveAnimationOptions.Left
                };
                await PopupNavigation.Instance.PushAsync(new DomainPopUp("My Resilience"));

            };

            Button health = new Button
            {
                Text = "Health",
                TextColor = Color.White,
                BackgroundColor = Color.FromHex("166DA3"),
                FontFamily = "Source Sans Pro",
                FontSize = 19,
                FontAttributes = FontAttributes.Bold,
                CornerRadius = 8,
                Margin = new Thickness(3),
                HeightRequest = 125
            };

            health.Clicked += onHealthClicked;

            async void onHealthClicked(object sender, EventArgs args)
            {
                var popupProperties = new PopupPage();
                var scaleAnimation = new ScaleAnimation
                {
                    PositionIn = Rg.Plugins.Popup.Enums.MoveAnimationOptions.Right,
                    PositionOut = Rg.Plugins.Popup.Enums.MoveAnimationOptions.Left
                };
                await PopupNavigation.Instance.PushAsync(new DomainPopUp("Health"));

            };

            Button uncertainty = new Button
            {
                Text = "Handling Uncertainty",
                TextColor = Color.White,
                BackgroundColor = Color.FromHex("166DA3"),
                FontFamily = "Source Sans Pro",
                FontSize = 19,
                FontAttributes = FontAttributes.Bold,
                CornerRadius = 8,
                Margin = new Thickness(3),
                HeightRequest = 125
            };
            uncertainty.Clicked += onUncertaintyClicked;

            async void onUncertaintyClicked(object sender, EventArgs args)
            {
                var popupProperties = new PopupPage();
                var scaleAnimation = new ScaleAnimation
                {
                    PositionIn = Rg.Plugins.Popup.Enums.MoveAnimationOptions.Right,
                    PositionOut = Rg.Plugins.Popup.Enums.MoveAnimationOptions.Left
                };
                await PopupNavigation.Instance.PushAsync(new DomainPopUp("Handling Uncertainty"));

            };

            Button surpriseMe = new Button
            {
                Text = "Surprise Me!",
                TextColor = Color.White,
                BackgroundColor = Color.FromHex("166DA3"),
                FontFamily = "Source Sans Pro",
                FontSize = 19,
                FontAttributes = FontAttributes.Bold,
                CornerRadius = 8,
                Margin = new Thickness(3),
                HeightRequest = 125
            };

            Button submitIdeas = new Button
            {
                Text = "Submit Ideas",
                TextColor = Color.White,
                BackgroundColor = Color.FromHex("166DA3"),
                FontFamily = "Source Sans Pro",
                FontSize = 19,
                FontAttributes = FontAttributes.Bold,
                CornerRadius = 8,
                Margin = new Thickness(3),
                HeightRequest = 125
            };

            // add all to tile grid 
            tileGrid.Children.Add(relationships, 0, 0);
            tileGrid.Children.Add(resilience, 1, 0);
            tileGrid.Children.Add(health, 0, 1);
            tileGrid.Children.Add(uncertainty, 1, 1);
            tileGrid.Children.Add(surpriseMe, 0, 2);
            tileGrid.Children.Add(submitIdeas, 1, 2);

            domainLayout.Children.Add(tileGrid);

        }
    }
}
