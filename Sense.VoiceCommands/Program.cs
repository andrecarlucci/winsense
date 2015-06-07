using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Sense.Services;
using SharpSenses;
using XamlActions.DI;

namespace Sense.VoiceCommands {
    class Program {
        static void Main(string[] args) {
            try {
                var camera = Camera.Create();
                ServiceLocator.Default.Register<ICamera>(() => camera);
                var speechService = new SpeechService(camera);
                speechService.Start();
            }
            catch (Exception ex) {
                Console.WriteLine("Speech: " + ex);
            }
            while (true) {
                Thread.Sleep(4000);
                Console.WriteLine("Alive");
            }
        }
        static void UnhandledExceptionTrapper(object sender, UnhandledExceptionEventArgs e) {
            Console.WriteLine(e.ExceptionObject.ToString());
            Console.WriteLine("Press Enter to continue");
            Console.ReadLine();
            Environment.Exit(1);
        }
    }
}
