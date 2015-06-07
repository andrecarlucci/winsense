using System;
using System.Collections.Generic;
using Sense.Behaviors;

namespace Sense.Profiles {
    public class PowerPoint : Profile {
        public PowerPoint(Dictionary<Type, Behavior> allBehaviors) : base(allBehaviors) {
            Add<DoubleBlickToRightBehavior>();
            Add<SlideToArrowsBehavior>();
            //Add<JoinHandsToCloseAppBehavior>();
        }
        public override string Name {
            get { return "powerpnt"; }
        }
    }
}
