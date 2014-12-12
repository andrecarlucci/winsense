using MrWindows;
using SharpSenses;
using SharpSenses.Poses;
using System;
using System.Linq;

namespace Playground {
    class Program {

        private static ICamera camera;

        static void Main(string[] args) {
            var win = new Windows();
            win.Mouse.MouseLeftClick();

            try {
                camera = Camera.Create(CameraKind.RealSense);
            }
            catch (Exception ex) {
                camera = Camera.Create(CameraKind.RealSense);                   
            }
            
            camera.LeftHand.Moved += HandMoved; 
            camera.Gestures.SwipeRight += OnSwipe;
            camera.Gestures.SwipeLeft += OnSwipe;

            camera.LeftHand.Visible += OnHandVisible;
            camera.LeftHand.Opened += OnHandOpen;
            camera.LeftHand.Closed += OnHandClose;

            //camera.Face.Month.Moved += p => Plot();
            //camera.LeftHand.Index.Moved += p => Plot();

            //camera.LeftHand.Index.Opened += () => Console.WriteLine("Index open");
            //camera.LeftHand.Index.Closed += () => Console.WriteLine("Index closed");

            //camera.LeftHand.FingerOpened += f => Console.WriteLine(f.Kind + " open");
            //camera.LeftHand.FingerClosed += f => Console.WriteLine(f.Kind + " close");


            camera.Poses.PeaceBegin += hand => Console.WriteLine("Peace, bro");
            camera.Poses.PeaceEnd += hand => Console.WriteLine("Bye");
            var pose = PoseBuilder.Create().ShouldTouch(camera.Face.Month, camera.LeftHand.Index).Build();
            bool ok = false;
            pose.Begin += p => {
                Console.WriteLine("Super pose!");
            };

            camera.Start();

            Console.ReadLine();
            camera.Dispose();
        }

        private static void Plot() {
            Console.WriteLine(@"CX: {0:0} MX: {1:0}",
                camera.LeftHand.Index.Position.Image.X,
                camera.Face.Month.Position.Image.X);
        }

        private static void OnHandVisible() {
            Console.WriteLine("Visible");
        }

        private static void OnHandClose() {
            Console.WriteLine("Close");
        }

        private static void OnHandOpen() {
            Console.WriteLine("Open");
        }

        private static void HandMoved(Position position) {
            //Console.WriteLine(position);
        }

        private static void OnSwipe(Hand hand) {
            Console.WriteLine(hand.Side);
        }
    }
}
