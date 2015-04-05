using System;
using System.Windows.Forms;
using System.Xaml;

namespace POS.Internals.Designer
{
    public partial class DesignerHost : UserControl
    {
        private readonly Control parent;

        public event EventHandler SelectionChanged;

        private RuntimeDesigner rd;

        private DesignerHost(Control parent)
        {
            this.parent = parent;
            InitializeComponent();

            rd = new RuntimeDesigner();
        }

        public void AddControl(Control c)
        {
            if (SelectionChanged != null)
            {
                SelectionChanged(c, null);
            }

            rd.EnableMoveResize(c);

            parent.Controls.Add(c);
        }


        public string GetDefinition()
        {
            return XamlServices.Save(parent);   
        }

        public static DesignerHost CreateHost(string definition, DragEventHandler dragdrop, EventHandler selected)
        {
            return CreateHost((Control)XamlServices.Parse(definition), dragdrop, selected);
        }

        public static DesignerHost CreateHost(Control parent, DragEventHandler dragdrop, EventHandler selected)
        {
            var n = new DesignerHost(parent);

            n.Dock = DockStyle.Fill;

            if (selected != null)
            {
                n.SelectionChanged += selected;
                selected(parent, null);
            }

            parent.DragOver += (sender, e) => { e.Effect = DragDropEffects.All; };
            parent.DragDrop += (sender, e) => {
                dragdrop(sender, e);
            };

            parent.AllowDrop = true;

            n.Controls.Add(parent);
            n.rd.EnableResizing(parent);

            return n;
        }
    }
}
