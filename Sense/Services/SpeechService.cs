using SharpSenses;

namespace Sense.Services {
    public class SpeechService {
        private readonly ISpeech _speech;

        public SpeechService(ICamera camera) {
            _speech = camera.Speech;
        }

        public void Start() {
            _speech.CurrentLanguage = SupportedLanguage.EnUS;
            _speech.SpeechRecognized += (sender, args) => {

            };
            _speech.EnableRecognition();
        }
    }
}
