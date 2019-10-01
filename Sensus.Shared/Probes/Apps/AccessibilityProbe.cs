﻿using System;
using Syncfusion.SfChart.XForms;

namespace Sensus.Probes.Apps
{
	public abstract class AccessibilityProbe : ListeningProbe
	{
		public override string DisplayName
		{
			get
			{
				return "Accessibility";
			}
		}

		public override Type DatumType
		{
			get
			{
				return typeof(AccessibilityDatum);
			}
		}

		protected override bool DefaultKeepDeviceAwake
		{
			get
			{
				return false;
			}
		}

		protected override string DeviceAwakeWarning
		{
			get
			{
				return "";
			}
		}

		protected override string DeviceAsleepWarning
		{
			get
			{
				return "";
			}
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

		protected override ChartSeries GetChartSeries()
		{
			throw new NotImplementedException();
		}
	}
}
