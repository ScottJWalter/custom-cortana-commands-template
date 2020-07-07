using System;
using Windows.System;

namespace CustomCortanaCommands
{
    public static partial class CortanaFunctions
    {
        public static async void Cmd_OpenWebsite()
        {
            Uri website = new Uri("http://www.crclayton.com");
            await Launcher.LaunchUriAsync(website);
        }
    }
}
