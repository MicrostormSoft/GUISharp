using DisplaySharp;
using GUISharp.Controls;
using System;
using System.Drawing;
using System.Threading;
using TouchScreenSharp;

namespace GUISharp
{
    public class InteractiveScreen
    {
        private TouchScreen touch;
        private Canvas canvas;
        private Thread Refresher;

        public Page CurrentPage;
        public bool UseParticularRefresh = true;

        public InteractiveScreen(string display = "/dev/dri/card0", string touchdev = "/dev/input/event1", bool autorefresh = true)
        {
            canvas = new Canvas(display);
            touch = new TouchScreen(touchdev);
            Refresher = new Thread(new ThreadStart(() =>
            {
                while (true)
                {
                    Refresh(UseParticularRefresh);
                    Thread.Sleep(0);
                }
            }));
            if (autorefresh)
            {
                Refresher.Start();
            }
        }

        public void ShowPage(Page page)
        {
            if (page.DrawingSize.Width != canvas.Width ||
                page.DrawingSize.Height != canvas.Height)
            {
                //throw new InvalidOperationException("Page should always be the same size as canvas.");
            }
            if (CurrentPage != null)
            {
                CurrentPage.RemoveTouchInputReceiver();
                CurrentPage.OnPageSwitchedOut(this, page);
            }
            canvas.Clear(page.Background);
            canvas.DrawBitmap(page.Draw());
            page.RegTouchInputReceiver(touch);
            page.OnPageSwitchedIn(this, CurrentPage);
            CurrentPage = page;
        }

        public void Refresh(bool particular = true)
        {
            if (CurrentPage == null)
                return;
            if (CurrentPage.Changed)
            {
                CurrentPage.Draw(!particular);
                if (particular)
                {
                    var image = CutImage(CurrentPage.Appearance, CurrentPage.ChangedArea);
                    canvas.DrawBitmap(image, CurrentPage.ChangedArea);
                }
                else
                {
                    canvas.DrawBitmap(CurrentPage.Appearance);
                }
            }
        }

        public Bitmap CutImage(Bitmap map, Rectangle area)
        {
            Bitmap nmap = new Bitmap(area.Width, area.Height);
            var g = Graphics.FromImage(nmap);
            g.DrawImage(map, area, area, GraphicsUnit.Pixel);
            return nmap;
        }
    }
}
