using Sensus.Probes.User.Health;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Android.Gms.Common.Apis;
using Android.Gms.Fitness;
using Android.Gms.Fitness.Request;
using Android.Gms.Fitness.Data;
using Android.App;
using Sensus.UI.UiProperties;
using Plugin.CurrentActivity;
using System;
using Java.Util.Concurrent;
using Android.Gms.Fitness.Result;
using Android.Gms.Common;
using Sensus.Extensions;
using Android.OS;
using Sensus.Exceptions;
using Xamarin.Essentials;
using Sensus.Context;

namespace Sensus.Android.Probes.User.Health
{
	public class AndroidHealthDataProbe : HealthDataProbe
	{
		private GoogleApiClient _client;
		private bool _isAuthorizing;
		private ManualResetEventSlim _connectEvent;

		public AndroidHealthDataProbe()
		{
			DataTypes = new HashSet<DataType>();

			_connectEvent = new ManualResetEventSlim();
		}

		public HashSet<DataType> DataTypes { get; set; }

		public long LastCollectionTime { get; set; }

		//[OnOffUiProperty("Collect Biological Sex:", true, 60)]
		//public bool CollectBiologicalSex { get; set; } // Does not use Google Fit
		//[OnOffUiProperty("Collect Birthdate:", true, 61)]
		//public bool CollectBirthdate { get; set; } // Does not use Google Fit
		//[OnOffUiProperty("Collect Blood Type:", true, 62)]
		//public bool CollectBloodType { get; set; } // unavailable/not in Google Fit
		//[OnOffUiProperty("Collect Body Mass Index:", true, 63)]
		//public bool CollectBodyMassIndex { get; set; } // can be calculated
		//[OnOffUiProperty("Collect Distance Walking/Ranning:", true, 64)]
		//public bool CollectDistanceWalkingRunning { get; set; }
		//[OnOffUiProperty("Collect Fitzpatrick Skin Type:", true, 65)]
		//public bool CollectFitzpatrickSkinType { get; set; } // unavailable/not in Google Fit
		//[OnOffUiProperty("Collect Flights Climbed:", true, 66)]
		//public bool CollectFlightsClimbed { get; set; }
		[OnOffUiProperty("Collect Heart Rate:", true, 67)]
		public bool CollectHeartRate { get; set; }
		[OnOffUiProperty("Collect Height:", true, 68)]
		public bool CollectHeight { get; set; }
		//[OnOffUiProperty("Collect Number Of Times Fallen:", true, 69)]
		//public bool CollectNumberOfTimesFallen { get; set; }
		//[OnOffUiProperty("Collect Step Count:", true, 70)]
		//public bool CollectStepCount { get; set; }
		[OnOffUiProperty("Collect Weight:", true, 71)]
		public bool CollectWeight { get; set; }

		//[OnOffUiProperty("Collect Wheelchair Use:", true, 72)]
		//public bool CollectWheelchairUse { get; set; }

		private void InitializeDataType(bool enabled, DataType dataType)
		{
			if (enabled && DataTypes.Contains(dataType) == false)
			{
				DataTypes.Add(dataType);
			}
			else if (enabled == false && DataTypes.Contains(dataType))
			{
				DataTypes.Remove(dataType);
			}
		}

		private void InitializeDataTypes()
		{
			DataTypes = new HashSet<DataType>();
			//_dataTypes = DataTypes.ToDictionary(x => x.Name);

			//InitializeCollector(CollectBiologicalSex, new SampleCollector(_client));
			//InitializeCollector(CollectBirthdate, new SampleCollector(_client));
			//InitializeCollector(CollectBloodType, new SampleCollector(_client));
			//InitializeCollector(CollectBodyMassIndex, new SampleCollector(_client));
			//InitializeCollector(CollectDistanceWalkingRunning, new SampleCollector(_client));
			//InitializeCollector(CollectFitzpatrickSkinType, new SampleCollector(_client));
			//InitializeCollector(CollectFlightsClimbed, new SampleCollector(_client));
			InitializeDataType(CollectHeartRate, DataType.TypeHeartRateBpm);
			InitializeDataType(CollectHeight, DataType.TypeHeight);
			//InitializeCollector(CollectNumberOfTimesFallen, new SampleCollector(_client));
			//InitializeCollector(CollectStepCount, new SampleCollector(_client));
			InitializeDataType(CollectWeight, DataType.TypeWeight);
			//InitializeCollector(CollectWheelchairUse, new SampleCollector(_client));

			//Collectors = _dataTypes.Values.ToList();
		}

		public void OnConnected(Bundle connectionHint)
		{
			_isAuthorizing = false;

			_connectEvent.Set();

			SensusServiceHelper.Get().Logger.Log("Connected to Google Fitness API", LoggingLevel.Normal, GetType());
		}

		public void OnConnectionSuspended(int cause)
		{
			_isAuthorizing = false;

			SensusServiceHelper.Get().Logger.Log("Connection to Google Fitness API suspended", LoggingLevel.Normal, GetType());
		}

		public void OnConnectionFailed(ConnectionResult result)
		{
			SensusContext.Current.MainThreadSynchronizer.ExecuteThreadSafe(() =>
			{
				if (result.HasResolution)
				{
					if (_isAuthorizing == false)
					{
						_isAuthorizing = true;

						SensusServiceHelper.Get().Logger.Log("Connecting to Google Fitness API...", LoggingLevel.Normal, GetType());

						result.StartResolutionForResult(CrossCurrentActivity.Current.Activity, (int)AndroidActivityResultRequestCode.OauthRequest);
					}
				}
				else
				{
					_isAuthorizing = false;

					GoogleApiAvailability.Instance.GetErrorDialog(CrossCurrentActivity.Current.Activity, result.ErrorCode, (int)AndroidActivityResultRequestCode.OauthRequest).Show();

					SensusServiceHelper.Get().Logger.Log("Could not connect to Google Fitness API", LoggingLevel.Normal, GetType());
				}
			});
		}

		protected override async Task InitializeAsync()
		{
			await base.InitializeAsync();

			SensusContext.Current.MainThreadSynchronizer.ExecuteThreadSafe(() =>
			{
				_client = new GoogleApiClient.Builder(Application.Context)
					.UseDefaultAccount()
					.AddApi(FitnessClass.HISTORY_API)
					.AddScope(FitnessClass.ScopeBodyRead)
					.AddScope(FitnessClass.ScopeActivityRead)
					.AddConnectionCallbacks(OnConnected, OnConnectionSuspended)
					.AddOnConnectionFailedListener(OnConnectionFailed)
					.Build();

				AndroidMainActivity.OAUTH_RESULT_RECEIVED += (sender, success) =>
				{
					if (success)
					{
						if (_client.IsConnecting == false && _client.IsConnected == false)
						{
							_client.Connect();
						}
					}
					else
					{
						_connectEvent.Set();
					}
				};

				_client.Connect();
			});

			_connectEvent.Wait();

			//if (_client.IsConnected == false)
			//{
			//	//SensusServiceHelper.Get().Logger.Log(, LoggingLevel.Normal, GetType());

			//	throw SensusException.Report("Unable to connect to Google Fitness API...");
			//}

			InitializeDataTypes();
		}

		protected async override Task<List<Datum>> PollAsync(CancellationToken cancellationToken)
		{
			List<Datum> data = new List<Datum>();

			if (_client.IsConnected)
			{
				long currentTime = Java.Lang.JavaSystem.CurrentTimeMillis();

				if (LastCollectionTime == 0)
				{
					LastCollectionTime = currentTime - PollingSleepDurationMS;
				}

				LastCollectionTime = new DateTime(2020, 9, 1).ToJavaCurrentTimeMillis();

				DataReadRequest.Builder builder = new DataReadRequest.Builder()
					//.EnableServerQueries()
					.SetTimeRange(LastCollectionTime, currentTime, TimeUnit.Milliseconds);

				foreach (DataType dataType in DataTypes)
				{
					builder = builder.Read(dataType);
				}

				try
				{
					DataReadRequest request = builder.Build();

					DataReadResult result = await FitnessClass.HistoryApi.ReadDataAsync(_client, request);

					foreach (DataSet dataSet in result.DataSets)
					{
						foreach (DataPoint dataPoint in dataSet.DataPoints)
						{
							//DateTime startTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddMilliseconds(dataPoint.GetStartTime);
							//DateTime endTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddMilliseconds(currentTime);

							//data.Add(new HealthDatum(dataPoint.DataType.Name, dataPoint.Start))
						}
					}

					LastCollectionTime = currentTime;
				}
				catch (Exception e)
				{

				}
			}

			return data;
		}

		protected override Task ProtectedStopAsync()
		{
			_client.Disconnect();

			return Task.CompletedTask;
		}
	}
}
