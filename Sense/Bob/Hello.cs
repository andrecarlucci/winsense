namespace Sense.Bob {
    public class Hello : ISubject {
        private int _count;

        public string GetAnswerFor(string sentence) {
            if (sentence.HasWordsInSequence("how", "are", "you")) {
                _count++;
                switch (_count) {
                    case 1:
                        return "I'm fine and you?";
                    case 2:
                        return "I'm fine.";
                    case 3:
                        return "I already told you.";
                    default:
                        return "Fine.";
                }    
            }
            if (sentence.HasWordsInSequence("say", "hello")) {
                return "hello";
            }
            if (sentence.HasWordsInSequence("i'm", "fine")) {
                return "great";
            }
            return "";
        }

        public void OnOtherSubject() {
        }
    }
}