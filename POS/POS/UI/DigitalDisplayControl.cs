using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace POS.UI
{
    public partial class DigitalDisplayControl : UserControl
    {
        private Color _digitColor = Color.GreenYellow;
        private string _digitText = "88.88";

        public DigitalDisplayControl()
        {
            this.SetStyle(ControlStyles.DoubleBuffer, true);
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.ResizeRedraw, true);
            this.SetStyle(ControlStyles.UserPaint, true);
            this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            this.InitializeComponent();
            this.BackColor = Color.Transparent;
        }

        [Browsable(true), DefaultValue("Color.GreenYellow")]
        public Color DigitColor
        {
            get
            {
                return this._digitColor;
            }
            set
            {
                this._digitColor = value;
                this.Invalidate();
            }
        }

        [Browsable(true), DefaultValue("88.88")]
        public string DigitText
        {
            get
            {
                return this._digitText;
            }
            set
            {
                this._digitText = value;
                this.Invalidate();
            }
        }

        private void DigitalGauge_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            SevenSegmentHelper sevenSegmentHelper = new SevenSegmentHelper(e.Graphics);

            SizeF digitSizeF = sevenSegmentHelper.GetStringSize(this._digitText, this.Font);
            float scaleFactor = Math.Min(this.ClientSize.Width / digitSizeF.Width, this.ClientSize.Height / digitSizeF.Height);
            Font font = new Font(this.Font.FontFamily, scaleFactor * this.Font.SizeInPoints);
            digitSizeF = sevenSegmentHelper.GetStringSize(this._digitText, font);

            using (SolidBrush brush = new SolidBrush(this._digitColor))
            {
                using (SolidBrush lightBrush = new SolidBrush(Color.FromArgb(20, this._digitColor)))
                {
                    sevenSegmentHelper.DrawDigits(
                        this._digitText, font, brush, lightBrush,
                        (this.ClientSize.Width - digitSizeF.Width) / 2,
                        (this.ClientSize.Height - digitSizeF.Height) / 2);
                }
            }
        }
    }
}