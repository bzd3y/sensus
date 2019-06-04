using Android.App;
using Android.App.Job;
using Android.Content;
using System.Linq;
using Java.Lang;
using Android.Provider;
using Android.Net;
using Android.Database;
using Android.OS;

namespace Sensus.Android.Probes.Apps
{
	[Service(Name = "com.sensus.android.AndroidImageJobService", Permission = "android.permission.BIND_JOB_SERVICE")]
	public class AndroidImageJobService : JobService
	{
		private static JobInfo _jobInfo;
		private readonly static int ExternalStoragePathSize;

		static AndroidImageJobService()
		{
			JobScheduler scheduler = (JobScheduler)Application.Context.GetSystemService(Class.FromType(typeof(JobScheduler)));
			int jobId = 0;

			if (scheduler.AllPendingJobs.Any())
			{
				jobId = scheduler.AllPendingJobs.Max(x => x.Id) + 1;
			}

			PersistableBundle bundle = new PersistableBundle();

			JobInfo.Builder builder = new JobInfo.Builder(jobId, new ComponentName(Application.Context, "com.sensus.android.AndroidImageJobService" /*Class.FromType(typeof(AndroidImageJobService))*/));

			builder.AddTriggerContentUri(new JobInfo.TriggerContentUri(MediaStore.Images.Media.ExternalContentUri, TriggerContentUriFlags.NotifyForDescendants));
			//builder.AddTriggerContentUri(new JobInfo.TriggerContentUri(Uri.Parse($"{ContentResolver.SchemeContent}://{MediaStore.Authority}/"), TriggerContentUriFlags.NotifyForDescendants));

			_jobInfo = builder.Build();

			ExternalStoragePathSize = MediaStore.Images.Media.ExternalContentUri.PathSegments.Count;
		}

		public static void Schedule(AndroidImageMetadataProbe probe)
		{
			JobScheduler scheduler = (JobScheduler)Application.Context.GetSystemService(Class.FromType(typeof(JobScheduler)));

			if (scheduler.Schedule(_jobInfo) == JobScheduler.ResultFailure)
			{
				throw new System.Exception("Failed to schedule Gallery Image Metadata job");
			}
		}

		public static void Unschedule()
		{
			JobScheduler scheduler = (JobScheduler)Application.Context.GetSystemService(Class.FromType(typeof(JobScheduler)));

			scheduler.Cancel(_jobInfo.Id);
		}

		public override bool OnStartJob(JobParameters @params)
		{
			Schedule(null);

			Uri[] uris = @params.GetTriggeredContentUris()?.Where(x => x.PathSegments.Count > ExternalStoragePathSize).ToArray();

			if (uris != null && uris.Any())
			{
				string inList = string.Join(", ", Enumerable.Repeat("?", uris.Length));

				ICursor cursor = Application.Context.ContentResolver.Query(MediaStore.Images.Media.ExternalContentUri, new string[] { MediaStore.Images.Media.InterfaceConsts.Data, MediaStore.Images.Media.InterfaceConsts.DateTaken }, MediaStore.Images.Media.InterfaceConsts.Id + $" IN ({inList})", uris.Select(x => x.LastPathSegment).ToArray(), MediaStore.Images.Media.InterfaceConsts.DateTaken + " DESC LIMIT 1");

				while (cursor.MoveToNext())
				{
					//_probe.CreateAndStoreDatum()
				}
			}

			return true;
		}

		public override bool OnStopJob(JobParameters @params)
		{
			return false;
		}
	}
}
