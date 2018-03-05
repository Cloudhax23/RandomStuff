using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Speech.Recognition;
using System.Speech.Synthesis;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace ShudUp
{
    class Program
    {
        private const int APPCOMMAND_VOLUME_MUTE = 0x80000;
        private const int WM_APPCOMMAND = 0x319;
        private bool mute = false;

        [DllImport("user32.dll")]
        public static extern IntPtr SendMessageW(IntPtr hWnd, int Msg, IntPtr wParam, IntPtr lParam);
        static void Main(string[] args)
        {
            SpeechRecognitionEngine SpeechEngine = new SpeechRecognitionEngine();
            GrammarBuilder gb = new GrammarBuilder();
            Choices hushTypes = new Choices();
            hushTypes.Add(new string[] { "Shhh", "shh", "shhh", "sshh", "hush", "speakers off", "speakers on" });
            gb.Append(hushTypes);

            Grammar g = new Grammar(gb);
            SpeechEngine.LoadGrammar(g);

            SpeechEngine.SpeechRecognized += SpeechEngine_SpeechRecognized;
            SpeechEngine.SetInputToDefaultAudioDevice();
            SpeechEngine.RecognizeAsync(RecognizeMode.Multiple);
            Console.ReadLine();
        }

        private static void SpeechEngine_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            if(e.Result.Text == "speakers on")
            {
                SendMessageW(Process.GetCurrentProcess().MainWindowHandle, WM_APPCOMMAND, Process.GetCurrentProcess().MainWindowHandle, (IntPtr)APPCOMMAND_VOLUME_MUTE);

            }
            else
            {
                SendMessageW(Process.GetCurrentProcess().MainWindowHandle, WM_APPCOMMAND, Process.GetCurrentProcess().MainWindowHandle, (IntPtr)APPCOMMAND_VOLUME_MUTE);
            }
        }
    }
}
