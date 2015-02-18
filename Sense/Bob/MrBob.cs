using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Sense.Storage;
using XamlActions.DI;

namespace Sense.Bob {
    public class MrBob {

        public static string MyName = "Sarah";

        private readonly List<ISubject> _subjects = new List<ISubject>();

        public MrBob() {
            foreach (Type mytype in Assembly.GetExecutingAssembly()
                                            .GetTypes()
                                            .Where(mytype => mytype.GetInterfaces().Contains(typeof(ISubject)))) {
                AddSubject(mytype);                
            }
        }

        private void AddSubject(Type type) {
            _subjects.Add((ISubject)ServiceLocator.Default.Resolve(type));
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
                for (int j = 1; j < _subjects.Count; j++) {
                    _subjects[j].OnOtherSubject();
                }
                return answer;
            }
            return "";
        }
    }
}