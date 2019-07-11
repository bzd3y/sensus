using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Sensus.UI.UiProperties;
using Syncfusion.SfChart.XForms;

namespace Sensus.Probes.Apps
{
    public abstract class CalendarProbe : ListeningProbe // PollingProbe
    {
		protected override bool DefaultKeepDeviceAwake => false;

		public override Type DatumType => typeof(CalendarDatum);

		public override string DisplayName => "Calendar Events";

		protected override string DeviceAwakeWarning => "";

		protected override string DeviceAsleepWarning => "";

		//[EntryIntegerUiProperty("Read Duration (MS):", true, 5, true)]
		//public int ReadDurationMS
		//{
		//    get
		//    {
		//        return _readDurationMS;
		//    }
		//    set
		//    {
		//        if (value < 5000)
		//        {
		//            value = 5000;
		//        }

		//        _readDurationMS = value;
		//    }

		//}

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

        //protected async override Task<List<Datum>> PollAsync(CancellationToken cancellationToken)
        //{
        //    List<Datum> calendarMetaData = new List<Datum>();

        //    foreach (Datum calendarDatum in await GetCalendarEventsAsync())
        //    {
        //        calendarMetaData.Add(calendarDatum);
        //    }

        //    return calendarMetaData;

        //}

        //protected abstract Task<List<CalendarDatum>> GetCalendarEventsAsync();
    }
}
