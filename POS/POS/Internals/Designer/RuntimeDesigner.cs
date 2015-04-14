using System;
using System.Windows.Forms;
using POS.Internals.Designer.Internal;

namespace POS.Internals.Designer
{
    public class RuntimeDesigner
    {
        public event EventHandler SelectionChanged;

        public Control SelectedControl { get; set; }

        public void EnableResizing(Control c)
        {
            c.Click += (sender, e) =>
            {
                if (this.SelectionChanged != null)
                {
                    this.SelectedControl = (Control)sender;
                    this.SelectionChanged(sender, e);
                }
            };

            new Resizer(c);
        }

        public void EnableMoveResize(Control c)
        {
            c.Click += (sender, e) =>
            {
                if (this.SelectionChanged != null)
                {
                    this.SelectedControl = (Control)sender;
                    this.SelectionChanged(sender, e);
                }
            };

            ControlMover.Init(c, ControlMover.Direction.Any);
            new Resizer(c);
        }
    }
}