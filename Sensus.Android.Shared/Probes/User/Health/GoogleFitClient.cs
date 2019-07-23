using Android.Gms.Common.Apis;
using Android.Gms.Auth.Api;
using System.Threading.Tasks;

namespace Sensus.Android.Probes.User.Health
{
	public class GoogleFitClient
	{
		const int OAUTH_REQUEST_CODE = 1;
		protected GoogleApiClient _client;
		protected bool _authorizing;

		public async Task SignInAsync()
		{
			IResult result = await Auth.GoogleSignInApi.SilentSignIn(_client);

			if (result.Status == Statuses.ResultSuccess)
			{
				// sign in manually if silent sign in failed...
			}
		}

		public void Connect()
		{
			//_client = new GoogleApiClient.Builder(Application.Context)
			//	.AddApi(FitnessClass.SENSORS_API)
			//	.AddConnectionCallbacks(b =>
			//	{
			//		//SensusServiceHelper.Get().Logger.Log(r.ErrorMessage, LoggingLevel.Debug, GetType());
			//	}, s =>
			//	{
			//		//SensusServiceHelper.Get().Logger.Log(r.ErrorMessage, LoggingLevel.Debug, GetType());
			//	})
			//	.AddOnConnectionFailedListener(r =>
			//	{

			//		if (!r.HasResolution)
			//		{
			//			SensusServiceHelper.Get().Logger.Log(r.ErrorMessage, LoggingLevel.Normal, GetType());

			//			throw new IntentSender.SendIntentException();
			//		}
			//		// The failure has a resolution. Resolve it.
			//		// Called typically when the app is not yet authorized, and an
			//		// authorization dialog is displayed to the user.
			//		if (_authorizing == false)
			//		{
			//			try
			//			{
			//				_authorizing = true;

			//				r.StartResolutionForResult(, OAUTH_REQUEST_CODE);
			//			}
			//			catch (IntentSender.SendIntentException e)
			//			{
			//				SensusServiceHelper.Get().Logger.Log(e.Message, LoggingLevel.Normal, GetType());
			//			}
			//		}
			//	})
			//	.Build();

			//_client.Connect();
		}

		public void Disconnect()
		{

		}
	}
}
