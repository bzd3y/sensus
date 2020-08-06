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
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup;
using Rg.Plugins.Popup.Animations;
using Rg.Plugins.Popup.Services;

namespace Sensus.UI
{
    public class DomainPopUp : PopupPage
    {
        StackLayout _frameLayout;
        
        public DomainPopUp()
        {
            BackgroundColor = Color.FromHex("D9FFFFFF");


            _frameLayout = new StackLayout
            {
                VerticalOptions = LayoutOptions.Center
            };


            Frame blueFrame = new Frame
            {
                BackgroundColor = Color.FromHex("166DA3"),
                Margin = 20,
                CornerRadius = 10,
                IsClippedToBounds = true,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center
            };

            _frameLayout.Children.Add(blueFrame);

            Content = blueFrame;

            StackLayout blueLayout = new StackLayout
            {
                Spacing = 0 
            };

            blueFrame.Content = blueLayout;

            Label domainArea = new Label
            {
                Text = "Relationships",
                FontSize = 30,
                HorizontalTextAlignment = TextAlignment.Center,
                FontFamily = "Source Sans Pro",
                TextColor = Color.White,
                FontAttributes = FontAttributes.Bold
            };

            Image domainIcon = new Image
            {
                Source = "Hands.png", // add this
                WidthRequest = 100
            };

            Label domainDetails = new Label
            {
                Text = "Reduce my worries about relationships and what others think of me",
                HorizontalOptions = LayoutOptions.Center,
                FontSize = 20,
                Margin = 20,
                TextColor = Color.White,
                HorizontalTextAlignment = TextAlignment.Center
            };

            Button selectModule = new Button
            {
                Text = "Select Module",
                FontSize = 20,
                FontAttributes = FontAttributes.Bold,
                TextColor = Color.Black,
                BackgroundColor = Color.FromHex("B5E7FA"),
                FontFamily = "Source Sans Pro",
                CornerRadius = 8,
                WidthRequest = 150
            };

            selectModule.Clicked += onModuleSelected;

            async void onModuleSelected(object sender, EventArgs args)
            {
                // await Navigation.PushAsync(new RateDomain());
                // try await Navigation.PushAsync(new NavigationPage(new RateDomain());
                // try making main page a navigation page
                await Navigation.PushModalAsync(new NavigationPage(new RateDomain())); // worked

                await PopupNavigation.Instance.PopAsync(true);

                // CANNOT GO BACK ON ANDROID : public override void OnBackPressed(){ if(user.IsAuthenticated(){base.OnBackPressed();}}
            }

            Button selectDiff = new Button
            {
                Text = "Select a different module",
                TextColor = Color.White,
                BackgroundColor = Color.Transparent
            };

            selectDiff.Clicked += selectDiff_Clicked;

            async void selectDiff_Clicked(object sender, EventArgs args)
            {
                await Navigation.PopAsync(true);
            };

            blueLayout.Children.Add(domainArea);
            blueLayout.Children.Add(domainIcon);
            blueLayout.Children.Add(domainDetails);
            blueLayout.Children.Add(selectModule);
            blueLayout.Children.Add(selectDiff);
        }


    }
}
