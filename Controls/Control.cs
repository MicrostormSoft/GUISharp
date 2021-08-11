using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TouchScreenSharp;

namespace GUISharp.Controls
{
    public abstract class Control
    {
        public Rectangle HitBox;
        public Rectangle DrawingSize;
        public Rectangle ChangedArea;
        public Color Background;
        public Color Foreground;
        public Bitmap Appearance;
        public Graphics Graphics;
        public bool Changed;

        public event Action<object, Point> OnPressDown;
        public event Action<object, Point> OnPressMove;
        public event Action<object, Point> OnPressUp;
        public event Action<object, Point> OnPressMoveEnter;
        public event Action<object, Point> OnPressMoveLeave;

        protected TouchScreen Touch;

        public abstract Bitmap Draw(bool drawall=false);

        public Control(int width, int height, Color background, Color foreground)
        {
            InitCanvas(width, height, background, foreground);
        }

        public Control(int width, int height)
        {
            InitCanvas(width, height, Color.White, Color.Black);
        }

        private void InitCanvas(int width, int height, Color background, Color foreground)
        {
            Background = background;
            Appearance = new Bitmap(DrawingSize.Width, DrawingSize.Height);
            Graphics = Graphics.FromImage(Appearance);
            Graphics.Clear(background);
        }

        public void ExpandChangedArea(Rectangle area)
        {
            if (area.X < ChangedArea.X)
            {
                int rightbd = ChangedArea.Right;
                ChangedArea.X = area.Location.X;
                ChangedArea.Width = rightbd - ChangedArea.X;
            }
            if (area.Y < ChangedArea.Y)
            {
                int bottonbd = ChangedArea.Bottom;
                ChangedArea.Y = area.Location.Y;
                ChangedArea.Width = bottonbd - ChangedArea.Y;
            }

            if (area.Right > ChangedArea.Right)
                ChangedArea.Width = area.Right - ChangedArea.X;
            if (area.Bottom > ChangedArea.Bottom)
                ChangedArea.Height = area.Bottom - ChangedArea.Y;
        }

        internal void RegTouchInputReceiver(TouchScreenSharp.TouchScreen touch)
        {
            this.touch = touch;
            touch.OnPress += Touch_OnPress;
            touch.OnMove += Touch_OnMove;
            touch.OnRelease += Touch_OnRelease;
        }

        internal void RemoveTouchInputReceiver()
        {
            touch.OnPress -= Touch_OnPress;
            touch.OnMove -= Touch_OnMove;
            touch.OnRelease -= Touch_OnRelease;
        }

        private void Touch_OnRelease(object arg1, TouchScreenSharp.TouchEventArgs arg2)
        {

        }

        private void Touch_OnMove(object arg1, TouchScreenSharp.TouchEventArgs arg2)
        {

        }

        private void Touch_OnPress(object arg1, TouchScreenSharp.TouchEventArgs arg2)
        {

        }
    }
}
