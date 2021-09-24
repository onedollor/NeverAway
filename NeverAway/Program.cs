using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NeverAway
{
    internal static class NativeMethods 
    {
        [DllImport("Kernel32.dll")]
        public static extern IntPtr GetConsoleWindow();

        [DllImport("user32.dll")]
        public static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        public const int SW_HIDE = 0;
        public const int SW_SHOW = 5;
        public static void HideConsole()
        {
            ShowWindow(GetConsoleWindow(), SW_HIDE);
        }

        [DllImport("user32.dll")]
        public static extern void keybd_event(byte bVk,byte bScan,uint dwFlags,IntPtr dwExtraInfo);

        public const byte VK_NUMLOCK = 0x90;
        public const int KEYEVENTF_EXTENDEDKEY = 0x0001;
        public const int KEYEVENTF_KEYUP = 0x0002;
        public const byte VK_LCONTROL = 0xA2;
        public const byte VK_RCONTROL = 0xA3;

        [DllImport("user32.dll")]
        public static extern int SetKeyboardState(byte[] keyState);

        [DllImport("user32.dll")]
        public static extern int GetKeyboardState(ref byte keyState);
        public static void SetNumLock(Boolean bState)
        {
            byte[] keyState = new byte[256];
            GetKeyboardState(ref keyState[0]);

            if ((bState && 0==(keyState[VK_NUMLOCK] & 1)) ||
                (!bState && 1==(keyState[VK_NUMLOCK] & 1)))
            {
                keybd_event(VK_NUMLOCK, 0x45, KEYEVENTF_EXTENDEDKEY | 0, (IntPtr)0);
                keybd_event(VK_NUMLOCK, 0x45, KEYEVENTF_EXTENDEDKEY | KEYEVENTF_KEYUP, (IntPtr)0);
            }
        }

        public static void PressControlKey(Boolean isLeft)
        {
            keybd_event(isLeft ? VK_LCONTROL : VK_RCONTROL, 0x45, 0, (IntPtr)0);
            keybd_event(isLeft ? VK_LCONTROL : VK_RCONTROL, 0x45, KEYEVENTF_KEYUP, (IntPtr)0);
        }
    }

    class Program
    {
        static void Main()
        {
            NativeMethods.HideConsole();

            int rand_num;
            var rand = new Random();

            long s = 0;
            while (true)
            {
                if (0 == (s % 2))
                {
                    NativeMethods.PressControlKey(true);
                }
                else 
                {
                    NativeMethods.PressControlKey(false);
                }
                
                s++;

                rand_num = ((Math.Abs(rand.Next()) + 1) % 30);
                if (0 == ((Math.Abs(rand.Next()) + 1) % 1000))
                {
                    if (0 == (s % 2))
                    {
                        NativeMethods.SetNumLock(true);
                    }
                    else
                    {
                        NativeMethods.SetNumLock(false);
                    }
                }

                Thread.Sleep(rand_num * 1000); 
            }
        }
    }
}
