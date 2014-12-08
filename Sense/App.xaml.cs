using System.Windows;
using MrWindows;
using SharpSenses;
using SimpleInjector;

namespace Sense {
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application {

        public static Container Container;

        static App() {
            Container = new Container();
            Container.Register<Windows>(Lifestyle.Singleton);
            Container.Register<ICamera>(() =>  Camera.Create(CameraKind.RealSense), Lifestyle.Singleton);
            Container.Register<ProcessMonitor>(Lifestyle.Singleton);
        }
    }
}