using System;
using System.Linq;
using System.Windows.Forms;

namespace POS.Internals.Designer.Internal
{
    public class Resizer
    {
        private static readonly int decoration = 1;
        private readonly Control controltobeResized;
        private readonly System.Windows.Forms.PictureBox pictureBox1 = new PictureBox();
        private readonly System.Windows.Forms.PictureBox pictureBox2 = new PictureBox();
        private readonly System.Windows.Forms.PictureBox pictureBox3 = new PictureBox();

        public Resizer(Control theControl)
        {
            this.controltobeResized = theControl;

            this.InitializeComponent();

            this.controltobeResized.Controls.Add(this.pictureBox1);
            this.controltobeResized.Controls.Add(this.pictureBox2);
            this.controltobeResized.Controls.Add(this.pictureBox3);
        }

        public static int Decoration
        {
            get
            {
                return decoration;
            }
        }

        private void InitializeComponent()
        {
            // 
            // pictureBox1
            // 
            this.pictureBox1.Anchor = (System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom |
                                       System.Windows.Forms.AnchorStyles.Right);
            this.pictureBox1.BackColor = System.Drawing.Color.LightGray;
            this.pictureBox1.Cursor = System.Windows.Forms.Cursors.SizeWE;
            this.pictureBox1.Location = new System.Drawing.Point(this.controltobeResized.GetClientWidth(), 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(decoration, this.controltobeResized.GetClientHeight());
            this.pictureBox1.TabIndex = 1;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseMove);
            // 
            // pictureBox2
            // 
            this.pictureBox2.Anchor = (System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left |
                                       System.Windows.Forms.AnchorStyles.Right);
            this.pictureBox2.BackColor = System.Drawing.Color.LightGray;
            this.pictureBox2.Cursor = System.Windows.Forms.Cursors.SizeNS;
            this.pictureBox2.Location = new System.Drawing.Point(0, this.controltobeResized.GetClientHeight());
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(this.controltobeResized.GetClientWidth(), decoration);
            this.pictureBox2.TabIndex = 2;
            this.pictureBox2.TabStop = false;
            this.pictureBox2.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pictureBox2_MouseMove);
            // 
            // pictureBox3
            // 
            this.pictureBox3.Anchor = (System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right);
            this.pictureBox3.BackColor = System.Drawing.Color.LightGray;
            this.pictureBox3.Cursor = System.Windows.Forms.Cursors.SizeNWSE;
            this.pictureBox3.Location = new System.Drawing.Point(this.controltobeResized.GetClientWidth(), this.controltobeResized.GetClientHeight());
            this.pictureBox3.Name = "pictureBox3";
            this.pictureBox3.Size = new System.Drawing.Size(decoration, decoration);
            this.pictureBox3.TabIndex = 3;
            this.pictureBox3.TabStop = false;
            this.pictureBox3.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pictureBox3_MouseMove);
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                this.controltobeResized.Width += e.X;
                if (this.controltobeResized.Width < decoration)
                {
                    this.controltobeResized.Width = decoration;
                }
            }
        }

        private void pictureBox2_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                this.controltobeResized.Height += e.Y;
                if (this.controltobeResized.Height < decoration)
                {
                    this.controltobeResized.Height = decoration;
                }
            }
        }

        private void pictureBox3_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                this.controltobeResized.Width += e.X;
                this.controltobeResized.Height += e.Y;
                if (this.controltobeResized.Width < decoration)
                {
                    this.controltobeResized.Width = decoration;
                }
                if (this.controltobeResized.Height < decoration)
                {
                    this.controltobeResized.Height = decoration;
                }
            }
        }
    }
}