using System;
using System.Drawing;
using System.Windows.Forms;

namespace POS.Internals.UI
{
    public class HistoryView : Control
    {
        protected override void OnPaint(PaintEventArgs e)
        {
            var gr = e.Graphics;

            PointF old = new PointF(5, 5);
            foreach (var s in ServiceLocator.ProductHistory.GetLastTwo())
            {
                var newP = new PointF(old.X + 15, old.Y + 15);

                gr.DrawString(s.ID, Font, Brushes.Black, newP);

                old = newP;
            }
        }
    }
}