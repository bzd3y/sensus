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
using SkiaSharp;
using SkiaSharp.Views.Forms;

namespace Sensus.UI
{
    public class RoundScore : BannerFrameTool
    {
        class ChartData
        {
            public ChartData(int value, SKColor color)
            {
                Value = value;
                Color = color;
            }

            public int Value { private set; get; }

            public SKColor Color { private set; get; }
        }
        ChartData[] chartData =
        {

            new ChartData(roundScore *10, SKColors.SteelBlue), // SKColors.Blue
            new ChartData(100 - roundScore * 10, SKColors.Transparent),
        };

        public RoundScore()
        {
            Console.WriteLine("Score");

            Console.WriteLine(roundScore);
            SKCanvasView canvasView = new SKCanvasView();
            canvasView.PaintSurface += OnCanvasViewPaintSurface;

            Content = _contentLayout;

            Label congrats = new Label
            {
                Text = "Congratulations!",
                FontAttributes = FontAttributes.Bold,
                FontSize = 30,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                VerticalOptions = LayoutOptions.Start,
                TextColor = Color.Black
            };
            _contentStack.Children.Add(congrats);

            Frame fullFrame = new Frame
            {
                VerticalOptions = LayoutOptions.FillAndExpand,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Padding = 0,
                BackgroundColor = Color.Transparent

            };
 

            fullFrame.Content = canvasView;

            _contentStack.Children.Add(fullFrame);

            Button nextRound = new Button
            {
                Text = "Start Round " + roundCounter.ToString(),
                Margin = new Thickness(10, 20, 10, 10),
                TextColor = Color.Black, // CHANGE white
                BackgroundColor = Color.FromHex("48AADF"), // CHANGE 166DA3 
                FontFamily = "Source Sans Pro",
                FontSize = 20,
                FontAttributes = FontAttributes.Bold,
                CornerRadius = 8,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.StartAndExpand,
                WidthRequest = 180

            };
            _contentStack.Children.Add(nextRound);
        }

        void OnCanvasViewPaintSurface(object sender, SKPaintSurfaceEventArgs args)
        {
            SKImageInfo info = args.Info;
            SKSurface surface = args.Surface;
            SKCanvas canvas = surface.Canvas;

            canvas.Clear();

            int totalValues = 0;

            foreach (ChartData item in chartData)
            {
                totalValues += item.Value;
            }

            //SKPoint center = new SKPoint(info.Width / 2, info.Height / 2);
            SKPoint center = new SKPoint(info.Width / 2, info.Height / 2);
            // at center put another pie chart (but white), with text inside
            float radius = 200;
            SKRect rect = new SKRect(center.X - radius, center.Y - radius,
                                     center.X + radius, center.Y + radius);

            float startAngle = -90;

            foreach (ChartData item in chartData)
            {
                float sweepAngle = 360f * item.Value / totalValues;

                using (SKPath path = new SKPath())
                using (SKPaint fillPaint = new SKPaint())
                {
                    path.MoveTo(center);
                    path.ArcTo(rect, startAngle, sweepAngle, false);
                    path.Close();

                    fillPaint.Style = SKPaintStyle.Fill;
                    fillPaint.Color = item.Color;

                    canvas.Save();

                    // Fill the path
                    canvas.DrawPath(path, fillPaint);
                    canvas.Restore();
                }

                startAngle += sweepAngle;
            }
        }
    }
}
