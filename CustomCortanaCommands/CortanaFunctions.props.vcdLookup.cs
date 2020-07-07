using System;
using System.Collections.Generic;

namespace CustomCortanaCommands
{
    public static partial class CortanaFunctions
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

/*          {"OpenWebsite", (Action)(async () => {
                 Uri website = new Uri("http://www.crclayton.com");
                 await Launcher.LaunchUriAsync(website);
             })},
*/          {"OpenWebsite", (Action)CortanaFunctions.Cmd_OpenWebsite},

/*          {"OpenFile", (Action)(async () => {
                StorageFile file = await Package.Current.InstalledLocation.GetFileAsync("Test.txt");
                await Launcher.LaunchFileAsync(file);
            })},
*/          {"OpenFile", (Action)CortanaFunctions.Cmd_OpenFile},

/*          {"CreateFile", (Action)(async () => {
                StorageFolder storageFolder = ApplicationData.Current.LocalFolder;
                StorageFile sampleFile = await storageFolder.CreateFileAsync(
                    "NewFile.txt", CreationCollisionOption.ReplaceExisting);

                await storageFolder.GetFileAsync("NewFile.txt");
                await FileIO.WriteTextAsync(sampleFile, "This file was created by Cortana at " + DateTime.Now);
            })},
*/          {"CreateFile", (Action)CortanaFunctions.Cmd_CreateFile},

/*          {"SendSerialData", (Action)(async () => {
                string comPort = "COM3";
                string serialMessage = "String sent to the COM port";

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
*/          {"SendSerialData", (Action)CortanaFunctions.Cmd_SendSerialData},

/*          {"RestApiPost", (Action)(async () => {
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
*/          {"RestApiPost", (Action)CortanaFunctions.Cmd_RestApiPost},

            /*
             * Command:  Hey Cortana, Listen Up! Initiate <script to run>
             */
/*          {"RunScript", (Action)(async () => {
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
*/          {"RunScript", (Action)CortanaFunctions.Cmd_RunScript},

            /*
             * Command:  Hey Cortana, Listen up!  Tell <device> <command>
             */
/*          {"SendMQTT", (Action)(async () => {
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
*/          {"SendMQTT", (Action)CortanaFunctions.Cmd_SendMQTT},

            /*
             * Command:  Hey Cortana, Listen Up! Send Push.
             */
/*          {"SendPushover", (Action)(async () => {
                try
                {
                    HttpClient httpClient = new HttpClient();
                    // The base URL for the Pushover.Net REST endpoint
                    Uri uri = new Uri("https://api.pushover.net/1/messages.json");

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
*/          {"SendPushover", (Action) CortanaFunctions.Cmd_SendPushover},
        };
    }
}
