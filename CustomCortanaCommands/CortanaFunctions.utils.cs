using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.ApplicationModel.VoiceCommands;
using Windows.Storage;

namespace CustomCortanaCommands
{
    public static partial class CortanaFunctions
    {
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

        /*
        Look up the spoken command and execute its corresponding action
        */
        public static void RunCommand(VoiceCommandActivatedEventArgs cmd)
        {
            CortanaFunctions._srr = cmd.Result;
            string commandName = CortanaFunctions._srr.RulePath[0];
            vcdLookup[commandName].DynamicInvoke();
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
    }
}
