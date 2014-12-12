using System;
using System.Collections.Generic;
using Sense.Behaviors;

namespace Sense.Profiles {
    public class Profile {
        private readonly Dictionary<Type, Behavior> _allBehaviors;

        public string Name { get; set; }
        protected List<Behavior> Behaviors { get; set; }

        public Profile(Dictionary<Type, Behavior> allBehaviors) {
            _allBehaviors = allBehaviors;
            Behaviors = new List<Behavior>();
        }

        protected void Add<T>() where T : Behavior {
            Behaviors.Add(_allBehaviors[typeof(T)]);
        }

        public virtual void Activate() {
            Behaviors.ForEach(b => b.Activate());
        }

        public virtual void Deactivate() {
            Behaviors.ForEach(b => b.Deactivate());
        }
    }
}