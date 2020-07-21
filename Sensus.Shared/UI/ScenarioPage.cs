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
    public class ScenarioPage
    {
        public ScenarioPage()
        {
            RelativeLayout relativeLayout = new RelativeLayout
            {
                BackgroundColor= Color.FromHex("E5E7ED"),
                Padding= new Thickness(20,15),

            };
            Frame bannerFrame = new Frame {
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
                Padding=0,
                HorizontalOptions=LayoutOptions.Center,
            };

            relativeLayout.Children.Add(bannerFrame,
                heightConstraint: Constraint.RelativeToParent ((parent) =>
                    { return parent.Height * 0.16; }),
                widthConstraint: Constraint.RelativeToParent((parent) =>
                    { return parent.Width; }));
            // if this isn't working, try: 
            // widthConstraint:Constraint.RelativeToParent(parent => parent.Width));

            relativeLayout.Children.Add(whiteFrame,
                heightConstraint: Constraint.RelativeToParent(
                    (parent) => { return parent.Height * .75; }),
                widthConstraint: Constraint.RelativeToView(bannerFrame,
                    (parent, sibling) => { return parent.Width * .8; }),
                xConstraint: Constraint.RelativeToParent(
                    (parent) => { return parent.Width * .1; }),
                yConstraint: Constraint.RelativeToView(bannerFrame,
                    (parent, sibling) => { return sibling.Height + 30; }));
        }
    }
}
