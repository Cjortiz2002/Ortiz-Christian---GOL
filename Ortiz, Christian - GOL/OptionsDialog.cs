using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Ortiz__Christian___GOL
{
    public partial class OptionsDialog : Form
    {
        public OptionsDialog()
        {
            InitializeComponent();
        }

        #region Options Dialog Properties
        public int UniWidth
        {
            get
            {
                return (int)NumericUpDownWidth.Value;
            }
            set
            {
                NumericUpDownWidth.Value = value;
            }
        }

        public int UniHeight
        {
            get
            {
                return (int)NumericUpDownHeight.Value;
            }
            set
            {
                NumericUpDownHeight.Value = value;
            }
        }
        public int GenInterval
        {
            get
            {
                return (int)GenerationInterval.Value;
            }
            set
            {
                GenerationInterval.Value = value;
            }
        } 
        #endregion

    }
}
