﻿using System;
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
    public partial class RunToGenerationDialog : Form
    {
        public RunToGenerationDialog()
        {
            InitializeComponent();
        }

        // Generation to run to property
        public int TargetGeneration
        {
            get
            {
                return (int)RunToGenNumericUpDown.Value;
            }
            set
            {
                RunToGenNumericUpDown.Value = value;
            }
        }

    }
}
