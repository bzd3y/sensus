using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BackgroundTasks;
using CoreFoundation;
using Foundation;
using Sensus.Callbacks;
using Sensus.Context;
using Sensus.iOS.Notifications.UNUserNotifications;
using Sensus.Probes;
using Sensus.Probes.User.Scripts;
using UIKit;
using UserNotifications;

namespace Sensus.iOS.Callbacks
{
	public class iOSTimerCallbackScheduler : iOSCallbackScheduler
	{
		private Dictionary<string, NSTimer> _timers;
		private DispatchQueue _dispatchQueue;

		public const string BACKGROUND_TASK_IDENTIFIER = "edu.virginia.sie.ptl.sensus.iostimercallbackscheduler";

		public iOSTimerCallbackScheduler()
		{
			_timers = new Dictionary<string, NSTimer>();
			_dispatchQueue = new DispatchQueue(BACKGROUND_TASK_IDENTIFIER, true);
			
			if (UIDevice.CurrentDevice.CheckSystemVersion(13, 0))
			{
				if (BGTaskScheduler.Shared.Register(BACKGROUND_TASK_IDENTIFIER, _dispatchQueue, RaiseCallbacksInBackgroundAsync) == false)
				{
					SensusServiceHelper.Get().Logger.Log("Could not register the background process.", LoggingLevel.Normal, GetType());
				}
			}
		}

		public override List<string> CallbackIds
		{
			get
			{
				List<string> callbackIds;

				lock (_timers)
				{
					callbackIds = _timers.Keys.ToList();
				}

				return callbackIds;
			}
		}

		protected override void CancelLocalInvocation(ScheduledCallback callback)
		{
			SensusContext.Current.MainThreadSynchronizer.ExecuteThreadSafe(() =>
			{
				lock (_timers)
				{
					if (_timers.TryGetValue(callback.Id, out NSTimer timer))
					{
						timer.Invalidate();

						_timers.Remove(callback.Id);
					}
				}
			});
		}

		protected override async Task RequestLocalInvocationAsync(ScheduledCallback callback)
		{
			if (callback.NextExecution != null)
			{
				if (_timers.TryGetValue(callback.Id, out NSTimer existingTimer) == false || existingTimer.TimeInterval != callback.RepeatDelay?.TotalSeconds)
				{
					CancelLocalInvocation(callback);

					SensusContext.Current.MainThreadSynchronizer.ExecuteThreadSafe(() =>
					{
						NSTimer timer = new NSTimer((NSDate)callback.NextExecution.Value, callback.RepeatDelay ?? TimeSpan.Zero, async t =>
						{
							await RaiseCallbackAsync(callback, callback.InvocationId);
						}, callback.RepeatDelay.HasValue);
						/*{
							Tolerance = callback.RepeatDelay.Value.TotalMilliseconds * .10
						};*/

						NSRunLoop.Main.AddTimer(timer, NSRunLoopMode.Default);

						lock (_timers)
						{
							_timers[callback.Id] = timer;
						}
					});
				}
			}

			await Task.CompletedTask;
		}

		public async Task RequestNotificationsAsync(bool gpsIsRunning)
		{
			UNUserNotificationNotifier notifier = SensusContext.Current.Notifier as UNUserNotificationNotifier;
			DateTime earliestExecutionTime = DateTime.MaxValue;

			foreach (string id in CallbackIds)
			{
				if (TryGetCallback(id) is ScheduledCallback callback)
				{
					using (NSMutableDictionary callbackInfo = GetCallbackInfo(callback))
					{
						if (callbackInfo != null)
						{
							if (callback.Silent == false)
							{
								await notifier.IssueNotificationAsync(callback.Protocol?.Name ?? "Alert", callback.UserNotificationMessage, callback.Id, true, callback.Protocol, null, callback.NotificationUserResponseAction, callback.NotificationUserResponseMessage, callback.NextExecution.Value, callbackInfo);
							}

							await RequestRemoteInvocationAsync(callback);

							if (callback.NextExecution < earliestExecutionTime)
							{
								earliestExecutionTime = callback.NextExecution.Value;
							}
						}
					}
				}
			}

			ScheduleBackgroundProcess(earliestExecutionTime);
		}

		protected void ScheduleBackgroundProcess(DateTime earliestExecutionTime)
		{
			if (UIDevice.CurrentDevice.CheckSystemVersion(13, 0))
			{
				BGProcessingTaskRequest request = new BGProcessingTaskRequest(BACKGROUND_TASK_IDENTIFIER)
				{
					EarliestBeginDate = (NSDate)earliestExecutionTime,
					// may need to allow these to be configurable?
					RequiresExternalPower = false,
					RequiresNetworkConnectivity = false
				};

				if (BGTaskScheduler.Shared.Submit(request, out NSError error) == false)
				{
					SensusServiceHelper.Get().Logger.Log(error.LocalizedDescription, LoggingLevel.Normal, GetType());

					error.Dispose();
				}
			}
		}

		public void CancelBackgroundProcess()
		{
			BGTaskScheduler.Shared.Cancel(BACKGROUND_TASK_IDENTIFIER);
		}

		protected async void RaiseCallbacksInBackgroundAsync(BGTask task)
		{
			SensusServiceHelper.Get().Logger.Log("Starting to execute callbacks in the background.", LoggingLevel.Normal, GetType());

			DateTime? earliestExecutionTime = DateTime.MaxValue;

			// raise callbacks and update
			await UpdateCallbacksOnActivationAsync();

			// get the earliest next execution time
			earliestExecutionTime = CallbackIds
				.Select(x => TryGetCallback(x))
				.Where(x => x != null)
				.Min(x => x.NextExecution);

			//IEnumerable<ScheduledCallback> callbacks = CallbackIds
			//	.Select(x => TryGetCallback(x))
			//	.Where(x => x != null)
			//	.OrderBy(x => x.NextExecution);

			//foreach (ScheduledCallback callback in callbacks)
			//{
			//	if (callback.NextExecution >= DateTime.Now)
			//	{
			//		await RaiseCallbackAsync(callback, callback.InvocationId);
			//	}

			//	if (callback.NextExecution < earliestExecutionTime)
			//	{
			//		earliestExecutionTime = callback.NextExecution.Value;
			//	}
			//}

			if (earliestExecutionTime != null)
			{
				ScheduleBackgroundProcess(earliestExecutionTime.Value);
			}

			task.SetTaskCompleted(true);

			SensusServiceHelper.Get().Logger.Log("Finished executing callbacks in the background.", LoggingLevel.Normal, GetType());
		}

		public async Task CancelNotificationsAsync()
		{
			UNUserNotificationNotifier notifier = SensusContext.Current.Notifier as UNUserNotificationNotifier;

			foreach (string id in CallbackIds)
			{
				if (TryGetCallback(id) is ScheduledCallback callback)
				{
					notifier.CancelNotification(callback.Id);

					await CancelRemoteInvocationAsync(callback);
				}
			}
		}

		protected override Task ReissueSilentNotificationAsync(string id)
		{
			throw new NotImplementedException();
		}

		public override void CancelSilentNotifications()
		{
			throw new NotImplementedException();
		}
	}
}
