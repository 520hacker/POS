using System;
using System.Windows.Forms;
using System.Xaml;

namespace POS.Internals.Designer
{
    public partial class DesignerHost : UserControl
    {
        public Control Parent { get; set; }

        public event EventHandler SelectionChanged;

        private RuntimeDesigner rd;

        public DesignerHost()
        {
            InitializeComponent();

            rd = new RuntimeDesigner();
        }

        public DesignerHost(Control parent)
        {
            this.Parent = parent;

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

            Parent.Controls.Add(c);
        }


        public string GetDefinition()
        {
            return XamlServices.Save(Parent);   
        }

        public static DesignerHost CreateHost(string definition, DragEventHandler dragdrop, EventHandler selected)
        {
            return CreateHost((Control)XamlServices.Parse(definition), dragdrop, selected);
        }

        public static DesignerHost CreateHost(Control parent, DragEventHandler dragdrop, EventHandler selected, bool moveparent = true)
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
