using System;
using System.Diagnostics;
using Sense.Bob;
using SharpSenses;
using XamlActions;

namespace Sense.Services {
    public class SpeechService {
        private readonly ISpeech _speech;
        private readonly MrBob _mrBob = new MrBob();

        public SpeechService(ICamera camera) {
            _speech = camera.Speech;
        }

        public void Start() {
            _speech.CurrentLanguage = SupportedLanguage.EnUS;
            _speech.SpeechRecognized += (sender, args) => {
                Console.WriteLine("Voice: " + args.Sentence);
                var answer = _mrBob.GetAnswerFor(args.Sentence);
                if (!String.IsNullOrEmpty(answer)) {
                    _speech.Say(answer);
                }
            };
            _speech.EnableRecognition();
            Console.WriteLine("recognition enabled");
        }
    }
}
