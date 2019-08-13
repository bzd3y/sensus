using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Foundation;
using Sensus.Callbacks;
using Sensus.Context;
using Sensus.Probes;
using Sensus.Probes.Location;
using UserNotifications;

namespace Sensus.iOS.Callbacks
{
	public class iOSTimerCallbackScheduler : iOSCallbackScheduler
	{
		public override List<string> CallbackIds => throw new NotImplementedException();

		private NSTimer _backgroundTimer;

		public void StartRunningInBackground()
		{
			List<Protocol> protocols = SensusServiceHelper.Get().GetRunningProtocols();

			// check to see if we have gps running
			if (protocols.SelectMany(x => x.Probes).OfType<ListeningLocationProbe>().Any(x => x.Enabled) && (SensusContext.Current.CallbackScheduler is iOSCallbackScheduler scheduler))
			{
				List<string> ids = new List<string>();

				// if we have gps running we can cancel push notification requests for the polling probes
				UNUserNotificationCenter.Current.GetPendingNotificationRequests(requests =>
				{
					foreach (UNNotificationRequest request in requests)
					{
						if (TryGetCallback(request.Content?.UserInfo)?.Silent ?? false)
						{
							ids.Add(request.Identifier);
						}
					}
				});

				UNUserNotificationCenter.Current.RemoveDeliveredNotifications(ids.ToArray());
				UNUserNotificationCenter.Current.RemovePendingNotificationRequests(ids.ToArray());

				// get the shortest poll time out of all of the polling probes
				int pollingDuration = protocols.SelectMany(x => x.Probes).OfType<PollingProbe>().Where(x => x.Enabled).Min(x => x.PollingSleepDurationMS);

				// start a timer to run the callbacks like they would normally be run when the app is activated. Some will run off of schedule and be late by the pollingDuration above.
				_backgroundTimer = NSTimer.CreateRepeatingScheduledTimer(TimeSpan.FromMilliseconds(pollingDuration), async t =>
				{
					await SensusContext.Current.MainThreadSynchronizer.ExecuteThreadSafe(async () =>
					{
						SensusServiceHelper.Get().Logger.Log("Firing callbacks in the background.", LoggingLevel.Normal, GetType());

						// update/run all callbacks
						await scheduler.UpdateCallbacksOnActivationAsync();
					});
				});
			}
		}
		public void StopRunningInBackground()
		{
			if (_backgroundTimer != null)
			{
				_backgroundTimer.Invalidate();
				_backgroundTimer = null;
			}
		}

		public override void CancelSilentNotifications()
		{
			throw new NotImplementedException();
		}

		protected override void CancelLocalInvocation(ScheduledCallback callback)
		{
			throw new NotImplementedException();
		}

		protected override Task ReissueSilentNotificationAsync(string id)
		{
			throw new NotImplementedException();
		}

		protected override Task RequestLocalInvocationAsync(ScheduledCallback callback)
		{
			throw new NotImplementedException();
		}
	}
}
