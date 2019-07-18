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
using Sensus.Probes.User.Health;
using Syncfusion.SfChart.XForms;

namespace Sensus.Android.Probes.User.Health
{
    public class GoogleFitNumberOfTimesFallenProbe : GoogleFitSamplingProbe
    {
        public sealed override string DisplayName
        {
            get
            {
                return "Number of Times Fallen";
            }
        }

        public override Type DatumType
        {
            get
            {
                return typeof(NumberOfTimesFallenDatum);
            }
        }

        public override int DefaultPollingSleepDurationMS
        {
            get
            {
                return (int)TimeSpan.FromDays(5).TotalMilliseconds;
            }
        }

        public GoogleFitNumberOfTimesFallenProbe() : base()
        {
        }

        protected override Datum ConvertSampleToDatum()
        {
			//HKQuantitySample quantitySample = sample as HKQuantitySample;

			//if (quantitySample == null)
			//{
			//    return null;
			//}
			//else
			//{
			//    return new NumberOfTimesFallenDatum(new DateTimeOffset(quantitySample.StartDate.ToDateTime(), TimeSpan.Zero), (int)quantitySample.Quantity.GetDoubleValue(HKUnit.Count));
			//}

			return null;
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
                    Text = "Number of Time Fallen"
                }
            };
        }

        protected override ChartDataPoint GetChartDataPointFromDatum(Datum datum)
        {
            return new ChartDataPoint(datum.Timestamp.LocalDateTime, (datum as NumberOfTimesFallenDatum).NumberOfTimesFallen);
        }
    }
}