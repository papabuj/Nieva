using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Nieva
{
    public partial class BReport : Form
    {
        public BReport()
        {
            InitializeComponent();
        }

        #region Per Client
        private void btnBRPClient_Click(object sender, EventArgs e)
        {
            Main.psBReport = "*Per Client";
            this.Hide();
        }
        #endregion

        #region Summary
        private void btnBRSummary_Click(object sender, EventArgs e)
        {
            Main.psBReport = "*Summary";
            this.Hide();
        }
        #endregion

    }
}
