using MrWindows;
using SharpSenses;
using SharpSenses.Poses;
using System;
using System.Linq;
using System.Threading;
using MrWindows.KeyboardControl;
using SharpSenses.Gestures;

namespace Playground {
    class Program {

        private static ICamera camera;

        static void Main(string[] args) {
            var win = new Windows();
            win.Mouse.MouseLeftClick();

            //Thread.Sleep(2000);

            //var p = win.CurrentWindow.GetForegroundProcess();
            //p.Kill();

            //Console.WriteLine("Done!");
            //Environment.Exit(0);

            camera = Camera.Create();

            camera.Gestures.SlideLeft += GesturesOnSlideLeft;
            camera.Gestures.SlideRight += GesturesOnSlideLeft;

            //camera.Face.Month.Moved += p => Plot();
            //camera.LeftHand.Index.Moved += p => Plot();

            //camera.LeftHand.Index.Opened += () => Console.WriteLine("Index open");
            //camera.LeftHand.Index.Closed += () => Console.WriteLine("Index closed");

            //camera.LeftHand.FingerOpened += f => Console.WriteLine(f.Kind + " open");
            //camera.LeftHand.FingerClosed += f => Console.WriteLine(f.Kind + " close");

            var pose = PoseBuilder.Create()
                                  .ShouldBeNear(camera.Face.Mouth, camera.LeftHand.Index, 50)
                                  .HoldPoseFor(1000)
                                  .Build();
            pose.Begin += (s,a) => {
                Console.WriteLine("Super pose!");
            };

            camera.Start();

            Console.ReadLine();
            camera.Dispose();
        }

        private static void GesturesOnSlideLeft(object sender, GestureEventArgs gestureEventArgs) {
            Console.WriteLine(gestureEventArgs.GestureName);
        }

        private static void Plot() {
            Console.WriteLine(@"CX: {0:0} MX: {1:0}",
                camera.LeftHand.Index.Position.Image.X,
                camera.Face.Mouth.Position.Image.X);
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
