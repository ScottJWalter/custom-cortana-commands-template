using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace CustomCortanaCommands
{
    public static partial class CortanaFunctions
    {
        public static async void Cmd_RunScript()
        {
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
        }

        public static async Task<int> ExecuteScript(string fileName, string args = null)
        {
            using (var process = new Process
            {
                StartInfo =
                {
                    FileName                = fileName,
                    Arguments               = args,
                    UseShellExecute         = false,
                    CreateNoWindow          = true,
                    RedirectStandardOutput  = true,
                    RedirectStandardError   = true
                },
                EnableRaisingEvents = true
            })
            {
                return await CortanaFunctions
                    .RunProcessAsync(process)
                    .ConfigureAwait(false)
                    ;
            }
        }
    }
}
