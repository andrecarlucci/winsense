using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sense.Models;
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
            _recognizeRequested = true;
            _camera.Face.UserId = -1;
            _camera.Face.RecognizeFace();
        }

        private void FaceOnFaceRecognized(object sender, FaceRecognizedEventArgs args) {
            int userId = args.UserId;
            if (userId < 0) {
                Username = "";
                return;
            }
            if (_recognizeRequested) {
                string username = _inputService.GetInput("Face recognized!", "Please, enter your name as the registered user");
                _recognizeRequested = false;
                if (String.IsNullOrWhiteSpace(username)) {
                    return;
                }
                username = username.Trim().ToLower();
                WinSenseConfig.SetUser(new User(userId, username));
                _camera.Speech.Say("Hello, " + username + ". Welcome to Windows Sense.");
            }
            var user = WinSenseConfig.GetUser();
            if (user.Id == userId) {
                Username = user.Name;
                return;
            }
            Username = "";
        }
    }
}
