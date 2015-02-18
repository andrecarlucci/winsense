namespace Sense.Bob {
    public class NiceToMeetYou : ISubject {
        public string GetAnswerFor(string sentence) {
            if (sentence.HasWordsInSequence("nice", "meet|meeting", "you")) {
                return "Nice to meet you too";
            }
            return "";
        }

        public void OnOtherSubject() {
            
        }
    }
}