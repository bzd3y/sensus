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
    public class IfThenJournalPage : BannerFrameTool
    {
        public IfThenJournalPage()
        {
            Content = _contentLayout;

            Label copyMessage = new Label
            {
                Text = "Copy message below "+
                "to add to your journal",
                Margin = new Thickness(15,5,15,10),
                FontAttributes = FontAttributes.Bold,
                HorizontalTextAlignment = TextAlignment.Center,
                TextColor = Color.Black,
                FontSize = 24
            };

            _whiteframeLayout.Children.Add(copyMessage);

            Frame grayFrame1 = new Frame
            {
                BackgroundColor = Color.FromHex("F0ECEC"),
                CornerRadius = 10,
                Margin = new Thickness(15, 2, 15, 10),

            };

            Frame grayFrame2 = new Frame
            {
                BackgroundColor = Color.FromHex("F0ECEC"),
                CornerRadius = 10,
                Margin = new Thickness(15, 0, 15, 10),

            };
            Label ifMessage = new Label
            {
                BackgroundColor = Color.FromHex("F0ECEC"),
                TextColor = Color.Black,
                HorizontalTextAlignment = TextAlignment.Start,
                FontSize = 22,
                Margin = new Thickness(10,0,10,0),
                HeightRequest = 60
            };

            var ifMessageString = new FormattedString();
            ifMessageString.Spans.Add(new Span { Text = "If ", FontSize = 22, FontAttributes = FontAttributes.Bold});
            ifMessageString.Spans.Add(new Span { Text = "my work is being evaluated...", FontSize = 22 });
            ifMessage.FormattedText = ifMessageString;


            Label thenMessage = new Label
            {
                BackgroundColor = Color.FromHex("F0ECEC"),
                TextColor = Color.Black,
                HorizontalTextAlignment = TextAlignment.End,
                FontSize = 22,
                Margin = new Thickness(10, 0, 10, 0),
                HeightRequest = 60
            };
            var thenMessageString = new FormattedString();
            thenMessageString.Spans.Add(new Span { Text = "then ", FontSize = 22, FontAttributes = FontAttributes.Bold });
            thenMessageString.Spans.Add(new Span { Text = "I will remind myself that I did my best", FontSize = 22 });
            thenMessage.FormattedText = thenMessageString;

            grayFrame1.Content = ifMessage;
            grayFrame2.Content = thenMessage;
            _whiteframeLayout.Children.Add(grayFrame1);
            _whiteframeLayout.Children.Add(grayFrame2);

            Frame journalFrame = new Frame
            {
                CornerRadius = 10,
                Margin = new Thickness(20, -30, 20, -20),
                Content = new Editor
                {
                    HeightRequest = 100,
                    WidthRequest = 400,
                    BackgroundColor = Color.White,
                    HorizontalOptions = LayoutOptions.CenterAndExpand,
                    TextColor = Color.Black
                }
            };
            Button add = new Button
            {
                // make it so once text is written, then you can add
                // gray and not enabled before text 
                Text = "Add",
                HorizontalOptions = LayoutOptions.Center,
                Margin = -30,
                CornerRadius = 10
                
            };

            add.Clicked += on_add;

            async void on_add(object sender, EventArgs args)
            {
                scenarioCounter++;
                await Navigation.PushAsync(new ScenarioPage());

            }

            _contentStack.Children.Add(journalFrame);
            _contentStack.Children.Add(add);

        }
    }
}
