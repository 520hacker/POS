using System;
using System.Drawing;

namespace POS.UI
{
    public class SevenSegmentHelper
    {
        private readonly Graphics _graphics;

        // Indicates what segments are illuminated for all 10 digits
        private static readonly byte[,] _segmentData =
        {
            { 1, 1, 1, 0, 1, 1, 1 },
            { 0, 0, 1, 0, 0, 1, 0 },
            { 1, 0, 1, 1, 1, 0, 1 },
            { 1, 0, 1, 1, 0, 1, 1 },
            { 0, 1, 1, 1, 0, 1, 0 },
            { 1, 1, 0, 1, 0, 1, 1 },
            { 1, 1, 0, 1, 1, 1, 1 },
            { 1, 0, 1, 0, 0, 1, 0 },
            { 1, 1, 1, 1, 1, 1, 1 },
            { 1, 1, 1, 1, 0, 1, 1 }
        }; 

        // Points that define each of the seven segments
        private readonly Point[][] _segmentPoints = new Point[7][];

        public SevenSegmentHelper(Graphics graphics)
        {
            this._graphics = graphics;
            this._segmentPoints[0] = new Point[] { new Point(3, 2), new Point(39, 2), new Point(31, 10), new Point(11, 10) };
            this._segmentPoints[1] = new Point[] { new Point(2, 3), new Point(10, 11), new Point(10, 31), new Point(2, 35) };
            this._segmentPoints[2] = new Point[] { new Point(40, 3), new Point(40, 35), new Point(32, 31), new Point(32, 11) };
            this._segmentPoints[3] = new Point[] { new Point(3, 36), new Point(11, 32), new Point(31, 32), new Point(39, 36), new Point(31, 40), new Point(11, 40) };
            this._segmentPoints[4] = new Point[] { new Point(2, 37), new Point(10, 41), new Point(10, 61), new Point(2, 69) };
            this._segmentPoints[5] = new Point[] { new Point(40, 37), new Point(40, 69), new Point(32, 61), new Point(32, 41) };
            this._segmentPoints[6] = new Point[] { new Point(11, 62), new Point(31, 62), new Point(39, 70), new Point(3, 70) };
        }

        public SizeF GetStringSize(string text, Font font)
        {
            SizeF sizef = new SizeF(0, this._graphics.DpiX * font.SizeInPoints / 72);

            for (int i = 0; i < text.Length; i++)
            {
                if (Char.IsDigit(text[i]))
                {
                    sizef.Width += 42 * this._graphics.DpiX * font.SizeInPoints / 72 / 72;
                }
                else if (text[i] == ':' || text[i] == '.')
                {
                    sizef.Width += 12 * this._graphics.DpiX * font.SizeInPoints / 72 / 72;
                }
            }
            return sizef;
        }

        public void DrawDigits(string text, Font font, Brush brush, Brush brushLight, float x, float y)
        {
            for (int cnt = 0; cnt < text.Length; cnt++)
            {
                // For digits 0-9
                if (Char.IsDigit(text[cnt]))
                {
                    x = this.DrawDigit(text[cnt] - '0', font, brush, brushLight, x, y);
                }
                // For colon :
                else if (text[cnt] == ':')
                {
                    x = this.DrawColon(font, brush, x, y);
                }
                // For dot .
                else if (text[cnt] == '.')
                {
                    x = this.DrawDot(font, brush, x, y);
                }
            }
        }

        private float DrawDigit(int num, Font font, Brush brush, Brush brushLight, float x, float y)
        {
            for (int cnt = 0; cnt < this._segmentPoints.Length; cnt++)
            {
                if (_segmentData[num, cnt] == 1)
                {
                    this.FillPolygon(_segmentPoints[cnt], font, brush, x, y);
                }
                else
                {
                    this.FillPolygon(_segmentPoints[cnt], font, brushLight, x, y);
                }
            }
            return x + 42 * this._graphics.DpiX * font.SizeInPoints / 72 / 72;
        }

        private float DrawDot(Font font, Brush brush, float x, float y)
        {
            Point[][] dotPoints = new Point[1][];

            dotPoints[0] = new Point[]
            {
                new Point(2, 64), new Point(6, 61),
                new Point(10, 64), new Point(6, 69)
            };

            for (int cnt = 0; cnt < dotPoints.Length; cnt++)
            {
                this.FillPolygon(dotPoints[cnt], font, brush, x, y);
            }
            return x + 12 * this._graphics.DpiX * font.SizeInPoints / 72 / 72;
        }

        private float DrawColon(Font font, Brush brush, float x, float y)
        {
            Point[][] colonPoints = new Point[2][];

            colonPoints[0] = new Point[] { new Point(2, 21), new Point(6, 17), new Point(10, 21), new Point(6, 25) };
            colonPoints[1] = new Point[] { new Point(2, 51), new Point(6, 47), new Point(10, 51), new Point(6, 55) };

            for (int cnt = 0; cnt < colonPoints.Length; cnt++)
            {
                this.FillPolygon(colonPoints[cnt], font, brush, x, y);
            }
            return x + 12 * this._graphics.DpiX * font.SizeInPoints / 72 / 72;
        }

        private void FillPolygon(Point[] polygonPoints, Font font, Brush brush, float x, float y)
        {
            PointF[] polygonPointsF = new PointF[polygonPoints.Length];

            for (int cnt = 0; cnt < polygonPoints.Length; cnt++)
            {
                polygonPointsF[cnt].X = x + polygonPoints[cnt].X * this._graphics.DpiX * font.SizeInPoints / 72 / 72;
                polygonPointsF[cnt].Y = y + polygonPoints[cnt].Y * this._graphics.DpiY * font.SizeInPoints / 72 / 72;
            }
            this._graphics.FillPolygon(brush, polygonPointsF);
        }
    }
}