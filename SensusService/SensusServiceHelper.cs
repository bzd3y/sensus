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

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using Newtonsoft.Json;
using SensusService.Probes;
using SensusService.Probes.Location;
using SensusUI.UiProperties;
using Xamarin;
using Xamarin.Geolocation;
using System.Collections.ObjectModel;
using SensusUI;
using SensusUI.Inputs;
using Xamarin.Forms;
using SensusService.Exceptions;

namespace SensusService
{
    /// <summary>
    /// Provides platform-independent service functionality.
    /// </summary>
    public abstract class SensusServiceHelper : IDisposable
    {
        /// <summary>
        /// Encapsulates information needed to run a scheduled callback.
        /// </summary>
        private class ScheduledCallback
        {
            /// <summary>
            /// Action to invoke.
            /// </summary>
            /// <value>The action.</value>
            public Action<CancellationToken> Action { get; set; }

            /// <summary>
            /// Name of callback.
            /// </summary>
            /// <value>The name.</value>
            public string Name { get; set; }

            /// <summary>
            /// Source of cancellation tokens when Action is invoked.
            /// </summary>
            /// <value>The canceller.</value>
            public CancellationTokenSource Canceller { get; set; }

            /// <summary>
            /// Notification message that should be displayed to the user when the callback is invoked.
            /// </summary>
            /// <value>The user notification message.</value>
            public string UserNotificationMessage { get; set; }

            /// <summary>
            /// Initializes a new instance of the <see cref="SensusService.SensusServiceHelper+ScheduledCallback"/> class.
            /// </summary>
            /// <param name="action">Action.</param>
            /// <param name="name">Name.</param>
            /// <param name="canceller">Canceller.</param>
            /// <param name="userNotificationMessage">User notification message.</param>
            public ScheduledCallback(Action<CancellationToken> action, string name, CancellationTokenSource canceller, string userNotificationMessage)
            {
                Action = action;
                Name = name;
                Canceller = canceller;
                UserNotificationMessage = userNotificationMessage;
            }
        }

        #region static members

        private static readonly object PROMPT_FOR_INPUTS_LOCKER = new object();
        private static bool PROMPT_FOR_INPUTS_RUNNING = false;
        public const string SENSUS_CALLBACK_KEY = "SENSUS-CALLBACK";
        public const string SENSUS_CALLBACK_ID_KEY = "SENSUS-CALLBACK-ID";
        public const string SENSUS_CALLBACK_REPEATING_KEY = "SENSUS-CALLBACK-REPEATING";
        protected const string XAMARIN_INSIGHTS_APP_KEY = "";
        private static SensusServiceHelper SINGLETON;
        private const string ENCRYPTION_KEY = "";
        private static readonly string SHARE_DIRECTORY = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "share");
        private static readonly string LOG_PATH = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "sensus_log.txt");
        private static readonly string SERIALIZATION_PATH = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "sensus_service_helper.json");
        private static readonly JsonSerializerSettings JSON_SERIALIZATION_SETTINGS = new JsonSerializerSettings
        {
            PreserveReferencesHandling = PreserveReferencesHandling.Objects,
            ReferenceLoopHandling = ReferenceLoopHandling.Serialize,
            TypeNameHandling = TypeNameHandling.All,
            ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor,
        };

        private static byte[] EncryptionKeyBytes
        {
            get
            {
                byte[] encryptionKeyBytes = new byte[32];
                byte[] bytes = Encoding.UTF8.GetBytes(ENCRYPTION_KEY);
                Array.Copy(bytes, encryptionKeyBytes, Math.Min(bytes.Length, encryptionKeyBytes.Length));
                return encryptionKeyBytes;
            }
        }

        /// <summary>
        /// Initializes the sensus service helper. Must be called when app first starts, from the main / UI thread.
        /// </summary>
        /// <param name="createNew">Function for creating a new service helper, if one is needed.</param>
        public static void Initialize(Func<SensusServiceHelper> createNew)
        {
            if (SINGLETON != null)
            {
                SINGLETON.Logger.Log("Serivce helper already initialized. Nothing to do.", LoggingLevel.Normal, SINGLETON.GetType());
                return;
            }
            
            try
            {
                SINGLETON = JsonConvert.DeserializeObject<SensusServiceHelper>(Decrypt(ReadAllBytes(SERIALIZATION_PATH)), JSON_SERIALIZATION_SETTINGS);
                SINGLETON.Logger.Log("Deserialized service helper with " + SINGLETON.RegisteredProtocols.Count + " protocols.", LoggingLevel.Normal, SINGLETON.GetType());
            }
            catch (Exception deserializeException)
            {
                Console.Error.WriteLine("Failed to deserialize Sensus service helper:  " + deserializeException.Message + System.Environment.NewLine +
                    "Creating new Sensus service helper.");

                try
                {
                    SINGLETON = createNew();
                }
                catch (Exception singletonCreationException)
                {
                    #region crash app and report to insights
                    string error = "Failed to construct service helper:  " + singletonCreationException.Message + System.Environment.NewLine + singletonCreationException.StackTrace;
                    Console.Error.WriteLine(error);
                    Exception exceptionToReport = new Exception(error);

                    try
                    {
                        Insights.Report(exceptionToReport, Xamarin.Insights.Severity.Error);
                    }
                    catch (Exception insightsReportException)
                    {
                        Console.Error.WriteLine("Failed to report exception to Xamarin Insights:  " + insightsReportException.Message);
                    }

                    throw exceptionToReport;
                    #endregion
                }

                #region save helper
                try
                {
                    SINGLETON.SaveAsync();
                }
                catch (Exception singletonSaveException)
                {
                    Console.Error.WriteLine("Failed to save new Sensus service helper:  " + singletonSaveException.Message);
                }
                #endregion
            }  
        }

        public static SensusServiceHelper Get()
        {
            if (SINGLETON == null)
                Console.Error.WriteLine("WARNING:  Tried to call SensusServiceHelper.Get() but SINGLETON was null. Stacktrace:  " + Environment.NewLine + Environment.StackTrace);
            
            return SINGLETON;
        }

        /// <summary>
        /// Reads all bytes from a file. There's a File.ReadAllBytes method in Android / iOS, but not in WinPhone.
        /// </summary>
        /// <returns>The bytes.</returns>
        /// <param name="path">Path.</param>
        public static byte[] ReadAllBytes(string path)
        {
            byte[] bytes = null;

            using (FileStream file = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                bytes = new byte[file.Length];
                int blockSize = 1024;
                byte[] block = new byte[blockSize];
                int bytesRead;
                int totalBytesRead = 0;
                while ((bytesRead = file.Read(block, 0, block.Length)) > 0)
                {
                    Array.Copy(block, 0, bytes, totalBytesRead, bytesRead);
                    totalBytesRead += bytesRead;
                }

                file.Close();
            }

            return bytes;
        }

        #region encryption

        public static byte[] Encrypt(string unencryptedString)
        {
            #if (__ANDROID__ || __IOS__)
            using (AesCryptoServiceProvider aes = new AesCryptoServiceProvider())
            {
                byte[] encryptionKeyBytes = EncryptionKeyBytes;
                aes.KeySize = encryptionKeyBytes.Length * 8;

                byte[] initialization = new byte[16];
                aes.BlockSize = initialization.Length * 8;

                using (ICryptoTransform transform = aes.CreateEncryptor(encryptionKeyBytes, initialization))
                {
                    byte[] unencrypted = Encoding.Unicode.GetBytes(unencryptedString);
                    return transform.TransformFinalBlock(unencrypted, 0, unencrypted.Length);
                }
            }
            #elif WINDOWS_PHONE
            return ProtectedData.Protect(Encoding.Unicode.GetBytes(unencryptedString), EncryptionKeyBytes);
            #else
            #error "Unrecognized platform."
            #endif
        }

        public static string Decrypt(byte[] encryptedBytes)
        {
            #if __ANDROID__ || __IOS__
            using (AesCryptoServiceProvider aes = new AesCryptoServiceProvider())
            {
                byte[] encryptionKeyBytes = EncryptionKeyBytes;
                aes.KeySize = encryptionKeyBytes.Length * 8;

                byte[] initialization = new byte[16];
                aes.BlockSize = initialization.Length * 8;

                using (ICryptoTransform transform = aes.CreateDecryptor(encryptionKeyBytes, initialization))
                {
                    return Encoding.Unicode.GetString(transform.TransformFinalBlock(encryptedBytes, 0, encryptedBytes.Length));
                }
            }
            #elif WINDOWS_PHONE
            byte[] unencryptedBytes = ProtectedData.Unprotect(encryptedBytes, EncryptionKeyBytes);
            return Encoding.Unicode.GetString(unencryptedBytes, 0, unencryptedBytes.Length);
            #else
            #error "Unrecognized platform."
            #endif
        }

        #endregion

        #endregion

        private bool _stopped;
        private Logger _logger;
        private ObservableCollection<Protocol> _registeredProtocols;
        private List<string> _runningProtocolIds;
        private string _healthTestCallbackId;
        private int _healthTestDelayMS;
        private int _healthTestCount;
        private int _healthTestsPerProtocolReport;
        private Dictionary<string, ScheduledCallback> _idCallback;
        private SHA256Managed _hasher;
        private List<PointOfInterest> _pointsOfInterest;

        private readonly object _locker = new object();

        [JsonIgnore]
        public Logger Logger
        {
            get { return _logger; }
        }

        public ObservableCollection<Protocol> RegisteredProtocols
        {
            get { return _registeredProtocols; }
        }

        public List<string> RunningProtocolIds
        {
            get{ return _runningProtocolIds; }
        }

        [EntryIntegerUiProperty("Health Test Delay (MS):", true, 9)]
        public int HealthTestDelayMS
        {
            get { return _healthTestDelayMS; }
            set
            {
                if (value <= 1000)
                    value = 1000;
                
                if (value != _healthTestDelayMS)
                {
                    _healthTestDelayMS = value;

                    if (_healthTestCallbackId != null)
                        _healthTestCallbackId = RescheduleRepeatingCallback(_healthTestCallbackId, _healthTestDelayMS, _healthTestDelayMS);

                    SaveAsync();
                }
            }
        }

        [EntryIntegerUiProperty("Health Tests Per Report:", true, 10)]
        public int HealthTestsPerProtocolReport
        {
            get { return _healthTestsPerProtocolReport; }
            set
            {
                if (value != _healthTestsPerProtocolReport)
                {
                    _healthTestsPerProtocolReport = value; 
                    SaveAsync();
                }
            }
        }

        [ListUiProperty("Logging Level:", true, 11, new object[] { LoggingLevel.Off, LoggingLevel.Normal, LoggingLevel.Verbose, LoggingLevel.Debug })]
        public LoggingLevel LoggingLevel
        {
            get { return _logger.Level; }
            set
            {
                if (value != _logger.Level)
                {
                    _logger.Level = value; 
                    SaveAsync();
                }
            }
        }

        public List<PointOfInterest> PointsOfInterest
        {
            get { return _pointsOfInterest; }
        }

        #region platform-specific properties

        [JsonIgnore]
        public abstract bool IsCharging { get; }

        [JsonIgnore]
        public abstract bool WiFiConnected { get; }

        [JsonIgnore]
        public abstract string DeviceId { get; }

        [JsonIgnore]
        public abstract string OperatingSystem { get; }

        protected abstract Geolocator Geolocator { get; }

        #endregion

        protected SensusServiceHelper()
        {
            if (SINGLETON != null)
                throw new SensusException("Attempted to construct new service helper when singleton already existed.");

            _stopped = true;
            _registeredProtocols = new ObservableCollection<Protocol>();
            _runningProtocolIds = new List<string>();
            _healthTestCallbackId = null;
            _healthTestDelayMS = 60000;
            _healthTestCount = 0;
            _healthTestsPerProtocolReport = 5;
            _idCallback = new Dictionary<string, ScheduledCallback>();
            _hasher = new SHA256Managed();
            _pointsOfInterest = new List<PointOfInterest>();

            if (!Directory.Exists(SHARE_DIRECTORY))
                Directory.CreateDirectory(SHARE_DIRECTORY); 

            #if DEBUG
            LoggingLevel loggingLevel = LoggingLevel.Debug;
            #elif RELEASE
            LoggingLevel loggingLevel = LoggingLevel.Normal;
            #else
            #error "Unrecognized compilation mode."
            #endif

            _logger = new Logger(LOG_PATH, loggingLevel, Console.Error);
            _logger.Log("Log file started at \"" + LOG_PATH + "\".", LoggingLevel.Normal, GetType());

            GpsReceiver.Get().Initialize(Geolocator);  // initialize GPS receiver with platform-specific geolocator

            if (Insights.IsInitialized)
                _logger.Log("Xamarin Insights is already initialized.", LoggingLevel.Normal, GetType());
            else
            {
                try
                {
                    _logger.Log("Initializing Xamarin Insights.", LoggingLevel.Normal, GetType());

                    // wait for startup crash to be logged -- https://insights.xamarin.com/docs
                    Insights.HasPendingCrashReport += (sender, isStartupCrash) =>
                    {
                        if (isStartupCrash)
                            Insights.PurgePendingCrashReports().Wait();
                    };

                    InitializeXamarinInsights();

                    Insights.Identify(DeviceId, "Device ID", DeviceId);
                }
                catch (Exception ex)
                {
                    _logger.Log("Failed to initialize Xamarin insights:  " + ex.Message, LoggingLevel.Normal, GetType());
                }
            }
        }

        public string GetHash(string s)
        {
            if (s == null)
                return null;
            
            StringBuilder hashBuilder = new StringBuilder();
            foreach (byte b in _hasher.ComputeHash(Encoding.UTF8.GetBytes(s)))
                hashBuilder.Append(b.ToString("x"));

            return hashBuilder.ToString();
        }

        #region platform-specific methods. this functionality cannot be implemented in a cross-platform way. it must be done separately for each platform.

        protected abstract void InitializeXamarinInsights();

        protected abstract void ScheduleRepeatingCallback(string callbackId, int initialDelayMS, int repeatDelayMS, string userNotificationMessage);

        protected abstract void ScheduleOneTimeCallback(string callbackId, int delayMS, string userNotificationMessage);

        protected abstract void UnscheduleCallback(string callbackId, bool repeating);

        public abstract void PromptForAndReadTextFileAsync(string promptTitle, Action<string> callback);

        public abstract void ShareFileAsync(string path, string subject);

        public abstract void TextToSpeechAsync(string text, Action callback);

        public abstract void RunVoicePromptAsync(string prompt, Action<string> callback);

        public abstract void IssueNotificationAsync(string message, string id);

        public abstract void FlashNotificationAsync(string message, Action callback);

        public abstract void KeepDeviceAwake();

        public abstract void LetDeviceSleep();

        public abstract void BringToForeground();

        public abstract void UpdateApplicationStatus(string status);

        /// <summary>
        /// The user can enable all probes at once. When this is done, it doesn't make sense to enable, e.g., the
        /// listening location probe as well as the polling location probe. This method allows the platforms to
        /// decide which probes to enable when enabling all probes.
        /// </summary>
        /// <returns><c>true</c>, if probe should be enabled, <c>false</c> otherwise.</returns>
        /// <param name="probe">Probe.</param>
        public abstract bool EnableProbeWhenEnablingAll(Probe probe);

        #endregion

        #region add/remove running protocol ids

        public void AddRunningProtocolId(string id)
        {
            lock (_locker)
            {
                if (!_runningProtocolIds.Contains(id))
                {
                    _runningProtocolIds.Add(id);
                    SaveAsync();
                }

                SensusServiceHelper.Get().UpdateApplicationStatus(_runningProtocolIds.Count + " protocol" + (_runningProtocolIds.Count == 1 ? " is " : "s are") + " running");

                if (_healthTestCallbackId == null)
                    _healthTestCallbackId = ScheduleRepeatingCallback(TestHealth, "Test Health", _healthTestDelayMS, _healthTestDelayMS);
            }
        }

        public void RemoveRunningProtocolId(string id)
        {
            lock (_locker)
            {
                if (_runningProtocolIds.Remove(id))
                    SaveAsync();

                SensusServiceHelper.Get().UpdateApplicationStatus(_runningProtocolIds.Count + " protocol" + (_runningProtocolIds.Count == 1 ? " is " : "s are") + " running");

                if (_runningProtocolIds.Count == 0)
                {
                    UnscheduleRepeatingCallback(_healthTestCallbackId);
                    _healthTestCallbackId = null;
                }
            }
        }

        public bool ProtocolShouldBeRunning(Protocol protocol)
        {
            return _runningProtocolIds.Contains(protocol.Id);
        }

        #endregion

        public void SaveAsync()
        {
            new Thread(() =>
                {
                    lock (_locker)
                    {
                        try
                        {
                            using (FileStream file = new FileStream(SERIALIZATION_PATH, FileMode.Create, FileAccess.Write))
                            {
                                byte[] encryptedBytes = Encrypt(JsonConvert.SerializeObject(this, JSON_SERIALIZATION_SETTINGS));
                                file.Write(encryptedBytes, 0, encryptedBytes.Length);
                                file.Close();
                            }
                        }
                        catch (Exception ex)
                        {
                            _logger.Log("Failed to serialize Sensus service helper:  " + ex.Message, LoggingLevel.Normal, GetType());
                        }
                    }

                }).Start();
        }

        public void StartAsync(Action callback)
        {
            new Thread(() =>
                {
                    Start();

                    if (callback != null)
                        callback();

                }).Start();
        }

        /// <summary>
        /// Starts platform-independent service functionality, including protocols that should be running. Okay to call multiple times, even if the service is already running.
        /// </summary>
        public void Start()
        {
            lock (_locker)
            {
                if (_stopped)
                    _stopped = false;
                else
                    return;

                foreach (Protocol protocol in _registeredProtocols)
                    if (!protocol.Running && _runningProtocolIds.Contains(protocol.Id))
                        protocol.Start();
            }
        }

        public void RegisterProtocol(Protocol protocol)
        {
            lock (_locker)
                if (!_stopped && !_registeredProtocols.Contains(protocol))
                {
                    _registeredProtocols.Add(protocol);
                    SaveAsync();
                }
        }

        #region callback scheduling

        public string ScheduleRepeatingCallback(Action<CancellationToken> callback, string name, int initialDelayMS, int repeatDelayMS)
        {
            return ScheduleRepeatingCallback(callback, name, initialDelayMS, repeatDelayMS, null);
        }

        public string ScheduleRepeatingCallback(Action<CancellationToken> callback, string name, int initialDelayMS, int repeatDelayMS, string userNotificationMessage)
        {
            lock (_idCallback)
            {
                string callbackId = AddCallback(callback, name, userNotificationMessage);
                ScheduleRepeatingCallback(callbackId, initialDelayMS, repeatDelayMS, userNotificationMessage);
                return callbackId;
            }
        }

        public string ScheduleOneTimeCallback(Action<CancellationToken> callback, string name, int delay)
        {
            return ScheduleOneTimeCallback(callback, name, delay, null);
        }

        public string ScheduleOneTimeCallback(Action<CancellationToken> callback, string name, int delay, string userNotificationMessage)
        {
            lock (_idCallback)
            {
                string callbackId = AddCallback(callback, name, userNotificationMessage);
                ScheduleOneTimeCallback(callbackId, delay, userNotificationMessage);
                return callbackId;
            }
        }

        private string AddCallback(Action<CancellationToken> callback, string name, string userNotificationMessage)
        {
            lock (_idCallback)
            {
                string callbackId = Guid.NewGuid().ToString();
                _idCallback.Add(callbackId, new ScheduledCallback(callback, name, new CancellationTokenSource(), userNotificationMessage));
                return callbackId;
            }
        }

        public bool CallbackIsScheduled(string callbackId)
        {
            lock (_idCallback)
                return _idCallback.ContainsKey(callbackId);
        }

        public string RescheduleRepeatingCallback(string callbackId, int initialDelayMS, int repeatDelayMS)
        {
            lock (_idCallback)
            {
                ScheduledCallback scheduledCallback;
                if (_idCallback.TryGetValue(callbackId, out scheduledCallback))
                {
                    UnscheduleRepeatingCallback(callbackId);
                    return ScheduleRepeatingCallback(scheduledCallback.Action, scheduledCallback.Name, initialDelayMS, repeatDelayMS, scheduledCallback.UserNotificationMessage);
                }
                else
                    return null;
            }
        }

        public void RaiseCallbackAsync(string callbackId, bool repeating, bool notifyUser)
        {
            RaiseCallbackAsync(callbackId, repeating, notifyUser, null);
        }

        public void RaiseCallbackAsync(string callbackId, bool repeating, bool notifyUser, Action callback)
        {         
            KeepDeviceAwake();  // call this before we start up the new thread, just in case the system decides to sleep before the thread is started.

            new Thread(() =>
                {
                    ScheduledCallback scheduledCallback;

                    lock (_idCallback)
                    {
                        // do we have callback information for the passed callbackId? we might not, in the case where the callback is canceled by the user and the system fires it subsequently.
                        if (!_idCallback.TryGetValue(callbackId, out scheduledCallback))
                            return;
                    }

                    // callback actions cannot be raised concurrently -- drop the current callback if it is already in progress
                    if (Monitor.TryEnter(scheduledCallback.Action))
                    {
                        try
                        {
                            if (scheduledCallback.Canceller.IsCancellationRequested)
                                _logger.Log("Callback \"" + scheduledCallback.Name + "\" (" + callbackId + ") was cancelled before it was started.", LoggingLevel.Debug, GetType());
                            else
                            {
                                _logger.Log("Raising callback \"" + scheduledCallback.Name + "\" (" + callbackId + ").", LoggingLevel.Debug, GetType());

                                if (notifyUser)
                                    IssueNotificationAsync(scheduledCallback.UserNotificationMessage, callbackId);

                                scheduledCallback.Action(scheduledCallback.Canceller.Token);
                            }
                        }
                        catch (Exception ex)
                        {
                            _logger.Log("Callback \"" + scheduledCallback.Name + "\" (" + callbackId + ") failed:  " + ex.Message, LoggingLevel.Normal, GetType());
                        }
                        finally
                        {
                            // if this is a repeating callback, then we'll need to reset the cancellation token source with a new instance, since they cannot be reused. if
                            // we enter the _idCallback lock before CancelRaisedCallback, then the next raise will be cancelled. if CancelRaisedCallback enters the 
                            // _idCallback lock first, then the cancellation token source will be overwritten here and the cancel will not have any effect. however,
                            // the latter case is a reasonable outcome, since the purpose of CancelRaisedCallback is to terminate any callbacks that are currently in 
                            // progress, and the current callback is no longer in progress. if the desired outcome is complete discontinuation of the repeating callback
                            // then UnscheduleRepeatingCallback should be used -- this method first cancels any raised callbacks and then removes the callback entirely.
                            try
                            {
                                if (repeating)
                                    lock (_idCallback)
                                        scheduledCallback.Canceller = new CancellationTokenSource();
                            }
                            catch (Exception)
                            {
                            }
                            finally
                            {
                                Monitor.Exit(scheduledCallback.Action);
                            }
                        }
                    }
                    else
                        _logger.Log("Callback \"" + scheduledCallback.Name + "\" (" + callbackId + ") is already running. Not running again.", LoggingLevel.Debug, GetType());
                    
                    // if this was a one-time callback, remove it from our collection. it is no longer needed.
                    if (!repeating)
                        lock (_idCallback)
                            _idCallback.Remove(callbackId);                               

                    if (callback != null)
                        callback();

                    LetDeviceSleep();

                }).Start();                           
        }

        /// <summary>
        /// Cancels a callback that has been raised and is currently executing.
        /// </summary>
        /// <param name="callbackId">Callback identifier.</param>
        public void CancelRaisedCallback(string callbackId)
        {
            lock (_idCallback)
            {
                ScheduledCallback scheduledCallback;
                if (_idCallback.TryGetValue(callbackId, out scheduledCallback))
                    scheduledCallback.Canceller.Cancel();
            }
        }

        public void UnscheduleOneTimeCallback(string callbackId)
        {
            if (callbackId != null)
                lock (_idCallback)
                {
                    SensusServiceHelper.Get().Logger.Log("Unscheduling one-time callback \"" + callbackId + "\".", LoggingLevel.Debug, GetType());

                    CancelRaisedCallback(callbackId);
                    _idCallback.Remove(callbackId);

                    UnscheduleCallback(callbackId, false);
                }
        }

        public void UnscheduleRepeatingCallback(string callbackId)
        {                                      
            if (callbackId != null)
                lock (_idCallback)
                {
                    SensusServiceHelper.Get().Logger.Log("Unscheduling repeating callback \"" + callbackId + "\".", LoggingLevel.Debug, GetType());

                    CancelRaisedCallback(callbackId);
                    _idCallback.Remove(callbackId);

                    UnscheduleCallback(callbackId, true);
                }
        }

        #endregion

        public void TextToSpeechAsync(string text)
        {
            TextToSpeechAsync(text, () =>
                {
                });                        
        }

        public void FlashNotificationAsync(string message)
        {
            FlashNotificationAsync(message, () =>
                {
                });
        }

        public void PromptForInputAsync(string windowTitle, Input input, Action<object> callback)
        {
            PromptForInputsAsync(windowTitle, new Input[] { input }, inputs =>
                {
                    if (inputs == null || inputs.Count == 0)
                        callback(null);
                    else
                        callback(inputs[0]);
                });
        }

        public void PromptForInputsAsync(string windowTitle, IEnumerable<Input> inputs, Action<List<object>> callback)
        {
            InputGroup inputGroup = new InputGroup(windowTitle);

            foreach (Input input in inputs)
                inputGroup.Inputs.Add(input);

            PromptForInputsAsync(null, false, DateTimeOffset.MinValue, new InputGroup[] { inputGroup }.ToList(), inputResponses =>
                {
                    if (inputResponses == null)
                        callback(null);
                    else
                        callback(inputResponses.Select(groupResponse => groupResponse.Item2).ToList());
                });
        }

        public void PromptForInputsAsync(Datum triggeringDatum, bool isReprompt, DateTimeOffset firstPromptTimestamp, IEnumerable<InputGroup> inputGroups, Action<List<Tuple<Input, object>>> callback)
        {
            new Thread(() =>
                {
                    // only one prompt can run at a time...enforce that here.
                    lock (PROMPT_FOR_INPUTS_LOCKER)
                    {
                        // calling while a previous call is in progress returns null
                        if (PROMPT_FOR_INPUTS_RUNNING)
                        {
                            callback(null);
                            return;
                        }
                        else
                            PROMPT_FOR_INPUTS_RUNNING = true;
                    }

                    List<Tuple<Input, object>> inputResponses = new List<Tuple<Input, object>>();

                    int groupNum = 0;
                    int incompleteGroups = inputGroups.Count(g => !g.Complete);

                    foreach (InputGroup inputGroup in inputGroups.Where(g => !g.Complete))
                    {
                        ++groupNum;

                        ManualResetEvent responseWait = new ManualResetEvent(false);

                        if (inputGroup.Inputs.Count == 1 && inputGroup.Inputs[0] is VoiceInput)
                        {
                            VoiceInput promptInput = inputGroup.Inputs[0] as VoiceInput;

                            promptInput.RunAsync(triggeringDatum, isReprompt, firstPromptTimestamp, response =>
                                {                
                                    inputResponses.Add(new Tuple<Input, object>(promptInput, response));
                                    responseWait.Set();
                                });
                        }
                        else
                        {
                            BringToForeground();

                            Device.BeginInvokeOnMainThread(async () =>
                                {
                                    await App.Current.MainPage.Navigation.PushAsync(new PromptForInputsPage(inputGroup, groupNum / (double)incompleteGroups, responses =>
                                            {
                                                if (responses != null)
                                                    inputResponses.AddRange(responses);

                                                responseWait.Set();
                                            }));
                                });
                        }

                        responseWait.WaitOne();
                    }

                    callback(inputResponses);

                    PROMPT_FOR_INPUTS_RUNNING = false;

                }).Start();
        }

        public void GetPositionsFromMapAsync(Xamarin.Forms.Maps.Position address, string newPinName, Action<List<Xamarin.Forms.Maps.Position>> callback)
        {
            Device.BeginInvokeOnMainThread(async () =>
                {
                    MapPage mapPage = new MapPage(address, newPinName);

                    mapPage.Disappearing += (o, e) =>
                    {
                        callback(mapPage.Pins.Select(pin => pin.Position).ToList());
                    };

                    await App.Current.MainPage.Navigation.PushAsync(mapPage);
                });
        }

        public void GetPositionsFromMapAsync(string address, string newPinName, Action<List<Xamarin.Forms.Maps.Position>> callback)
        {
            Device.BeginInvokeOnMainThread(async () =>
                {
                    MapPage mapPage = new MapPage(address, newPinName);

                    mapPage.Disappearing += (o, e) =>
                    {
                        callback(mapPage.Pins.Select(pin => pin.Position).ToList());
                    };

                    await App.Current.MainPage.Navigation.PushAsync(mapPage);
                });
        }

        public void TestHealth(CancellationToken cancellationToken)
        {
            lock (_locker)
            {
                if (_stopped)
                    return;

                _logger.Log("Sensus health test is running (test " + ++_healthTestCount + ")", LoggingLevel.Normal, GetType());

                foreach (Protocol protocol in _registeredProtocols)
                {
                    if (cancellationToken.IsCancellationRequested)
                        break;
                    
                    if (_runningProtocolIds.Contains(protocol.Id))
                    {
                        protocol.TestHealth();

                        if (_healthTestCount % _healthTestsPerProtocolReport == 0)
                            protocol.StoreMostRecentProtocolReport();
                    }
                }
            }
        }

        public void UnregisterProtocol(Protocol protocol)
        {
            lock (_locker)
            {
                protocol.Stop();
                _registeredProtocols.Remove(protocol);
                SaveAsync();
            }
        }

        /// <summary>
        /// Stops the service helper, but leaves it in a state in which subsequent calls to Start will succeed. This happens, for example, when the service is stopped and then 
        /// restarted without being destroyed.
        /// </summary>
        public void StopAsync()
        {
            new Thread(() =>
                {
                    Stop();

                }).Start();
        }

        public virtual void Stop()
        {
            lock (_locker)
            {
                if (_stopped)
                    return;

                _logger.Log("Stopping Sensus service.", LoggingLevel.Normal, GetType());

                foreach (Protocol protocol in _registeredProtocols)
                    protocol.Stop();

                _stopped = true;
            }
        }

        public string GetSharePath(string extension)
        {
            lock (_locker)
            {
                int fileNum = 0;
                string path = null;
                while (path == null || File.Exists(path))
                    path = Path.Combine(SHARE_DIRECTORY, fileNum++ + (string.IsNullOrWhiteSpace(extension) ? "" : "." + extension.Trim('.')));

                return path;
            }
        }

        public virtual void Dispose()
        {
            try
            {
                Stop();
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("Failed to stop service helper:  " + ex.Message);
            }

            try
            {
                _logger.Close();
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("Failed to close logger:  " + ex.Message);
            }

            SINGLETON = null;
        }
    }
}
