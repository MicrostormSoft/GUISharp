using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUISharp.Controls
{
    public abstract class Page : Containers
    {
        public Page(int width, int height, Color background, Color foreground) : base(width, height) { }
        public Page(int width, int height) : base(width, height) { }

        /// <summary>
        /// Another page is swaping out this page on display.
        /// </summary>
        /// <param name="sender">Usually instance of InteractiveScreen, the manager of the display.</param>
        /// <param name="next">The page that swaping out this page.</param>
        public abstract void OnPageSwitchedOut(object sender, Page next);

        /// <summary>
        /// This page is swaping into the display.
        /// </summary>
        /// <param name="sender">Usually instance of InteractiveScreen, the manager of the display.</param>
        /// <param name="previous">The page swapped out, if exists. null otherway.</param>
        public abstract void OnPageSwitchedIn(object sender, Page previous);
    }
}
