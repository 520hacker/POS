using System;
using System.Windows.Forms;
using POS.Internals.Designer.Internal;

namespace POS.Internals.Designer
{
    public class RuntimeDesigner
    {
        public Control SelectedControl { get; set; }

        public event EventHandler SelectionChanged;

        public void EnableResizing(Control c)
        {
            c.Click += (sender, e) =>
            {
                if (SelectionChanged != null)
                {
                    SelectedControl = (Control)sender;
                    SelectionChanged(sender, e);
                }
            };

            new Resizer(c);
        }

        public void EnableMoveResize(Control c)
        {
            c.Click += (sender, e) => {
                if (SelectionChanged != null)
                {
                    SelectedControl = (Control)sender;
                    SelectionChanged(sender, e);
                }
                    };

            ControlMover.Init(c, ControlMover.Direction.Any);
            new Resizer(c);
        }
    }
}