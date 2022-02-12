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
    public partial class RandomDialog : Form
    {
        public RandomDialog()
        {
            InitializeComponent();
        }
        // Seed property
        public int Seed
        {
            get
            {
                return (int)numericUpDownSeed.Value;
            }
            set
            {
                numericUpDownSeed.Value = value;
            }
        }
    }
}
