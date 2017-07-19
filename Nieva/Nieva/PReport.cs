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
    public partial class PReport : Form
    {
        public PReport()
        {
            InitializeComponent();
        }

        #region Per Client
        private void btnPRPClient_Click(object sender, EventArgs e)
        {
            Main.psPReport = "*Per Client";
            this.Hide();
        }
        #endregion

        #region Summary
        private void btnPRSummary_Click(object sender, EventArgs e)
        {
            Main.psPReport = "*Summary";
            this.Hide();
        }
        #endregion

    }
}
