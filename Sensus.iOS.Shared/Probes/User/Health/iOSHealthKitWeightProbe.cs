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
using Sensus;
using Sensus.Probes.User.Health;
using HealthKit;
using Xamarin.Forms.Platform.iOS;
using Syncfusion.SfChart.XForms;

namespace Sensus.iOS.Probes.User.Health
{
    public class iOSHealthKitWeightProbe : iOSHealthKitSamplingProbe
    {
        public sealed override string DisplayName
        {
            get
            {
                return "HealthKit Weight";
            }
        }

        public override Type DatumType
        {
            get
            {
                return typeof(WeightDatum);
            }
        }

        public override int DefaultPollingSleepDurationMS
        {
            get
            {
                return (int)TimeSpan.FromDays(5).TotalMilliseconds;
            }
        }

        public iOSHealthKitWeightProbe()
            : base(HKQuantityType.Create(HKQuantityTypeIdentifier.BodyMass))
        {
        }

        protected override Datum ConvertSampleToDatum(HKSample sample)
        {
            HKQuantitySample quantitySample = sample as HKQuantitySample;

            if (quantitySample == null)
            {
                return null;
            }
            else
            {
                return new WeightDatum(new DateTimeOffset(quantitySample.StartDate.ToDateTime(), TimeSpan.Zero), quantitySample.Quantity.GetDoubleValue(HKUnit.Pound));
            }
        }

        protected override ChartSeries GetChartSeries()
        {
            return new LineSeries();
        }

        protected override ChartAxis GetChartPrimaryAxis()
        {
            return new DateTimeAxis
            {
                Title = new ChartAxisTitle
                {
                    Text = "Time"
                }
            };
        }

        protected override RangeAxisBase GetChartSecondaryAxis()
        {
            return new NumericalAxis
            {
                Title = new ChartAxisTitle
                {
                    Text = "Weight (Pounds)"
                }
            };
        }

        protected override ChartDataPoint GetChartDataPointFromDatum(Datum datum)
        {
            return new ChartDataPoint(datum.Timestamp.LocalDateTime, (datum as WeightDatum).WeightPounds);
        }
    }
}