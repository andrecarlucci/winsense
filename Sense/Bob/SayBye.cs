namespace Sense.Bob {
    public class SayBye : ISubject {

        public string GetAnswerFor(string sentence) {
            if (sentence.HasWordsInSequence("say|soon|see", "bye|by")) {
                return "bye bye! See you soon!";
            }
            return "";
        }

        public void OnOtherSubject() {

        }
    }
}