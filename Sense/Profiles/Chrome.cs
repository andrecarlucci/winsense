using System;
using System.Collections.Generic;
using Sense.Behaviors;

namespace Sense.Profiles {
    public class Chrome : Profile {
        public Chrome(Dictionary<Type, Behavior> allBehaviors) : base(allBehaviors) {
            Add<HandToMouseBehavior>();
            Add<DoubleBlinkToScrollDown>();
        }

        public override string Name {
            get { return "chrome"; }
        }
    }
}
