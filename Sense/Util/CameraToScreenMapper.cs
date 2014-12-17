using System;
using System.Drawing;
using SharpSenses;

namespace Sense.Util {
    public class CameraToScreenMapper : IDisposable {
        private int _screenWidth;
        private int _screenHeight;
        private Item _item;
        private Point _lastPosition;

        public event Action<Point, Point> Moved;

        public CameraToScreenMapper(int screenWidth, int screenHeight, Item item) {
            _screenWidth = screenWidth;
            _screenHeight = screenHeight;
            _item = item;
            item.Moved += ItemOnMoved;
        }

        private void ItemOnMoved(object sender, PositionEventArgs positionEventArgs) {
            FilterMove(positionEventArgs.NewPosition);
        }

        private void FilterMove(Position position) {
            var newPosition = MapToScreen(position.Image, _screenWidth, _screenHeight);
            if (IsNoise(newPosition)) {
                return;
            }
            OnMoved(_lastPosition, newPosition);
            _lastPosition = newPosition;
        }

        public static Point MapToScreen(Point3D point, int screenWidth, int screenHeight) {
            int maxX = 500;
            int maxY = 400;
            var x = point.X;
            var y = point.Y;
            
            //screenWidth = Convert.ToInt32(screenWidth * 1.75);
            //screenHeight = Convert.ToInt32(screenHeight * 1.5);
            var left = (int)(screenWidth - (x / maxX) * screenWidth);
            var top = (int)((y / maxY) * screenHeight);

            //Debug.WriteLine("Left: {0}|{1} Top: {2}|{3}", x, left, y, top);
            
            //left = left - screenWidth / 5;
            //top = top - screenHeight / 4;
            return new Point(left, top);
        }

        private bool IsNoise(Point newPosition) {
            return false;
            //return MathEx.CalcDistance(_lastPosition, newPosition) <= 10;
        }

        protected virtual void OnMoved(Point from, Point to) {
            Action<Point, Point> handler = Moved;
            if (handler != null) handler(@from, to);
        }

        public void Dispose() {
            _item.Moved -= ItemOnMoved;
        }
    }
}