using System;
using System.Drawing;
using System.Windows.Forms;
using POS.Internals.Designer;
using POS.Internals.FilterBuilder;
using POS.Internals.UndoRedo;
using POS.Properties;
using Telerik.WinControls.UI;

namespace POS
{
    public partial class MainForm : Telerik.WinControls.UI.RadForm
    {
        DesignerHost host;

        public MainForm()
        {
            InitializeComponent();

            var f = new Panel();
            f.BorderStyle = BorderStyle.FixedSingle;
            f.Visible = true;

            var undoBtn = new RadButtonElement();
            undoBtn.Image = Resources.Undo_icon;
            undoBtn.Name = "undoBtn";
            undoBtn.ShowBorder = false;
            undoBtn.Enabled = false;
            undoBtn.Click += (s, e) => UndoRedoManager.Undo();

            var redoBtn = new RadButtonElement();
            redoBtn.Image = Resources.Redo_icon;
            redoBtn.Name = "redoBtn";
            redoBtn.ShowBorder = false;
            redoBtn.Enabled = false;
            redoBtn.Click += (s, e) => UndoRedoManager.Redo();

            UndoRedoManager.CommandDone += (s, e) =>
            {
                undoBtn.Enabled = UndoRedoManager.CanUndo;
                redoBtn.Enabled = UndoRedoManager.CanRedo;
            };

            this.FormElement.TitleBar.SystemButtons.Children.Insert(0, undoBtn);
            this.FormElement.TitleBar.SystemButtons.Children.Insert(1, redoBtn);

            host = DesignerHost.CreateHost(f, null, (sender, e) => { propertyGrid.SelectedObject = sender; }, false);

            host.AddControl(new Button());

            radPanel1.Controls.Add(host);

            #if DEBUG
            Cursor.Show();
            #else
            Cursor.Hide();
            #endif

        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

        private void PropertiesBtn_Click(object sender, EventArgs e)
        {
            propertyGrid.Visible = !propertyGrid.Visible;
        }

        private void ImageBtn_Click(object sender, EventArgs e)
        {
            var fb = new FilterBuilder();
            fb.Add(FilterBuilder.Filters.AllImageFiles);

            OpenFileDialog f = new OpenFileDialog();
            f.Filter = fb.ToString();

            if (f.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                var img = Image.FromFile(f.FileName);
                var pb = new PictureBox();
                pb.Image = img;

                host.AddControl(pb);
            }
        }

        private void ProductsView_Click(object sender, EventArgs e)
        {

        }
    }
}