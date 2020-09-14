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
using Newtonsoft.Json;
using System.Linq;
using System;
using Java.Util.Concurrent;
using Android.Gms.Fitness.Result;

namespace Sensus.Android.Probes.User.Health
{
	public class AndroidHealthDataProbe : HealthDataProbe
	{
		private GoogleApiClient _client;

		public AndroidHealthDataProbe()
		{

		}

		//#region Health data collector classes...
		//public abstract class HealthDataCollector
		//{
		//	public HealthDataCollector()
		//	{

		//	}

		//	public HealthDataCollector(GoogleApiClient client)
		//	{
		//		Client = client;
		//	}

		//	[JsonIgnore]
		//	public GoogleApiClient Client { get; set; }
		//	[JsonIgnore]
		//	public string Key { get; protected set; }

		//	public abstract Task<List<HealthDatum>> GetDataAsync();
		//}

		//public class SampleCollector : HealthDataCollector
		//{
		//	public SampleCollector()
		//	{

		//	}

		//	public SampleCollector(GoogleApiClient client) : base(client)
		//	{

		//	}

		//	public override Task<List<HealthDatum>> GetDataAsync()
		//	{
		//		TaskCompletionSource<List<HealthDatum>> completionSource = new TaskCompletionSource<List<HealthDatum>>();

		//		return completionSource.Task;
		//	}
		//}

		////public abstract class CharacteristicCollector : HealthDataCollector
		////{
		////	private HKCharacteristicTypeIdentifier _characteristicType;

		////	public HKCharacteristicTypeIdentifier CharacteristicType
		////	{
		////		get
		////		{
		////			return _characteristicType;
		////		}
		////		set
		////		{
		////			_characteristicType = value;

		////			ObjectType = HKCharacteristicType.Create(value);
		////			Key = value.ToString();
		////		}
		////	}

		////	public string LastValue { get; set; }

		////	protected List<HealthDatum> CreateData(string currentValue)
		////	{
		////		List<HealthDatum> data = new List<HealthDatum>();

		////		if (currentValue != LastValue)
		////		{
		////			HealthDatum datum = new HealthDatum(CharacteristicType.ToString(), currentValue, null, null, DateTime.Now, DateTime.Now, DateTimeOffset.UtcNow);

		////			LastValue = currentValue;

		////			data.Add(datum);
		////		}
		////		else
		////		{
		////			data.Add(null);
		////		}

		////		return data;
		////	}
		////	protected bool CanCreateData(NSError error)
		////	{
		////		if (error != null)
		////		{
		////			SensusServiceHelper.Get().Logger.Log($"Failed to collect {CharacteristicType}: " + error.Description, LoggingLevel.Normal, GetType());

		////			return false;
		////		}

		////		return true;
		////	}

		////	public CharacteristicCollector()
		////	{

		////	}

		////	public CharacteristicCollector(HKHealthStore healthStore, HKCharacteristicTypeIdentifier characteristicType) : base(healthStore)
		////	{
		////		CharacteristicType = characteristicType;
		////	}
		////}

		////public class BiologicalSexCollector : CharacteristicCollector
		////{
		////	public BiologicalSexCollector()
		////	{

		////	}

		////	public BiologicalSexCollector(HKHealthStore healthStore) : base(healthStore, HKCharacteristicTypeIdentifier.BiologicalSex)
		////	{

		////	}

		////	public override Task<List<HealthDatum>> GetDataAsync()
		////	{
		////		HKBiologicalSexObject biologicalSex = HealthStore.GetBiologicalSex(out NSError error);

		////		if (CanCreateData(error))
		////		{
		////			string currentValue = biologicalSex.BiologicalSex.ToString();

		////			return Task.FromResult(CreateData(currentValue));
		////		}

		////		return Task.FromResult(new List<HealthDatum> { null });
		////	}
		////}

		////public class BirthdateCollector : CharacteristicCollector
		////{
		////	public BirthdateCollector()
		////	{

		////	}

		////	public BirthdateCollector(HKHealthStore healthStore) : base(healthStore, HKCharacteristicTypeIdentifier.DateOfBirth)
		////	{

		////	}

		////	public override Task<List<HealthDatum>> GetDataAsync()
		////	{
		////		NSDateComponents birthdate = HealthStore.GetDateOfBirthComponents(out NSError error);

		////		if (birthdate != null && CanCreateData(error))
		////		{
		////			string currentValue = NSIso8601DateFormatter.Format(birthdate.Date, birthdate.TimeZone, NSIso8601DateFormatOptions.FullDate);

		////			return Task.FromResult(CreateData(currentValue));
		////		}

		////		return Task.FromResult(new List<HealthDatum> { null });
		////	}
		////}

		////public class BloodTypeCollector : CharacteristicCollector
		////{
		////	public BloodTypeCollector()
		////	{

		////	}

		////	public BloodTypeCollector(HKHealthStore healthStore) : base(healthStore, HKCharacteristicTypeIdentifier.BloodType)
		////	{

		////	}

		////	public override Task<List<HealthDatum>> GetDataAsync()
		////	{
		////		HKBloodTypeObject bloodType = HealthStore.GetBloodType(out NSError error);

		////		if (CanCreateData(error))
		////		{
		////			string currentValue = bloodType.BloodType.ToString();

		////			return Task.FromResult(CreateData(currentValue));
		////		}

		////		return Task.FromResult(new List<HealthDatum> { null });
		////	}
		////}

		////public class FitzpatrickSkinTypeCollector : CharacteristicCollector
		////{
		////	public FitzpatrickSkinTypeCollector()
		////	{

		////	}

		////	public FitzpatrickSkinTypeCollector(HKHealthStore healthStore) : base(healthStore, HKCharacteristicTypeIdentifier.FitzpatrickSkinType)
		////	{

		////	}

		////	public override Task<List<HealthDatum>> GetDataAsync()
		////	{
		////		HKFitzpatrickSkinTypeObject skinType = HealthStore.GetFitzpatrickSkinType(out NSError error);

		////		if (CanCreateData(error))
		////		{
		////			string currentValue = skinType.SkinType.ToString();

		////			return Task.FromResult(CreateData(currentValue));
		////		}

		////		return Task.FromResult(new List<HealthDatum> { null });
		////	}
		////}

		////public class WheelchairUseCollector : CharacteristicCollector
		////{
		////	public WheelchairUseCollector()
		////	{

		////	}

		////	public WheelchairUseCollector(HKHealthStore healthStore) : base(healthStore, HKCharacteristicTypeIdentifier.WheelchairUse)
		////	{

		////	}

		////	public override Task<List<HealthDatum>> GetDataAsync()
		////	{
		////		HKWheelchairUseObject wheelchairUse = HealthStore.GetWheelchairUse(out NSError error);

		////		if (CanCreateData(error))
		////		{
		////			string currentValue = wheelchairUse.WheelchairUse.ToString();

		////			return Task.FromResult(CreateData(currentValue));
		////		}

		////		return Task.FromResult(new List<HealthDatum> { null });
		////	}
		////}
		//#endregion

		//private Dictionary<string, DataType> _collectors;
		//public List<HealthDataCollector> Collectors { get; set; }

		//private Dictionary<string, DataType> _dataTypes;
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

		private bool _authorizationInProgress;

		private void InitializeDataType(bool enabled, DataType dataType)
		{
			if (enabled)
			{
				DataTypes.Add(dataType);
			}
			else if (enabled)
			{
				DataTypes.Remove(dataType);
			}
		}

		private void InitializeDataTypes()
		{
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

		protected override async Task InitializeAsync()
		{
			await base.InitializeAsync();

			_client = new GoogleApiClient.Builder(Application.Context)
				.AddApi(FitnessClass.HISTORY_API)
				.AddOnConnectionFailedListener(x =>
				{
					if (x.HasResolution)
					{
						if (_authorizationInProgress == false)
						{
							_authorizationInProgress = true;

							x.StartResolutionForResult(CrossCurrentActivity.Current.Activity, 1);
						}
					}
					else
					{
						SensusServiceHelper.Get().Logger.Log("Could not connect to Google Fitness API", LoggingLevel.Normal, GetType());
					}
				})
				.Build();

			_client.Connect();

			InitializeDataTypes();
		}

		protected async override Task<List<Datum>> PollAsync(CancellationToken cancellationToken)
		{
			List<Datum> data = new List<Datum>();

			if (_client.IsConnected == false)
			{
				_client.Connect();
			}

			long currentTime = Java.Lang.JavaSystem.CurrentTimeMillis();

			DataReadRequest.Builder builder = new DataReadRequest.Builder()
				.SetTimeRange(LastCollectionTime, currentTime, TimeUnit.Milliseconds);

			foreach (DataType dataType in DataTypes)
			{
				builder = builder.Read(dataType);
			}

			DataReadRequest request = builder.Build();

			DataReadResult result = await FitnessClass.HistoryApi.ReadDataAsync(_client, request);

			LastCollectionTime = currentTime;

			return data;
		}
	}
}
