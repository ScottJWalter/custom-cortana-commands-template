using System;
using System.Diagnostics;
using Windows.Storage.Streams;
using Windows.Web.Http;

namespace CustomCortanaCommands
{
    public static partial class CortanaFunctions
    {
        public static async void Cmd_RestApiPost()
        {
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
