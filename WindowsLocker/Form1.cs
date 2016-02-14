using EasyFullScreen;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WindowsLocker {
    public partial class Form1 : Form {
        IntPtr desktop = user32.GetDesktopWindow();
        IntPtr target = user32.GetDesktopWindow();
        public Form1() {
            InitializeComponent();
        }

        private void button5_Click(object sender, EventArgs e) {

            ProcessList p = new ProcessList();
            p.ShowDialog();
            if (p.selectProcess == null) return;
            target = p.selectProcess.MainWindowHandle;
            StringBuilder WindowText = new StringBuilder(50);
            user32.GetWindowText(target, WindowText, WindowText.Capacity);
            StringBuilder ClassName = new StringBuilder(50);
            user32.GetClassName(target, ClassName, ClassName.Capacity);
            textBox1.Text = WindowText.ToString() + " (" + ClassName + ")";
        }

        private void button3_MouseDown(object sender, MouseEventArgs e) {
            button3.BackColor = Color.Red;
            timer1.Enabled = true;
            timer2.Enabled = false;
        }

        private void button3_MouseUp(object sender, MouseEventArgs e) {
            button3.BackColor = Control.DefaultBackColor;
            timer1.Enabled = false;
            timer2.Enabled = true;
        }

        private void timer1_Tick(object sender, EventArgs e) {

            Point pret = new Point();
            user32.GetCursorPos(ref pret);
            IntPtr newtarget = user32.WindowFromPoint(pret);
            if (newtarget != target) {
                target = newtarget;
            }
            StringBuilder WindowText = new StringBuilder(50);
            user32.GetWindowText(target, WindowText, WindowText.Capacity);
            StringBuilder ClassName = new StringBuilder(50);
            user32.GetClassName(target, ClassName, ClassName.Capacity);
            textBox1.Text = WindowText.ToString() + " (" + ClassName + ")";
        }

        private void timer2_Tick(object sender, EventArgs e) {
            IntPtr focus = user32.GetForegroundWindow();
            if (target != IntPtr.Zero && focus == target) {
                user32.RECT rect;
                //user32.GetWindowRect(new System.Runtime.InteropServices.HandleRef(this, target), out rect);
                //user32.RECT rect2;
                user32.GetClientRect(target, out rect);
                Point p = new Point() ;
                user32.ScreenToClient(target, ref p);
                rect.Top -= p.Y;
                rect.Bottom -= p.Y;
                rect.Left -= p.X;
                rect.Right -= p.X;
                Console.WriteLine(rect + "   " + p);
                user32.ClipCursor(ref rect);
            } else {
                StringBuilder WindowText = new StringBuilder(50);
                user32.GetWindowText(focus, WindowText, WindowText.Capacity);
                StringBuilder ClassName = new StringBuilder(50);
                user32.GetClassName(focus, ClassName, ClassName.Capacity);
                Console.WriteLine("focus: " + WindowText.ToString() + " (" + ClassName + ")" );
                user32.ClipCursor(IntPtr.Zero);
            }
        }
    }
}
