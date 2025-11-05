using RoundComboboxMultiSelect;
using RoundComboboxTest;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace MyAPP
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            roundComboboxMultiSelect1.DataSource = new string[] { "blabla","bibi","dada"};
        }
        int c = 0;
        private void button_add_Click(object sender, EventArgs e)
        {

            roundComboboxMultiSelect1.Add("pippo" + c);
            string[] l = roundComboboxMultiSelect1.DataSource;
            c++;
            //   roundCombobo2x1.Clear();
        
    }

        private void button_clear_Click(object sender, EventArgs e)
        {
            roundComboboxMultiSelect1.Clear();
        }

        private void Selected_button_Click(object sender, EventArgs e)
        {
            MessageBox.Show(string.Join(", ",roundComboboxMultiSelect1.SelectedItem));
        }
    }
}
