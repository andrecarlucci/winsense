using System;
using System.Collections.Generic;
using Sense.Behaviors;

namespace Sense.Profiles {
    public class Chrome : Profile {
        public Chrome(Dictionary<Type, Behavior> allBehaviors) : base(allBehaviors) {
            Add<SlideToControlTabBehavior>();
            Add<HandToMouseBehavior>();
            Add<DoubleBlinkForScrollDown>();
        }

        public override string Name {
            get { return "chrome"; }
        }
    }
}
