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

using Sensus.Probes.User.Health;
using Syncfusion.SfChart.XForms;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Sensus.Android.Probes.User.Health
{
	public class GoogleFitBirthdateProbe : GoogleFitProbe
	{
		public sealed override string DisplayName => "Google Fit Birthdate";

		public override Type DatumType => typeof(BirthdateDatum);

		public override int DefaultPollingSleepDurationMS => int.MaxValue;

		public GoogleFitBirthdateProbe() : base()
		{
		}

		protected override Task<List<Datum>> PollAsync(CancellationToken cancellationToken)
		{
			List<Datum> data = new List<Datum>();

			//NSError error;
			//NSDate dateOfBirth = HealthStore.GetDateOfBirth(out error);

			//if (error == null)
			//{
			//	if (dateOfBirth == null)
			//	{
			//		throw new Exception("User has not provided -- or has not allowed access to -- their date of birth.");
			//	}
			//	else
			//	{
			//		data.Add(new BirthdateDatum(DateTimeOffset.UtcNow, new DateTimeOffset(dateOfBirth.ToDateTime(), TimeSpan.Zero)));
			//	}
			//}
			//else
			//{
			//	throw new Exception("Error reading date of birth:  " + error.Description);
			//}

			return Task.FromResult(data);
		}

		protected override ChartSeries GetChartSeries()
		{
			throw new NotImplementedException();
		}

		protected override ChartDataPoint GetChartDataPointFromDatum(Datum datum)
		{
			throw new NotImplementedException();
		}

		protected override ChartAxis GetChartPrimaryAxis()
		{
			throw new NotImplementedException();
		}

		protected override RangeAxisBase GetChartSecondaryAxis()
		{
			throw new NotImplementedException();
		}
	}
}