namespace Sense.Bob {
    public class ThankYou : ISubject {

        public string GetAnswerFor(string sentence) {
            if (sentence.HasWordsInSequence("thank", "you")) {
                return "you are welcome";
            }
            return "";
        }

        public void OnOtherSubject() {
            
        }
    }
}