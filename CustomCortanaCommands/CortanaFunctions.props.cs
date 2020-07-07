using Windows.Media.SpeechRecognition;

namespace CustomCortanaCommands
{
    public static partial class CortanaFunctions
    {
        /**
         * A local copy of the SpeechRecognitionResult returned from Cortana, necessary to grab the contents
         * of data slots in the intent.
         */
        private static SpeechRecognitionResult _srr
        {
            get
            {
                return CortanaFunctions._srr;
            }
            set
            {
                CortanaFunctions._srr = value;
            }
        }
    }
}
