using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace MouseMover
{
    public class Window
    {
        const int Hide = 0;
        const int Show = 1;

        [DllImport("Kernel32.dll")]
        private static extern IntPtr GetConsoleWindow();

        [DllImport("User32.dll")]
        private static extern bool ShowWindow(IntPtr hWnd, int cmdShow);

        public static void HideCurrentWindow()
        {
            IntPtr hWndConsole = GetConsoleWindow();

            if (hWndConsole != IntPtr.Zero)
                ShowWindow(hWndConsole, Hide);
        }
    }
}
