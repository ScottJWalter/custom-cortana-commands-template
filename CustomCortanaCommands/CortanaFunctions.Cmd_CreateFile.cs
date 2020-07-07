using System;
using Windows.Storage;

namespace CustomCortanaCommands
{
    public static partial class CortanaFunctions
    {
        public static async void Cmd_CreateFile()
        {
            StorageFolder storageFolder = ApplicationData.Current.LocalFolder;
            StorageFile sampleFile = await storageFolder.CreateFileAsync(
                "NewFile.txt", CreationCollisionOption.ReplaceExisting);

            await storageFolder.GetFileAsync("NewFile.txt");
            await FileIO.WriteTextAsync(sampleFile, "This file was created by Cortana at " + DateTime.Now);
        }
    }
}
