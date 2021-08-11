using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUISharp.Controls
{
    public abstract class Containers : Control
    {
        protected List<Control> Childs;
        public new bool Changed
        {
            get
            {
                if (changed) return true;
                foreach (Control c in Childs)
                    if (c.Changed) return true;
                return false;
            }//Check if all the childs are unchanged.
            set
            {
                changed = value;
            }//Set self changed.
        }

        private bool changed = false;

        public Containers(int width, int height, Color background, Color foreground) : base(width, height, background, foreground)
        {
            Childs = new List<Control>();
        }

        public Containers(int width, int height) : base(width, height)
        {
            Childs = new List<Control>();
        }

        public void AddChild(Control control)
        {
            Childs.Add(control);
            control.RegTouchInputReceiver(Touch);
        }

        public override Bitmap Draw(bool drawall = false)
        {
            ChangedArea = Rectangle.Empty;
            if (drawall)
            {
                foreach (Control c in Childs)
                {
                    Graphics.DrawImage(c.Draw(), c.DrawingSize);
                    ExpandChangedArea(c.ChangedArea);
                }
                Changed = false;
            }
            else
            if (Changed)
            {
                foreach (Control c in Childs)
                {
                    if (c.Changed)
                    {
                        Graphics.DrawImage(c.Draw(), c.DrawingSize);
                        ExpandChangedArea(c.ChangedArea);
                        ReDrawAffected(c, ListWithout(Childs, c));
                    }
                }
                Changed = false;
            }
            return Appearance;
        }

        public void ReDrawAffected(Control control, List<Control> checklist)
        {
            foreach (Control c in checklist)
            {
                if (c == control) continue;
                if (!Rectangle.Intersect(c.DrawingSize, control.DrawingSize).IsEmpty)
                {
                    Graphics.DrawImage(c.Draw(), c.DrawingSize);
                    ExpandChangedArea(c.ChangedArea);
                    ReDrawAffected(c, ListWithout(checklist, c));
                }
            }
        }

        public List<Control> ListWithout(List<Control> list, Control exception)
        {
            List<Control> chklist = new List<Control>(list);
            chklist.Remove(exception);
            return chklist;
        }
    }
}
