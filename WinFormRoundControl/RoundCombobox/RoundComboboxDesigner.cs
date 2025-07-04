using System.Windows.Forms.Design;
using System.Windows.Forms;
using System.ComponentModel.Design;

namespace RoundComboboxTest
{
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
