using System;

namespace MouseMover
{
    class Program
    {
        static void Main(string[] args)
        {
            Window.HideCurrentWindow();
            Mouse mouse = new Mouse();

            while (true)
            {
                mouse.CurrentPosition = new Mouse.Point(960, 270);
                bool result = mouse.MoveByPoints(5000, new Mouse.Point(960, 540), new Mouse.Point(960, 270));
                if (!result)
                    break;
            }
        }
    }
}
