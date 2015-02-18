namespace Sense.Bob {
    public class WhatsYourName : ISubject {
        public string GetAnswerFor(string sentence) {
            if (sentence.HasWordsInSequence("what's|wants", "your|her", "name")) {
                return "my name is " + MrBob.MyName;
            }
            return "";
        }

        public void OnOtherSubject() {
            
        }
    }
}