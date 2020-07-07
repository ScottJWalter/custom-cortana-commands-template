using MQTTnet;
using MQTTnet.Client.Options;
using MQTTnet.Extensions.ManagedClient;
using System;
using System.Diagnostics;

namespace CustomCortanaCommands
{
    public static partial class CortanaFunctions
    {
        public static async void Cmd_SendMQTT()
        {
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
        }
    }
}
