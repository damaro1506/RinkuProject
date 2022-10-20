using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Printing;

namespace Cover.Backend.Printer
{
    public class PrinterBase
    {
        #region "Properties"
        public Int32 fontSize = 8;

        private Font _FontBase;
        private Font _FontBaseBold;
        public Font FontBase
        {
            get
            {
                if (_FontBase == null)
                    _FontBase = new Font("Arial", fontSize);
                return _FontBase;
            }
        }
        public Font FontBaseBold
        {
            get
            {
                if (_FontBaseBold == null)
                    _FontBaseBold = new Font("Arial", fontSize, FontStyle.Bold);
                return _FontBaseBold;
            }
        }

        public Int32 y = 0;
        private Font _font;
        private Font _FontBold;
        public Font font
        {
            get
            {
                if (_font == null)
                    _font = new Font("Arial", 8);
                return _font;
            }
        }
        public Font FontBold
        {
            get
            {
                if (_FontBold == null)
                    _FontBold = new Font("Arial", 8, FontStyle.Bold);
                return _FontBold;
            }
        }

        public float Scale
        {
            get
            {
                return 1f;
            }
        }

        public float X_TicketWidth
        {
            get
            {
                return 270f;
            }
        }

        public float X_Amounts
        {
            get
            {
                return 200f;
            }

        }
        public PrintPageEventArgs PrintPageEvent { get; set; }

        #endregion

        #region "Add_events"

        public void AddLabel(String text, float x, float y, Align align)
        {
            AddLabel(text, font, Brushes.Black, x, y, align);
        }

        public void AddLabelBold(String text, float x, float y, Align align)
        {
            AddLabel(text, FontBold, Brushes.Black, x, y, align);
        }

        public void AddLabel(String text, Font fuente, Brush brush, float x, float y, Align align)
        {
            var format = GetStringFormat(align);
            PrintPageEvent.Graphics.DrawString(text, fuente, brush, x, y, format);
        }

        public void AddLabel(String text, System.Drawing.RectangleF rect, Align align)
        {
            var format = GetStringFormat(align);
            PrintPageEvent.Graphics.DrawString(text, FontBase, Brushes.Black, rect, format);
        }

        public void AddJump(ref int y)
        {
            y += 13;
        }

        public void AddLine(ref int y)
        {
            y += 20;
            PrintPageEvent.Graphics.DrawLine(new System.Drawing.Pen(Brushes.Black), new Point(0, y), new Point((int)X_TicketWidth, y));
            y += 5;
        }

        public void AddImage(Image image, float x, float y, float width, float height)
        {
            PrintPageEvent.Graphics.DrawImage(image, x, y, width, height);
        }

        public void AddTitle(ref int y, String title)
        {
            AddLine(ref y);
            AddLabelBold(title, ((X_TicketWidth * Scale) / 2), y, Align.Center);
            AddLine(ref y);
        }

        #endregion

        public StringFormat GetStringFormat(Align align)
        {
            var stringFormat = new System.Drawing.StringFormat();
            stringFormat.Trimming = StringTrimming.None;
            stringFormat.FormatFlags = StringFormatFlags.NoClip;
            switch (align)
            {
                case Align.Left: stringFormat.Alignment = StringAlignment.Near; break;
                case Align.Center: stringFormat.Alignment = StringAlignment.Center; break;
                case Align.Right: stringFormat.Alignment = StringAlignment.Far; break;
            }
            return stringFormat;
        }

        public enum Align
        {
            Left = 1,
            Center = 2,
            Right = 3
        }

        public String FormatMoney(Decimal amount)
        {
            return amount.ToString("C");
        }

        public void PrintTitle(ref int y, String title)
        {
            AddLine(ref y);
            AddLabelBold(title, ((X_TicketWidth * Scale) / 2), y, Align.Center);
            AddLine(ref y);
        }

       
    }
}
