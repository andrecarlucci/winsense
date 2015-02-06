using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpSenses;
using XamlActions.DI;

namespace Sense.ViewModels {
    public class ViewModelLocator {

        public MainViewModel MainViewModel { get; set; }

        public ViewModelLocator() {
            MainViewModel = ServiceLocator.Default.Resolve<MainViewModel>();
        }
        
    }
}
