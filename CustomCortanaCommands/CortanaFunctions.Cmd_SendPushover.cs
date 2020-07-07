using System;
using System.Diagnostics;
using Windows.Storage.Streams;
using Windows.Web.Http;

namespace CustomCortanaCommands
{
    public static partial class CortanaFunctions
    {
        public static async void Cmd_SendPushover()
        {
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
                HttpResponseMessage httpResponseMessage = await httpClient.PostAsync(uri, content);

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
        }
    }
}
