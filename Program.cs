﻿using System;
using System.Windows.Forms;
using System.Drawing;
using System.Runtime.InteropServices;

namespace VirtualDesktopSwitcher
{
    static class Program
    {
        [STAThread]
        [DllImport("user32.dll", SetLastError = true)]
        static extern void keybd_event(byte bVk, byte bScan, int dwFlags, int dwExtraInfo);

        public const int KEYEVENTF_KEYDOWN = 0x0000; //Key down flag
        public const int KEYEVENTF_KEYUP = 0x0002; //Key up flag

        //Key Code
        public const int VK_LCONTROL = 0x0011;
        public const int VK_WIN = 0x005B;
        public const int VK_LEFT = 0x0025;
        public const int VK_RIGHT = 0x0027;
        public const int KEY_D = 0x0044;
        public const int VK_F4 = 0x0073;
        public const int VK_TAB = 0x0009;

        static readonly NotifyIcon notifyIconLeft = new NotifyIcon();
        static readonly NotifyIcon notifyIconRight = new NotifyIcon();

        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            //Initialize Left Notify Icon
            notifyIconLeft.Text = "Go to the left desktop";
            notifyIconLeft.ContextMenuStrip = GetContext();
            notifyIconLeft.Icon = new Icon("Left.ico");

            //Initialize Right Notify Icon
            notifyIconRight.Text = "Go to the right desktop";
            notifyIconRight.ContextMenuStrip = GetContext();
            notifyIconRight.Icon = new Icon("Right.ico");

            notifyIconLeft.Visible = true;
            notifyIconRight.Visible = true;

            notifyIconLeft.MouseClick += OnIconLeft_Click;
            notifyIconRight.MouseClick += OnIconRight_Click;

            Application.Run();
        }

        //Contex Menu
        private static ContextMenuStrip GetContext()
        {
            ContextMenuStrip CMS = new ContextMenuStrip();
            CMS.Items.Add("About", null, new EventHandler(About_Program));
            CMS.Items.Add("Close Current Desktop", null, new EventHandler(OnCloseCurrentDesktop_Click));
            CMS.Items.Add("Open New Desktop", null, new EventHandler(OnOpenNewDesktop_Click));
            CMS.Items.Add("Exit", null, new EventHandler(Exit_Click));

            return CMS;
        }
        private static void About_Program(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://github.com/hangacs/VirtualDesktopSwitcher");
        }

        private static void OnCloseCurrentDesktop_Click(object sender, EventArgs e)
        {
            CloseCurrentDesktop();
        }
        private static void OnOpenNewDesktop_Click(object sender, EventArgs e)
        {
            OpenNewDesktop();
        }

        private static void Exit_Click(object sender, EventArgs e)
        {
            notifyIconLeft.Dispose();
            notifyIconRight.Dispose();
            Application.Exit();
        }

        //Switch Virtual Desktop
        private static void OnIconLeft_Click(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                GoLeftDesktop();
            }
        }
        private static void OnIconRight_Click(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                GoRightDesktop();
            }
        }

        #region Keyboard Event
        public static void OpenNewDesktop()
        {
            CombineKey(KEY_D);
        }
        public static void CloseCurrentDesktop()
        {
            CombineKey(VK_F4);
        }
        public static void GoLeftDesktop()
        {
            CombineKey(VK_LEFT);
        }
        public static void GoRightDesktop()
        {
            CombineKey(VK_RIGHT);
        }
        #endregion

        public static void CombineKey(byte KEY)
        {
            keybd_event(VK_LCONTROL, 0, KEYEVENTF_KEYDOWN, 0);
            keybd_event(VK_WIN, 0, KEYEVENTF_KEYDOWN, 0);
            keybd_event(KEY, 0, KEYEVENTF_KEYDOWN, 0);

            keybd_event(VK_LCONTROL, 0, KEYEVENTF_KEYUP, 0);
            keybd_event(VK_WIN, 0, KEYEVENTF_KEYUP, 0);
            keybd_event(KEY, 0, KEYEVENTF_KEYUP, 0);
        }
    }
}
