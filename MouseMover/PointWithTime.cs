using System;
using System.Collections.Generic;
using System.Text;

namespace MouseMover
{
    public class PointWithTime
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Miliseconds { get; set; }

        public PointWithTime() { }
        public PointWithTime(int x, int y, int miliseconds)
        {
            this.X = x;
            this.Y = y;
            this.Miliseconds = miliseconds;
        }

        public override string ToString()
        {
            return $"X: {X}, Y: {Y}, Miliseconds: {Miliseconds}";
        }

        public bool IsEquealCoordinates(PointWithTime point)
        {
            if (point == null)
                return false;

            return this.X == point.X && this.Y == point.Y;
        }
    }
}
