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

            Label reflect1 = new Label
            {
                Text = "What did you learn from " +
    "the last story?",
                FontSize = 40,
                HorizontalOptions = LayoutOptions.Center,
            };

            Label reflect2 = new Label
            {
                Text = "Let's take a moment to reflect!",
                FontSize = 40,
                HorizontalOptions = LayoutOptions.Center,
                FontFamily = "Source Sans Pro",
                TextColor = Color.White,
                FontAttributes = FontAttributes.Bold,
                HorizontalTextAlignment = TextAlignment.Center
            };
            
            Label reflect3 = new Label
            {
                Text = "Take a moment " +
                "to add a reflection" +
                "to your journal.",
                FontSize = 40,
                HorizontalOptions = LayoutOptions.Center,
            };

            Button journal = new Button
            {
                Text = "Go to journal!",
                FontAttributes = FontAttributes.Bold,
                FontSize = 22,
                CornerRadius = 10,
                HorizontalOptions = LayoutOptions.Center,
                WidthRequest = 150,
                TextColor = Color.Black,
                BackgroundColor = Color.FromHex("B5E7FA"),
                FontFamily = "Source Sans Pro"

            };

        }
    }
}
