using System;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace MouseMover
{
    public partial class Mouse
    {
        [DllImport("user32.dll")]
        private static extern bool GetCursorPos(out Point lpPoint);

        [DllImport("user32.dll")]
        private static extern bool SetCursorPos(int X, int Y);




        public Point CurrentPosition
        {
            get
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

               

                int currentX = CurrentPosition.X;
                int currentY = CurrentPosition.Y;

                if (currentX < setXPos - deviation || currentX > setXPos + deviation ||
                    currentY < setYPos - deviation || currentY > setYPos + deviation)
                    return false;

                setXPos += xStep;
                setYPos += yStep;

                CurrentPosition = new Point() { X = Convert.ToInt32(setXPos), Y = Convert.ToInt32(setYPos) };

                if (i > 0)
                {
                    double needWaitTime = (double)miliseconds / totalFrames * (double)i;
                    int waitTime = Convert.ToInt32(needWaitTime - btw.TotalMilliseconds);
                    if (waitTime - 3 > 0)
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
            foreach (var point in points)
            {
                bool result = SmoothMove(point, milisecondsByPoint);
                if (!result)
                    return result;
            }

            return true;
        }

        public bool MoveByTimePoints(params PointWithTime[] points)
        {
            foreach (var point in points)
            {
                bool result = SmoothMove(new Point(point.X, point.Y), point.Miliseconds);
                if (!result)
                    return result;
            }

            return true;
        }

        public List<PointWithTime> Record(int miliseconds)
        {
            List<PointWithTime> points = new List<PointWithTime>();
            const int stopRecordMiliseconds = 2000;

            while (true)
            {
                PointWithTime point = new PointWithTime(CurrentPosition.X, CurrentPosition.Y, miliseconds);
                PointWithTime lastPoint = points.LastOrDefault();
                Console.WriteLine(point);

                if (lastPoint != null)
                {
                    int totalMiliseconds = 0;
                    for (int i = points.Count - 1; i > 0; i--)
                    {
                        if (!points[i].IsEquealCoordinates(lastPoint))
                            break;

                        totalMiliseconds += points[i].Miliseconds;
                    }

                    if (totalMiliseconds > stopRecordMiliseconds)
                        break;

                }

                points.Add(point);
                System.Threading.Thread.Sleep(miliseconds);
            }

            PointWithTime _lastPoint = points.LastOrDefault();
            List<PointWithTime> pointsForDelete = new List<PointWithTime>();
            for (int i = points.Count - 1; i > 0; i--)
            {
                if (!points[i].IsEquealCoordinates(_lastPoint))
                    break;

                pointsForDelete.Add(points[i]);
            }

            points = points.Where(p => !pointsForDelete.Any(_p => _p == p)).ToList();
            points.Add(_lastPoint);

            return points;
        }

        public List<PointWithTime> Record() =>
            Record(50);

    }

    public partial class Mouse
    {
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

    }
}
