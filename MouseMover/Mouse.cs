using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace MouseMover
{
    public class Mouse
    {
        [DllImport("user32.dll")]
        private static extern bool GetCursorPos(out Point lpPoint);

        [DllImport("user32.dll")]
        private static extern bool SetCursorPos(int X, int Y);

        public struct Point
        {
            public int X;
            public int Y;
            public Point(int x, int y)
            {
                X = x;
                Y = y;
            }

        }
        public Point CurrentPosition { get
            {
                Point p = new Point();
                GetCursorPos(out p);
                return p;
            }
            set 
            {
                SetCursorPos(value.X, value.Y);
            }
        }

        public bool SmoothMove(Point p1, Point p2, int miliseconds)
        {
            const decimal fps = 60;
            const int milisecondsForFrame = 13;
            const int deviation = 15;


            CurrentPosition = p1;
            Point p3 = new Point(p2.X - p1.X, p2.Y - p1.Y);
            int totalFrames = Convert.ToInt32((decimal)miliseconds / 1000.0m * fps);

            decimal xStep = (decimal)p3.X / (decimal)totalFrames;
            decimal yStep = (decimal)p3.Y / (decimal)totalFrames;

            decimal setXPos = p1.X;
            decimal setYPos = p1.Y;


            DateTime begin = DateTime.Now;

            for (int i = 0; i < totalFrames; i++)
            {
                TimeSpan btw = DateTime.Now - begin;

                setXPos += xStep;
                setYPos += yStep;

                int currentX = CurrentPosition.X;
                int currentY = CurrentPosition.Y;

                if (currentX < setXPos - deviation || currentX > setXPos + deviation ||
                    currentY < setYPos - deviation || currentY > setYPos + deviation)
                    return false;

                CurrentPosition = new Point() { X = Convert.ToInt32(setXPos), Y = Convert.ToInt32(setYPos) };

                if(i > 0)
                {
                    double needWaitTime = (double)miliseconds / totalFrames * (double)i;
                    int waitTime = Convert.ToInt32(needWaitTime - btw.TotalMilliseconds);
                    if(waitTime - 3 > 0)
                        System.Threading.Thread.Sleep(waitTime - 3);
                }

                else
                    System.Threading.Thread.Sleep(milisecondsForFrame - 3);
            }

            return true;
        }

        public bool SmoothMove(Point p2, int miliseconds) =>
            SmoothMove(CurrentPosition, p2, miliseconds);

        public bool MoveByPoints(int miliseconds, params Point[] points)
        {
            int milisecondsByPoint = miliseconds / points.Length;

            //CurrentPosition = points[0];

            foreach (var point in points)
            {
                bool result = SmoothMove(point, milisecondsByPoint);
                if (!result)
                    return result;
            }

            return true;
        }
    }
}
