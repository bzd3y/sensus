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
    public class ReflectionsPage : BannerFrameTool
    {
        public ReflectionsPage()
        {
            Content = _contentLayout;

            _contentLayout.BackgroundColor = Color.FromHex("233367");
            _contentStack.BackgroundColor = Color.FromHex("233367");

            Label reflect1 = new Label
            {
                Text = "What did you learn " +
    "from the last story?",
                Margin = new Thickness(30,100,30,0),
                FontSize = 30,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
                FontFamily = "Source Sans Pro",
                TextColor = Color.White,
                HorizontalTextAlignment = TextAlignment.Center
            };

            Label reflect2 = new Label
            {
                Text = "Let's take a moment to reflect!",
                Margin = new Thickness(30, 100, 30, 0),
                FontSize = 30,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
                FontFamily = "Source Sans Pro",
                TextColor = Color.White,
                HorizontalTextAlignment = TextAlignment.Center
            };

            Label reflect3 = new Label
            {
                Text = "Take a moment " +
                "to add a reflection " +
                "to your journal.",
                Margin = new Thickness(30, 75, 30, 10),
                FontSize = 30,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
                FontFamily = "Source Sans Pro",
                TextColor = Color.White,
                HorizontalTextAlignment = TextAlignment.Center
            };

            Button journal = new Button
            {
                Text = "Go to journal!",
                FontAttributes = FontAttributes.Bold,
                FontSize = 22,
                CornerRadius = 10,
                WidthRequest = 200,
                HorizontalOptions = LayoutOptions.Center,
                TextColor = Color.Black,
                BackgroundColor = Color.FromHex("B5E7FA"),
                FontFamily = "Source Sans Pro"

            };

            journal.Clicked += onJournalClicked;

            async void onJournalClicked(object sender, EventArgs args)
            {
                await Navigation.PushAsync(new IfThenJournalPage());
            };

            _contentStack.Children.Add(reflect1);

            Device.StartTimer(TimeSpan.FromSeconds(3), () =>
            {
                _contentStack.Children.Remove(reflect1);
                _contentStack.Children.Add(reflect2);

                return false;
            });


            Device.StartTimer(TimeSpan.FromSeconds(6), () =>
            {
                _contentStack.Children.Remove(reflect2);
                _contentStack.Children.Add(reflect3);
                _contentStack.Children.Add(journal);
                return false;
            });



        }
    }
}
