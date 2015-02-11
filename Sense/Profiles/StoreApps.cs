using System;
using System.Collections.Generic;
using Sense.Behaviors;

namespace Sense.Profiles {
    public class StoreApps : Profile {
        public StoreApps(Dictionary<Type, Behavior> allBehaviors)
            : base(allBehaviors) {
            Add<SlideToArrowsBehavior>();
            Add<WaveToCloseApp>();
            Add<LookToAllDirectionsToArrows>();
        }
        public override string Name {
            get { return "WWAHOST"; }
        }
    }
}