using MySql.Data.MySqlClient;
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
    public partial class Certificate : Form
    {
        public static string psWCall = "", psFName = "", psLName = "", psMI = "", psVCall = "", psCDT = "", psRDate = "", psCNumber = "", psCName = "", psAddress = "", psRDY = "", psPurpose = "", psCPAName = "", psCPACNumber = "", psPBNumber = "", psSECNumber = "", psCCNumber = "", psBIRNumber = "", psBSPYear = "", psTIN = "", psPTRNumber = ""; 
        public static string psPBFrom = "", psPBTo = "", psSFrom = "", psSTo = "", psCCFrom = "", psCCTo = "", psBFrom = "", psBTo = "", psPFrom = "", psPTo = "";

        public Certificate()
        {
            InitializeComponent();

            PGYear();

            if (psWCall == "CS")
            {
                txtCName.Text = psFName.ToUpper() + " " + psMI.ToUpper() + " " + psLName.ToUpper();
            }

            else if (psWCall == "C")
            {
                dtpRDate.Text = psRDate;
                txtCNumber.Text = psCNumber;
                txtCName.Text = psCName;
                txtAddress.Text = psAddress;
                txtRDY.Text = psRDY;
                txtPurpose.Text = psPurpose;
                txtCPAName.Text = psCPAName;
                txtCPACNumber.Text = psCPACNumber;
                txtPBNumber.Text = psPBNumber;
                txtSECNumber.Text = psSECNumber;
                txtCCNumber.Text = psCCNumber;
                txtBIRNumber.Text = psBIRNumber;
                cmbBSPYear.Text = "                    " + psBSPYear;
                txtTIN.Text = psTIN;
                txtPTRNumber.Text = psPTRNumber;
            }

            EDButtons();
        }

        private void Certificate_Activated(object sender, EventArgs e)
        {
            if(psCDT == "*EFrom")
            {
                psCDT = "";

                MessageBox.Show("From Date field is empty.", "Empty Field", MessageBoxButtons.OK, MessageBoxIcon.Error);

                VDate w = new VDate();
                w.ShowDialog();
            }

            else if (psCDT == "*ETo")
            {
                psCDT = "";

                MessageBox.Show("To Date field is empty.", "Empty Field", MessageBoxButtons.OK, MessageBoxIcon.Error);

                VDate w = new VDate();
                w.ShowDialog();
            }

            else if (psCDT == "*EFT")
            {
                psCDT = "";

                MessageBox.Show("To Date field is earlier than From Date.", "Earlier To Date", MessageBoxButtons.OK, MessageBoxIcon.Error);

                VDate w = new VDate();
                w.ShowDialog();
            }
        }

        #region Back Color on Enable or Disable Buttons
        void EDButtons()
        {
            if (btnBack.Enabled == false)
            {
                btnBack.BackColor = Color.Silver;
            }

            else
            {
                btnBack.BackColor = Color.LimeGreen;
            }

            if (btnNext.Enabled == false)
            {
                btnNext.BackColor = Color.Silver;
            }

            else
            {
                btnNext.BackColor = Color.LimeGreen;
            }
        }
        #endregion

        #region DatePicker ValueChanged
        private void dtpRDate_ValueChanged(object sender, EventArgs e)
        {
            dtpRDate.CustomFormat = "                MM-dd-yyyy";
        }
        #endregion

        #region Shortcut Keys
        private void Certificate_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnPrint_Click(sender, e);
            }

            if (e.KeyCode == Keys.Escape)
            {
                btnCancel_Click(sender, e);
            }
        }
        #endregion

        #region Back
        private void btnBack_Click(object sender, EventArgs e)
        {
            if (flpP3.Visible)
            {
                flpP3.Visible = false;
                flpP2.Visible = true;
                flpP1.Visible = false;

                txtPurpose.Focus();

                btnNext.Enabled = true;

                EDButtons();
            }

            else if (flpP2.Visible)
            {
                flpP3.Visible = false;
                flpP2.Visible = false;
                flpP1.Visible = true;
                btnBack.Enabled = false;

                EDButtons();
            }
        }
        #endregion

        #region Next
        private void btnNext_Click(object sender, EventArgs e)
        {
            if (flpP1.Visible)
            {
                flpP1.Visible = false;
                flpP2.Visible = true;
                flpP3.Visible = false;

                txtPurpose.Focus();

                btnBack.Enabled = true;

                EDButtons();
            }

            else if (flpP2.Visible)
            {
                flpP1.Visible = false;
                flpP2.Visible = false;
                flpP3.Visible = true;
                btnNext.Enabled = false;

                EDButtons();
            }
        }
        #endregion

        #region Populating Group Year
        void PGYear()
        {
            for (int i = 1970; i <= DateTime.Now.Year; i++)
            {
                cmbBSPYear.Items.Add(Convert.ToString("                    " + i));
            }

            cmbBSPYear.Items.Add(Convert.ToString("                    " + (DateTime.Now.Year + 1)));
        }
        #endregion

        #region Calendar Buttons
        private void btnCPB_Click(object sender, EventArgs e)
        {
            psVCall = "PB";

            VDate w = new VDate();
            w.ShowDialog();
        }

        private void btnCS_Click(object sender, EventArgs e)
        {
            psVCall = "S";

            VDate w = new VDate();
            w.ShowDialog();
        }

        private void btnCCC_Click(object sender, EventArgs e)
        {
            psVCall = "CC";

            VDate w = new VDate();
            w.ShowDialog();
        }

        private void btnCB_Click(object sender, EventArgs e)
        {
            psVCall = "B";

            VDate w = new VDate();
            w.ShowDialog();
        }

        private void btnCP_Click(object sender, EventArgs e)
        {
            psVCall = "P";

            VDate w = new VDate();
            w.ShowDialog();
        }
        #endregion

        #region Print
        void P1()
        {
            flpP1.Visible = true;
            flpP2.Visible = false;
            flpP3.Visible = false;

            btnBack.Enabled = false;
            btnNext.Enabled = true;

            EDButtons();
        }

        void P2()
        {
            flpP1.Visible = false;
            flpP2.Visible = true;
            flpP3.Visible = false;

            btnBack.Enabled = true;
            btnNext.Enabled = true;

            EDButtons();
        }

        void P3()
        {
            flpP1.Visible = false;
            flpP2.Visible = false;
            flpP3.Visible = true;

            btnBack.Enabled = true;
            btnNext.Enabled = false;

            EDButtons();
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            if (dtpRDate.CustomFormat == " ")
            {
                MessageBox.Show("Request Date field is empty.", "Empty Field", MessageBoxButtons.OK, MessageBoxIcon.Error);

                P1();
                dtpRDate.Focus();
            }

            else if (txtCNumber.Text == "")
            {
                MessageBox.Show("Control Number field is empty.", "Empty Field", MessageBoxButtons.OK, MessageBoxIcon.Error);

                P1();
                txtCNumber.Focus();
            }

            else if (txtCName.Text == "")
            {
                MessageBox.Show("Client Name field is empty.", "Empty Field", MessageBoxButtons.OK, MessageBoxIcon.Error);

                P1();
                txtCName.Focus();
            }

            else if (txtAddress.Text == "")
            {
                MessageBox.Show("Address field is empty.", "Empty Field", MessageBoxButtons.OK, MessageBoxIcon.Error);

                P1();
                txtAddress.Focus();
            }

            else if (txtRDY.Text == "")
            {
                MessageBox.Show("Report Date - Year field is empty.", "Empty Field", MessageBoxButtons.OK, MessageBoxIcon.Error);

                P1();
                txtRDY.Focus();
            }

            else if (txtPurpose.Text == "")
            {
                MessageBox.Show("Purpose field is empty.", "Empty Field", MessageBoxButtons.OK, MessageBoxIcon.Error);

                P2();
                txtPurpose.Focus();
            }

            else if (txtCPAName.Text == "")
            {
                MessageBox.Show("CPA Name field is empty.", "Empty Field", MessageBoxButtons.OK, MessageBoxIcon.Error);

                P2();
                txtCPAName.Focus();
            }

            else if (txtCPACNumber.Text == "")
            {
                MessageBox.Show("CPA Certificate Number field is empty.", "Empty Field", MessageBoxButtons.OK, MessageBoxIcon.Error);

                P2();
                txtCPACNumber.Focus();
            }

            else if (txtPBNumber.Text == "")
            {
                MessageBox.Show("PRC / BOA Number field is empty.", "Empty Field", MessageBoxButtons.OK, MessageBoxIcon.Error);

                P2();
                txtPBNumber.Focus();
            }

            else if (psPBFrom == "")
            {
                MessageBox.Show("PRC Validity Date fields is empty.", "Empty Field", MessageBoxButtons.OK, MessageBoxIcon.Error);

                P2();
            }

            else if (txtSECNumber.Text == "")
            {
                MessageBox.Show("SEC Number field is empty.", "Empty Field", MessageBoxButtons.OK, MessageBoxIcon.Error);

                P2();
                txtSECNumber.Focus();
            }

            else if (psSFrom == "")
            {
                MessageBox.Show("SEC Validity Date fields is empty.", "Empty Field", MessageBoxButtons.OK, MessageBoxIcon.Error);

                P2();
            }

            else if (txtCCNumber.Text == "")
            {
                MessageBox.Show("CEA CDA Number field is empty.", "Empty Field", MessageBoxButtons.OK, MessageBoxIcon.Error);

                P3();
                txtCCNumber.Focus();
            }

            else if (psCCFrom == "")
            {
                MessageBox.Show("CEA CDA Validity Date fields is empty.", "Empty Field", MessageBoxButtons.OK, MessageBoxIcon.Error);

                P3();
            }

            else if (txtBIRNumber.Text == "")
            {
                MessageBox.Show("BIR Number field is empty.", "Empty Field", MessageBoxButtons.OK, MessageBoxIcon.Error);

                P3();
                txtBIRNumber.Focus();
            }

            else if (psBFrom == "")
            {
                MessageBox.Show("BIR Validity Date fields is empty.", "Empty Field", MessageBoxButtons.OK, MessageBoxIcon.Error);

                P3();
            }

            else if (cmbBSPYear.Text == "")
            {
                MessageBox.Show("BSP Year field is empty.", "Empty Field", MessageBoxButtons.OK, MessageBoxIcon.Error);

                P3();
                cmbBSPYear.Focus();
            }

            else if (txtTIN.Text == "")
            {
                MessageBox.Show("TIN field is empty.", "Empty Field", MessageBoxButtons.OK, MessageBoxIcon.Error);

                P3();
                txtTIN.Focus();
            }

            else if (txtPTRNumber.Text == "")
            {
                MessageBox.Show("PTR Number field is empty.", "Empty Field", MessageBoxButtons.OK, MessageBoxIcon.Error);

                P3();
                txtPTRNumber.Focus();
            }

            else if (psPFrom == "")
            {
                MessageBox.Show("PTR Validity Date fields is empty.", "Empty Field", MessageBoxButtons.OK, MessageBoxIcon.Error);

                P3();
            }

            else
            {
                DateTime drdate, dpbfrom, dpbto, dsfrom, dsto, dccfrom, dccto, dbfrom, dbto, dpfrom, dpto;
                string srdate, spbfrom, spbto, ssfrom, ssto, sccfrom, sccto, sbfrom, sbto, spfrom, spto;

                drdate = dtpRDate.Value;
                srdate = drdate.ToString("yyyy-MM-dd");

                dpbfrom = Convert.ToDateTime(psPBFrom);
                spbfrom = dpbfrom.ToString("yyyy-MM-dd");

                dsfrom = Convert.ToDateTime(psSFrom);
                ssfrom = dsfrom.ToString("yyyy-MM-dd");

                dccfrom = Convert.ToDateTime(psCCFrom);
                sccfrom = dccfrom.ToString("yyyy-MM-dd");

                dbfrom = Convert.ToDateTime(psBFrom);
                sbfrom = dbfrom.ToString("yyyy-MM-dd");

                dpfrom = Convert.ToDateTime(psPFrom);
                spfrom = dpfrom.ToString("yyyy-MM-dd");

                dpbto = Convert.ToDateTime(psPBTo);
                spbto = dpbto.ToString("yyyy-MM-dd");

                dsto = Convert.ToDateTime(psSTo);
                ssto = dsto.ToString("yyyy-MM-dd");

                dccto = Convert.ToDateTime(psCCTo);
                sccto = dccto.ToString("yyyy-MM-dd");

                dbto = Convert.ToDateTime(psBTo);
                sbto = dbto.ToString("yyyy-MM-dd");

                dpto = Convert.ToDateTime(psPTo);
                spto = dpto.ToString("yyyy-MM-dd");

                if (psWCall == "CS")
                {
                    string cQuery = "INSERT INTO tbl_cert(Request_Date, Control_Number, Client_Name, Address,"
                        + " Report_Date_and_Year, Purpose, CPA_Name, CPA_Cert_Number, PRC_BOA_Number, PRC_BOA_Valid_From,"
                        + " PRC_BOA_Valid_To, SEC_Number, SEC_Valid_From, SEC_Valid_To, CDA_CEA_Number, CDA_CEA_Valid_From,"
                        + " CDA_CEA_Valid_To, BIR_Number, BIR_Valid_From, BIR_Valid_To, BSP_Accreditation_Year, TIN, PTR_Number, PTR_Valid_From, PTR_Valid_To)"
                        + " VALUES('" + srdate.Replace(" ", "") + "',"
                        + " '" + txtCNumber.Text.Replace("'", "''") + "',"
                        + " '" + txtCName.Text.Replace("'", "''") + "',"
                        + " '" + txtAddress.Text.Replace("'", "''") + "',"
                        + " '" + txtRDY.Text.Replace("'", "''") + "',"
                        + " '" + txtPurpose.Text.Replace("'", "''") + "',"
                        + " '" + txtCPAName.Text.Replace("'", "''") + "',"
                        + " '" + txtCPACNumber.Text.Replace("'", "''") + "',"
                        + " '" + txtPBNumber.Text.Replace("'", "''") + "',"
                        + " '" + spbfrom.Replace(" ", "") + "',"
                        + " '" + spbto.Replace(" ", "") + "',"
                        + " '" + txtSECNumber.Text.Replace("'", "''") + "',"
                        + " '" + ssfrom.Replace(" ", "") + "',"
                        + " '" + ssto.Replace(" ", "") + "',"
                        + " '" + txtCCNumber.Text.Replace("'", "''") + "',"
                        + " '" + sccfrom.Replace(" ", "") + "',"
                        + " '" + sccto.Replace(" ", "") + "',"
                        + " '" + txtBIRNumber.Text.Replace("'", "''") + "',"
                        + " '" + sbfrom.Replace(" ", "") + "',"
                        + " '" + sbto.Replace(" ", "") + "',"
                        + " '" + cmbBSPYear.Text.Replace(" ", "") + "',"
                        + " '" + txtTIN.Text.Replace("'", "''") + "',"
                        + " '" + txtPTRNumber.Text.Replace("'", "''") + "',"
                        + " '" + spfrom.Replace(" ", "") + "',"
                        + " '" + spto.Replace(" ", "") + "');";
                    MySqlConnection cConnection = new MySqlConnection(Conn.cString);
                    MySqlCommand cCommand = new MySqlCommand(cQuery, cConnection);
                    MySqlDataReader cReader;

                    try
                    {
                        cConnection.Open();
                        cReader = cCommand.ExecuteReader();
                        while (cReader.Read()) { }

                        psRDate = dtpRDate.Text;
                        psCNumber = txtCNumber.Text;
                        psCName = txtCName.Text;
                        psAddress = txtAddress.Text;
                        psRDY = txtRDY.Text;
                        psPurpose = txtPurpose.Text;
                        psCPAName = txtCPAName.Text;
                        psCPACNumber = txtCPACNumber.Text;
                        psPBNumber = txtPBNumber.Text;
                        psSECNumber = txtSECNumber.Text;
                        psCCNumber = txtCCNumber.Text;
                        psBIRNumber = txtBIRNumber.Text;
                        psBSPYear = cmbBSPYear.Text;
                        psTIN = txtTIN.Text;
                        psPTRNumber = txtPTRNumber.Text;

                        Main.psCert = "*Cert";

                        this.Hide();
                    }

                    catch (MySqlException ex)
                    {
                        if (ex.Message.Contains("Duplicate entry"))
                        {
                            MessageBox.Show("Control Number already exist.", "Already Exist", MessageBoxButtons.OK, MessageBoxIcon.Error);

                            P1();
                            txtCNumber.Focus();
                        }

                        else
                        {
                            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }

                    finally
                    {
                        cConnection.Close();
                    }
                }

                else if (psWCall == "C")
                {
                    string cQuery = "UPDATE tbl_cert SET"
                        + " Request_Date = '" + srdate.Replace(" ", "") + "',"
                        + " Control_Number = '" + txtCNumber.Text.Replace("'", "''") + "',"
                        + " Client_Name = '" + txtCName.Text.Replace("'", "''") + "'," 
                        + " Address = '" + txtAddress.Text.Replace("'", "''") + "',"
                        + " Report_Date_and_Year = '" + txtRDY.Text.Replace("'", "''") + "',"
                        + " Purpose = '" + txtPurpose.Text.Replace("'", "''") + "',"
                        + " CPA_Name = '" + txtCPAName.Text.Replace("'", "''") + "',"
                        + " CPA_Cert_Number = '" + txtCPACNumber.Text.Replace("'", "''") + "',"
                        + " PRC_BOA_Number = '" + txtPBNumber.Text.Replace("'", "''") + "',"
                        + " PRC_BOA_Valid_From = '" + spbfrom.Replace(" ", "") + "',"
                        + " PRC_BOA_Valid_To = '" + spbto.Replace(" ", "") + "',"
                        + " SEC_Number = '" + txtSECNumber.Text.Replace("'", "''") + "',"
                        + " SEC_Valid_From = '" + ssfrom.Replace(" ", "") + "',"
                        + " SEC_Valid_To = '" + ssto.Replace(" ", "") + "',"
                        + " CDA_CEA_Number = '" + txtCCNumber.Text.Replace("'", "''") + "',"
                        + " CDA_CEA_Valid_From = '" + sccfrom.Replace(" ", "") + "',"
                        + " CDA_CEA_Valid_To = '" + sccto.Replace(" ", "") + "',"
                        + " BIR_Number = '" + txtBIRNumber.Text.Replace("'", "''") + "',"
                        + " BIR_Valid_From = '" + sbfrom.Replace(" ", "") + "',"
                        + " BIR_Valid_To = '" + sbto.Replace(" ", "") + "',"
                        + " BSP_Accreditation_Year = '" + cmbBSPYear.Text.Replace(" ", "") + "',"
                        + " TIN = '" + txtTIN.Text.Replace("'", "''") + "',"
                        + " PTR_Number = '" + txtPTRNumber.Text.Replace("'", "''") + "',"
                        + " PTR_Valid_From = '" + spfrom.Replace(" ", "") + "',"
                        + " PTR_Valid_To = '" + spto.Replace(" ", "") + "'"
                        + " WHERE Control_Number = '" + psCNumber + "';";
                    MySqlConnection cConnection = new MySqlConnection(Conn.cString);
                    MySqlCommand cCommand = new MySqlCommand(cQuery, cConnection);
                    MySqlDataReader cReader;

                    try
                    {
                        cConnection.Open();
                        cReader = cCommand.ExecuteReader();
                        while (cReader.Read()) { }

                        psRDate = dtpRDate.Text;
                        psCNumber = txtCNumber.Text;
                        psCName = txtCName.Text;
                        psAddress = txtAddress.Text;
                        psRDY = txtRDY.Text;
                        psPurpose = txtPurpose.Text;
                        psCPAName = txtCPAName.Text;
                        psCPACNumber = txtCPACNumber.Text;
                        psPBNumber = txtPBNumber.Text;
                        psSECNumber = txtSECNumber.Text;
                        psCCNumber = txtCCNumber.Text;
                        psBIRNumber = txtBIRNumber.Text;
                        psBSPYear = cmbBSPYear.Text;
                        psTIN = txtTIN.Text;
                        psPTRNumber = txtPTRNumber.Text;

                        Main.psCert = "*Cert";

                        this.Hide();
                    }

                    catch (MySqlException ex)
                    {
                        if (ex.Message.Contains("Duplicate entry"))
                        {
                            MessageBox.Show("Control Number already exist.", "Already Exist", MessageBoxButtons.OK, MessageBoxIcon.Error);

                            P1();
                            txtCNumber.Focus();
                        }

                        else
                        {
                            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }

                    finally
                    {
                        cConnection.Close();
                    }
                }
            }
        }
        #endregion

        #region Cancel
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Hide();
        }
        #endregion

    }
}
