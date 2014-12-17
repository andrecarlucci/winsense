using System;
using System.Collections.Generic;
using Sense.Behaviors;

namespace Sense.Profiles {
    public abstract class Profile {
        private readonly Dictionary<Type, Behavior> _allBehaviors;

        public abstract string Name { get; }
        protected List<Behavior> Behaviors { get; set; }

        protected Profile(Dictionary<Type, Behavior> allBehaviors) {
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