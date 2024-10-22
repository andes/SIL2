using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Microsoft.Ink;

namespace Firma2
{
    public partial class InkControl : UserControl
    {
        private InkOverlay inkO;
        public InkControl()
        {
            InitializeComponent();
        }

        private void UserControl1_Load(object sender,
       EventArgs e)
        {
            inkO = new InkOverlay(this);
            inkO.Enabled = true;
        }
    }
}
