using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace RoundComboboxTest
{
    [Designer(typeof(RoundComboboxDesigner))] //-> designer personalizzato per bloccare la modifica dell'altezza
    public partial class RoundCombobox : UserControl, IMessageFilter
    {
        // ===============================
        //  Evento SelectedIndexChanged
        // ===============================
        public event EventHandler SelectedIndexChanged;

        // ===============================
        //  Proprietà di selezione
        // ===============================
        private string _currentSelected = string.Empty;
        private int _selectedIndex = -1;

        [Browsable(false)]
        public string SelectedItem
        {
            get => _currentSelected;
            private set
            {
                if (_currentSelected == value) return;

                _currentSelected = value;
                _selectedIndex = (Data != null && value != null)
                                   ? Array.IndexOf(Data, value)
                                   : -1;

                // Aggiorna la TextBox PRIMA di emettere l'evento
                SearchBlock = true;
                Text = value ?? string.Empty;

                OnSelectedIndexChanged(EventArgs.Empty);
            }
        }

        [Browsable(false)]
        public int SelectedIndex
        {
            get { return _selectedIndex; }
            set
            {
                if (value == _selectedIndex)
                    return;

                if (Data == null)
                    throw new InvalidOperationException("DataSource è null.");

                if (value < -1 || value >= Data.Length)
                    throw new ArgumentOutOfRangeException("SelectedIndex");

                if (value == -1)
                {
                    // Deseleziona
                    SelectedItem = string.Empty; // setter scatena l'evento.
                }
                else
                {
                    // Imposta elemento corrispondente; setter scatenerà l'evento.
                    SelectedItem = Data[value];
                }
            }
        }

        protected virtual void OnSelectedIndexChanged(EventArgs e)
        {
            var handler = SelectedIndexChanged;
            if (handler != null)
                handler(this, e);
        }

        // ===============================
        //  Campi già esistenti
        // ===============================
        private int _maxDimension = 200; // valore default
        private Size _closedSize;
        private bool SearchBlock = false;
        private Color _LabelSelectedColor = Color.Gray;
        private Color _LabelOnHoverColor = Color.Gray;
        private Color _LabelBackColor = Color.Gray;
        private Font _LabelFont = new Font("Century Gothic", 11.25f);

        private static readonly Image _defaultArrow =
            File.Exists(Path.Combine(Application.StartupPath, "images", "FrecciaGiu.png"))
            ? Image.FromFile(Path.Combine(Application.StartupPath, "images", "FrecciaGiu.png"))
            : null;

        // ===============================
        //  PROPRIETÀ DI ESPOSIZIONE
        // ===============================
        [Browsable(true)]
        [EditorBrowsable(EditorBrowsableState.Always)]
        public override string Text
        {
            get { return txtCombobox.Text; }
            set { txtCombobox.Text = value; }
        }

        [Browsable(false)]
        public string[] Items
        {
            get
            {
                return FLPItem.Controls
                    .OfType<Label>()
                    .Select(l => l.Text)
                    .ToArray();
            }
        }


        [Browsable(true)]
        public new int Width
        {
            get { return base.Width; }
            set { base.Width = value; }
        }
        // ALTEZZA NON MODIFICABILE se non modificando il FONT
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public new int Height
        {
            get { return base.Height; }
            set
            {
                // Solo se non siamo in design‑time
                if (!DesignMode)
                    base.Height = _closedSize.Height;
            }
        }


        // Questa variabile contiene gli elementi della RoundCombobox.
        private int i = 0;
        public string[] Data;
        public string[] DataSource
        {
            get { return Data; }
            set
            {
                Data = value ?? new string[0];
                FLPItem.Controls.Clear();

                foreach (var item in Data)
                    AddItemToPanel(item);

                Invalidate();
            }
        }


        [Browsable(true)]
        [Category("Appearance")]
        [Description("Imposta il raggio stondatura bordo pannelli")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public int Radius
        {
            get => PnlItem.BorderRadius;
            set { PnlItem.BorderRadius = value; Invalidate(); }
        }

        [Browsable(true)]
        [Category("Appearance")]
        [Description("Imposta la dimensione del Bordo dei pannelli")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public int BorderSize
        {
            get => PnlItem.BorderSize;
            set { PnlItem.BorderSize = value; Invalidate(); }
        }

        [Browsable(true)]
        [Category("Appearance")]
        [Description("Imposta il Colore del Bordo dei pannelli")]
        [DefaultValue(typeof(Color), "White")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public Color BorderColor
        {
            get => PnlItem.BorderColor;
            set { PnlItem.BorderColor = value; Invalidate(); }
        }

        [Browsable(true)]
        [Category("Appearance")]
        [Description("Icona mostrata nella freccia della combobox")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public Image IconImage
        {
            get => PicCombobox.Image;
            set => PicCombobox.Image = value;
        }

        [Browsable(true)]
        [Category("Appearance")]
        [Description("Imposta il colore di sfondo dei pannelli, tetbox e label")]
        [DefaultValue(typeof(Color), "LightGray")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public Color PanelsBackgroundColor
        {
            get => PnlTitle.BackColor;
            set
            {
                PnlItem.BackColor = value;
                PnlTitle.BackColor = value;
                txtCombobox.BackColor = value;
                _LabelBackColor = value;
                Invalidate();
            }
        }

        [Browsable(true)]
        [Category("Appearance")]
        [Description("Imposta il colore del label selezionato (CurrentSelected)")]
        [DefaultValue(typeof(Color), "Gray")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public Color LabelSelectedColor
        {
            get => _LabelSelectedColor;
            set { _LabelSelectedColor = value; Invalidate(); }
        }

        [Browsable(true)]
        [Category("Appearance")]
        [Description("Imposta il colore del label in Hover")]
        [DefaultValue(typeof(Color), "LightGray")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public Color LabelOnHoverColor
        {
            get => _LabelOnHoverColor;
            set { _LabelOnHoverColor = value; Invalidate(); }
        }

        [Browsable(true)]
        [Category("Appearance")]
        [Description("Altezza della tendina che si apre in pixel")]
        [DefaultValue(typeof(int), "200")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public int MaxDimension
        {
            get => _maxDimension;
            set
            {
                if (_maxDimension != value)
                {
                    _maxDimension = value;
                    Invalidate(); // forza il ridisegno del controllo
                }
            }
        }

        [Browsable(true)]
        [Category("Appearance")]
        [Description("Font usato per gli item della tendina")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public Font LabelFont
        {
            get => _LabelFont;
            set { _LabelFont = value; Invalidate(); }
        }

        // ===============================================
        //  COSTRUTTORE
        // ===============================================
        public RoundCombobox()
        {
            InitializeComponent();
            Data = new string[0];

            if (!LicenseManager.UsageMode.Equals(LicenseUsageMode.Designtime) &&
                _defaultArrow != null)
            {
                IconImage = (Image)_defaultArrow.Clone();
            }

            PanelsBackgroundColor = Color.LightGray;
        }
        // ===============================================
        //  METODI PUBBLICI PRINCIPALI
        // ===============================================
        public void Add(string item)
        {
            if (string.IsNullOrWhiteSpace(item))
                return;

            // Aggiunge l'item alla lista esistente
            if (Data == null)
                Data = new[] { item };
            else
                Data = Data.Concat(new[] { item }).ToArray();

            AddItemToPanel(item);
        }


        public void Clear()
        {
            Data = new string[0]; // Svuota l'array
            FLPItem.Controls.Clear();
            SearchBlock = true;
            txtCombobox.Clear();
            SelectedItem = string.Empty;
        }


        // ===============================================
        //  GESTIONE SELEZIONE LABEL
        // ===============================================
        private void Label_Selected(object sender, MouseEventArgs e)
        {
            var label = (Label)sender;
            SelectedItem = label.Text; // Aggiorna SelectedItem (che scatena l'evento)

            // Aggiorna colori
            foreach (Label lbl in FLPItem.Controls.OfType<Label>())
            {
                lbl.BackColor = lbl.Text == SelectedItem ? _LabelSelectedColor : _LabelBackColor;
            }

            i = 0;
            ChiudiTendina();
        }

        // ===============================================
        //  EVENTI UI VARI (clic, testo ecc.)
        // ===============================================
        private void PicCombobox_Click(object sender, EventArgs e)
        {
            i = 0;
            if (isTendinaChiusa())
            {
                FLPItem.Controls.Clear();
                LoadUnfilteredData();
                Apritendina();
            }
            else
            {
                ChiudiTendina();
            }
        }

        // Apertura graduale
        private void TimerItem_Tick(object sender, EventArgs e)
        {
            if (LicenseManager.UsageMode == LicenseUsageMode.Designtime)
                return;

            if (Height > 0 && Height < _maxDimension - 5)
            {
                SetControlHeight(Height + i);
                i += 5;
            }
            else
            {
                TimerItem.Stop();
            }

            PnlItem.BorderRadius = this.Radius;
        }

        private void txtCombobox_TextChanged(object sender, EventArgs e)
        {
            if (SearchBlock)
            {
                SearchBlock = false;
                return;
            }

            string filter = txtCombobox.Text.ToLower();
            FLPItem.Controls.Clear();

            if (string.IsNullOrWhiteSpace(filter))
            {
                LoadUnfilteredData();
            }
            else
            {
                foreach (var item in Data.Where(d => d.ToLower().Contains(filter)))
                {
                    AddItemToPanel(item);
                }
            }
            Apritendina();
        }

        private void txtCombobox_Click(object sender, EventArgs e)
        {
            txtCombobox.SelectAll();
            if (isTendinaChiusa())
            {
                FLPItem.Controls.Clear();
                LoadUnfilteredData();
                Apritendina();
            }
        }

        // ===============================================
        //  APERTURA / CHIUSURA TENDINA
        // ===============================================
        private void Apritendina()
        {
            if (isTendinaChiusa())
            {
                i = 0;
                TimerItem.Start();
                Application.AddMessageFilter(this);
                FlippaImmagine();
            }
        }

        private void ChiudiTendina()
        {
            if (!isTendinaChiusa())
            {
                i = 0;
                Application.RemoveMessageFilter(this);
                Thread.Sleep(100);
                this.Height = _closedSize.Height;
                FlippaImmagine();
            }
        }

        private void FlippaImmagine()
        {
            if (PicCombobox.Image != null)
            {
                Image img = (Image)PicCombobox.Image.Clone();
                img.RotateFlip(RotateFlipType.RotateNoneFlipY);
                PicCombobox.Image = img;
            }
        }

        private bool isTendinaChiusa() => (Height == _closedSize.Height);

        // ===============================================
        //  CARICAMENTO DATI E PANEL LABEL
        // ===============================================
        private void LoadUnfilteredData()
        {
            foreach (var item in Data)
            {
                AddItemToPanel(item);
            }
        }

        private void AddItemToPanel(string text)
        {
            var label = new Label
            {
                Font = _LabelFont,
                Text = text.Trim(),
                ForeColor = Color.Black,
                TextAlign = ContentAlignment.MiddleLeft,
                Height = this.FontHeight + 4,
                Width = FLPItem.Width - 30,
                BackColor = (text.Trim() == SelectedItem && text.Trim() != string.Empty) ? _LabelSelectedColor : _LabelBackColor
            };

            label.MouseClick += Label_Selected;

            // Hover
            label.MouseEnter += (s, e) =>
            {
                if (label.Text != SelectedItem)
                    label.BackColor = _LabelOnHoverColor;
            };
            label.MouseLeave += (s, e) =>
            {
                label.BackColor = (label.Text == SelectedItem && label.Text != string.Empty) ? _LabelSelectedColor : _LabelBackColor;
            };

            FLPItem.Controls.Add(label);
        }

        // ===============================================
        //  GESTIONE DIMENSIONI
        // ===============================================
        private void RoundCombobox_Load(object sender, EventArgs e)
        {
            _closedSize = new Size(this.Width, txtCombobox.Height + txtCombobox.Margin.Top + txtCombobox.Margin.Bottom + 10);
            _closedSize = this.Size;
            PnlTitle.Height = _closedSize.Height;
            PnlTitle.BorderRadius = this.Radius;
            PnlItem.BorderRadius = this.Radius;
        }

        private void RoundCombobox_FontChanged(object sender, EventArgs e) => UpdateHeightBasedOnFont();

        protected override void OnFontChanged(EventArgs e)
        {
            base.OnFontChanged(e);
            UpdateHeightBasedOnFont();
        }

        private void UpdateHeightBasedOnFont()
        {
            int baseHeight = txtCombobox.Height + txtCombobox.Margin.Top + txtCombobox.Margin.Bottom + 10;
            _closedSize = new Size(this.Width, baseHeight);
            base.Height = baseHeight;
            PnlTitle.Height = _closedSize.Height;
        }

        private void SetControlHeight(int newHeight) => base.Height = newHeight;

        // ===============================================
        //  IMessageFilter per chiusura automatica
        // ===============================================
        public bool PreFilterMessage(ref Message m)
        {
            const int WM_LBUTTONDOWN = 0x0201;

            if (!isTendinaChiusa() && m.Msg == WM_LBUTTONDOWN)
            {
                Point mousePos = Control.MousePosition;
                if (!this.Bounds.Contains(this.Parent.PointToClient(mousePos)))
                {
                    ChiudiTendina();
                }
            }
            return false;
        }
        public int Count()
        {
            return DataSource.Count();
        }

        // Serializzazione icona
        public bool ShouldSerializeIconImage() => IconImage != null;
        public void ResetIconImage() => IconImage = null;
    }
}
