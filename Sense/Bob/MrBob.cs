using System;
using System.Collections.Generic;
using Sense.Storage;
using XamlActions.DI;

namespace Sense.Bob {
    public class MrBob {
        private readonly List<ISubject> _subjects = new List<ISubject>();

        public MrBob() {
            AddSubject<MoveActions>();
        }

        private void AddSubject<T>() where T : class, ISubject {
            _subjects.Add(ServiceLocator.Default.Resolve<T>());
        }

        public string GetAnswerFor(string sentence) {
            sentence = sentence.ToLower();
            for (var i = 0; i < _subjects.Count; i++) {
                var current = _subjects[i];
                var answer = current.GetAnswerFor(sentence);
                if (String.IsNullOrEmpty(answer)) {
                    continue;
                }
                _subjects.Remove(current);
                _subjects.Insert(0, current);
                return answer;
            }
            return "";
        }
    }
}