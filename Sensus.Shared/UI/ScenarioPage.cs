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
    public class ScenarioPage : ContentPage
    {
        protected Grid _contentGrid;
        protected RelativeLayout _contentLayout;
        protected ListView _notificationList;

        public ScenarioPage()
        {
            Title = "Scenario 1";

            NavigationPage.SetHasNavigationBar(this, false);

            _contentLayout = new RelativeLayout
            {
                BackgroundColor = Color.FromHex("E5E7ED"),
                Padding = new Thickness(20, 15),
            };

            Content = _contentLayout;

            Frame bannerFrame = new Frame
            {
                BackgroundColor = Color.FromHex("233367"),
                HeightRequest = 90,
                CornerRadius = 0
                // eventually add MTlogo + "Session 2" label 
            };

            Frame whiteFrame = new Frame
            {
                BackgroundColor = Color.White,
                HasShadow = false,
                BorderColor = Color.Gray,
                CornerRadius = 10,
                Padding = 0,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
            };

            _contentLayout.Children.Add(whiteFrame,
                heightConstraint: Constraint.RelativeToParent(
                    (parent) => { return parent.Height; }),
                widthConstraint: Constraint.RelativeToParent(
                    (parent) => { return parent.Width; }));
            _contentLayout.Children.Add(bannerFrame,
                heightConstraint: Constraint.RelativeToParent((parent) =>
                { return parent.Height * 0.16; }),
                widthConstraint: Constraint.RelativeToParent((parent) =>
                { return parent.Width; }));
            // if this isn't working, try: 
            // widthConstraint:Constraint.RelativeToParent(parent => parent.Width));


        }

    }
}
