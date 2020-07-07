using System;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Popups;

using Windows.Storage;
using Windows.ApplicationModel;
using Windows.ApplicationModel.VoiceCommands;

using Windows.System;
using Windows.Media.SpeechRecognition;
using Windows.ApplicationModel.Activation;

// for SendSerialData example
using Windows.Devices.SerialCommunication;
using Windows.Devices.Enumeration;
using Windows.Storage.Streams;

// for PushIt example
using System.Diagnostics;
using Windows.Web.Http;

// MQTT
using MQTTnet.Extensions.ManagedClient;
using MQTTnet.Client.Options;
using MQTTnet;
using System.Threading.Tasks;

namespace CustomCortanaCommands
{
    public static class CortanaFunctions
    {
        /*
        This is the lookup of VCD CommandNames as defined in
        CustomVoiceCommandDefinitions.xml to their corresponding actions
        */
        private readonly static IReadOnlyDictionary<string, Delegate> vcdLookup = new Dictionary<string, Delegate>{
            /*
            {<command name from VCD>, (Action)(async () => {
                 <code that runs when that commmand is called>
            })}
            */

            {"OpenWebsite", (Action)(async () => {
                 Uri website = new Uri("http://www.crclayton.com");
                 await Launcher.LaunchUriAsync(website);
             })},

            {"OpenFile", (Action)(async () => {
                StorageFile file = await Package.Current.InstalledLocation.GetFileAsync("Test.txt");
                await Launcher.LaunchFileAsync(file);
            })},

            {"CreateFile", (Action)(async () => {
                StorageFolder storageFolder = ApplicationData.Current.LocalFolder;
                StorageFile sampleFile = await storageFolder.CreateFileAsync(
                    "NewFile.txt", CreationCollisionOption.ReplaceExisting);

                await storageFolder.GetFileAsync("NewFile.txt");
                await FileIO.WriteTextAsync(sampleFile, "This file was created by Cortana at " + DateTime.Now);
            })},

            {"SendSerialData", (Action)(async () => {
                const string comPort = "COM3";
                const string serialMessage = "String sent to the COM port";

                string selector = SerialDevice.GetDeviceSelector(comPort);
                DeviceInformationCollection devices = await DeviceInformation.FindAllAsync(selector);

                if(devices.Count == 0)
                {
                    MessageDialog popup = new MessageDialog($"No {comPort} device found.");
                    await popup.ShowAsync();
                    return;
                }

                DeviceInformation deviceInfo = devices[0];
                SerialDevice serialDevice = await SerialDevice.FromIdAsync(deviceInfo.Id);
                serialDevice.BaudRate = 9600;
                serialDevice.DataBits = 8;
                serialDevice.StopBits = SerialStopBitCount.Two;
                serialDevice.Parity = SerialParity.None;

                DataWriter dataWriter = new DataWriter(serialDevice.OutputStream);
                dataWriter.WriteString(serialMessage);
                await dataWriter.StoreAsync();
                dataWriter.DetachStream();
                dataWriter = null;
            })},

            /*
             * Command:  TODO
             */
            {"REST-POST", (Action)(async () => {
                try
                {
                    Debug.WriteLine("Posting to REST API.");

                    HttpClient httpClient = new HttpClient();
                    // The base URL for the REST endpoint
                    Uri uri = new Uri("");

                    // Construct JSON to post
                    // NOTE:  The following chunk is very API dependent, with each API possibly requiring
                    // a different payload configuration
                    const string APPLICATION_API_TOKEN = "";
                    const string USER_KEY = "";
                    const string MESSAGE = "You Have Been Pushed!"; // Or whatever you wish to send

                    HttpStringContent content = new HttpStringContent(
                        "{\"token\": \"" + APPLICATION_API_TOKEN + "\", \"user\": \"" + USER_KEY + "\", \"message\": \"" + MESSAGE + "\" }",
                        UnicodeEncoding.Utf8,
                        "application/json"
                        );

                    // Post the JSON and wait for a response.
                    HttpResponseMessage httpResponseMessage = await httpClient.PostAsync( uri, content );

                    // Make sure the post succeeded, and write out the response.
                    httpResponseMessage.EnsureSuccessStatusCode();
                    var httpResponseBody = await httpResponseMessage.Content.ReadAsStringAsync();
                    Debug.WriteLine(httpResponseBody);
                }
                catch (Exception ex)
                {
                    // Write out any exceptions.
                    Debug.WriteLine(ex);
                }
             })},

            /*
             * Command:  Hey Cortana, Listen Up! Initiate <script to run>
             */
            {"RunScript", (Action)(async () => {
                string script_name = CortanaFunctions.GetSlotData("free_form_text");

                try
                {
                    Debug.WriteLine("Run script here.");

                    await CortanaFunctions
                        .ExecuteScript(script_name)
                        .ConfigureAwait(false)
                        ;

                    Debug.WriteLine("Bat file executed !!");
                }
                catch (Exception ex)
                {
                    // Write out any exceptions.
                    Debug.WriteLine(ex);
                }
             })},

            /*
             * Command:  Hey Cortana, Listen up!  Tell <device> <command>
             */
            {"SendMQTT", (Action)(async () => {
                string device = CortanaFunctions.GetSlotData("MQTT_device");
                string message = CortanaFunctions.GetSlotData("free_form_text");

                try
                {
                    Debug.WriteLine("Send MQTT here.");

                    string clientId = Guid.NewGuid().ToString();
                    const string mqttURI = "";
                    const string mqttUser = "";
                    const string mqttPassword = "";
                    const int mqttPort = 0;
                    const bool mqttSecure = false;

                    var messageBuilder = new MqttClientOptionsBuilder()
                        .WithClientId(clientId)
                        .WithCredentials(mqttUser, mqttPassword)
                        .WithTcpServer(mqttURI, mqttPort)
                        .WithCleanSession()
                        ;

                    var options = mqttSecure
                        ? messageBuilder
                            .WithTls()
                            .Build()
                        : messageBuilder
                            .Build()
                        ;

                    var managedOptions = new ManagedMqttClientOptionsBuilder()
                        .WithAutoReconnectDelay(TimeSpan.FromSeconds(5))
                        .WithClientOptions(options)
                        .Build()
                        ;

                    IManagedMqttClient client = new MqttFactory().CreateManagedMqttClient();

                    await client
                        .StartAsync(managedOptions)
                        .ConfigureAwait(false)
                        ;
                }
                catch (Exception ex)
                {
                    // Write out any exceptions.
                    Debug.WriteLine(ex);
                }
             })},

            /*
             * Command:  Hey Cortana, Listen Up! Send Push.
             */
            {"PushIt", (Action)(async () => {
                try
                {
                    HttpClient httpClient = new HttpClient();
                    // The base URL for the Pushover.Net REST endpoint
                    Uri uri = new Uri("https://api.pushover.net/1/messages.json");

                    /*
                     * NOTE:  It's both advisable and left as an exercise to pull the application key
                     *        and user key out of the strings here, and store them elsewhere for
                     *        security purposes.  THIS IS JUST A SIMPLE EXMPLE.  CAVEAT EMPTOR.
                     */
                    // Construct JSON to post
                    const string APPLICATION_API_TOKEN = ""; // Generated at https://pushover.net/apps/build
                    const string USER_KEY = ""; // Found on your dashboard at https://pushover.net
                    const string MESSAGE = "You Have Been Pushed!"; // Or whatever you wish to send

                    HttpStringContent content = new HttpStringContent(
                        "{\"token\": \"" + APPLICATION_API_TOKEN + "\", \"user\": \"" + USER_KEY + "\", \"message\": \"" + MESSAGE + "\" }",
                        UnicodeEncoding.Utf8,
                        "application/json"
                        );

                    // Post the JSON and wait for a response.
                    HttpResponseMessage httpResponseMessage = await httpClient.PostAsync( uri, content );

                    // Make sure the post succeeded, and write out the response.
                    httpResponseMessage.EnsureSuccessStatusCode();
                    var httpResponseBody = await httpResponseMessage.Content.ReadAsStringAsync();
                    Debug.WriteLine(httpResponseBody);
                }
                catch (Exception ex)
                {
                    // Write out any exceptions.
                    Debug.WriteLine(ex);
                }
             })},
        };

        /**
         * A local copy of the SpeechRecognitionResult returned from Cortana, necessary to grab the contents
         * of data slots in the intent.
         */
        private static SpeechRecognitionResult _srr = null;

        /*
        Register Custom Cortana Commands from VCD file
        */
        public static async void RegisterVCD()
        {
            try
            {
                StorageFile vcd = await Package.Current.InstalledLocation.GetFileAsync("CustomVoiceCommandDefinitions.xml");
                await VoiceCommandDefinitionManager.InstallCommandDefinitionsFromStorageFileAsync(vcd);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("There was an error registering the Voice Command Definitions", ex);
            }
        }

        public static string GetSlotData(string interpretationKey)
        {
            return CortanaFunctions._srr.SemanticInterpretation.Properties[interpretationKey].FirstOrDefault();
        }

        public static async Task<int> ExecuteScript(string fileName, string args = null)
        {
            using (var process = new Process
            {
                StartInfo =
                {
                    FileName = fileName, Arguments = args,
                    UseShellExecute = false, CreateNoWindow = true,
                    RedirectStandardOutput = true, RedirectStandardError = true
                },
                EnableRaisingEvents = true
            })
            {
                return await CortanaFunctions.RunProcessAsync(process).ConfigureAwait(false);
            }
        }

        private static Task<int> RunProcessAsync(Process process)
        {
            var tcs = new TaskCompletionSource<int>();

            process.Exited += (s, ea) => tcs.SetResult(process.ExitCode);
            process.OutputDataReceived += (s, ea) => Console.WriteLine(ea.Data);
            process.ErrorDataReceived += (s, ea) => Console.WriteLine("ERR: " + ea.Data);

            bool started = process.Start();
            if (!started)
            {
                //you may allow for the process to be re-used (started = false)
                //but I'm not sure about the guarantees of the Exited event in such a case
                throw new InvalidOperationException("Could not start process: " + process);
            }

            process.BeginOutputReadLine();
            process.BeginErrorReadLine();

            return tcs.Task;
        }

        /*
        Look up the spoken command and execute its corresponding action
        */
        public static void RunCommand(VoiceCommandActivatedEventArgs cmd)
        {
            CortanaFunctions._srr = cmd.Result;
            string commandName = CortanaFunctions._srr.RulePath[0];
            vcdLookup[commandName].DynamicInvoke();
        }
    }
}
