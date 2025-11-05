// SelectionElement.cs
// User‑control that exposes its inner control properties (image, font, background)
// and automatically resizes itself when the font changes.
// Now propagates MouseClick, MouseEnter and MouseLeave from its inner controls.

using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace RoundComboboxMultiSelect
{
    /// <summary>
    /// A simple selectable element that shows an icon (PictureBox) and text (Label).
    /// Exposes: ImgNotSelected, ImgSelected, IsSelected, LabelBackgroundColor and a live‑linked Font.
    /// Height is adjusted automatically to fit the current Font size.
    /// MouseClick / MouseEnter / MouseLeave events raised even when the user interacts with inner controls.
    /// </summary>
    public partial class SelectionElement : UserControl
    {
        private Image _imgNotSelected;
        private Image _imgSelected;
        private bool _isSelected;

        public SelectionElement()
        {
            // ---------------  Designer ----------------
            pictureBox1 = new PictureBox();
            label1 = new Label();
            ((ISupportInitialize)pictureBox1).BeginInit();
            SuspendLayout();


            // pictureBox1
            pictureBox1.Dock = DockStyle.Left;
            pictureBox1.Width = 24;                         // icon width
            pictureBox1.SizeMode = PictureBoxSizeMode.CenterImage;

            // label1
            label1.Dock = DockStyle.Fill;
            label1.TextAlign = ContentAlignment.MiddleLeft;
            label1.AutoSize = false;

            // this (UserControl)
            AutoScaleMode = AutoScaleMode.None;          // we manage sizing ourselves
            MinimumSize = new Size(48, 0);
            Controls.Add(label1);
            Controls.Add(pictureBox1);

            ((ISupportInitialize)pictureBox1).EndInit();
            ResumeLayout(false);
            // -------------------------------------------

            // Sync inner controls with current properties
            SyncFont();
            AdjustHeight();

            // React to runtime font changes
            FontChanged += (_, __) =>
            {
                SyncFont();
                AdjustHeight();
            };

            // -------------------------------------------------------------
            // Propagate mouse events fired on child controls.
            // -------------------------------------------------------------
            pictureBox1.MouseClick += Child_MouseClick;
            label1.MouseClick += Child_MouseClick;
            pictureBox1.MouseEnter += Child_MouseEnter;
            label1.MouseEnter += Child_MouseEnter;
            pictureBox1.MouseLeave += Child_MouseLeave;
            label1.MouseLeave += Child_MouseLeave;
        }

        //scambio le immagine della box selezionata
        public void ChangeImage()
        {
            if (pictureBox1.Image == _imgSelected)
                pictureBox1.Image = _imgNotSelected;
            else
                pictureBox1.Image = _imgSelected;
        }


        // -----------------------------------------------------------------
        //  Public API
        // -----------------------------------------------------------------

        // queste 2 proprietà delle immagini sono utilizzate dall'oggetto ComboboxMultiselect per modificare l'immagine.

        [Category("Appearance"), Description("Image displayed when the element is NOT selected.")]
        public Image ImgNotSelected
        {
            get => _imgNotSelected;
            set
            {
                _imgNotSelected = value;
                if (!IsSelected)
                    pictureBox1.Image = value;
            }
        }

        [Category("Appearance"), Description("Image displayed when the element IS selected.")]
        public Image ImgSelected
        {
            get => _imgSelected;
            set
            {
                _imgSelected = value;
                if (IsSelected)
                    pictureBox1.Image = value;
            }
        }

        [Category("Appearance"), Description("Background color of the label area.")]
        public Color LabelBackgroundColor
        {
            get => label1.BackColor;
            set => label1.BackColor = value;
        }

        // usiamo questo pezzetto di codice per aggiornare l'immagine automaticamente
        [Category("Behavior"), Description("True if the element is currently selected.")]
        public bool IsSelected
        {
            get => _isSelected;
            set
            {
                _isSelected = value;
                pictureBox1.Image = value ? _imgSelected : _imgNotSelected;
            }
        }

        /// <summary>
        /// Exposes the Font property *and* ensures that label1 uses the same font.
        /// Height is recalculated automatically.
        /// </summary>
        [Browsable(true)]
        public override Font Font
        {
            get => base.Font;
            set
            {
                base.Font = value;
                SyncFont();
                AdjustHeight();
            }
        }

        // -----------------------------------------------------------------
        //  Private helpers
        // -----------------------------------------------------------------

        private void SyncFont()
        {
            label1.Font = Font;               // keep label synchronous with control
        }

        private void AdjustHeight()
        {
            // A small padding above/below the text so glyphs don’t touch the border
            const int padding = 0;
            Height = label1.PreferredHeight + padding * 2;
            pictureBox1.Height = Height;      // keep icon stretched vertically
        }

        // -----------------------------------------------------------------
        //  Child‑to‑parent mouse event propagation
        // -----------------------------------------------------------------

        private void Child_MouseClick(object sender, MouseEventArgs e)
        {
            // Convert child coordinates to control coordinates for consistency
            var clientPoint = PointToClient(((Control)sender).PointToScreen(e.Location));
            var args = new MouseEventArgs(e.Button, e.Clicks, clientPoint.X, clientPoint.Y, e.Delta);
            OnMouseClick(args);               // raises MouseClick event for SelectionElement
        }

        private void Child_MouseEnter(object sender, EventArgs e)
        {
            OnMouseEnter(e);                  // raises MouseEnter event for SelectionElement
        }

        private void Child_MouseLeave(object sender, EventArgs e)
        {
            OnMouseLeave(e);                  // raises MouseLeave event for SelectionElement
        }

        // -----------------------------------------------------------------
        //  Public text accessors (optional convenience)
        // -----------------------------------------------------------------

        [Category("Appearance"), Description("Text displayed inside the element."),
         Browsable(true)]
        public override string Text
        {
            get => label1.Text;
            set => label1.Text = value;
        }


    }
}
