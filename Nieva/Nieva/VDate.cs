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
    public partial class VDate : Form
    {
        public VDate()
        {
            InitializeComponent();

            GVDate();
        }

        #region Get Validity Date
        void GVDate()
        {
            if (Certificate.psVCall == "PB")
            {
                lblHVDate.Text = "PRC / BOA Validity Date";

                if (Certificate.psPBFrom != "")
                {
                    dtpFDate.CustomFormat = "                MM-dd-yyyy";
                    dtpFDate.Text = Certificate.psPBFrom.Replace(" ", "");

                    dtpTDate.CustomFormat = "                MM-dd-yyyy";
                    dtpTDate.Text = Certificate.psPBTo.Replace(" ", "");
                }
            }

            else if (Certificate.psVCall == "S")
            {
                lblHVDate.Text = "SEC Validity Date";

                if (Certificate.psSFrom != "")
                {
                    dtpFDate.CustomFormat = "                MM-dd-yyyy";
                    dtpFDate.Text = Certificate.psSFrom.Replace(" ", "");

                    dtpTDate.CustomFormat = "                MM-dd-yyyy";
                    dtpTDate.Text = Certificate.psSTo.Replace(" ", "");
                }
            }

            else if (Certificate.psVCall == "CC")
            {
                lblHVDate.Text = "CEA CDA Validity Date";

                if (Certificate.psCCFrom != "")
                {
                    dtpFDate.CustomFormat = "                MM-dd-yyyy";
                    dtpFDate.Text = Certificate.psCCFrom.Replace(" ", "");

                    dtpTDate.CustomFormat = "                MM-dd-yyyy";
                    dtpTDate.Text = Certificate.psCCTo.Replace(" ", "");
                }
            }
            
            else if (Certificate.psVCall == "B")
            {
                lblHVDate.Text = "BIR Validity Date";

                if (Certificate.psBFrom != "")
                {
                    dtpFDate.CustomFormat = "                MM-dd-yyyy";
                    dtpFDate.Text = Certificate.psBFrom.Replace(" ", "");

                    dtpTDate.CustomFormat = "                MM-dd-yyyy";
                    dtpTDate.Text = Certificate.psBTo.Replace(" ", "");
                }
            }


            else if (Certificate.psVCall == "P")
            {
                lblHVDate.Text = "PTR Validity Date";

                if (Certificate.psPFrom != "")
                {
                    dtpFDate.CustomFormat = "                MM-dd-yyyy";
                    dtpFDate.Text = Certificate.psPFrom.Replace(" ", "");

                    dtpTDate.CustomFormat = "                MM-dd-yyyy";
                    dtpTDate.Text = Certificate.psPTo.Replace(" ", "");
                }
            }
        }
        #endregion

        #region Shortcut Keys
        private void VDate_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnVOk_Click(sender, e);
            }

            if (e.KeyCode == Keys.Escape)
            {
                btnVCancel_Click(sender, e);
            }
        }
        #endregion

        #region DatePicker ValueChanged
        private void dtpFDate_ValueChanged(object sender, EventArgs e)
        {
            dtpFDate.CustomFormat = "                MM-dd-yyyy";
        }

        private void dtpTDate_ValueChanged(object sender, EventArgs e)
        {
            dtpTDate.CustomFormat = "                MM-dd-yyyy";
        }
        #endregion

        #region Ok
        private void btnVOk_Click(object sender, EventArgs e)
        {
            if (dtpFDate.CustomFormat == " ")
            {
                Certificate.psCDT = "*EFrom";
                this.Hide();
            }

            else if (dtpTDate.CustomFormat == " ")
            {
                Certificate.psCDT = "*ETo";
                this.Hide();
            }

            else if (dtpFDate.Value > dtpTDate.Value)
            {
                Certificate.psCDT = "*EFT";
                this.Hide();
            }

            else
            {
                if (Certificate.psVCall == "PB")
                {
                    Certificate.psPBFrom = dtpFDate.Text;
                    Certificate.psPBTo = dtpTDate.Text;
                }

                else if (Certificate.psVCall == "S")
                {
                    Certificate.psSFrom = dtpFDate.Text;
                    Certificate.psSTo = dtpTDate.Text;
                }

                else if (Certificate.psVCall == "CC")
                {
                    Certificate.psCCFrom = dtpFDate.Text;
                    Certificate.psCCTo = dtpTDate.Text;
                }

                else if (Certificate.psVCall == "B")
                {
                    Certificate.psBFrom = dtpFDate.Text;
                    Certificate.psBTo = dtpTDate.Text;
                }


                else if (Certificate.psVCall == "P")
                {
                    Certificate.psPFrom = dtpFDate.Text;
                    Certificate.psPTo = dtpTDate.Text;
                }

                this.Hide();
            }
        }
        #endregion

        #region Cancel
        private void btnVCancel_Click(object sender, EventArgs e)
        {
            this.Hide();
        }
        #endregion

    }
}
