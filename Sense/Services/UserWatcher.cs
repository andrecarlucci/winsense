using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sense.Storage;
using Sense.Util;
using SharpSenses;
using XamlActions;

namespace Sense.Services {
    public class UserWatcher {
        private readonly ICamera _camera;
        private readonly IInputService _inputService;
        private bool _recognizeRequested;
        private string _username;

        public string Username {
            get { return _username; }
            set {
                if (_username == value) {
                    return;
                }
                _username = value;
                Mediator.Default.Publish(new UserChangedMessage(value));
            }
        }

        public UserWatcher(ICamera camera, IInputService inputService) {
            _camera = camera;
            _camera.Face.FaceRecognized += FaceOnFaceRecognized;
            _inputService = inputService;
        }

        public void RecognizeUser() {
            if (_recognizeRequested) {
                return;
            }
            _recognizeRequested = true;
            _camera.Face.RecognizeFace();
        }

        private void FaceOnFaceRecognized(object sender, FaceRecognizedEventArgs args) {
            int userId = args.UserId;
            if (IsTheRegisteredUser(userId)) {
                Username = Config.Default.Get(ConfigKeys.Username);
                return;
            }
            if (_recognizeRequested) {
                string username = _inputService.GetInput("Face recognized!", "Please, enter your name");
                if (String.IsNullOrWhiteSpace(username)) {
                    return;
                }
                username = username.Trim().ToLower();
                Config.Default.Set(ConfigKeys.UserId, userId);
                Config.Default.Set(ConfigKeys.Username, username);
                Username = username;    
            }
            else {
                Username = "";
            }
            _recognizeRequested = false;
        }

        private static bool IsTheRegisteredUser(int userId) {
            return Config.Default.GetInt(ConfigKeys.UserId) == userId;
        }
    }
}
