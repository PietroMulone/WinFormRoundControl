using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Forms.Design;


/*
    UTILIZZO DELL'OGGETTO MULTISELECT COMBOBOX
  Copia incolle nel codice i file 
"SelectionElement.cs" -> oggetto che rappresenta l'elemento della lista selezionabile. è composto da una pictureBox e una label.
"RoundPanel.cs" -> elemento pannello dentro il quale sviluppo la box multiselezione. grazie a questo pannello abbiamo la possibilità di arrotondare visivamente l'oggetto
"RoundComboboxMultiSelect.cs" -> Oggetto ComboboxMultiSelect

 Personalizzazioni: TUTTO di questi elementi è modificabile, vediamo in seguito dove e come:

Nelle proprietà dal Designer di MultiselectCombobox:
- Font : Font della textbox sempre visibile con gli elementi (dove clicchiamo per aprire)
 - labelFont : Font dei vari elementi "SelectionElement", quindi dei vari elementi

- PanelsBackGroundColor, cambia i colori di sfondo dei vari elementi (pannello che si apre, label varie e pannello superiore)

- LabelSelectedColor   -> colore di sfondo degli elementi "SelectionElement" se l'elemento è selezionato
- LabelOnHover color  -> colore di sfondo degli elementi "SelectionElement" se passiamo sopra con il mouse. -> se l'elemento è già selezionato e ci passo sopra il colore visualizzato sarà LabelSelectedColor

- Radius -> raggio arrotondamento dei 2 panneli del quale l'oggetto è formato. se il raggio è 0, il pannello risulterà rettangolare

- Icon Image è l'icona della freccietta verso il basso che apre il pannello. è programmata in modo che all'apertura su giri di 180 gradi.

- MaxDimension è il valore massimo in pixel per il quale il menù a tendina si apre. se il numero di elementi non occupa tutto lo spazio, il menù a tendina si apre tanto quanto il numero di elementi per la loro altezza


Immagini di selezione : le 2 immagini presenti nei "SelectionElement" per indicare se l'elemento è stato selezionato ho deciso di inserirle forzatamente direttamente da codice,
in quanto una volta selezionati non sono elementi che si toccano più. e soprattutto rimangono uguali per tutto il codice.



PROPRIETà DA CODICE: ho cercato di mimare le principali funzionalità di una combobox:

.Clear()  -> svuota gli elementi
.Add(string) -> aggiunge un elemento
SelectedItem -> è la variabile pubblica raggiungibile che esprime i valori selezionati sottoforma di Array di elementi. L'odine di questo array è quello di selezione
DataSource -> questa variabile contiene gli elementi presenti nella lista. inizializzabile in questo modo:  roundComboboxMultiSelect1.DataSource = new string[] { "blabla","bibi","dada"};

per altre modifiche chiedere a GPT, ne sa molto sull'argomento

 */





namespace RoundComboboxMultiSelect
{
    [Designer(typeof(RoundComboboxDesigner))] //-> designer personalizzato per bloccare la modifica dell'altezza
    public partial class ComboboxMultiSelect : UserControl, IMessageFilter
    {
        // ===============================
        //  Evento SelectedIndexChanged
        // ===============================
        public event EventHandler SelectedIndexChanged;

        // ===============================
        //  Proprietà di selezione
        // ===============================
        private List<string> _currentSelected = new List<string>();
        private int _selectedIndex = -1;
        private int _targetHeight; // altezza apertura
        private string _text;


        // VALORI STANDARD IMMAGINI del SELECTION ELEMENT
        private static string stringSelectionElementImgNotSelected = "iVBORw0KGgoAAAANSUhEUgAAABQAAAAUCAYAAACNiR0NAAAAAXNSR0IB2cksfwAAAARnQU1BAACxjwv8YQUAAAAgY0hSTQAAeiYAAICEAAD6AAAAgOgAAHUwAADqYAAAOpgAABdwnLpRPAAAAAlwSFlzAAAuIwAALiMBeKU/dgAAAAd0SU1FB+kHBw0EEzpiE8oAAAEOSURBVDjL7dS9K8VhFAfwj7eidBd/AUmZlAEZbMoi/gFmG2U7q5zR4A8wKNkMlMVGFtnEqLzFqsSVxfKjG9fL9VIG3+3p6XzqPOfp8MOpqzxExDiG0Ysy7t6pfcAptjJz/RUYEdPowwracFYUvZUGDGIMq5m5+AxGxAhmMJeZu59tLyJaMYERzGfmXn1xN45d7NfyXpl5gyVsYxKewB5cZOZ9rUMoaq6Kd38G73D8zQE3V4LlDwbw6dT/9D/8B/8wWIemb1rlSvAcnV9RIqIBrTioBDcwFBGlGrESpjCKzZfraxZdOMQ1dnBbxWlEe9HRAPqxnJkL1RbsWLF5unGCyypgCzpQwhHWMnPDb+URggVHA8EskfwAAAAASUVORK5CYII=";
        private static string stringSelectionElementImgSelected = "iVBORw0KGgoAAAANSUhEUgAAABQAAAAUCAYAAACNiR0NAAAAAXNSR0IB2cksfwAAAARnQU1BAACxjwv8YQUAAAAgY0hSTQAAeiYAAICEAAD6AAAAgOgAAHUwAADqYAAAOpgAABdwnLpRPAAAAAlwSFlzAAAuIwAALiMBeKU/dgAAAAd0SU1FB+kHBw0HDwVOHEYAAAIGSURBVDjLtZQ9aBRBFMd/eyao4B2IGHCtcmjgiLJHEr+aoEjA4FcghbBCCCIknUW6bYwIU1pZaBNEcLEQi8Qv0qjEgEUSGBJthHh+zalVhJDcGeFs3sbxmDu20H+zb97b+c37mF2Pf6DYD24BI0C3ZweiKBoA+oAuoAKsN+FsAB87709f9CobO8W34FmwK8Bh4B6wC/gkm5zaPzN3ufXbymBmtZoVVzk02m8R2CmgH7iulJpNUeI4MJysV4v5UqlYuIDSZMQ3AMwCcynbdkaeL37tzh4rFQs3gSFgExgAX5RS1RTZPQC6ZdkzpF+9Br5K3zeB68ByClgXMCjLWmh01gpvs4GVZgOwdM6yX7peaAE4cOdxfzXfVo79oA+YDI1eaAC8mhih0SdcL2RiP6gBbF3+fkk2zMtFdU020bVGJWRCoz2HfyT2A1PnO2vZkw2BAEvDp59W820TcnJZYnuS7EUdae5TMhTe9R6aCI0eD432gdtWqbXYD54DO8Q136THf4C2QqNHbShw3LIfpcnQA1od0JJjT6P+VWzgZ2CfI9N24KflWqkvN4qiLdKORRs4BfRGUZRznLxk2SfrYDlgVG7Ak6TUJDgmk3wD/ABmgDWAzvjZ9FrH3rH3PQcX5WNol4qOAkeAu0qpG38BBXpe/jwF4IN1hWxtB/JADngLPFRKTfG/9BtLPqCwS2fR7AAAAABJRU5ErkJggg==";
        private Image _SelectionElementImgNotSelected = Base64ToImage(stringSelectionElementImgNotSelected);
        private Image _SelectionElementImgSelected = Base64ToImage(stringSelectionElementImgSelected);

        [Browsable(false)]
        public List<string> SelectedItem
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
                Text = string.Join(", ", _currentSelected) ?? string.Empty;

                OnSelectedIndexChanged(EventArgs.Empty);
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
        public ComboboxMultiSelect()
        {
            InitializeComponent();
            _text = this.Text; // mi salvo il valore di text

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
            SelectedItem.Clear();
            SearchBlock = true;
             this.Text = _text; // resetto il valore --> causa problemi, mi riapre la tendina -> uso SearchBlock
        }


        // ===============================================
        //  GESTIONE SELEZIONE LABEL
        // ===============================================
        private void Label_Selected(object sender, MouseEventArgs e)
        {
            var selectionElement_Selected = (SelectionElement)sender;

            if (SelectedItem.Contains(selectionElement_Selected.Text))
                SelectedItem.Remove(selectionElement_Selected.Text);
            else
            {
                SelectedItem.Add(selectionElement_Selected.Text); // Aggiorna SelectedItem (che scatena l'evento)
                //SelectedItem.Sort(); // inserisco gli elementi sempre in ordine alfabetico
            }

            // aggiorna testo
            SearchBlock = true;
            if (SelectedItem.Count > 0)
            {
                this.Text = string.Join(",", SelectedItem);
            }
            else this.Text = _text; // resetto il valore


            // Aggiorna colori
            foreach (SelectionElement selectionElement in FLPItem.Controls.OfType<SelectionElement>())
            {
                selectionElement.BackColor =  SelectedItem.Contains(selectionElement.Text) ? _LabelSelectedColor : _LabelBackColor; // cambia colore sfondo degli elementi selezionati
            }
            selectionElement_Selected.ChangeImage();  // cambio immagine degli elementi selezionati
            i = 0;
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

            // Calcola altezza target solo una volta (all’inizio dell’animazione)
            if (_targetHeight == 0)
            {
                int itemCount = 0;
                int itemHeight = 0;

                // se DataSource è una collezione, ottieni il numero di elementi
                if (DataSource is ICollection collection)
                    itemCount = collection.Count;

                // se hai un controllo (es. ListBox, FlowLayoutPanel, ecc.)
                // potresti anche calcolare l’altezza media di un elemento:
                itemHeight = this.FontHeight + 4;  // è prensente anche in AddItemToPanel

                int desired = Height + itemCount * itemHeight + 20;

                _targetHeight = Math.Min(desired > 20 ? desired : Height+20, _maxDimension); // decide l'altezza-> desiderata ma non superiore a maxdimension
            }

            // Animazione graduale verso _targetHeight
            if (Height < _targetHeight)
            {
                SetControlHeight(Math.Min(Height + i, _targetHeight));
                i += 5;
            }
            else
            {
                TimerItem.Stop();
                _targetHeight = 0; // reset per la prossima apertura
                i = 1;             // reset incrementale se serve
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
            var selectionElement = new SelectionElement
            {
                Font = _LabelFont,
                Text = text.Trim(),
                ForeColor = Color.Black,
                //TextAlign = ContentAlignment.MiddleLeft,
                Margin = new Padding(0, 0, 0, 0), // Spazio tra gli elementi
                Height = this.FontHeight + 4,  // è prensente anche in TimerItem_Tick
                Width = FLPItem.Width - 30,
                ImgNotSelected = _SelectionElementImgNotSelected,  // MODIFICA CON LE 2 IMMAGINI CHE PER IL TICK si/no
                ImgSelected = _SelectionElementImgSelected,     // MODIFICA CON LE 2 IMMAGINI CHE PER IL TICK si/no
                BackColor = (SelectedItem.Contains(text.Trim()) && text.Trim() != string.Empty) ? _LabelSelectedColor : _LabelBackColor
            };

            // Imposta immagine "SELECTED" se è selezionata
            if (SelectedItem.Contains(text.Trim()))
                selectionElement.ChangeImage();



            selectionElement.MouseClick += Label_Selected;

            // Hover
            selectionElement.MouseEnter += (s, e) =>
            {
                if ( !SelectedItem.Contains(selectionElement.Text))
                    selectionElement.BackColor = _LabelOnHoverColor;
                selectionElement.Cursor = Cursors.Hand;
            };
            selectionElement.MouseLeave += (s, e) =>
            {
                selectionElement.BackColor = (SelectedItem.Contains(selectionElement.Text) && selectionElement.Text != string.Empty) ? _LabelSelectedColor : _LabelBackColor;
                selectionElement.Cursor = Cursors.Default;
            };

            FLPItem.Controls.Add(selectionElement);
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





        private static Image Base64ToImage(string base64)
        {
            byte[] imageBytes = Convert.FromBase64String(base64);
            using (var ms = new MemoryStream(imageBytes))
            {
                return Image.FromStream(ms);
            }
        }
    }


    public class RoundComboboxDesigner : ControlDesigner
    {
        public override SelectionRules SelectionRules
        {
            get
            {
                SelectionRules rules = base.SelectionRules;

                // Rimuove la possibilità di cambiare l'altezza con il mouse nel designer
                rules &= ~SelectionRules.TopSizeable;
                rules &= ~SelectionRules.BottomSizeable;

                return rules;
            }
        }
    }
}
