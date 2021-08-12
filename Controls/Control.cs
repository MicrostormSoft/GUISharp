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
        public bool TouchReceiverRegestered { get; private set; }

        public event Action<object, Point> OnPressDown;
        public event Action<object, Point> OnPressMove;
        public event Action<object, Point> OnPressUp;
        public event Action<object, Point> OnPressMoveEnter;
        public event Action<object, Point> OnPressMoveLeave;

        protected TouchScreen Touch;

        public abstract Bitmap Draw(bool drawall = false);

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
            if (touch != null && !TouchReceiverRegestered)
            {
                this.Touch = touch;
                touch.OnPress += Touch_OnPress;
                touch.OnMove += Touch_OnMove;
                touch.OnRelease += Touch_OnRelease;
                TouchReceiverRegestered = true;
            }
        }

        internal void RemoveTouchInputReceiver()
        {
            if (Touch != null && TouchReceiverRegestered)
            {
                Touch.OnPress -= Touch_OnPress;
                Touch.OnMove -= Touch_OnMove;
                Touch.OnRelease -= Touch_OnRelease;
                TouchReceiverRegestered = false;
            }
        }

        //TODO: 将三个原始触摸事件转换为五个封装后的触摸事件
        private bool isInside = false;
        private void Touch_OnRelease(object arg1, TouchScreenSharp.TouchEventArgs arg2)
        {
            var point = new Point(arg2.X, arg2.Y);
            if (this.HitBox.Contains(point))
            {
                OnPressUp?.Invoke(this, point);
                isInside = false;
            }
        }

        private void Touch_OnMove(object arg1, TouchScreenSharp.TouchEventArgs arg2)
        {
            var point = new Point(arg2.X, arg2.Y);
            if (this.HitBox.Contains(point))
            {
                OnPressUp?.Invoke(this, point);
                if (!isInside)
                    OnPressMoveEnter?.Invoke(this, point);
                isInside = true;
            }
            else
            {
                if (isInside)
                    OnPressMoveLeave?.Invoke(this, point);
                isInside = false;
            }
        }

        private void Touch_OnPress(object arg1, TouchScreenSharp.TouchEventArgs arg2)
        {
            var point = new Point(arg2.X, arg2.Y);
            if (this.HitBox.Contains(point))
            {
                OnPressDown?.Invoke(this, point);
                isInside = true;
            }
        }
    }
}
