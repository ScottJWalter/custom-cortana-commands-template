using System;
using Windows.Devices.Enumeration;
using Windows.Devices.SerialCommunication;
using Windows.Storage.Streams;
using Windows.UI.Popups;

namespace CustomCortanaCommands
{
    public static partial class CortanaFunctions
    {
        public static async void Cmd_SendSerialData()
        {
            const string comPort = "COM3";
            const string serialMessage = "String sent to the COM port";

            string selector = SerialDevice.GetDeviceSelector(comPort);
            DeviceInformationCollection devices = await DeviceInformation.FindAllAsync(selector);

            if (devices.Count == 0)
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
        }
    }
}
