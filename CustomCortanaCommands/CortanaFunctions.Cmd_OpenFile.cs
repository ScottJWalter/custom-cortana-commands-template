using System;
using Windows.ApplicationModel;
using Windows.Storage;
using Windows.System;

namespace CustomCortanaCommands
{
    public static partial class CortanaFunctions
    {
        public static async void Cmd_OpenFile()
        {
            StorageFile file = await Package.Current.InstalledLocation.GetFileAsync("Test.txt");
            await Launcher.LaunchFileAsync(file);
        }
    }
}
