using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace RoundComboboxTest
{
    public class RoundPanel : Panel
    {
        // Fields
        private int borderRadius;
        private float gradientAngle = 90F;
        private Color gradientTopColor = Color.White;
        private Color gradientBottomColor = Color.DarkRed;
        private int borderSize;            // Nuova proprietà per spessore bordo
        private Color borderColor; // Nuova proprietà per colore bordo

        public RoundPanel()
        {
            this.BackColor = Color.White;
            this.ForeColor = Color.Black;
            this.Size = new Size(300, 220);
        }

        // Properties
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public int BorderRadius
        {
            get => borderRadius;
            set
            {
                borderRadius = value;
                this.Invalidate();
            }
        }

        public float GradientAngle
        {
            get => gradientAngle;
            set
            {
                gradientAngle = value;
                this.Invalidate();
            }
        }

        public Color GradientTopColor
        {
            get => gradientTopColor;
            set
            {
                gradientTopColor = value;
                this.Invalidate();
            }
        }

        public Color GradientBottomColor
        {
            get => gradientBottomColor;
            set
            {
                gradientBottomColor = value;
                this.Invalidate();
            }
        }
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public int BorderSize
        {
            get => borderSize;
            set
            {
                borderSize = value < 0 ? 0 : value;
                this.Invalidate();
            }
        }

        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public Color BorderColor
        {
            get => borderColor;
            set
            {
                borderColor = value;
                this.Invalidate();
            }
        }


        // Methods
        private GraphicsPath GetRectanglePath(RectangleF rectangle, float radius)
        {
            GraphicsPath graphicsPath = new GraphicsPath();
            graphicsPath.StartFigure();
            graphicsPath.AddArc(rectangle.Width - radius, rectangle.Height - radius, radius, radius, 0, 90);
            graphicsPath.AddArc(rectangle.X, rectangle.Height - radius, radius, radius, 90, 90);
            graphicsPath.AddArc(rectangle.X, rectangle.Y, radius, radius, 180, 90);
            graphicsPath.AddArc(rectangle.Width - radius, rectangle.Y, radius, radius, 270, 90);
            graphicsPath.CloseFigure();
            return graphicsPath;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            if (this.Width <= 0 || this.Height <= 0) return;

            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            // Disegno il gradiente su tutta la superficie (senza tener conto del bordo)
            RectangleF rectSurface = new RectangleF(0, 0, this.Width, this.Height);

            using (LinearGradientBrush brushPanel = new LinearGradientBrush(
                rectSurface, this.GradientTopColor, this.GradientBottomColor, this.gradientAngle))
            {
                if (borderRadius > 2)
                {
                    using (GraphicsPath pathSurface = GetRectanglePath(rectSurface, borderRadius))
                    {
                        e.Graphics.FillPath(brushPanel, pathSurface);
                    }
                }
                else
                {
                    e.Graphics.FillRectangle(brushPanel, rectSurface);
                }
            }

            // Regione e bordo: il bordo si disegna con un rettangolo rientrato di metà spessore del bordo,
            // così il bordo non esce fuori dal pannello, ma il gradiente rimane a tutta larghezza
            if (borderRadius > 2)
            {
                RectangleF rectBorder = new RectangleF(
                    borderSize / 2f,
                    borderSize / 2f,
                    this.Width ,
                    this.Height );

                using (GraphicsPath pathBorder = GetRectanglePath(rectBorder, borderRadius - borderSize / 2f))
                {
                    this.Region = new Region(pathBorder);

                    if (borderSize > 0)
                    {
                        using (Pen penBorder = new Pen(borderColor, borderSize))
                        {
                            penBorder.Alignment = PenAlignment.Center;
                            e.Graphics.DrawPath(penBorder, pathBorder);
                        }
                    }
                }
            }
            else
            {
                this.Region = new Region(rectSurface);
                if (borderSize > 0)
                {
                    using (Pen penBorder = new Pen(borderColor, borderSize))
                    {
                        penBorder.Alignment = PenAlignment.Center;
                        e.Graphics.DrawRectangle(penBorder, borderSize / 2, borderSize / 2, this.Width - borderSize, this.Height - borderSize);
                    }
                }
            }
        }

    }
}