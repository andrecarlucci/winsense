using System;
using System.Threading.Tasks;
using SharpSenses;
using XamlActions;

namespace Sense.Services {
    public class StartCameraService {
        private readonly ICamera _camera;

        public StartCameraService(ICamera camera) {
            _camera = camera;
        }

        public void StartAsync() {
            Task.Run(async () => {
                while (true) {
                    try {
                        _camera.Start();
                        Mediator.Default.Publish(new NotifyIconMessage("RealSense Camera Started"));
                        return;
                    }
                    catch (CameraException) {
                        Mediator.Default.Publish(new NotifyIconMessage("Please, connect the RealSense Camera"));
                    }
                    await Task.Delay(TimeSpan.FromSeconds(5));
                }
            });
        }
    }
}