using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.IO;
using CrystalDecisions.CrystalReports.Engine;
using MySql.Data.MySqlClient;
using System.Net;
using System.Net.Sockets;
using System.Reflection;

namespace Nieva
{
    public partial class Main : Form
    {
        DataTable cDatasetCS, cDatasetCSM, cDatasetCSQ, cDatasetCSA, cDatalistCS, cDatalistCSM, cDatalistCSQ, cDatalistCSA, cDatasetBPM, cDatasetBPQ, cDatasetBPA, cDatasetUL, cDatasetPLM, cDatasetPLQ, cDatasetPLA, cDatasetC;
        public static string psCID = "", psCSNE = "", psCSU = "", psCSBName = "", psTFTrigger = "", psPBills = "", psBPNID = "", psRemarks = "", psRContent = "", psBReport = "", psPLNID = "", psPLTRN = "", psPayments = "", psPReport = "", psBPFDate = "", psBPTDate = "", psPLFDate = "", psPLTDate = "", psHIP = "", psCIP = "", psHPort = "", psCPort = "", psTransfer = "", psTSearch = "", psCFDate = "", psCTDate = "", psCert = "", psCNID = "", psCNum = "";
        public static int tcontent = 20;
        ToolTip tltp = new ToolTip();
        public static Socket sck;
        public static EndPoint epHost, epClient;

        public Main()
        {
            InitializeComponent();

            Cursor.Current = Cursors.WaitCursor;

            dgvCSClients.DoubleBuffered(true);
            dgvCSMonthly.DoubleBuffered(true);
            dgvCSQuarterly.DoubleBuffered(true);
            dgvCSAnnually.DoubleBuffered(true);

            dgvCert.DoubleBuffered(true);

            dgvBPMonthly.DoubleBuffered(true);
            dgvBPQuarterly.DoubleBuffered(true);
            dgvBPAnnually.DoubleBuffered(true);

            dgvPLMonthly.DoubleBuffered(true);
            dgvPLQuarterly.DoubleBuffered(true);
            dgvPLAnnually.DoubleBuffered(true);

            dgvULogs.DoubleBuffered(true);

            //----------> Clients and Services Table View

            thide = new Timer { Interval = 1 };
            thide.Tick += HPanel;

            SRestrict();

            CSTRestrict();

            CSOOption();

            CSCLoad(icsp);
            CSMLoad(icsp);
            CSQLoad(icsp);
            CSALoad(icsp);

            CSSearch();

            CSRCDefault();

            cmbCSSOption.SelectedIndex = 0;
            cmbCSOOption.SelectedIndex = 0;

            CSBBDefault();

            //----------> Clients and Services Field View

            PGYear();

            //----------> Billing and Payments

            BPTRestrict();

            BPOOption();

            BPMLoad(ibpmp);
            BPQLoad(ibpqp);
            BPALoad(ibpap);

            BPSearch();

            cmbBPSOption.SelectedIndex = 0;
            cmbBPOOption.SelectedIndex = 0;
            cmbBPFOption.SelectedIndex = 0;

            BPMRCDefault();
            BPQRCDefault();
            BPARCDefault();

            BPBDefault();

            tlpBPContent.ColumnStyles[2].Width = 0;

            //----------> Payment Logs

            PLOOption();

            PLMLoad(iplmp);
            PLQLoad(iplqp);
            PLALoad(iplap);

            PLSearch();

            cmbPLSOption.SelectedIndex = 0;
            cmbPLOOption.SelectedIndex = 0;
            cmbPLFOption.SelectedIndex = 0;

            PLMRCDefault();
            PLQRCDefault();
            PLARCDefault();

            PLBDefault();

            //----------> User Logs

            ULOOption();

            ULLoad(iulp);

            ULSearch();

            cmbULOOption.SelectedIndex = 0;
            cmbULTOption.SelectedIndex = 0;

            ULRCDefault();

            //----------> Clients

            COOption();

            CLoad(icp);

            CSearch();

            cmbCSOption.SelectedIndex = 0;
            cmbCOOption.SelectedIndex = 0;
            cmbCFOption.SelectedIndex = 0;

            CRCDefault();

            //----------> Others

            EDButtons();
            this.ActiveControl = dgvCSClients;
            TTip();

            psHIP = Login.psIP;
            psHPort = Login.psPort;

            SScocket();

            Cursor.Current = Cursors.Default;
        }

        private void Main_Activated(object sender, EventArgs e)
        {
            #region Produce Bills
            if (psPBills == "*EBDate")
            {
                psPBills = "";

                MessageBox.Show("Bill Date field is empty.", "Empty Field", MessageBoxButtons.OK, MessageBoxIcon.Error);
                PBills w = new PBills();
                w.ShowDialog();
            }
            #endregion

            #region Transfer
            if (psTransfer == "*ETransfer")
            {
                psTransfer = "";

                MessageBox.Show("Transfer field is empty.", "Empty field", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Transfer w = new Transfer();
                w.ShowDialog();
            }

            else if (psTransfer == "*STransfer")
            {
                psTransfer = "";

                CSCLoad(icsp);
                CSMLoad(icsp);
                CSQLoad(icsp);
                CSALoad(icsp);

                CSRCDefault();

                cmbCSSOption_SelectedIndexChanged(sender, e);
            }
            #endregion

            #region Remarks
            if (psRemarks == "*Monthly")
            {
                psRemarks = "";

                string trn = txtBPTR.Text;

                BPMLoad(ibpmp);
                cmbBPSOption_SelectedIndexChanged(sender, e);

                DataRow[] bpdrow = cDatasetBPM.Select("[Transaction Number] = '" + trn + "'");

                if (bpdrow.Length > 0)
                {
                    foreach (DataGridViewRow row in dgvBPMonthly.Rows)
                    {
                        if (row.Cells[2].Value.ToString().Equals("" + trn + ""))
                        {
                            dgvBPMonthly.CurrentCell = dgvBPMonthly[2, row.Index];
                        }
                    }
                }
            }

            else if (psRemarks == "*Quarterly")
            {
                psRemarks = "";

                string trn = txtBPTR.Text;

                BPQLoad(ibpqp);
                cmbBPSOption_SelectedIndexChanged(sender, e);

                DataRow[] bpdrow = cDatasetBPM.Select("[Transaction Number] = '" + trn + "'");

                if (bpdrow.Length > 0)
                {
                    foreach (DataGridViewRow row in dgvBPQuarterly.Rows)
                    {
                        if (row.Cells[2].Value.ToString().Equals("" + trn + ""))
                        {
                            dgvBPQuarterly.CurrentCell = dgvBPQuarterly[2, row.Index];
                        }
                    }
                }
            }

            else if (psRemarks == "*Annually")
            {
                psRemarks = "";

                string trn = txtBPTR.Text;

                BPALoad(ibpap);
                cmbBPSOption_SelectedIndexChanged(sender, e);

                DataRow[] bpdrow = cDatasetBPA.Select("[Transaction Number] = '" + trn + "'");

                if (bpdrow.Length > 0)
                {
                    foreach (DataGridViewRow row in dgvBPAnnually.Rows)
                    {
                        if (row.Cells[2].Value.ToString().Equals("" + trn + ""))
                        {
                            dgvBPAnnually.CurrentCell = dgvBPAnnually[2, row.Index];
                        }
                    }
                }
            }
            #endregion

            #region Bills Report
            if (psBReport == "*Per Client")
            {
                Cursor.Current = Cursors.WaitCursor;

                psBReport = "";

                string rpt = "";

                string uname = "";

                if (Login.psUType == "Staff")
                {
                    if (sbpfsearch == "" && sbpfdrange == "")
                    {
                        uname = "{Bills.Username} = '" + Login.psUName + "'";
                    }


                    else
                    {
                        uname = " and {Bills.Username} = '" + Login.psUName + "'";
                    }
                }

                else
                {
                    uname = "";
                }

                if (btnBPMonthly.BackColor == Color.LimeGreen)
                {
                    rpt = "CRMBP.rpt";
                }

                else if (btnBPQuarterly.BackColor == Color.LimeGreen)
                {
                    rpt = "CRQBP.rpt";
                }

                else if (btnBPAnnually.BackColor == Color.LimeGreen)
                {
                    rpt = "CRABP.rpt";
                }

                ReportDocument rd = new ReportDocument();
                rd.Load(Application.StartupPath + "\\CRBP\\" + rpt + "");

                string crf = sbpfsearch + sbpfdrange + uname + ";";
                crViewer.SelectionFormula = crf;

                FieldDefinition fd = rd.Database.Tables[0].Fields[sbpfield];
                rd.DataDefinition.SortFields[0].Field = fd;

                TextObject txtFTD = (TextObject)rd.ReportDefinition.Sections["Section1"].ReportObjects["txtFTDate"];

                if (cmbBPFOption.Text == "Frequency Option" || cmbBPFOption.Text == "All Time")
                {
                    txtFTD.Text = "All Time";
                }

                else
                {
                    txtFTD.Text = psBPFDate + " to " + psBPTDate;
                }

                crViewer.ReportSource = rd;
                crViewer.Refresh();
                crViewer.RefreshReport();

                btnRBack.Visible = true;

                crViewer.Visible = true;
                tlpBP.Visible = false;
            }

            else if (psBReport == "*Summary")
            {
                psBReport = "";

                string rpt = "";

                string uname = "";

                if (Login.psUType == "Staff")
                {
                    if (sbpfsearch == "" && sbpfdrange == "")
                    {
                        uname = "{Bills.Username} = '" + Login.psUName + "'";
                    }


                    else
                    {
                        uname = " and {Bills.Username} = '" + Login.psUName + "'";
                    }
                }

                if (btnBPMonthly.BackColor == Color.LimeGreen)
                {
                    if (cmbBPFOption.Text == "Daily")
                    {
                        rpt = "CRMBPSD.rpt";
                    }

                    else if (cmbBPFOption.Text == "Monthly")
                    {
                        rpt = "CRMBPSM.rpt";
                    }

                    else if (cmbBPFOption.Text == "Annually")
                    {
                        rpt = "CRMBPSY.rpt";
                    }

                    else
                    {
                        rpt = "CRMBPSA.rpt";
                    }
                }

                else if (btnBPQuarterly.BackColor == Color.LimeGreen)
                {
                    if (cmbBPFOption.Text == "Daily")
                    {
                        rpt = "CRQBPSD.rpt";
                    }

                    else if (cmbBPFOption.Text == "Monthly")
                    {
                        rpt = "CRQBPSM.rpt";
                    }

                    else if (cmbBPFOption.Text == "Annually")
                    {
                        rpt = "CRQBPSY.rpt";
                    }

                    else
                    {
                        rpt = "CRQBPSA.rpt";
                    }
                }

                else if (btnBPAnnually.BackColor == Color.LimeGreen)
                {
                    if (cmbBPFOption.Text == "Daily")
                    {
                        rpt = "CRABPSD.rpt";
                    }

                    else if (cmbBPFOption.Text == "Monthly")
                    {
                        rpt = "CRABPSM.rpt";
                    }

                    else if (cmbBPFOption.Text == "Annually")
                    {
                        rpt = "CRABPSY.rpt";
                    }

                    else
                    {
                        rpt = "CRABPSA.rpt";
                    }
                }

                ReportDocument rd = new ReportDocument();
                rd.Load(Application.StartupPath + "\\CRBP\\" + rpt + "");

                string crf = sbpfsearch + sbpfdrange + uname + ";";
                crViewer.SelectionFormula = crf;

                TextObject txtFTD = (TextObject)rd.ReportDefinition.Sections["Section1"].ReportObjects["txtFTDate"];

                if (cmbBPFOption.Text == "Frequency Option" || cmbBPFOption.Text == "All Time")
                {
                    txtFTD.Text = "All Time";
                }

                else
                {
                    txtFTD.Text = psBPFDate + " to " + psBPTDate;
                }

                crViewer.EnableDrillDown = false;
                crViewer.ReportSource = rd;
                crViewer.Refresh();
                crViewer.RefreshReport();

                btnRBack.Visible = true;

                crViewer.Visible = true;
                tlpBP.Visible = false;

                Cursor.Current = Cursors.Default;
            }
            #endregion

            #region Payment Report
            if (psPReport == "*Per Client")
            {
                Cursor.Current = Cursors.WaitCursor;
                psPReport = "";

                string rpt = "";

                if (btnPLMonthly.BackColor == Color.LimeGreen)
                {
                    rpt = "CRMPL.rpt";
                }

                else if (btnPLQuarterly.BackColor == Color.LimeGreen)
                {
                    rpt = "CRQPL.rpt";
                }

                else if (btnPLAnnually.BackColor == Color.LimeGreen)
                {
                    rpt = "CRAPL.rpt";
                }

                ReportDocument rd = new ReportDocument();
                rd.Load(Application.StartupPath + "\\CRPL\\" + rpt + "");

                string crf = splfsearch + splfdrange + ";";
                crViewer.SelectionFormula = crf;

                FieldDefinition fd = rd.Database.Tables[0].Fields[splfield];
                rd.DataDefinition.SortFields[0].Field = fd;

                TextObject txtFTD = (TextObject)rd.ReportDefinition.Sections["Section1"].ReportObjects["txtFTDate"];

                if (cmbPLFOption.Text == "Frequency Option" || cmbPLFOption.Text == "All Time")
                {
                    txtFTD.Text = "All Time";
                }

                else
                {
                    txtFTD.Text = psPLFDate + " to " + psPLTDate;
                }

                crViewer.ReportSource = rd;
                crViewer.Refresh();
                crViewer.RefreshReport();

                btnRBack.Visible = true;

                crViewer.Visible = true;
                tlpPL.Visible = false;
            }

            else if (psPReport == "*Summary")
            {
                psPReport = "";

                string rpt = "";

                if (btnPLMonthly.BackColor == Color.LimeGreen)
                {
                    if (cmbPLFOption.Text == "Daily")
                    {
                        rpt = "CRMPLSD.rpt";
                    }

                    else if (cmbPLFOption.Text == "Monthly")
                    {
                        rpt = "CRMPLSM.rpt";
                    }

                    else if (cmbPLFOption.Text == "Annually")
                    {
                        rpt = "CRMPLSY.rpt";
                    }

                    else
                    {
                        rpt = "CRMPLSA.rpt";
                    }
                }

                else if (btnPLQuarterly.BackColor == Color.LimeGreen)
                {
                    if (cmbPLFOption.Text == "Daily")
                    {
                        rpt = "CRQPLSD.rpt";
                    }

                    else if (cmbPLFOption.Text == "Monthly")
                    {
                        rpt = "CRQPLSM.rpt";
                    }

                    else if (cmbPLFOption.Text == "Annually")
                    {
                        rpt = "CRQPLSY.rpt";
                    }

                    else
                    {
                        rpt = "CRQPLSA.rpt";
                    }
                }

                else if (btnPLAnnually.BackColor == Color.LimeGreen)
                {
                    if (cmbPLFOption.Text == "Daily")
                    {
                        rpt = "CRAPLSD.rpt";
                    }

                    else if (cmbPLFOption.Text == "Monthly")
                    {
                        rpt = "CRAPLSM.rpt";
                    }

                    else if (cmbPLFOption.Text == "Annually")
                    {
                        rpt = "CRAPLSY.rpt";
                    }

                    else
                    {
                        rpt = "CRAPLSA.rpt";
                    }
                }

                ReportDocument rd = new ReportDocument();
                rd.Load(Application.StartupPath + "\\CRPL\\" + rpt + "");

                string crf = splfsearch + splfdrange + ";";
                crViewer.SelectionFormula = crf;

                TextObject txtFTD = (TextObject)rd.ReportDefinition.Sections["Section1"].ReportObjects["txtFTDate"];

                if (cmbPLFOption.Text == "Frequency Option" || cmbPLFOption.Text == "All Time")
                {
                    txtFTD.Text = "All Time";
                }

                else
                {
                    txtFTD.Text = psPLFDate + " to " + psPLTDate;
                }

                crViewer.ReportSource = rd;
                crViewer.Refresh();
                crViewer.RefreshReport();

                btnRBack.Visible = true;

                crViewer.Visible = true;
                tlpPL.Visible = false;

                Cursor.Current = Cursors.Default;
            }
            #endregion

            #region Certificate
            if (psCert == "*Cert")
            {
                Cursor.Current = Cursors.WaitCursor;

                psCert = "";

                ReportDocument rd = new ReportDocument();
                rd.Load(Application.StartupPath + "\\CRFT\\CTFT.rpt");

                rd.DataDefinition.FormulaFields["Request Date"].Text = "'" + Certificate.psRDate + "'";
                rd.DataDefinition.FormulaFields["Control Number"].Text = "'" + Certificate.psCNumber + "'";
                rd.DataDefinition.FormulaFields["Client Name"].Text = "'" + Certificate.psCName + "'";
                rd.DataDefinition.FormulaFields["Address"].Text = "'" + Certificate.psAddress + "'";
                rd.DataDefinition.FormulaFields["Report Date and Year"].Text = "'" + Certificate.psRDY + "'";
                rd.DataDefinition.FormulaFields["Purpose"].Text = "'" + Certificate.psPurpose + "'";
                rd.DataDefinition.FormulaFields["CPA Name"].Text = "'" + Certificate.psCPAName + "'";
                rd.DataDefinition.FormulaFields["CPA Certificate Number"].Text = "'" + Certificate.psCPACNumber + "'";

                rd.DataDefinition.FormulaFields["PRC / BOA Number"].Text = "'" + Certificate.psPBNumber + "'";
                rd.DataDefinition.FormulaFields["PRC / BOA Valid From"].Text = "'" + Certificate.psPBFrom + "'";
                rd.DataDefinition.FormulaFields["PRC / BOA Valid To"].Text = "'" + Certificate.psPBTo + "'";

                rd.DataDefinition.FormulaFields["SEC Number"].Text = "'" + Certificate.psSECNumber + "'";
                rd.DataDefinition.FormulaFields["SEC Valid From"].Text = "'" + Certificate.psSFrom + "'";
                rd.DataDefinition.FormulaFields["SEC Valid To"].Text = "'" + Certificate.psSTo + "'";

                rd.DataDefinition.FormulaFields["CDA CEA Number"].Text = "'" + Certificate.psCCNumber + "'";
                rd.DataDefinition.FormulaFields["CDA CEA Valid From"].Text = "'" + Certificate.psCCFrom + "'";
                rd.DataDefinition.FormulaFields["CDA CEA Valid To"].Text = "'" + Certificate.psCCTo + "'";

                rd.DataDefinition.FormulaFields["BIR Number"].Text = "'" + Certificate.psBIRNumber + "'";
                rd.DataDefinition.FormulaFields["BIR Valid From"].Text = "'" + Certificate.psBFrom + "'";
                rd.DataDefinition.FormulaFields["BIR Valid To"].Text = "'" + Certificate.psBTo + "'";

                rd.DataDefinition.FormulaFields["BSP Accreditation Year"].Text = "'" + Certificate.psBSPYear + "'";
                rd.DataDefinition.FormulaFields["TIN"].Text = "'" + Certificate.psTIN + "'";

                rd.DataDefinition.FormulaFields["PTR Number"].Text = "'" + Certificate.psPTRNumber + "'";
                rd.DataDefinition.FormulaFields["PTR Valid From"].Text = "'" + Certificate.psPFrom + "'";
                rd.DataDefinition.FormulaFields["PTR Valid To"].Text = "'" + Certificate.psPTo + "'";

                crViewer.ReportSource = rd;
                crViewer.Refresh();
                crViewer.RefreshReport();

                btnRBack.Visible = true;

                crViewer.Visible = true;

                if (Certificate.psWCall == "CS")
                {
                    tlpCS.Visible = false;
                }

                else if (Certificate.psWCall == "C")
                {
                    tlpCert.Visible = false;

                    btnCRefresh_Click(sender, e);
                }

                Cursor.Current = Cursors.Default;
            }
            #endregion
        }

        #region Generate Client I.D.
        void GCID()
        {
            Guid guid = Guid.NewGuid();
            string sguid = Convert.ToBase64String(guid.ToByteArray()).ToUpper();
            sguid = sguid.Replace("=", "").Replace("+", "").Replace("/", "");
            string clientid = sguid.Substring(0, 10);

            string cQuery = "SELECT Client_ID FROM tbl_clients WHERE Client_ID = '" + clientid + "';";
            MySqlConnection cConnection = new MySqlConnection(Conn.cString);
            MySqlCommand cCommand = new MySqlCommand(cQuery, cConnection);
            MySqlDataReader cReader;

            try
            {
                cConnection.Open();
                cReader = cCommand.ExecuteReader();
                int count = 0;
                while (cReader.Read())
                {
                    count = count + 1;
                }

                if (count == 1)
                {
                    GCID();
                }

                else
                {
                    psCID = clientid;
                }
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            finally
            {
                cConnection.Close();
            }
        }
        #endregion

        #region Navigation
        void NDefault()
        {
            btnCS.BackColor = Color.FromArgb(64, 64, 64);
            btnCS.FlatAppearance.MouseOverBackColor = Color.LimeGreen;
            btnCS.FlatAppearance.MouseDownBackColor = Color.ForestGreen;
            btnCS.Cursor = Cursors.Hand;

            btnBP.BackColor = Color.FromArgb(64, 64, 64);
            btnBP.FlatAppearance.MouseOverBackColor = Color.LimeGreen;
            btnBP.FlatAppearance.MouseDownBackColor = Color.ForestGreen;
            btnBP.Cursor = Cursors.Hand;

            btnPLogs.BackColor = Color.FromArgb(64, 64, 64);
            btnPLogs.FlatAppearance.MouseOverBackColor = Color.LimeGreen;
            btnPLogs.FlatAppearance.MouseDownBackColor = Color.ForestGreen;
            btnPLogs.Cursor = Cursors.Hand;

            btnULogs.BackColor = Color.FromArgb(64, 64, 64);
            btnULogs.FlatAppearance.MouseOverBackColor = Color.LimeGreen;
            btnULogs.FlatAppearance.MouseDownBackColor = Color.ForestGreen;
            btnULogs.Cursor = Cursors.Hand;

            btnCert.BackColor = Color.FromArgb(64, 64, 64);
            btnCert.FlatAppearance.MouseOverBackColor = Color.LimeGreen;
            btnCert.FlatAppearance.MouseDownBackColor = Color.ForestGreen;
            btnCert.Cursor = Cursors.Hand;

            tlpCS.Visible = false;
            tlpBP.Visible = false;
            tlpPL.Visible = false;
            tlpUL.Visible = false;
            tlpCert.Visible = false;
            crViewer.Visible = false;
            btnRBack.Visible = false;
        }

        private void btnCS_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;

            if (btnCS.BackColor != Color.LimeGreen)
            {
                NDefault();

                btnCS.BackColor = Color.LimeGreen;
                btnCS.FlatAppearance.MouseOverBackColor = Color.LimeGreen;
                btnCS.FlatAppearance.MouseDownBackColor = Color.LimeGreen;
                btnCS.Cursor = Cursors.Default;

                tlpCS.Visible = true;

                CSCLoad(icsp);
                CSMLoad(icsp);
                CSQLoad(icsp);
                CSALoad(icsp);

                CSRCDefault();

                cmbCSSOption_SelectedIndexChanged(sender, e);

                EDButtons();
            }

            Cursor.Current = Cursors.Default;
        }

        private void btnBP_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;

            if (btnBP.BackColor != Color.LimeGreen)
            {
                NDefault();

                if (psCSNE != "" || psCSU != "")
                {
                    btnCSCancel_Click(sender, e);
                }

                btnBP.BackColor = Color.LimeGreen;
                btnBP.FlatAppearance.MouseOverBackColor = Color.LimeGreen;
                btnBP.FlatAppearance.MouseDownBackColor = Color.LimeGreen;
                btnBP.Cursor = Cursors.Default;

                tlpBP.Visible = true;

                BPMLoad(ibpmp);
                BPQLoad(ibpqp);
                BPALoad(ibpap);

                BPMRCDefault();
                BPQRCDefault();
                BPARCDefault();

                cmbBPSOption_SelectedIndexChanged(sender, e);

                BPGSRow();

                BPBDefault();

                if (dgvBPMonthly.Visible && dgvBPMonthly.RowCount == 0)
                {
                    txtBPTR.Clear();
                    txtBPCID.Clear();
                    txtBPCName.Clear();
                    txtBPTPayment.Clear();
                    txtBPCPayment.Clear();
                    txtBPUPayment.Clear();
                }

                else if (dgvBPQuarterly.Visible && dgvBPQuarterly.RowCount == 0)
                {
                    txtBPTR.Clear();
                    txtBPCID.Clear();
                    txtBPCName.Clear();
                    txtBPTPayment.Clear();
                    txtBPCPayment.Clear();
                    txtBPUPayment.Clear();
                }

                else if (dgvBPAnnually.Visible && dgvBPAnnually.RowCount == 0)
                {
                    txtBPTR.Clear();
                    txtBPCID.Clear();
                    txtBPCName.Clear();
                    txtBPTPayment.Clear();
                    txtBPCPayment.Clear();
                    txtBPUPayment.Clear();
                }

                EDButtons();
            }

            Cursor.Current = Cursors.Default;
        }

        private void btnPLogs_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;

            if (btnPLogs.BackColor != Color.LimeGreen)
            {
                NDefault();

                if (psCSNE != "" || psCSU != "")
                {
                    btnCSCancel_Click(sender, e);
                }

                btnPLogs.BackColor = Color.LimeGreen;
                btnPLogs.FlatAppearance.MouseOverBackColor = Color.LimeGreen;
                btnPLogs.FlatAppearance.MouseDownBackColor = Color.LimeGreen;
                btnPLogs.Cursor = Cursors.Default;

                tlpPL.Visible = true;

                PLMLoad(iplmp);
                PLQLoad(iplqp);
                PLALoad(iplap);

                PLMRCDefault();
                PLQRCDefault();
                PLARCDefault();

                cmbPLSOption_SelectedIndexChanged(sender, e);

                PLGSRow();

                PLBDefault();

                EDButtons();
            }

            Cursor.Current = Cursors.Default;
        }

        private void btnULogs_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;

            if (btnULogs.BackColor != Color.LimeGreen)
            {
                NDefault();

                ULClear();

                if (psCSNE != "" || psCSU != "")
                {
                    btnCSCancel_Click(sender, e);
                }

                btnULogs.BackColor = Color.LimeGreen;
                btnULogs.FlatAppearance.MouseOverBackColor = Color.LimeGreen;
                btnULogs.FlatAppearance.MouseDownBackColor = Color.LimeGreen;
                btnULogs.Cursor = Cursors.Default;

                tlpUL.Visible = true;
            }

            Cursor.Current = Cursors.Default;
        }

        private void btnCert_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;

            NDefault();

            if (psCSNE != "" || psCSU != "")
            {
                btnCSCancel_Click(sender, e);
            }

            btnCert.BackColor = Color.LimeGreen;
            btnCert.FlatAppearance.MouseOverBackColor = Color.LimeGreen;
            btnCert.FlatAppearance.MouseDownBackColor = Color.LimeGreen;
            btnCert.Cursor = Cursors.Default;

            tlpCert.Visible = true;

            CLoad(icp);

            CRCDefault();

            cmbCSOption_SelectedIndexChanged(sender, e);

            CGSRow();

            CBDefault();

            EDButtons();

            Cursor.Current = Cursors.Default;
        }

        #endregion

        #region Staff Restriction
        void SRestrict()
        {
            if(Login.psUType == "Staff")
            {
                btnPLogs.Visible = false;
                btnULogs.Visible = false;
                btnBackup.Visible = false;
                btnRestore.Visible = false;

                btnCSTransfer.Visible = false;

                pnlFNav.Visible = true;

                btnBPDBill.Visible = false;

                btnBPS.Visible = false;

                btnCDelete.Visible = false;

                dgvBPMonthly.CellDoubleClick -= new DataGridViewCellEventHandler(this.dgvBP_CellDoubleClick);
                dgvBPQuarterly.CellDoubleClick -= new DataGridViewCellEventHandler(this.dgvBP_CellDoubleClick);
                dgvBPAnnually.CellDoubleClick -= new DataGridViewCellEventHandler(this.dgvBP_CellDoubleClick);
            }

            else
            {
                btnPLogs.Visible = true;
                btnULogs.Visible = true;
                btnBackup.Visible = true;
                btnRestore.Visible = true;

                btnCSTransfer.Visible = true;

                pnlFNav.Visible = false;

                btnBPDBill.Visible = true;

                btnBPS.Visible = true;

                btnCDelete.Visible = true;

                cmbCSSOption.Items.Add("User in Charge");
            }
        }
        #endregion

        #region Back Color on Enable or Disable Buttons
        void EDButtons()
        {
            #region Clients and Services Table View
            if (btnCSTFView.Enabled == false)
            {
                btnCSTFView.BackColor = Color.Silver;
            }

            else
            {
                btnCSTFView.BackColor = Color.LimeGreen;
            }

            if (btnCSClients.Enabled == false)
            {
                btnCSClients.BackColor = Color.Silver;
            }

            else
            {
                btnCSClients.BackColor = Color.LimeGreen;

                if (btnCSServices.BackColor == Color.LimeGreen)
                {
                    btnCSClients.BackColor = Color.FromArgb(64, 64, 64);
                }
            }

            if (btnCSServices.Enabled == false)
            {
                btnCSServices.BackColor = Color.Silver;
            }

            else
            {
                btnCSServices.BackColor = Color.LimeGreen;

                if (btnCSClients.BackColor == Color.LimeGreen)
                {
                    btnCSServices.BackColor = Color.FromArgb(64, 64, 64);
                }
            }

            if (btnCSFirst.Enabled == false)
            {
                btnCSFirst.BackColor = Color.Silver;
            }

            else
            {
                btnCSFirst.BackColor = Color.LimeGreen;
            }

            if (btnCSBack.Enabled == false)
            {
                btnCSBack.BackColor = Color.Silver;
            }

            else
            {
                btnCSBack.BackColor = Color.LimeGreen;
            }

            if (btnCSNext.Enabled == false)
            {
                btnCSNext.BackColor = Color.Silver;
            }

            else
            {
                btnCSNext.BackColor = Color.LimeGreen;
            }

            if (btnCSLast.Enabled == false)
            {
                btnCSLast.BackColor = Color.Silver;
            }

            else
            {
                btnCSLast.BackColor = Color.LimeGreen;
            }

            if (btnCSPBills.Enabled == false)
            {
                btnCSPBills.BackColor = Color.Silver;
            }

            else
            {
                btnCSPBills.BackColor = Color.LimeGreen;
            }

            if (btnCSReport.Enabled == false)
            {
                btnCSReport.BackColor = Color.Silver;
            }

            else
            {
                btnCSReport.BackColor = Color.LimeGreen;
            }

            if (btnCSTransfer.Enabled == false)
            {
                btnCSTransfer.BackColor = Color.Silver;
            }

            else
            {
                btnCSTransfer.BackColor = Color.LimeGreen;
            }

            if (btnCSCert.Enabled == false)
            {
                btnCSCert.BackColor = Color.Silver;
            }

            else
            {
                btnCSCert.BackColor = Color.LimeGreen;
            }
            #endregion

            #region Clients and Services Field View
            if (btnCSNew.Enabled == false)
            {
                btnCSNew.BackColor = Color.Silver;
            }

            else
            {
                btnCSNew.BackColor = Color.LimeGreen;
            }

            if (btnCSEdit.Enabled == false)
            {
                btnCSEdit.BackColor = Color.Silver;
            }

            else
            {
                btnCSEdit.BackColor = Color.LimeGreen;
            }

            if (btnCSDA.Enabled == false)
            {
                btnCSDA.BackColor = Color.Silver;
            }

            else
            {
                btnCSDA.BackColor = Color.LimeGreen;
            }

            if (btnCSUpdate.Enabled == false)
            {
                btnCSUpdate.BackColor = Color.Silver;
            }

            else
            {
                btnCSUpdate.BackColor = Color.LimeGreen;
            }

            if (btnCSCancel.Enabled == false)
            {
                btnCSCancel.BackColor = Color.Silver;
            }

            else
            {
                btnCSCancel.BackColor = Color.LimeGreen;
            }

            if (btnCSSave.Enabled == false)
            {
                btnCSSave.BackColor = Color.Silver;
            }

            else
            {
                btnCSSave.BackColor = Color.LimeGreen;
            }
            #endregion

            #region Billing and Payments

            #region Monthly Services
            if (btnBPMFirst.Enabled == false)
            {
                btnBPMFirst.BackColor = Color.Silver;
            }

            else
            {
                btnBPMFirst.BackColor = Color.LimeGreen;
            }

            if (btnBPMBack.Enabled == false)
            {
                btnBPMBack.BackColor = Color.Silver;
            }

            else
            {
                btnBPMBack.BackColor = Color.LimeGreen;
            }

            if (btnBPMNext.Enabled == false)
            {
                btnBPMNext.BackColor = Color.Silver;
            }

            else
            {
                btnBPMNext.BackColor = Color.LimeGreen;
            }

            if (btnBPMLast.Enabled == false)
            {
                btnBPMLast.BackColor = Color.Silver;
            }

            else
            {
                btnBPMLast.BackColor = Color.LimeGreen;
            }
            #endregion

            #region Quarterly Services
            if (btnBPQFirst.Enabled == false)
            {
                btnBPQFirst.BackColor = Color.Silver;
            }

            else
            {
                btnBPQFirst.BackColor = Color.LimeGreen;
            }

            if (btnBPQBack.Enabled == false)
            {
                btnBPQBack.BackColor = Color.Silver;
            }

            else
            {
                btnBPQBack.BackColor = Color.LimeGreen;
            }

            if (btnBPQNext.Enabled == false)
            {
                btnBPQNext.BackColor = Color.Silver;
            }

            else
            {
                btnBPQNext.BackColor = Color.LimeGreen;
            }

            if (btnBPQLast.Enabled == false)
            {
                btnBPQLast.BackColor = Color.Silver;
            }

            else
            {
                btnBPQLast.BackColor = Color.LimeGreen;
            }
            #endregion

            #region Annually Services
            if (btnBPAFirst.Enabled == false)
            {
                btnBPAFirst.BackColor = Color.Silver;
            }

            else
            {
                btnBPAFirst.BackColor = Color.LimeGreen;
            }

            if (btnBPABack.Enabled == false)
            {
                btnBPABack.BackColor = Color.Silver;
            }

            else
            {
                btnBPABack.BackColor = Color.LimeGreen;
            }

            if (btnBPANext.Enabled == false)
            {
                btnBPANext.BackColor = Color.Silver;
            }

            else
            {
                btnBPANext.BackColor = Color.LimeGreen;
            }

            if (btnBPALast.Enabled == false)
            {
                btnBPALast.BackColor = Color.Silver;
            }

            else
            {
                btnBPALast.BackColor = Color.LimeGreen;
            }
            #endregion

            if (btnBPRemarks.Enabled == false)
            {
                btnBPRemarks.BackColor = Color.Silver;
            }

            else
            {
                btnBPRemarks.BackColor = Color.LimeGreen;
            }

            if (btnBPDBill.Enabled == false)
            {
                btnBPDBill.BackColor = Color.Silver;
            }

            else
            {
                btnBPDBill.BackColor = Color.LimeGreen;
            }

            if (btnBPPBills.Enabled == false)
            {
                btnBPPBills.BackColor = Color.Silver;
            }

            else
            {
                btnBPPBills.BackColor = Color.LimeGreen;
            }

            if (btnBPReport.Enabled == false)
            {
                btnBPReport.BackColor = Color.Silver;
            }

            else
            {
                btnBPReport.BackColor = Color.LimeGreen;
            }

            if (btnBPPay.Enabled == false)
            {
                btnBPPay.BackColor = Color.Silver;
            }

            else
            {
                btnBPPay.BackColor = Color.LimeGreen;
            }
            #endregion

            #region Payment Logs

            #region Monthly Services
            if (btnPLMFirst.Enabled == false)
            {
                btnPLMFirst.BackColor = Color.Silver;
            }

            else
            {
                btnPLMFirst.BackColor = Color.LimeGreen;
            }

            if (btnPLMBack.Enabled == false)
            {
                btnPLMBack.BackColor = Color.Silver;
            }

            else
            {
                btnPLMBack.BackColor = Color.LimeGreen;
            }

            if (btnPLMNext.Enabled == false)
            {
                btnPLMNext.BackColor = Color.Silver;
            }

            else
            {
                btnPLMNext.BackColor = Color.LimeGreen;
            }

            if (btnPLMLast.Enabled == false)
            {
                btnPLMLast.BackColor = Color.Silver;
            }

            else
            {
                btnPLMLast.BackColor = Color.LimeGreen;
            }
            #endregion

            #region Quarterly Services
            if (btnPLQFirst.Enabled == false)
            {
                btnPLQFirst.BackColor = Color.Silver;
            }

            else
            {
                btnPLQFirst.BackColor = Color.LimeGreen;
            }

            if (btnPLQBack.Enabled == false)
            {
                btnPLQBack.BackColor = Color.Silver;
            }

            else
            {
                btnPLQBack.BackColor = Color.LimeGreen;
            }

            if (btnPLQNext.Enabled == false)
            {
                btnPLQNext.BackColor = Color.Silver;
            }

            else
            {
                btnPLQNext.BackColor = Color.LimeGreen;
            }

            if (btnPLQLast.Enabled == false)
            {
                btnPLQLast.BackColor = Color.Silver;
            }

            else
            {
                btnPLQLast.BackColor = Color.LimeGreen;
            }
            #endregion

            #region Annually Services
            if (btnPLAFirst.Enabled == false)
            {
                btnPLAFirst.BackColor = Color.Silver;
            }

            else
            {
                btnPLAFirst.BackColor = Color.LimeGreen;
            }

            if (btnPLABack.Enabled == false)
            {
                btnPLABack.BackColor = Color.Silver;
            }

            else
            {
                btnPLABack.BackColor = Color.LimeGreen;
            }

            if (btnPLANext.Enabled == false)
            {
                btnPLANext.BackColor = Color.Silver;
            }

            else
            {
                btnPLANext.BackColor = Color.LimeGreen;
            }

            if (btnPLALast.Enabled == false)
            {
                btnPLALast.BackColor = Color.Silver;
            }

            else
            {
                btnPLALast.BackColor = Color.LimeGreen;
            }
            #endregion

            if (btnPLDLogs.Enabled == false)
            {
                btnPLDLogs.BackColor = Color.Silver;
            }

            else
            {
                btnPLDLogs.BackColor = Color.LimeGreen;
            }

            if (btnPLReport.Enabled == false)
            {
                btnPLReport.BackColor = Color.Silver;
            }

            else
            {
                btnPLReport.BackColor = Color.LimeGreen;
            }

            #endregion

            #region User Logs
            if (btnULFirst.Enabled == false)
            {
                btnULFirst.BackColor = Color.Silver;
            }

            else
            {
                btnULFirst.BackColor = Color.LimeGreen;
            }

            if (btnULBack.Enabled == false)
            {
                btnULBack.BackColor = Color.Silver;
            }

            else
            {
                btnULBack.BackColor = Color.LimeGreen;
            }

            if (btnULNext.Enabled == false)
            {
                btnULNext.BackColor = Color.Silver;
            }

            else
            {
                btnULNext.BackColor = Color.LimeGreen;
            }

            if (btnULLast.Enabled == false)
            {
                btnULLast.BackColor = Color.Silver;
            }

            else
            {
                btnULLast.BackColor = Color.LimeGreen;
            }
            #endregion

            #region Certificate
            if (btnCFirst.Enabled == false)
            {
                btnCFirst.BackColor = Color.Silver;
            }

            else
            {
                btnCFirst.BackColor = Color.LimeGreen;
            }

            if (btnCBack.Enabled == false)
            {
                btnCBack.BackColor = Color.Silver;
            }

            else
            {
                btnCBack.BackColor = Color.LimeGreen;
            }

            if (btnCNext.Enabled == false)
            {
                btnCNext.BackColor = Color.Silver;
            }

            else
            {
                btnCNext.BackColor = Color.LimeGreen;
            }

            if (btnCLast.Enabled == false)
            {
                btnCLast.BackColor = Color.Silver;
            }

            else
            {
                btnCLast.BackColor = Color.LimeGreen;
            }

            if (btnCDelete.Enabled == false)
            {
                btnCDelete.BackColor = Color.Silver;
            }

            else
            {
                btnCDelete.BackColor = Color.LimeGreen;
            }
            
            if (btnCRPrint.Enabled == false)
            {
                btnCRPrint.BackColor = Color.Silver;
            }

            else
            {
                btnCRPrint.BackColor = Color.LimeGreen;
            }
            #endregion

        }
        #endregion

        #region Exceptions
        private void NWException_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        private void LCException_KeyPress(object sender, KeyPressEventArgs e)
        {
            char c = e.KeyChar;

            if (!Char.IsDigit(c) && c != 8 && c != 46 || c == '.')
            {
                e.Handled = true;
            }
        }

        private void SCException_KeyPress(object sender, KeyPressEventArgs e)
        {
            TextBox tb = (TextBox)sender;

            char c = e.KeyChar;

            if (!Char.IsDigit(c) && c != 8 && c != 46)
            {
                e.Handled = true;
            }

            if (c == '.' && (tb.Text == "" || tb.SelectedText != String.Empty && (tb.Text.Contains(".") || !tb.Text.Contains("."))))
            {
                tb.Text = "0.";
                tb.SelectionStart = tb.Text.Length;
            }

            if (c == '0' && tb.Text == "0")
            {
                e.Handled = true;
            }

            if (tb.Text.StartsWith("0") && !tb.Text.Contains("."))
            {
                if (c != '0' && c != '.' && c != 8 && c != 46)
                {
                    tb.Text = tb.Text.Remove(0, 1);
                }
            }

            if (tb.Text.Contains(".") && c == '.')
            {
                e.Handled = true;
            }

            if (Regex.IsMatch(tb.Text, @"\.\d\d") && c != 8 && c != 46)
            {
                e.Handled = true;
            }

            int ict = tb.Text.Length;

            if (tb.SelectionLength == ict)
            {
                e.Handled = false;

                if (e.Handled == false && !Char.IsDigit(c) && c != 8 && c != 46)
                {
                    e.Handled = true;
                }
            }
        }

        private void SCException_TextChanged(object sender, EventArgs e)
        {
            TextBox tb = (TextBox)sender;

            if (tb.Text == ".")
            {
                tb.Clear();
            }

            if (tb.Text.StartsWith("."))
            {
                tb.Text = tb.Text.Insert(0, "0");
            }

            if (tb.Text.StartsWith("0") && tb.Text.Length >= 2 && !tb.Text.StartsWith("0."))
            {
                tb.Text = tb.Text.Remove(0, 1);
            }

            if (tb.Text == "Php ")
            {
                tb.Text = "";
            }
        }

        private void SCException_Enter(object sender, EventArgs e)
        {
            TextBox tb = (TextBox)sender;

            tb.Text = tb.Text.Replace("Php ", "");
        }

        private void SCException_Leave(object sender, EventArgs e)
        {
            TextBox tb = (TextBox)sender;

            if (tb.Text != "")
            {
                double f = Convert.ToDouble(tb.Text);
                tb.Text = string.Format("Php {0:0.00}", f);
            }
        }
        #endregion

        #region Shortcut Keys
        private void Main_KeyDown(object sender, KeyEventArgs e)
        {
            #region Arrow Keys for Clients and Services
            if ((dgvCSClients.Focused || dgvCSMonthly.Focused || dgvCSQuarterly.Focused || dgvCSAnnually.Focused) && tlpCS.Visible)
            {
                if (e.Alt && e.KeyCode == Keys.Left && btnCSFirst.Enabled)
                {
                    btnCSFirst_Click(sender, e);
                }

                if (!e.Alt && e.KeyCode == Keys.Left && btnCSBack.Enabled)
                {
                    btnCSBack_Click(sender, e);
                }

                if (!e.Alt && e.KeyCode == Keys.Right && btnCSNext.Enabled)
                {
                    btnCSNext_Click(sender, e);
                }

                if (e.Alt && e.KeyCode == Keys.Right && btnCSLast.Enabled)
                {
                    btnCSLast_Click(sender, e);
                }
            }
            #endregion

            #region Arrow Keys for Billings and Payments

            #region Monthly Services
            if (dgvBPMonthly.Focused && dgvBPMonthly.Visible && tlpBP.Visible)
            {
                if (e.Alt && e.KeyCode == Keys.Left && btnBPMFirst.Enabled)
                {
                    btnBPMFirst_Click(sender, e);
                }

                if (!e.Alt && e.KeyCode == Keys.Left && btnBPMBack.Enabled)
                {
                    btnBPMBack_Click(sender, e);
                }

                if (!e.Alt && e.KeyCode == Keys.Right && btnBPMNext.Enabled)
                {
                    btnBPMNext_Click(sender, e);
                }

                if (e.Alt && e.KeyCode == Keys.Right && btnBPMLast.Enabled)
                {
                    btnBPMLast_Click(sender, e);
                }
            }
            #endregion

            #region Quarterly Services
            if (dgvBPQuarterly.Focused && dgvBPQuarterly.Visible && tlpBP.Visible)
            {
                if (e.Alt && e.KeyCode == Keys.Left && btnBPQFirst.Enabled)
                {
                    btnBPQFirst_Click(sender, e);
                }

                if (!e.Alt && e.KeyCode == Keys.Left && btnBPQBack.Enabled)
                {
                    btnBPQBack_Click(sender, e);
                }

                if (!e.Alt && e.KeyCode == Keys.Right && btnBPQNext.Enabled)
                {
                    btnBPQNext_Click(sender, e);
                }

                if (e.Alt && e.KeyCode == Keys.Right && btnBPQLast.Enabled)
                {
                    btnBPQLast_Click(sender, e);
                }
            }
            #endregion

            #region Annually Services
            if (dgvBPAnnually.Focused && dgvBPAnnually.Visible && tlpBP.Visible)
            {
                if (e.Alt && e.KeyCode == Keys.Left && btnBPAFirst.Enabled)
                {
                    btnBPAFirst_Click(sender, e);
                }

                if (!e.Alt && e.KeyCode == Keys.Left && btnBPABack.Enabled)
                {
                    btnBPABack_Click(sender, e);
                }

                if (!e.Alt && e.KeyCode == Keys.Right && btnBPANext.Enabled)
                {
                    btnBPANext_Click(sender, e);
                }

                if (e.Alt && e.KeyCode == Keys.Right && btnBPALast.Enabled)
                {
                    btnBPALast_Click(sender, e);
                }
            }
            #endregion

            #endregion

            #region Arrow Keys for Payment Logs

            #region Monthly Services
            if (dgvPLMonthly.Focused && dgvPLMonthly.Visible && tlpPL.Visible)
            {
                if (e.Alt && e.KeyCode == Keys.Left && btnPLMFirst.Enabled)
                {
                    btnPLMFirst_Click(sender, e);
                }

                if (!e.Alt && e.KeyCode == Keys.Left && btnPLMBack.Enabled)
                {
                    btnPLMBack_Click(sender, e);
                }

                if (!e.Alt && e.KeyCode == Keys.Right && btnPLMNext.Enabled)
                {
                    btnPLMNext_Click(sender, e);
                }

                if (e.Alt && e.KeyCode == Keys.Right && btnPLMLast.Enabled)
                {
                    btnPLMLast_Click(sender, e);
                }
            }
            #endregion

            #region Quarterly Services
            if (dgvPLQuarterly.Focused && dgvPLQuarterly.Visible && tlpPL.Visible)
            {
                if (e.Alt && e.KeyCode == Keys.Left && btnPLQFirst.Enabled)
                {
                    btnPLQFirst_Click(sender, e);
                }

                if (!e.Alt && e.KeyCode == Keys.Left && btnPLQBack.Enabled)
                {
                    btnPLQBack_Click(sender, e);
                }

                if (!e.Alt && e.KeyCode == Keys.Right && btnPLQNext.Enabled)
                {
                    btnPLQNext_Click(sender, e);
                }

                if (e.Alt && e.KeyCode == Keys.Right && btnPLQLast.Enabled)
                {
                    btnPLQLast_Click(sender, e);
                }
            }
            #endregion

            #region Annually Services
            if (dgvPLAnnually.Focused && dgvPLAnnually.Visible && tlpPL.Visible)
            {
                if (e.Alt && e.KeyCode == Keys.Left && btnPLAFirst.Enabled)
                {
                    btnPLAFirst_Click(sender, e);
                }

                if (!e.Alt && e.KeyCode == Keys.Left && btnPLABack.Enabled)
                {
                    btnPLABack_Click(sender, e);
                }

                if (!e.Alt && e.KeyCode == Keys.Right && btnPLANext.Enabled)
                {
                    btnPLANext_Click(sender, e);
                }

                if (e.Alt && e.KeyCode == Keys.Right && btnPLALast.Enabled)
                {
                    btnPLALast_Click(sender, e);
                }
            }
            #endregion

            #endregion

            #region Arrow Keys for Clients and Services
            if (tlpUL.Visible)
            {
                if (e.Alt && e.KeyCode == Keys.Left && btnULFirst.Enabled)
                {
                    btnULFirst_Click(sender, e);
                }

                if (!e.Alt && e.KeyCode == Keys.Left && btnULBack.Enabled)
                {
                    btnULBack_Click(sender, e);
                }

                if (!e.Alt && e.KeyCode == Keys.Right && btnULNext.Enabled)
                {
                    btnULNext_Click(sender, e);
                }

                if (e.Alt && e.KeyCode == Keys.Right && btnULLast.Enabled)
                {
                    btnULLast_Click(sender, e);
                }
            }
            #endregion

            #region Arrow Keys for Certificate
            if (tlpCert.Visible)
            {
                if (e.Alt && e.KeyCode == Keys.Left && btnCFirst.Enabled)
                {
                    btnCFirst_Click(sender, e);
                }

                if (!e.Alt && e.KeyCode == Keys.Left && btnCBack.Enabled)
                {
                    btnCBack_Click(sender, e);
                }

                if (!e.Alt && e.KeyCode == Keys.Right && btnCNext.Enabled)
                {
                    btnCNext_Click(sender, e);
                }

                if (e.Alt && e.KeyCode == Keys.Right && btnCLast.Enabled)
                {
                    btnCLast_Click(sender, e);
                }
            }
            #endregion

            #region Clients and Services Services
            if (tlpCS.Visible && btnCSServices.Enabled)
            {
                if (e.Alt && e.KeyCode == Keys.M)
                {
                    btnCSMonthly_Click(sender, e);
                    e.Handled = true;
                    e.SuppressKeyPress = true;
                }

                if (e.Alt && e.KeyCode == Keys.Q)
                {
                    btnCSQuarterly_Click(sender, e);
                    e.Handled = true;
                    e.SuppressKeyPress = true;
                }

                if (e.Alt && e.KeyCode == Keys.A)
                {
                    btnCSAnnually_Click(sender, e);
                    e.Handled = true;
                    e.SuppressKeyPress = true;
                }
            }
            #endregion

            #region Billing and Payments Services
            if (tlpBP.Visible && btnBPServices.Enabled)
            {
                if (e.Alt && e.KeyCode == Keys.M)
                {
                    btnBPMonthly_Click(sender, e);
                    e.Handled = true;
                    e.SuppressKeyPress = true;
                }

                if (e.Alt && e.KeyCode == Keys.Q)
                {
                    btnBPQuarterly_Click(sender, e);
                    e.Handled = true;
                    e.SuppressKeyPress = true;
                }

                if (e.Alt && e.KeyCode == Keys.A)
                {
                    btnBPAnnually_Click(sender, e);
                    e.Handled = true;
                    e.SuppressKeyPress = true;
                }
            }
            #endregion

            #region Payment Logs Services
            if (tlpPL.Visible && btnPLServices.Enabled)
            {
                if (e.Alt && e.KeyCode == Keys.M)
                {
                    btnPLMonthly_Click(sender, e);
                    e.Handled = true;
                    e.SuppressKeyPress = true;
                }

                if (e.Alt && e.KeyCode == Keys.Q)
                {
                    btnPLQuarterly_Click(sender, e);
                    e.Handled = true;
                    e.SuppressKeyPress = true;
                }

                if (e.Alt && e.KeyCode == Keys.A)
                {
                    btnPLAnnually_Click(sender, e);
                    e.Handled = true;
                    e.SuppressKeyPress = true;
                }
            }
            #endregion

            #region User Logs Manage
            if (tlpUL.Visible)
            {
                if (e.Alt && e.KeyCode == Keys.T)
                {
                    btnULAStaff_Click(sender, e);
                    e.Handled = true;
                    e.SuppressKeyPress = true;
                }

                if (e.Alt && e.KeyCode == Keys.E)
                {
                    btnULDelete_Click(sender, e);
                    e.Handled = true;
                    e.SuppressKeyPress = true;
                }

                if (e.Alt && e.KeyCode == Keys.C)
                {
                    btnULChange_Click(sender, e);
                    e.Handled = true;
                    e.SuppressKeyPress = true;
                }
            }
            #endregion
        }
        #endregion

        #region Cell Formatting

        #region Clients and Services

        #region Monthly Services
        private void dgvCSMonthly_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            dgvCSMonthly.Columns["Retainers Fee"].DefaultCellStyle.Format = "Php 0.00";
            dgvCSMonthly.Columns["Professional Fee"].DefaultCellStyle.Format = "Php 0.00";
            dgvCSMonthly.Columns["Service Fee"].DefaultCellStyle.Format = "Php 0.00";
            dgvCSMonthly.Columns["VAT"].DefaultCellStyle.Format = "Php 0.00";
            dgvCSMonthly.Columns["Non-VAT"].DefaultCellStyle.Format = "Php 0.00";
            dgvCSMonthly.Columns["1601C"].DefaultCellStyle.Format = "Php 0.00";
            dgvCSMonthly.Columns["1601E"].DefaultCellStyle.Format = "Php 0.00";
            dgvCSMonthly.Columns["SSS (ER)"].DefaultCellStyle.Format = "Php 0.00";
            dgvCSMonthly.Columns["PHIC (ER)"].DefaultCellStyle.Format = "Php 0.00";
            dgvCSMonthly.Columns["Pag-IBIG (ER)"].DefaultCellStyle.Format = "Php 0.00";
            dgvCSMonthly.Columns["SSS (EE)"].DefaultCellStyle.Format = "Php 0.00";
            dgvCSMonthly.Columns["PHIC (EE)"].DefaultCellStyle.Format = "Php 0.00";
            dgvCSMonthly.Columns["Pag-IBIG (EE)"].DefaultCellStyle.Format = "Php 0.00";
            dgvCSMonthly.Columns["Certification Fee"].DefaultCellStyle.Format = "Php 0.00";
            dgvCSMonthly.Columns["Bookkeeping"].DefaultCellStyle.Format = "Php 0.00";
            dgvCSMonthly.Columns["Inventory"].DefaultCellStyle.Format = "Php 0.00";

            dgvCSMonthly.Columns["Retainers Fee"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvCSMonthly.Columns["Professional Fee"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvCSMonthly.Columns["Service Fee"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvCSMonthly.Columns["VAT"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvCSMonthly.Columns["Non-VAT"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvCSMonthly.Columns["1601C"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvCSMonthly.Columns["1601E"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvCSMonthly.Columns["SSS (ER)"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvCSMonthly.Columns["PHIC (ER)"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvCSMonthly.Columns["Pag-IBIG (ER)"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvCSMonthly.Columns["SSS (EE)"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvCSMonthly.Columns["PHIC (EE)"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvCSMonthly.Columns["Pag-IBIG (EE)"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvCSMonthly.Columns["Certification Fee"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvCSMonthly.Columns["Bookkeeping"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvCSMonthly.Columns["Inventory"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
        }
        #endregion

        #region Quarterly Services
        private void dgvCSQuarterly_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            dgvCSQuarterly.Columns["Retainers Fee"].DefaultCellStyle.Format = "Php 0.00";
            dgvCSQuarterly.Columns["Professional Fee"].DefaultCellStyle.Format = "Php 0.00";
            dgvCSQuarterly.Columns["Service Fee"].DefaultCellStyle.Format = "Php 0.00";
            dgvCSQuarterly.Columns["1701Q"].DefaultCellStyle.Format = "Php 0.00";
            dgvCSQuarterly.Columns["1702Q"].DefaultCellStyle.Format = "Php 0.00";
            dgvCSQuarterly.Columns["Bookkeeping"].DefaultCellStyle.Format = "Php 0.00";
            dgvCSQuarterly.Columns["Inventory"].DefaultCellStyle.Format = "Php 0.00";

            dgvCSQuarterly.Columns["Retainers Fee"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvCSQuarterly.Columns["Professional Fee"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvCSQuarterly.Columns["Service Fee"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvCSQuarterly.Columns["1701Q"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvCSQuarterly.Columns["1702Q"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvCSQuarterly.Columns["Bookkeeping"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvCSQuarterly.Columns["Inventory"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;    
        }
        #endregion

        #region Annually Services
        private void dgvCSAnnually_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            dgvCSAnnually.Columns["Retainers Fee"].DefaultCellStyle.Format = "Php 0.00";
            dgvCSAnnually.Columns["Professional Fee"].DefaultCellStyle.Format = "Php 0.00";
            dgvCSAnnually.Columns["Service Fee"].DefaultCellStyle.Format = "Php 0.00";
            dgvCSAnnually.Columns["1701"].DefaultCellStyle.Format = "Php 0.00";
            dgvCSAnnually.Columns["1702"].DefaultCellStyle.Format = "Php 0.00";
            dgvCSAnnually.Columns["1604CF"].DefaultCellStyle.Format = "Php 0.00";
            dgvCSAnnually.Columns["1604E"].DefaultCellStyle.Format = "Php 0.00";
            dgvCSAnnually.Columns["Municipal License"].DefaultCellStyle.Format = "Php 0.00";
            dgvCSAnnually.Columns["COR"].DefaultCellStyle.Format = "Php 0.00";
            dgvCSAnnually.Columns["Bookkeeping"].DefaultCellStyle.Format = "Php 0.00";
            dgvCSAnnually.Columns["Inventory"].DefaultCellStyle.Format = "Php 0.00";

            dgvCSAnnually.Columns["Retainers Fee"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvCSAnnually.Columns["Professional Fee"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvCSAnnually.Columns["Service Fee"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvCSAnnually.Columns["1701"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvCSAnnually.Columns["1702"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvCSAnnually.Columns["1604CF"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvCSAnnually.Columns["1604E"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvCSAnnually.Columns["Municipal License"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvCSAnnually.Columns["COR"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvCSAnnually.Columns["Bookkeeping"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvCSAnnually.Columns["Inventory"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;  
        }
        #endregion

        #endregion

        #region Billing

        #region Monthly Services
        private void dgvBPMonthly_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            dgvBPMonthly.Columns["Retainers Fee"].DefaultCellStyle.Format = "Php 0.00";
            dgvBPMonthly.Columns["Professional Fee"].DefaultCellStyle.Format = "Php 0.00";
            dgvBPMonthly.Columns["Service Fee"].DefaultCellStyle.Format = "Php 0.00";
            dgvBPMonthly.Columns["VAT"].DefaultCellStyle.Format = "Php 0.00";
            dgvBPMonthly.Columns["Non-VAT"].DefaultCellStyle.Format = "Php 0.00";
            dgvBPMonthly.Columns["1601C"].DefaultCellStyle.Format = "Php 0.00";
            dgvBPMonthly.Columns["1601E"].DefaultCellStyle.Format = "Php 0.00";
            dgvBPMonthly.Columns["SSS (ER)"].DefaultCellStyle.Format = "Php 0.00";
            dgvBPMonthly.Columns["PHIC (ER)"].DefaultCellStyle.Format = "Php 0.00";
            dgvBPMonthly.Columns["Pag-IBIG (ER)"].DefaultCellStyle.Format = "Php 0.00";
            dgvBPMonthly.Columns["SSS (EE)"].DefaultCellStyle.Format = "Php 0.00";
            dgvBPMonthly.Columns["PHIC (EE)"].DefaultCellStyle.Format = "Php 0.00";
            dgvBPMonthly.Columns["Pag-IBIG (EE)"].DefaultCellStyle.Format = "Php 0.00";
            dgvBPMonthly.Columns["Certification Fee"].DefaultCellStyle.Format = "Php 0.00";
            dgvBPMonthly.Columns["Bookkeeping"].DefaultCellStyle.Format = "Php 0.00";
            dgvBPMonthly.Columns["Inventory"].DefaultCellStyle.Format = "Php 0.00";
            dgvBPMonthly.Columns["Total Payments"].DefaultCellStyle.Format = "Php 0.00";
            dgvBPMonthly.Columns["Collected Payments"].DefaultCellStyle.Format = "Php 0.00";
            dgvBPMonthly.Columns["Uncollected Payments"].DefaultCellStyle.Format = "Php 0.00";

            dgvBPMonthly.Columns["Total Payments"].DefaultCellStyle.ForeColor = Color.Green;
            dgvBPMonthly.Columns["Collected Payments"].DefaultCellStyle.ForeColor = Color.MediumBlue;
            dgvBPMonthly.Columns["Uncollected Payments"].DefaultCellStyle.ForeColor = Color.Red;
            dgvBPMonthly.Columns["Total Payments"].DefaultCellStyle.SelectionBackColor = Color.Green;
            dgvBPMonthly.Columns["Collected Payments"].DefaultCellStyle.SelectionBackColor = Color.MediumBlue;
            dgvBPMonthly.Columns["Uncollected Payments"].DefaultCellStyle.SelectionBackColor = Color.Red;

            dgvBPMonthly.Columns["Retainers Fee"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvBPMonthly.Columns["Professional Fee"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvBPMonthly.Columns["Service Fee"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvBPMonthly.Columns["VAT"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvBPMonthly.Columns["Non-VAT"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvBPMonthly.Columns["1601C"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvBPMonthly.Columns["1601E"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvBPMonthly.Columns["SSS (ER)"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvBPMonthly.Columns["PHIC (ER)"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvBPMonthly.Columns["Pag-IBIG (ER)"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvBPMonthly.Columns["SSS (EE)"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvBPMonthly.Columns["PHIC (EE)"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvBPMonthly.Columns["Pag-IBIG (EE)"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvBPMonthly.Columns["Certification Fee"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvBPMonthly.Columns["Bookkeeping"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvBPMonthly.Columns["Inventory"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvBPMonthly.Columns["Total Payments"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvBPMonthly.Columns["Collected Payments"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvBPMonthly.Columns["Uncollected Payments"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
        }
        #endregion

        #region Quarterly Services
        private void dgvBPQuarterly_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            dgvBPQuarterly.Columns["Retainers Fee"].DefaultCellStyle.Format = "Php 0.00";
            dgvBPQuarterly.Columns["Professional Fee"].DefaultCellStyle.Format = "Php 0.00";
            dgvBPQuarterly.Columns["Service Fee"].DefaultCellStyle.Format = "Php 0.00";
            dgvBPQuarterly.Columns["1701Q"].DefaultCellStyle.Format = "Php 0.00";
            dgvBPQuarterly.Columns["1702Q"].DefaultCellStyle.Format = "Php 0.00";
            dgvBPQuarterly.Columns["Bookkeeping"].DefaultCellStyle.Format = "Php 0.00";
            dgvBPQuarterly.Columns["Inventory"].DefaultCellStyle.Format = "Php 0.00";
            dgvBPQuarterly.Columns["Total Payments"].DefaultCellStyle.Format = "Php 0.00";
            dgvBPQuarterly.Columns["Collected Payments"].DefaultCellStyle.Format = "Php 0.00";
            dgvBPQuarterly.Columns["Uncollected Payments"].DefaultCellStyle.Format = "Php 0.00";

            dgvBPQuarterly.Columns["Total Payments"].DefaultCellStyle.ForeColor = Color.Green;
            dgvBPQuarterly.Columns["Collected Payments"].DefaultCellStyle.ForeColor = Color.MediumBlue;
            dgvBPQuarterly.Columns["Uncollected Payments"].DefaultCellStyle.ForeColor = Color.Red;
            dgvBPQuarterly.Columns["Total Payments"].DefaultCellStyle.SelectionBackColor = Color.Green;
            dgvBPQuarterly.Columns["Collected Payments"].DefaultCellStyle.SelectionBackColor = Color.MediumBlue;
            dgvBPQuarterly.Columns["Uncollected Payments"].DefaultCellStyle.SelectionBackColor = Color.Red;

            dgvBPQuarterly.Columns["Retainers Fee"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvBPQuarterly.Columns["Professional Fee"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvBPQuarterly.Columns["Service Fee"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvBPQuarterly.Columns["1701Q"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvBPQuarterly.Columns["1702Q"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvBPQuarterly.Columns["Bookkeeping"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvBPQuarterly.Columns["Inventory"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvBPQuarterly.Columns["Total Payments"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvBPQuarterly.Columns["Collected Payments"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvBPQuarterly.Columns["Uncollected Payments"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
        }
        #endregion

        #region Annually Services
        private void dgvBPAnnually_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            dgvBPAnnually.Columns["Retainers Fee"].DefaultCellStyle.Format = "Php 0.00";
            dgvBPAnnually.Columns["Professional Fee"].DefaultCellStyle.Format = "Php 0.00";
            dgvBPAnnually.Columns["Service Fee"].DefaultCellStyle.Format = "Php 0.00";
            dgvBPAnnually.Columns["1701"].DefaultCellStyle.Format = "Php 0.00";
            dgvBPAnnually.Columns["1702"].DefaultCellStyle.Format = "Php 0.00";
            dgvBPAnnually.Columns["1604CF"].DefaultCellStyle.Format = "Php 0.00";
            dgvBPAnnually.Columns["1604E"].DefaultCellStyle.Format = "Php 0.00";
            dgvBPAnnually.Columns["Municipal License"].DefaultCellStyle.Format = "Php 0.00";
            dgvBPAnnually.Columns["COR"].DefaultCellStyle.Format = "Php 0.00";
            dgvBPAnnually.Columns["Bookkeeping"].DefaultCellStyle.Format = "Php 0.00";
            dgvBPAnnually.Columns["Inventory"].DefaultCellStyle.Format = "Php 0.00";
            dgvBPAnnually.Columns["Total Payments"].DefaultCellStyle.Format = "Php 0.00";
            dgvBPAnnually.Columns["Collected Payments"].DefaultCellStyle.Format = "Php 0.00";
            dgvBPAnnually.Columns["Uncollected Payments"].DefaultCellStyle.Format = "Php 0.00";

            dgvBPAnnually.Columns["Total Payments"].DefaultCellStyle.ForeColor = Color.Green;
            dgvBPAnnually.Columns["Collected Payments"].DefaultCellStyle.ForeColor = Color.MediumBlue;
            dgvBPAnnually.Columns["Uncollected Payments"].DefaultCellStyle.ForeColor = Color.Red;
            dgvBPAnnually.Columns["Total Payments"].DefaultCellStyle.SelectionBackColor = Color.Green;
            dgvBPAnnually.Columns["Collected Payments"].DefaultCellStyle.SelectionBackColor = Color.MediumBlue;
            dgvBPAnnually.Columns["Uncollected Payments"].DefaultCellStyle.SelectionBackColor = Color.Red;

            dgvBPAnnually.Columns["Retainers Fee"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvBPAnnually.Columns["Professional Fee"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvBPAnnually.Columns["Service Fee"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvBPAnnually.Columns["1701"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvBPAnnually.Columns["1702"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvBPAnnually.Columns["1604CF"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvBPAnnually.Columns["1604E"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvBPAnnually.Columns["Municipal License"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvBPAnnually.Columns["COR"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvBPAnnually.Columns["Bookkeeping"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvBPAnnually.Columns["Inventory"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvBPAnnually.Columns["Total Payments"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvBPAnnually.Columns["Collected Payments"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvBPAnnually.Columns["Uncollected Payments"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
        }
        #endregion

        #endregion

        #region Payment Logs

        #region Monthly Services
        private void dgvPLMonthly_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            dgvPLMonthly.Columns["Retainers Fee"].DefaultCellStyle.Format = "Php 0.00";
            dgvPLMonthly.Columns["Professional Fee"].DefaultCellStyle.Format = "Php 0.00";
            dgvPLMonthly.Columns["Service Fee"].DefaultCellStyle.Format = "Php 0.00";
            dgvPLMonthly.Columns["VAT"].DefaultCellStyle.Format = "Php 0.00";
            dgvPLMonthly.Columns["Non-VAT"].DefaultCellStyle.Format = "Php 0.00";
            dgvPLMonthly.Columns["1601C"].DefaultCellStyle.Format = "Php 0.00";
            dgvPLMonthly.Columns["1601E"].DefaultCellStyle.Format = "Php 0.00";
            dgvPLMonthly.Columns["SSS (ER)"].DefaultCellStyle.Format = "Php 0.00";
            dgvPLMonthly.Columns["PHIC (ER)"].DefaultCellStyle.Format = "Php 0.00";
            dgvPLMonthly.Columns["Pag-IBIG (ER)"].DefaultCellStyle.Format = "Php 0.00";
            dgvPLMonthly.Columns["SSS (EE)"].DefaultCellStyle.Format = "Php 0.00";
            dgvPLMonthly.Columns["PHIC (EE)"].DefaultCellStyle.Format = "Php 0.00";
            dgvPLMonthly.Columns["Pag-IBIG (EE)"].DefaultCellStyle.Format = "Php 0.00";
            dgvPLMonthly.Columns["Certification Fee"].DefaultCellStyle.Format = "Php 0.00";
            dgvPLMonthly.Columns["Bookkeeping"].DefaultCellStyle.Format = "Php 0.00";
            dgvPLMonthly.Columns["Inventory"].DefaultCellStyle.Format = "Php 0.00";
            dgvPLMonthly.Columns["Total Payments"].DefaultCellStyle.Format = "Php 0.00";

            dgvPLMonthly.Columns["Total Payments"].DefaultCellStyle.ForeColor = Color.Green;
            dgvPLMonthly.Columns["Total Payments"].DefaultCellStyle.SelectionBackColor = Color.Green;

            dgvPLMonthly.Columns["Retainers Fee"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvPLMonthly.Columns["Professional Fee"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvPLMonthly.Columns["Service Fee"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvPLMonthly.Columns["VAT"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvPLMonthly.Columns["Non-VAT"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvPLMonthly.Columns["1601C"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvPLMonthly.Columns["1601E"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvPLMonthly.Columns["SSS (ER)"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvPLMonthly.Columns["PHIC (ER)"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvPLMonthly.Columns["Pag-IBIG (ER)"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvPLMonthly.Columns["SSS (EE)"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvPLMonthly.Columns["PHIC (EE)"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvPLMonthly.Columns["Pag-IBIG (EE)"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvPLMonthly.Columns["Certification Fee"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvPLMonthly.Columns["Bookkeeping"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvPLMonthly.Columns["Inventory"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvPLMonthly.Columns["Total Payments"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight; 
        }
        #endregion

        #region Quarterly Services
        private void dgvPLQuarterly_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            dgvPLQuarterly.Columns["Retainers Fee"].DefaultCellStyle.Format = "Php 0.00";
            dgvPLQuarterly.Columns["Professional Fee"].DefaultCellStyle.Format = "Php 0.00";
            dgvPLQuarterly.Columns["Service Fee"].DefaultCellStyle.Format = "Php 0.00";
            dgvPLQuarterly.Columns["1701Q"].DefaultCellStyle.Format = "Php 0.00";
            dgvPLQuarterly.Columns["1702Q"].DefaultCellStyle.Format = "Php 0.00";
            dgvPLQuarterly.Columns["Bookkeeping"].DefaultCellStyle.Format = "Php 0.00";
            dgvPLQuarterly.Columns["Inventory"].DefaultCellStyle.Format = "Php 0.00";
            dgvPLQuarterly.Columns["Total Payments"].DefaultCellStyle.Format = "Php 0.00";

            dgvPLQuarterly.Columns["Total Payments"].DefaultCellStyle.ForeColor = Color.Green;
            dgvPLQuarterly.Columns["Total Payments"].DefaultCellStyle.SelectionBackColor = Color.Green;

            dgvPLQuarterly.Columns["Retainers Fee"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvPLQuarterly.Columns["Professional Fee"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvPLQuarterly.Columns["Service Fee"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvPLQuarterly.Columns["1701Q"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvPLQuarterly.Columns["1702Q"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvPLQuarterly.Columns["Bookkeeping"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvPLQuarterly.Columns["Inventory"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvPLQuarterly.Columns["Total Payments"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
        }
        #endregion

        #region Annually Services
        private void dgvPLAnnually_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            dgvPLAnnually.Columns["Retainers Fee"].DefaultCellStyle.Format = "Php 0.00";
            dgvPLAnnually.Columns["Professional Fee"].DefaultCellStyle.Format = "Php 0.00";
            dgvPLAnnually.Columns["Service Fee"].DefaultCellStyle.Format = "Php 0.00";
            dgvPLAnnually.Columns["1701"].DefaultCellStyle.Format = "Php 0.00";
            dgvPLAnnually.Columns["1702"].DefaultCellStyle.Format = "Php 0.00";
            dgvPLAnnually.Columns["1604CF"].DefaultCellStyle.Format = "Php 0.00";
            dgvPLAnnually.Columns["1604E"].DefaultCellStyle.Format = "Php 0.00";
            dgvPLAnnually.Columns["Municipal License"].DefaultCellStyle.Format = "Php 0.00";
            dgvPLAnnually.Columns["COR"].DefaultCellStyle.Format = "Php 0.00";
            dgvPLAnnually.Columns["Bookkeeping"].DefaultCellStyle.Format = "Php 0.00";
            dgvPLAnnually.Columns["Inventory"].DefaultCellStyle.Format = "Php 0.00";
            dgvPLAnnually.Columns["Total Payments"].DefaultCellStyle.Format = "Php 0.00";

            dgvPLAnnually.Columns["Total Payments"].DefaultCellStyle.ForeColor = Color.Green;
            dgvPLAnnually.Columns["Total Payments"].DefaultCellStyle.SelectionBackColor = Color.Green;

            dgvPLAnnually.Columns["Retainers Fee"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvPLAnnually.Columns["Professional Fee"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvPLAnnually.Columns["Service Fee"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvPLAnnually.Columns["1701"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvPLAnnually.Columns["1702"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvPLAnnually.Columns["1604CF"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvPLAnnually.Columns["1604E"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvPLAnnually.Columns["Municipal License"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvPLAnnually.Columns["COR"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvPLAnnually.Columns["Bookkeeping"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvPLAnnually.Columns["Inventory"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvPLAnnually.Columns["Total Payments"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
        }
        #endregion

        #endregion

        #endregion

        #region Tooltip
        void TTip()
        {
            tltp.AutoPopDelay = 5000;
            tltp.InitialDelay = 1000;
            tltp.ReshowDelay = 500;

            tltp.ShowAlways = true;

            #region Client and Services
            if (txtCSSearch.ForeColor != Color.DarkGray)
            {
                tltp.SetToolTip(txtCSSearch, "Search");
            }

            else
            {
                tltp.SetToolTip(txtCSSearch, "");
            }

            if (cmbCSSOption.ForeColor != Color.DarkGray)
            {
                tltp.SetToolTip(cmbCSSOption, "Search Option");
            }

            else
            {
                tltp.SetToolTip(cmbCSSOption, "");
            }

            if (cmbCSOOption.ForeColor != Color.DarkGray)
            {
                tltp.SetToolTip(cmbCSOOption, "Order Option");
            }

            else
            {
                tltp.SetToolTip(cmbCSOOption, "");
            }
            #endregion

            #region Billing and Payments
            if (txtBPSearch.ForeColor != Color.DarkGray)
            {
                tltp.SetToolTip(txtBPSearch, "Search");
            }

            else
            {
                tltp.SetToolTip(txtBPSearch, "");
            }

            if (cmbBPSOption.ForeColor != Color.DarkGray)
            {
                tltp.SetToolTip(cmbBPSOption, "Search Option");
            }

            else
            {
                tltp.SetToolTip(cmbBPSOption, "");
            }

            if (cmbBPOOption.ForeColor != Color.DarkGray)
            {
                tltp.SetToolTip(cmbBPOOption, "Order Option");
            }

            else
            {
                tltp.SetToolTip(cmbBPOOption, "");
            }

            if (cmbBPFOption.ForeColor != Color.DarkGray)
            {
                tltp.SetToolTip(cmbBPFOption, "Frequency Option");
            }

            else
            {
                tltp.SetToolTip(cmbBPFOption, "");
            }
            #endregion

            #region Payment Logs
            if (txtPLSearch.ForeColor != Color.DarkGray)
            {
                tltp.SetToolTip(txtPLSearch, "Search");
            }

            else
            {
                tltp.SetToolTip(txtPLSearch, "");
            }

            if (cmbPLSOption.ForeColor != Color.DarkGray)
            {
                tltp.SetToolTip(cmbPLSOption, "Search Option");
            }

            else
            {
                tltp.SetToolTip(cmbPLSOption, "");
            }

            if (cmbPLOOption.ForeColor != Color.DarkGray)
            {
                tltp.SetToolTip(cmbPLOOption, "Order Option");
            }

            else
            {
                tltp.SetToolTip(cmbPLOOption, "");
            }

            if (cmbPLFOption.ForeColor != Color.DarkGray)
            {
                tltp.SetToolTip(cmbPLFOption, "Frequency Option");
            }

            else
            {
                tltp.SetToolTip(cmbPLFOption, "");
            }
            #endregion

            #region Userlogs

            tltp.SetToolTip(txtULCNUsername, "Leave New Username empty if not intended to change.");
            tltp.SetToolTip(txtULCNPassword, "Leave New Password and Confirm Password empty if not intended to change.");
            tltp.SetToolTip(txtULCCPassword, "Leave New Password and Confirm Password empty if not intended to change.");

            if (txtULSearch.ForeColor != Color.DarkGray)
            {
                tltp.SetToolTip(txtULSearch, "Search");
            }

            else
            {
                tltp.SetToolTip(txtULSearch, "");
            }

            if (cmbULOOption.ForeColor != Color.DarkGray)
            {
                tltp.SetToolTip(cmbULOOption, "Order Option");
            }

            else
            {
                tltp.SetToolTip(cmbULOOption, "");
            }

            if (cmbULTOption.ForeColor != Color.DarkGray)
            {
                tltp.SetToolTip(cmbULTOption, "Time Option");
            }

            else
            {
                tltp.SetToolTip(cmbULTOption, "");
            }
            #endregion

            #region Certificate
            if (txtCSearch.ForeColor != Color.DarkGray)
            {
                tltp.SetToolTip(txtCSearch, "Search");
            }

            else
            {
                tltp.SetToolTip(txtCSearch, "");
            }

            if (cmbCSOption.ForeColor != Color.DarkGray)
            {
                tltp.SetToolTip(cmbCSOption, "Search Option");
            }

            else
            {
                tltp.SetToolTip(cmbCSOption, "");
            }

            if (cmbCOOption.ForeColor != Color.DarkGray)
            {
                tltp.SetToolTip(cmbCOOption, "Order Option");
            }

            else
            {
                tltp.SetToolTip(cmbCOOption, "");
            }

            if (cmbCFOption.ForeColor != Color.DarkGray)
            {
                tltp.SetToolTip(cmbCFOption, "Frequency Option");
            }

            else
            {
                tltp.SetToolTip(cmbCFOption, "");
            }
            #endregion

            #region Refresh
            tltp.SetToolTip(btnCSRefresh, "Refresh");
            tltp.SetToolTip(btnBPRefresh, "Refresh");
            tltp.SetToolTip(btnPLRefresh, "Refresh");
            tltp.SetToolTip(btnULRefresh, "Refresh");
            #endregion

        }
        #endregion

        #region Logout
        private void btnLogout_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;

            string nuser = "";
            int nu = 0;

            string cQuery = "SELECT COUNT(*) FROM tbl_user WHERE Status = 'Connected' AND User_Type = 'Staff';";
            MySqlConnection cConnection = new MySqlConnection(Conn.uString);
            MySqlCommand cCommand = new MySqlCommand(cQuery, cConnection);
            MySqlDataReader cReader;

            try
            {
                cConnection.Open();
                cReader = cCommand.ExecuteReader();
                while (cReader.Read())
                {
                    nuser = cReader.GetValue(0).ToString();
                    nu = Convert.ToInt32(nuser);
                }
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            finally
            {
                cConnection.Close();
            }

            if (psCSNE != "" || psCSU != "")
            {
                if (Login.psUType == "Admin" && nu != 0)
                {
                    MessageBox.Show("You can't logout because there are connected staff user.", "Can't Logout", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                else
                {
                    dynamic dialog = MessageBox.Show("Are you sure you want to logout the program without saving?", "Logout", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (dialog == DialogResult.No) { }

                    else if (dialog == DialogResult.Yes)
                    {
                        btnCSCancel_Click(sender, e);

                        RLogout();

                        Login w = new Login();
                        this.Hide();
                        w.Show();
                    }
                }
            }

            else
            {
                if (Login.psUType == "Admin" && nu != 0)
                {
                    MessageBox.Show("You can't logout because there are connected staff user.", "Can't Logout", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                else
                {
                    RLogout();

                    Login w = new Login();
                    this.Hide();
                    w.Show();
                }
            }

            Cursor.Current = Cursors.Default;
        }

        void RLogout()
        {
            string cQuery = " INSERT INTO tbl_userlogs(Username, User_Type, Activity, Time, PC_Name) VALUES('" + Login.psUName + "', '" + Login.psUType + "', 'Logout', '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm") + "', '" + Login.psMName + "');";
            MySqlConnection cConnection = new MySqlConnection(Conn.uString);
            MySqlCommand cCommand = new MySqlCommand(cQuery, cConnection);
            MySqlDataReader cReader;

            try
            {
                cConnection.Open();
                cReader = cCommand.ExecuteReader();
                while (cReader.Read()) { }
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            finally
            {
                cConnection.Close();
            }

            string cQueryI = "UPDATE tbl_user SET Status = 'Not Connected', IP_Address = NULL WHERE Username = '" + Login.psUName + "';";
            MySqlConnection cConnectionI = new MySqlConnection(Conn.uString);
            MySqlCommand cCommandI = new MySqlCommand(cQueryI, cConnectionI);
            MySqlDataReader cReaderI;

            try
            {
                cConnectionI.Open();
                cReaderI = cCommandI.ExecuteReader();
                while (cReaderI.Read()) { }
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            finally
            {
                cConnectionI.Close();
            }
        }
        #endregion

        #region Exit
        private void Main_FormClosing(object sender, FormClosingEventArgs e)
        {
            string nuser = "";
            int nu = 0;

            string cQuery = "SELECT COUNT(*) FROM tbl_user WHERE Status = 'Connected' AND User_Type = 'Staff';";
            MySqlConnection cConnection = new MySqlConnection(Conn.uString);
            MySqlCommand cCommand = new MySqlCommand(cQuery, cConnection);
            MySqlDataReader cReader;

            try
            {
                cConnection.Open();
                cReader = cCommand.ExecuteReader();
                while (cReader.Read())
                {
                    nuser = cReader.GetValue(0).ToString();
                    nu = Convert.ToInt32(nuser);
                }
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            finally
            {
                cConnection.Close();
            }

            if (e.CloseReason == CloseReason.UserClosing && (psCSNE != "" || psCSU != ""))
            {
                if (Login.psUType == "Admin" && nu != 0)
                {
                    MessageBox.Show("You can't exit the program because there are connected staff user.", "Can't Exit", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    e.Cancel = true;
                }

                else
                {
                    dynamic dialog = MessageBox.Show("Are you sure you want to exit the program without saving?", "Exit", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (dialog == DialogResult.No)
                    {
                        e.Cancel = true;
                    }

                    else if (dialog == DialogResult.Yes)
                    {
                        RLogout();

                        e.Cancel = false;
                        Application.Exit();
                    }
                }
            }

            else
            {
                if (Login.psUType == "Admin" && nu != 0)
                {
                    MessageBox.Show("You can't exit the program because there are connected staff user.", "Can't Exit", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    e.Cancel = true;
                }

                else
                {
                    if (e.CloseReason == CloseReason.UserClosing)
                    {
                        RLogout();

                        e.Cancel = false;
                        Application.Exit();
                    }
                }
            }
        }
        #endregion

        //----------------------------------------------------------------------// Clients and Services Table View

        #region Load Table
        int icsp = 0, icspage = 1, icsdpage = tcontent;
        double dcscount = 0, dcspage = 0, dcstpage = 0, dcsrpage = 0;
        string scscsearch = "", scscs = "", scsssearch = "", scsss = "", scscorder = "", scssorder = "", scspage = "", scsfsearch = "", scsfield = "", scsuname = "", scssuname = "";

        #region Clients
        public void CSCLoad(int p)
        {
            string cQueryI = "SELECT COUNT(*) FROM tbl_clients" + scscsearch + scsuname + ";";
            MySqlConnection cConnectionI = new MySqlConnection(Conn.cString);
            MySqlCommand cCommandI = new MySqlCommand(cQueryI, cConnectionI);
            MySqlDataReader cReaderI;

            try
            {
                cConnectionI.Open();
                cReaderI = cCommandI.ExecuteReader();
                while (cReaderI.Read())
                {
                    dcscount = cReaderI.GetDouble(0);
                }
            }

            catch (MySqlException ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            finally
            {
                cConnectionI.Close();
            }

            MySqlConnection cConn = new MySqlConnection(Conn.cString);
            MySqlCommand cCommand = new MySqlCommand("SELECT No_ID,"
                + " Client_ID AS 'Client I.D.',"
                + " CONCAT(Last_Name, ', ', First_Name, ' ', Middle_Initial) AS 'Client Name',"
                + " Business_Name AS 'Business Name',"
                + " Nature_of_Business AS 'Nature of Business',"
                + " Business_or_Owner_TIN AS 'Business / Owner TIN',"
                + " Tax_Type AS 'Tax Type',"
                + " Address,"
                + " Contact_Number AS 'Contact Number',"
                + " Email_Address AS 'Email Address',"
                + " SEC_Registration_Number AS 'SEC Reg. Number',"
                + " SEC_Registration_Date AS 'SEC Reg. Date',"
                + " DTI_Number AS 'DTI Number',"
                + " DTI_Issuance_Date AS 'DTI Issuance Date',"
                + " COR_Number AS 'COR Number',"
                + " COR_Date AS 'COR Date',"
                + " SSS_Number AS 'SSS Number',"
                + " Pag_IBIG_Number AS 'Pag-IBIG Number',"
                + " PhilHealth_Number AS 'PhilHealth Number',"
                + " Group_Name AS 'Group Name',"
                + " Sub_Group_Name AS 'Sub-group Name',"
                + " Group_Year AS 'Group Year',"
                + " Status, Username AS 'User in Charge' FROM tbl_clients" + scscsearch + scsuname + scscorder + ";", cConn);

            try
            {
                MySqlDataAdapter cAdapter = new MySqlDataAdapter();
                cAdapter.SelectCommand = cCommand;
                cDatasetCS = new DataTable();
                cAdapter.Fill(p, icsdpage, cDatasetCS);
                BindingSource cSource = new BindingSource();
                cSource.DataSource = cDatasetCS;
                dgvCSClients.DataSource = cSource;
                cAdapter.Update(cDatasetCS);
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            dgvCSClients.Columns[0].Visible = false;
            dgvCSClients.Columns[1].Visible = false;

            if(Login.psUType == "Staff")
            {
                dgvCSClients.Columns[23].Visible = false;
            }

            else
            {
                dgvCSClients.Columns[23].Visible = true;
            }
        }
        #endregion

        #region Monthly Services
        void CSMLoad(int p)
        {
            MySqlConnection cConn = new MySqlConnection(Conn.cString);
            MySqlCommand cCommand = new MySqlCommand("SELECT s.No_ID,"
                + " s.Client_ID AS  'Client I.D.',"
                + " CONCAT(c.Last_Name, ', ', c.First_Name, ' ', c.Middle_Initial) AS 'Client Name',"
                + " c.Business_Name AS 'Business Name',"
                + " c.Nature_of_Business AS 'Nature of Business',"
                + " c.Business_or_Owner_TIN AS 'Business / Owner TIN',"
                + " c.Tax_Type AS 'Tax Type',"
                + " c.SEC_Registration_Number AS 'SEC Reg. Number',"
                + " c.Group_Name AS 'Group Name',"
                + " c.Sub_Group_Name AS 'Sub-group Name',"
                + " c.Group_Year AS 'Group Year',"
                + " c.Status,"
                + " IFNULL(CAST(s.Retainers_Fee_M AS DECIMAL(20, 2)), 0) AS 'Retainers Fee',"
                + " IFNULL(CAST(s.Professional_Fee_M AS DECIMAL(20, 2)), 0) AS 'Professional Fee',"
                + " IFNULL(CAST(s.Service_Fee_M AS DECIMAL(20, 2)), 0) AS 'Service Fee',"
                + " IFNULL(CAST(s.VAT AS DECIMAL(20, 2)), 0) AS 'VAT',"
                + " IFNULL(CAST(s.Non_VAT AS DECIMAL(20, 2)), 0) AS 'Non-VAT',"
                + " IFNULL(CAST(s.D1601C AS DECIMAL(20, 2)), 0) AS '1601C',"
                + " IFNULL(CAST(s.D1601E AS DECIMAL(20, 2)), 0) AS '1601E',"
                + " IFNULL(CAST(s.SSS_ER AS DECIMAL(20, 2)), 0) AS 'SSS (ER)',"
                + " IFNULL(CAST(s.PHIC_ER AS DECIMAL(20, 2)), 0) AS 'PHIC (ER)',"
                + " IFNULL(CAST(s.Pag_IBIG_ER AS DECIMAL(20, 2)), 0) AS 'Pag-IBIG (ER)',"
                + " IFNULL(CAST(s.SSS_EE AS DECIMAL(20, 2)), 0) AS 'SSS (EE)',"
                + " IFNULL(CAST(s.PHIC_EE AS DECIMAL(20, 2)), 0) AS 'PHIC (EE)',"
                + " IFNULL(CAST(s.Pag_IBIG_EE AS DECIMAL(20, 2)), 0) AS 'Pag-IBIG (EE)',"
                + " IFNULL(CAST(s.Certification_Fee AS DECIMAL(20, 2)), 0) AS 'Certification Fee',"
                + " IFNULL(CAST(s.Bookkeeping_M AS DECIMAL(20, 2)), 0) AS 'Bookkeeping',"
                + " IFNULL(CAST(s.Inventory_M AS DECIMAL(20, 2)), 0) AS 'Inventory'"
                + " FROM tbl_mservices s LEFT JOIN tbl_clients c ON s.Client_ID = c.Client_ID" + scsssearch + scssuname + scssorder + ";", cConn);

            try
            {
                MySqlDataAdapter cAdapter = new MySqlDataAdapter();
                cAdapter.SelectCommand = cCommand;
                cDatasetCSM = new DataTable();
                cAdapter.Fill(p, icsdpage, cDatasetCSM);
                BindingSource cSource = new BindingSource();
                cSource.DataSource = cDatasetCSM;
                dgvCSMonthly.DataSource = cSource;
                cAdapter.Update(cDatasetCSM);
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            dgvCSMonthly.Columns[0].Visible = false;
            dgvCSMonthly.Columns[1].Visible = false;
            dgvCSMonthly.Columns[4].Visible = false;
            dgvCSMonthly.Columns[5].Visible = false;
            dgvCSMonthly.Columns[6].Visible = false;
            dgvCSMonthly.Columns[7].Visible = false;
            dgvCSMonthly.Columns[8].Visible = false;
            dgvCSMonthly.Columns[9].Visible = false;
            dgvCSMonthly.Columns[10].Visible = false;
            dgvCSMonthly.Columns[11].Visible = false;

            CSTColors();
        }
        #endregion

        #region Quarterly Services
        void CSQLoad(int p)
        {
            MySqlConnection cConn = new MySqlConnection(Conn.cString);
            MySqlCommand cCommand = new MySqlCommand("SELECT s.No_ID,"
                + " s.Client_ID AS  'Client I.D.',"
                + " CONCAT(c.Last_Name, ', ', c.First_Name, ' ', c.Middle_Initial) AS 'Client Name',"
                + " c.Business_Name AS 'Business Name',"
                + " c.Nature_of_Business AS 'Nature of Business',"
                + " c.Business_or_Owner_TIN AS 'Business / Owner TIN',"
                + " c.Tax_Type AS 'Tax Type',"
                + " c.SEC_Registration_Number AS 'SEC Reg. Number',"
                + " c.Group_Name AS 'Group Name',"
                + " c.Sub_Group_Name AS 'Sub-group Name',"
                + " c.Group_Year AS 'Group Year',"
                + " c.Status,"
                + " IFNULL(CAST(s.Retainers_Fee_Q AS DECIMAL(20, 2)), 0) AS 'Retainers Fee',"
                + " IFNULL(CAST(s.Professional_Fee_Q AS DECIMAL(20, 2)), 0) AS 'Professional Fee',"
                + " IFNULL(CAST(s.Service_Fee_Q AS DECIMAL(20, 2)), 0) AS 'Service Fee',"
                + " IFNULL(CAST(s.D1701Q AS DECIMAL(20, 2)), 0) AS '1701Q',"
                + " IFNULL(CAST(s.D1702Q AS DECIMAL(20, 2)), 0) AS '1702Q',"
                + " IFNULL(CAST(s.Bookkeeping_Q AS DECIMAL(20, 2)), 0) AS 'Bookkeeping',"
                + " IFNULL(CAST(s.Inventory_Q AS DECIMAL(20, 2)), 0) AS 'Inventory'"
                + " FROM tbl_qservices s LEFT JOIN tbl_clients c ON s.Client_ID = c.Client_ID" + scsssearch + scssuname + scssorder + ";", cConn);

            try
            {
                MySqlDataAdapter cAdapter = new MySqlDataAdapter();
                cAdapter.SelectCommand = cCommand;
                cDatasetCSQ = new DataTable();
                cAdapter.Fill(p, icsdpage, cDatasetCSQ);
                BindingSource cSource = new BindingSource();
                cSource.DataSource = cDatasetCSQ;
                dgvCSQuarterly.DataSource = cSource;
                cAdapter.Update(cDatasetCSQ);
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            dgvCSQuarterly.Columns[0].Visible = false;
            dgvCSQuarterly.Columns[1].Visible = false;
            dgvCSQuarterly.Columns[4].Visible = false;
            dgvCSQuarterly.Columns[5].Visible = false;
            dgvCSQuarterly.Columns[6].Visible = false;
            dgvCSQuarterly.Columns[7].Visible = false;
            dgvCSQuarterly.Columns[8].Visible = false;
            dgvCSQuarterly.Columns[9].Visible = false;
            dgvCSQuarterly.Columns[10].Visible = false;
            dgvCSQuarterly.Columns[11].Visible = false;

            CSTColors();
        }
        #endregion

        #region Annually Services
        void CSALoad(int p)
        {
            MySqlConnection cConn = new MySqlConnection(Conn.cString);
            MySqlCommand cCommand = new MySqlCommand("SELECT s.No_ID,"
                + " s.Client_ID AS  'Client I.D.',"
                + " CONCAT(c.Last_Name, ', ', c.First_Name, ' ', c.Middle_Initial) AS 'Client Name',"
                + " c.Business_Name AS 'Business Name',"
                + " c.Nature_of_Business AS 'Nature of Business',"
                + " c.Business_or_Owner_TIN AS 'Business / Owner TIN',"
                + " c.Tax_Type AS 'Tax Type',"
                + " c.SEC_Registration_Number AS 'SEC Reg. Number',"
                + " c.Group_Name AS 'Group Name',"
                + " c.Sub_Group_Name AS 'Sub-group Name',"
                + " c.Group_Year AS 'Group Year',"
                + " c.Status,"
                + " IFNULL(CAST(s.Retainers_Fee_A AS DECIMAL(20, 2)), 0) AS 'Retainers Fee',"
                + " IFNULL(CAST(s.Professional_Fee_A AS DECIMAL(20, 2)), 0) AS 'Professional Fee',"
                + " IFNULL(CAST(s.Service_Fee_A AS DECIMAL(20, 2)), 0) AS 'Service Fee',"
                + " IFNULL(CAST(s.D1701 AS DECIMAL(20, 2)), 0) AS '1701',"
                + " IFNULL(CAST(s.D1702 AS DECIMAL(20, 2)), 0) AS '1702',"
                + " IFNULL(CAST(s.D1604CF AS DECIMAL(20, 2)), 0) AS '1604CF',"
                + " IFNULL(CAST(s.D1604E AS DECIMAL(20, 2)), 0) AS '1604E',"
                + " IFNULL(CAST(s.Municipal_License AS DECIMAL(20, 2)), 0) AS 'Municipal License',"
                + " IFNULL(CAST(s.COR AS DECIMAL(20, 2)), 0) AS 'COR',"
                + " IFNULL(CAST(s.Bookkeeping_A AS DECIMAL(20, 2)), 0) AS 'Bookkeeping',"
                + " IFNULL(CAST(s.Inventory_A AS DECIMAL(20, 2)), 0) AS 'Inventory'"
                + " FROM tbl_aservices s LEFT JOIN tbl_clients c ON s.Client_ID = c.Client_ID" + scsssearch + scssuname + scssorder + ";", cConn);

            try
            {
                MySqlDataAdapter cAdapter = new MySqlDataAdapter();
                cAdapter.SelectCommand = cCommand;
                cDatasetCSA = new DataTable();
                cAdapter.Fill(p, icsdpage, cDatasetCSA);
                BindingSource cSource = new BindingSource();
                cSource.DataSource = cDatasetCSA;
                dgvCSAnnually.DataSource = cSource;
                cAdapter.Update(cDatasetCSA);
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            dgvCSAnnually.Columns[0].Visible = false;
            dgvCSAnnually.Columns[1].Visible = false;
            dgvCSAnnually.Columns[4].Visible = false;
            dgvCSAnnually.Columns[5].Visible = false;
            dgvCSAnnually.Columns[6].Visible = false;
            dgvCSAnnually.Columns[7].Visible = false;
            dgvCSAnnually.Columns[8].Visible = false;
            dgvCSAnnually.Columns[9].Visible = false;
            dgvCSAnnually.Columns[10].Visible = false;
            dgvCSAnnually.Columns[11].Visible = false;

            CSTColors();
        }
        #endregion

        #endregion

        #region Table Colors
        void CSTColors()
        {
            double z = 0;

            #region Monthly Services
            foreach (DataGridViewRow row in dgvCSMonthly.Rows)
            {
                if (Double.TryParse(row.Cells["Retainers Fee"].Value.ToString().Replace("Php ", ""), out z) && z == 0)
                {
                    row.Cells["Retainers Fee"].Style.ForeColor = Color.Black;
                    row.Cells["Retainers Fee"].Style.SelectionBackColor = SystemColors.Highlight;
                }

                else
                {
                    row.Cells["Retainers Fee"].Style.ForeColor = Color.MediumBlue;
                    row.Cells["Retainers Fee"].Style.SelectionBackColor = Color.MediumBlue;
                }

                if (Double.TryParse(row.Cells["Professional Fee"].Value.ToString().Replace("Php ", ""), out z) && z == 0)
                {
                    row.Cells["Professional Fee"].Style.ForeColor = Color.Black;
                    row.Cells["Professional Fee"].Style.SelectionBackColor = SystemColors.Highlight;
                }

                else
                {
                    row.Cells["Professional Fee"].Style.ForeColor = Color.MediumBlue;
                    row.Cells["Professional Fee"].Style.SelectionBackColor = Color.MediumBlue;
                }

                if (Double.TryParse(row.Cells["Service Fee"].Value.ToString().Replace("Php ", ""), out z) && z == 0)
                {
                    row.Cells["Service Fee"].Style.ForeColor = Color.Black;
                    row.Cells["Service Fee"].Style.SelectionBackColor = SystemColors.Highlight;
                }

                else
                {
                    row.Cells["Service Fee"].Style.ForeColor = Color.MediumBlue;
                    row.Cells["Service Fee"].Style.SelectionBackColor = Color.MediumBlue;
                }

                if (Double.TryParse(row.Cells["VAT"].Value.ToString().Replace("Php ", ""), out z) && z == 0)
                {
                    row.Cells["VAT"].Style.ForeColor = Color.Black;
                    row.Cells["VAT"].Style.SelectionBackColor = SystemColors.Highlight;
                }

                else
                {
                    row.Cells["VAT"].Style.ForeColor = Color.MediumBlue;
                    row.Cells["VAT"].Style.SelectionBackColor = Color.MediumBlue;
                }

                if (Double.TryParse(row.Cells["Non-VAT"].Value.ToString().Replace("Php ", ""), out z) && z == 0)
                {
                    row.Cells["Non-VAT"].Style.ForeColor = Color.Black;
                    row.Cells["Non-VAT"].Style.SelectionBackColor = SystemColors.Highlight;
                }

                else
                {
                    row.Cells["Non-VAT"].Style.ForeColor = Color.MediumBlue;
                    row.Cells["Non-VAT"].Style.SelectionBackColor = Color.MediumBlue;
                }

                if (Double.TryParse(row.Cells["1601C"].Value.ToString().Replace("Php ", ""), out z) && z == 0)
                {
                    row.Cells["1601C"].Style.ForeColor = Color.Black;
                    row.Cells["1601C"].Style.SelectionBackColor = SystemColors.Highlight;
                }

                else
                {
                    row.Cells["1601C"].Style.ForeColor = Color.MediumBlue;
                    row.Cells["1601C"].Style.SelectionBackColor = Color.MediumBlue;
                }

                if (Double.TryParse(row.Cells["1601E"].Value.ToString().Replace("Php ", ""), out z) && z == 0)
                {
                    row.Cells["1601E"].Style.ForeColor = Color.Black;
                    row.Cells["1601E"].Style.SelectionBackColor = SystemColors.Highlight;
                }

                else
                {
                    row.Cells["1601E"].Style.ForeColor = Color.MediumBlue;
                    row.Cells["1601E"].Style.SelectionBackColor = Color.MediumBlue;
                }

                if (Double.TryParse(row.Cells["SSS (ER)"].Value.ToString().Replace("Php ", ""), out z) && z == 0)
                {
                    row.Cells["SSS (ER)"].Style.ForeColor = Color.Black;
                    row.Cells["SSS (ER)"].Style.SelectionBackColor = SystemColors.Highlight;
                }

                else
                {
                    row.Cells["SSS (ER)"].Style.ForeColor = Color.MediumBlue;
                    row.Cells["SSS (ER)"].Style.SelectionBackColor = Color.MediumBlue;
                }

                if (Double.TryParse(row.Cells["PHIC (ER)"].Value.ToString().Replace("Php ", ""), out z) && z == 0)
                {
                    row.Cells["PHIC (ER)"].Style.ForeColor = Color.Black;
                    row.Cells["PHIC (ER)"].Style.SelectionBackColor = SystemColors.Highlight;
                }

                else
                {
                    row.Cells["PHIC (ER)"].Style.ForeColor = Color.MediumBlue;
                    row.Cells["PHIC (ER)"].Style.SelectionBackColor = Color.MediumBlue;
                }

                if (Double.TryParse(row.Cells["Pag-IBIG (ER)"].Value.ToString().Replace("Php ", ""), out z) && z == 0)
                {
                    row.Cells["Pag-IBIG (ER)"].Style.ForeColor = Color.Black;
                    row.Cells["Pag-IBIG (ER)"].Style.SelectionBackColor = SystemColors.Highlight;
                }

                else
                {
                    row.Cells["Pag-IBIG (ER)"].Style.ForeColor = Color.MediumBlue;
                    row.Cells["Pag-IBIG (ER)"].Style.SelectionBackColor = Color.MediumBlue;
                }

                if (Double.TryParse(row.Cells["SSS (EE)"].Value.ToString().Replace("Php ", ""), out z) && z == 0)
                {
                    row.Cells["SSS (EE)"].Style.ForeColor = Color.Black;
                    row.Cells["SSS (EE)"].Style.SelectionBackColor = SystemColors.Highlight;
                }

                else
                {
                    row.Cells["SSS (EE)"].Style.ForeColor = Color.MediumBlue;
                    row.Cells["SSS (EE)"].Style.SelectionBackColor = Color.MediumBlue;
                }

                if (Double.TryParse(row.Cells["PHIC (EE)"].Value.ToString().Replace("Php ", ""), out z) && z == 0)
                {
                    row.Cells["PHIC (EE)"].Style.ForeColor = Color.Black;
                    row.Cells["PHIC (EE)"].Style.SelectionBackColor = SystemColors.Highlight;
                }

                else
                {
                    row.Cells["PHIC (EE)"].Style.ForeColor = Color.MediumBlue;
                    row.Cells["PHIC (EE)"].Style.SelectionBackColor = Color.MediumBlue;
                }

                if (Double.TryParse(row.Cells["Pag-IBIG (EE)"].Value.ToString().Replace("Php ", ""), out z) && z == 0)
                {
                    row.Cells["Pag-IBIG (EE)"].Style.ForeColor = Color.Black;
                    row.Cells["Pag-IBIG (EE)"].Style.SelectionBackColor = SystemColors.Highlight;
                }

                else
                {
                    row.Cells["Pag-IBIG (EE)"].Style.ForeColor = Color.MediumBlue;
                    row.Cells["Pag-IBIG (EE)"].Style.SelectionBackColor = Color.MediumBlue;
                }

                if (Double.TryParse(row.Cells["Certification Fee"].Value.ToString().Replace("Php ", ""), out z) && z == 0)
                {
                    row.Cells["Certification Fee"].Style.ForeColor = Color.Black;
                    row.Cells["Certification Fee"].Style.SelectionBackColor = SystemColors.Highlight;
                }

                else
                {
                    row.Cells["Certification Fee"].Style.ForeColor = Color.MediumBlue;
                    row.Cells["Certification Fee"].Style.SelectionBackColor = Color.MediumBlue;
                }

                if (Double.TryParse(row.Cells["Bookkeeping"].Value.ToString().Replace("Php ", ""), out z) && z == 0)
                {
                    row.Cells["Bookkeeping"].Style.ForeColor = Color.Black;
                    row.Cells["Bookkeeping"].Style.SelectionBackColor = SystemColors.Highlight;
                }

                else
                {
                    row.Cells["Bookkeeping"].Style.ForeColor = Color.MediumBlue;
                    row.Cells["Bookkeeping"].Style.SelectionBackColor = Color.MediumBlue;
                }

                if (Double.TryParse(row.Cells["Inventory"].Value.ToString().Replace("Php ", ""), out z) && z == 0)
                {
                    row.Cells["Inventory"].Style.ForeColor = Color.Black;
                    row.Cells["Inventory"].Style.SelectionBackColor = SystemColors.Highlight;
                }

                else
                {
                    row.Cells["Inventory"].Style.ForeColor = Color.MediumBlue;
                    row.Cells["Inventory"].Style.SelectionBackColor = Color.MediumBlue;
                }
            }
            #endregion

            #region Quarterly Services
            foreach (DataGridViewRow row in dgvCSQuarterly.Rows)
            {
                if (Double.TryParse(row.Cells["Retainers Fee"].Value.ToString().Replace("Php ", ""), out z) && z == 0)
                {
                    row.Cells["Retainers Fee"].Style.ForeColor = Color.Black;
                    row.Cells["Retainers Fee"].Style.SelectionBackColor = SystemColors.Highlight;
                }

                else
                {
                    row.Cells["Retainers Fee"].Style.ForeColor = Color.MediumBlue;
                    row.Cells["Retainers Fee"].Style.SelectionBackColor = Color.MediumBlue;
                }

                if (Double.TryParse(row.Cells["Professional Fee"].Value.ToString().Replace("Php ", ""), out z) && z == 0)
                {
                    row.Cells["Professional Fee"].Style.ForeColor = Color.Black;
                    row.Cells["Professional Fee"].Style.SelectionBackColor = SystemColors.Highlight;
                }

                else
                {
                    row.Cells["Professional Fee"].Style.ForeColor = Color.MediumBlue;
                    row.Cells["Professional Fee"].Style.SelectionBackColor = Color.MediumBlue;
                }

                if (Double.TryParse(row.Cells["Service Fee"].Value.ToString().Replace("Php ", ""), out z) && z == 0)
                {
                    row.Cells["Service Fee"].Style.ForeColor = Color.Black;
                    row.Cells["Service Fee"].Style.SelectionBackColor = SystemColors.Highlight;
                }

                else
                {
                    row.Cells["Service Fee"].Style.ForeColor = Color.MediumBlue;
                    row.Cells["Service Fee"].Style.SelectionBackColor = Color.MediumBlue;
                }



                if (Double.TryParse(row.Cells["1701Q"].Value.ToString().Replace("Php ", ""), out z) && z == 0)
                {
                    row.Cells["1701Q"].Style.ForeColor = Color.Black;
                    row.Cells["1701Q"].Style.SelectionBackColor = SystemColors.Highlight;
                }

                else
                {
                    row.Cells["1701Q"].Style.ForeColor = Color.MediumBlue;
                    row.Cells["1701Q"].Style.SelectionBackColor = Color.MediumBlue;
                }

                if (Double.TryParse(row.Cells["1702Q"].Value.ToString().Replace("Php ", ""), out z) && z == 0)
                {
                    row.Cells["1702Q"].Style.ForeColor = Color.Black;
                    row.Cells["1702Q"].Style.SelectionBackColor = SystemColors.Highlight;
                }

                else
                {
                    row.Cells["1702Q"].Style.ForeColor = Color.MediumBlue;
                    row.Cells["1702Q"].Style.SelectionBackColor = Color.MediumBlue;
                }

                if (Double.TryParse(row.Cells["Bookkeeping"].Value.ToString().Replace("Php ", ""), out z) && z == 0)
                {
                    row.Cells["Bookkeeping"].Style.ForeColor = Color.Black;
                    row.Cells["Bookkeeping"].Style.SelectionBackColor = SystemColors.Highlight;
                }

                else
                {
                    row.Cells["Bookkeeping"].Style.ForeColor = Color.MediumBlue;
                    row.Cells["Bookkeeping"].Style.SelectionBackColor = Color.MediumBlue;
                }

                if (Double.TryParse(row.Cells["Inventory"].Value.ToString().Replace("Php ", ""), out z) && z == 0)
                {
                    row.Cells["Inventory"].Style.ForeColor = Color.Black;
                    row.Cells["Inventory"].Style.SelectionBackColor = SystemColors.Highlight;
                }

                else
                {
                    row.Cells["Inventory"].Style.ForeColor = Color.MediumBlue;
                    row.Cells["Inventory"].Style.SelectionBackColor = Color.MediumBlue;
                }
            }
            #endregion

            #region Annually Services
            foreach (DataGridViewRow row in dgvCSAnnually.Rows)
            {
                if (Double.TryParse(row.Cells["Retainers Fee"].Value.ToString().Replace("Php ", ""), out z) && z == 0)
                {
                    row.Cells["Retainers Fee"].Style.ForeColor = Color.Black;
                    row.Cells["Retainers Fee"].Style.SelectionBackColor = SystemColors.Highlight;
                }

                else
                {
                    row.Cells["Retainers Fee"].Style.ForeColor = Color.MediumBlue;
                    row.Cells["Retainers Fee"].Style.SelectionBackColor = Color.MediumBlue;
                }

                if (Double.TryParse(row.Cells["Professional Fee"].Value.ToString().Replace("Php ", ""), out z) && z == 0)
                {
                    row.Cells["Professional Fee"].Style.ForeColor = Color.Black;
                    row.Cells["Professional Fee"].Style.SelectionBackColor = SystemColors.Highlight;
                }

                else
                {
                    row.Cells["Professional Fee"].Style.ForeColor = Color.MediumBlue;
                    row.Cells["Professional Fee"].Style.SelectionBackColor = Color.MediumBlue;
                }

                if (Double.TryParse(row.Cells["Service Fee"].Value.ToString().Replace("Php ", ""), out z) && z == 0)
                {
                    row.Cells["Service Fee"].Style.ForeColor = Color.Black;
                    row.Cells["Service Fee"].Style.SelectionBackColor = SystemColors.Highlight;
                }

                else
                {
                    row.Cells["Service Fee"].Style.ForeColor = Color.MediumBlue;
                    row.Cells["Service Fee"].Style.SelectionBackColor = Color.MediumBlue;
                }



                if (Double.TryParse(row.Cells["1701"].Value.ToString().Replace("Php ", ""), out z) && z == 0)
                {
                    row.Cells["1701"].Style.ForeColor = Color.Black;
                    row.Cells["1701"].Style.SelectionBackColor = SystemColors.Highlight;
                }

                else
                {
                    row.Cells["1701"].Style.ForeColor = Color.MediumBlue;
                    row.Cells["1701"].Style.SelectionBackColor = Color.MediumBlue;
                }

                if (Double.TryParse(row.Cells["1702"].Value.ToString().Replace("Php ", ""), out z) && z == 0)
                {
                    row.Cells["1702"].Style.ForeColor = Color.Black;
                    row.Cells["1702"].Style.SelectionBackColor = SystemColors.Highlight;
                }

                else
                {
                    row.Cells["1702"].Style.ForeColor = Color.MediumBlue;
                    row.Cells["1702"].Style.SelectionBackColor = Color.MediumBlue;
                }

                if (Double.TryParse(row.Cells["1604CF"].Value.ToString().Replace("Php ", ""), out z) && z == 0)
                {
                    row.Cells["1604CF"].Style.ForeColor = Color.Black;
                    row.Cells["1604CF"].Style.SelectionBackColor = SystemColors.Highlight;
                }

                else
                {
                    row.Cells["1604CF"].Style.ForeColor = Color.MediumBlue;
                    row.Cells["1604CF"].Style.SelectionBackColor = Color.MediumBlue;
                }

                if (Double.TryParse(row.Cells["1604E"].Value.ToString().Replace("Php ", ""), out z) && z == 0)
                {
                    row.Cells["1604E"].Style.ForeColor = Color.Black;
                    row.Cells["1604E"].Style.SelectionBackColor = SystemColors.Highlight;
                }

                else
                {
                    row.Cells["1604E"].Style.ForeColor = Color.MediumBlue;
                    row.Cells["1604E"].Style.SelectionBackColor = Color.MediumBlue;
                }

                if (Double.TryParse(row.Cells["Municipal License"].Value.ToString().Replace("Php ", ""), out z) && z == 0)
                {
                    row.Cells["Municipal License"].Style.ForeColor = Color.Black;
                    row.Cells["Municipal License"].Style.SelectionBackColor = SystemColors.Highlight;
                }

                else
                {
                    row.Cells["Municipal License"].Style.ForeColor = Color.MediumBlue;
                    row.Cells["Municipal License"].Style.SelectionBackColor = Color.MediumBlue;
                }

                if (Double.TryParse(row.Cells["COR"].Value.ToString().Replace("Php ", ""), out z) && z == 0)
                {
                    row.Cells["COR"].Style.ForeColor = Color.Black;
                    row.Cells["COR"].Style.SelectionBackColor = SystemColors.Highlight;
                }

                else
                {
                    row.Cells["COR"].Style.ForeColor = Color.MediumBlue;
                    row.Cells["COR"].Style.SelectionBackColor = Color.MediumBlue;
                }

                if (Double.TryParse(row.Cells["Bookkeeping"].Value.ToString().Replace("Php ", ""), out z) && z == 0)
                {
                    row.Cells["Bookkeeping"].Style.ForeColor = Color.Black;
                    row.Cells["Bookkeeping"].Style.SelectionBackColor = SystemColors.Highlight;
                }

                else
                {
                    row.Cells["Bookkeeping"].Style.ForeColor = Color.MediumBlue;
                    row.Cells["Bookkeeping"].Style.SelectionBackColor = Color.MediumBlue;
                }

                if (Double.TryParse(row.Cells["Inventory"].Value.ToString().Replace("Php ", ""), out z) && z == 0)
                {
                    row.Cells["Inventory"].Style.ForeColor = Color.Black;
                    row.Cells["Inventory"].Style.SelectionBackColor = SystemColors.Highlight;
                }

                else
                {
                    row.Cells["Inventory"].Style.ForeColor = Color.MediumBlue;
                    row.Cells["Inventory"].Style.SelectionBackColor = Color.MediumBlue;
                }
            }
            #endregion
        }
        #endregion

        #region Bottom Buttons Default
        void CSBBDefault()
        {
            if (dgvCSClients.RowCount == 0)
            {
                btnCSReport.Enabled = false;
                btnCSTransfer.Enabled = false;
                btnCSCert.Enabled = false;
                btnCSPBills.Enabled = false;
            }

            else
            {
                btnCSReport.Enabled = true;
                btnCSTransfer.Enabled = true;
                btnCSCert.Enabled = true;
                btnCSPBills.Enabled = true;
            }
        }
        #endregion

        #region Cell Double Click
        private void dgvCS_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            btnCSTFView_Click(sender, e);
        }
        #endregion

        #region Search

        #region Search Button
        private void btnCSNSearch_MouseEnter(object sender, EventArgs e)
        {
            btnCSNSearch.Visible = false;
            btnCSHSearch.Visible = true;
        }

        private void btnCSHSearch_MouseLeave(object sender, EventArgs e)
        {
            btnCSNSearch.Visible = true;
            btnCSHSearch.Visible = false;
        }

        private void btnCSHSearch_MouseDown(object sender, MouseEventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;

            btnCSHSearch.Visible = false;
            btnCSDSearch.Visible = true;

            CSTSearch();

            cmbCSSOption_SelectedIndexChanged(sender, e);

            Cursor.Current = Cursors.Default;
        }

        private void btnCSDSearch_MouseUp(object sender, MouseEventArgs e)
        {
            btnCSHSearch.Visible = true;
            btnCSDSearch.Visible = false;
        }

        private void btnCSDSearch_MouseLeave(object sender, EventArgs e)
        {
            btnCSHSearch.Visible = true;
            btnCSDSearch.Visible = false;
        }
        #endregion

        #region Search Shortcut Key
        private void txtCSSearch_KeyDown(object sender, KeyEventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;

            if (txtCSSearch.Focused && e.KeyCode == Keys.Enter)
            {
                CSTSearch();

                cmbCSSOption_SelectedIndexChanged(sender, e);
            }

            Cursor.Current = Cursors.Default;
        }
        #endregion

        #region Search On or Off Focus
        private void txtCSSearch_Enter(object sender, EventArgs e)
        {
            if (txtCSSearch.ForeColor == Color.DarkGray)
            {
                txtCSSearch.Clear();
                txtCSSearch.ForeColor = Color.Black;
            }
        }

        private void txtCSSearch_Leave(object sender, EventArgs e)
        {
            if (txtCSSearch.ForeColor == Color.Black && txtCSSearch.Text == "")
            {
                txtCSSearch.Text = "Search";
                txtCSSearch.ForeColor = Color.DarkGray;
            }
        }
        #endregion

        #region Table Search
        void CSTSearch()
        {
            if (txtCSSearch.Text == "" || txtCSSearch.ForeColor == Color.DarkGray) { }

            else if (txtCSSearch.Text.Equals("All", StringComparison.InvariantCultureIgnoreCase) && cmbCSSOption.Text == "All")
            {
                icspage = 1;
                icsp = 0;

                scscsearch = "";
                scsssearch = "";
                scsfsearch = "";

                CSTRestrict();

                CSCLoad(icsp);
                CSMLoad(icsp);
                CSQLoad(icsp);
                CSALoad(icsp);

                CSRCDefault();

                btnCSReport.Enabled = true;
                btnCSTransfer.Enabled = true;
                btnCSCert.Enabled = true;

                if (!dgvCSClients.Visible)
                {
                    btnCSPBills.Enabled = true;
                }
            }

            else
            {
                icspage = 1;
                icsp = 0;

                CSSOption();
                CSTRestrict();

                CSCLoad(icsp);
                CSMLoad(icsp);
                CSQLoad(icsp);
                CSALoad(icsp);

                CSRCDefault();

                if (cDatasetCS.Rows.Count == 0)
                {
                    btnCSDSearch.Visible = false;
                    btnCSReport.Enabled = false;
                    btnCSTransfer.Enabled = false;
                    btnCSCert.Enabled = false;
                    btnCSPBills.Enabled = false;

                    CSClear();
                    CSMClear();
                    CSQClear();
                    CSAClear();
                }

                else
                {
                    btnCSReport.Enabled = true;
                    btnCSTransfer.Enabled = true;
                    btnCSCert.Enabled = true;

                    if (!dgvCSClients.Visible)
                    {
                        btnCSPBills.Enabled = true;
                    }
                }
            }

            EDButtons();
        }
        #endregion

        #region Search Option
        private void cmbCSSOption_DropDown(object sender, EventArgs e)
        {
            cmbCSSOption.Items.Remove("Search Option");
            cmbCSSOption.ForeColor = Color.Black;
        }

        private void cmbCSSOption_DropDownClosed(object sender, EventArgs e)
        {
            if (cmbCSSOption.SelectedItem == null)
            {
                if (!cmbCSSOption.Items.Contains("Search Option"))
                {
                    cmbCSSOption.Items.Insert(0, "Search Option");
                }

                cmbCSSOption.Text = "Search Option";
                cmbCSSOption.ForeColor = Color.DarkGray;
            }
        }

        private void cmbCSSOption_SelectedIndexChanged(object sender, EventArgs e)
        {
            CSSearch();

            dgvCSMonthly.Columns[4].Visible = false;
            dgvCSMonthly.Columns[5].Visible = false;
            dgvCSMonthly.Columns[6].Visible = false;
            dgvCSMonthly.Columns[7].Visible = false;
            dgvCSMonthly.Columns[8].Visible = false;
            dgvCSMonthly.Columns[9].Visible = false;
            dgvCSMonthly.Columns[10].Visible = false;
            dgvCSMonthly.Columns[11].Visible = false;

            dgvCSQuarterly.Columns[4].Visible = false;
            dgvCSQuarterly.Columns[5].Visible = false;
            dgvCSQuarterly.Columns[6].Visible = false;
            dgvCSQuarterly.Columns[7].Visible = false;
            dgvCSQuarterly.Columns[8].Visible = false;
            dgvCSQuarterly.Columns[9].Visible = false;
            dgvCSQuarterly.Columns[10].Visible = false;
            dgvCSQuarterly.Columns[11].Visible = false;

            dgvCSAnnually.Columns[4].Visible = false;
            dgvCSAnnually.Columns[5].Visible = false;
            dgvCSAnnually.Columns[6].Visible = false;
            dgvCSAnnually.Columns[7].Visible = false;
            dgvCSAnnually.Columns[8].Visible = false;
            dgvCSAnnually.Columns[9].Visible = false;
            dgvCSAnnually.Columns[10].Visible = false;
            dgvCSAnnually.Columns[11].Visible = false;

            if (cmbCSSOption.Text == "Nature of Business")
            {
                dgvCSMonthly.Columns[4].Visible = true;
                dgvCSQuarterly.Columns[4].Visible = true;
                dgvCSAnnually.Columns[4].Visible = true;
            }

            else if (cmbCSSOption.Text == "Business / Owner TIN")
            {
                dgvCSMonthly.Columns[5].Visible = true;
                dgvCSQuarterly.Columns[5].Visible = true;
                dgvCSAnnually.Columns[5].Visible = true;
            }

            else if (cmbCSSOption.Text == "Tax Type")
            {
                dgvCSMonthly.Columns[6].Visible = true;
                dgvCSQuarterly.Columns[6].Visible = true;
                dgvCSAnnually.Columns[6].Visible = true;
            }

            else if (cmbCSSOption.Text == "SEC Reg. Number")
            {
                dgvCSMonthly.Columns[7].Visible = true;
                dgvCSQuarterly.Columns[7].Visible = true;
                dgvCSAnnually.Columns[7].Visible = true;
            }

            else if (cmbCSSOption.Text == "Group Name")
            {
                dgvCSMonthly.Columns[8].Visible = true;
                dgvCSQuarterly.Columns[8].Visible = true;
                dgvCSAnnually.Columns[8].Visible = true;
            }

            else if (cmbCSSOption.Text == "Sub-group Name")
            {
                dgvCSMonthly.Columns[9].Visible = true;
                dgvCSQuarterly.Columns[9].Visible = true;
                dgvCSAnnually.Columns[9].Visible = true;
            }

            else if (cmbCSSOption.Text == "Group Year")
            {
                dgvCSMonthly.Columns[10].Visible = true;
                dgvCSQuarterly.Columns[10].Visible = true;
                dgvCSAnnually.Columns[10].Visible = true;
            }

            else if (cmbCSSOption.Text == "Status")
            {
                dgvCSMonthly.Columns[11].Visible = true;
                dgvCSQuarterly.Columns[11].Visible = true;
                dgvCSAnnually.Columns[11].Visible = true;
            }
        }

        void CSSOption()
        {
            scscs = txtCSSearch.Text.Replace("'", "''");
            scsss = txtCSSearch.Text.Replace("'", "''");

            if (cmbCSSOption.Text == "Business Name")
            {
                scscsearch = " WHERE Business_Name LIKE '" + scscs + "'";
                scsssearch = " WHERE c.Business_Name LIKE '" + scsss + "'";
                scsfsearch = "{Clients.Business Name} = '" + scscs + "'";
            }

            else if (cmbCSSOption.Text == "Nature of Business")
            {
                scscsearch = " WHERE Nature_of_Business LIKE '" + scscs + "'";
                scsssearch = " WHERE c.Nature_of_Business LIKE '" + scsss + "'";
                scsfsearch = "{Clients.Nature of Business} = '" + scscs + "'";
            }

            else if (cmbCSSOption.Text == "Business / Owner TIN")
            {
                scscsearch = " WHERE Business_or_Owner_TIN LIKE '" + scscs + "'";
                scsssearch = " WHERE c.Business_or_Owner_TIN LIKE '" + scsss + "'";
                scsfsearch = "{Clients.Business / Owner TIN} = '" + scscs + "'";
            }

            else if (cmbCSSOption.Text == "Tax Type")
            {
                scscsearch = " WHERE Tax_Type LIKE '" + scscs + "'";
                scsssearch = " WHERE c.Tax_Type LIKE '" + scsss + "'";
                scsfsearch = "{Clients.Tax Type} = '" + scscs + "'";
            }

            else if (cmbCSSOption.Text == "SEC Reg. Number")
            {
                scscsearch = " WHERE SEC_Registration_Number LIKE '" + scscs + "'";
                scsssearch = " WHERE c.SEC_Registration_Number LIKE '" + scsss + "'";
                scsfsearch = "{Clients.SEC Reg. Number} = '" + scscs + "'";
            }

            else if (cmbCSSOption.Text == "Group Name")
            {
                scscsearch = " WHERE Group_Name LIKE '" + scscs + "'";
                scsssearch = " WHERE c.Group_Name LIKE '" + scsss + "'";
                scsfsearch = "{Clients.Group Name} = '" + scscs + "'";
            }

            else if (cmbCSSOption.Text == "Sub-group Name")
            {
                scscsearch = " WHERE Sub_Group_Name LIKE '" + scscs + "'";
                scsssearch = " WHERE c.Sub_Group_Name LIKE '" + scsss + "'";
                scsfsearch = "{Clients.Sub-group Name} = '" + scscs + "'";
            }

            else if (cmbCSSOption.Text == "Group Year")
            {
                scscsearch = " WHERE Group_Year LIKE '" + scscs + "'";
                scsssearch = " WHERE c.Group_Year LIKE '" + scsss + "'";
                scsfsearch = "{Clients.Group Year} = '" + scscs + "'";
            }

            else if (cmbCSSOption.Text == "Status")
            {
                scscsearch = " WHERE Status LIKE '" + scscs + "'";
                scsssearch = " WHERE c.Status LIKE '" + scsss + "'";
                scsfsearch = "{Clients.Status} = '" + scscs + "'";
            }

            else if (cmbCSSOption.Text == "User in Charge")
            {
                scscsearch = " WHERE Username LIKE '" + scscs + "'";
                scsssearch = " WHERE c.Username LIKE '" + scsss + "'";
                scsfsearch = "{Clients.Username} = '" + scscs + "'";
            }

            else
            {
                scscsearch = " WHERE CONCAT(Last_Name, ', ', First_Name, ' ', Middle_Initial) LIKE '" + scscs + "'";
                scsssearch = " WHERE CONCAT(c.Last_Name, ', ', c.First_Name, ' ', c.Middle_Initial) LIKE '" + scsss + "'";
                scsfsearch = "{Clients.Client Name} = '" + scscs + "'";
            }
        }
        #endregion

        #region Order Option
        private void cmbCSOOption_DropDown(object sender, EventArgs e)
        {
            cmbCSOOption.Items.Remove("Order Option");
            cmbCSOOption.ForeColor = Color.Black;
        }

        private void cmbCSOOption_DropDownClosed(object sender, EventArgs e)
        {
            if (cmbCSOOption.SelectedItem == null)
            {
                if (!cmbCSOOption.Items.Contains("Order Option"))
                {
                    cmbCSOOption.Items.Insert(0, "Order Option");
                }

                cmbCSOOption.Text = "Order Option";
                cmbCSOOption.ForeColor = Color.DarkGray;
            }
        }

        private void cmbCSOOption_SelectedIndexChanged(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;

            if (cmbCSOOption.ForeColor != Color.DarkGray && cmbCSOOption.Text != "Order Option")
            {
                CSOOption();
                CSCLoad(icsp);
                CSMLoad(icsp);
                CSQLoad(icsp);
                CSALoad(icsp);

                cmbCSSOption_SelectedIndexChanged(sender, e);
            }

            Cursor.Current = Cursors.Default;
        }

        void CSOOption()
        {
            if (cmbCSOOption.Text == "Business Name")
            {
                scscorder = " ORDER BY Business_Name";
                scssorder = " ORDER BY c.Business_Name";
                scsfield = "Business Name";
            }

            else if (cmbCSOOption.Text == "Group Name")
            {
                scscorder = " ORDER BY Group_Name";
                scssorder = " ORDER BY c.Group_Name";
                scsfield = "Group Name";
            }

            else if (cmbCSOOption.Text == "Sub-group Name")
            {
                scscorder = " ORDER BY Sub_Group_Name";
                scssorder = " ORDER BY c.Sub_Group_Name";
                scsfield = "Sub-group Name";
            }

            else if (cmbCSOOption.Text == "Group Year")
            {
                scscorder = " ORDER BY Group_Year";
                scssorder = " ORDER BY c.Group_Year";
                scsfield = "Group Year";
            }

            else
            {
                scscorder = " ORDER BY CONCAT(Last_Name, ', ', First_Name, ' ', Middle_Initial)";
                scssorder = " ORDER BY CONCAT(c.Last_Name, ', ', c.First_Name, ' ', c.Middle_Initial)";
                scsfield = "Client Name";
            }
        }
        #endregion

        #region Auto Complete Search
        void CSSearch()
        {
            string uname = "";

            if(Login.psUType == "Staff")
            {
                uname = " WHERE Username = '" + Login.psUName + "'";
            }

            else
            {
                uname = "";
            }

            AutoCompleteStringCollection acs = new AutoCompleteStringCollection();

            string cQuery = "SELECT CONCAT(Last_Name, ', ', First_Name, ' ', Middle_Initial),"
                + " Business_Name, Nature_of_Business, Business_or_Owner_TIN, Tax_Type,"
                + " SEC_Registration_Number, Group_Name, Sub_Group_Name, Group_Year,"
                + " Status, Username FROM tbl_clients" + uname + ";";
            MySqlConnection cConnection = new MySqlConnection(Conn.cString);
            MySqlCommand cCommand = new MySqlCommand(cQuery, cConnection);
            MySqlDataReader cReader;

            try
            {
                cConnection.Open();
                cReader = cCommand.ExecuteReader();

                acs.Clear();

                while (cReader.Read())
                {
                    string cn, bn, nob, bot, tt, sec, gn, sgn, gy, st, un;

                    cn = cReader.GetString(0);
                    bn = cReader.GetString(1);
                    nob = cReader.GetString(2);
                    bot = cReader.GetString(3);
                    tt = cReader.GetString(4);
                    sec = cReader.GetString(5);
                    gn = cReader.GetString(6);
                    sgn = cReader.GetString(7);
                    gy = cReader.GetString(8);
                    st = cReader.GetString(9);
                    un = cReader.GetString(10);

                    if (cmbCSSOption.Text == "All")
                    {
                        acs.Add("All");
                        acs.Add(cn);
                    }

                    if (cmbCSSOption.Text == "Business Name")
                    {
                        acs.Add(bn);
                    }

                    else if (cmbCSSOption.Text == "Nature of Business")
                    {
                        acs.Add(nob);
                    }

                    else if (cmbCSSOption.Text == "Business / Owner TIN")
                    {
                        acs.Add(bot);
                    }

                    else if (cmbCSSOption.Text == "Tax Type")
                    {
                        acs.Add(tt);
                    }

                    else if (cmbCSSOption.Text == "SEC Reg. Number")
                    {
                        acs.Add(sec);
                    }

                    else if (cmbCSSOption.Text == "Group Name")
                    {
                        acs.Add(gn);
                    }

                    else if (cmbCSSOption.Text == "Sub-group Name")
                    {
                        acs.Add(sgn);
                    }

                    else if (cmbCSSOption.Text == "Group Year")
                    {
                        acs.Add(gy);
                    }

                    else if (cmbCSSOption.Text == "Status")
                    {
                        acs.Add(st);
                    }

                    else if (cmbCSSOption.Text == "User in Charge")
                    {
                        acs.Add(un);
                    }

                    else
                    {
                        acs.Add(cn);
                    }
                }
            }

            catch (MySqlException ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            finally
            {
                cConnection.Close();
            }

            txtCSSearch.AutoCompleteCustomSource = acs;
        }
        #endregion

        #region Enable Tooltip
        private void txtCSSearch_ForeColorChanged(object sender, EventArgs e)
        {
            TTip();
        }

        private void cmbCSSOption_ForeColorChanged(object sender, EventArgs e)
        {
            TTip();
        }

        private void cmbCSOOption_ForeColorChanged(object sender, EventArgs e)
        {
            TTip();
        }
        #endregion

        #endregion

        #region Staff Table Restriction
        void CSTRestrict()
        {
            if(Login.psUType == "Staff")
            {
                if (scscsearch == "")
                {
                    scsuname = " WHERE Username = '" + Login.psUName + "'";
                    scssuname = " WHERE c.Username = '" + Login.psUName + "'";
                }

                else
                {
                    scsuname = " AND Username = '" + Login.psUName + "'";
                    scssuname = " AND c.Username = '" + Login.psUName + "'";
                }
            }

            else
            {
                scsuname = "";
                scssuname = "";
            }
        }
        #endregion

        #region Services Drop Down
        Timer thide;
        bool bcsservices, bbbilling, bulogs, bplogs;
        string csddstatus = "*Close";

        private void HPanel(object sender, EventArgs e)
        {
            thide.Stop();
            if (!bcsservices) flpCSDDown.Visible = false;
            if (!bcsservices) btnCSServices.Text = "   Services     ►";

            if (!bbbilling) flpBPDDown.Visible = false;
            if (!bbbilling) btnBPServices.Text = "   Services     ►";

            if (!bplogs) flpPLDDown.Visible = false;
            if (!bplogs) btnPLServices.Text = "   Services     ►";

            if (!bulogs) flpULDDown.Visible = false;
            if (!bulogs) btnULManage.Text = "  Manage   ►";
        }

        void CSDOpen()
        {
            if (csddstatus == "*Open")
            {
                btnCSServices.BackColor = Color.LimeGreen;
                bcsservices = true;
                flpCSDDown.Visible = true;
            }
        }

        void CSDClose()
        {
            if (!dgvCSClients.Visible && !flpCSCFields.Visible)
            {
                bcsservices = false;
                thide.Start();
            }

            else
            {
                btnCSServices.BackColor = Color.FromArgb(64, 64, 64);
                bcsservices = false;
                thide.Start();
            }
        }

        private void btnCSServices_MouseEnter(object sender, EventArgs e)
        {
            if (csddstatus == "*Open")
            {
                btnCSServices.Text = "   Services     ▼";
                bcsservices = true;
                flpCSDDown.Visible = true;
            }
        }

        private void btnCSServices_MouseLeave(object sender, EventArgs e)
        {
            bcsservices = false;
            thide.Start();
        }

        private void btnCSMonthly_MouseEnter(object sender, EventArgs e)
        {
            CSDOpen();
        }

        private void btnCSMonthly_MouseLeave(object sender, EventArgs e)
        {
            CSDClose();
        }

        private void btnCSQuarterly_MouseEnter(object sender, EventArgs e)
        {
            CSDOpen();
        }

        private void btnCSQuarterly_MouseLeave(object sender, EventArgs e)
        {
            CSDClose();
        }

        private void btnCSAnnually_MouseEnter(object sender, EventArgs e)
        {
            CSDOpen();
        }

        private void btnCSAnnually_MouseLeave(object sender, EventArgs e)
        {
            CSDClose();
        }
        #endregion

        #region Client and Services Navigation
        void CSNBDefault()
        {
            btnCSClients.BackColor = Color.FromArgb(64, 64, 64);
            btnCSClients.FlatAppearance.MouseOverBackColor = Color.LimeGreen;
            btnCSClients.FlatAppearance.MouseDownBackColor = Color.ForestGreen;
            btnCSClients.Cursor = Cursors.Hand;

            btnCSServices.BackColor = Color.FromArgb(64, 64, 64);
            btnCSServices.FlatAppearance.MouseOverBackColor = Color.LimeGreen;
            btnCSServices.FlatAppearance.MouseDownBackColor = Color.ForestGreen;

            btnCSMonthly.BackColor = Color.FromArgb(64, 64, 64); ;
            btnCSMonthly.FlatAppearance.MouseOverBackColor = Color.LimeGreen;
            btnCSMonthly.FlatAppearance.MouseDownBackColor = Color.ForestGreen;
            btnCSMonthly.Cursor = Cursors.Hand;

            btnCSQuarterly.BackColor = Color.FromArgb(64, 64, 64); ;
            btnCSQuarterly.FlatAppearance.MouseOverBackColor = Color.LimeGreen;
            btnCSQuarterly.FlatAppearance.MouseDownBackColor = Color.ForestGreen;
            btnCSQuarterly.Cursor = Cursors.Hand;

            btnCSAnnually.BackColor = Color.FromArgb(64, 64, 64); ;
            btnCSAnnually.FlatAppearance.MouseOverBackColor = Color.LimeGreen;
            btnCSAnnually.FlatAppearance.MouseDownBackColor = Color.ForestGreen;
            btnCSAnnually.Cursor = Cursors.Hand;

            if (!btnCSClients.Enabled)
            {
                EDButtons();
            }
        }

        void CSCCell()
        {
            if (!dgvCSClients.Visible)
            {
                foreach (DataGridViewRow row in dgvCSClients.Rows)
                {
                    if (row.Cells[3].Value.ToString() == psCSBName)
                    {
                        dgvCSClients.CurrentCell = dgvCSClients[3, row.Index];
                    }
                }
            }

            if (!dgvCSMonthly.Visible)
            {
                foreach (DataGridViewRow row in dgvCSMonthly.Rows)
                {
                    if (row.Cells[3].Value.ToString() == psCSBName)
                    {
                        dgvCSMonthly.CurrentCell = dgvCSMonthly[3, row.Index];
                    }
                }
            }

            if (!dgvCSQuarterly.Visible)
            {
                foreach (DataGridViewRow row in dgvCSQuarterly.Rows)
                {
                    if (row.Cells[3].Value.ToString() == psCSBName)
                    {
                        dgvCSQuarterly.CurrentCell = dgvCSQuarterly[3, row.Index];
                    }
                }
            }

            if (!dgvCSAnnually.Visible)
            {
                foreach (DataGridViewRow row in dgvCSAnnually.Rows)
                {
                    if (row.Cells[3].Value.ToString() == psCSBName)
                    {
                        dgvCSAnnually.CurrentCell = dgvCSAnnually[3, row.Index];
                    }
                }
            }
        }

        private void btnCSClients_Click(object sender, EventArgs e)
        {
            if (!dgvCSClients.Visible && !flpCSCFields.Visible)
            {
                CSNBDefault();

                btnCSClients.BackColor = Color.LimeGreen;
                btnCSClients.FlatAppearance.MouseOverBackColor = Color.LimeGreen;
                btnCSClients.FlatAppearance.MouseDownBackColor = Color.LimeGreen;
                btnCSClients.Cursor = Cursors.Default;

                CSCDefault();
                TBCDefault();

                if (btnCSTFView.Text == "Field Vie&w")
                {
                    dgvCSClients.Visible = true;
                    flpCSTTControls.Visible = true;
                    tlpCSTBControls.Visible = true;

                    btnCSPBills.Enabled = false;

                    if (dgvCSClients.RowCount != 0)
                    {
                        btnCSReport.Enabled = true;
                        btnCSTransfer.Enabled = true;
                        btnCSCert.Enabled = true;
                        btnCSPBills.Enabled = true;
                    }

                    else
                    {
                        btnCSReport.Enabled = false;
                        btnCSTransfer.Enabled = false;
                        btnCSCert.Enabled = false;
                        btnCSPBills.Enabled = false;
                    }

                    btnCSReport.Visible = true;

                    if (Login.psUType == "Admin")
                    {
                        btnCSTransfer.Visible = true;
                    }

                    btnCSCert.Visible = true;
                    btnCSPBills.Visible = false;

                    EDButtons();
                }

                else
                {
                    flpCSCFields.Visible = true;
                    flpCSFTCControls.Visible = true;
                    flpCSFBControls.Visible = true;
                }
            }
        }

        private void btnCSServices_Click(object sender, EventArgs e)
        {
            if (csddstatus == "*Close")
            {
                csddstatus = "*Open";

                btnCSServices.Text = "   Services     ▼";
                bcsservices = true;
                flpCSDDown.Visible = true;
            }

            else
            {
                csddstatus = "*Close";

                bcsservices = false;
                thide.Start();
            }
        }

        private void btnCSMonthly_Click(object sender, EventArgs e)
        {
            if (!dgvCSMonthly.Visible && !flpCSMFields.Visible)
            {
                CSNBDefault();

                btnCSServices.BackColor = Color.LimeGreen;
                btnCSServices.FlatAppearance.MouseOverBackColor = Color.LimeGreen;

                btnCSMonthly.BackColor = Color.LimeGreen;
                btnCSMonthly.FlatAppearance.MouseOverBackColor = Color.LimeGreen;
                btnCSMonthly.FlatAppearance.MouseDownBackColor = Color.LimeGreen;
                btnCSMonthly.Cursor = Cursors.Default;

                CSCDefault();
                TBCDefault();

                if (btnCSTFView.Text == "Field Vie&w")
                {
                    dgvCSMonthly.Visible = true;
                    flpCSTTControls.Visible = true;
                    tlpCSTBControls.Visible = true;

                    if (dgvCSClients.RowCount != 0)
                    {
                        btnCSReport.Enabled = true;
                        btnCSTransfer.Enabled = true;
                        btnCSCert.Enabled = true;
                        btnCSPBills.Enabled = true;
                    }

                    else
                    {
                        btnCSReport.Enabled = false;
                        btnCSTransfer.Enabled = false;
                        btnCSCert.Enabled = false;
                        btnCSPBills.Enabled = false;
                    }

                    btnCSReport.Visible = false;
                    btnCSTransfer.Visible = false;
                    btnCSCert.Visible = false;
                    btnCSPBills.Visible = true;

                    EDButtons();
                }

                else
                {
                    flpCSMFields.Visible = true;
                    flpCSFTSControls.Visible = true;
                    flpCSFBControls.Visible = true;
                }
            }

            btnCSServices.Focus();
        }

        private void btnCSQuarterly_Click(object sender, EventArgs e)
        {
            if (!dgvCSQuarterly.Visible && !flpCSQFields.Visible)
            {
                CSNBDefault();

                btnCSServices.BackColor = Color.LimeGreen;
                btnCSServices.FlatAppearance.MouseOverBackColor = Color.LimeGreen;

                btnCSQuarterly.BackColor = Color.LimeGreen;
                btnCSQuarterly.FlatAppearance.MouseOverBackColor = Color.LimeGreen;
                btnCSQuarterly.FlatAppearance.MouseDownBackColor = Color.LimeGreen;
                btnCSQuarterly.Cursor = Cursors.Default;

                CSCDefault();
                TBCDefault();

                if (btnCSTFView.Text == "Field Vie&w")
                {
                    dgvCSQuarterly.Visible = true;
                    flpCSTTControls.Visible = true;
                    tlpCSTBControls.Visible = true;

                    if (dgvCSClients.RowCount != 0)
                    {
                        btnCSReport.Enabled = true;
                        btnCSTransfer.Enabled = true;
                        btnCSCert.Enabled = true;
                        btnCSPBills.Enabled = true;
                    }

                    else
                    {
                        btnCSReport.Enabled = false;
                        btnCSTransfer.Enabled = false;
                        btnCSCert.Enabled = false;
                        btnCSPBills.Enabled = false;
                    }

                    btnCSReport.Visible = false;
                    btnCSTransfer.Visible = false;
                    btnCSCert.Visible = false;
                    btnCSPBills.Visible = true;

                    EDButtons();
                }

                else
                {
                    flpCSQFields.Visible = true;
                    flpCSFTSControls.Visible = true;
                    flpCSFBControls.Visible = true;
                }
            }

            btnCSServices.Focus();
        }

        private void btnCSAnnually_Click(object sender, EventArgs e)
        {
            if (!dgvCSAnnually.Visible && !flpCSAFields.Visible)
            {
                CSNBDefault();

                btnCSServices.BackColor = Color.LimeGreen;
                btnCSServices.FlatAppearance.MouseOverBackColor = Color.LimeGreen;

                btnCSAnnually.BackColor = Color.LimeGreen;
                btnCSAnnually.FlatAppearance.MouseOverBackColor = Color.LimeGreen;
                btnCSAnnually.FlatAppearance.MouseDownBackColor = Color.LimeGreen;
                btnCSAnnually.Cursor = Cursors.Default;

                CSCDefault();
                TBCDefault();

                if (btnCSTFView.Text == "Field Vie&w")
                {
                    dgvCSAnnually.Visible = true;
                    flpCSTTControls.Visible = true;
                    tlpCSTBControls.Visible = true;

                    if (dgvCSClients.RowCount != 0)
                    {
                        btnCSReport.Enabled = true;
                        btnCSTransfer.Enabled = true;
                        btnCSCert.Enabled = true;
                        btnCSPBills.Enabled = true;
                    }

                    else
                    {
                        btnCSReport.Enabled = false;
                        btnCSTransfer.Enabled = false;
                        btnCSCert.Enabled = false;
                        btnCSPBills.Enabled = false;
                    }

                    btnCSReport.Visible = false;
                    btnCSTransfer.Visible = false;
                    btnCSCert.Visible = false;
                    btnCSPBills.Visible = true;

                    EDButtons();
                }

                else
                {
                    flpCSAFields.Visible = true;
                    flpCSFTSControls.Visible = true;
                    flpCSFBControls.Visible = true;
                }
            }

            btnCSServices.Focus();
        }
        #endregion

        #region Field and Table View Navigation
        void CSCDefault()
        {
            dgvCSClients.Visible = false;
            dgvCSMonthly.Visible = false;
            dgvCSQuarterly.Visible = false;
            dgvCSAnnually.Visible = false;
            flpCSCFields.Visible = false;
            flpCSMFields.Visible = false;
            flpCSQFields.Visible = false;
            flpCSAFields.Visible = false;
        }

        void TBCDefault()
        {
            flpCSTTControls.Visible = false;
            flpCSFTCControls.Visible = false;
            flpCSFTSControls.Visible = false;

            flpCSFBControls.Visible = false;
            tlpCSTBControls.Visible = false;
        }

        private void btnCSTFView_Click(object sender, EventArgs e)
        {
            if (btnCSTFView.Text == "Field Vie&w")
            {
                btnCSTFView.Text = "Table Vie&w";
                btnCSRefresh.Visible = false;

                CSCDefault();
                TBCDefault();

                pnlCSContent.Padding = new Padding(1, 1, 1, 1);

                if (btnCSClients.BackColor == Color.LimeGreen)
                {
                    flpCSCFields.Visible = true;
                    flpCSFTCControls.Visible = true;

                    if (dgvCSClients.Rows.Count == 0)
                    {
                        CSClear();
                        CSMClear();
                        CSQClear();
                        CSAClear();
                    }

                    else
                    {
                        CSSDAll();
                    }
                }

                else if (btnCSMonthly.BackColor == Color.LimeGreen)
                {
                    flpCSMFields.Visible = true;
                    flpCSFTSControls.Visible = true;

                    if (dgvCSClients.Rows.Count == 0)
                    {
                        CSClear();
                        CSMClear();
                        CSQClear();
                        CSAClear();
                    }

                    else
                    {
                        CSSDAll();
                    }
                }

                else if (btnCSQuarterly.BackColor == Color.LimeGreen)
                {
                    flpCSQFields.Visible = true;
                    flpCSFTSControls.Visible = true;

                    if (dgvCSClients.Rows.Count == 0)
                    {
                        CSClear();
                        CSMClear();
                        CSQClear();
                        CSAClear();
                    }

                    else
                    {
                        CSSDAll();
                    }
                }

                else if (btnCSAnnually.BackColor == Color.LimeGreen)
                {
                    flpCSAFields.Visible = true;
                    flpCSFTSControls.Visible = true;

                    if (dgvCSClients.Rows.Count == 0)
                    {
                        CSClear();
                        CSMClear();
                        CSQClear();
                        CSAClear();
                    }

                    else
                    {
                        CSSDAll();
                    }
                }

                flpCSFBControls.Visible = true;
            }

            else
            {
                btnCSTFView.Text = "Field Vie&w";
                btnCSRefresh.Visible = true;

                CSCDefault();
                TBCDefault();

                pnlCSContent.Padding = new Padding(0, 0, 0, 0);

                if (btnCSClients.BackColor == Color.LimeGreen)
                {
                    TFTrigger();

                    btnCSClients_Click(sender, e);

                    psTFTrigger = "";
                }

                else if (btnCSMonthly.BackColor == Color.LimeGreen)
                {
                    TFTrigger();

                    btnCSMonthly_Click(sender, e);

                    psTFTrigger = "";
                }

                else if (btnCSQuarterly.BackColor == Color.LimeGreen)
                {
                    TFTrigger();

                    btnCSQuarterly_Click(sender, e);

                    psTFTrigger = "";
                }

                else if (btnCSAnnually.BackColor == Color.LimeGreen)
                {
                    TFTrigger();

                    btnCSAnnually_Click(sender, e);

                    psTFTrigger = "";
                }

                flpCSTTControls.Visible = true;
                tlpCSTBControls.Visible = true;
            }

            txtCSSearch.Text = "Search";
            txtCSSearch.ForeColor = Color.DarkGray;
        }
        #endregion

        #region Pagination
        void CSPage()
        {
            dcspage = dcscount / icsdpage;
            dcstpage = Math.Truncate(dcspage);
            scspage = Convert.ToString(dcspage);

            if (scspage.Contains("."))
            {
                dcsrpage = dcstpage + 1;
            }

            else
            {
                dcsrpage = dcstpage;
            }
        }
        #endregion

        #region Row Count Default
        void CSRCDefault()
        {
            CSPage();

            if (icspage == 1 && dcsrpage <= 1)
            {
                btnCSFirst.Enabled = false;
                btnCSBack.Enabled = false;
                btnCSNext.Enabled = false;
                btnCSLast.Enabled = false;
            }

            else if (icspage == 1 && dcsrpage > 1)
            {
                btnCSFirst.Enabled = false;
                btnCSBack.Enabled = false;
                btnCSNext.Enabled = true;
                btnCSLast.Enabled = true;
            }

            else if (icspage > 1 && icspage < dcsrpage)
            {
                btnCSFirst.Enabled = true;
                btnCSBack.Enabled = true;
                btnCSNext.Enabled = true;
                btnCSLast.Enabled = true;
            }

            else if (icspage == dcsrpage)
            {
                btnCSFirst.Enabled = true;
                btnCSBack.Enabled = true;
                btnCSNext.Enabled = false;
                btnCSLast.Enabled = false;
            }

            EDButtons();
        }
        #endregion

        #region Pagination Focus Control
        void CSPFC()
        {
            if (btnCSClients.BackColor == Color.LimeGreen)
            {
                dgvCSClients.Focus();
            }

            else if (btnCSMonthly.BackColor == Color.LimeGreen)
            {
                dgvCSMonthly.Focus();
            }

            else if (btnCSQuarterly.BackColor == Color.LimeGreen)
            {
                dgvCSQuarterly.Focus();
            }

            else if (btnCSAnnually.BackColor == Color.LimeGreen)
            {
                dgvCSAnnually.Focus();
            }
        }
        #endregion

        #region First
        private void btnCSFirst_Click(object sender, EventArgs e)
        {
            icspage = 1;
            icsp = 0;

            CSCLoad(icsp);
            CSMLoad(icsp);
            CSQLoad(icsp);
            CSALoad(icsp);

            cmbCSSOption_SelectedIndexChanged(sender, e);

            CSRCDefault();

            CSPFC();
        }
        #endregion

        #region Back
        private void btnCSBack_Click(object sender, EventArgs e)
        {
            CSPage();

            icspage--;

            if (icspage == 1)
            {
                btnCSFirst.Enabled = false;
                btnCSBack.Enabled = false;
            }

            if (icspage < dcsrpage)
            {
                btnCSNext.Enabled = true;
                btnCSLast.Enabled = true;
            }

            icsp = icsp - icsdpage;
            CSCLoad(icsp);
            CSMLoad(icsp);
            CSQLoad(icsp);
            CSALoad(icsp);

            cmbCSSOption_SelectedIndexChanged(sender, e);

            EDButtons();

            CSPFC();
        }
        #endregion

        #region Next
        private void btnCSNext_Click(object sender, EventArgs e)
        {
            CSPage();

            icspage++;

            if (icspage == dcsrpage)
            {
                btnCSNext.Enabled = false;
                btnCSLast.Enabled = false;
            }

            if (icspage > 1)
            {
                btnCSFirst.Enabled = true;
                btnCSBack.Enabled = true;
            }

            icsp = icsp + icsdpage;
            CSCLoad(icsp);
            CSMLoad(icsp);
            CSQLoad(icsp);
            CSALoad(icsp);

            cmbCSSOption_SelectedIndexChanged(sender, e);

            EDButtons();

            CSPFC();
        }
        #endregion

        #region Last
        private void btnCSLast_Click(object sender, EventArgs e)
        {
            CSPage();

            icsp = Convert.ToInt32((dcsrpage * icsdpage) - icsdpage);
            icspage = Convert.ToInt32(dcsrpage);

            CSCLoad(icsp);
            CSMLoad(icsp);
            CSQLoad(icsp);
            CSALoad(icsp);

            btnCSFirst.Enabled = true;
            btnCSBack.Enabled = true;
            btnCSNext.Enabled = false;
            btnCSLast.Enabled = false;

            cmbCSSOption_SelectedIndexChanged(sender, e);

            EDButtons();

            CSPFC();
        }
        #endregion

        #region Cell Enter
        private void dgvCS_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            DataGridView dgv = (DataGridView)sender;

            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgv.Rows[e.RowIndex];

                psCSBName = row.Cells["Business Name"].Value.ToString();

                if (dgvCSClients.RowCount == 0)
                {
                    CSSDAll();
                }

                BeginInvoke(new MethodInvoker(CSCCell));
            }

            CSTColors();
        }
        #endregion

        #region Produce Bills
        private void btnCSPBills_Click(object sender, EventArgs e)
        {
            if (dgvCSMonthly.Visible)
            {
                PBills.psFrequency = "*Monthly";

                PBills.psSearch = scsssearch;

                PBills.psUName = scssuname;

                PBills w = new PBills();
                w.ShowDialog();
            }

            else if (dgvCSQuarterly.Visible)
            {
                PBills.psFrequency = "*Quarterly";

                PBills.psSearch = scsssearch;

                PBills.psUName = scssuname;

                PBills w = new PBills();
                w.ShowDialog();
            }

            else if (dgvCSAnnually.Visible)
            {
                PBills.psFrequency = "*Annually";

                PBills.psSearch = scsssearch;

                PBills.psUName = scssuname;

                PBills w = new PBills();
                w.ShowDialog();
            }
        }
        #endregion

        #region Transfer
        private void btnCSTransfer_Click(object sender, EventArgs e)
        {
            psTSearch = scscsearch;
            Transfer w = new Transfer();
            w.ShowDialog();
        }
        #endregion

        #region Report
        private void btnCSReport_Click(object sender, EventArgs e)
        {
            string uname = "";

            if(Login.psUType == "Staff")
            {
                if(scsfsearch == "")
                {
                    uname = "{Clients.Username} = '" + Login.psUName + "'";
                }
                

                else
                {
                    uname = " and {Clients.Username} = '" + Login.psUName + "'";
                }
            }

            else
            {
                uname = "";
            }

            Cursor.Current = Cursors.WaitCursor;

            ReportDocument rd = new ReportDocument();
            rd.Load(Application.StartupPath + "\\CRCS\\CRClients.rpt");

            string crf = scsfsearch + uname + ";";
            crViewer.SelectionFormula = crf;

            FieldDefinition fd = rd.Database.Tables[0].Fields[scsfield];
            rd.DataDefinition.SortFields[0].Field = fd;

            crViewer.ReportSource = rd;
            crViewer.Refresh();
            crViewer.RefreshReport();

            btnRBack.Visible = true;

            crViewer.Visible = true;
            tlpCS.Visible = false;

            Cursor.Current = Cursors.Default;
        }
        #endregion

        #region Certificate
        private void btnCSCert_Click(object sender, EventArgs e)
        {
            CRTClear();

            string fname = "", mi = "", lname = "";

            string cQuery = "SELECT First_Name, Last_Name, Middle_Initial FROM tbl_clients WHERE Business_Name = '" + psCSBName.Replace("'", "''") + "';";
            MySqlConnection cConnection = new MySqlConnection(Conn.cString);
            MySqlCommand cCommand = new MySqlCommand(cQuery, cConnection);
            MySqlDataReader cReader;

            try
            {
                cConnection.Open();
                cReader = cCommand.ExecuteReader();
                while (cReader.Read())
                {
                    fname = cReader.GetValue(0).ToString();
                    lname = cReader.GetValue(1).ToString();
                    mi = cReader.GetValue(2).ToString();
                }
            }

            catch (MySqlException ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            finally
            {
                cConnection.Close();
            }

            Certificate.psFName = fname;
            Certificate.psLName = lname;
            Certificate.psMI = mi;

            Certificate.psWCall = "CS";

            Certificate w = new Certificate();
            w.ShowDialog();
        }
        #endregion

        #region Refresh
        private void btnCSRefresh_Click(object sender, EventArgs e)
        {
            CSCLoad(icsp);
            CSMLoad(icsp);
            CSQLoad(icsp);
            CSALoad(icsp);

            CSRCDefault();

            cmbCSSOption_SelectedIndexChanged(sender, e);

            EDButtons();
        }
        #endregion

        //----------------------------------------------------------------------// Clients and Services Field View

        #region Load List
        int icsl = 0;
        string sgbname = "";
        void CSLLoad()
        {
            #region Clients
            MySqlConnection cConn = new MySqlConnection(Conn.cString);
            MySqlCommand cCommand = new MySqlCommand("SELECT * FROM tbl_clients WHERE Business_Name = '" + psCSBName.Replace("'", "''") + "';", cConn);

            try
            {
                MySqlDataAdapter cAdapter = new MySqlDataAdapter();
                cAdapter.SelectCommand = cCommand;
                cDatalistCS = new DataTable();
                cAdapter.Fill(cDatalistCS);
                cAdapter.Update(cDatalistCS);
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            #endregion

            #region Monthly Services
            MySqlConnection cConnI = new MySqlConnection(Conn.cString);
            MySqlCommand cCommandI = new MySqlCommand("SELECT s.No_ID,"
                + " s.Client_ID,"
                + " IFNULL(CAST(s.Retainers_Fee_M AS DECIMAL(20, 2)), 0),"
                + " IFNULL(CAST(s.Professional_Fee_M AS DECIMAL(20, 2)), 0),"
                + " IFNULL(CAST(s.Service_Fee_M AS DECIMAL(20, 2)), 0),"
                + " IFNULL(CAST(s.VAT AS DECIMAL(20, 2)), 0),"
                + " IFNULL(CAST(s.Non_VAT AS DECIMAL(20, 2)), 0),"
                + " IFNULL(CAST(s.D1601C AS DECIMAL(20, 2)), 0),"
                + " IFNULL(CAST(s.D1601E AS DECIMAL(20, 2)), 0),"
                + " IFNULL(CAST(s.SSS_ER AS DECIMAL(20, 2)), 0),"
                + " IFNULL(CAST(s.PHIC_ER AS DECIMAL(20, 2)), 0),"
                + " IFNULL(CAST(s.Pag_IBIG_ER AS DECIMAL(20, 2)), 0),"
                + " IFNULL(CAST(s.SSS_EE AS DECIMAL(20, 2)), 0),"
                + " IFNULL(CAST(s.PHIC_EE AS DECIMAL(20, 2)), 0),"
                + " IFNULL(CAST(s.Pag_IBIG_EE AS DECIMAL(20, 2)), 0),"
                + " IFNULL(CAST(s.Bookkeeping_M AS DECIMAL(20, 2)), 0),"
                + " IFNULL(CAST(s.Inventory_M AS DECIMAL(20, 2)), 0),"
                + " CONCAT(c.Last_Name, ', ', c.First_Name, ' ', c.Middle_Initial) AS 'Client Name',"
                + " c.Business_Name,"
                + " IFNULL(CAST(s.Certification_Fee AS DECIMAL(20, 2)), 0) AS 'Certification Fee'"
                + " FROM tbl_mservices s LEFT JOIN tbl_clients c ON s.Client_ID = c.Client_ID WHERE c.Business_Name = '" + psCSBName.Replace("'", "''") + "';", cConnI);

            try
            {
                MySqlDataAdapter cAdapterI = new MySqlDataAdapter();
                cAdapterI.SelectCommand = cCommandI;
                cDatalistCSM = new DataTable();
                cAdapterI.Fill(cDatalistCSM);
                cAdapterI.Update(cDatalistCSM);
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            #endregion

            #region Quarterly Services
            MySqlConnection cConnII = new MySqlConnection(Conn.cString);
            MySqlCommand cCommandII = new MySqlCommand("SELECT s.No_ID,"
                + " s.Client_ID,"
                + " IFNULL(CAST(s.Retainers_Fee_Q AS DECIMAL(20, 2)), 0),"
                + " IFNULL(CAST(s.Professional_Fee_Q AS DECIMAL(20, 2)), 0),"
                + " IFNULL(CAST(s.Service_Fee_Q AS DECIMAL(20, 2)), 0),"
                + " IFNULL(CAST(s.D1701Q AS DECIMAL(20, 2)), 0),"
                + " IFNULL(CAST(s.D1702Q AS DECIMAL(20, 2)), 0),"
                + " IFNULL(CAST(s.Bookkeeping_Q AS DECIMAL(20, 2)), 0),"
                + " IFNULL(CAST(s.Inventory_Q AS DECIMAL(20, 2)), 0),"
                + " CONCAT(c.Last_Name, ', ', c.First_Name, ' ', c.Middle_Initial) AS 'Client Name',"
                + " c.Business_Name"
                + " FROM tbl_qservices s LEFT JOIN tbl_clients c ON s.Client_ID = c.Client_ID WHERE c.Business_Name = '" + psCSBName.Replace("'", "''") + "';", cConnII);

            try
            {
                MySqlDataAdapter cAdapterII = new MySqlDataAdapter();
                cAdapterII.SelectCommand = cCommandII;
                cDatalistCSQ = new DataTable();
                cAdapterII.Fill(cDatalistCSQ);
                cAdapterII.Update(cDatalistCSQ);
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            #endregion

            #region Annually Services
            MySqlConnection cConnIII = new MySqlConnection(Conn.cString);
            MySqlCommand cCommandIII = new MySqlCommand("SELECT s.No_ID,"
                + " s.Client_ID,"
                + " IFNULL(CAST(s.Retainers_Fee_A AS DECIMAL(20, 2)), 0),"
                + " IFNULL(CAST(s.Professional_Fee_A AS DECIMAL(20, 2)), 0),"
                + " IFNULL(CAST(s.Service_Fee_A AS DECIMAL(20, 2)), 0),"
                + " IFNULL(CAST(s.D1701 AS DECIMAL(20, 2)), 0),"
                + " IFNULL(CAST(s.D1702 AS DECIMAL(20, 2)), 0),"
                + " IFNULL(CAST(s.D1604CF AS DECIMAL(20, 2)), 0),"
                + " IFNULL(CAST(s.D1604E AS DECIMAL(20, 2)), 0),"
                + " IFNULL(CAST(s.Municipal_License AS DECIMAL(20, 2)), 0),"
                + " IFNULL(CAST(s.COR AS DECIMAL(20, 2)), 0),"
                + " IFNULL(CAST(s.Bookkeeping_A AS DECIMAL(20, 2)), 0),"
                + " IFNULL(CAST(s.Inventory_A AS DECIMAL(20, 2)), 0),"
                + " CONCAT(c.Last_Name, ', ', c.First_Name, ' ', c.Middle_Initial) AS 'Client Name',"
                + " c.Business_Name"
                + " FROM tbl_aservices s LEFT JOIN tbl_clients c ON s.Client_ID = c.Client_ID WHERE c.Business_Name = '" + psCSBName.Replace("'", "''") + "'", cConnIII);

            try
            {
                MySqlDataAdapter cAdapterIII = new MySqlDataAdapter();
                cAdapterIII.SelectCommand = cCommandIII;
                cDatalistCSA = new DataTable();
                cAdapterIII.Fill(cDatalistCSA);
                cAdapterIII.Update(cDatalistCSA);
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            #endregion
        }

        #region Clients
        public void CSSData(int c)
        {
            string cnb = "", tt = "", srd = "", did = "", crd = "";

            if (cDatalistCS.Rows[c][6].ToString() == "Sole Proprietorship")
            {
                cnb = "        Sole Proprietorship";
            }

            else if (cDatalistCS.Rows[c][6].ToString() == "Partnership")
            {
                cnb = "              Partnership";
            }

            else if (cDatalistCS.Rows[c][6].ToString() == "Corporation")
            {
                cnb = "             Corporation";
            }

            if (cDatalistCS.Rows[c][8].ToString() == "VAT")
            {
                tt = "                    VAT";
            }

            else if (cDatalistCS.Rows[c][8].ToString() == "Non-VAT")
            {
                tt = "                Non-VAT";
            }

            if (cDatalistCS.Rows[c][13].ToString() == "")
            {
                srd = DateTime.Now.Date.ToString();
            }

            else
            {
                dtpCSSRDate.CustomFormat = "                MM-dd-yyyy";
                srd = cDatalistCS.Rows[c][13].ToString();
            }

            if (cDatalistCS.Rows[c][15].ToString() == "")
            {
                did = DateTime.Now.Date.ToString();
            }

            else
            {
                dtpCSDIDate.CustomFormat = "                MM-dd-yyyy";
                did = cDatalistCS.Rows[c][15].ToString();
            }

            if (cDatalistCS.Rows[c][17].ToString() == "")
            {
                crd = DateTime.Now.Date.ToString();
            }

            else
            {
                dtpCSCRDate.CustomFormat = "                MM-dd-yyyy";
                crd = cDatalistCS.Rows[c][17].ToString();
            }

            txtCSCID.Text = cDatalistCS.Rows[c][1].ToString();
            txtCSLName.Text = cDatalistCS.Rows[c][2].ToString();
            txtCSFName.Text = cDatalistCS.Rows[c][3].ToString();
            txtCSMI.Text = cDatalistCS.Rows[c][4].ToString();
            txtCSBName.Text = cDatalistCS.Rows[c][5].ToString();
            cmbCSNBusiness.Text = cnb;
            txtCSBOTin.Text = cDatalistCS.Rows[c][7].ToString();
            cmbCSTType.Text = tt;
            txtCSAddress.Text = cDatalistCS.Rows[c][9].ToString();
            txtCSCNumber.Text = cDatalistCS.Rows[c][10].ToString();
            txtCSEAddress.Text = cDatalistCS.Rows[c][11].ToString();
            txtCSSRNumber.Text = cDatalistCS.Rows[c][12].ToString();
            dtpCSSRDate.Text = srd;
            txtCSDNumber.Text = cDatalistCS.Rows[c][14].ToString();
            dtpCSDIDate.Text = did;
            txtCSCRNumber.Text = cDatalistCS.Rows[c][16].ToString();
            dtpCSCRDate.Text = crd;
            txtCSSNumber.Text = cDatalistCS.Rows[c][18].ToString();
            txtCSPINumber.Text = cDatalistCS.Rows[c][19].ToString();
            txtCSPHNumber.Text = cDatalistCS.Rows[c][20].ToString();
            txtCSGName.Text = cDatalistCS.Rows[c][21].ToString();
            txtCSSGName.Text = cDatalistCS.Rows[c][22].ToString();
            cmbCSGYear.Text = "                    " + cDatalistCS.Rows[c][23].ToString();
            txtCSStatus.Text = cDatalistCS.Rows[c][24].ToString();

            if (cDatalistCS.Rows[c][13].ToString() == "")
            {
                dtpCSSRDate.CustomFormat = " ";
            }

            if (cDatalistCS.Rows[c][15].ToString() == "")
            {
                dtpCSDIDate.CustomFormat = " ";
            }

            if (cDatalistCS.Rows[c][17].ToString() == "")
            {
                dtpCSCRDate.CustomFormat = " ";
            }
        }
        #endregion

        #region Monthly Services
        public void CSMSData(int s)
        {
            txtCSMCID.Text = cDatalistCSM.Rows[s][1].ToString();
            txtCSMRFee.Text = string.Format("Php {0:0.00}", cDatalistCSM.Rows[s][2]);
            txtCSMPFee.Text = string.Format("Php {0:0.00}", cDatalistCSM.Rows[s][3]);
            txtCSMSFee.Text = string.Format("Php {0:0.00}", cDatalistCSM.Rows[s][4]);
            txtCSMVat.Text = string.Format("Php {0:0.00}", cDatalistCSM.Rows[s][5]);
            txtCSMNVat.Text = string.Format("Php {0:0.00}", cDatalistCSM.Rows[s][6]);
            txtCSM1601C.Text = string.Format("Php {0:0.00}", cDatalistCSM.Rows[s][7]);
            txtCSM1601E.Text = string.Format("Php {0:0.00}", cDatalistCSM.Rows[s][8]);
            txtCSMSER.Text = string.Format("Php {0:0.00}", cDatalistCSM.Rows[s][9]);
            txtCSMPHER.Text = string.Format("Php {0:0.00}", cDatalistCSM.Rows[s][10]);
            txtCSMPIER.Text = string.Format("Php {0:0.00}", cDatalistCSM.Rows[s][11]);
            txtCSMSEE.Text = string.Format("Php {0:0.00}", cDatalistCSM.Rows[s][12]);
            txtCSMPHEE.Text = string.Format("Php {0:0.00}", cDatalistCSM.Rows[s][13]);
            txtCSMPIEE.Text = string.Format("Php {0:0.00}", cDatalistCSM.Rows[s][14]);
            txtCSMBK.Text = string.Format("Php {0:0.00}", cDatalistCSM.Rows[s][15]);
            txtCSMInventory.Text = string.Format("Php {0:0.00}", cDatalistCSM.Rows[s][16]);
            txtCSMName.Text = cDatalistCSM.Rows[s][17].ToString();
            txtCSMBName.Text = cDatalistCSM.Rows[s][18].ToString();
            txtCSCFee.Text = string.Format("Php {0:0.00}", cDatalistCSM.Rows[s][19]);
        }
        #endregion

        #region Quarter Services
        public void CSQSData(int s)
        {
            txtCSQCID.Text = cDatalistCSQ.Rows[s][1].ToString();
            txtCSQRFee.Text = string.Format("Php {0:0.00}", cDatalistCSQ.Rows[s][2]);
            txtCSQPFee.Text = string.Format("Php {0:0.00}", cDatalistCSQ.Rows[s][3]);
            txtCSQSFee.Text = string.Format("Php {0:0.00}", cDatalistCSQ.Rows[s][4]);
            txtCSQ1701Q.Text = string.Format("Php {0:0.00}", cDatalistCSQ.Rows[s][5]);
            txtCSQ1702Q.Text = string.Format("Php {0:0.00}", cDatalistCSQ.Rows[s][6]);
            txtCSQBK.Text = string.Format("Php {0:0.00}", cDatalistCSQ.Rows[s][7]);
            txtCSQInventory.Text = string.Format("Php {0:0.00}", cDatalistCSQ.Rows[s][8]);
            txtCSQName.Text = cDatalistCSQ.Rows[s][9].ToString();
            txtCSQBName.Text = cDatalistCSQ.Rows[s][10].ToString();
        }

        #endregion

        #region Annually Services
        public void CSASData(int s)
        {
            txtCSACID.Text = cDatalistCSA.Rows[s][1].ToString();
            txtCSARFee.Text = string.Format("Php {0:0.00}", cDatalistCSA.Rows[s][2]);
            txtCSAPFee.Text = string.Format("Php {0:0.00}", cDatalistCSA.Rows[s][3]);
            txtCSASFee.Text = string.Format("Php {0:0.00}", cDatalistCSA.Rows[s][4]);
            txtCSA1701.Text = string.Format("Php {0:0.00}", cDatalistCSA.Rows[s][5]);
            txtCSA1702.Text = string.Format("Php {0:0.00}", cDatalistCSA.Rows[s][6]);
            txtCSA1604CF.Text = string.Format("Php {0:0.00}", cDatalistCSA.Rows[s][7]);
            txtCSA1604E.Text = string.Format("Php {0:0.00}", cDatalistCSA.Rows[s][8]);
            txtCSAMLicense.Text = string.Format("Php {0:0.00}", cDatalistCSA.Rows[s][9]);
            txtCSACor.Text = string.Format("Php {0:0.00}", cDatalistCSA.Rows[s][10]);
            txtCSABK.Text = string.Format("Php {0:0.00}", cDatalistCSA.Rows[s][11]);
            txtCSAInventory.Text = string.Format("Php {0:0.00}", cDatalistCSA.Rows[s][12]);
            txtCSAName.Text = cDatalistCSA.Rows[s][13].ToString();
            txtCSABName.Text = cDatalistCSA.Rows[s][14].ToString();
        }
        #endregion

        #region Display Clients and Services
        void CSSDAll()
        {
            CSLLoad();

            DataRow[] csdrow = cDatalistCS.Select("Business_Name = '" + psCSBName.Replace("'", "''") + "'");
            DataRow[] csmdrow = cDatalistCSM.Select("Business_Name = '" + psCSBName.Replace("'", "''") + "'");
            DataRow[] csqdrow = cDatalistCSQ.Select("Business_Name = '" + psCSBName.Replace("'", "''") + "'");
            DataRow[] csadrow = cDatalistCSA.Select("Business_Name = '" + psCSBName.Replace("'", "''") + "'");

            if (csdrow.Length > 0)
            {
                icsl = cDatalistCS.Rows.IndexOf(csdrow[0]);
                CSSData(icsl);
            }

            if (csmdrow.Length > 0)
            {
                icsl = cDatalistCSM.Rows.IndexOf(csmdrow[0]);
                CSMSData(icsl);
            }

            if (csqdrow.Length > 0)
            {
                icsl = cDatalistCSQ.Rows.IndexOf(csqdrow[0]);
                CSQSData(icsl);
            }

            if (csadrow.Length > 0)
            {
                icsl = cDatalistCSA.Rows.IndexOf(csadrow[0]);
                CSASData(icsl);
            }
        }
        #endregion

        #endregion

        #region Populating Group Year
        void PGYear()
        {
            for (int i = 1970; i <= DateTime.Now.Year; i++)
            {
                cmbCSGYear.Items.Add(Convert.ToString("                    " + i));
            }

            cmbCSGYear.Items.Add(Convert.ToString("                    " + (DateTime.Now.Year + 1)));
        }
        #endregion

        #region DatePicker ValueChanged
        private void dtpCSSRDate_ValueChanged(object sender, EventArgs e)
        {
            dtpCSSRDate.CustomFormat = "                MM-dd-yyyy";
        }

        private void dtpCSDIDate_ValueChanged(object sender, EventArgs e)
        {
            dtpCSDIDate.CustomFormat = "                MM-dd-yyyy";
        }

        private void dtpCSCRDate_ValueChanged(object sender, EventArgs e)
        {
            dtpCSCRDate.CustomFormat = "                MM-dd-yyyy";
        }
        #endregion

        #region Enable Clients Fields
        void CSCEFields()
        {
            txtCSCID.Enabled = true;
            txtCSStatus.Enabled = true;
            txtCSLName.Enabled = true;
            txtCSFName.Enabled = true;
            txtCSMI.Enabled = true;
            txtCSBName.Enabled = true;
            cmbCSNBusiness.Enabled = true;
            txtCSBOTin.Enabled = true;
            cmbCSTType.Enabled = true;
            txtCSAddress.Enabled = true;
            txtCSCNumber.Enabled = true;
            txtCSEAddress.Enabled = true;
            txtCSDNumber.Enabled = true;
            txtCSCRNumber.Enabled = true;
            txtCSSNumber.Enabled = true;
            txtCSPINumber.Enabled = true;
            txtCSPHNumber.Enabled = true;
            txtCSGName.Enabled = true;
            txtCSSGName.Enabled = true;
            cmbCSGYear.Enabled = true;
        }
        #endregion

        #region Disable Clients Fields
        void CSCDFields()
        {
            txtCSCID.Enabled = false;
            txtCSStatus.Enabled = false;
            txtCSLName.Enabled = false;
            txtCSFName.Enabled = false;
            txtCSMI.Enabled = false;
            txtCSBName.Enabled = false;
            cmbCSNBusiness.Enabled = false;
            txtCSBOTin.Enabled = false;
            cmbCSTType.Enabled = false;
            txtCSAddress.Enabled = false;
            txtCSCNumber.Enabled = false;
            txtCSEAddress.Enabled = false;
            txtCSSRNumber.Enabled = false;
            dtpCSSRDate.Enabled = false;
            txtCSDNumber.Enabled = false;
            dtpCSDIDate.Enabled = false;
            txtCSCRNumber.Enabled = false;
            dtpCSCRDate.Enabled = false;
            txtCSSNumber.Enabled = false;
            txtCSPINumber.Enabled = false;
            txtCSPHNumber.Enabled = false;
            txtCSGName.Enabled = false;
            txtCSSGName.Enabled = false;
            cmbCSGYear.Enabled = false;
        }

        #endregion

        #region Enable Services Fields
        void CSSEFields()
        {
            txtCSMCID.Enabled = true;
            txtCSMName.Enabled = true;
            txtCSMBName.Enabled = true;
            txtCSMRFee.Enabled = true;
            txtCSMPFee.Enabled = true;
            txtCSMSFee.Enabled = true;
            txtCSMVat.Enabled = true;
            txtCSMNVat.Enabled = true;
            txtCSM1601C.Enabled = true;
            txtCSM1601E.Enabled = true;
            txtCSMSER.Enabled = true;
            txtCSMPHER.Enabled = true;
            txtCSMPIER.Enabled = true;
            txtCSMSEE.Enabled = true;
            txtCSMPHEE.Enabled = true;
            txtCSMPIEE.Enabled = true;
            txtCSCFee.Enabled = true;
            txtCSMBK.Enabled = true;
            txtCSMInventory.Enabled = true;

            txtCSQCID.Enabled = true;
            txtCSQName.Enabled = true;
            txtCSQBName.Enabled = true;
            txtCSQRFee.Enabled = true;
            txtCSQPFee.Enabled = true;
            txtCSQSFee.Enabled = true;
            txtCSQ1701Q.Enabled = true;
            txtCSQ1702Q.Enabled = true;
            txtCSQBK.Enabled = true;
            txtCSQInventory.Enabled = true;

            txtCSACID.Enabled = true;
            txtCSAName.Enabled = true;
            txtCSABName.Enabled = true;
            txtCSARFee.Enabled = true;
            txtCSAPFee.Enabled = true;
            txtCSASFee.Enabled = true;
            txtCSA1701.Enabled = true;
            txtCSA1702.Enabled = true;
            txtCSA1604CF.Enabled = true;
            txtCSA1604E.Enabled = true;
            txtCSAMLicense.Enabled = true;
            txtCSACor.Enabled = true;
            txtCSABK.Enabled = true;
            txtCSAInventory.Enabled = true;
        }
        #endregion

        #region Disable Services Fields
        void CSSDFields()
        {
            txtCSMCID.Enabled = false;
            txtCSMName.Enabled = false;
            txtCSMBName.Enabled = false;
            txtCSMRFee.Enabled = false;
            txtCSMPFee.Enabled = false;
            txtCSMSFee.Enabled = false;
            txtCSMVat.Enabled = false;
            txtCSMNVat.Enabled = false;
            txtCSM1601C.Enabled = false;
            txtCSM1601E.Enabled = false;
            txtCSMSER.Enabled = false;
            txtCSMPHER.Enabled = false;
            txtCSMPIER.Enabled = false;
            txtCSMSEE.Enabled = false;
            txtCSMPHEE.Enabled = false;
            txtCSMPIEE.Enabled = false;
            txtCSCFee.Enabled = false;
            txtCSMBK.Enabled = false;
            txtCSMInventory.Enabled = false;

            txtCSQCID.Enabled = false;
            txtCSQName.Enabled = false;
            txtCSQBName.Enabled = false;
            txtCSQRFee.Enabled = false;
            txtCSQPFee.Enabled = false;
            txtCSQSFee.Enabled = false;
            txtCSQ1701Q.Enabled = false;
            txtCSQ1702Q.Enabled = false;
            txtCSQBK.Enabled = false;
            txtCSQInventory.Enabled = false;

            txtCSACID.Enabled = false;
            txtCSAName.Enabled = false;
            txtCSABName.Enabled = false;
            txtCSARFee.Enabled = false;
            txtCSAPFee.Enabled = false;
            txtCSASFee.Enabled = false;
            txtCSA1701.Enabled = false;
            txtCSA1702.Enabled = false;
            txtCSA1604CF.Enabled = false;
            txtCSA1604E.Enabled = false;
            txtCSAMLicense.Enabled = false;
            txtCSACor.Enabled = false;
            txtCSABK.Enabled = false;
            txtCSAInventory.Enabled = false;
        }
        #endregion

        #region Enable or Disable Edit
        private void txtCSCID_TextChanged(object sender, EventArgs e)
        {
            if (txtCSCID.Text != "" && psCSNE == "")
            {
                btnCSEdit.Enabled = true;
            }

            else
            {
                btnCSEdit.Enabled = false;
            }

            EDButtons();
        }
        #endregion

        #region Enable or Disable DA
        private void txtCSStatus_TextChanged(object sender, EventArgs e)
        {
            if (txtCSStatus.Text != "" && psCSNE == "")
            {
                btnCSDA.Enabled = true;

                if (txtCSStatus.Text == "Active")
                {
                    btnCSDA.Text = "Deact&ivate";
                }

                else if (txtCSStatus.Text == "Inactive")
                {
                    btnCSDA.Text = "Act&ivate";
                }
            }

            else
            {
                btnCSDA.Text = "Deact&ivate";
                btnCSDA.Enabled = false;
            }

            EDButtons();
        }
        #endregion

        #region Enable or Disable DatePicker
        private void txtCSSRNumber_TextChanged(object sender, EventArgs e)
        {
            if (txtCSSRNumber.Text != "" && psCSNE != "")
            {
                dtpCSSRDate.Enabled = true;
            }

            else
            {
                dtpCSSRDate.Enabled = false;
                dtpCSSRDate.Value = DateTime.Now.Date;
                dtpCSSRDate.CustomFormat = " ";
            }
        }

        private void txtCSDNumber_TextChanged(object sender, EventArgs e)
        {
            if (txtCSDNumber.Text != "" && psCSNE != "")
            {
                dtpCSDIDate.Enabled = true;
            }

            else
            {
                dtpCSDIDate.Enabled = false;
                dtpCSDIDate.Value = DateTime.Now.Date;
                dtpCSDIDate.CustomFormat = " ";
            }
        }

        private void txtCSCRNumber_TextChanged(object sender, EventArgs e)
        {
            if (txtCSCRNumber.Text != "" && psCSNE != "")
            {
                dtpCSCRDate.Enabled = true;
            }

            else
            {
                dtpCSCRDate.Enabled = false;
                dtpCSCRDate.Value = DateTime.Now.Date;
                dtpCSCRDate.CustomFormat = " ";
            }
        }
        #endregion

        #region Enable or Disable SEC
        private void cmbCSNBusiness_SelectedIndexChanged(object sender, EventArgs e)
        {
            if ((cmbCSNBusiness.Text == "              Partnership" || cmbCSNBusiness.Text == "             Corporation") && psCSNE != "")
            {
                txtCSSRNumber.Enabled = true;
            }

            else
            {
                txtCSSRNumber.Enabled = false;
                txtCSSRNumber.Clear();
            }
        }
        #endregion

        #region Enable Update
        private void txtCSSCID_TextChanged(object sender, EventArgs e)
        {
            if ((txtCSMCID.Text != "" || txtCSQCID.Text != "" || txtCSACID.Text != "") && psCSU == "")
            {
                btnCSUpdate.Enabled = true;
            }

            else
            {
                btnCSUpdate.Enabled = false;
            }

            EDButtons();
        }
        #endregion

        #region Activate and Deactivate
        private void btnCSDA_Click(object sender, EventArgs e)
        {
            string st = "";

            if (txtCSStatus.Text == "Active")
            {
                st = "Inactive";
            }

            else
            {
                st = "Active";
            }

            string cQuery = "UPDATE tbl_clients SET Status = '" + st + "' WHERE Client_ID = '" + txtCSCID.Text + "';";
            MySqlConnection cConnection = new MySqlConnection(Conn.cString);
            MySqlCommand cCommand = new MySqlCommand(cQuery, cConnection);
            MySqlDataReader cReader;

            try
            {
                cConnection.Open();
                cReader = cCommand.ExecuteReader();
                while (cReader.Read()) { }

                sgbname = txtCSBName.Text;

                CSCLoad(icsp);

                psCSBName = sgbname;

                CSSDAll();

                psTFTrigger = "*DA";

                if (txtCSStatus.Text == "Active")
                {
                    MessageBox.Show("Client " + txtCSLName.Text + ", " + txtCSFName.Text + " " + txtCSMI.Text + " has turn active.", "Activate", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                else
                {
                    MessageBox.Show("Client " + txtCSLName.Text + ", " + txtCSFName.Text + " " + txtCSMI.Text + " has turn inactive.", "Deactivate", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }

            catch (MySqlException ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            finally
            {
                cConnection.Close();
            }
        }
        #endregion

        #region Replace Php
        string mrfee, mpfee, msfee, vat, nvat, d1601c, d1601e, ser, pher, pier, see, phee, piee, cfee, mbk, mi;
        string qrfee, qpfee, qsfee, d1701q, d1702q, qbk, qi;
        string arfee, apfee, asfee, d1701, d1702, d1604cf, d1604e, ml, cor, abk, ai;

        void CSRPhp()
        {
            mrfee = txtCSMRFee.Text.Replace("Php ", "");
            mpfee = txtCSMPFee.Text.Replace("Php ", "");
            msfee = txtCSMSFee.Text.Replace("Php ", "");
            vat = txtCSMVat.Text.Replace("Php ", "");
            nvat = txtCSMNVat.Text.Replace("Php ", "");
            d1601c = txtCSM1601C.Text.Replace("Php ", "");
            d1601e = txtCSM1601E.Text.Replace("Php ", "");
            ser = txtCSMSER.Text.Replace("Php ", "");
            pher = txtCSMPHER.Text.Replace("Php ", "");
            pier = txtCSMPIER.Text.Replace("Php ", "");
            see = txtCSMSEE.Text.Replace("Php ", "");
            phee = txtCSMPHEE.Text.Replace("Php ", "");
            piee = txtCSMPIEE.Text.Replace("Php ", "");
            cfee = txtCSCFee.Text.Replace("Php ", "");
            mbk = txtCSMBK.Text.Replace("Php ", "");
            mi = txtCSMInventory.Text.Replace("Php ", "");

            qrfee = txtCSQRFee.Text.Replace("Php ", "");
            qpfee = txtCSQPFee.Text.Replace("Php ", "");
            qsfee = txtCSQSFee.Text.Replace("Php ", "");
            d1701q = txtCSQ1701Q.Text.Replace("Php ", "");
            d1702q = txtCSQ1702Q.Text.Replace("Php ", "");
            qbk = txtCSQBK.Text.Replace("Php ", "");
            qi = txtCSQInventory.Text.Replace("Php ", "");

            arfee = txtCSARFee.Text.Replace("Php ", "");
            apfee = txtCSAPFee.Text.Replace("Php ", "");
            asfee = txtCSASFee.Text.Replace("Php ", "");
            d1701 = txtCSA1701.Text.Replace("Php ", "");
            d1702 = txtCSA1702.Text.Replace("Php ", "");
            d1604cf = txtCSA1604CF.Text.Replace("Php ", "");
            d1604e = txtCSA1604E.Text.Replace("Php ", "");
            ml = txtCSAMLicense.Text.Replace("Php ", "");
            cor = txtCSACor.Text.Replace("Php ", "");
            abk = txtCSABK.Text.Replace("Php ", "");
            ai = txtCSAInventory.Text.Replace("Php ", "");
        }
        #endregion

        #region Find Row After Save
        void FRASave()
        {
            psCSBName = sgbname;

            DataRow[] csdrow = cDatasetCSM.Select("[Business Name] = '" + psCSBName.Replace("'", "''") + "'");

            if (csdrow.Length > 0)
            {
                foreach (DataGridViewRow row in dgvCSMonthly.Rows)
                {
                    if (row.Cells[3].Value.ToString().Equals("" + psCSBName + ""))
                    {
                        CSCLoad(icsp);
                        dgvCSMonthly.CurrentCell = dgvCSMonthly[3, row.Index];
                    }
                }
            }

            else
            {
                CSPage();

                icspage++;

                if (icspage == dcsrpage)
                {
                    btnCSNext.Enabled = false;
                    btnCSLast.Enabled = false;
                }

                if (icspage > 1)
                {
                    btnCSFirst.Enabled = true;
                    btnCSBack.Enabled = true;
                }

                icsp = icsp + icsdpage;
                CSMLoad(icsp);
                CSQLoad(icsp);
                CSALoad(icsp);

                EDButtons();

                FRASave();
            }
        }
        #endregion

        #region Field to Table Trigger
        void TFTrigger()
        {
            if (psTFTrigger == "*New")
            {
                FRASave();
            }

            else if (psTFTrigger == "*Edit" || psTFTrigger == "*Update" || psTFTrigger == "*DA")
            {
                psCSBName = sgbname;

                DataRow[] csdrow = cDatasetCS.Select("[Business Name] = '" + psCSBName.Replace("'", "''") + "'");

                if (csdrow.Length > 0)
                {
                    foreach (DataGridViewRow row in dgvCSClients.Rows)
                    {
                        if (row.Cells[3].Value.ToString().Equals("" + psCSBName + ""))
                        {
                            dgvCSClients.CurrentCell = dgvCSClients[3, row.Index];
                        }
                    }
                }

            }
        }
        #endregion

        #region Clear

        #region Clients
        void CSClear()
        {
            txtCSCID.Clear();
            txtCSStatus.Clear();
            txtCSLName.Clear();
            txtCSFName.Clear();
            txtCSMI.Clear();
            txtCSBName.Clear();
            txtCSBOTin.Clear();
            txtCSAddress.Clear();
            txtCSCNumber.Clear();
            txtCSEAddress.Clear();
            txtCSSRNumber.Clear();
            dtpCSSRDate.Value = DateTime.Now;
            dtpCSSRDate.CustomFormat = " ";
            txtCSDNumber.Clear();
            dtpCSDIDate.Value = DateTime.Now;
            dtpCSDIDate.CustomFormat = " ";
            txtCSCRNumber.Clear();
            dtpCSCRDate.Value = DateTime.Now;
            dtpCSCRDate.CustomFormat = " ";
            txtCSSNumber.Clear();
            txtCSPINumber.Clear();
            txtCSPHNumber.Clear();
            txtCSGName.Clear();
            txtCSSGName.Clear();

            if (!cmbCSNBusiness.Items.Contains(""))
            {
                cmbCSNBusiness.Items.Add("");
                cmbCSNBusiness.Text = "";
                cmbCSNBusiness.Items.Remove("");
            }

            if (!cmbCSTType.Items.Contains(""))
            {
                cmbCSTType.Items.Add("");
                cmbCSTType.Text = "";
                cmbCSTType.Items.Remove("");
            }

            if (!cmbCSGYear.Items.Contains(""))
            {
                cmbCSGYear.Items.Add("");
                cmbCSGYear.Text = "";
                cmbCSGYear.Items.Remove("");
            }
        }
        #endregion

        #region Monthly Services
        void CSMClear()
        {
            txtCSMCID.Clear();
            txtCSMRFee.Clear();
            txtCSMPFee.Clear();
            txtCSMSFee.Clear();
            txtCSMVat.Clear();
            txtCSMNVat.Clear();
            txtCSM1601C.Clear();
            txtCSM1601E.Clear();
            txtCSMSER.Clear();
            txtCSMPHER.Clear();
            txtCSMPIER.Clear();
            txtCSMSEE.Clear();
            txtCSMPHEE.Clear();
            txtCSMPIEE.Clear();
            txtCSMBK.Clear();
            txtCSMInventory.Clear();
            txtCSMName.Clear();
            txtCSMBName.Clear();
        }
        #endregion

        #region Annually Services
        public void CSAClear()
        {
            txtCSACID.Clear();
            txtCSARFee.Clear();
            txtCSAPFee.Clear();
            txtCSASFee.Clear();
            txtCSA1701.Clear();
            txtCSA1702.Clear();
            txtCSA1604CF.Clear();
            txtCSA1604E.Clear();
            txtCSAMLicense.Clear();
            txtCSACor.Clear();
            txtCSABK.Clear();
            txtCSAInventory.Clear();
            txtCSAName.Clear();
            txtCSABName.Clear();
        }
        #endregion

        #region Quarter Services
        public void CSQClear()
        {
            txtCSQCID.Clear();
            txtCSQRFee.Clear();
            txtCSQPFee.Clear();
            txtCSQSFee.Clear();
            txtCSQ1701Q.Clear();
            txtCSQ1702Q.Clear();
            txtCSQBK.Clear();
            txtCSQInventory.Clear();
            txtCSQName.Clear();
            txtCSQBName.Clear();
        }
        #endregion

        #endregion

        #region New
        private void btnCSNew_Click(object sender, EventArgs e)
        {
            psCSNE = "*New";

            CSCEFields();
            CSClear();
            GCID();
            txtCSCID.Text = psCID;
            txtCSStatus.Text = "Active";

            btnCSNew.Enabled = false;
            btnCSEdit.Enabled = false;
            btnCSDA.Enabled = false;
            btnCSTFView.Enabled = false;
            btnCSClients.Enabled = false;
            btnCSServices.Enabled = false;

            btnCSCancel.Enabled = true;
            btnCSSave.Enabled = true;

            EDButtons();

        }
        #endregion

        #region Edit
        private void btnCSEdit_Click(object sender, EventArgs e)
        {
            psCSNE = "*Edit";

            CSCEFields();

            btnCSNew.Enabled = false;
            btnCSEdit.Enabled = false;
            btnCSDA.Enabled = false;
            btnCSTFView.Enabled = false;
            btnCSClients.Enabled = false;
            btnCSServices.Enabled = false;

            btnCSCancel.Enabled = true;
            btnCSSave.Enabled = true;

            cmbCSNBusiness_SelectedIndexChanged(sender, e);

            EDButtons();
        }
        #endregion

        #region Update
        private void btnCSUpdate_Click(object sender, EventArgs e)
        {
            if (txtCSStatus.Text == "Inactive")
            {
                MessageBox.Show("You can't update services of an inactive account.", "Inactive Account", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            else
            {
                psCSU = "*Update";

                CSSEFields();

                btnCSTFView.Enabled = false;
                btnCSUpdate.Enabled = false;
                btnCSClients.Enabled = false;

                btnCSCancel.Enabled = true;
                btnCSSave.Enabled = true;

                EDButtons();

                string nb = "", tt = "";

                string cQuery = "SELECT Nature_of_Business, Tax_Type FROM tbl_clients WHERE Client_ID = '" + txtCSACID.Text + "';";
                MySqlConnection cConnection = new MySqlConnection(Conn.cString);
                MySqlCommand cCommand = new MySqlCommand(cQuery, cConnection);
                MySqlDataReader cReader;

                try
                {
                    cConnection.Open();
                    cReader = cCommand.ExecuteReader();
                    while (cReader.Read())
                    {
                        nb = cReader.GetValue(0).ToString();
                        tt = cReader.GetValue(1).ToString();
                    }
                }

                catch (MySqlException ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                finally
                {
                    cConnection.Close();
                }

                if (nb == "Sole Proprietorship")
                {
                    txtCSA1701.Enabled = true;
                    txtCSQ1701Q.Enabled = true;

                    txtCSA1702.Enabled = false;
                    txtCSQ1702Q.Enabled = false;
                }

                else
                {
                    txtCSA1701.Enabled = false;
                    txtCSQ1701Q.Enabled = false;

                    txtCSA1702.Enabled = true;
                    txtCSQ1702Q.Enabled = true;
                }

                if (tt == "VAT")
                {
                    txtCSMVat.Enabled = true;
                    txtCSMNVat.Enabled = false;
                }

                else
                {
                    txtCSMVat.Enabled = false;
                    txtCSMNVat.Enabled = true;
                }
            }
        }
        #endregion

        #region Clear on Change of Business Type and Tax Type
        void CCBT()
        {
            string tt = "", nob = "", ctt, cnob;

            string cQuery = "SELECT Nature_of_Business, Tax_Type FROM tbl_clients WHERE Client_ID = '" + txtCSCID.Text + "';";
            MySqlConnection cConnection = new MySqlConnection(Conn.cString);
            MySqlCommand cCommand = new MySqlCommand(cQuery, cConnection);
            MySqlDataReader cReader;

            try
            {
                cConnection.Open();
                cReader = cCommand.ExecuteReader();
                while (cReader.Read())
                {
                    nob = cReader.GetValue(0).ToString();
                    tt = cReader.GetValue(1).ToString();
                }
            }

            catch (MySqlException ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            finally
            {
                cConnection.Close();
            }

            ctt = cmbCSTType.Text.Replace(" ", "");
            cnob = cmbCSNBusiness.Text.Replace(" ", "");

            if (cnob == "SoleProprietorship")
            {
                cnob = "Sole Proprietorship";
            }

            if (cnob != nob)
            {
                string cQueryI = "UPDATE tbl_qservices SET D1701Q = NULL, D1702Q = NULL WHERE Client_ID = '" + txtCSCID.Text + "';";
                MySqlConnection cConnectionI = new MySqlConnection(Conn.cString);
                MySqlCommand cCommandI = new MySqlCommand(cQueryI, cConnectionI);
                MySqlDataReader cReaderI;

                try
                {
                    cConnectionI.Open();
                    cReaderI = cCommandI.ExecuteReader();
                    while (cReaderI.Read()) { }
                }

                catch (MySqlException ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                finally
                {
                    cConnectionI.Close();
                }

                string cQueryII = "UPDATE tbl_aservices SET D1701 = NULL, D1702 = NULL WHERE Client_ID = '" + txtCSCID.Text + "';";
                MySqlConnection cConnectionII = new MySqlConnection(Conn.cString);
                MySqlCommand cCommandII = new MySqlCommand(cQueryII, cConnectionII);
                MySqlDataReader cReaderII;

                try
                {
                    cConnectionII.Open();
                    cReaderII = cCommandII.ExecuteReader();
                    while (cReaderII.Read()) { }
                }

                catch (MySqlException ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                finally
                {
                    cConnectionII.Close();
                }
            }

            if (ctt != tt)
            {
                string cQueryI = "UPDATE tbl_mservices SET VAT = '', Non_VAT = '' WHERE Client_ID = '" + txtCSCID.Text + "';";
                MySqlConnection cConnectionI = new MySqlConnection(Conn.cString);
                MySqlCommand cCommandI = new MySqlCommand(cQueryI, cConnectionI);
                MySqlDataReader cReaderI;

                try
                {
                    cConnectionI.Open();
                    cReaderI = cCommandI.ExecuteReader();
                    while (cReaderI.Read()) { }
                }

                catch (MySqlException ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                finally
                {
                    cConnectionI.Close();
                }
            }
        }
        #endregion

        #region New in Services
        void SNew()
        {
            #region Monthly
            string cQuery = "INSERT INTO tbl_mservices(Client_ID) VAlUES('" + txtCSCID.Text + "');";
            MySqlConnection cConnection = new MySqlConnection(Conn.cString);
            MySqlCommand cCommand = new MySqlCommand(cQuery, cConnection);
            MySqlDataReader cReader;

            try
            {
                cConnection.Open();
                cReader = cCommand.ExecuteReader();
                while (cReader.Read()) { }
            }

            catch (MySqlException ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            finally
            {
                cConnection.Close();
            }
            #endregion

            #region Quarterly
            string cQueryI = "INSERT INTO tbl_qservices(Client_ID) VAlUES('" + txtCSCID.Text + "');";
            MySqlConnection cConnectionI = new MySqlConnection(Conn.cString);
            MySqlCommand cCommandI = new MySqlCommand(cQueryI, cConnectionI);
            MySqlDataReader cReaderI;

            try
            {
                cConnectionI.Open();
                cReaderI = cCommandI.ExecuteReader();
                while (cReaderI.Read()) { }
            }

            catch (MySqlException ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            finally
            {
                cConnectionI.Close();
            }
            #endregion

            #region Annually
            string cQueryII = "INSERT INTO tbl_aservices(Client_ID) VAlUES('" + txtCSCID.Text + "');";
            MySqlConnection cConnectionII = new MySqlConnection(Conn.cString);
            MySqlCommand cCommandII = new MySqlCommand(cQueryII, cConnectionII);
            MySqlDataReader cReaderII;

            try
            {
                cConnectionII.Open();
                cReaderII = cCommandII.ExecuteReader();
                while (cReaderII.Read()) { }
            }

            catch (MySqlException ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            finally
            {
                cConnectionII.Close();
            }
            #endregion

        }
        #endregion

        #region Cancel
        private void btnCSCancel_Click(object sender, EventArgs e)
        {
            lblHeader.Focus();

            if (txtCSCID.Enabled == true)
            {
                psCSNE = "";

                txtCSCID_TextChanged(sender, e);
                txtCSStatus_TextChanged(sender, e);

                CSCDFields();

                btnCSNew.Enabled = true;
                btnCSTFView.Enabled = true;
                btnCSClients.Enabled = true;
                btnCSServices.Enabled = true;

                btnCSCancel.Enabled = false;
                btnCSSave.Enabled = false;

                CSSDAll();

                EDButtons();
            }

            else if (txtCSMCID.Enabled == true)
            {
                psCSU = "";

                txtCSSCID_TextChanged(sender, e);

                CSSDFields();

                btnCSTFView.Enabled = true;
                btnCSClients.Enabled = true;

                btnCSCancel.Enabled = false;
                btnCSSave.Enabled = false;

                CSSDAll();

                EDButtons();
            }
        }
        #endregion

        #region Save
        private void btnCSSave_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;

            lblHeader.Focus();

            if (flpCSCFields.Visible)
            {
                #region Clients
                if (txtCSLName.Text == "")
                {
                    MessageBox.Show("Last Name field is empty.", "Empty Fields", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtCSLName.Focus();
                }

                else if (txtCSFName.Text == "")
                {
                    MessageBox.Show("First Name field is empty.", "Empty Fields", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtCSFName.Focus();
                }

                else if (txtCSBName.Text == "")
                {
                    MessageBox.Show("Business Name field is empty.", "Empty Fields", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtCSBName.Focus();
                }

                else if (cmbCSNBusiness.Text == "")
                {
                    MessageBox.Show("Select Nature of Business.", "Empty Fields", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    cmbCSNBusiness.Focus();
                }

                else if (txtCSBOTin.Text == "")
                {
                    MessageBox.Show("Business / Owner TIN field is empty.", "Empty Fields", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtCSBOTin.Focus();
                }

                else if (cmbCSTType.Text == "")
                {
                    MessageBox.Show("Select Tax Type.", "Empty Fields", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    cmbCSTType.Focus();
                }

                else if (txtCSAddress.Text == "")
                {
                    MessageBox.Show("Address field is empty.", "Empty Fields", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtCSAddress.Focus();
                }

                else if (txtCSSRNumber.Text == "" && (cmbCSNBusiness.Text == "              Partnership" || cmbCSNBusiness.Text == "             Corporation"))
                {
                    MessageBox.Show("SEC Reg. Number is empty.", "Empty Fields", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtCSSRNumber.Focus();
                }

                else if (txtCSGName.Text == "")
                {
                    MessageBox.Show("Group Name field is empty.", "Empty Fields", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtCSGName.Focus();
                }

                else if (txtCSSGName.Text == "")
                {
                    MessageBox.Show("Sub-group Name field is empty.", "Empty Fields", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtCSSGName.Focus();
                }

                else if (cmbCSGYear.Text == "")
                {
                    MessageBox.Show("Group Year field is empty.", "Empty Fields", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtCSSGName.Focus();
                }

                else
                {
                    string nob = "", tt = "", gy = "", srd = "", did = "", crd = "";

                    nob = cmbCSNBusiness.Text.Replace(" ", "");
                    if (nob == "SoleProprietorship")
                    {
                        nob = "Sole Proprietorship";
                    }
                    tt = cmbCSTType.Text.Replace(" ", "");
                    gy = cmbCSGYear.Text.Replace(" ", "");

                    if (dtpCSSRDate.CustomFormat == "                MM-dd-yyyy")
                    {
                        DateTime srdate = dtpCSSRDate.Value;
                        srd = srdate.ToString("yyyy-MM-dd");
                    }

                    else if (dtpCSSRDate.CustomFormat == " ")
                    {
                        srd = "";
                    }

                    if (dtpCSDIDate.CustomFormat == "                MM-dd-yyyy")
                    {
                        DateTime didate = dtpCSDIDate.Value;
                        did = didate.ToString("yyyy-MM-dd");
                    }

                    else if (dtpCSDIDate.CustomFormat == " ")
                    {
                        did = "";
                    }

                    if (dtpCSCRDate.CustomFormat == "                MM-dd-yyyy")
                    {
                        DateTime crdate = dtpCSCRDate.Value;
                        crd = crdate.ToString("yyyy-MM-dd");
                    }

                    else if (dtpCSCRDate.CustomFormat == " ")
                    {
                        crd = "";
                    }

                    if (psCSNE == "*New")
                    {
                        #region New
                        string cQuery = "INSERT INTO tbl_clients(Client_ID, Last_Name, First_Name, Middle_Initial,"
                            + " Business_Name, Nature_of_Business, Business_or_Owner_TIN, Tax_Type, Address,"
                            + " Contact_Number, Email_Address, SEC_Registration_Number, SEC_Registration_Date,"
                            + " DTI_Number, DTI_Issuance_Date, COR_Number, COR_Date, SSS_Number, Pag_IBIG_Number,"
                            + " PhilHealth_Number, Group_Name, Sub_Group_Name, Group_Year, Status, Username)"
                            + " VALUES('" + txtCSCID.Text + "', '" + txtCSLName.Text.Replace("'", "''") + "', '" + txtCSFName.Text.Replace("'", "''") + "',"
                            + " '" + txtCSMI.Text.Replace("'", "''") + "', '" + txtCSBName.Text.Replace("'", "''") + "', '" + nob + "',"
                            + " '" + txtCSBOTin.Text.Replace("'", "''") + "', '" + tt + "', '" + txtCSAddress.Text.Replace("'", "''") + "',"
                            + " '" + txtCSCNumber.Text + "', '" + txtCSEAddress.Text.Replace("'", "''") + "', '" + txtCSSRNumber.Text.Replace("'", "''") + "',"
                            + " '" + srd + "', '" + txtCSDNumber.Text.Replace("'", "''") + "', '" + did + "',"
                            + " '" + txtCSCRNumber.Text.Replace("'", "''") + "', '" + crd + "', '" + txtCSSNumber.Text.Replace("'", "''") + "',"
                            + " '" + txtCSPINumber.Text.Replace("'", "''") + "', '" + txtCSPHNumber.Text.Replace("'", "''") + "', '" + txtCSGName.Text.Replace("'", "''") + "',"
                            + " '" + txtCSSGName.Text.Replace("'", "''") + "', '" + gy + "', '" + txtCSStatus.Text + "', '" + Login.psUName.Replace("'", "''") + "');";
                        MySqlConnection cConnection = new MySqlConnection(Conn.cString);
                        MySqlCommand cCommand = new MySqlCommand(cQuery, cConnection);
                        MySqlDataReader cReader;

                        try
                        {
                            cConnection.Open();
                            cReader = cCommand.ExecuteReader();
                            while (cReader.Read()) { }

                            SNew();

                            psCSNE = "";

                            sgbname = txtCSBName.Text;

                            if (!cmbCSSOption.Items.Contains("Search Option"))
                            {
                                cmbCSSOption.Items.Insert(0, "Search Option");
                                cmbCSSOption.Text = "Search Option";
                                cmbCSSOption.ForeColor = Color.DarkGray;
                            }

                            if (!cmbCSOOption.Items.Contains("Order Option"))
                            {
                                cmbCSOOption.Items.Insert(0, "Order Option");
                                cmbCSOOption.Text = "Order Option";
                                cmbCSOOption.ForeColor = Color.DarkGray;
                            }

                            scscsearch = "";
                            scsssearch = "";

                            scscorder = " ORDER BY Client_ID";
                            scssorder = " ORDER BY s.Client_ID";

                            icsp = 0;
                            icspage = 1;

                            CSCLoad(icsp);
                            CSMLoad(icsp);
                            CSQLoad(icsp);
                            CSALoad(icsp);
                            CSSearch();

                            CSRCDefault();

                            txtCSCID_TextChanged(sender, e);
                            txtCSStatus_TextChanged(sender, e);
                            txtCSSCID_TextChanged(sender, e);

                            CSCDFields();

                            btnCSNew.Enabled = true;
                            btnCSTFView.Enabled = true;
                            btnCSClients.Enabled = true;
                            btnCSServices.Enabled = true;

                            btnCSCancel.Enabled = false;
                            btnCSSave.Enabled = false;

                            psCSBName = sgbname;

                            CSSDAll();

                            psTFTrigger = "*New";
                            TFTrigger();
                            psTFTrigger = "";

                            EDButtons();

                            MessageBox.Show("New client name " + txtCSLName.Text + ", " + txtCSFName.Text + " " + txtCSMI.Text + " has been saved.", "New Client", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }

                        catch (MySqlException ex)
                        {
                            if (ex.Message.Contains("Duplicate entry"))
                            {
                                MessageBox.Show("Business Name already exist.", "Already Exist", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                txtCSBName.Focus();
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
                        #endregion
                    }

                    else if (psCSNE == "*Edit")
                    {
                        #region Edit
                        CCBT();

                        string cQuery = "UPDATE tbl_clients SET Last_Name = '" + txtCSLName.Text.Replace("'", "''") + "', First_Name = '" + txtCSFName.Text.Replace("'", "''") + "',"
                            + " Middle_Initial = '" + txtCSMI.Text.Replace("'", "''") + "', Business_Name = '" + txtCSBName.Text.Replace("'", "''") + "', Nature_of_Business = '" + nob + "',"
                            + " Business_or_Owner_TIN = '" + txtCSBOTin.Text.Replace("'", "''") + "', Tax_Type = '" + tt + "', Address = '" + txtCSAddress.Text.Replace("'", "''") + "',"
                            + " Contact_Number = '" + txtCSCNumber.Text + "', Email_Address = '" + txtCSEAddress.Text.Replace("'", "''") + "', SEC_Registration_Number = '" + txtCSSRNumber.Text.Replace("'", "''") + "',"
                            + " SEC_Registration_Date = '" + srd + "', DTI_Number = '" + txtCSDNumber.Text.Replace("'", "''") + "', DTI_Issuance_Date = '" + did + "',"
                            + " COR_Number = '" + txtCSCRNumber.Text.Replace("'", "''") + "', COR_Date = '" + crd + "', SSS_Number = '" + txtCSSNumber.Text.Replace("'", "''") + "',"
                            + " Pag_IBIG_Number = '" + txtCSPINumber.Text.Replace("'", "''") + "', PhilHealth_Number = '" + txtCSPHNumber.Text.Replace("'", "''") + "', Group_Name = '" + txtCSGName.Text.Replace("'", "''") + "',"
                            + " Sub_Group_Name = '" + txtCSSGName.Text.Replace("'", "''") + "', Group_Year = '" + gy + "' WHERE Client_ID = '" + txtCSCID.Text + "';";
                        MySqlConnection cConnection = new MySqlConnection(Conn.cString);
                        MySqlCommand cCommand = new MySqlCommand(cQuery, cConnection);
                        MySqlDataReader cReader;

                        try
                        {
                            cConnection.Open();
                            cReader = cCommand.ExecuteReader();
                            while (cReader.Read()) { }

                            psCSNE = "";

                            sgbname = txtCSBName.Text;

                            CSCLoad(icsp);
                            CSMLoad(icsp);
                            CSQLoad(icsp);
                            CSALoad(icsp);
                            CSSearch();

                            BPMLoad(ibpmp);
                            BPQLoad(ibpqp);
                            BPALoad(ibpap);
                            BPSearch();

                            psCSBName = sgbname;

                            CSSDAll();

                            CSCDFields();

                            btnCSNew.Enabled = true;
                            btnCSEdit.Enabled = true;
                            btnCSDA.Enabled = true;
                            btnCSTFView.Enabled = true;
                            btnCSClients.Enabled = true;
                            btnCSServices.Enabled = true;

                            btnCSCancel.Enabled = false;
                            btnCSSave.Enabled = false;

                            psTFTrigger = "*Edit";

                            EDButtons();

                            MessageBox.Show("Client " + txtCSLName.Text + ", " + txtCSFName.Text + " " + txtCSMI.Text + " information has been edited.", "Edit Client", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }

                        catch (MySqlException ex)
                        {
                            if (ex.Message.Contains("Duplicate entry"))
                            {
                                MessageBox.Show("Business Name already exist", "Already Exists", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                txtCSBName.Focus();
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
                        #endregion
                    }
                }
                #endregion
            }

            else
            {
                #region Services
                CSRPhp();

                string cQuery = "UPDATE tbl_mservices SET Retainers_Fee_M = '" + mrfee + "', Professional_Fee_M = '" + mpfee + "',"
                + " Service_Fee_M = '" + msfee + "', VAT = '" + vat + "', Non_VAT = '" + nvat + "', D1601C = '" + d1601c + "',"
                + " D1601E = '" + d1601e + "', SSS_ER = '" + ser + "', PHIC_ER = '" + pher + "', Pag_IBIG_ER = '" + pier + "',"
                + " SSS_EE = '" + see + "', PHIC_EE = '" + phee + "', Pag_IBIG_EE = '" + piee + "', Certification_Fee = '" + cfee + "',"
                + " Bookkeeping_M = '" + mbk + "', Inventory_M = '" + mi + "' WHERE Client_ID = '" + txtCSMCID.Text + "';";
                MySqlConnection cConnection = new MySqlConnection(Conn.cString);
                MySqlCommand cCommand = new MySqlCommand(cQuery, cConnection);
                MySqlDataReader cReader;

                try
                {
                    cConnection.Open();
                    cReader = cCommand.ExecuteReader();
                    while (cReader.Read()) { }
                }

                catch (MySqlException ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                finally
                {
                    cConnection.Close();
                }

                string cQueryI = "UPDATE tbl_qservices SET Retainers_Fee_Q = '" + qrfee + "', Professional_Fee_Q = '" + qpfee + "',"
                    + " Service_Fee_Q = '" + qsfee + "', D1701Q = '" + d1701q + "', D1702Q = '" + d1702q + "', Bookkeeping_Q = '" + qbk + "',"
                    + " Inventory_Q = '" + qi + "' WHERE Client_ID = '" + txtCSQCID.Text + "';";
                MySqlConnection cConnectionI = new MySqlConnection(Conn.cString);
                MySqlCommand cCommandI = new MySqlCommand(cQueryI, cConnectionI);
                MySqlDataReader cReaderI;

                try
                {
                    cConnectionI.Open();
                    cReaderI = cCommandI.ExecuteReader();
                    while (cReaderI.Read()) { }
                }

                catch (MySqlException ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                finally
                {
                    cConnectionI.Close();
                }

                string cQueryII = "UPDATE tbl_aservices SET Retainers_Fee_A = '" + arfee + "', Professional_Fee_A = '" + apfee + "',"
                    + " Service_Fee_A = '" + asfee + "', D1701 = '" + d1701 + "', D1702 = '" + d1702 + "', Bookkeeping_A = '" + abk + "',"
                    + " D1604CF = '" + d1604cf + "', D1604E = '" + d1604e + "', Municipal_License = '" + ml + "', COR = '" + cor + "',"
                    + " Inventory_A = '" + ai + "' WHERE Client_ID = '" + txtCSACID.Text + "';";
                MySqlConnection cConnectionII = new MySqlConnection(Conn.cString);
                MySqlCommand cCommandII = new MySqlCommand(cQueryII, cConnectionII);
                MySqlDataReader cReaderII;

                try
                {
                    cConnectionII.Open();
                    cReaderII = cCommandII.ExecuteReader();
                    while (cReaderII.Read()) { }

                    psCSU = "";

                    sgbname = txtCSABName.Text;

                    CSCLoad(icsp);
                    CSMLoad(icsp);
                    CSQLoad(icsp);
                    CSALoad(icsp);

                    txtCSSCID_TextChanged(sender, e);

                    CSSDFields();

                    btnCSTFView.Enabled = true;
                    btnCSClients.Enabled = true;

                    btnCSCancel.Enabled = false;
                    btnCSSave.Enabled = false;

                    psCSBName = sgbname;

                    CSSDAll();

                    psTFTrigger = "*Edit";

                    EDButtons();

                    cmbCSSOption_SelectedIndexChanged(sender, e);

                    MessageBox.Show("Client " + txtCSACID.Text + " services has been updated.", "Update Services", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                catch (MySqlException ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                finally
                {
                    cConnectionII.Close();
                }
                #endregion
            }

            Cursor.Current = Cursors.Default;
        }
        #endregion

        //----------------------------------------------------------------------// Billing and Payments

        #region Load Table
        string sbpsearch = "", sbps = "", sbporder = "", sbpdrange = "", sbpdfrom = "", sbpdto = "", sbpmpage = "", sbpqpage = "", sbpapage = "", sbpfsearch = "", sbpfield = "", sbpfdrange = "", sbpuname = "";

        #region Monthly Services
        int ibpmp = 0, ibpmpage = 1, ibpmdpage = tcontent;
        double dbpmcount = 0, dbpmpage = 0, dbpmtpage = 0, dbpmrpage = 0;

        void BPMLoad(int p)
        {
            string cQueryI = "SELECT COUNT(*) FROM tbl_mbillpay s LEFT JOIN tbl_clients c ON s.Client_ID = c.Client_ID LEFT JOIN vw_mpaylogs p ON CONCAT('TRN-', s.No_ID) = p.Transaction_Number" + sbpsearch + sbpdrange + sbpuname + ";";
            MySqlConnection cConnectionI = new MySqlConnection(Conn.cString);
            MySqlCommand cCommandI = new MySqlCommand(cQueryI, cConnectionI);
            MySqlDataReader cReaderI;

            try
            {
                cConnectionI.Open();
                cReaderI = cCommandI.ExecuteReader();
                while (cReaderI.Read())
                {
                    dbpmcount = cReaderI.GetDouble(0);
                }
            }

            catch (MySqlException ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            finally
            {
                cConnectionI.Close();
            }

            MySqlConnection cConn = new MySqlConnection(Conn.cString);
            MySqlCommand cCommand = new MySqlCommand("SELECT s.No_ID,"
                + " DATE_FORMAT(DATE(Bill_Date), '%m-%d-%Y') AS 'Bill Date',"
                + " CONCAT('TRN-', s.No_ID) AS 'Transaction Number',"
                + " s.Client_ID AS 'Client I.D.',"
                + " CONCAT(c.Last_Name, ', ', c.First_Name, ' ', c.Middle_Initial) AS 'Client Name',"
                + " c.Business_Name AS 'Business Name',"
                + " c.Address,"
                + " c.Nature_of_Business AS 'Nature of Business',"
                + " c.Business_or_Owner_TIN AS 'Business / Owner TIN',"
                + " c.Tax_Type AS 'Tax Type',"
                + " c.SEC_Registration_Number AS 'SEC Reg. Number',"
                + " c.Group_Name AS 'Group Name',"
                + " c.Sub_Group_Name AS 'Sub-group Name',"
                + " c.Group_Year AS 'Group Year',"
                + " c.Status,"
                + " IFNULL(CAST(s.Retainers_Fee_M AS DECIMAL(20, 2)), 0) AS 'Retainers Fee',"
                + " IFNULL(CAST(s.Professional_Fee_M AS DECIMAL(20, 2)), 0) AS 'Professional Fee',"
                + " IFNULL(CAST(s.Service_Fee_M AS DECIMAL(20, 2)), 0) AS 'Service Fee',"
                + " IFNULL(CAST(s.VAT AS DECIMAL(20, 2)), 0) AS 'VAT',"
                + " IFNULL(CAST(s.Non_VAT AS DECIMAL(20, 2)), 0) AS 'Non-VAT',"
                + " IFNULL(CAST(s.D1601C AS DECIMAL(20, 2)), 0) AS '1601C',"
                + " IFNULL(CAST(s.D1601E AS DECIMAL(20, 2)), 0) AS '1601E',"
                + " IFNULL(CAST(s.SSS_ER AS DECIMAL(20, 2)), 0) AS 'SSS (ER)',"
                + " IFNULL(CAST(s.PHIC_ER AS DECIMAL(20, 2)), 0) AS 'PHIC (ER)',"
                + " IFNULL(CAST(s.Pag_IBIG_ER AS DECIMAL(20, 2)), 0) AS 'Pag-IBIG (ER)',"
                + " IFNULL(CAST(s.SSS_EE AS DECIMAL(20, 2)), 0) AS 'SSS (EE)',"
                + " IFNULL(CAST(s.PHIC_EE AS DECIMAL(20, 2)), 0) AS 'PHIC (EE)',"
                + " IFNULL(CAST(s.Pag_IBIG_EE AS DECIMAL(20, 2)), 0) AS 'Pag-IBIG (EE)',"
                + " IFNULL(CAST(s.Certification_Fee AS DECIMAL(20, 2)), 0) AS 'Certification Fee',"
                + " IFNULL(CAST(s.Bookkeeping_M AS DECIMAL(20, 2)), 0) AS 'Bookkeeping',"
                + " IFNULL(CAST(s.Inventory_M AS DECIMAL(20, 2)), 0) AS 'Inventory',"

                + " IFNULL(CAST(s.Retainers_Fee_M AS DECIMAL(20, 2)), 0)"
                + " + IFNULL(CAST(s.Professional_Fee_M AS DECIMAL(20, 2)), 0)"
                + " + IFNULL(CAST(s.Service_Fee_M AS DECIMAL(20, 2)), 0)"
                + " + IFNULL(CAST(s.VAT AS DECIMAL(20, 2)), 0)"
                + " + IFNULL(CAST(s.Non_VAT AS DECIMAL(20, 2)), 0)"
                + " + IFNULL(CAST(s.D1601C AS DECIMAL(20, 2)), 0)"
                + " + IFNULL(CAST(s.D1601E AS DECIMAL(20, 2)), 0)"
                + " + IFNULL(CAST(s.SSS_ER AS DECIMAL(20, 2)), 0)"
                + " + IFNULL(CAST(s.PHIC_ER AS DECIMAL(20, 2)), 0)"
                + " + IFNULL(CAST(s.Pag_IBIG_ER AS DECIMAL(20, 2)), 0)"
                + " + IFNULL(CAST(s.SSS_EE AS DECIMAL(20, 2)), 0)"
                + " + IFNULL(CAST(s.PHIC_EE AS DECIMAL(20, 2)), 0)"
                + " + IFNULL(CAST(s.Pag_IBIG_EE AS DECIMAL(20, 2)), 0)"
                + " + IFNULL(CAST(s.Certification_Fee AS DECIMAL(20, 2)), 0)"
                + " + IFNULL(CAST(s.Bookkeeping_M AS DECIMAL(20, 2)), 0)"
                + " + IFNULL(CAST(s.Inventory_M AS DECIMAL(20, 2)), 0) AS 'Total Payments',"

                + " IFNULL(CAST(p.Retainers_Fee_M AS DECIMAL(20, 2)), 0)"
                + " + IFNULL(CAST(p.Professional_Fee_M AS DECIMAL(20, 2)), 0)"
                + " + IFNULL(CAST(p.Service_Fee_M AS DECIMAL(20, 2)), 0)"
                + " + IFNULL(CAST(p.VAT AS DECIMAL(20, 2)), 0)"
                + " + IFNULL(CAST(p.Non_VAT AS DECIMAL(20, 2)), 0)"
                + " + IFNULL(CAST(p.D1601C AS DECIMAL(20, 2)), 0)"
                + " + IFNULL(CAST(p.D1601E AS DECIMAL(20, 2)), 0)"
                + " + IFNULL(CAST(p.SSS_ER AS DECIMAL(20, 2)), 0)"
                + " + IFNULL(CAST(p.PHIC_ER AS DECIMAL(20, 2)), 0)"
                + " + IFNULL(CAST(p.Pag_IBIG_ER AS DECIMAL(20, 2)), 0)"
                + " + IFNULL(CAST(p.SSS_EE AS DECIMAL(20, 2)), 0)"
                + " + IFNULL(CAST(p.PHIC_EE AS DECIMAL(20, 2)), 0)"
                + " + IFNULL(CAST(p.Pag_IBIG_EE AS DECIMAL(20, 2)), 0)"
                + " + IFNULL(CAST(p.Certification_Fee AS DECIMAL(20, 2)), 0)"
                + " + IFNULL(CAST(p.Bookkeeping_M AS DECIMAL(20, 2)), 0)"
                + " + IFNULL(CAST(p.Inventory_M AS DECIMAL(20, 2)), 0) AS 'Collected Payments',"

                + " IFNULL(CAST(s.Retainers_Fee_M AS DECIMAL(20, 2)), 0)"
                + " + IFNULL(CAST(s.Professional_Fee_M AS DECIMAL(20, 2)), 0)"
                + " + IFNULL(CAST(s.Service_Fee_M AS DECIMAL(20, 2)), 0)"
                + " + IFNULL(CAST(s.VAT AS DECIMAL(20, 2)), 0)"
                + " + IFNULL(CAST(s.Non_VAT AS DECIMAL(20, 2)), 0)"
                + " + IFNULL(CAST(s.D1601C AS DECIMAL(20, 2)), 0)"
                + " + IFNULL(CAST(s.D1601E AS DECIMAL(20, 2)), 0)"
                + " + IFNULL(CAST(s.SSS_ER AS DECIMAL(20, 2)), 0)"
                + " + IFNULL(CAST(s.PHIC_ER AS DECIMAL(20, 2)), 0)"
                + " + IFNULL(CAST(s.Pag_IBIG_ER AS DECIMAL(20, 2)), 0)"
                + " + IFNULL(CAST(s.SSS_EE AS DECIMAL(20, 2)), 0)"
                + " + IFNULL(CAST(s.PHIC_EE AS DECIMAL(20, 2)), 0)"
                + " + IFNULL(CAST(s.Pag_IBIG_EE AS DECIMAL(20, 2)), 0)"
                + " + IFNULL(CAST(s.Certification_Fee AS DECIMAL(20, 2)), 0)"
                + " + IFNULL(CAST(s.Bookkeeping_M AS DECIMAL(20, 2)), 0)"
                + " + IFNULL(CAST(s.Inventory_M AS DECIMAL(20, 2)), 0)"

                + " - IFNULL(CAST(p.Retainers_Fee_M AS DECIMAL(20, 2)), 0)"
                + " + IFNULL(CAST(p.Professional_Fee_M AS DECIMAL(20, 2)), 0)"
                + " + IFNULL(CAST(p.Service_Fee_M AS DECIMAL(20, 2)), 0)"
                + " + IFNULL(CAST(p.VAT AS DECIMAL(20, 2)), 0)"
                + " + IFNULL(CAST(p.Non_VAT AS DECIMAL(20, 2)), 0)"
                + " + IFNULL(CAST(p.D1601C AS DECIMAL(20, 2)), 0)"
                + " + IFNULL(CAST(p.D1601E AS DECIMAL(20, 2)), 0)"
                + " + IFNULL(CAST(p.SSS_ER AS DECIMAL(20, 2)), 0)"
                + " + IFNULL(CAST(p.PHIC_ER AS DECIMAL(20, 2)), 0)"
                + " + IFNULL(CAST(p.Pag_IBIG_ER AS DECIMAL(20, 2)), 0)"
                + " + IFNULL(CAST(p.SSS_EE AS DECIMAL(20, 2)), 0)"
                + " + IFNULL(CAST(p.PHIC_EE AS DECIMAL(20, 2)), 0)"
                + " + IFNULL(CAST(p.Pag_IBIG_EE AS DECIMAL(20, 2)), 0)"
                + " + IFNULL(CAST(p.Certification_Fee AS DECIMAL(20, 2)), 0)"
                + " + IFNULL(CAST(p.Bookkeeping_M AS DECIMAL(20, 2)), 0)"
                + " + IFNULL(CAST(p.Inventory_M AS DECIMAL(20, 2)), 0) AS 'Uncollected Payments',"

                + " s.Remarks,"
                + " IF(IFNULL(CAST(s.Retainers_Fee_M AS DECIMAL(20, 2)), 0) = IFNULL(CAST(p.Retainers_Fee_M AS DECIMAL(20, 2)), 0) AND IFNULL(CAST(s.Retainers_Fee_M AS DECIMAL(20, 2)), 0) != 0, 1, 0) AS 'Retainers Fee Status',"
                + " IF(IFNULL(CAST(s.Professional_Fee_M AS DECIMAL(20, 2)), 0) = IFNULL(CAST(p.Professional_Fee_M AS DECIMAL(20, 2)), 0) AND IFNULL(CAST(s.Professional_Fee_M AS DECIMAL(20, 2)), 0) != 0, 1, 0) AS 'Professional Fee Status',"
                + " IF(IFNULL(CAST(s.Service_Fee_M AS DECIMAL(20, 2)), 0) = IFNULL(CAST(p.Service_Fee_M AS DECIMAL(20, 2)), 0) AND IFNULL(CAST(s.Service_Fee_M AS DECIMAL(20, 2)), 0) != 0, 1, 0) AS 'Service Fee Status',"
                + " IF(IFNULL(CAST(s.VAT AS DECIMAL(20, 2)), 0) = IFNULL(CAST(p.VAT AS DECIMAL(20, 2)), 0) AND IFNULL(CAST(s.VAT AS DECIMAL(20, 2)), 0) != 0, 1, 0) AS 'VAT Status',"
                + " IF(IFNULL(CAST(s.Non_VAT AS DECIMAL(20, 2)), 0) = IFNULL(CAST(p.Non_VAT AS DECIMAL(20, 2)), 0) AND IFNULL(CAST(s.Non_VAT AS DECIMAL(20, 2)), 0) != 0, 1, 0) AS 'Non-VAT Status',"
                + " IF(IFNULL(CAST(s.D1601C AS DECIMAL(20, 2)), 0) = IFNULL(CAST(p.D1601C AS DECIMAL(20, 2)), 0) AND IFNULL(CAST(s.D1601C AS DECIMAL(20, 2)), 0) != 0, 1, 0) AS '1601C Status',"
                + " IF(IFNULL(CAST(s.D1601E AS DECIMAL(20, 2)), 0) = IFNULL(CAST(p.D1601E AS DECIMAL(20, 2)), 0) AND IFNULL(CAST(s.D1601E AS DECIMAL(20, 2)), 0) != 0, 1, 0) AS '1601E Status',"
                + " IF(IFNULL(CAST(s.SSS_ER AS DECIMAL(20, 2)), 0) = IFNULL(CAST(p.SSS_ER AS DECIMAL(20, 2)), 0) AND IFNULL(CAST(s.SSS_ER AS DECIMAL(20, 2)), 0) != 0, 1, 0) AS 'SSS (ER) Status',"
                + " IF(IFNULL(CAST(s.PHIC_ER AS DECIMAL(20, 2)), 0) = IFNULL(CAST(p.PHIC_ER AS DECIMAL(20, 2)), 0) AND IFNULL(CAST(s.PHIC_ER AS DECIMAL(20, 2)), 0) != 0, 1, 0) AS 'PHIC (ER) Status',"
                + " IF(IFNULL(CAST(s.Pag_IBIG_ER AS DECIMAL(20, 2)), 0) = IFNULL(CAST(p.Pag_IBIG_ER AS DECIMAL(20, 2)), 0) AND IFNULL(CAST(s.Pag_IBIG_ER AS DECIMAL(20, 2)), 0) != 0, 1, 0) AS 'Pag-IBIG (ER) Status',"
                + " IF(IFNULL(CAST(s.SSS_EE AS DECIMAL(20, 2)), 0) = IFNULL(CAST(p.SSS_EE AS DECIMAL(20, 2)), 0) AND IFNULL(CAST(s.SSS_EE AS DECIMAL(20, 2)), 0) != 0, 1, 0) AS 'SSS (EE) Status',"
                + " IF(IFNULL(CAST(s.PHIC_EE AS DECIMAL(20, 2)), 0) = IFNULL(CAST(p.PHIC_EE AS DECIMAL(20, 2)), 0) AND IFNULL(CAST(s.PHIC_EE AS DECIMAL(20, 2)), 0) != 0, 1, 0) AS 'PHIC (EE) Status',"
                + " IF(IFNULL(CAST(s.Pag_IBIG_EE AS DECIMAL(20, 2)), 0) = IFNULL(CAST(p.Pag_IBIG_EE AS DECIMAL(20, 2)), 0) AND IFNULL(CAST(s.Pag_IBIG_EE AS DECIMAL(20, 2)), 0) != 0, 1, 0) AS 'Pag-IBIG (EE) Status',"
                + " IF(IFNULL(CAST(s.Certification_Fee AS DECIMAL(20, 2)), 0) = IFNULL(CAST(p.Certification_Fee AS DECIMAL(20, 2)), 0) AND IFNULL(CAST(s.Certification_Fee AS DECIMAL(20, 2)), 0) != 0, 1, 0) AS 'Certification Fee Status',"
                + " IF(IFNULL(CAST(s.Bookkeeping_M AS DECIMAL(20, 2)), 0) = IFNULL(CAST(p.Bookkeeping_M AS DECIMAL(20, 2)), 0) AND IFNULL(CAST(s.Bookkeeping_M AS DECIMAL(20, 2)), 0) != 0, 1, 0) AS 'Bookkeeping Status',"
                + " IF(IFNULL(CAST(s.Inventory_M AS DECIMAL(20, 2)), 0) = IFNULL(CAST(p.Inventory_M AS DECIMAL(20, 2)), 0) AND IFNULL(CAST(s.Inventory_M AS DECIMAL(20, 2)), 0) != 0, 1, 0) AS 'Inventory Status'"

                + " FROM tbl_mbillpay s LEFT JOIN tbl_clients c ON s.Client_ID = c.Client_ID LEFT JOIN vw_mpaylogs p ON CONCAT('TRN-', s.No_ID) = p.Transaction_Number" + sbpsearch + sbpdrange + sbpuname + sbporder + ";", cConn);

            try
            {
                MySqlDataAdapter cAdapter = new MySqlDataAdapter();
                cAdapter.SelectCommand = cCommand;
                cDatasetBPM = new DataTable();
                cAdapter.Fill(p, ibpmdpage, cDatasetBPM);
                BindingSource cSource = new BindingSource();
                cSource.DataSource = cDatasetBPM;
                dgvBPMonthly.DataSource = cSource;
                cAdapter.Update(cDatasetBPM);
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            dgvBPMonthly.Columns[0].Visible = false;
            dgvBPMonthly.Columns[3].Visible = false;
            dgvBPMonthly.Columns[7].Visible = false;
            dgvBPMonthly.Columns[8].Visible = false;
            dgvBPMonthly.Columns[9].Visible = false;
            dgvBPMonthly.Columns[10].Visible = false;
            dgvBPMonthly.Columns[11].Visible = false;
            dgvBPMonthly.Columns[12].Visible = false;
            dgvBPMonthly.Columns[13].Visible = false;
            dgvBPMonthly.Columns[14].Visible = false;

            dgvBPMonthly.Columns[35].Visible = false;
            dgvBPMonthly.Columns[36].Visible = false;
            dgvBPMonthly.Columns[37].Visible = false;
            dgvBPMonthly.Columns[38].Visible = false;
            dgvBPMonthly.Columns[39].Visible = false;
            dgvBPMonthly.Columns[40].Visible = false;
            dgvBPMonthly.Columns[41].Visible = false;
            dgvBPMonthly.Columns[42].Visible = false;
            dgvBPMonthly.Columns[43].Visible = false;
            dgvBPMonthly.Columns[44].Visible = false;
            dgvBPMonthly.Columns[45].Visible = false;
            dgvBPMonthly.Columns[46].Visible = false;
            dgvBPMonthly.Columns[47].Visible = false;
            dgvBPMonthly.Columns[48].Visible = false;
            dgvBPMonthly.Columns[49].Visible = false;
            dgvBPMonthly.Columns[50].Visible = false;

            BPTColors();
            CPayment();
            BPCServices();
        }
        #endregion

        #region Quarterly Services
        int ibpqp = 0, ibpqpage = 1, ibpqdpage = tcontent;
        double dbpqcount = 0, dbpqpage = 0, dbpqtpage = 0, dbpqrpage = 0;

        void BPQLoad(int p)
        {
            string cQueryI = "SELECT COUNT(*) FROM tbl_qbillpay s LEFT JOIN tbl_clients c ON s.Client_ID = c.Client_ID LEFT JOIN vw_qpaylogs p ON CONCAT('TRN-', s.No_ID) = p.Transaction_Number" + sbpsearch + sbpdrange + sbpuname + ";";
            MySqlConnection cConnectionI = new MySqlConnection(Conn.cString);
            MySqlCommand cCommandI = new MySqlCommand(cQueryI, cConnectionI);
            MySqlDataReader cReaderI;

            try
            {
                cConnectionI.Open();
                cReaderI = cCommandI.ExecuteReader();
                while (cReaderI.Read())
                {
                    dbpqcount = cReaderI.GetDouble(0);
                }
            }

            catch (MySqlException ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            finally
            {
                cConnectionI.Close();
            }

            MySqlConnection cConn = new MySqlConnection(Conn.cString);
            MySqlCommand cCommand = new MySqlCommand("SELECT s.No_ID,"
                + " DATE_FORMAT(DATE(Bill_Date), '%m-%d-%Y') AS 'Bill Date',"
                + " CONCAT('TRN-', s.No_ID) AS 'Transaction Number',"
                + " s.Client_ID AS 'Client I.D.',"
                + " CONCAT(c.Last_Name, ', ', c.First_Name, ' ', c.Middle_Initial) AS 'Client Name',"
                + " c.Business_Name AS 'Business Name',"
                + " c.Address,"
                + " c.Nature_of_Business AS 'Nature of Business',"
                + " c.Business_or_Owner_TIN AS 'Business / Owner TIN',"
                + " c.Tax_Type AS 'Tax Type',"
                + " c.SEC_Registration_Number AS 'SEC Reg. Number',"
                + " c.Group_Name AS 'Group Name',"
                + " c.Sub_Group_Name AS 'Sub-group Name',"
                + " c.Group_Year AS 'Group Year',"
                + " c.Status,"
                + " IFNULL(CAST(s.Retainers_Fee_Q AS DECIMAL(20, 2)), 0) AS 'Retainers Fee',"
                + " IFNULL(CAST(s.Professional_Fee_Q AS DECIMAL(20, 2)), 0) AS 'Professional Fee',"
                + " IFNULL(CAST(s.Service_Fee_Q AS DECIMAL(20, 2)), 0) AS 'Service Fee',"
                + " IFNULL(CAST(s.D1701Q AS DECIMAL(20, 2)), 0) AS '1701Q',"
                + " IFNULL(CAST(s.D1702Q AS DECIMAL(20, 2)), 0) AS '1702Q',"
                + " IFNULL(CAST(s.Bookkeeping_Q AS DECIMAL(20, 2)), 0) AS 'Bookkeeping',"
                + " IFNULL(CAST(s.Inventory_Q AS DECIMAL(20, 2)), 0) AS 'Inventory',"

                + " IFNULL(CAST(s.Retainers_Fee_Q AS DECIMAL(20, 2)), 0)"
                + " + IFNULL(CAST(s.Professional_Fee_Q AS DECIMAL(20, 2)), 0)"
                + " + IFNULL(CAST(s.Service_Fee_Q AS DECIMAL(20, 2)), 0)"
                + " + IFNULL(CAST(s.D1701Q AS DECIMAL(20, 2)), 0)"
                + " + IFNULL(CAST(s.D1702Q AS DECIMAL(20, 2)), 0)"
                + " + IFNULL(CAST(s.Bookkeeping_Q AS DECIMAL(20, 2)), 0)"
                + " + IFNULL(CAST(s.Inventory_Q AS DECIMAL(20, 2)), 0) AS 'Total Payments',"

                + " IFNULL(CAST(p.Retainers_Fee_Q AS DECIMAL(20, 2)), 0)"
                + " + IFNULL(CAST(p.Professional_Fee_Q AS DECIMAL(20, 2)), 0)"
                + " + IFNULL(CAST(p.Service_Fee_Q AS DECIMAL(20, 2)), 0)"
                + " + IFNULL(CAST(p.D1701Q AS DECIMAL(20, 2)), 0)"
                + " + IFNULL(CAST(p.D1702Q AS DECIMAL(20, 2)), 0)"
                + " + IFNULL(CAST(p.Bookkeeping_Q AS DECIMAL(20, 2)), 0)"
                + " + IFNULL(CAST(p.Inventory_Q AS DECIMAL(20, 2)), 0) AS 'Collected Payments',"

                + " IFNULL(CAST(s.Retainers_Fee_Q AS DECIMAL(20, 2)), 0)"
                + " + IFNULL(CAST(s.Professional_Fee_Q AS DECIMAL(20, 2)), 0)"
                + " + IFNULL(CAST(s.Service_Fee_Q AS DECIMAL(20, 2)), 0)"
                + " + IFNULL(CAST(s.D1701Q AS DECIMAL(20, 2)), 0)"
                + " + IFNULL(CAST(s.D1702Q AS DECIMAL(20, 2)), 0)"
                + " + IFNULL(CAST(s.Bookkeeping_Q AS DECIMAL(20, 2)), 0)"
                + " + IFNULL(CAST(s.Inventory_Q AS DECIMAL(20, 2)), 0)"

                + " - IFNULL(CAST(p.Retainers_Fee_Q AS DECIMAL(20, 2)), 0)"
                + " + IFNULL(CAST(p.Professional_Fee_Q AS DECIMAL(20, 2)), 0)"
                + " + IFNULL(CAST(p.Service_Fee_Q AS DECIMAL(20, 2)), 0)"
                + " + IFNULL(CAST(p.D1701Q AS DECIMAL(20, 2)), 0)"
                + " + IFNULL(CAST(p.D1702Q AS DECIMAL(20, 2)), 0)"
                + " + IFNULL(CAST(p.Bookkeeping_Q AS DECIMAL(20, 2)), 0)"
                + " + IFNULL(CAST(p.Inventory_Q AS DECIMAL(20, 2)), 0) AS 'Uncollected Payments',"

                + " s.Remarks,"
                + " IF(IFNULL(CAST(s.Retainers_Fee_Q AS DECIMAL(20, 2)), 0) = IFNULL(CAST(p.Retainers_Fee_Q AS DECIMAL(20, 2)), 0) AND IFNULL(CAST(s.Retainers_Fee_Q AS DECIMAL(20, 2)), 0) != 0, 1, 0) AS 'Retainers Fee Status',"
                + " IF(IFNULL(CAST(s.Professional_Fee_Q AS DECIMAL(20, 2)), 0) = IFNULL(CAST(p.Professional_Fee_Q AS DECIMAL(20, 2)), 0) AND IFNULL(CAST(s.Professional_Fee_Q AS DECIMAL(20, 2)), 0) != 0, 1, 0) AS 'Professional Fee Status',"
                + " IF(IFNULL(CAST(s.Service_Fee_Q AS DECIMAL(20, 2)), 0) = IFNULL(CAST(p.Service_Fee_Q AS DECIMAL(20, 2)), 0) AND IFNULL(CAST(s.Service_Fee_Q AS DECIMAL(20, 2)), 0) != 0, 1, 0) AS 'Service Fee Status',"
                + " IF(IFNULL(CAST(s.D1701Q AS DECIMAL(20, 2)), 0) = IFNULL(CAST(p.D1701Q AS DECIMAL(20, 2)), 0) AND IFNULL(CAST(s.D1701Q AS DECIMAL(20, 2)), 0) != 0, 1, 0) AS '1701Q Status',"
                + " IF(IFNULL(CAST(s.D1702Q AS DECIMAL(20, 2)), 0) = IFNULL(CAST(p.D1702Q AS DECIMAL(20, 2)), 0) AND IFNULL(CAST(s.D1702Q AS DECIMAL(20, 2)), 0) != 0, 1, 0) AS '1702Q Status',"
                + " IF(IFNULL(CAST(s.Bookkeeping_Q AS DECIMAL(20, 2)), 0) = IFNULL(CAST(p.Bookkeeping_Q AS DECIMAL(20, 2)), 0) AND IFNULL(CAST(s.Bookkeeping_Q AS DECIMAL(20, 2)), 0) != 0, 1, 0) AS 'Bookkeeping Status',"
                + " IF(IFNULL(CAST(s.Inventory_Q AS DECIMAL(20, 2)), 0) = IFNULL(CAST(p.Inventory_Q AS DECIMAL(20, 2)), 0) AND IFNULL(CAST(s.Inventory_Q AS DECIMAL(20, 2)), 0) != 0, 1, 0) AS 'Inventory Status'"

                + " FROM tbl_qbillpay s LEFT JOIN tbl_clients c ON s.Client_ID = c.Client_ID LEFT JOIN vw_qpaylogs p ON CONCAT('TRN-', s.No_ID) = p.Transaction_Number" + sbpsearch + sbpdrange + sbpuname + sbporder + ";", cConn);

            try
            {
                MySqlDataAdapter cAdapter = new MySqlDataAdapter();
                cAdapter.SelectCommand = cCommand;
                cDatasetBPQ = new DataTable();
                cAdapter.Fill(p, ibpqdpage, cDatasetBPQ);
                BindingSource cSource = new BindingSource();
                cSource.DataSource = cDatasetBPQ;
                dgvBPQuarterly.DataSource = cSource;
                cAdapter.Update(cDatasetBPQ);
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            dgvBPQuarterly.Columns[0].Visible = false;
            dgvBPQuarterly.Columns[3].Visible = false;
            dgvBPQuarterly.Columns[7].Visible = false;
            dgvBPQuarterly.Columns[8].Visible = false;
            dgvBPQuarterly.Columns[9].Visible = false;
            dgvBPQuarterly.Columns[10].Visible = false;
            dgvBPQuarterly.Columns[11].Visible = false;
            dgvBPQuarterly.Columns[12].Visible = false;
            dgvBPQuarterly.Columns[13].Visible = false;
            dgvBPQuarterly.Columns[14].Visible = false;

            dgvBPQuarterly.Columns[26].Visible = false;
            dgvBPQuarterly.Columns[27].Visible = false;
            dgvBPQuarterly.Columns[28].Visible = false;
            dgvBPQuarterly.Columns[29].Visible = false;
            dgvBPQuarterly.Columns[30].Visible = false;
            dgvBPQuarterly.Columns[31].Visible = false;
            dgvBPQuarterly.Columns[32].Visible = false;

            BPTColors();
            CPayment();
            BPCServices();
        }
        #endregion

        #region Annually Services
        int ibpap = 0, ibpapage = 1, ibpadpage = tcontent;
        double dbpacount = 0, dbpapage = 0, dbpatpage = 0, dbparpage = 0;

        void BPALoad(int p)
        {
            string cQueryI = "SELECT COUNT(*) FROM tbl_abillpay s LEFT JOIN tbl_clients c ON s.Client_ID = c.Client_ID LEFT JOIN vw_apaylogs p ON CONCAT('TRN-', s.No_ID) = p.Transaction_Number" + sbpsearch + sbpdrange + sbpuname + ";";
            MySqlConnection cConnectionI = new MySqlConnection(Conn.cString);
            MySqlCommand cCommandI = new MySqlCommand(cQueryI, cConnectionI);
            MySqlDataReader cReaderI;

            try
            {
                cConnectionI.Open();
                cReaderI = cCommandI.ExecuteReader();
                while (cReaderI.Read())
                {
                    dbpacount = cReaderI.GetDouble(0);
                }
            }

            catch (MySqlException ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            finally
            {
                cConnectionI.Close();
            }

            MySqlConnection cConn = new MySqlConnection(Conn.cString);
            MySqlCommand cCommand = new MySqlCommand("SELECT s.No_ID,"
                + " DATE_FORMAT(DATE(Bill_Date), '%m-%d-%Y') AS 'Bill Date',"
                + " CONCAT('TRN-', s.No_ID) AS 'Transaction Number',"
                + " s.Client_ID AS 'Client I.D.',"
                + " CONCAT(c.Last_Name, ', ', c.First_Name, ' ', c.Middle_Initial) AS 'Client Name',"
                + " c.Business_Name AS 'Business Name',"
                + " c.Address,"
                + " c.Nature_of_Business AS 'Nature of Business',"
                + " c.Business_or_Owner_TIN AS 'Business / Owner TIN',"
                + " c.Tax_Type AS 'Tax Type',"
                + " c.SEC_Registration_Number AS 'SEC Reg. Number',"
                + " c.Group_Name AS 'Group Name',"
                + " c.Sub_Group_Name AS 'Sub-group Name',"
                + " c.Group_Year AS 'Group Year',"
                + " c.Status,"
                + " IFNULL(CAST(s.Retainers_Fee_A AS DECIMAL(20, 2)), 0) AS 'Retainers Fee',"
                + " IFNULL(CAST(s.Professional_Fee_A AS DECIMAL(20, 2)), 0) AS 'Professional Fee',"
                + " IFNULL(CAST(s.Service_Fee_A AS DECIMAL(20, 2)), 0) AS 'Service Fee',"
                + " IFNULL(CAST(s.D1701 AS DECIMAL(20, 2)), 0) AS '1701',"
                + " IFNULL(CAST(s.D1702 AS DECIMAL(20, 2)), 0) AS '1702',"
                + " IFNULL(CAST(s.D1604CF AS DECIMAL(20, 2)), 0) AS '1604CF',"
                + " IFNULL(CAST(s.D1604E AS DECIMAL(20, 2)), 0) AS '1604E',"
                + " IFNULL(CAST(s.Municipal_License AS DECIMAL(20, 2)), 0) AS 'Municipal License',"
                + " IFNULL(CAST(s.COR AS DECIMAL(20, 2)), 0) AS 'COR',"
                + " IFNULL(CAST(s.Bookkeeping_A AS DECIMAL(20, 2)), 0) AS 'Bookkeeping',"
                + " IFNULL(CAST(s.Inventory_A AS DECIMAL(20, 2)), 0) AS 'Inventory',"

                + " IFNULL(CAST(s.Retainers_Fee_A AS DECIMAL(20, 2)), 0)"
                + " + IFNULL(CAST(s.Professional_Fee_A AS DECIMAL(20, 2)), 0)"
                + " + IFNULL(CAST(s.Service_Fee_A AS DECIMAL(20, 2)), 0)"
                + " + IFNULL(CAST(s.D1701 AS DECIMAL(20, 2)), 0)"
                + " + IFNULL(CAST(s.D1702 AS DECIMAL(20, 2)), 0)"
                + " + IFNULL(CAST(s.D1604CF AS DECIMAL(20, 2)), 0)"
                + " + IFNULL(CAST(s.D1604E AS DECIMAL(20, 2)), 0)"
                + " + IFNULL(CAST(s.Municipal_License AS DECIMAL(20, 2)), 0)"
                + " + IFNULL(CAST(s.COR AS DECIMAL(20, 2)), 0)"
                + " + IFNULL(CAST(s.Bookkeeping_A AS DECIMAL(20, 2)), 0)"
                + " + IFNULL(CAST(s.Inventory_A AS DECIMAL(20, 2)), 0) AS 'Total Payments',"

                + " IFNULL(CAST(p.Retainers_Fee_A AS DECIMAL(20, 2)), 0)"
                + " + IFNULL(CAST(p.Professional_Fee_A AS DECIMAL(20, 2)), 0)"
                + " + IFNULL(CAST(p.Service_Fee_A AS DECIMAL(20, 2)), 0)"
                + " + IFNULL(CAST(p.D1701 AS DECIMAL(20, 2)), 0)"
                + " + IFNULL(CAST(p.D1702 AS DECIMAL(20, 2)), 0)"
                + " + IFNULL(CAST(p.D1604CF AS DECIMAL(20, 2)), 0)"
                + " + IFNULL(CAST(p.D1604E AS DECIMAL(20, 2)), 0)"
                + " + IFNULL(CAST(p.Municipal_License AS DECIMAL(20, 2)), 0)"
                + " + IFNULL(CAST(p.COR AS DECIMAL(20, 2)), 0)"
                + " + IFNULL(CAST(p.Bookkeeping_A AS DECIMAL(20, 2)), 0)"
                + " + IFNULL(CAST(p.Inventory_A AS DECIMAL(20, 2)), 0) AS 'Collected Payments',"

                + " IFNULL(CAST(s.Retainers_Fee_A AS DECIMAL(20, 2)), 0)"
                + " + IFNULL(CAST(s.Professional_Fee_A AS DECIMAL(20, 2)), 0)"
                + " + IFNULL(CAST(s.Service_Fee_A AS DECIMAL(20, 2)), 0)"
                + " + IFNULL(CAST(s.D1701 AS DECIMAL(20, 2)), 0)"
                + " + IFNULL(CAST(s.D1702 AS DECIMAL(20, 2)), 0)"
                + " + IFNULL(CAST(s.D1604CF AS DECIMAL(20, 2)), 0)"
                + " + IFNULL(CAST(s.D1604E AS DECIMAL(20, 2)), 0)"
                + " + IFNULL(CAST(s.Municipal_License AS DECIMAL(20, 2)), 0)"
                + " + IFNULL(CAST(s.COR AS DECIMAL(20, 2)), 0)"
                + " + IFNULL(CAST(s.Bookkeeping_A AS DECIMAL(20, 2)), 0)"
                + " + IFNULL(CAST(s.Inventory_A AS DECIMAL(20, 2)), 0)"

                + " - IFNULL(CAST(p.Retainers_Fee_A AS DECIMAL(20, 2)), 0)"
                + " + IFNULL(CAST(p.Professional_Fee_A AS DECIMAL(20, 2)), 0)"
                + " + IFNULL(CAST(p.Service_Fee_A AS DECIMAL(20, 2)), 0)"
                + " + IFNULL(CAST(p.D1701 AS DECIMAL(20, 2)), 0)"
                + " + IFNULL(CAST(p.D1702 AS DECIMAL(20, 2)), 0)"
                + " + IFNULL(CAST(p.D1604CF AS DECIMAL(20, 2)), 0)"
                + " + IFNULL(CAST(p.D1604E AS DECIMAL(20, 2)), 0)"
                + " + IFNULL(CAST(p.Municipal_License AS DECIMAL(20, 2)), 0)"
                + " + IFNULL(CAST(p.COR AS DECIMAL(20, 2)), 0)"
                + " + IFNULL(CAST(p.Bookkeeping_A AS DECIMAL(20, 2)), 0)"
                + " + IFNULL(CAST(p.Inventory_A AS DECIMAL(20, 2)), 0) AS 'Uncollected Payments',"

                + " s.Remarks,"
                + " IF(IFNULL(CAST(s.Retainers_Fee_A AS DECIMAL(20, 2)), 0) = IFNULL(CAST(p.Retainers_Fee_A AS DECIMAL(20, 2)), 0) AND IFNULL(CAST(s.Retainers_Fee_A AS DECIMAL(20, 2)), 0) != 0, 1, 0) AS 'Retainers Fee Status',"
                + " IF(IFNULL(CAST(s.Professional_Fee_A AS DECIMAL(20, 2)), 0) = IFNULL(CAST(p.Professional_Fee_A AS DECIMAL(20, 2)), 0) AND IFNULL(CAST(s.Professional_Fee_A AS DECIMAL(20, 2)), 0) != 0, 1, 0) AS 'Professional Fee Status',"
                + " IF(IFNULL(CAST(s.Service_Fee_A AS DECIMAL(20, 2)), 0) = IFNULL(CAST(p.Service_Fee_A AS DECIMAL(20, 2)), 0) AND IFNULL(CAST(s.Service_Fee_A AS DECIMAL(20, 2)), 0) != 0, 1, 0) AS 'Service Fee Status',"
                + " IF(IFNULL(CAST(s.D1701 AS DECIMAL(20, 2)), 0) = IFNULL(CAST(p.D1701 AS DECIMAL(20, 2)), 0) AND IFNULL(CAST(s.D1701 AS DECIMAL(20, 2)), 0) != 0, 1, 0) AS '1701 Status',"
                + " IF(IFNULL(CAST(s.D1702 AS DECIMAL(20, 2)), 0) = IFNULL(CAST(p.D1702 AS DECIMAL(20, 2)), 0) AND IFNULL(CAST(s.D1702 AS DECIMAL(20, 2)), 0) != 0, 1, 0) AS '1702 Status',"
                + " IF(IFNULL(CAST(s.D1604CF AS DECIMAL(20, 2)), 0) = IFNULL(CAST(p.D1604CF AS DECIMAL(20, 2)), 0) AND IFNULL(CAST(s.D1604CF AS DECIMAL(20, 2)), 0) != 0, 1, 0) AS '1604CF Status',"
                + " IF(IFNULL(CAST(s.D1604E AS DECIMAL(20, 2)), 0) = IFNULL(CAST(p.D1604E AS DECIMAL(20, 2)), 0) AND IFNULL(CAST(s.D1604E AS DECIMAL(20, 2)), 0) != 0, 1, 0) AS '1604E Status',"
                + " IF(IFNULL(CAST(s.Municipal_License AS DECIMAL(20, 2)), 0) = IFNULL(CAST(p.Municipal_License AS DECIMAL(20, 2)), 0) AND IFNULL(CAST(s.Municipal_License AS DECIMAL(20, 2)), 0) != 0, 1, 0) AS 'Municipal License Status',"
                + " IF(IFNULL(CAST(s.COR AS DECIMAL(20, 2)), 0) = IFNULL(CAST(p.COR AS DECIMAL(20, 2)), 0) AND IFNULL(CAST(s.COR AS DECIMAL(20, 2)), 0) != 0, 1, 0) AS 'COR Status',"
                + " IF(IFNULL(CAST(s.Bookkeeping_A AS DECIMAL(20, 2)), 0) = IFNULL(CAST(p.Bookkeeping_A AS DECIMAL(20, 2)), 0) AND IFNULL(CAST(s.Bookkeeping_A AS DECIMAL(20, 2)), 0) != 0, 1, 0) AS 'Bookkeeping Status',"
                + " IF(IFNULL(CAST(s.Inventory_A AS DECIMAL(20, 2)), 0) = IFNULL(CAST(p.Inventory_A AS DECIMAL(20, 2)), 0) AND IFNULL(CAST(s.Inventory_A AS DECIMAL(20, 2)), 0) != 0, 1, 0) AS 'Inventory Status'"

                + " FROM tbl_abillpay s LEFT JOIN tbl_clients c ON s.Client_ID = c.Client_ID LEFT JOIN vw_apaylogs p ON CONCAT('TRN-', s.No_ID) = p.Transaction_Number" + sbpsearch + sbpdrange + sbpuname + sbporder + ";", cConn);

            try
            {
                MySqlDataAdapter cAdapter = new MySqlDataAdapter();
                cAdapter.SelectCommand = cCommand;
                cDatasetBPA = new DataTable();
                cAdapter.Fill(p, ibpadpage, cDatasetBPA);
                BindingSource cSource = new BindingSource();
                cSource.DataSource = cDatasetBPA;
                dgvBPAnnually.DataSource = cSource;
                cAdapter.Update(cDatasetBPA);
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            dgvBPAnnually.Columns[0].Visible = false;
            dgvBPAnnually.Columns[3].Visible = false;
            dgvBPAnnually.Columns[7].Visible = false;
            dgvBPAnnually.Columns[8].Visible = false;
            dgvBPAnnually.Columns[9].Visible = false;
            dgvBPAnnually.Columns[10].Visible = false;
            dgvBPAnnually.Columns[11].Visible = false;
            dgvBPAnnually.Columns[12].Visible = false;
            dgvBPAnnually.Columns[13].Visible = false;
            dgvBPAnnually.Columns[14].Visible = false;

            dgvBPAnnually.Columns[30].Visible = false;
            dgvBPAnnually.Columns[31].Visible = false;
            dgvBPAnnually.Columns[32].Visible = false;
            dgvBPAnnually.Columns[33].Visible = false;
            dgvBPAnnually.Columns[34].Visible = false;
            dgvBPAnnually.Columns[35].Visible = false;
            dgvBPAnnually.Columns[36].Visible = false;
            dgvBPAnnually.Columns[37].Visible = false;
            dgvBPAnnually.Columns[38].Visible = false;
            dgvBPAnnually.Columns[39].Visible = false;
            dgvBPAnnually.Columns[40].Visible = false;

            BPTColors();
            CPayment();
            BPCServices();
        }
        #endregion

        #region Clear Payment
        void CPayment()
        {
            if (dgvBPMonthly.Visible)
            {
                if (dgvBPMonthly.RowCount == 0)
                {
                    txtBPPayment.Clear();
                }

                else
                {
                    txtBPPayment.Text = "Php 0.00";
                }
            }

            else if (dgvBPQuarterly.Visible)
            {
                if (dgvBPQuarterly.RowCount == 0)
                {
                    txtBPPayment.Clear();
                }

                else
                {
                    txtBPPayment.Text = "Php 0.00";
                }
            }

            else if (dgvBPAnnually.Visible)
            {
                if (dgvBPAnnually.RowCount == 0)
                {
                    txtBPPayment.Clear();
                }

                else
                {
                    txtBPPayment.Text = "Php 0.00";
                }
            }
        }
        #endregion

        #endregion

        #region Table Colors
        void BPTColors()
        {
            double z = 0;

            #region Monthly Services
            foreach (DataGridViewRow row in dgvBPMonthly.Rows)
            {
                if (row.Cells["Retainers Fee Status"].Value.ToString() == "1")
                {
                    row.Cells["Retainers Fee"].Style.ForeColor = Color.MediumBlue;
                    row.Cells["Retainers Fee"].Style.SelectionBackColor = Color.MediumBlue;
                }

                else if (Double.TryParse(row.Cells["Retainers Fee"].Value.ToString().Replace("Php ", ""), out z) && z == 0)
                {
                    row.Cells["Retainers Fee"].Style.ForeColor = Color.Black;
                    row.Cells["Retainers Fee"].Style.SelectionBackColor = SystemColors.Highlight;
                }

                else
                {
                    row.Cells["Retainers Fee"].Style.ForeColor = Color.Red;
                    row.Cells["Retainers Fee"].Style.SelectionBackColor = Color.Red;
                }

                if (row.Cells["Professional Fee Status"].Value.ToString() == "1")
                {
                    row.Cells["Professional Fee"].Style.ForeColor = Color.MediumBlue;
                    row.Cells["Professional Fee"].Style.SelectionBackColor = Color.MediumBlue;
                }

                else if (Double.TryParse(row.Cells["Professional Fee"].Value.ToString().Replace("Php ", ""), out z) && z == 0)
                {
                    row.Cells["Professional Fee"].Style.ForeColor = Color.Black;
                    row.Cells["Professional Fee"].Style.SelectionBackColor = SystemColors.Highlight;
                }

                else
                {
                    row.Cells["Professional Fee"].Style.ForeColor = Color.Red;
                    row.Cells["Professional Fee"].Style.SelectionBackColor = Color.Red;
                }

                if (row.Cells["Service Fee Status"].Value.ToString() == "1")
                {
                    row.Cells["Service Fee"].Style.ForeColor = Color.MediumBlue;
                    row.Cells["Service Fee"].Style.SelectionBackColor = Color.MediumBlue;
                }

                else if (Double.TryParse(row.Cells["Service Fee"].Value.ToString().Replace("Php ", ""), out z) && z == 0)
                {
                    row.Cells["Service Fee"].Style.ForeColor = Color.Black;
                    row.Cells["Service Fee"].Style.SelectionBackColor = SystemColors.Highlight;
                }

                else
                {
                    row.Cells["Service Fee"].Style.ForeColor = Color.Red;
                    row.Cells["Service Fee"].Style.SelectionBackColor = Color.Red;
                }

                if (row.Cells["VAT Status"].Value.ToString() == "1")
                {
                    row.Cells["VAT"].Style.ForeColor = Color.MediumBlue;
                    row.Cells["VAT"].Style.SelectionBackColor = Color.MediumBlue;
                }

                else if (Double.TryParse(row.Cells["VAT"].Value.ToString().Replace("Php ", ""), out z) && z == 0)
                {
                    row.Cells["VAT"].Style.ForeColor = Color.Black;
                    row.Cells["VAT"].Style.SelectionBackColor = SystemColors.Highlight;
                }

                else
                {
                    row.Cells["VAT"].Style.ForeColor = Color.Red;
                    row.Cells["VAT"].Style.SelectionBackColor = Color.Red;
                }

                if (row.Cells["Non-VAT Status"].Value.ToString() == "1")
                {
                    row.Cells["Non-VAT"].Style.ForeColor = Color.MediumBlue;
                    row.Cells["Non-VAT"].Style.SelectionBackColor = Color.MediumBlue;
                }

                else if (Double.TryParse(row.Cells["Non-VAT"].Value.ToString().Replace("Php ", ""), out z) && z == 0)
                {
                    row.Cells["Non-VAT"].Style.ForeColor = Color.Black;
                    row.Cells["Non-VAT"].Style.SelectionBackColor = SystemColors.Highlight;
                }

                else
                {
                    row.Cells["Non-VAT"].Style.ForeColor = Color.Red;
                    row.Cells["Non-VAT"].Style.SelectionBackColor = Color.Red;
                }

                if (row.Cells["1601C Status"].Value.ToString() == "1")
                {
                    row.Cells["1601C"].Style.ForeColor = Color.MediumBlue;
                    row.Cells["1601C"].Style.SelectionBackColor = Color.MediumBlue;
                }

                else if (Double.TryParse(row.Cells["1601C"].Value.ToString().Replace("Php ", ""), out z) && z == 0)
                {
                    row.Cells["1601C"].Style.ForeColor = Color.Black;
                    row.Cells["1601C"].Style.SelectionBackColor = SystemColors.Highlight;
                }

                else
                {
                    row.Cells["1601C"].Style.ForeColor = Color.Red;
                    row.Cells["1601C"].Style.SelectionBackColor = Color.Red;
                }

                if (row.Cells["1601E Status"].Value.ToString() == "1")
                {
                    row.Cells["1601E"].Style.ForeColor = Color.MediumBlue;
                    row.Cells["1601E"].Style.SelectionBackColor = Color.MediumBlue;
                }

                else if (Double.TryParse(row.Cells["1601E"].Value.ToString().Replace("Php ", ""), out z) && z == 0)
                {
                    row.Cells["1601E"].Style.ForeColor = Color.Black;
                    row.Cells["1601E"].Style.SelectionBackColor = SystemColors.Highlight;
                }

                else
                {
                    row.Cells["1601E"].Style.ForeColor = Color.Red;
                    row.Cells["1601E"].Style.SelectionBackColor = Color.Red;
                }

                if (row.Cells["SSS (ER) Status"].Value.ToString() == "1")
                {
                    row.Cells["SSS (ER)"].Style.ForeColor = Color.MediumBlue;
                    row.Cells["SSS (ER)"].Style.SelectionBackColor = Color.MediumBlue;
                }

                else if (Double.TryParse(row.Cells["SSS (ER)"].Value.ToString().Replace("Php ", ""), out z) && z == 0)
                {
                    row.Cells["SSS (ER)"].Style.ForeColor = Color.Black;
                    row.Cells["SSS (ER)"].Style.SelectionBackColor = SystemColors.Highlight;
                }

                else
                {
                    row.Cells["SSS (ER)"].Style.ForeColor = Color.Red;
                    row.Cells["SSS (ER)"].Style.SelectionBackColor = Color.Red;
                }

                if (row.Cells["PHIC (ER) Status"].Value.ToString() == "1")
                {
                    row.Cells["PHIC (ER)"].Style.ForeColor = Color.MediumBlue;
                    row.Cells["PHIC (ER)"].Style.SelectionBackColor = Color.MediumBlue;
                }

                else if (Double.TryParse(row.Cells["PHIC (ER)"].Value.ToString().Replace("Php ", ""), out z) && z == 0)
                {
                    row.Cells["PHIC (ER)"].Style.ForeColor = Color.Black;
                    row.Cells["PHIC (ER)"].Style.SelectionBackColor = SystemColors.Highlight;
                }

                else
                {
                    row.Cells["PHIC (ER)"].Style.ForeColor = Color.Red;
                    row.Cells["PHIC (ER)"].Style.SelectionBackColor = Color.Red;
                }

                if (row.Cells["Pag-IBIG (ER) Status"].Value.ToString() == "1")
                {
                    row.Cells["Pag-IBIG (ER)"].Style.ForeColor = Color.MediumBlue;
                    row.Cells["Pag-IBIG (ER)"].Style.SelectionBackColor = Color.MediumBlue;
                }

                else if (Double.TryParse(row.Cells["Pag-IBIG (ER)"].Value.ToString().Replace("Php ", ""), out z) && z == 0)
                {
                    row.Cells["Pag-IBIG (ER)"].Style.ForeColor = Color.Black;
                    row.Cells["Pag-IBIG (ER)"].Style.SelectionBackColor = SystemColors.Highlight;
                }

                else
                {
                    row.Cells["Pag-IBIG (ER)"].Style.ForeColor = Color.Red;
                    row.Cells["Pag-IBIG (ER)"].Style.SelectionBackColor = Color.Red;
                }

                if (row.Cells["SSS (EE) Status"].Value.ToString() == "1")
                {
                    row.Cells["SSS (EE)"].Style.ForeColor = Color.MediumBlue;
                    row.Cells["SSS (EE)"].Style.SelectionBackColor = Color.MediumBlue;
                }

                else if (Double.TryParse(row.Cells["SSS (EE)"].Value.ToString().Replace("Php ", ""), out z) && z == 0)
                {
                    row.Cells["SSS (EE)"].Style.ForeColor = Color.Black;
                    row.Cells["SSS (EE)"].Style.SelectionBackColor = SystemColors.Highlight;
                }

                else
                {
                    row.Cells["SSS (EE)"].Style.ForeColor = Color.Red;
                    row.Cells["SSS (EE)"].Style.SelectionBackColor = Color.Red;
                }

                if (row.Cells["PHIC (EE) Status"].Value.ToString() == "1")
                {
                    row.Cells["PHIC (EE)"].Style.ForeColor = Color.MediumBlue;
                    row.Cells["PHIC (EE)"].Style.SelectionBackColor = Color.MediumBlue;
                }

                else if (Double.TryParse(row.Cells["PHIC (EE)"].Value.ToString().Replace("Php ", ""), out z) && z == 0)
                {
                    row.Cells["PHIC (EE)"].Style.ForeColor = Color.Black;
                    row.Cells["PHIC (EE)"].Style.SelectionBackColor = SystemColors.Highlight;
                }

                else
                {
                    row.Cells["PHIC (EE)"].Style.ForeColor = Color.Red;
                    row.Cells["PHIC (EE)"].Style.SelectionBackColor = Color.Red;
                }

                if (row.Cells["Pag-IBIG (EE) Status"].Value.ToString() == "1")
                {
                    row.Cells["Pag-IBIG (EE)"].Style.ForeColor = Color.MediumBlue;
                    row.Cells["Pag-IBIG (EE)"].Style.SelectionBackColor = Color.MediumBlue;
                }

                else if (Double.TryParse(row.Cells["Pag-IBIG (EE)"].Value.ToString().Replace("Php ", ""), out z) && z == 0)
                {
                    row.Cells["Pag-IBIG (EE)"].Style.ForeColor = Color.Black;
                    row.Cells["Pag-IBIG (EE)"].Style.SelectionBackColor = SystemColors.Highlight;
                }

                else
                {
                    row.Cells["Pag-IBIG (EE)"].Style.ForeColor = Color.Red;
                    row.Cells["Pag-IBIG (EE)"].Style.SelectionBackColor = Color.Red;
                }

                if (row.Cells["Certification Fee Status"].Value.ToString() == "1")
                {
                    row.Cells["Certification Fee"].Style.ForeColor = Color.MediumBlue;
                    row.Cells["Certification Fee"].Style.SelectionBackColor = Color.MediumBlue;
                }

                else if (Double.TryParse(row.Cells["Certification Fee"].Value.ToString().Replace("Php ", ""), out z) && z == 0)
                {
                    row.Cells["Certification Fee"].Style.ForeColor = Color.Black;
                    row.Cells["Certification Fee"].Style.SelectionBackColor = SystemColors.Highlight;
                }

                else
                {
                    row.Cells["Certification Fee"].Style.ForeColor = Color.Red;
                    row.Cells["Certification Fee"].Style.SelectionBackColor = Color.Red;
                }

                if (row.Cells["Inventory Status"].Value.ToString() == "1")
                {
                    row.Cells["Inventory"].Style.ForeColor = Color.MediumBlue;
                    row.Cells["Inventory"].Style.SelectionBackColor = Color.MediumBlue;
                }

                else if (Double.TryParse(row.Cells["Inventory"].Value.ToString().Replace("Php ", ""), out z) && z == 0)
                {
                    row.Cells["Inventory"].Style.ForeColor = Color.Black;
                    row.Cells["Inventory"].Style.SelectionBackColor = SystemColors.Highlight;
                }

                else
                {
                    row.Cells["Inventory"].Style.ForeColor = Color.Red;
                    row.Cells["Inventory"].Style.SelectionBackColor = Color.Red;
                }

                if (row.Cells["Bookkeeping Status"].Value.ToString() == "1")
                {
                    row.Cells["Bookkeeping"].Style.ForeColor = Color.MediumBlue;
                    row.Cells["Bookkeeping"].Style.SelectionBackColor = Color.MediumBlue;
                }

                else if (Double.TryParse(row.Cells["Bookkeeping"].Value.ToString().Replace("Php ", ""), out z) && z == 0)
                {
                    row.Cells["Bookkeeping"].Style.ForeColor = Color.Black;
                    row.Cells["Bookkeeping"].Style.SelectionBackColor = SystemColors.Highlight;
                }

                else
                {
                    row.Cells["Bookkeeping"].Style.ForeColor = Color.Red;
                    row.Cells["Bookkeeping"].Style.SelectionBackColor = Color.Red;
                }
            }
            #endregion

            #region Quarterly Services
            foreach (DataGridViewRow row in dgvBPQuarterly.Rows)
            {
                if (row.Cells["Retainers Fee Status"].Value.ToString() == "1")
                {
                    row.Cells["Retainers Fee"].Style.ForeColor = Color.MediumBlue;
                    row.Cells["Retainers Fee"].Style.SelectionBackColor = Color.MediumBlue;
                }

                else if (Double.TryParse(row.Cells["Retainers Fee"].Value.ToString().Replace("Php ", ""), out z) && z == 0)
                {
                    row.Cells["Retainers Fee"].Style.ForeColor = Color.Black;
                    row.Cells["Retainers Fee"].Style.SelectionBackColor = SystemColors.Highlight;
                }

                else
                {
                    row.Cells["Retainers Fee"].Style.ForeColor = Color.Red;
                    row.Cells["Retainers Fee"].Style.SelectionBackColor = Color.Red;
                }

                if (row.Cells["Professional Fee Status"].Value.ToString() == "1")
                {
                    row.Cells["Professional Fee"].Style.ForeColor = Color.MediumBlue;
                    row.Cells["Professional Fee"].Style.SelectionBackColor = Color.MediumBlue;
                }

                else if (Double.TryParse(row.Cells["Professional Fee"].Value.ToString().Replace("Php ", ""), out z) && z == 0)
                {
                    row.Cells["Professional Fee"].Style.ForeColor = Color.Black;
                    row.Cells["Professional Fee"].Style.SelectionBackColor = SystemColors.Highlight;
                }

                else
                {
                    row.Cells["Professional Fee"].Style.ForeColor = Color.Red;
                    row.Cells["Professional Fee"].Style.SelectionBackColor = Color.Red;
                }

                if (row.Cells["Service Fee Status"].Value.ToString() == "1")
                {
                    row.Cells["Service Fee"].Style.ForeColor = Color.MediumBlue;
                    row.Cells["Service Fee"].Style.SelectionBackColor = Color.MediumBlue;
                }

                else if (Double.TryParse(row.Cells["Service Fee"].Value.ToString().Replace("Php ", ""), out z) && z == 0)
                {
                    row.Cells["Service Fee"].Style.ForeColor = Color.Black;
                    row.Cells["Service Fee"].Style.SelectionBackColor = SystemColors.Highlight;
                }

                else
                {
                    row.Cells["Service Fee"].Style.ForeColor = Color.Red;
                    row.Cells["Service Fee"].Style.SelectionBackColor = Color.Red;
                }

                if (row.Cells["1701Q Status"].Value.ToString() == "1")
                {
                    row.Cells["1701Q"].Style.ForeColor = Color.MediumBlue;
                    row.Cells["1701Q"].Style.SelectionBackColor = Color.MediumBlue;
                }

                else if (Double.TryParse(row.Cells["1701Q"].Value.ToString().Replace("Php ", ""), out z) && z == 0)
                {
                    row.Cells["1701Q"].Style.ForeColor = Color.Black;
                    row.Cells["1701Q"].Style.SelectionBackColor = SystemColors.Highlight;
                }

                else
                {
                    row.Cells["1701Q"].Style.ForeColor = Color.Red;
                    row.Cells["1701Q"].Style.SelectionBackColor = Color.Red;
                }

                if (row.Cells["1702Q Status"].Value.ToString() == "1")
                {
                    row.Cells["1702Q"].Style.ForeColor = Color.MediumBlue;
                    row.Cells["1702Q"].Style.SelectionBackColor = Color.MediumBlue;
                }

                else if (Double.TryParse(row.Cells["1702Q"].Value.ToString().Replace("Php ", ""), out z) && z == 0)
                {
                    row.Cells["1702Q"].Style.ForeColor = Color.Black;
                    row.Cells["1702Q"].Style.SelectionBackColor = SystemColors.Highlight;
                }

                else
                {
                    row.Cells["1702Q"].Style.ForeColor = Color.Red;
                    row.Cells["1702Q"].Style.SelectionBackColor = Color.Red;
                }

                if (row.Cells["Inventory Status"].Value.ToString() == "1")
                {
                    row.Cells["Inventory"].Style.ForeColor = Color.MediumBlue;
                    row.Cells["Inventory"].Style.SelectionBackColor = Color.MediumBlue;
                }

                else if (Double.TryParse(row.Cells["Inventory"].Value.ToString().Replace("Php ", ""), out z) && z == 0)
                {
                    row.Cells["Inventory"].Style.ForeColor = Color.Black;
                    row.Cells["Inventory"].Style.SelectionBackColor = SystemColors.Highlight;
                }

                else
                {
                    row.Cells["Inventory"].Style.ForeColor = Color.Red;
                    row.Cells["Inventory"].Style.SelectionBackColor = Color.Red;
                }

                if (row.Cells["Bookkeeping Status"].Value.ToString() == "1")
                {
                    row.Cells["Bookkeeping"].Style.ForeColor = Color.MediumBlue;
                    row.Cells["Bookkeeping"].Style.SelectionBackColor = Color.MediumBlue;
                }

                else if (Double.TryParse(row.Cells["Bookkeeping"].Value.ToString().Replace("Php ", ""), out z) && z == 0)
                {
                    row.Cells["Bookkeeping"].Style.ForeColor = Color.Black;
                    row.Cells["Bookkeeping"].Style.SelectionBackColor = SystemColors.Highlight;
                }

                else
                {
                    row.Cells["Bookkeeping"].Style.ForeColor = Color.Red;
                    row.Cells["Bookkeeping"].Style.SelectionBackColor = Color.Red;
                }
            }
            #endregion

            #region Annually Services
            foreach (DataGridViewRow row in dgvBPAnnually.Rows)
            {
                if (row.Cells["Retainers Fee Status"].Value.ToString() == "1")
                {
                    row.Cells["Retainers Fee"].Style.ForeColor = Color.MediumBlue;
                    row.Cells["Retainers Fee"].Style.SelectionBackColor = Color.MediumBlue;
                }

                else if (Double.TryParse(row.Cells["Retainers Fee"].Value.ToString().Replace("Php ", ""), out z) && z == 0)
                {
                    row.Cells["Retainers Fee"].Style.ForeColor = Color.Black;
                    row.Cells["Retainers Fee"].Style.SelectionBackColor = SystemColors.Highlight;
                }

                else
                {
                    row.Cells["Retainers Fee"].Style.ForeColor = Color.Red;
                    row.Cells["Retainers Fee"].Style.SelectionBackColor = Color.Red;
                }

                if (row.Cells["Professional Fee Status"].Value.ToString() == "1")
                {
                    row.Cells["Professional Fee"].Style.ForeColor = Color.MediumBlue;
                    row.Cells["Professional Fee"].Style.SelectionBackColor = Color.MediumBlue;
                }

                else if (Double.TryParse(row.Cells["Professional Fee"].Value.ToString().Replace("Php ", ""), out z) && z == 0)
                {
                    row.Cells["Professional Fee"].Style.ForeColor = Color.Black;
                    row.Cells["Professional Fee"].Style.SelectionBackColor = SystemColors.Highlight;
                }

                else
                {
                    row.Cells["Professional Fee"].Style.ForeColor = Color.Red;
                    row.Cells["Professional Fee"].Style.SelectionBackColor = Color.Red;
                }

                if (row.Cells["Service Fee Status"].Value.ToString() == "1")
                {
                    row.Cells["Service Fee"].Style.ForeColor = Color.MediumBlue;
                    row.Cells["Service Fee"].Style.SelectionBackColor = Color.MediumBlue;
                }

                else if (Double.TryParse(row.Cells["Service Fee"].Value.ToString().Replace("Php ", ""), out z) && z == 0)
                {
                    row.Cells["Service Fee"].Style.ForeColor = Color.Black;
                    row.Cells["Service Fee"].Style.SelectionBackColor = SystemColors.Highlight;
                }

                else
                {
                    row.Cells["Service Fee"].Style.ForeColor = Color.Red;
                    row.Cells["Service Fee"].Style.SelectionBackColor = Color.Red;
                }

                if (row.Cells["1701 Status"].Value.ToString() == "1")
                {
                    row.Cells["1701"].Style.ForeColor = Color.MediumBlue;
                    row.Cells["1701"].Style.SelectionBackColor = Color.MediumBlue;
                }

                else if (Double.TryParse(row.Cells["1701"].Value.ToString().Replace("Php ", ""), out z) && z == 0)
                {
                    row.Cells["1701"].Style.ForeColor = Color.Black;
                    row.Cells["1701"].Style.SelectionBackColor = SystemColors.Highlight;
                }

                else
                {
                    row.Cells["1701"].Style.ForeColor = Color.Red;
                    row.Cells["1701"].Style.SelectionBackColor = Color.Red;
                }

                if (row.Cells["1702 Status"].Value.ToString() == "1")
                {
                    row.Cells["1702"].Style.ForeColor = Color.MediumBlue;
                    row.Cells["1702"].Style.SelectionBackColor = Color.MediumBlue;
                }

                else if (Double.TryParse(row.Cells["1702"].Value.ToString().Replace("Php ", ""), out z) && z == 0)
                {
                    row.Cells["1702"].Style.ForeColor = Color.Black;
                    row.Cells["1702"].Style.SelectionBackColor = SystemColors.Highlight;
                }

                else
                {
                    row.Cells["1702"].Style.ForeColor = Color.Red;
                    row.Cells["1702"].Style.SelectionBackColor = Color.Red;
                }

                if (row.Cells["1604CF Status"].Value.ToString() == "1")
                {
                    row.Cells["1604CF"].Style.ForeColor = Color.MediumBlue;
                    row.Cells["1604CF"].Style.SelectionBackColor = Color.MediumBlue;
                }

                else if (Double.TryParse(row.Cells["1604CF"].Value.ToString().Replace("Php ", ""), out z) && z == 0)
                {
                    row.Cells["1604CF"].Style.ForeColor = Color.Black;
                    row.Cells["1604CF"].Style.SelectionBackColor = SystemColors.Highlight;
                }

                else
                {
                    row.Cells["1604CF"].Style.ForeColor = Color.Red;
                    row.Cells["1604CF"].Style.SelectionBackColor = Color.Red;
                }

                if (row.Cells["1604E Status"].Value.ToString() == "1")
                {
                    row.Cells["1604E"].Style.ForeColor = Color.MediumBlue;
                    row.Cells["1604E"].Style.SelectionBackColor = Color.MediumBlue;
                }

                else if (Double.TryParse(row.Cells["1604E"].Value.ToString().Replace("Php ", ""), out z) && z == 0)
                {
                    row.Cells["1604E"].Style.ForeColor = Color.Black;
                    row.Cells["1604E"].Style.SelectionBackColor = SystemColors.Highlight;
                }

                else
                {
                    row.Cells["1604E"].Style.ForeColor = Color.Red;
                    row.Cells["1604E"].Style.SelectionBackColor = Color.Red;
                }

                if (row.Cells["Municipal License Status"].Value.ToString() == "1")
                {
                    row.Cells["Municipal License"].Style.ForeColor = Color.MediumBlue;
                    row.Cells["Municipal License"].Style.SelectionBackColor = Color.MediumBlue;
                }

                else if (Double.TryParse(row.Cells["Municipal License"].Value.ToString().Replace("Php ", ""), out z) && z == 0)
                {
                    row.Cells["Municipal License"].Style.ForeColor = Color.Black;
                    row.Cells["Municipal License"].Style.SelectionBackColor = SystemColors.Highlight;
                }

                else
                {
                    row.Cells["Municipal License"].Style.ForeColor = Color.Red;
                    row.Cells["Municipal License"].Style.SelectionBackColor = Color.Red;
                }

                if (row.Cells["COR Status"].Value.ToString() == "1")
                {
                    row.Cells["COR"].Style.ForeColor = Color.MediumBlue;
                    row.Cells["COR"].Style.SelectionBackColor = Color.MediumBlue;
                }

                else if (Double.TryParse(row.Cells["COR"].Value.ToString().Replace("Php ", ""), out z) && z == 0)
                {
                    row.Cells["COR"].Style.ForeColor = Color.Black;
                    row.Cells["COR"].Style.SelectionBackColor = SystemColors.Highlight;
                }

                else
                {
                    row.Cells["COR"].Style.ForeColor = Color.Red;
                    row.Cells["COR"].Style.SelectionBackColor = Color.Red;
                }

                if (row.Cells["Inventory Status"].Value.ToString() == "1")
                {
                    row.Cells["Inventory"].Style.ForeColor = Color.MediumBlue;
                    row.Cells["Inventory"].Style.SelectionBackColor = Color.MediumBlue;
                }

                else if (Double.TryParse(row.Cells["Inventory"].Value.ToString().Replace("Php ", ""), out z) && z == 0)
                {
                    row.Cells["Inventory"].Style.ForeColor = Color.Black;
                    row.Cells["Inventory"].Style.SelectionBackColor = SystemColors.Highlight;
                }

                else
                {
                    row.Cells["Inventory"].Style.ForeColor = Color.Red;
                    row.Cells["Inventory"].Style.SelectionBackColor = Color.Red;
                }

                if (row.Cells["Bookkeeping Status"].Value.ToString() == "1")
                {
                    row.Cells["Bookkeeping"].Style.ForeColor = Color.MediumBlue;
                    row.Cells["Bookkeeping"].Style.SelectionBackColor = Color.MediumBlue;
                }

                else if (Double.TryParse(row.Cells["Bookkeeping"].Value.ToString().Replace("Php ", ""), out z) && z == 0)
                {
                    row.Cells["Bookkeeping"].Style.ForeColor = Color.Black;
                    row.Cells["Bookkeeping"].Style.SelectionBackColor = SystemColors.Highlight;
                }

                else
                {
                    row.Cells["Bookkeeping"].Style.ForeColor = Color.Red;
                    row.Cells["Bookkeeping"].Style.SelectionBackColor = Color.Red;
                }
            }
            #endregion

        }
        #endregion

        #region Bottom Buttons Default
        void BPBDefault()
        {
            if ((btnBPMonthly.BackColor == Color.LimeGreen && dgvBPMonthly.RowCount == 0)
                || (btnBPQuarterly.BackColor == Color.LimeGreen && dgvBPQuarterly.RowCount == 0)
                || (btnBPAnnually.BackColor == Color.LimeGreen && dgvBPAnnually.RowCount == 0))
            {
                btnBPRemarks.Enabled = false;
                btnBPDBill.Enabled = false;
                btnBPPBills.Enabled = false;
                btnBPReport.Enabled = false;
            }

            else
            {
                btnBPRemarks.Enabled = true;
                btnBPDBill.Enabled = true;
                btnBPPBills.Enabled = true;
                btnBPReport.Enabled = true;
            }
        }
        #endregion

        #region Search

        #region Search Button
        private void btnBPNSearch_MouseEnter(object sender, EventArgs e)
        {
            btnBPNSearch.Visible = false;
            btnBPHSearch.Visible = true;
        }

        private void btnBPHSearch_MouseLeave(object sender, EventArgs e)
        {
            btnBPNSearch.Visible = true;
            btnBPHSearch.Visible = false;
        }

        private void btnBPHSearch_MouseDown(object sender, MouseEventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;

            btnBPHSearch.Visible = false;
            btnBPDSearch.Visible = true;

            BPTSearch();

            cmbBPSOption_SelectedIndexChanged(sender, e);

            Cursor.Current = Cursors.Default;
        }

        private void btnBPDSearch_MouseUp(object sender, MouseEventArgs e)
        {
            btnBPHSearch.Visible = true;
            btnBPDSearch.Visible = false;
        }

        private void btnBPDSearch_MouseLeave(object sender, EventArgs e)
        {
            btnBPHSearch.Visible = true;
            btnBPDSearch.Visible = false;
        }
        #endregion

        #region Search Shortcut Key
        private void txtBPSearch_KeyDown(object sender, KeyEventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;

            if (txtBPSearch.Focused && e.KeyCode == Keys.Enter)
            {
                BPTSearch();

                cmbBPSOption_SelectedIndexChanged(sender, e);
            }

            Cursor.Current = Cursors.Default;
        }
        #endregion

        #region Search On or Off Focus
        private void txtBPSearch_Enter(object sender, EventArgs e)
        {
            if (txtBPSearch.ForeColor == Color.DarkGray)
            {
                txtBPSearch.Clear();
                txtBPSearch.ForeColor = Color.Black;
            }
        }

        private void txtBPSearch_Leave(object sender, EventArgs e)
        {
            if (txtBPSearch.ForeColor == Color.Black && txtBPSearch.Text == "")
            {
                txtBPSearch.Text = "Search";
                txtBPSearch.ForeColor = Color.DarkGray;
            }
        }
        #endregion

        #region Table Search
        void BPTSearch()
        {
            if (txtBPSearch.Text == "" || txtBPSearch.ForeColor == Color.DarkGray) { }

            else if (txtBPSearch.Text.Equals("All", StringComparison.InvariantCultureIgnoreCase) && cmbBPSOption.Text == "All")
            {
                ibpmpage = 1;
                ibpmp = 0;

                ibpqpage = 1;
                ibpqp = 0;

                ibpapage = 1;
                ibpap = 0;

                sbpsearch = "";
                sbpfsearch = "";

                BPFOption();
                BPTRestrict();

                BPMLoad(ibpmp);
                BPQLoad(ibpqp);
                BPALoad(ibpap);

                BPMRCDefault();
                BPQRCDefault();
                BPARCDefault();

                BPBDefault();
            }

            else
            {
                ibpmpage = 1;
                ibpmp = 0;

                ibpqpage = 1;
                ibpqp = 0;

                ibpapage = 1;
                ibpap = 0;

                BPSOption();

                BPFOption();

                BPTRestrict();

                BPMLoad(ibpmp);
                BPQLoad(ibpqp);
                BPALoad(ibpap);

                BPMRCDefault();
                BPQRCDefault();
                BPARCDefault();

                BPBDefault();
            }

            EDButtons();
        }
        #endregion

        #region Search Option
        private void cmbBPSOption_DropDown(object sender, EventArgs e)
        {
            cmbBPSOption.Items.Remove("Search Option");
            cmbBPSOption.ForeColor = Color.Black;
        }

        private void cmbBPSOption_DropDownClosed(object sender, EventArgs e)
        {
            if (cmbBPSOption.SelectedItem == null)
            {
                if (!cmbBPSOption.Items.Contains("Search Option"))
                {
                    cmbBPSOption.Items.Insert(0, "Search Option");
                }

                cmbBPSOption.Text = "Search Option";
                cmbBPSOption.ForeColor = Color.DarkGray;
            }
        }

        private void cmbBPSOption_SelectedIndexChanged(object sender, EventArgs e)
        {
            BPSearch();

            dgvBPMonthly.Columns[7].Visible = false;
            dgvBPMonthly.Columns[8].Visible = false;
            dgvBPMonthly.Columns[9].Visible = false;
            dgvBPMonthly.Columns[10].Visible = false;
            dgvBPMonthly.Columns[11].Visible = false;
            dgvBPMonthly.Columns[12].Visible = false;
            dgvBPMonthly.Columns[13].Visible = false;
            dgvBPMonthly.Columns[14].Visible = false;

            dgvBPQuarterly.Columns[7].Visible = false;
            dgvBPQuarterly.Columns[8].Visible = false;
            dgvBPQuarterly.Columns[9].Visible = false;
            dgvBPQuarterly.Columns[10].Visible = false;
            dgvBPQuarterly.Columns[11].Visible = false;
            dgvBPQuarterly.Columns[12].Visible = false;
            dgvBPQuarterly.Columns[13].Visible = false;
            dgvBPQuarterly.Columns[14].Visible = false;

            dgvBPAnnually.Columns[7].Visible = false;
            dgvBPAnnually.Columns[8].Visible = false;
            dgvBPAnnually.Columns[9].Visible = false;
            dgvBPAnnually.Columns[10].Visible = false;
            dgvBPAnnually.Columns[11].Visible = false;
            dgvBPAnnually.Columns[12].Visible = false;
            dgvBPAnnually.Columns[13].Visible = false;
            dgvBPAnnually.Columns[14].Visible = false;

            if (cmbBPSOption.Text == "Nature of Business")
            {
                dgvBPMonthly.Columns[7].Visible = true;
                dgvBPQuarterly.Columns[7].Visible = true;
                dgvBPAnnually.Columns[7].Visible = true;
            }

            else if (cmbBPSOption.Text == "Business / Owner TIN")
            {
                dgvBPMonthly.Columns[8].Visible = true;
                dgvBPQuarterly.Columns[8].Visible = true;
                dgvBPAnnually.Columns[8].Visible = true;
            }

            else if (cmbBPSOption.Text == "Tax Type")
            {
                dgvBPMonthly.Columns[9].Visible = true;
                dgvBPQuarterly.Columns[9].Visible = true;
                dgvBPAnnually.Columns[9].Visible = true;
            }

            else if (cmbBPSOption.Text == "SEC Reg. Number")
            {
                dgvBPMonthly.Columns[10].Visible = true;
                dgvBPQuarterly.Columns[10].Visible = true;
                dgvBPAnnually.Columns[10].Visible = true;
            }

            else if (cmbBPSOption.Text == "Group Name")
            {
                dgvBPMonthly.Columns[11].Visible = true;
                dgvBPQuarterly.Columns[11].Visible = true;
                dgvBPAnnually.Columns[11].Visible = true;
            }

            else if (cmbBPSOption.Text == "Sub-group Name")
            {
                dgvBPMonthly.Columns[12].Visible = true;
                dgvBPQuarterly.Columns[12].Visible = true;
                dgvBPAnnually.Columns[12].Visible = true;
            }

            else if (cmbBPSOption.Text == "Group Year")
            {
                dgvBPMonthly.Columns[13].Visible = true;
                dgvBPQuarterly.Columns[13].Visible = true;
                dgvBPAnnually.Columns[13].Visible = true;
            }

            else if (cmbBPSOption.Text == "Status")
            {
                dgvBPMonthly.Columns[14].Visible = true;
                dgvBPQuarterly.Columns[14].Visible = true;
                dgvBPAnnually.Columns[14].Visible = true;
            }
        }

        void BPSOption()
        {
            sbps = txtBPSearch.Text.Replace("'", "''");

            if (cmbBPSOption.Text == "Client Name")
            {
                sbpsearch = " WHERE CONCAT(c.Last_Name, ', ', c.First_Name, ' ', c.Middle_Initial) LIKE '" + sbps + "'";
                sbpfsearch = "{Bills.Client Name} = '" + sbps + "'";
            }

            else if (cmbBPSOption.Text == "Business Name")
            {
                sbpsearch = " WHERE c.Business_Name LIKE '" + sbps + "'";
                sbpfsearch = "{Bills.Business Name} = '" + sbps + "'";
            }

            else if (cmbBPSOption.Text == "Nature of Business")
            {
                sbpsearch = " WHERE c.Nature_of_Business LIKE '" + sbps + "'";
                sbpfsearch = "{Bills.Nature of Business} = '" + sbps + "'";
            }

            else if (cmbBPSOption.Text == "Business / Owner TIN")
            {
                sbpsearch = " WHERE c.Business_or_Owner_TIN LIKE '" + sbps + "'";
                sbpfsearch = "{Bills.Business / Owner TIN} = '" + sbps + "'";
            }

            else if (cmbBPSOption.Text == "Tax Type")
            {
                sbpsearch = " WHERE c.Tax_Type LIKE '" + sbps + "'";
                sbpfsearch = "{Bills.Tax Type} = '" + sbps + "'";
            }

            else if (cmbBPSOption.Text == "SEC Reg. Number")
            {
                sbpsearch = " WHERE c.SEC_Registration_Number LIKE '" + sbps + "'";
                sbpfsearch = "{Bills.SEC Reg. Number} = '" + sbps + "'";
            }

            else if (cmbBPSOption.Text == "Group Name")
            {
                sbpsearch = " WHERE c.Group_Name LIKE '" + sbps + "'";
                sbpfsearch = "{Bills.Group Name} = '" + sbps + "'";
            }

            else if (cmbBPSOption.Text == "Sub-group Name")
            {
                sbpsearch = " WHERE c.Sub_Group_Name LIKE '" + sbps + "'";
                sbpfsearch = "{Bills.Sub-group Name} = '" + sbps + "'";
            }

            else if (cmbBPSOption.Text == "Group Year")
            {
                sbpsearch = " WHERE c.Group_Year LIKE '" + sbps + "'";
                sbpfsearch = "{Bills.Group Year} = '" + sbps + "'";
            }

            else if (cmbBPSOption.Text == "Status")
            {
                sbpsearch = " WHERE c.Status LIKE '" + sbps + "'";
                sbpfsearch = "{Bills.Status} = '" + sbps + "'";
            }

            else
            {
                sbpsearch = " WHERE CONCAT('TRN-', s.No_ID) LIKE '" + sbps + "'";
                sbpfsearch = "{Bills.Transaction Number} = '" + sbps + "'";
            }
        }
        #endregion

        #region Order Option
        private void cmbBPOOption_DropDown(object sender, EventArgs e)
        {
            cmbBPOOption.Items.Remove("Order Option");
            cmbBPOOption.ForeColor = Color.Black;
        }

        private void cmbBPOOption_DropDownClosed(object sender, EventArgs e)
        {
            if (cmbBPOOption.SelectedItem == null)
            {
                if (!cmbBPOOption.Items.Contains("Order Option"))
                {
                    cmbBPOOption.Items.Insert(0, "Order Option");
                }

                cmbBPOOption.Text = "Order Option";
                cmbBPOOption.ForeColor = Color.DarkGray;
            }
        }

        private void cmbBPOOption_SelectedIndexChanged(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;

            if (cmbBPOOption.ForeColor != Color.DarkGray && cmbBPOOption.Text != "Order Option")
            {
                BPOOption();
                BPMLoad(ibpmp);
                BPQLoad(ibpqp);
                BPALoad(ibpap);
            }

            Cursor.Current = Cursors.Default;
        }

        void BPOOption()
        {
            if (cmbBPOOption.Text == "Client Name")
            {
                sbporder = " ORDER BY CONCAT(c.Last_Name, ', ', c.First_Name, ' ', c.Middle_Initial)";
                sbpfield = "Client Name";
            }

            else if (cmbBPOOption.Text == "Business Name")
            {
                sbporder = " ORDER BY c.Business_Name";
                sbpfield = "Business Name";
            }

            else if (cmbBPOOption.Text == "Group Name")
            {
                sbporder = " ORDER BY c.Group_Name";
                sbpfield = "Group Name";
            }

            else if (cmbBPOOption.Text == "Sub-group Name")
            {
                sbporder = " ORDER BY c.Sub_Group_Name";
                sbpfield = "Sub-group Name";
            }

            else if (cmbBPOOption.Text == "Group Year")
            {
                sbporder = " ORDER BY c.Group_Year";
                sbpfield = "Group Year";
            }

            else if (cmbBPOOption.Text == "Frequency")
            {
                sbporder = " ORDER BY DATE(Bill_Date)";
                sbpfield = "Sort Date";
            }

            else
            {
                sbporder = " ORDER BY s.No_ID";
                sbpfield = "No_ID";
            }
        }
        #endregion

        #region Frequency Option
        private void cmbBPFOption_DropDown(object sender, EventArgs e)
        {
            cmbBPFOption.Items.Remove("Frequency Option");
            cmbBPFOption.ForeColor = Color.Black;
        }

        private void cmbBPFOption_DropDownClosed(object sender, EventArgs e)
        {
            if (cmbBPFOption.SelectedItem == null)
            {
                if (!cmbBPFOption.Items.Contains("Frequency Option"))
                {
                    cmbBPFOption.Items.Insert(0, "Frequency Option");
                }

                cmbBPFOption.Text = "Frequency Option";
                cmbBPFOption.ForeColor = Color.DarkGray;
            }
        }

        private void cmbBPFOption_SelectedIndexChanged(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;

            BPFOption();
            BPTRestrict();

            ibpmpage = 1;
            ibpmp = 0;

            ibpqpage = 1;
            ibpqp = 0;

            ibpapage = 1;
            ibpap = 0;

            BPMLoad(ibpmp);
            BPQLoad(ibpqp);
            BPALoad(ibpap);

            BPMRCDefault();
            BPQRCDefault();
            BPARCDefault();

            BPBDefault();

            EDButtons();

            Cursor.Current = Cursors.Default;
        }

        private void dtpBPFrom_ValueChanged(object sender, EventArgs e)
        {
            cmbBPFOption_SelectedIndexChanged(sender, e);
        }

        private void dtpBPTo_ValueChanged(object sender, EventArgs e)
        {
            cmbBPFOption_SelectedIndexChanged(sender, e);
        }

        void BPFOption()
        {
            if (cmbBPFOption.Text == "Daily")
            {
                dtpBPFrom.Enabled = true;
                dtpBPTo.Enabled = true;

                dtpBPFrom.CustomFormat = "MM-dd-yyyy";
                dtpBPTo.CustomFormat = "MM-dd-yyyy";

                dtpBPFrom.ShowUpDown = false;
                dtpBPTo.ShowUpDown = false;

                DateTime dfrom, dto;

                dfrom = dtpBPFrom.Value;
                dto = dtpBPTo.Value;

                sbpdfrom = dfrom.ToString("yyyy-MM-dd");
                sbpdto = dto.ToString("yyyy-MM-dd");

                psBPFDate = dfrom.ToString("MM-dd-yyyy");
                psBPTDate = dto.ToString("MM-dd-yyyy");

                if (sbpsearch == "")
                {
                    sbpdrange = " WHERE DATE(Bill_Date) BETWEEN '" + sbpdfrom + "' AND '" + sbpdto + "'";
                }

                else
                {
                    sbpdrange = " AND DATE(Bill_Date) BETWEEN '" + sbpdfrom + "' AND '" + sbpdto + "'";
                }

                if (sbpfsearch == "")
                {
                    sbpfdrange = "datevalue({Bills.Bill Date}) >= datevalue('" + sbpdfrom + "') and datevalue({Bills.Bill Date}) <= datevalue('" + sbpdto + "')";
                }

                else
                {
                    sbpfdrange = " and datevalue({Bills.Bill Date}) >= datevalue('" + sbpdfrom + "') and datevalue({Bills.Bill Date}) <= datevalue('" + sbpdto + "')";
                }
            }

            else if (cmbBPFOption.Text == "Monthly")
            {
                dtpBPFrom.Enabled = true;
                dtpBPTo.Enabled = true;

                dtpBPFrom.CustomFormat = "MM-yyyy";
                dtpBPTo.CustomFormat = "MM-yyyy";

                dtpBPFrom.ShowUpDown = true;
                dtpBPTo.ShowUpDown = true;

                DateTime dfrom, dto;
                int m, y;

                dfrom = dtpBPFrom.Value;
                dto = dtpBPTo.Value;

                m = Convert.ToInt32(dto.ToString("MM"));
                y = Convert.ToInt32(dto.ToString("yyyy"));

                sbpdfrom = dfrom.ToString("yyyy-MM-01");
                sbpdto = dto.ToString("yyyy-MM-" + DateTime.DaysInMonth(y, m));

                psBPFDate = dfrom.ToString("MM-01-yyyy");
                psBPTDate = dto.ToString("MM-" + DateTime.DaysInMonth(y, m) + "-yyyy");

                if (sbpsearch == "")
                {
                    sbpdrange = " WHERE DATE(Bill_Date) BETWEEN '" + sbpdfrom + "' AND '" + sbpdto + "'";
                }

                else
                {
                    sbpdrange = " AND DATE(Bill_Date) BETWEEN '" + sbpdfrom + "' AND '" + sbpdto + "'";
                }

                if (sbpfsearch == "")
                {
                    sbpfdrange = "datevalue({Bills.Bill Date}) >= datevalue('" + sbpdfrom + "') and datevalue({Bills.Bill Date}) <= datevalue('" + sbpdto + "')";
                }

                else
                {
                    sbpfdrange = " and datevalue({Bills.Bill Date}) >= datevalue('" + sbpdfrom + "') and datevalue({Bills.Bill Date}) <= datevalue('" + sbpdto + "')";
                }
            }

            else if (cmbBPFOption.Text == "Annually")
            {
                dtpBPFrom.Enabled = true;
                dtpBPTo.Enabled = true;

                dtpBPFrom.CustomFormat = "yyyy";
                dtpBPTo.CustomFormat = "yyyy";

                dtpBPFrom.ShowUpDown = true;
                dtpBPTo.ShowUpDown = true;

                DateTime dfrom, dto;

                dfrom = dtpBPFrom.Value;
                dto = dtpBPTo.Value;

                sbpdfrom = dfrom.ToString("yyyy-01-01");
                sbpdto = dto.ToString("yyyy-12-31");

                psBPFDate = dfrom.ToString("01-01-yyyy");
                psBPTDate = dto.ToString("12-31-yyyy");

                if (sbpsearch == "")
                {
                    sbpdrange = " WHERE DATE(Bill_Date) BETWEEN '" + sbpdfrom + "' AND '" + sbpdto + "'";
                }

                else
                {
                    sbpdrange = " AND DATE(Bill_Date) BETWEEN '" + sbpdfrom + "' AND '" + sbpdto + "'";
                }

                if (sbpfsearch == "")
                {
                    sbpfdrange = "datevalue({Bills.Bill Date}) >= datevalue('" + sbpdfrom + "') and datevalue({Bills.Bill Date}) <= datevalue('" + sbpdto + "')";
                }

                else
                {
                    sbpfdrange = " and datevalue({Bills.Bill Date}) >= datevalue('" + sbpdfrom + "') and datevalue({Bills.Bill Date}) <= datevalue('" + sbpdto + "')";
                }
            }

            else
            {
                dtpBPFrom.Enabled = false;
                dtpBPTo.Enabled = false;

                dtpBPFrom.CustomFormat = "MM-dd-yyyy";
                dtpBPTo.CustomFormat = "MM-dd-yyyy";

                dtpBPFrom.ShowUpDown = false;
                dtpBPTo.ShowUpDown = false;

                sbpdrange = "";
                sbpfdrange = "";
            }
        }
        #endregion

        #region Auto Complete Search
        void BPSearch()
        {
            string tbl = "";

            if (btnBPMonthly.BackColor == Color.LimeGreen)
            {
                tbl = "tbl_mbillpay";
            }

            else if (btnBPQuarterly.BackColor == Color.LimeGreen)
            {
                tbl = "tbl_qbillpay";
            }

            else if (btnBPAnnually.BackColor == Color.LimeGreen)
            {
                tbl = "tbl_abillpay";
            }

            string uname = "";

            if (Login.psUType == "Staff")
            {
                uname = " WHERE c.Username = '" + Login.psUName + "'";
            }

            else
            {
                uname = "";
            }

            AutoCompleteStringCollection acs = new AutoCompleteStringCollection();

            string cQuery = "SELECT CONCAT('TRN-', s.No_ID), CONCAT(c.Last_Name, ', ', c.First_Name, ' ', c.Middle_Initial),"
                + " c.Business_Name, c.Nature_of_Business, c.Business_or_Owner_TIN, c.Tax_Type,"
                + " c.SEC_Registration_Number, c.Group_Name, c.Sub_Group_Name, c.Group_Year,"
                + " c.Status FROM " + tbl + " s LEFT JOIN tbl_clients c ON s.Client_ID = c.Client_ID" + uname + ";";
            MySqlConnection cConnection = new MySqlConnection(Conn.cString);
            MySqlCommand cCommand = new MySqlCommand(cQuery, cConnection);
            MySqlDataReader cReader;

            try
            {
                cConnection.Open();
                cReader = cCommand.ExecuteReader();

                acs.Clear();

                while (cReader.Read())
                {
                    string trn, cn, bn, nob, bot, tt, sec, gn, sgn, gy, st;

                    trn = cReader.GetString(0);
                    cn = cReader.GetString(1);
                    bn = cReader.GetString(2);
                    nob = cReader.GetString(3);
                    bot = cReader.GetString(4);
                    tt = cReader.GetString(5);
                    sec = cReader.GetString(6);
                    gn = cReader.GetString(7);
                    sgn = cReader.GetString(8);
                    gy = cReader.GetString(9);
                    st = cReader.GetString(10);

                    if (cmbBPSOption.Text == "All")
                    {
                        acs.Add("All");
                        acs.Add(trn);
                    }

                    else if (cmbBPSOption.Text == "Client Name")
                    {
                        acs.Add(cn);
                    }

                    else if (cmbBPSOption.Text == "Business Name")
                    {
                        acs.Add(bn);
                    }

                    else if (cmbBPSOption.Text == "Nature of Business")
                    {
                        acs.Add(nob);
                    }

                    else if (cmbBPSOption.Text == "Business / Owner TIN")
                    {
                        acs.Add(bot);
                    }

                    else if (cmbBPSOption.Text == "Tax Type")
                    {
                        acs.Add(tt);
                    }

                    else if (cmbBPSOption.Text == "SEC Reg. Number")
                    {
                        acs.Add(sec);
                    }

                    else if (cmbBPSOption.Text == "Group Name")
                    {
                        acs.Add(gn);
                    }

                    else if (cmbBPSOption.Text == "Sub-group Name")
                    {
                        acs.Add(sgn);
                    }

                    else if (cmbBPSOption.Text == "Group Year")
                    {
                        acs.Add(gy);
                    }

                    else if (cmbBPSOption.Text == "Status")
                    {
                        acs.Add(st);
                    }

                    else
                    {
                        acs.Add(trn);
                    }
                }
            }

            catch (MySqlException ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            finally
            {
                cConnection.Close();
            }

            txtBPSearch.AutoCompleteCustomSource = acs;
        }
        #endregion

        #region Enable Tooltip
        private void txtBPSearch_ForeColorChanged(object sender, EventArgs e)
        {
            TTip();
        }

        private void cmbBPSOption_ForeColorChanged(object sender, EventArgs e)
        {
            TTip();
        }

        private void cmbBPOOption_ForeColorChanged(object sender, EventArgs e)
        {
            TTip();
        }

        private void cmbBPFOption_ForeColorChanged(object sender, EventArgs e)
        {
            TTip();
        }
        #endregion

        #endregion

        #region Staff Table Restriction
        void BPTRestrict()
        {
            if (Login.psUType == "Staff")
            {
                if (sbpsearch == "" && sbpdrange == "")
                {
                    sbpuname = " WHERE c.Username = '" + Login.psUName + "'";
                }

                else
                {
                    sbpuname = " AND c.Username = '" + Login.psUName + "'";
                }
            }

            else
            {
                sbpuname = "";
            }
        }
        #endregion

        #region Print Bills
        private void btnBPPBills_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;

            string rpt = "";

            if (btnBPMonthly.BackColor == Color.LimeGreen)
            {
                rpt = "CRMBills.rpt";
            }

            else if (btnBPQuarterly.BackColor == Color.LimeGreen)
            {
                rpt = "CRQBills.rpt";
            }

            else if (btnBPAnnually.BackColor == Color.LimeGreen)
            {
                rpt = "CRABills.rpt";
            }

            string uname = "";

            if (Login.psUType == "Staff")
            {
                if (sbpfsearch == "" && sbpfdrange == "")
                {
                    uname = "{Bills.Username} = '" + Login.psUName + "'";
                }


                else
                {
                    uname = " and {Bills.Username} = '" + Login.psUName + "'";
                }
            }

            else
            {
                uname = "";
            }

            ReportDocument rd = new ReportDocument();
            rd.Load(Application.StartupPath + "\\CRBP\\" + rpt + "");

            string crf = sbpfsearch + sbpfdrange + uname + ";";
            crViewer.SelectionFormula = crf;

            FieldDefinition fd = rd.Database.Tables[0].Fields[sbpfield];
            rd.DataDefinition.SortFields[0].Field = fd;

            crViewer.ReportSource = rd;
            crViewer.Refresh();
            crViewer.RefreshReport();

            btnRBack.Visible = true;

            crViewer.Visible = true;
            tlpBP.Visible = false;

            Cursor.Current = Cursors.Default;
        }
        #endregion

        #region Report
        private void btnBPReport_Click(object sender, EventArgs e)
        {
            BReport w = new BReport();
            w.ShowDialog();
        }
        #endregion

        #region Services Drop Down
        string bpddstatus = "*Close";

        void BPDOpen()
        {
            if (bpddstatus == "*Open")
            {
                btnBPServices.BackColor = Color.LimeGreen;
                bbbilling = true;
                flpBPDDown.Visible = true;
            }
        }

        void BPDClose()
        {
            bbbilling = false;
            thide.Start();
        }

        private void btnBPServices_MouseEnter(object sender, EventArgs e)
        {
            if (bpddstatus == "*Open")
            {
                btnBPServices.Text = "   Services     ▼";
                bbbilling = true;
                flpBPDDown.Visible = true;
            }
        }

        private void btnBPServices_MouseLeave(object sender, EventArgs e)
        {
            bbbilling = false;
            thide.Start();
        }

        private void btnBPMonthly_MouseEnter(object sender, EventArgs e)
        {
            BPDOpen();
        }

        private void btnBPMonthly_MouseLeave(object sender, EventArgs e)
        {
            BPDClose();
        }

        private void btnBPQuarterly_MouseEnter(object sender, EventArgs e)
        {
            BPDOpen();
        }

        private void btnBPQuarterly_MouseLeave(object sender, EventArgs e)
        {
            BPDClose();
        }

        private void btnBPAnnually_MouseEnter(object sender, EventArgs e)
        {
            BPDOpen();
        }

        private void btnBPAnnually_MouseLeave(object sender, EventArgs e)
        {
            BPDClose();
        }
        #endregion

        #region Billing and Payments Services Navigation
        void BPCDefault()
        {
            dgvBPMonthly.Visible = false;
            dgvBPQuarterly.Visible = false;
            dgvBPAnnually.Visible = false;
        }

        void BPNBDefault()
        {
            btnBPMonthly.BackColor = Color.FromArgb(64, 64, 64); ;
            btnBPMonthly.FlatAppearance.MouseOverBackColor = Color.LimeGreen;
            btnBPMonthly.FlatAppearance.MouseDownBackColor = Color.ForestGreen;
            btnBPMonthly.Cursor = Cursors.Hand;

            btnBPQuarterly.BackColor = Color.FromArgb(64, 64, 64); ;
            btnBPQuarterly.FlatAppearance.MouseOverBackColor = Color.LimeGreen;
            btnBPQuarterly.FlatAppearance.MouseDownBackColor = Color.ForestGreen;
            btnBPQuarterly.Cursor = Cursors.Hand;

            btnBPAnnually.BackColor = Color.FromArgb(64, 64, 64); ;
            btnBPAnnually.FlatAppearance.MouseOverBackColor = Color.LimeGreen;
            btnBPAnnually.FlatAppearance.MouseDownBackColor = Color.ForestGreen;
            btnBPAnnually.Cursor = Cursors.Hand;
        }

        void BPNDefault()
        {
            flpBPM.Visible = false;
            flpBPQ.Visible = false;
            flpBPA.Visible = false;
        }

        private void btnBPServices_Click(object sender, EventArgs e)
        {
            if (bpddstatus == "*Close")
            {
                bpddstatus = "*Open";

                btnBPServices.Text = "   Services     ▼";
                bbbilling = true;
                flpBPDDown.Visible = true;
            }

            else
            {
                bpddstatus = "*Close";

                bbbilling = false;
                thide.Start();
            }
        }

        private void btnBPMonthly_Click(object sender, EventArgs e)
        {
            if (!dgvBPMonthly.Visible)
            {
                BPNBDefault();

                btnBPMonthly.BackColor = Color.LimeGreen;
                btnBPMonthly.FlatAppearance.MouseOverBackColor = Color.LimeGreen;
                btnBPMonthly.FlatAppearance.MouseDownBackColor = Color.LimeGreen;
                btnBPMonthly.Cursor = Cursors.Default;

                BPCDefault();

                dgvBPMonthly.Visible = true;
                BPNDefault();
                flpBPM.Visible = true;
                BPBDefault();
                BPGSRow();

                if (dgvBPMonthly.RowCount == 0)
                {
                    txtBPTR.Clear();
                    txtBPCID.Clear();
                    txtBPCName.Clear();
                    txtBPTPayment.Clear();
                    txtBPCPayment.Clear();
                    txtBPUPayment.Clear();
                }

                dtpBPPDate.Value = DateTime.Now;
                dtpBPPDate.CustomFormat = " ";
                CPayment();
                BPCServices();

                BPSearch();

                EDButtons();
            }

            btnBPServices.Focus();
            BPTColors();
        }

        private void btnBPQuarterly_Click(object sender, EventArgs e)
        {
            if (!dgvBPQuarterly.Visible)
            {
                BPNBDefault();

                btnBPQuarterly.BackColor = Color.LimeGreen;
                btnBPQuarterly.FlatAppearance.MouseOverBackColor = Color.LimeGreen;
                btnBPQuarterly.FlatAppearance.MouseDownBackColor = Color.LimeGreen;
                btnBPQuarterly.Cursor = Cursors.Default;

                BPCDefault();

                dgvBPQuarterly.Visible = true;
                BPNDefault();
                flpBPQ.Visible = true;
                BPBDefault();
                BPGSRow();

                if (dgvBPQuarterly.RowCount == 0)
                {
                    txtBPTR.Clear();
                    txtBPCID.Clear();
                    txtBPCName.Clear();
                    txtBPTPayment.Clear();
                    txtBPCPayment.Clear();
                    txtBPUPayment.Clear();
                }

                dtpBPPDate.Value = DateTime.Now;
                dtpBPPDate.CustomFormat = " ";
                CPayment();
                BPCServices();

                BPSearch();

                EDButtons();
            }

            btnBPServices.Focus();
            BPTColors();
        }

        private void btnBPAnnually_Click(object sender, EventArgs e)
        {
            if (!dgvBPAnnually.Visible)
            {
                BPNBDefault();

                btnBPAnnually.BackColor = Color.LimeGreen;
                btnBPAnnually.FlatAppearance.MouseOverBackColor = Color.LimeGreen;
                btnBPAnnually.FlatAppearance.MouseDownBackColor = Color.LimeGreen;
                btnBPAnnually.Cursor = Cursors.Default;

                BPCDefault();

                dgvBPAnnually.Visible = true;
                BPNDefault();
                flpBPA.Visible = true;
                BPBDefault();
                BPGSRow();

                if (dgvBPAnnually.RowCount == 0)
                {
                    txtBPTR.Clear();
                    txtBPCID.Clear();
                    txtBPCName.Clear();
                    txtBPTPayment.Clear();
                    txtBPCPayment.Clear();
                    txtBPUPayment.Clear();
                }

                dtpBPPDate.Value = DateTime.Now;
                dtpBPPDate.CustomFormat = " ";
                CPayment();
                BPCServices();

                BPSearch();

                EDButtons();
            }

            btnBPServices.Focus();
            BPTColors();
        }
        #endregion

        #region Billing and Payment Navigation
        private void btnBPS_Click(object sender, EventArgs e)
        {
            if (btnBPS.Text == "Billi&ng")
            {
                btnBPS.Text = "Payme&nts";

                flpBPC2.Visible = true;

                tlpBPContent.ColumnStyles[2].Width = 0;
            }

            else
            {
                btnBPS.Text = "Billi&ng";

                flpBPC2.Visible = false;

                tlpBPContent.ColumnStyles[2].Width = 395;
            }
        }
        #endregion

        #region Pagination

        #region Monthly Services
        void BPMPage()
        {
            dbpmpage = dbpmcount / ibpmdpage;
            dbpmtpage = Math.Truncate(dbpmpage);
            sbpmpage = Convert.ToString(dbpmpage);

            if (sbpmpage.Contains("."))
            {
                dbpmrpage = dbpmtpage + 1;
            }

            else
            {
                dbpmrpage = dbpmtpage;
            }
        }
        #endregion

        #region Quarterly Services
        void BPQPage()
        {
            dbpqpage = dbpqcount / ibpqdpage;
            dbpqtpage = Math.Truncate(dbpqpage);
            sbpqpage = Convert.ToString(dbpqpage);

            if (sbpqpage.Contains("."))
            {
                dbpqrpage = dbpqtpage + 1;
            }

            else
            {
                dbpqrpage = dbpqtpage;
            }
        }
        #endregion

        #region Annually Services
        void BPAPage()
        {
            dbpapage = dbpacount / ibpadpage;
            dbpatpage = Math.Truncate(dbpapage);
            sbpapage = Convert.ToString(dbpapage);

            if (sbpapage.Contains("."))
            {
                dbparpage = dbpatpage + 1;
            }

            else
            {
                dbparpage = dbpatpage;
            }
        }
        #endregion

        #endregion

        #region Row Count Default
        void BPMRCDefault()
        {
            BPMPage();

            if (ibpmpage == 1 && dbpmrpage <= 1)
            {
                btnBPMFirst.Enabled = false;
                btnBPMBack.Enabled = false;
                btnBPMNext.Enabled = false;
                btnBPMLast.Enabled = false;
            }

            else if (ibpmpage == 1 && dbpmrpage > 1)
            {
                btnBPMFirst.Enabled = false;
                btnBPMBack.Enabled = false;
                btnBPMNext.Enabled = true;
                btnBPMLast.Enabled = true;
            }

            else if (ibpmpage > 1 && ibpmpage < dbpmrpage)
            {
                btnBPMFirst.Enabled = true;
                btnBPMBack.Enabled = true;
                btnBPMNext.Enabled = true;
                btnBPMLast.Enabled = true;
            }

            else if (ibpmpage == dbpmrpage)
            {
                btnBPMFirst.Enabled = true;
                btnBPMBack.Enabled = true;
                btnBPMNext.Enabled = false;
                btnBPMLast.Enabled = false;
            }

            EDButtons();
        }

        void BPQRCDefault()
        {
            BPQPage();

            if (ibpqpage == 1 && dbpqrpage <= 1)
            {
                btnBPQFirst.Enabled = false;
                btnBPQBack.Enabled = false;
                btnBPQNext.Enabled = false;
                btnBPQLast.Enabled = false;
            }

            else if (ibpqpage == 1 && dbpqrpage > 1)
            {
                btnBPQFirst.Enabled = false;
                btnBPQBack.Enabled = false;
                btnBPQNext.Enabled = true;
                btnBPQLast.Enabled = true;
            }

            else if (ibpqpage > 1 && ibpqpage < dbpqrpage)
            {
                btnBPQFirst.Enabled = true;
                btnBPQBack.Enabled = true;
                btnBPQNext.Enabled = true;
                btnBPQLast.Enabled = true;
            }

            else if (ibpqpage == dbpqrpage)
            {
                btnBPQFirst.Enabled = true;
                btnBPQBack.Enabled = true;
                btnBPQNext.Enabled = false;
                btnBPQLast.Enabled = false;
            }

            EDButtons();
        }

        void BPARCDefault()
        {
            BPAPage();

            if (ibpapage == 1 && dbparpage <= 1)
            {
                btnBPAFirst.Enabled = false;
                btnBPABack.Enabled = false;
                btnBPANext.Enabled = false;
                btnBPALast.Enabled = false;
            }

            else if (ibpapage == 1 && dbparpage > 1)
            {
                btnBPAFirst.Enabled = false;
                btnBPABack.Enabled = false;
                btnBPANext.Enabled = true;
                btnBPALast.Enabled = true;
            }

            else if (ibpapage > 1 && ibpapage < dbparpage)
            {
                btnBPAFirst.Enabled = true;
                btnBPABack.Enabled = true;
                btnBPANext.Enabled = true;
                btnBPALast.Enabled = true;
            }

            else if (ibpapage == dbparpage)
            {
                btnBPAFirst.Enabled = true;
                btnBPABack.Enabled = true;
                btnBPANext.Enabled = false;
                btnBPALast.Enabled = false;
            }

            EDButtons();
        }
        #endregion

        #region Pagination Focus Control
        void BPPFC()
        {
            if (btnBPMonthly.BackColor == Color.LimeGreen)
            {
                dgvBPMonthly.Focus();
            }

            else if (btnBPQuarterly.BackColor == Color.LimeGreen)
            {
                dgvBPQuarterly.Focus();
            }

            else if (btnBPAnnually.BackColor == Color.LimeGreen)
            {
                dgvBPAnnually.Focus();
            }
        }
        #endregion

        #region First

        #region Monthly Services
        private void btnBPMFirst_Click(object sender, EventArgs e)
        {
            ibpmpage = 1;
            ibpmp = 0;

            BPMLoad(ibpmp);

            cmbBPSOption_SelectedIndexChanged(sender, e);

            BPMRCDefault();

            BPPFC();
        }
        #endregion

        #region Quarterly Services
        private void btnBPQFirst_Click(object sender, EventArgs e)
        {
            ibpqpage = 1;
            ibpqp = 0;

            BPQLoad(ibpqp);

            cmbBPSOption_SelectedIndexChanged(sender, e);

            BPQRCDefault();

            BPPFC();
        }
        #endregion

        #region Annually Services
        private void btnBPAFirst_Click(object sender, EventArgs e)
        {
            ibpapage = 1;
            ibpap = 0;

            BPALoad(ibpap);

            cmbBPSOption_SelectedIndexChanged(sender, e);

            BPARCDefault();

            BPPFC();
        }
        #endregion

        #endregion

        #region Back

        #region Monthly Services
        private void btnBPMBack_Click(object sender, EventArgs e)
        {
            BPMPage();

            ibpmpage--;

            if (ibpmpage == 1)
            {
                btnBPMFirst.Enabled = false;
                btnBPMBack.Enabled = false;
            }

            if (ibpmpage < dbpmrpage)
            {
                btnBPMNext.Enabled = true;
                btnBPMLast.Enabled = true;
            }

            ibpmp = ibpmp - ibpmdpage;
            BPMLoad(ibpmp);

            cmbBPSOption_SelectedIndexChanged(sender, e);

            EDButtons();

            BPPFC();
        }
        #endregion

        #region Quarterly Services
        private void btnBPQBack_Click(object sender, EventArgs e)
        {
            BPQPage();

            ibpqpage--;

            if (ibpqpage == 1)
            {
                btnBPQFirst.Enabled = false;
                btnBPQBack.Enabled = false;
            }

            if (ibpqpage < dbpqrpage)
            {
                btnBPQNext.Enabled = true;
                btnBPQLast.Enabled = true;
            }

            ibpqp = ibpqp - ibpqdpage;
            BPQLoad(ibpqp);

            cmbBPSOption_SelectedIndexChanged(sender, e);

            EDButtons();

            BPPFC();
        }
        #endregion

        #region Annually
        private void btnBPABack_Click(object sender, EventArgs e)
        {
            BPAPage();

            ibpapage--;

            if (ibpapage == 1)
            {
                btnBPAFirst.Enabled = false;
                btnBPABack.Enabled = false;
            }

            if (ibpapage < dbparpage)
            {
                btnBPANext.Enabled = true;
                btnBPALast.Enabled = true;
            }

            ibpap = ibpap - ibpadpage;
            BPALoad(ibpap);

            cmbBPSOption_SelectedIndexChanged(sender, e);

            EDButtons();

            BPPFC();
        }
        #endregion

        #endregion

        #region Next

        #region Monthly Services
        private void btnBPMNext_Click(object sender, EventArgs e)
        {
            BPMPage();

            ibpmpage++;

            if (ibpmpage == dbpmrpage)
            {
                btnBPMNext.Enabled = false;
                btnBPMLast.Enabled = false;
            }

            if (ibpmpage > 1)
            {
                btnBPMFirst.Enabled = true;
                btnBPMBack.Enabled = true;
            }

            ibpmp = ibpmp + ibpmdpage;
            BPMLoad(ibpmp);

            cmbBPSOption_SelectedIndexChanged(sender, e);

            EDButtons();

            BPPFC();
        }
        #endregion

        #region Quarterly Services
        private void btnBPQNext_Click(object sender, EventArgs e)
        {
            BPQPage();

            ibpqpage++;

            if (ibpqpage == dbpqrpage)
            {
                btnBPQNext.Enabled = false;
                btnBPQLast.Enabled = false;
            }

            if (ibpqpage > 1)
            {
                btnBPQFirst.Enabled = true;
                btnBPQBack.Enabled = true;
            }

            ibpqp = ibpqp + ibpqdpage;
            BPQLoad(ibpqp);

            cmbBPSOption_SelectedIndexChanged(sender, e);

            EDButtons();

            BPPFC();
        }
        #endregion

        #region Annually Services
        private void btnBPANext_Click(object sender, EventArgs e)
        {
            BPAPage();

            ibpapage++;

            if (ibpapage == dbparpage)
            {
                btnBPANext.Enabled = false;
                btnBPALast.Enabled = false;
            }

            if (ibpapage > 1)
            {
                btnBPAFirst.Enabled = true;
                btnBPABack.Enabled = true;
            }

            ibpap = ibpap + ibpadpage;
            BPALoad(ibpap);

            cmbBPSOption_SelectedIndexChanged(sender, e);

            EDButtons();

            BPPFC();
        }
        #endregion

        #endregion

        #region Last

        #region Mothly Services
        private void btnBPMLast_Click(object sender, EventArgs e)
        {
            BPMPage();

            ibpmp = Convert.ToInt32((dbpmrpage * ibpmdpage) - ibpmdpage);
            ibpmpage = Convert.ToInt32(dbpmrpage);

            BPMLoad(ibpmp);

            btnBPMFirst.Enabled = true;
            btnBPMBack.Enabled = true;
            btnBPMNext.Enabled = false;
            btnBPMLast.Enabled = false;

            cmbBPSOption_SelectedIndexChanged(sender, e);

            EDButtons();

            BPPFC();
        }
        #endregion

        #region Quarterly Services
        private void btnBPQLast_Click(object sender, EventArgs e)
        {
            BPQPage();

            ibpqp = Convert.ToInt32((dbpqrpage * ibpqdpage) - ibpqdpage);
            ibpqpage = Convert.ToInt32(dbpqrpage);

            BPQLoad(ibpqp);

            btnBPQFirst.Enabled = true;
            btnBPQBack.Enabled = true;
            btnBPQNext.Enabled = false;
            btnBPQLast.Enabled = false;

            cmbBPSOption_SelectedIndexChanged(sender, e);

            EDButtons();

            BPPFC();
        }
        #endregion

        #region Annually Services
        private void btnBPALast_Click(object sender, EventArgs e)
        {
            BPAPage();

            ibpap = Convert.ToInt32((dbparpage * ibpadpage) - ibpadpage);
            ibpapage = Convert.ToInt32(dbparpage);

            BPALoad(ibpap);

            btnBPAFirst.Enabled = true;
            btnBPABack.Enabled = true;
            btnBPANext.Enabled = false;
            btnBPALast.Enabled = false;

            cmbBPSOption_SelectedIndexChanged(sender, e);

            EDButtons();

            BPPFC();
        }
        #endregion

        #endregion

        #region Cell Enter
        DateTime bdate, pdate;
        int rcindex = 0, rnindex = 0;
        string chead, smrfee, smpfee, smsfee, smvat, smnvat, sm1601c, sm1601e, smser, smpher, smpier, smsee, smphee, smpiee, smcfee, smbk, smi;
        string sqrfee, sqpfee, sqsfee, sq1701q, sq1702q, sqbk, sqi;
        string sarfee, sapfee, sasfee, sa1701, sa1702, sa1604cf, sa1604e, saml, sacor, sabk, sai;

        private void dgvBP_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            DataGridView dgv = (DataGridView)sender;

            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgv.Rows[e.RowIndex];

                psBPNID = row.Cells["No_ID"].Value.ToString();

                txtBPTR.Text = row.Cells["Transaction Number"].Value.ToString();
                txtBPCID.Text = row.Cells["Client I.D."].Value.ToString();
                txtBPCName.Text = row.Cells["Client Name"].Value.ToString();
                txtBPTPayment.Text = string.Format("Php {0:0.00}", row.Cells["Total Payments"].Value);
                txtBPCPayment.Text = string.Format("Php {0:0.00}", row.Cells["Collected Payments"].Value);
                txtBPUPayment.Text = string.Format("Php {0:0.00}", row.Cells["Uncollected Payments"].Value);
                psRContent = row.Cells["Remarks"].Value.ToString();
                bdate = Convert.ToDateTime(row.Cells["Bill Date"].Value);
            }

            rcindex = dgv.CurrentRow.Index;

            if (rcindex != rnindex)
            {
                CPayment();
                BPTColors();
                BPCServices();
            }
        }

        private void dgv_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            BPTColors();
            CPayment();
            BPCServices();
        }

        void BPCServices()
        {
            chead = ""; smrfee = ""; smpfee = ""; smsfee = ""; smvat = ""; smnvat = ""; sm1601c = ""; sm1601e = ""; smser = ""; smpher = ""; smpier = ""; smsee = ""; smphee = ""; smpiee = ""; smcfee = ""; smbk = ""; smi = "";
            sqrfee = ""; sqpfee = ""; sqsfee = ""; sq1701q = ""; sq1702q = ""; sqbk = ""; sqi = "";
            sarfee = ""; sapfee = ""; sasfee = ""; sa1701 = ""; sa1702 = ""; sa1604cf = ""; sa1604e = ""; saml = ""; sacor = ""; sabk = ""; sai = "";
        }
        #endregion

        #region Cell Double Click

        private void dgvBP_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridView dgv = (DataGridView)sender;

            if (btnBPS.Text == "Payme&nts")
            {
                btnBPS_Click(sender, e);
            }

            DataGridViewCell cell = (DataGridViewCell)dgv.Rows[e.RowIndex].Cells[e.ColumnIndex];

            rnindex = dgv.CurrentRow.Index;

            if ((cell.ColumnIndex != dgv.Columns["Bill Date"].Index &&
                cell.ColumnIndex != dgv.Columns["Transaction Number"].Index &&
                cell.ColumnIndex != dgv.Columns["Client Name"].Index &&
                cell.ColumnIndex != dgv.Columns["Business Name"].Index &&
                cell.ColumnIndex != dgv.Columns["Address"].Index &&
                cell.ColumnIndex != dgv.Columns["Total Payments"].Index &&
                cell.ColumnIndex != dgv.Columns["Collected Payments"].Index &&
                cell.ColumnIndex != dgv.Columns["Uncollected Payments"].Index &&
                cell.ColumnIndex != dgv.Columns["Remarks"].Index) &&
                dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.ForeColor == Color.Red &&
                dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.SelectionBackColor != Color.Orange)
            {
                dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.SelectionBackColor = Color.Orange;

                double pm, sv;

                pm = Convert.ToDouble(txtBPPayment.Text.Replace("Php ", ""));
                sv = Convert.ToDouble(dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString().Replace("Php ", ""));

                pm = pm + sv;

                txtBPPayment.Text = string.Format("Php {0:0.00}", pm);

                chead = dgv.Columns[e.ColumnIndex].HeaderText;

                #region Monthly Services
                if (dgvBPMonthly.Visible)
                {
                    if (chead == "Retainers Fee")
                    {
                        smrfee = dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString().Replace("Php ", "");
                    }

                    else if (chead == "Professional Fee")
                    {
                        smpfee = dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString().Replace("Php ", "");
                    }

                    else if (chead == "Service Fee")
                    {
                        smsfee = dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString().Replace("Php ", "");
                    }

                    else if (chead == "VAT")
                    {
                        smvat = dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString().Replace("Php ", "");
                    }

                    else if (chead == "Non-VAT")
                    {
                        smnvat = dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString().Replace("Php ", "");
                    }

                    else if (chead == "1601C")
                    {
                        sm1601c = dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString().Replace("Php ", "");
                    }

                    else if (chead == "1601E")
                    {
                        sm1601e = dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString().Replace("Php ", "");
                    }

                    else if (chead == "SSS (ER)")
                    {
                        smser = dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString().Replace("Php ", "");
                    }

                    else if (chead == "PHIC (ER)")
                    {
                        smpher = dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString().Replace("Php ", "");
                    }

                    else if (chead == "Pag-IBIG (ER)")
                    {
                        smpier = dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString().Replace("Php ", "");
                    }

                    else if (chead == "SSS (EE)")
                    {
                        smsee = dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString().Replace("Php ", "");
                    }

                    else if (chead == "PHIC (EE)")
                    {
                        smphee = dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString().Replace("Php ", "");
                    }

                    else if (chead == "Pag-IBIG (EE)")
                    {
                        smpiee = dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString().Replace("Php ", "");
                    }

                    else if (chead == "Certification Fee")
                    {
                        smcfee = dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString().Replace("Php ", "");
                    }

                    else if (chead == "Bookkeeping")
                    {
                        smbk = dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString().Replace("Php ", "");
                    }

                    else if (chead == "Inventory")
                    {
                        smi = dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString().Replace("Php ", "");
                    }
                }
                #endregion

                #region Quarterly Services
                if (dgvBPQuarterly.Visible)
                {
                    if (chead == "Retainers Fee")
                    {
                        sqrfee = dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString().Replace("Php ", "");
                    }

                    else if (chead == "Professional Fee")
                    {
                        sqpfee = dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString().Replace("Php ", "");
                    }

                    else if (chead == "Service Fee")
                    {
                        sqsfee = dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString().Replace("Php ", "");
                    }

                    else if (chead == "1701Q")
                    {
                        sq1701q = dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString().Replace("Php ", "");
                    }

                    else if (chead == "1702Q")
                    {
                        sq1702q = dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString().Replace("Php ", "");
                    }

                    else if (chead == "Bookkeeping")
                    {
                        sqbk = dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString().Replace("Php ", "");
                    }

                    else if (chead == "Inventory")
                    {
                        sqi = dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString().Replace("Php ", "");
                    }
                }
                #endregion

                #region Annually Services
                if (dgvBPAnnually.Visible)
                {
                    if (chead == "Retainers Fee")
                    {
                        sarfee = dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString().Replace("Php ", "");
                    }

                    else if (chead == "Professional Fee")
                    {
                        sapfee = dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString().Replace("Php ", "");
                    }

                    else if (chead == "Service Fee")
                    {
                        sasfee = dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString().Replace("Php ", "");
                    }

                    else if (chead == "1701")
                    {
                        sa1701 = dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString().Replace("Php ", "");
                    }

                    else if (chead == "1702")
                    {
                        sa1702 = dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString().Replace("Php ", "");
                    }

                    else if (chead == "1604CF")
                    {
                        sa1604cf = dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString().Replace("Php ", "");
                    }

                    else if (chead == "1604E")
                    {
                        sa1604e = dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString().Replace("Php ", "");
                    }

                    else if (chead == "Municipal License")
                    {
                        saml = dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString().Replace("Php ", "");
                    }

                    else if (chead == "COR")
                    {
                        sacor = dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString().Replace("Php ", "");
                    }

                    else if (chead == "Bookkeeping")
                    {
                        sabk = dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString().Replace("Php ", "");
                    }

                    else if (chead == "Inventory")
                    {
                        sai = dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString().Replace("Php ", "");
                    }
                }
                #endregion
            }

            else if ((cell.ColumnIndex != dgv.Columns["Bill Date"].Index &&
                cell.ColumnIndex != dgv.Columns["Transaction Number"].Index &&
                cell.ColumnIndex != dgv.Columns["Client Name"].Index &&
                cell.ColumnIndex != dgv.Columns["Business Name"].Index &&
                cell.ColumnIndex != dgv.Columns["Address"].Index &&
                cell.ColumnIndex != dgv.Columns["Total Payments"].Index &&
                cell.ColumnIndex != dgv.Columns["Collected Payments"].Index &&
                cell.ColumnIndex != dgv.Columns["Uncollected Payments"].Index &&
                cell.ColumnIndex != dgv.Columns["Remarks"].Index) &&
                dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.SelectionBackColor == Color.Orange)
            {
                dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.SelectionBackColor = Color.Red;

                double pm, sv;

                pm = Convert.ToDouble(txtBPPayment.Text.Replace("Php ", ""));
                sv = Convert.ToDouble(dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString().Replace("Php ", ""));

                pm = pm - sv;

                txtBPPayment.Text = string.Format("Php {0:0.00}", pm);

                chead = dgv.Columns[e.ColumnIndex].HeaderText;

                #region Monthly Services
                if (dgvBPMonthly.Visible)
                {
                    if (chead == "Retainers Fee")
                    {
                        smrfee = "";
                    }

                    else if (chead == "Professional Fee")
                    {
                        smpfee = "";
                    }

                    else if (chead == "Service Fee")
                    {
                        smsfee = "";
                    }

                    else if (chead == "VAT")
                    {
                        smvat = "";
                    }

                    else if (chead == "Non-VAT")
                    {
                        smnvat = "";
                    }

                    else if (chead == "1601C")
                    {
                        sm1601c = "";
                    }

                    else if (chead == "1601E")
                    {
                        sm1601e = "";
                    }

                    else if (chead == "SSS (ER)")
                    {
                        smser = "";
                    }

                    else if (chead == "PHIC (ER)")
                    {
                        smpher = "";
                    }

                    else if (chead == "Pag-IBIG (ER)")
                    {
                        smpier = "";
                    }

                    else if (chead == "SSS (EE)")
                    {
                        smsee = "";
                    }

                    else if (chead == "PHIC (EE)")
                    {
                        smphee = "";
                    }

                    else if (chead == "Pag-IBIG (EE)")
                    {
                        smpiee = "";
                    }

                    else if (chead == "Certification Fee")
                    {
                        smcfee = "";
                    }

                    else if (chead == "Bookkeeping")
                    {
                        smbk = "";
                    }

                    else if (chead == "Inventory")
                    {
                        smi = "";
                    }
                }
                #endregion

                #region Quarterly Services
                if (dgvBPQuarterly.Visible)
                {
                    if (chead == "Retainers Fee")
                    {
                        sqrfee = "";
                    }

                    else if (chead == "Professional Fee")
                    {
                        sqpfee = "";
                    }

                    else if (chead == "Service Fee")
                    {
                        sqsfee = "";
                    }

                    else if (chead == "1701Q")
                    {
                        sq1701q = "";
                    }

                    else if (chead == "1702Q")
                    {
                        sq1702q = "";
                    }

                    else if (chead == "Bookkeeping")
                    {
                        sqbk = "";
                    }

                    else if (chead == "Inventory")
                    {
                        sqi = "";
                    }
                }
                #endregion

                #region Annually Services
                if (dgvBPAnnually.Visible)
                {
                    if (chead == "Retainers Fee")
                    {
                        sarfee = "";
                    }

                    else if (chead == "Professional Fee")
                    {
                        sapfee = "";
                    }

                    else if (chead == "Service Fee")
                    {
                        sasfee = "";
                    }

                    else if (chead == "1701")
                    {
                        sa1701 = "";
                    }

                    else if (chead == "1702")
                    {
                        sa1702 = "";
                    }

                    else if (chead == "1604CF")
                    {
                        sa1604cf = "";
                    }

                    else if (chead == "1604E")
                    {
                        sa1604e = "";
                    }

                    else if (chead == "Municipal License")
                    {
                        saml = "";
                    }

                    else if (chead == "COR")
                    {
                        sacor = "";
                    }

                    else if (chead == "Bookkeeping")
                    {
                        sabk = "";
                    }

                    else if (chead == "Inventory")
                    {
                        sai = "";
                    }
                }
                #endregion
            }
        }
        #endregion

        #region Remarks
        private void btnBPRemarks_Click(object sender, EventArgs e)
        {
            if (btnBPMonthly.BackColor == Color.LimeGreen)
            {
                Remarks.psFrequency = "*Monthly";
                Remarks.psNID = psBPNID;

                Remarks w = new Remarks();
                w.ShowDialog();
            }

            else if (btnBPQuarterly.BackColor == Color.LimeGreen)
            {
                Remarks.psFrequency = "*Quarterly";
                Remarks.psNID = psBPNID;

                Remarks w = new Remarks();
                w.ShowDialog();
            }

            else if (btnBPAnnually.BackColor == Color.LimeGreen)
            {
                Remarks.psFrequency = "*Annually";
                Remarks.psNID = psBPNID;

                Remarks w = new Remarks();
                w.ShowDialog();
            }
        }
        #endregion

        #region Delete Bill
        private void btnBPDBill_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;

            double z;

            if (btnBPMonthly.BackColor == Color.LimeGreen)
            {
                #region Monthly Services
                DialogResult dr = MessageBox.Show("Are you sure you want to delete this monthly bill?", "Delete Bill", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (dr == DialogResult.No) { }

                else
                {
                    if (Double.TryParse(txtBPCPayment.Text.Replace("Php ", ""), out z) && z != 0)
                    {
                        DialogResult sdr = MessageBox.Show("There are processed payment found in this monthly bill. Entire payment transaction will also be deleted if you click Ok to continue.", "Delete Bill Warning", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation);

                        if (sdr == DialogResult.Cancel) { }

                        else
                        {
                            string cQuery = "DELETE FROM tbl_mpaylogs WHERE Transaction_Number = '" + txtBPTR.Text + "';";
                            MySqlConnection cConnection = new MySqlConnection(Conn.cString);
                            MySqlCommand cCommand = new MySqlCommand(cQuery, cConnection);
                            MySqlDataReader cReader;

                            try
                            {
                                cConnection.Open();
                                cReader = cCommand.ExecuteReader();
                                while (cReader.Read()) { }
                            }

                            catch (MySqlException ex)
                            {
                                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }

                            finally
                            {
                                cConnection.Close();
                            }

                            string cQueryI = "DELETE FROM tbl_mbillpay WHERE No_ID = '" + psBPNID + "';";
                            MySqlConnection cConnectionI = new MySqlConnection(Conn.cString);
                            MySqlCommand cCommandI = new MySqlCommand(cQueryI, cConnectionI);
                            MySqlDataReader cReaderI;

                            try
                            {
                                cConnectionI.Open();
                                cReaderI = cCommandI.ExecuteReader();
                                while (cReaderI.Read()) { }

                                string nid = psBPNID;

                                BPMLoad(ibpmp);
                                BPSearch();

                                BPBDefault();
                                EDButtons();

                                cmbBPSOption_SelectedIndexChanged(sender, e);

                                MessageBox.Show("Monthly bill with transaction number TRN-" + nid + " has been deleted.", "Delete Bill", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }

                            catch (MySqlException ex)
                            {
                                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }

                            finally
                            {
                                cConnectionI.Close();
                            }
                        }
                    }

                    else
                    {
                        string cQuery = "DELETE FROM tbl_mbillpay WHERE No_ID = '" + psBPNID + "';";
                        MySqlConnection cConnection = new MySqlConnection(Conn.cString);
                        MySqlCommand cCommand = new MySqlCommand(cQuery, cConnection);
                        MySqlDataReader cReader;

                        try
                        {
                            cConnection.Open();
                            cReader = cCommand.ExecuteReader();
                            while (cReader.Read()) { }

                            string nid = psBPNID;

                            BPMLoad(ibpmp);
                            BPSearch();

                            BPBDefault();
                            EDButtons();

                            cmbBPSOption_SelectedIndexChanged(sender, e);

                            MessageBox.Show("Monthly bill with transaction number TRN-" + nid + " has been deleted.", "Delete Bill", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }

                        catch (MySqlException ex)
                        {
                            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }

                        finally
                        {
                            cConnection.Close();
                        }
                    }
                }
                #endregion
            }

            else if (btnBPQuarterly.BackColor == Color.LimeGreen)
            {
                #region Quarterly Services
                DialogResult dr = MessageBox.Show("Are you sure you want to delete this quarterly bill?", "Delete Bill", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (dr == DialogResult.No) { }

                else
                {

                    if (Double.TryParse(txtBPCPayment.Text.Replace("Php ", ""), out z) && z != 0)
                    {
                        DialogResult sdr = MessageBox.Show("There are processed payment found in this quarterly bill. Entire payment transaction will also be deleted if you click Ok to continue.", "Delete Bill Warning", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation);

                        if (sdr == DialogResult.Cancel) { }

                        else
                        {
                            string cQuery = "DELETE FROM tbl_qpaylogs WHERE Transaction_Number = '" + txtBPTR.Text + "';";
                            MySqlConnection cConnection = new MySqlConnection(Conn.cString);
                            MySqlCommand cCommand = new MySqlCommand(cQuery, cConnection);
                            MySqlDataReader cReader;

                            try
                            {
                                cConnection.Open();
                                cReader = cCommand.ExecuteReader();
                                while (cReader.Read()) { }
                            }

                            catch (MySqlException ex)
                            {
                                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }

                            finally
                            {
                                cConnection.Close();
                            }

                            string cQueryI = "DELETE FROM tbl_qbillpay WHERE No_ID = '" + psBPNID + "';";
                            MySqlConnection cConnectionI = new MySqlConnection(Conn.cString);
                            MySqlCommand cCommandI = new MySqlCommand(cQueryI, cConnectionI);
                            MySqlDataReader cReaderI;

                            try
                            {
                                cConnectionI.Open();
                                cReaderI = cCommandI.ExecuteReader();
                                while (cReaderI.Read()) { }

                                string nid = psBPNID;

                                BPQLoad(ibpqp);
                                BPSearch();

                                BPBDefault();
                                EDButtons();

                                cmbBPSOption_SelectedIndexChanged(sender, e);

                                MessageBox.Show("Quarterly bill with transaction number TRN-" + nid + " has been deleted.", "Delete Bill", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }

                            catch (MySqlException ex)
                            {
                                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }

                            finally
                            {
                                cConnectionI.Close();
                            }
                        }
                    }

                    else
                    {
                        string cQuery = "DELETE FROM tbl_qbillpay WHERE No_ID = '" + psBPNID + "';";
                        MySqlConnection cConnection = new MySqlConnection(Conn.cString);
                        MySqlCommand cCommand = new MySqlCommand(cQuery, cConnection);
                        MySqlDataReader cReader;

                        try
                        {
                            cConnection.Open();
                            cReader = cCommand.ExecuteReader();
                            while (cReader.Read()) { }

                            string nid = psBPNID;

                            BPQLoad(ibpqp);
                            BPSearch();

                            BPBDefault();
                            EDButtons();

                            cmbBPSOption_SelectedIndexChanged(sender, e);

                            MessageBox.Show("Quarterly bill with transaction number TRN-" + nid + " has been deleted.", "Delete Bill", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }

                        catch (MySqlException ex)
                        {
                            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }

                        finally
                        {
                            cConnection.Close();
                        }
                    }
                }
                #endregion
            }

            else if (btnBPAnnually.BackColor == Color.LimeGreen)
            {
                #region Annually Services
                DialogResult dr = MessageBox.Show("Are you sure you want to delete this annual bill?", "Delete Bill", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (dr == DialogResult.No) { }

                else
                {
                    if (Double.TryParse(txtBPCPayment.Text.Replace("Php ", ""), out z) && z != 0)
                    {
                        DialogResult sdr = MessageBox.Show("There are processed payment found in this annual bill. Entire payment transaction will also be deleted if you click Ok to continue.", "Delete Bill Warning", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation);

                        if (sdr == DialogResult.Cancel) { }

                        else
                        {
                            string cQuery = "DELETE FROM tbl_apaylogs WHERE Transaction_Number = '" + txtBPTR.Text + "';";
                            MySqlConnection cConnection = new MySqlConnection(Conn.cString);
                            MySqlCommand cCommand = new MySqlCommand(cQuery, cConnection);
                            MySqlDataReader cReader;

                            try
                            {
                                cConnection.Open();
                                cReader = cCommand.ExecuteReader();
                                while (cReader.Read()) { }
                            }

                            catch (MySqlException ex)
                            {
                                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }

                            finally
                            {
                                cConnection.Close();
                            }

                            string cQueryI = "DELETE FROM tbl_abillpay WHERE No_ID = '" + psBPNID + "';";
                            MySqlConnection cConnectionI = new MySqlConnection(Conn.cString);
                            MySqlCommand cCommandI = new MySqlCommand(cQueryI, cConnectionI);
                            MySqlDataReader cReaderI;

                            try
                            {
                                cConnectionI.Open();
                                cReaderI = cCommandI.ExecuteReader();
                                while (cReaderI.Read()) { }

                                string nid = psBPNID;

                                BPALoad(ibpap);
                                BPSearch();

                                BPBDefault();
                                EDButtons();

                                cmbBPSOption_SelectedIndexChanged(sender, e);

                                MessageBox.Show("Annual bill with transaction number TRN-" + nid + " has been deleted.", "Delete Bill", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }

                            catch (MySqlException ex)
                            {
                                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }

                            finally
                            {
                                cConnectionI.Close();
                            }
                        }
                    }

                    else
                    {
                        string cQuery = "DELETE FROM tbl_abillpay WHERE No_ID = '" + psBPNID + "';";
                        MySqlConnection cConnection = new MySqlConnection(Conn.cString);
                        MySqlCommand cCommand = new MySqlCommand(cQuery, cConnection);
                        MySqlDataReader cReader;

                        try
                        {
                            cConnection.Open();
                            cReader = cCommand.ExecuteReader();
                            while (cReader.Read()) { }

                            string nid = psBPNID;

                            BPALoad(ibpap);
                            BPSearch();

                            BPBDefault();
                            EDButtons();

                            cmbBPSOption_SelectedIndexChanged(sender, e);

                            MessageBox.Show("Annual bill with transaction number TRN-" + nid + " has been deleted.", "Delete Bill", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }

                        catch (MySqlException ex)
                        {
                            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }

                        finally
                        {
                            cConnection.Close();
                        }
                    }
                }
                #endregion
            }

            Cursor.Current = Cursors.Default;
        }
        #endregion

        #region Get Selected Row
        void BPGSRow()
        {
            if (btnBPMonthly.BackColor == Color.LimeGreen)
            {
                foreach (DataGridViewRow row in dgvBPMonthly.SelectedRows)
                {
                    psBPNID = row.Cells["No_ID"].Value.ToString();

                    txtBPTR.Text = row.Cells["Transaction Number"].Value.ToString();
                    txtBPCID.Text = row.Cells["Client I.D."].Value.ToString();
                    txtBPCName.Text = row.Cells["Client Name"].Value.ToString();
                    txtBPTPayment.Text = string.Format("Php {0:0.00}", row.Cells["Total Payments"].Value);
                    txtBPCPayment.Text = string.Format("Php {0:0.00}", row.Cells["Collected Payments"].Value);
                    txtBPUPayment.Text = string.Format("Php {0:0.00}", row.Cells["Uncollected Payments"].Value);
                    psRContent = row.Cells["Remarks"].Value.ToString();
                    bdate = Convert.ToDateTime(row.Cells["Bill Date"].Value);
                }
            }

            else if (btnBPQuarterly.BackColor == Color.LimeGreen)
            {
                foreach (DataGridViewRow row in dgvBPQuarterly.SelectedRows)
                {
                    psBPNID = row.Cells["No_ID"].Value.ToString();

                    txtBPTR.Text = row.Cells["Transaction Number"].Value.ToString();
                    txtBPCID.Text = row.Cells["Client I.D."].Value.ToString();
                    txtBPCName.Text = row.Cells["Client Name"].Value.ToString();
                    txtBPTPayment.Text = string.Format("Php {0:0.00}", row.Cells["Total Payments"].Value);
                    txtBPCPayment.Text = string.Format("Php {0:0.00}", row.Cells["Collected Payments"].Value);
                    txtBPUPayment.Text = string.Format("Php {0:0.00}", row.Cells["Uncollected Payments"].Value);
                    psRContent = row.Cells["Remarks"].Value.ToString();
                    bdate = Convert.ToDateTime(row.Cells["Bill Date"].Value);
                }
            }

            else if (btnBPAnnually.BackColor == Color.LimeGreen)
            {
                foreach (DataGridViewRow row in dgvBPAnnually.SelectedRows)
                {
                    psBPNID = row.Cells["No_ID"].Value.ToString();

                    txtBPTR.Text = row.Cells["Transaction Number"].Value.ToString();
                    txtBPCID.Text = row.Cells["Client I.D."].Value.ToString();
                    txtBPCName.Text = row.Cells["Client Name"].Value.ToString();
                    txtBPTPayment.Text = string.Format("Php {0:0.00}", row.Cells["Total Payments"].Value);
                    txtBPCPayment.Text = string.Format("Php {0:0.00}", row.Cells["Collected Payments"].Value);
                    txtBPUPayment.Text = string.Format("Php {0:0.00}", row.Cells["Uncollected Payments"].Value);
                    psRContent = row.Cells["Remarks"].Value.ToString();
                    bdate = Convert.ToDateTime(row.Cells["Bill Date"].Value);
                }
            }
        }
        #endregion

        #region Enable Payment Button
        private void txtBPTR_TextChanged(object sender, EventArgs e)
        {
            if (txtBPTR.Text != "")
            {
                btnBPPay.Enabled = true;
            }

            else
            {
                btnBPPay.Enabled = false;
            }

            EDButtons();
        }
        #endregion

        #region DatePicker ValueChanged
        private void dtpBPPDate_ValueChanged(object sender, EventArgs e)
        {
            dtpBPPDate.CustomFormat = "                MM-dd-yyyy";
        }
        #endregion

        #region Pay
        private void btnBPPay_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;

            double z, ucpay, cpay, tpay, pay, rpay;
            string bpd = "";

            ucpay = Convert.ToDouble(txtBPUPayment.Text.Replace("Php ", ""));
            cpay = Convert.ToDouble(txtBPCPayment.Text.Replace("Php ", ""));
            tpay = Convert.ToDouble(txtBPTPayment.Text.Replace("Php ", ""));


            if (tpay == 0)
            {
                MessageBox.Show("Payment can't be process. Php 0.00 total payment value.", "Zero Value", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            else if (ucpay == 0)
            {
                MessageBox.Show("Payment can't be process. This transaction is already paid.", "Paid", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtBPPayment.Focus();
            }

            else if (dtpBPPDate.CustomFormat == " ")
            {
                MessageBox.Show("Payment Date field is empty.", "Empty Field", MessageBoxButtons.OK, MessageBoxIcon.Error);
                dtpBPPDate.Focus();
            }

            else if (txtBPPayment.Text == "")
            {
                MessageBox.Show("Payment field is empty.", "Empty Field", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtBPPayment.Focus();
            }

            else if (Double.TryParse(txtBPPayment.Text.Replace("Php ", ""), out z) && z == 0)
            {
                MessageBox.Show("Php 0.00 payment value.", "Zero Value", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtBPPayment.Focus();
            }

            else
            {
                pay = Convert.ToDouble(txtBPPayment.Text.Replace("Php ", ""));
                pdate = dtpBPPDate.Value;

                if (bdate > pdate)
                {
                    MessageBox.Show("Payment Date is earlier than Bill Date.", "Earlier Payment Date", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                else if (pay > ucpay)
                {
                    MessageBox.Show("Payment value is greater than uncollected payment.", "Greater Value", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtBPPayment.Focus();
                }

                else
                {
                    if (dtpBPPDate.CustomFormat == "                MM-dd-yyyy")
                    {
                        DateTime bpdate = dtpBPPDate.Value;
                        bpd = bpdate.ToString("yyyy-MM-dd");
                    }

                    rpay = pay + cpay;

                    if (btnBPMonthly.BackColor == Color.LimeGreen)
                    {
                        #region Monthly Services
                        string cQueryI = "INSERT INTO tbl_mpaylogs(Transaction_Number, Payment_Date, Client_ID, Retainers_Fee_M,"
                            + " Professional_Fee_M, Service_Fee_M, VAT, Non_VAT, D1601C, D1601E, SSS_ER, PHIC_ER, Pag_IBIG_ER,"
                            + " SSS_EE, PHIC_EE, Pag_IBIG_EE, Certification_Fee, Bookkeeping_M, Inventory_M)"
                            + " VALUES('" + txtBPTR.Text + "', '" + bpd + "', '" + txtBPCID.Text + "', '" + smrfee + "', '" + smpfee + "',"
                            + " '" + smsfee + "', '" + smvat + "', '" + smnvat + "', '" + sm1601c + "', '" + sm1601e + "', '" + smser + "',"
                            + " '" + smpher + "', '" + smpier + "', '" + smsee + "', '" + smphee + "', '" + smpiee + "', '" + smcfee + "',"
                            + " '" + smbk + "', '" + smi + "');";
                        MySqlConnection cConnectionI = new MySqlConnection(Conn.cString);
                        MySqlCommand cCommandI = new MySqlCommand(cQueryI, cConnectionI);
                        MySqlDataReader cReaderI;

                        try
                        {
                            cConnectionI.Open();
                            cReaderI = cCommandI.ExecuteReader();
                            while (cReaderI.Read()) { }

                            string pmt, trn;

                            pmt = txtBPPayment.Text;
                            trn = txtBPTR.Text;

                            dtpBPPDate.Value = DateTime.Now;
                            dtpBPPDate.CustomFormat = " ";
                            txtBPPayment.Clear();

                            BPMLoad(ibpmp);

                            cmbBPSOption_SelectedIndexChanged(sender, e);

                            DataRow[] bpdrow = cDatasetBPM.Select("[Transaction Number] = '" + trn + "'");

                            if (bpdrow.Length > 0)
                            {
                                foreach (DataGridViewRow row in dgvBPMonthly.Rows)
                                {
                                    if (row.Cells[2].Value.ToString().Equals("" + trn + ""))
                                    {
                                        dgvBPMonthly.CurrentCell = dgvBPMonthly[2, row.Index];
                                    }
                                }
                            }

                            MessageBox.Show("New monthly payment " + pmt + " for Transaction Number " + trn + " has been saved.", "Payment", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }

                        catch (MySqlException ex)
                        {
                            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }

                        finally
                        {
                            cConnectionI.Close();
                        }
                        #endregion
                    }

                    else if (btnBPQuarterly.BackColor == Color.LimeGreen)
                    {
                        #region Quarterly Services
                        string cQueryI = "INSERT INTO tbl_qpaylogs(Transaction_Number, Payment_Date, Client_ID, Retainers_Fee_Q,"
                            + " Professional_Fee_Q, Service_Fee_Q, D1701Q, D1702Q, Bookkeeping_Q, Inventory_Q)"
                            + " VALUES('" + txtBPTR.Text + "', '" + bpd + "', '" + txtBPCID.Text + "', '" + sqrfee + "', '" + sqpfee + "',"
                            + " '" + sqsfee + "', '" + sq1701q + "', '" + sq1702q + "', '" + sqbk + "', '" + sqi + "');";
                        MySqlConnection cConnectionI = new MySqlConnection(Conn.cString);
                        MySqlCommand cCommandI = new MySqlCommand(cQueryI, cConnectionI);
                        MySqlDataReader cReaderI;

                        try
                        {
                            cConnectionI.Open();
                            cReaderI = cCommandI.ExecuteReader();
                            while (cReaderI.Read()) { }

                            string pmt, trn;

                            pmt = txtBPPayment.Text;
                            trn = txtBPTR.Text;

                            dtpBPPDate.Value = DateTime.Now;
                            dtpBPPDate.CustomFormat = " ";
                            txtBPPayment.Clear();

                            BPQLoad(ibpqp);

                            cmbBPSOption_SelectedIndexChanged(sender, e);

                            DataRow[] bpdrow = cDatasetBPQ.Select("[Transaction Number] = '" + trn + "'");

                            if (bpdrow.Length > 0)
                            {
                                foreach (DataGridViewRow row in dgvBPQuarterly.Rows)
                                {
                                    if (row.Cells[2].Value.ToString().Equals("" + trn + ""))
                                    {
                                        dgvBPQuarterly.CurrentCell = dgvBPQuarterly[2, row.Index];
                                    }
                                }
                            }

                            MessageBox.Show("New quarterly payment " + pmt + " for Transaction Number " + trn + " has been saved.", "Payment", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }

                        catch (MySqlException ex)
                        {
                            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }

                        finally
                        {
                            cConnectionI.Close();
                        }
                        #endregion
                    }

                    else if (btnBPAnnually.BackColor == Color.LimeGreen)
                    {
                        #region Annually Services
                        string cQueryI = "INSERT INTO tbl_apaylogs(Transaction_Number, Payment_Date, Client_ID, Retainers_Fee_A,"
                            + " Professional_Fee_A, Service_Fee_A, D1701, D1702, D1604CF, D1604E, Municipal_License, COR,"
                            + " Bookkeeping_A, Inventory_A)"
                            + " VALUES('" + txtBPTR.Text + "', '" + bpd + "', '" + txtBPCID.Text + "', '" + sarfee + "',"
                            + " '" + sapfee + "', '" + sasfee + "', '" + sa1701 + "', '" + sa1702 + "', '" + sa1604cf + "',"
                            + " '" + sa1604e + "', '" + saml + "', '" + sacor + "', '" + sabk + "', '" + sai + "');";
                        MySqlConnection cConnectionI = new MySqlConnection(Conn.cString);
                        MySqlCommand cCommandI = new MySqlCommand(cQueryI, cConnectionI);
                        MySqlDataReader cReaderI;

                        try
                        {
                            cConnectionI.Open();
                            cReaderI = cCommandI.ExecuteReader();
                            while (cReaderI.Read()) { }

                            string pmt, trn;

                            pmt = txtBPPayment.Text;
                            trn = txtBPTR.Text;

                            dtpBPPDate.Value = DateTime.Now;
                            dtpBPPDate.CustomFormat = " ";
                            txtBPPayment.Clear();

                            BPALoad(ibpap);

                            cmbBPSOption_SelectedIndexChanged(sender, e);

                            DataRow[] bpdrow = cDatasetBPA.Select("[Transaction Number] = '" + trn + "'");

                            if (bpdrow.Length > 0)
                            {
                                foreach (DataGridViewRow row in dgvBPAnnually.Rows)
                                {
                                    if (row.Cells[2].Value.ToString().Equals("" + trn + ""))
                                    {
                                        dgvBPAnnually.CurrentCell = dgvBPAnnually[2, row.Index];
                                    }
                                }
                            }

                            MessageBox.Show("New annual payment " + pmt + " for Transaction Number " + trn + " has been saved.", "Payment", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }

                        catch (MySqlException ex)
                        {
                            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }

                        finally
                        {
                            cConnectionI.Close();
                        }
                        #endregion
                    }
                }
            }

            Cursor.Current = Cursors.Default;
        }

        #endregion

        #region Refresh
        private void btnBPRefresh_Click(object sender, EventArgs e)
        {
            BPMLoad(ibpmp);
            BPQLoad(ibpqp);
            BPALoad(ibpap);

            BPMRCDefault();
            BPQRCDefault();
            BPARCDefault();

            cmbBPSOption_SelectedIndexChanged(sender, e);

            BPGSRow();

            BPBDefault();

            if (dgvBPMonthly.Visible && dgvBPMonthly.RowCount == 0)
            {
                txtBPTR.Clear();
                txtBPCID.Clear();
                txtBPCName.Clear();
                txtBPTPayment.Clear();
                txtBPCPayment.Clear();
                txtBPUPayment.Clear();
            }

            else if (dgvBPQuarterly.Visible && dgvBPQuarterly.RowCount == 0)
            {
                txtBPTR.Clear();
                txtBPCID.Clear();
                txtBPCName.Clear();
                txtBPTPayment.Clear();
                txtBPCPayment.Clear();
                txtBPUPayment.Clear();
            }

            else if (dgvBPAnnually.Visible && dgvBPAnnually.RowCount == 0)
            {
                txtBPTR.Clear();
                txtBPCID.Clear();
                txtBPCName.Clear();
                txtBPTPayment.Clear();
                txtBPCPayment.Clear();
                txtBPUPayment.Clear();
            }

            dtpBPPDate.Value = DateTime.Now;
            dtpBPPDate.CustomFormat = " ";

            EDButtons();
        }
        #endregion

        //----------------------------------------------------------------------// Payment Logs

        #region Load Table
        string splsearch = "", spls = "", splorder = "", spldrange = "", spldfrom = "", spldto = "", splmpage = "", splqpage = "", splapage = "", splfsearch = "", splfield = "", splfdrange = "";

        #region Monthly Services
        int iplmp = 0, iplmpage = 1, iplmdpage = tcontent;
        double dplmcount = 0, dplmpage = 0, dplmtpage = 0, dplmrpage = 0;

        void PLMLoad(int p)
        {
            string cQueryI = "SELECT COUNT(*) FROM tbl_mpaylogs" + splsearch + spldrange + ";";
            MySqlConnection cConnectionI = new MySqlConnection(Conn.cString);
            MySqlCommand cCommandI = new MySqlCommand(cQueryI, cConnectionI);
            MySqlDataReader cReaderI;

            try
            {
                cConnectionI.Open();
                cReaderI = cCommandI.ExecuteReader();
                while (cReaderI.Read())
                {
                    dplmcount = cReaderI.GetDouble(0);
                }
            }

            catch (MySqlException ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            finally
            {
                cConnectionI.Close();
            }

            MySqlConnection cConn = new MySqlConnection(Conn.cString);
            MySqlCommand cCommand = new MySqlCommand("SELECT s.No_ID,"
                + " DATE_FORMAT(DATE(Payment_Date), '%m-%d-%Y') AS 'Payment Date',"
                + " s.Transaction_Number AS 'Transaction Number',"
                + " s.Client_ID AS 'Client I.D.',"
                + " CONCAT(Last_Name, ', ', First_Name, ' ', Middle_Initial) AS 'Client Name',"
                + " c.Business_Name AS 'Business Name',"
                + " c.Address,"
                + " c.Nature_of_Business AS 'Nature of Business',"
                + " c.Business_or_Owner_TIN AS 'Business / Owner TIN',"
                + " c.Tax_Type AS 'Tax Type',"
                + " c.SEC_Registration_Number AS 'SEC Reg. Number',"
                + " c.Group_Name AS 'Group Name',"
                + " c.Sub_Group_Name AS 'Sub-group Name',"
                + " c.Group_Year AS 'Group Year',"
                + " c.Status,"
                + " IFNULL(CAST(s.Retainers_Fee_M AS DECIMAL(20, 2)), 0) AS 'Retainers Fee',"
                + " IFNULL(CAST(s.Professional_Fee_M AS DECIMAL(20, 2)), 0) AS 'Professional Fee',"
                + " IFNULL(CAST(s.Service_Fee_M AS DECIMAL(20, 2)), 0) AS 'Service Fee',"
                + " IFNULL(CAST(s.VAT AS DECIMAL(20, 2)), 0) AS 'VAT',"
                + " IFNULL(CAST(s.Non_VAT AS DECIMAL(20, 2)), 0) AS 'Non-VAT',"
                + " IFNULL(CAST(s.D1601C AS DECIMAL(20, 2)), 0) AS '1601C',"
                + " IFNULL(CAST(s.D1601E AS DECIMAL(20, 2)), 0) AS '1601E',"
                + " IFNULL(CAST(s.SSS_ER AS DECIMAL(20, 2)), 0) AS 'SSS (ER)',"
                + " IFNULL(CAST(s.PHIC_ER AS DECIMAL(20, 2)), 0) AS 'PHIC (ER)',"
                + " IFNULL(CAST(s.Pag_IBIG_ER AS DECIMAL(20, 2)), 0) AS 'Pag-IBIG (ER)',"
                + " IFNULL(CAST(s.SSS_EE AS DECIMAL(20, 2)), 0) AS 'SSS (EE)',"
                + " IFNULL(CAST(s.PHIC_EE AS DECIMAL(20, 2)), 0) AS 'PHIC (EE)',"
                + " IFNULL(CAST(s.Pag_IBIG_EE AS DECIMAL(20, 2)), 0) AS 'Pag-IBIG (EE)',"
                + " IFNULL(CAST(s.Certification_Fee AS DECIMAL(20, 2)), 0) AS 'Certification Fee',"
                + " IFNULL(CAST(s.Bookkeeping_M AS DECIMAL(20, 2)), 0) AS 'Bookkeeping',"
                + " IFNULL(CAST(s.Inventory_M AS DECIMAL(20, 2)), 0) AS 'Inventory',"

                + " IFNULL(CAST(s.Retainers_Fee_M AS DECIMAL(20, 2)), 0)"
                + " + IFNULL(CAST(s.Professional_Fee_M AS DECIMAL(20, 2)), 0)"
                + " + IFNULL(CAST(s.Service_Fee_M AS DECIMAL(20, 2)), 0)"
                + " + IFNULL(CAST(s.VAT AS DECIMAL(20, 2)), 0)"
                + " + IFNULL(CAST(s.Non_VAT AS DECIMAL(20, 2)), 0)"
                + " + IFNULL(CAST(s.D1601C AS DECIMAL(20, 2)), 0)"
                + " + IFNULL(CAST(s.D1601E AS DECIMAL(20, 2)), 0)"
                + " + IFNULL(CAST(s.SSS_ER AS DECIMAL(20, 2)), 0)"
                + " + IFNULL(CAST(s.PHIC_ER AS DECIMAL(20, 2)), 0)"
                + " + IFNULL(CAST(s.Pag_IBIG_ER AS DECIMAL(20, 2)), 0)"
                + " + IFNULL(CAST(s.SSS_EE AS DECIMAL(20, 2)), 0)"
                + " + IFNULL(CAST(s.PHIC_EE AS DECIMAL(20, 2)), 0)"
                + " + IFNULL(CAST(s.Pag_IBIG_EE AS DECIMAL(20, 2)), 0)"
                + " + IFNULL(CAST(s.Certification_Fee AS DECIMAL(20, 2)), 0)"
                + " + IFNULL(CAST(s.Bookkeeping_M AS DECIMAL(20, 2)), 0)"
                + " + IFNULL(CAST(s.Inventory_M AS DECIMAL(20, 2)), 0) AS 'Total Payments'"

                + " FROM tbl_mpaylogs s LEFT JOIN tbl_clients c ON s.Client_ID = c.Client_ID" + splsearch + spldrange + splorder + ";", cConn);

            try
            {
                MySqlDataAdapter cAdapter = new MySqlDataAdapter();
                cAdapter.SelectCommand = cCommand;
                cDatasetPLM = new DataTable();
                cAdapter.Fill(p, iplmdpage, cDatasetPLM);
                BindingSource cSource = new BindingSource();
                cSource.DataSource = cDatasetPLM;
                dgvPLMonthly.DataSource = cSource;
                cAdapter.Update(cDatasetPLM);
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            dgvPLMonthly.Columns[0].Visible = false;
            dgvPLMonthly.Columns[3].Visible = false;
            dgvPLMonthly.Columns[7].Visible = false;
            dgvPLMonthly.Columns[8].Visible = false;
            dgvPLMonthly.Columns[9].Visible = false;
            dgvPLMonthly.Columns[10].Visible = false;
            dgvPLMonthly.Columns[11].Visible = false;
            dgvPLMonthly.Columns[12].Visible = false;
            dgvPLMonthly.Columns[13].Visible = false;
            dgvPLMonthly.Columns[14].Visible = false;

            PLTColors();
        }
        #endregion

        #region Quarterly Services
        int iplqp = 0, iplqpage = 1, iplqdpage = tcontent;
        double dplqcount = 0, dplqpage = 0, dplqtpage = 0, dplqrpage = 0;

        void PLQLoad(int p)
        {
            string cQueryI = "SELECT COUNT(*) FROM tbl_qpaylogs" + splsearch + spldrange + ";";
            MySqlConnection cConnectionI = new MySqlConnection(Conn.cString);
            MySqlCommand cCommandI = new MySqlCommand(cQueryI, cConnectionI);
            MySqlDataReader cReaderI;

            try
            {
                cConnectionI.Open();
                cReaderI = cCommandI.ExecuteReader();
                while (cReaderI.Read())
                {
                    dplqcount = cReaderI.GetDouble(0);
                }
            }

            catch (MySqlException ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            finally
            {
                cConnectionI.Close();
            }

            MySqlConnection cConn = new MySqlConnection(Conn.cString);
            MySqlCommand cCommand = new MySqlCommand("SELECT s.No_ID,"
                + " DATE_FORMAT(DATE(Payment_Date), '%m-%d-%Y') AS 'Payment Date',"
                + " s.Transaction_Number AS 'Transaction Number',"
                + " s.Client_ID AS 'Client I.D.',"
                + " CONCAT(Last_Name, ', ', First_Name, ' ', Middle_Initial) AS 'Client Name',"
                + " c.Business_Name AS 'Business Name',"
                + " c.Address,"
                + " c.Nature_of_Business AS 'Nature of Business',"
                + " c.Business_or_Owner_TIN AS 'Business / Owner TIN',"
                + " c.Tax_Type AS 'Tax Type',"
                + " c.SEC_Registration_Number AS 'SEC Reg. Number',"
                + " c.Group_Name AS 'Group Name',"
                + " c.Sub_Group_Name AS 'Sub-group Name',"
                + " c.Group_Year AS 'Group Year',"
                + " c.Status,"
                + " IFNULL(CAST(s.Retainers_Fee_Q AS DECIMAL(20, 2)), 0) AS 'Retainers Fee',"
                + " IFNULL(CAST(s.Professional_Fee_Q AS DECIMAL(20, 2)), 0) AS 'Professional Fee',"
                + " IFNULL(CAST(s.Service_Fee_Q AS DECIMAL(20, 2)), 0) AS 'Service Fee',"
                + " IFNULL(CAST(s.D1701Q AS DECIMAL(20, 2)), 0) AS '1701Q',"
                + " IFNULL(CAST(s.D1702Q AS DECIMAL(20, 2)), 0) AS '1702Q',"
                + " IFNULL(CAST(s.Bookkeeping_Q AS DECIMAL(20, 2)), 0) AS 'Bookkeeping',"
                + " IFNULL(CAST(s.Inventory_Q AS DECIMAL(20, 2)), 0) AS 'Inventory',"

                + " IFNULL(CAST(s.Retainers_Fee_Q AS DECIMAL(20, 2)), 0)"
                + " + IFNULL(CAST(s.Professional_Fee_Q AS DECIMAL(20, 2)), 0)"
                + " + IFNULL(CAST(s.Service_Fee_Q AS DECIMAL(20, 2)), 0)"
                + " + IFNULL(CAST(s.D1701Q AS DECIMAL(20, 2)), 0)"
                + " + IFNULL(CAST(s.D1702Q AS DECIMAL(20, 2)), 0)"
                + " + IFNULL(CAST(s.Bookkeeping_Q AS DECIMAL(20, 2)), 0)"
                + " + IFNULL(CAST(s.Inventory_Q AS DECIMAL(20, 2)), 0) AS 'Total Payments'"
                + " FROM tbl_qpaylogs s LEFT JOIN tbl_clients c ON s.Client_ID = c.Client_ID" + splsearch + spldrange + splorder + ";", cConn);

            try
            {
                MySqlDataAdapter cAdapter = new MySqlDataAdapter();
                cAdapter.SelectCommand = cCommand;
                cDatasetPLQ = new DataTable();
                cAdapter.Fill(p, iplqdpage, cDatasetPLQ);
                BindingSource cSource = new BindingSource();
                cSource.DataSource = cDatasetPLQ;
                dgvPLQuarterly.DataSource = cSource;
                cAdapter.Update(cDatasetPLQ);
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            dgvPLQuarterly.Columns[0].Visible = false;
            dgvPLQuarterly.Columns[3].Visible = false;
            dgvPLQuarterly.Columns[7].Visible = false;
            dgvPLQuarterly.Columns[8].Visible = false;
            dgvPLQuarterly.Columns[9].Visible = false;
            dgvPLQuarterly.Columns[10].Visible = false;
            dgvPLQuarterly.Columns[11].Visible = false;
            dgvPLQuarterly.Columns[12].Visible = false;
            dgvPLQuarterly.Columns[13].Visible = false;
            dgvPLQuarterly.Columns[14].Visible = false;

            PLTColors();
        }
        #endregion

        #region Annually Services
        int iplap = 0, iplapage = 1, ipladpage = tcontent;
        double dplacount = 0, dplapage = 0, dplatpage = 0, dplarpage = 0;

        void PLALoad(int p)
        {
            string cQueryI = "SELECT COUNT(*) FROM tbl_apaylogs" + splsearch + spldrange + ";";
            MySqlConnection cConnectionI = new MySqlConnection(Conn.cString);
            MySqlCommand cCommandI = new MySqlCommand(cQueryI, cConnectionI);
            MySqlDataReader cReaderI;

            try
            {
                cConnectionI.Open();
                cReaderI = cCommandI.ExecuteReader();
                while (cReaderI.Read())
                {
                    dplacount = cReaderI.GetDouble(0);
                }
            }

            catch (MySqlException ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            finally
            {
                cConnectionI.Close();
            }

            MySqlConnection cConn = new MySqlConnection(Conn.cString);
            MySqlCommand cCommand = new MySqlCommand("SELECT s.No_ID, "
                + " DATE_FORMAT(DATE(Payment_Date), '%m-%d-%Y') AS 'Payment Date',"
                + " s.Transaction_Number AS 'Transaction Number',"
                + " s.Client_ID AS 'Client I.D.',"
                + " CONCAT(Last_Name, ', ', First_Name, ' ', Middle_Initial) AS 'Client Name',"
                + " c.Business_Name AS 'Business Name',"
                + " c.Address,"
                + " c.Nature_of_Business AS 'Nature of Business',"
                + " c.Business_or_Owner_TIN AS 'Business / Owner TIN',"
                + " c.Tax_Type AS 'Tax Type',"
                + " c.SEC_Registration_Number AS 'SEC Reg. Number',"
                + " c.Group_Name AS 'Group Name',"
                + " c.Sub_Group_Name AS 'Sub-group Name',"
                + " c.Group_Year AS 'Group Year',"
                + " c.Status,"
                + " IFNULL(CAST(s.Retainers_Fee_A AS DECIMAL(20, 2)), 0) AS 'Retainers Fee',"
                + " IFNULL(CAST(s.Professional_Fee_A AS DECIMAL(20, 2)), 0) AS 'Professional Fee',"
                + " IFNULL(CAST(s.Service_Fee_A AS DECIMAL(20, 2)), 0) AS 'Service Fee',"
                + " IFNULL(CAST(s.D1701 AS DECIMAL(20, 2)), 0) AS '1701',"
                + " IFNULL(CAST(s.D1702 AS DECIMAL(20, 2)), 0) AS '1702',"
                + " IFNULL(CAST(s.D1604CF AS DECIMAL(20, 2)), 0) AS '1604CF',"
                + " IFNULL(CAST(s.D1604E AS DECIMAL(20, 2)), 0) AS '1604E',"
                + " IFNULL(CAST(s.Municipal_License AS DECIMAL(20, 2)), 0) AS 'Municipal License',"
                + " IFNULL(CAST(s.COR AS DECIMAL(20, 2)), 0) AS 'COR',"
                + " IFNULL(CAST(s.Bookkeeping_A AS DECIMAL(20, 2)), 0) AS 'Bookkeeping',"
                + " IFNULL(CAST(s.Inventory_A AS DECIMAL(20, 2)), 0) AS 'Inventory',"

                + " IFNULL(CAST(s.Retainers_Fee_A AS DECIMAL(20, 2)), 0)"
                + " + IFNULL(CAST(s.Professional_Fee_A AS DECIMAL(20, 2)), 0)"
                + " + IFNULL(CAST(s.Service_Fee_A AS DECIMAL(20, 2)), 0)"
                + " + IFNULL(CAST(s.D1701 AS DECIMAL(20, 2)), 0)"
                + " + IFNULL(CAST(s.D1702 AS DECIMAL(20, 2)), 0)"
                + " + IFNULL(CAST(s.D1604CF AS DECIMAL(20, 2)), 0)"
                + " + IFNULL(CAST(s.D1604E AS DECIMAL(20, 2)), 0)"
                + " + IFNULL(CAST(s.Municipal_License AS DECIMAL(20, 2)), 0)"
                + " + IFNULL(CAST(s.COR AS DECIMAL(20, 2)), 0)"
                + " + IFNULL(CAST(s.Bookkeeping_A AS DECIMAL(20, 2)), 0)"
                + " + IFNULL(CAST(s.Inventory_A AS DECIMAL(20, 2)), 0) AS 'Total Payments'"
                + " FROM tbl_apaylogs s LEFT JOIN tbl_clients c ON s.Client_ID = c.Client_ID" + splsearch + spldrange + splorder + ";", cConn);

            try
            {
                MySqlDataAdapter cAdapter = new MySqlDataAdapter();
                cAdapter.SelectCommand = cCommand;
                cDatasetPLA = new DataTable();
                cAdapter.Fill(cDatasetPLA);
                BindingSource cSource = new BindingSource();
                cSource.DataSource = cDatasetPLA;
                dgvPLAnnually.DataSource = cSource;
                cAdapter.Update(cDatasetPLA);
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            dgvPLAnnually.Columns[0].Visible = false;
            dgvPLAnnually.Columns[3].Visible = false;
            dgvPLAnnually.Columns[7].Visible = false;
            dgvPLAnnually.Columns[8].Visible = false;
            dgvPLAnnually.Columns[9].Visible = false;
            dgvPLAnnually.Columns[10].Visible = false;
            dgvPLAnnually.Columns[11].Visible = false;
            dgvPLAnnually.Columns[12].Visible = false;
            dgvPLAnnually.Columns[13].Visible = false;
            dgvPLAnnually.Columns[14].Visible = false;

            PLTColors();
        }
        #endregion

        #endregion

        #region Table Colors
        void PLTColors()
        {
            double z = 0;

            #region Monthly Services
            foreach (DataGridViewRow row in dgvPLMonthly.Rows)
            {
                if (Double.TryParse(row.Cells["Retainers Fee"].Value.ToString().Replace("Php ", ""), out z) && z == 0)
                {
                    row.Cells["Retainers Fee"].Style.ForeColor = Color.Black;
                    row.Cells["Retainers Fee"].Style.SelectionBackColor = SystemColors.Highlight;
                }

                else
                {
                    row.Cells["Retainers Fee"].Style.ForeColor = Color.MediumBlue;
                    row.Cells["Retainers Fee"].Style.SelectionBackColor = Color.MediumBlue;
                }

                if (Double.TryParse(row.Cells["Professional Fee"].Value.ToString().Replace("Php ", ""), out z) && z == 0)
                {
                    row.Cells["Professional Fee"].Style.ForeColor = Color.Black;
                    row.Cells["Professional Fee"].Style.SelectionBackColor = SystemColors.Highlight;
                }

                else
                {
                    row.Cells["Professional Fee"].Style.ForeColor = Color.MediumBlue;
                    row.Cells["Professional Fee"].Style.SelectionBackColor = Color.MediumBlue;
                }

                if (Double.TryParse(row.Cells["Service Fee"].Value.ToString().Replace("Php ", ""), out z) && z == 0)
                {
                    row.Cells["Service Fee"].Style.ForeColor = Color.Black;
                    row.Cells["Service Fee"].Style.SelectionBackColor = SystemColors.Highlight;
                }

                else
                {
                    row.Cells["Service Fee"].Style.ForeColor = Color.MediumBlue;
                    row.Cells["Service Fee"].Style.SelectionBackColor = Color.MediumBlue;
                }

                if (Double.TryParse(row.Cells["VAT"].Value.ToString().Replace("Php ", ""), out z) && z == 0)
                {
                    row.Cells["VAT"].Style.ForeColor = Color.Black;
                    row.Cells["VAT"].Style.SelectionBackColor = SystemColors.Highlight;
                }

                else
                {
                    row.Cells["VAT"].Style.ForeColor = Color.MediumBlue;
                    row.Cells["VAT"].Style.SelectionBackColor = Color.MediumBlue;
                }

                if (Double.TryParse(row.Cells["Non-VAT"].Value.ToString().Replace("Php ", ""), out z) && z == 0)
                {
                    row.Cells["Non-VAT"].Style.ForeColor = Color.Black;
                    row.Cells["Non-VAT"].Style.SelectionBackColor = SystemColors.Highlight;
                }

                else
                {
                    row.Cells["Non-VAT"].Style.ForeColor = Color.MediumBlue;
                    row.Cells["Non-VAT"].Style.SelectionBackColor = Color.MediumBlue;
                }

                if (Double.TryParse(row.Cells["1601C"].Value.ToString().Replace("Php ", ""), out z) && z == 0)
                {
                    row.Cells["1601C"].Style.ForeColor = Color.Black;
                    row.Cells["1601C"].Style.SelectionBackColor = SystemColors.Highlight;
                }

                else
                {
                    row.Cells["1601C"].Style.ForeColor = Color.MediumBlue;
                    row.Cells["1601C"].Style.SelectionBackColor = Color.MediumBlue;
                }

                if (Double.TryParse(row.Cells["1601E"].Value.ToString().Replace("Php ", ""), out z) && z == 0)
                {
                    row.Cells["1601E"].Style.ForeColor = Color.Black;
                    row.Cells["1601E"].Style.SelectionBackColor = SystemColors.Highlight;
                }

                else
                {
                    row.Cells["1601E"].Style.ForeColor = Color.MediumBlue;
                    row.Cells["1601E"].Style.SelectionBackColor = Color.MediumBlue;
                }

                if (Double.TryParse(row.Cells["SSS (ER)"].Value.ToString().Replace("Php ", ""), out z) && z == 0)
                {
                    row.Cells["SSS (ER)"].Style.ForeColor = Color.Black;
                    row.Cells["SSS (ER)"].Style.SelectionBackColor = SystemColors.Highlight;
                }

                else
                {
                    row.Cells["SSS (ER)"].Style.ForeColor = Color.MediumBlue;
                    row.Cells["SSS (ER)"].Style.SelectionBackColor = Color.MediumBlue;
                }

                if (Double.TryParse(row.Cells["PHIC (ER)"].Value.ToString().Replace("Php ", ""), out z) && z == 0)
                {
                    row.Cells["PHIC (ER)"].Style.ForeColor = Color.Black;
                    row.Cells["PHIC (ER)"].Style.SelectionBackColor = SystemColors.Highlight;
                }

                else
                {
                    row.Cells["PHIC (ER)"].Style.ForeColor = Color.MediumBlue;
                    row.Cells["PHIC (ER)"].Style.SelectionBackColor = Color.MediumBlue;
                }

                if (Double.TryParse(row.Cells["Pag-IBIG (ER)"].Value.ToString().Replace("Php ", ""), out z) && z == 0)
                {
                    row.Cells["Pag-IBIG (ER)"].Style.ForeColor = Color.Black;
                    row.Cells["Pag-IBIG (ER)"].Style.SelectionBackColor = SystemColors.Highlight;
                }

                else
                {
                    row.Cells["Pag-IBIG (ER)"].Style.ForeColor = Color.MediumBlue;
                    row.Cells["Pag-IBIG (ER)"].Style.SelectionBackColor = Color.MediumBlue;
                }

                if (Double.TryParse(row.Cells["SSS (EE)"].Value.ToString().Replace("Php ", ""), out z) && z == 0)
                {
                    row.Cells["SSS (EE)"].Style.ForeColor = Color.Black;
                    row.Cells["SSS (EE)"].Style.SelectionBackColor = SystemColors.Highlight;
                }

                else
                {
                    row.Cells["SSS (EE)"].Style.ForeColor = Color.MediumBlue;
                    row.Cells["SSS (EE)"].Style.SelectionBackColor = Color.MediumBlue;
                }

                if (Double.TryParse(row.Cells["PHIC (EE)"].Value.ToString().Replace("Php ", ""), out z) && z == 0)
                {
                    row.Cells["PHIC (EE)"].Style.ForeColor = Color.Black;
                    row.Cells["PHIC (EE)"].Style.SelectionBackColor = SystemColors.Highlight;
                }

                else
                {
                    row.Cells["PHIC (EE)"].Style.ForeColor = Color.MediumBlue;
                    row.Cells["PHIC (EE)"].Style.SelectionBackColor = Color.MediumBlue;
                }

                if (Double.TryParse(row.Cells["Pag-IBIG (EE)"].Value.ToString().Replace("Php ", ""), out z) && z == 0)
                {
                    row.Cells["Pag-IBIG (EE)"].Style.ForeColor = Color.Black;
                    row.Cells["Pag-IBIG (EE)"].Style.SelectionBackColor = SystemColors.Highlight;
                }

                else
                {
                    row.Cells["Pag-IBIG (EE)"].Style.ForeColor = Color.MediumBlue;
                    row.Cells["Pag-IBIG (EE)"].Style.SelectionBackColor = Color.MediumBlue;
                }

                if (Double.TryParse(row.Cells["Certification Fee"].Value.ToString().Replace("Php ", ""), out z) && z == 0)
                {
                    row.Cells["Certification Fee"].Style.ForeColor = Color.Black;
                    row.Cells["Certification Fee"].Style.SelectionBackColor = SystemColors.Highlight;
                }

                else
                {
                    row.Cells["Certification Fee"].Style.ForeColor = Color.MediumBlue;
                    row.Cells["Certification Fee"].Style.SelectionBackColor = Color.MediumBlue;
                }

                if (Double.TryParse(row.Cells["Bookkeeping"].Value.ToString().Replace("Php ", ""), out z) && z == 0)
                {
                    row.Cells["Bookkeeping"].Style.ForeColor = Color.Black;
                    row.Cells["Bookkeeping"].Style.SelectionBackColor = SystemColors.Highlight;
                }

                else
                {
                    row.Cells["Bookkeeping"].Style.ForeColor = Color.MediumBlue;
                    row.Cells["Bookkeeping"].Style.SelectionBackColor = Color.MediumBlue;
                }

                if (Double.TryParse(row.Cells["Inventory"].Value.ToString().Replace("Php ", ""), out z) && z == 0)
                {
                    row.Cells["Inventory"].Style.ForeColor = Color.Black;
                    row.Cells["Inventory"].Style.SelectionBackColor = SystemColors.Highlight;
                }

                else
                {
                    row.Cells["Inventory"].Style.ForeColor = Color.MediumBlue;
                    row.Cells["Inventory"].Style.SelectionBackColor = Color.MediumBlue;
                }
            }
            #endregion

            #region Quarterly Services
            foreach (DataGridViewRow row in dgvPLQuarterly.Rows)
            {
                if (Double.TryParse(row.Cells["Retainers Fee"].Value.ToString().Replace("Php ", ""), out z) && z == 0)
                {
                    row.Cells["Retainers Fee"].Style.ForeColor = Color.Black;
                    row.Cells["Retainers Fee"].Style.SelectionBackColor = SystemColors.Highlight;
                }

                else
                {
                    row.Cells["Retainers Fee"].Style.ForeColor = Color.MediumBlue;
                    row.Cells["Retainers Fee"].Style.SelectionBackColor = Color.MediumBlue;
                }

                if (Double.TryParse(row.Cells["Professional Fee"].Value.ToString().Replace("Php ", ""), out z) && z == 0)
                {
                    row.Cells["Professional Fee"].Style.ForeColor = Color.Black;
                    row.Cells["Professional Fee"].Style.SelectionBackColor = SystemColors.Highlight;
                }

                else
                {
                    row.Cells["Professional Fee"].Style.ForeColor = Color.MediumBlue;
                    row.Cells["Professional Fee"].Style.SelectionBackColor = Color.MediumBlue;
                }

                if (Double.TryParse(row.Cells["Service Fee"].Value.ToString().Replace("Php ", ""), out z) && z == 0)
                {
                    row.Cells["Service Fee"].Style.ForeColor = Color.Black;
                    row.Cells["Service Fee"].Style.SelectionBackColor = SystemColors.Highlight;
                }

                else
                {
                    row.Cells["Service Fee"].Style.ForeColor = Color.MediumBlue;
                    row.Cells["Service Fee"].Style.SelectionBackColor = Color.MediumBlue;
                }



                if (Double.TryParse(row.Cells["1701Q"].Value.ToString().Replace("Php ", ""), out z) && z == 0)
                {
                    row.Cells["1701Q"].Style.ForeColor = Color.Black;
                    row.Cells["1701Q"].Style.SelectionBackColor = SystemColors.Highlight;
                }

                else
                {
                    row.Cells["1701Q"].Style.ForeColor = Color.MediumBlue;
                    row.Cells["1701Q"].Style.SelectionBackColor = Color.MediumBlue;
                }

                if (Double.TryParse(row.Cells["1702Q"].Value.ToString().Replace("Php ", ""), out z) && z == 0)
                {
                    row.Cells["1702Q"].Style.ForeColor = Color.Black;
                    row.Cells["1702Q"].Style.SelectionBackColor = SystemColors.Highlight;
                }

                else
                {
                    row.Cells["1702Q"].Style.ForeColor = Color.MediumBlue;
                    row.Cells["1702Q"].Style.SelectionBackColor = Color.MediumBlue;
                }

                if (Double.TryParse(row.Cells["Bookkeeping"].Value.ToString().Replace("Php ", ""), out z) && z == 0)
                {
                    row.Cells["Bookkeeping"].Style.ForeColor = Color.Black;
                    row.Cells["Bookkeeping"].Style.SelectionBackColor = SystemColors.Highlight;
                }

                else
                {
                    row.Cells["Bookkeeping"].Style.ForeColor = Color.MediumBlue;
                    row.Cells["Bookkeeping"].Style.SelectionBackColor = Color.MediumBlue;
                }

                if (Double.TryParse(row.Cells["Inventory"].Value.ToString().Replace("Php ", ""), out z) && z == 0)
                {
                    row.Cells["Inventory"].Style.ForeColor = Color.Black;
                    row.Cells["Inventory"].Style.SelectionBackColor = SystemColors.Highlight;
                }

                else
                {
                    row.Cells["Inventory"].Style.ForeColor = Color.MediumBlue;
                    row.Cells["Inventory"].Style.SelectionBackColor = Color.MediumBlue;
                }
            }
            #endregion

            #region Annually Services
            foreach (DataGridViewRow row in dgvPLAnnually.Rows)
            {
                if (Double.TryParse(row.Cells["Retainers Fee"].Value.ToString().Replace("Php ", ""), out z) && z == 0)
                {
                    row.Cells["Retainers Fee"].Style.ForeColor = Color.Black;
                    row.Cells["Retainers Fee"].Style.SelectionBackColor = SystemColors.Highlight;
                }

                else
                {
                    row.Cells["Retainers Fee"].Style.ForeColor = Color.MediumBlue;
                    row.Cells["Retainers Fee"].Style.SelectionBackColor = Color.MediumBlue;
                }

                if (Double.TryParse(row.Cells["Professional Fee"].Value.ToString().Replace("Php ", ""), out z) && z == 0)
                {
                    row.Cells["Professional Fee"].Style.ForeColor = Color.Black;
                    row.Cells["Professional Fee"].Style.SelectionBackColor = SystemColors.Highlight;
                }

                else
                {
                    row.Cells["Professional Fee"].Style.ForeColor = Color.MediumBlue;
                    row.Cells["Professional Fee"].Style.SelectionBackColor = Color.MediumBlue;
                }

                if (Double.TryParse(row.Cells["Service Fee"].Value.ToString().Replace("Php ", ""), out z) && z == 0)
                {
                    row.Cells["Service Fee"].Style.ForeColor = Color.Black;
                    row.Cells["Service Fee"].Style.SelectionBackColor = SystemColors.Highlight;
                }

                else
                {
                    row.Cells["Service Fee"].Style.ForeColor = Color.MediumBlue;
                    row.Cells["Service Fee"].Style.SelectionBackColor = Color.MediumBlue;
                }



                if (Double.TryParse(row.Cells["1701"].Value.ToString().Replace("Php ", ""), out z) && z == 0)
                {
                    row.Cells["1701"].Style.ForeColor = Color.Black;
                    row.Cells["1701"].Style.SelectionBackColor = SystemColors.Highlight;
                }

                else
                {
                    row.Cells["1701"].Style.ForeColor = Color.MediumBlue;
                    row.Cells["1701"].Style.SelectionBackColor = Color.MediumBlue;
                }

                if (Double.TryParse(row.Cells["1702"].Value.ToString().Replace("Php ", ""), out z) && z == 0)
                {
                    row.Cells["1702"].Style.ForeColor = Color.Black;
                    row.Cells["1702"].Style.SelectionBackColor = SystemColors.Highlight;
                }

                else
                {
                    row.Cells["1702"].Style.ForeColor = Color.MediumBlue;
                    row.Cells["1702"].Style.SelectionBackColor = Color.MediumBlue;
                }

                if (Double.TryParse(row.Cells["1604CF"].Value.ToString().Replace("Php ", ""), out z) && z == 0)
                {
                    row.Cells["1604CF"].Style.ForeColor = Color.Black;
                    row.Cells["1604CF"].Style.SelectionBackColor = SystemColors.Highlight;
                }

                else
                {
                    row.Cells["1604CF"].Style.ForeColor = Color.MediumBlue;
                    row.Cells["1604CF"].Style.SelectionBackColor = Color.MediumBlue;
                }

                if (Double.TryParse(row.Cells["1604E"].Value.ToString().Replace("Php ", ""), out z) && z == 0)
                {
                    row.Cells["1604E"].Style.ForeColor = Color.Black;
                    row.Cells["1604E"].Style.SelectionBackColor = SystemColors.Highlight;
                }

                else
                {
                    row.Cells["1604E"].Style.ForeColor = Color.MediumBlue;
                    row.Cells["1604E"].Style.SelectionBackColor = Color.MediumBlue;
                }

                if (Double.TryParse(row.Cells["Municipal License"].Value.ToString().Replace("Php ", ""), out z) && z == 0)
                {
                    row.Cells["Municipal License"].Style.ForeColor = Color.Black;
                    row.Cells["Municipal License"].Style.SelectionBackColor = SystemColors.Highlight;
                }

                else
                {
                    row.Cells["Municipal License"].Style.ForeColor = Color.MediumBlue;
                    row.Cells["Municipal License"].Style.SelectionBackColor = Color.MediumBlue;
                }

                if (Double.TryParse(row.Cells["COR"].Value.ToString().Replace("Php ", ""), out z) && z == 0)
                {
                    row.Cells["COR"].Style.ForeColor = Color.Black;
                    row.Cells["COR"].Style.SelectionBackColor = SystemColors.Highlight;
                }

                else
                {
                    row.Cells["COR"].Style.ForeColor = Color.MediumBlue;
                    row.Cells["COR"].Style.SelectionBackColor = Color.MediumBlue;
                }

                if (Double.TryParse(row.Cells["Bookkeeping"].Value.ToString().Replace("Php ", ""), out z) && z == 0)
                {
                    row.Cells["Bookkeeping"].Style.ForeColor = Color.Black;
                    row.Cells["Bookkeeping"].Style.SelectionBackColor = SystemColors.Highlight;
                }

                else
                {
                    row.Cells["Bookkeeping"].Style.ForeColor = Color.MediumBlue;
                    row.Cells["Bookkeeping"].Style.SelectionBackColor = Color.MediumBlue;
                }

                if (Double.TryParse(row.Cells["Inventory"].Value.ToString().Replace("Php ", ""), out z) && z == 0)
                {
                    row.Cells["Inventory"].Style.ForeColor = Color.Black;
                    row.Cells["Inventory"].Style.SelectionBackColor = SystemColors.Highlight;
                }

                else
                {
                    row.Cells["Inventory"].Style.ForeColor = Color.MediumBlue;
                    row.Cells["Inventory"].Style.SelectionBackColor = Color.MediumBlue;
                }
            }
            #endregion
        }
        #endregion

        #region Bottom Buttons Default
        void PLBDefault()
        {
            if ((btnPLMonthly.BackColor == Color.LimeGreen && dgvPLMonthly.RowCount == 0)
                || (btnPLQuarterly.BackColor == Color.LimeGreen && dgvPLQuarterly.RowCount == 0)
                || (btnPLAnnually.BackColor == Color.LimeGreen && dgvPLAnnually.RowCount == 0))
            {
                btnPLDLogs.Enabled = false;
                btnPLReport.Enabled = false;
            }

            else
            {
                btnPLDLogs.Enabled = true;
                btnPLReport.Enabled = true;
            }
        }
        #endregion

        #region Search

        #region Search Button
        private void btnPLNSearch_MouseEnter(object sender, EventArgs e)
        {
            btnPLNSearch.Visible = false;
            btnPLHSearch.Visible = true;
        }

        private void btnPLHSearch_MouseLeave(object sender, EventArgs e)
        {
            btnPLNSearch.Visible = true;
            btnPLHSearch.Visible = false;
        }

        private void btnPLHSearch_MouseDown(object sender, MouseEventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;

            btnPLHSearch.Visible = false;
            btnPLDSearch.Visible = true;

            PLTSearch();

            cmbPLSOption_SelectedIndexChanged(sender, e);

            Cursor.Current = Cursors.Default;
        }

        private void btnPLDSearch_MouseUp(object sender, MouseEventArgs e)
        {
            btnPLHSearch.Visible = true;
            btnPLDSearch.Visible = false;
        }

        private void btnPLDSearch_MouseLeave(object sender, EventArgs e)
        {
            btnPLHSearch.Visible = true;
            btnPLDSearch.Visible = false;
        }
        #endregion

        #region Search Shortcut Key
        private void txtPLSearch_KeyDown(object sender, KeyEventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;

            if (txtPLSearch.Focused && e.KeyCode == Keys.Enter)
            {
                PLTSearch();

                cmbPLSOption_SelectedIndexChanged(sender, e);
            }

            Cursor.Current = Cursors.Default;
        }
        #endregion

        #region Search On or Off Focus
        private void txtPLSearch_Enter(object sender, EventArgs e)
        {
            if (txtPLSearch.ForeColor == Color.DarkGray)
            {
                txtPLSearch.Clear();
                txtPLSearch.ForeColor = Color.Black;
            }
        }

        private void txtPLSearch_Leave(object sender, EventArgs e)
        {
            if (txtPLSearch.ForeColor == Color.Black && txtPLSearch.Text == "")
            {
                txtPLSearch.Text = "Search";
                txtPLSearch.ForeColor = Color.DarkGray;
            }
        }
        #endregion

        #region Table Search
        void PLTSearch()
        {
            if (txtPLSearch.Text == "" || txtPLSearch.ForeColor == Color.DarkGray) { }

            else if (txtPLSearch.Text.Equals("All", StringComparison.InvariantCultureIgnoreCase) && cmbPLSOption.Text == "All")
            {
                iplmpage = 1;
                iplmp = 0;

                iplqpage = 1;
                iplqp = 0;

                iplapage = 1;
                iplap = 0;

                splsearch = "";
                splfsearch = "";

                PLFOption();

                PLMLoad(iplmp);
                PLQLoad(iplqp);
                PLALoad(iplap);

                PLMRCDefault();
                PLQRCDefault();
                PLARCDefault();

                PLBDefault();
            }

            else
            {
                iplmpage = 1;
                iplmp = 0;

                iplqpage = 1;
                iplqp = 0;

                iplapage = 1;
                iplap = 0;

                PLSOption();

                PLFOption();

                PLMLoad(iplmp);
                PLQLoad(iplqp);
                PLALoad(iplap);

                PLMRCDefault();
                PLQRCDefault();
                PLARCDefault();

                PLBDefault();
            }

            EDButtons();
        }
        #endregion

        #region Search Option
        private void cmbPLSOption_DropDown(object sender, EventArgs e)
        {
            cmbPLSOption.Items.Remove("Search Option");
            cmbPLSOption.ForeColor = Color.Black;
        }

        private void cmbPLSOption_DropDownClosed(object sender, EventArgs e)
        {
            if (cmbPLSOption.SelectedItem == null)
            {
                if (!cmbPLSOption.Items.Contains("Search Option"))
                {
                    cmbPLSOption.Items.Insert(0, "Search Option");
                }

                cmbPLSOption.Text = "Search Option";
                cmbPLSOption.ForeColor = Color.DarkGray;
            }
        }

        private void cmbPLSOption_SelectedIndexChanged(object sender, EventArgs e)
        {
            PLSearch();

            dgvPLMonthly.Columns[7].Visible = false;
            dgvPLMonthly.Columns[8].Visible = false;
            dgvPLMonthly.Columns[9].Visible = false;
            dgvPLMonthly.Columns[10].Visible = false;
            dgvPLMonthly.Columns[11].Visible = false;
            dgvPLMonthly.Columns[12].Visible = false;
            dgvPLMonthly.Columns[13].Visible = false;
            dgvPLMonthly.Columns[14].Visible = false;

            dgvPLQuarterly.Columns[7].Visible = false;
            dgvPLQuarterly.Columns[8].Visible = false;
            dgvPLQuarterly.Columns[9].Visible = false;
            dgvPLQuarterly.Columns[10].Visible = false;
            dgvPLQuarterly.Columns[11].Visible = false;
            dgvPLQuarterly.Columns[12].Visible = false;
            dgvPLQuarterly.Columns[13].Visible = false;
            dgvPLQuarterly.Columns[14].Visible = false;

            dgvPLAnnually.Columns[7].Visible = false;
            dgvPLAnnually.Columns[8].Visible = false;
            dgvPLAnnually.Columns[9].Visible = false;
            dgvPLAnnually.Columns[10].Visible = false;
            dgvPLAnnually.Columns[11].Visible = false;
            dgvPLAnnually.Columns[12].Visible = false;
            dgvPLAnnually.Columns[13].Visible = false;
            dgvPLAnnually.Columns[14].Visible = false;

            if (cmbPLSOption.Text == "Nature of Business")
            {
                dgvPLMonthly.Columns[7].Visible = true;
                dgvPLQuarterly.Columns[7].Visible = true;
                dgvPLAnnually.Columns[7].Visible = true;
            }

            else if (cmbPLSOption.Text == "Business / Owner TIN")
            {
                dgvPLMonthly.Columns[8].Visible = true;
                dgvPLQuarterly.Columns[8].Visible = true;
                dgvPLAnnually.Columns[8].Visible = true;
            }

            else if (cmbPLSOption.Text == "Tax Type")
            {
                dgvPLMonthly.Columns[9].Visible = true;
                dgvPLQuarterly.Columns[9].Visible = true;
                dgvPLAnnually.Columns[9].Visible = true;
            }

            else if (cmbPLSOption.Text == "SEC Reg. Number")
            {
                dgvPLMonthly.Columns[10].Visible = true;
                dgvPLQuarterly.Columns[10].Visible = true;
                dgvPLAnnually.Columns[10].Visible = true;
            }

            else if (cmbPLSOption.Text == "Group Name")
            {
                dgvPLMonthly.Columns[11].Visible = true;
                dgvPLQuarterly.Columns[11].Visible = true;
                dgvPLAnnually.Columns[11].Visible = true;
            }

            else if (cmbPLSOption.Text == "Sub-group Name")
            {
                dgvPLMonthly.Columns[12].Visible = true;
                dgvPLQuarterly.Columns[12].Visible = true;
                dgvPLAnnually.Columns[12].Visible = true;
            }

            else if (cmbPLSOption.Text == "Group Year")
            {
                dgvPLMonthly.Columns[13].Visible = true;
                dgvPLQuarterly.Columns[13].Visible = true;
                dgvPLAnnually.Columns[13].Visible = true;
            }

            else if (cmbPLSOption.Text == "Status")
            {
                dgvPLMonthly.Columns[14].Visible = true;
                dgvPLQuarterly.Columns[14].Visible = true;
                dgvPLAnnually.Columns[14].Visible = true;
            }
        }

        void PLSOption()
        {
            spls = txtPLSearch.Text.Replace("'", "''");

            if (cmbPLSOption.Text == "Client I.D.")
            {
                splsearch = " WHERE s.Client_ID LIKE '" + spls + "'";
                splfsearch = "{Pay.Client ID} = '" + spls + "'";
            }

            else if (cmbPLSOption.Text == "Client Name")
            {
                splsearch = " WHERE CONCAT(c.Last_Name, ', ', c.First_Name, ' ', c.Middle_Initial) LIKE '" + spls + "'";
                splfsearch = "{Pay.Client Name} = '" + spls + "'";
            }

            else if (cmbPLSOption.Text == "Business Name")
            {
                splsearch = " WHERE c.Business_Name LIKE '" + spls + "'";
                splfsearch = "{Pay.Business Name} = '" + spls + "'";
            }

            else if (cmbPLSOption.Text == "Nature of Business")
            {
                splsearch = " WHERE c.Nature_of_Business LIKE '" + spls + "'";
                splfsearch = "{Pay.Nature of Business} = '" + spls + "'";
            }

            else if (cmbPLSOption.Text == "Business / Owner TIN")
            {
                splsearch = " WHERE c.Business_or_Owner_TIN LIKE '" + spls + "'";
                splfsearch = "{Pay.Business / Owner TIN} = '" + spls + "'";
            }

            else if (cmbPLSOption.Text == "Tax Type")
            {
                splsearch = " WHERE c.Tax_Type LIKE '" + spls + "'";
                splfsearch = "{Pay.Tax Type} = '" + spls + "'";
            }

            else if (cmbPLSOption.Text == "SEC Reg. Number")
            {
                splsearch = " WHERE c.SEC_Registration_Number LIKE '" + spls + "'";
                splfsearch = "{Pay.SEC Reg Number} = '" + spls + "'";
            }

            else if (cmbPLSOption.Text == "Group Name")
            {
                splsearch = " WHERE c.Group_Name LIKE '" + spls + "'";
                splfsearch = "{Pay.Group Name} = '" + spls + "'";
            }

            else if (cmbPLSOption.Text == "Sub-group Name")
            {
                splsearch = " WHERE c.Sub_Group_Name LIKE '" + spls + "'";
                splfsearch = "{Pay.Sub-group Name} = '" + spls + "'";
            }

            else if (cmbPLSOption.Text == "Group Year")
            {
                splsearch = " WHERE c.Group_Year LIKE '" + spls + "'";
                splfsearch = "{Pay.Group Year} = '" + spls + "'";
            }

            else if (cmbPLSOption.Text == "Status")
            {
                splsearch = " WHERE c.Status LIKE '" + spls + "'";
                splfsearch = "{Pay.Status} = '" + spls + "'";
            }

            else
            {
                splsearch = " WHERE Transaction_Number LIKE '" + spls + "'";
                splfsearch = "{Pay.Transaction Number} = '" + spls + "'";
            }
        }
        #endregion

        #region Order Option
        private void cmbPLOOption_DropDown(object sender, EventArgs e)
        {
            cmbPLOOption.Items.Remove("Order Option");
            cmbPLOOption.ForeColor = Color.Black;
        }

        private void cmbPLOOption_DropDownClosed(object sender, EventArgs e)
        {
            if (cmbPLOOption.SelectedItem == null)
            {
                if (!cmbPLOOption.Items.Contains("Order Option"))
                {
                    cmbPLOOption.Items.Insert(0, "Order Option");
                }

                cmbPLOOption.Text = "Order Option";
                cmbPLOOption.ForeColor = Color.DarkGray;
            }
        }

        private void cmbPLOOption_SelectedIndexChanged(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;

            if (cmbPLOOption.ForeColor != Color.DarkGray && cmbPLOOption.Text != "Order Option")
            {
                PLOOption();
                PLMLoad(iplmp);
                PLQLoad(iplqp);
                PLALoad(iplap);
            }

            Cursor.Current = Cursors.Default;
        }

        void PLOOption()
        {
            if (cmbPLOOption.Text == "Client I.D.")
            {
                splorder = " ORDER BY s.Client_ID;";
                splfield = "Client ID";
            }

            else if (cmbPLOOption.Text == "Client Name")
            {
                splorder = " ORDER BY CONCAT(c.Last_Name, ', ', c.First_Name, ' ', c.Middle_Initial)";
                splfield = "Client Name";
            }

            else if (cmbPLOOption.Text == "Business Name")
            {
                splorder = " ORDER BY c.Business_Name";
                splfield = "Business Name";
            }

            else if (cmbPLOOption.Text == "Group Name")
            {
                splorder = " ORDER BY c.Group_Name";
                splfield = "Group Name";
            }

            else if (cmbPLOOption.Text == "Sub-group Name")
            {
                splorder = " ORDER BY c.Sub_Group_Name";
                splfield = "Sub-group Name";
            }

            else if (cmbPLOOption.Text == "Group Year")
            {
                splorder = " ORDER BY c.Group_Year";
                splfield = "Group Year";
            }

            else if (cmbPLOOption.Text == "Frequency")
            {
                splorder = " ORDER BY DATE(Payment_Date)";
                splfield = "Sort Date";
            }

            else
            {
                splorder = " ORDER BY s.Transaction_Number";
                splfield = "Transaction Number";
            }
        }
        #endregion

        #region Frequency Option
        private void cmbPLFOption_DropDown(object sender, EventArgs e)
        {
            cmbPLFOption.Items.Remove("Frequency Option");
            cmbPLFOption.ForeColor = Color.Black;
        }

        private void cmbPLFOption_DropDownClosed(object sender, EventArgs e)
        {
            if (cmbPLFOption.SelectedItem == null)
            {
                if (!cmbPLFOption.Items.Contains("Frequency Option"))
                {
                    cmbPLFOption.Items.Insert(0, "Frequency Option");
                }

                cmbPLFOption.Text = "Frequency Option";
                cmbPLFOption.ForeColor = Color.DarkGray;
            }
        }

        private void cmbPLFOption_SelectedIndexChanged(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;

            PLFOption();

            iplmpage = 1;
            iplmp = 0;

            iplqpage = 1;
            iplqp = 0;

            iplapage = 1;
            iplap = 0;

            PLMLoad(iplmp);
            PLQLoad(iplqp);
            PLALoad(iplap);

            PLMRCDefault();
            PLQRCDefault();
            PLARCDefault();

            PLBDefault();

            EDButtons();

            Cursor.Current = Cursors.Default;
        }

        private void dtpPLFrom_ValueChanged(object sender, EventArgs e)
        {
            cmbPLFOption_SelectedIndexChanged(sender, e);
        }

        private void dtpPLTo_ValueChanged(object sender, EventArgs e)
        {
            cmbPLFOption_SelectedIndexChanged(sender, e);
        }

        void PLFOption()
        {
            if (cmbPLFOption.Text == "Daily")
            {
                dtpPLFrom.Enabled = true;
                dtpPLTo.Enabled = true;

                dtpPLFrom.CustomFormat = "MM-dd-yyyy";
                dtpPLTo.CustomFormat = "MM-dd-yyyy";

                dtpPLFrom.ShowUpDown = false;
                dtpPLTo.ShowUpDown = false;

                DateTime dfrom, dto;

                dfrom = dtpPLFrom.Value;
                dto = dtpPLTo.Value;

                spldfrom = dfrom.ToString("yyyy-MM-dd");
                spldto = dto.ToString("yyyy-MM-dd");

                psPLFDate = dfrom.ToString("MM-dd-yyyy");
                psPLTDate = dto.ToString("MM-dd-yyyy");

                if (splsearch == "")
                {
                    spldrange = " WHERE DATE(s.Payment_Date) BETWEEN '" + spldfrom + "' AND '" + spldto + "'";
                }

                else
                {
                    spldrange = " AND DATE(s.Payment_Date) BETWEEN '" + spldfrom + "' AND '" + spldto + "'";
                }

                if (splfsearch == "")
                {
                    splfdrange = "datevalue({Pay.Payment Date}) >= datevalue('" + spldfrom + "') and datevalue({Pay.Payment Date}) <= datevalue('" + spldto + "')";
                }

                else
                {
                    splfdrange = " and datevalue({Pay.Payment Date}) >= datevalue('" + spldfrom + "') and datevalue({Pay.Payment Date}) <= datevalue('" + spldto + "')";
                }
            }

            else if (cmbPLFOption.Text == "Monthly")
            {
                dtpPLFrom.Enabled = true;
                dtpPLTo.Enabled = true;

                dtpPLFrom.CustomFormat = "MM-yyyy";
                dtpPLTo.CustomFormat = "MM-yyyy";

                dtpPLFrom.ShowUpDown = true;
                dtpPLTo.ShowUpDown = true;

                DateTime dfrom, dto;
                int m, y;

                dfrom = dtpPLFrom.Value;
                dto = dtpPLTo.Value;

                m = Convert.ToInt32(dto.ToString("MM"));
                y = Convert.ToInt32(dto.ToString("yyyy"));

                spldfrom = dfrom.ToString("yyyy-MM-01");
                spldto = dto.ToString("yyyy-MM-" + DateTime.DaysInMonth(y, m));

                psPLFDate = dfrom.ToString("MM-01-yyyy");
                psPLTDate = dto.ToString("MM-" + DateTime.DaysInMonth(y, m) + "-yyyy");

                if (splsearch == "")
                {
                    spldrange = " WHERE DATE(s.Payment_Date) BETWEEN '" + spldfrom + "' AND '" + spldto + "'";
                }

                else
                {
                    spldrange = " AND DATE(s.Payment_Date) BETWEEN '" + spldfrom + "' AND '" + spldto + "'";
                }

                if (splfsearch == "")
                {
                    splfdrange = "datevalue({Pay.Payment Date}) >= datevalue('" + spldfrom + "') and datevalue({Pay.Payment Date}) <= datevalue('" + spldto + "')";
                }

                else
                {
                    splfdrange = " and datevalue({Pay.Payment Date}) >= datevalue('" + spldfrom + "') and datevalue({Pay.Payment Date}) <= datevalue('" + spldto + "')";
                }
            }

            else if (cmbPLFOption.Text == "Annually")
            {
                dtpPLFrom.Enabled = true;
                dtpPLTo.Enabled = true;

                dtpPLFrom.CustomFormat = "yyyy";
                dtpPLTo.CustomFormat = "yyyy";

                dtpPLFrom.ShowUpDown = true;
                dtpPLTo.ShowUpDown = true;

                DateTime dfrom, dto;

                dfrom = dtpPLFrom.Value;
                dto = dtpPLTo.Value;

                spldfrom = dfrom.ToString("yyyy-01-01");
                spldto = dto.ToString("yyyy-12-31");

                psPLFDate = dfrom.ToString("01-01-yyyy");
                psPLTDate = dto.ToString("12-31-yyyy");

                if (splsearch == "")
                {
                    spldrange = " WHERE DATE(s.Payment_Date) BETWEEN '" + spldfrom + "' AND '" + spldto + "'";
                }

                else
                {
                    spldrange = " AND DATE(s.Payment_Date) BETWEEN '" + spldfrom + "' AND '" + spldto + "'";
                }

                if (splfsearch == "")
                {
                    splfdrange = "datevalue({Pay.Payment Date}) >= datevalue('" + spldfrom + "') and datevalue({Pay.Payment Date}) <= datevalue('" + spldto + "')";
                }

                else
                {
                    splfdrange = " and datevalue({Pay.Payment Date}) >= datevalue('" + spldfrom + "') and datevalue({Pay.Payment Date}) <= datevalue('" + spldto + "')";
                }
            }

            else
            {
                dtpPLFrom.Enabled = false;
                dtpPLTo.Enabled = false;

                dtpPLFrom.CustomFormat = "MM-dd-yyyy";
                dtpPLTo.CustomFormat = "MM-dd-yyyy";

                dtpPLFrom.ShowUpDown = false;
                dtpPLTo.ShowUpDown = false;

                spldrange = "";
                splfdrange = "";
            }
        }
        #endregion

        #region Auto Complete Search
        void PLSearch()
        {
            string tbl = "";

            if (btnPLMonthly.BackColor == Color.LimeGreen)
            {
                tbl = "tbl_mpaylogs";
            }

            else if (btnPLQuarterly.BackColor == Color.LimeGreen)
            {
                tbl = "tbl_qpaylogs";
            }

            else if (btnPLAnnually.BackColor == Color.LimeGreen)
            {
                tbl = "tbl_apaylogs";
            }

            AutoCompleteStringCollection acs = new AutoCompleteStringCollection();

            string cQuery = "SELECT s.Client_ID, s.Transaction_Number, CONCAT(c.Last_Name, ', ', c.First_Name, ' ', c.Middle_Initial),"
                + " c.Business_Name, c.Nature_of_Business, c.Business_or_Owner_TIN, c.Tax_Type,"
                + " c.SEC_Registration_Number, c.Group_Name, c.Sub_Group_Name, c.Group_Year,"
                + " c.Status FROM " + tbl + " s LEFT JOIN tbl_clients c ON s.Client_ID = c.Client_ID;";
            MySqlConnection cConnection = new MySqlConnection(Conn.cString);
            MySqlCommand cCommand = new MySqlCommand(cQuery, cConnection);
            MySqlDataReader cReader;

            try
            {
                cConnection.Open();
                cReader = cCommand.ExecuteReader();

                acs.Clear();

                while (cReader.Read())
                {
                    string cid, trn, cn, bn, nob, bot, tt, sec, gn, sgn, gy, st;

                    cid = cReader.GetString(0);
                    trn = cReader.GetString(1);
                    cn = cReader.GetString(2);
                    bn = cReader.GetString(3);
                    nob = cReader.GetString(4);
                    bot = cReader.GetString(5);
                    tt = cReader.GetString(6);
                    sec = cReader.GetString(7);
                    gn = cReader.GetString(8);
                    sgn = cReader.GetString(9);
                    gy = cReader.GetString(10);
                    st = cReader.GetString(11);

                    if (cmbPLSOption.Text == "All")
                    {
                        acs.Add("All");
                        acs.Add(trn);
                    }

                    else if (cmbPLSOption.Text == "Client I.D.")
                    {
                        acs.Add(cid);
                    }

                    else if (cmbPLSOption.Text == "Client Name")
                    {
                        acs.Add(cn);
                    }

                    else if (cmbPLSOption.Text == "Business Name")
                    {
                        acs.Add(bn);
                    }

                    else if (cmbPLSOption.Text == "Nature of Business")
                    {
                        acs.Add(nob);
                    }

                    else if (cmbPLSOption.Text == "Business / Owner TIN")
                    {
                        acs.Add(bot);
                    }

                    else if (cmbPLSOption.Text == "Tax Type")
                    {
                        acs.Add(tt);
                    }

                    else if (cmbPLSOption.Text == "SEC Reg. Number")
                    {
                        acs.Add(sec);
                    }

                    else if (cmbPLSOption.Text == "Group Name")
                    {
                        acs.Add(gn);
                    }

                    else if (cmbPLSOption.Text == "Sub-group Name")
                    {
                        acs.Add(sgn);
                    }

                    else if (cmbPLSOption.Text == "Group Year")
                    {
                        acs.Add(gy);
                    }

                    else if (cmbPLSOption.Text == "Status")
                    {
                        acs.Add(st);
                    }

                    else
                    {
                        acs.Add(trn);
                    }
                }
            }

            catch (MySqlException ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            finally
            {
                cConnection.Close();
            }

            txtPLSearch.AutoCompleteCustomSource = acs;
        }
        #endregion

        #region Enable Tooltip
        private void txtPLSearch_ForeColorChanged(object sender, EventArgs e)
        {
            TTip();
        }

        private void cmbPLSOption_ForeColorChanged(object sender, EventArgs e)
        {
            TTip();
        }

        private void cmbPLOOption_ForeColorChanged(object sender, EventArgs e)
        {
            TTip();
        }

        private void cmbPLFOption_ForeColorChanged(object sender, EventArgs e)
        {
            TTip();
        }
        #endregion

        #endregion

        #region Report
        private void btnPLReport_Click(object sender, EventArgs e)
        {
            PReport w = new PReport();
            w.ShowDialog();
        }
        #endregion

        #region Services Drop Down
        string plddstatus = "*Close";

        void PLDOpen()
        {
            if (plddstatus == "*Open")
            {
                btnPLServices.BackColor = Color.LimeGreen;
                bplogs = true;
                flpPLDDown.Visible = true;
            }
        }

        void PLDClose()
        {
            bplogs = false;
            thide.Start();
        }

        private void btnPLServices_MouseEnter(object sender, EventArgs e)
        {
            if (plddstatus == "*Open")
            {
                btnPLServices.Text = "   Services     ▼";
                bplogs = true;
                flpPLDDown.Visible = true;
            }
        }

        private void btnPLServices_MouseLeave(object sender, EventArgs e)
        {
            bplogs = false;
            thide.Start();
        }

        private void btnPLMonthly_MouseEnter(object sender, EventArgs e)
        {
            PLDOpen();
        }

        private void btnPLMonthly_MouseLeave(object sender, EventArgs e)
        {
            PLDClose();
        }

        private void btnPLQuarterly_MouseEnter(object sender, EventArgs e)
        {
            PLDOpen();
        }

        private void btnPLQuarterly_MouseLeave(object sender, EventArgs e)
        {
            PLDClose();
        }

        private void btnPLAnnually_MouseEnter(object sender, EventArgs e)
        {
            PLDOpen();
        }

        private void btnPLAnnually_MouseLeave(object sender, EventArgs e)
        {
            PLDClose();
        }
        #endregion

        #region Billing and Payments Services Navigation
        void PLCDefault()
        {
            dgvPLMonthly.Visible = false;
            dgvPLQuarterly.Visible = false;
            dgvPLAnnually.Visible = false;
        }

        void PLNBDefault()
        {
            btnPLMonthly.BackColor = Color.FromArgb(64, 64, 64); ;
            btnPLMonthly.FlatAppearance.MouseOverBackColor = Color.LimeGreen;
            btnPLMonthly.FlatAppearance.MouseDownBackColor = Color.ForestGreen;
            btnPLMonthly.Cursor = Cursors.Hand;

            btnPLQuarterly.BackColor = Color.FromArgb(64, 64, 64); ;
            btnPLQuarterly.FlatAppearance.MouseOverBackColor = Color.LimeGreen;
            btnPLQuarterly.FlatAppearance.MouseDownBackColor = Color.ForestGreen;
            btnPLQuarterly.Cursor = Cursors.Hand;

            btnPLAnnually.BackColor = Color.FromArgb(64, 64, 64); ;
            btnPLAnnually.FlatAppearance.MouseOverBackColor = Color.LimeGreen;
            btnPLAnnually.FlatAppearance.MouseDownBackColor = Color.ForestGreen;
            btnPLAnnually.Cursor = Cursors.Hand;
        }

        void PLNDefault()
        {
            flpPLM.Visible = false;
            flpPLQ.Visible = false;
            flpPLA.Visible = false;
        }

        private void btnPLServices_Click(object sender, EventArgs e)
        {
            if (plddstatus == "*Close")
            {
                plddstatus = "*Open";

                btnPLServices.Text = "   Services     ▼";
                bplogs = true;
                flpPLDDown.Visible = true;
            }

            else
            {
                plddstatus = "*Close";

                bplogs = false;
                thide.Start();
            }
        }

        private void btnPLMonthly_Click(object sender, EventArgs e)
        {
            if (!dgvPLMonthly.Visible)
            {
                PLNBDefault();

                btnPLMonthly.BackColor = Color.LimeGreen;
                btnPLMonthly.FlatAppearance.MouseOverBackColor = Color.LimeGreen;
                btnPLMonthly.FlatAppearance.MouseDownBackColor = Color.LimeGreen;
                btnPLMonthly.Cursor = Cursors.Default;

                PLCDefault();

                dgvPLMonthly.Visible = true;
                PLNDefault();
                flpPLM.Visible = true;
                PLBDefault();
                PLGSRow();

                PLSearch();

                EDButtons();
            }

            btnPLServices.Focus();
        }

        private void btnPLQuarterly_Click(object sender, EventArgs e)
        {
            if (!dgvBPQuarterly.Visible)
            {
                PLNBDefault();

                btnPLQuarterly.BackColor = Color.LimeGreen;
                btnPLQuarterly.FlatAppearance.MouseOverBackColor = Color.LimeGreen;
                btnPLQuarterly.FlatAppearance.MouseDownBackColor = Color.LimeGreen;
                btnPLQuarterly.Cursor = Cursors.Default;

                PLCDefault();

                dgvPLQuarterly.Visible = true;
                PLNDefault();
                flpPLQ.Visible = true;
                PLBDefault();
                PLGSRow();

                PLSearch();

                EDButtons();
            }

            btnPLServices.Focus();
        }

        private void btnPLAnnually_Click(object sender, EventArgs e)
        {
            if (!dgvPLAnnually.Visible)
            {
                PLNBDefault();

                btnPLAnnually.BackColor = Color.LimeGreen;
                btnPLAnnually.FlatAppearance.MouseOverBackColor = Color.LimeGreen;
                btnPLAnnually.FlatAppearance.MouseDownBackColor = Color.LimeGreen;
                btnPLAnnually.Cursor = Cursors.Default;

                PLCDefault();

                dgvPLAnnually.Visible = true;
                PLNDefault();
                flpPLA.Visible = true;
                PLBDefault();
                PLGSRow();

                PLSearch();

                EDButtons();
            }

            btnPLServices.Focus();
        }
        #endregion

        #region Pagination

        #region Monthly Services
        void PLMPage()
        {
            dplmpage = dplmcount / iplmdpage;
            dplmtpage = Math.Truncate(dplmpage);
            splmpage = Convert.ToString(dplmpage);

            if (splmpage.Contains("."))
            {
                dplmrpage = dplmtpage + 1;
            }

            else
            {
                dplmrpage = dplmtpage;
            }
        }
        #endregion

        #region Quarterly Services
        void PLQPage()
        {
            dplqpage = dplqcount / iplqdpage;
            dplqtpage = Math.Truncate(dplqpage);
            splqpage = Convert.ToString(dplqpage);

            if (splqpage.Contains("."))
            {
                dplqrpage = dplqtpage + 1;
            }

            else
            {
                dplqrpage = dplqtpage;
            }
        }
        #endregion

        #region Annually Services
        void PLAPage()
        {
            dplapage = dplacount / ipladpage;
            dplatpage = Math.Truncate(dplapage);
            splapage = Convert.ToString(dplapage);

            if (splapage.Contains("."))
            {
                dplarpage = dplatpage + 1;
            }

            else
            {
                dplarpage = dplatpage;
            }
        }
        #endregion

        #endregion

        #region Row Count Default
        void PLMRCDefault()
        {
            PLMPage();

            if (iplmpage == 1 && dplmrpage <= 1)
            {
                btnPLMFirst.Enabled = false;
                btnPLMBack.Enabled = false;
                btnPLMNext.Enabled = false;
                btnPLMLast.Enabled = false;
            }

            else if (iplmpage == 1 && dplmrpage > 1)
            {
                btnPLMFirst.Enabled = false;
                btnPLMBack.Enabled = false;
                btnPLMNext.Enabled = true;
                btnPLMLast.Enabled = true;
            }

            else if (iplmpage > 1 && iplmpage < dplmrpage)
            {
                btnPLMFirst.Enabled = true;
                btnPLMBack.Enabled = true;
                btnPLMNext.Enabled = true;
                btnPLMLast.Enabled = true;
            }

            else if (iplmpage == dplmrpage)
            {
                btnPLMFirst.Enabled = true;
                btnPLMBack.Enabled = true;
                btnPLMNext.Enabled = false;
                btnPLMLast.Enabled = false;
            }

            EDButtons();
        }

        void PLQRCDefault()
        {
            PLQPage();

            if (iplqpage == 1 && dplqrpage <= 1)
            {
                btnPLQFirst.Enabled = false;
                btnPLQBack.Enabled = false;
                btnPLQNext.Enabled = false;
                btnPLQLast.Enabled = false;
            }

            else if (iplqpage == 1 && dplqrpage > 1)
            {
                btnPLQFirst.Enabled = false;
                btnPLQBack.Enabled = false;
                btnPLQNext.Enabled = true;
                btnPLQLast.Enabled = true;
            }

            else if (iplqpage > 1 && iplqpage < dplqrpage)
            {
                btnPLQFirst.Enabled = true;
                btnPLQBack.Enabled = true;
                btnPLQNext.Enabled = true;
                btnPLQLast.Enabled = true;
            }

            else if (iplqpage == dplqrpage)
            {
                btnPLQFirst.Enabled = true;
                btnPLQBack.Enabled = true;
                btnPLQNext.Enabled = false;
                btnPLQLast.Enabled = false;
            }

            EDButtons();
        }

        void PLARCDefault()
        {
            PLAPage();

            if (iplapage == 1 && dplarpage <= 1)
            {
                btnPLAFirst.Enabled = false;
                btnPLABack.Enabled = false;
                btnPLANext.Enabled = false;
                btnPLALast.Enabled = false;
            }

            else if (iplapage == 1 && dplarpage > 1)
            {
                btnPLAFirst.Enabled = false;
                btnPLABack.Enabled = false;
                btnPLANext.Enabled = true;
                btnPLALast.Enabled = true;
            }

            else if (iplapage > 1 && iplapage < dplarpage)
            {
                btnPLAFirst.Enabled = true;
                btnPLABack.Enabled = true;
                btnPLANext.Enabled = true;
                btnPLALast.Enabled = true;
            }

            else if (iplapage == dplarpage)
            {
                btnPLAFirst.Enabled = true;
                btnPLABack.Enabled = true;
                btnPLANext.Enabled = false;
                btnPLALast.Enabled = false;
            }

            EDButtons();
        }
        #endregion

        #region Pagination Focus Control
        void PLPFC()
        {
            if (btnPLMonthly.BackColor == Color.LimeGreen)
            {
                dgvPLMonthly.Focus();
            }

            else if (btnPLQuarterly.BackColor == Color.LimeGreen)
            {
                dgvPLQuarterly.Focus();
            }

            else if (btnPLAnnually.BackColor == Color.LimeGreen)
            {
                dgvPLAnnually.Focus();
            }
        }
        #endregion

        #region First

        #region Monthly Services
        private void btnPLMFirst_Click(object sender, EventArgs e)
        {
            iplmpage = 1;
            iplmp = 0;

            PLMLoad(iplmp);

            cmbPLSOption_SelectedIndexChanged(sender, e);

            PLMRCDefault();

            PLPFC();
        }
        #endregion

        #region Quarterly Services
        private void btnPLQFirst_Click(object sender, EventArgs e)
        {
            iplqpage = 1;
            iplqp = 0;

            PLQLoad(iplqp);

            cmbPLSOption_SelectedIndexChanged(sender, e);

            PLQRCDefault();

            PLPFC();
        }
        #endregion

        #region Annually Services
        private void btnPLAFirst_Click(object sender, EventArgs e)
        {
            iplapage = 1;
            iplap = 0;

            PLALoad(iplap);

            cmbPLSOption_SelectedIndexChanged(sender, e);

            PLARCDefault();

            PLPFC();
        }
        #endregion

        #endregion

        #region Back

        #region Monthly Services
        private void btnPLMBack_Click(object sender, EventArgs e)
        {
            PLMPage();

            iplmpage--;

            if (iplmpage == 1)
            {
                btnPLMFirst.Enabled = false;
                btnPLMBack.Enabled = false;
            }

            if (iplmpage < dplmrpage)
            {
                btnPLMNext.Enabled = true;
                btnPLMLast.Enabled = true;
            }

            iplmp = iplmp - iplmdpage;
            PLMLoad(iplmp);

            cmbPLSOption_SelectedIndexChanged(sender, e);

            EDButtons();

            PLPFC();
        }
        #endregion

        #region Quarterly Services
        private void btnPLQBack_Click(object sender, EventArgs e)
        {
            PLQPage();

            iplqpage--;

            if (iplqpage == 1)
            {
                btnPLQFirst.Enabled = false;
                btnPLQBack.Enabled = false;
            }

            if (iplqpage < dplqrpage)
            {
                btnPLQNext.Enabled = true;
                btnPLQLast.Enabled = true;
            }

            iplqp = iplqp - iplqdpage;
            PLQLoad(iplqp);

            cmbPLSOption_SelectedIndexChanged(sender, e);

            EDButtons();

            PLPFC();
        }
        #endregion

        #region Annually
        private void btnPLABack_Click(object sender, EventArgs e)
        {
            PLAPage();

            iplapage--;

            if (iplapage == 1)
            {
                btnPLAFirst.Enabled = false;
                btnPLABack.Enabled = false;
            }

            if (iplapage < dplarpage)
            {
                btnPLANext.Enabled = true;
                btnPLALast.Enabled = true;
            }

            iplap = iplap - ipladpage;
            PLALoad(iplap);

            cmbPLSOption_SelectedIndexChanged(sender, e);

            EDButtons();

            PLPFC();
        }
        #endregion

        #endregion

        #region Next

        #region Monthly Services
        private void btnPLMNext_Click(object sender, EventArgs e)
        {
            PLMPage();

            iplmpage++;

            if (iplmpage == dplmrpage)
            {
                btnPLMNext.Enabled = false;
                btnPLMLast.Enabled = false;
            }

            if (iplmpage > 1)
            {
                btnPLMFirst.Enabled = true;
                btnPLMBack.Enabled = true;
            }

            iplmp = iplmp + iplmdpage;
            PLMLoad(iplmp);

            cmbPLSOption_SelectedIndexChanged(sender, e);

            EDButtons();

            PLPFC();
        }
        #endregion

        #region Quarterly Services
        private void btnPLQNext_Click(object sender, EventArgs e)
        {
            PLQPage();

            iplqpage++;

            if (iplqpage == dplqrpage)
            {
                btnPLQNext.Enabled = false;
                btnPLQLast.Enabled = false;
            }

            if (iplqpage > 1)
            {
                btnPLQFirst.Enabled = true;
                btnPLQBack.Enabled = true;
            }

            iplqp = iplqp + iplqdpage;
            PLQLoad(iplqp);

            cmbPLSOption_SelectedIndexChanged(sender, e);

            EDButtons();

            PLPFC();
        }
        #endregion

        #region Annually Services
        private void btnPLANext_Click(object sender, EventArgs e)
        {
            PLAPage();

            iplapage++;

            if (iplapage == dplarpage)
            {
                btnPLANext.Enabled = false;
                btnPLALast.Enabled = false;
            }

            if (iplapage > 1)
            {
                btnPLAFirst.Enabled = true;
                btnPLABack.Enabled = true;
            }

            iplap = iplap + ipladpage;
            PLALoad(iplap);

            cmbPLSOption_SelectedIndexChanged(sender, e);

            EDButtons();

            PLPFC();
        }
        #endregion

        #endregion

        #region Last

        #region Mothly Services
        private void btnPLMLast_Click(object sender, EventArgs e)
        {
            PLMPage();

            iplmp = Convert.ToInt32((dplmrpage * iplmdpage) - iplmdpage);
            iplmpage = Convert.ToInt32(dplmrpage);

            PLMLoad(iplmp);

            btnPLMFirst.Enabled = true;
            btnPLMBack.Enabled = true;
            btnPLMNext.Enabled = false;
            btnPLMLast.Enabled = false;

            cmbPLSOption_SelectedIndexChanged(sender, e);

            EDButtons();

            PLPFC();
        }
        #endregion

        #region Quarterly Services
        private void btnPLQLast_Click(object sender, EventArgs e)
        {
            PLQPage();

            iplqp = Convert.ToInt32((dplqrpage * iplqdpage) - iplqdpage);
            iplqpage = Convert.ToInt32(dplqrpage);

            PLQLoad(iplqp);

            btnPLQFirst.Enabled = true;
            btnPLQBack.Enabled = true;
            btnPLQNext.Enabled = false;
            btnPLQLast.Enabled = false;

            cmbPLSOption_SelectedIndexChanged(sender, e);

            EDButtons();

            PLPFC();
        }
        #endregion

        #region Annually Services
        private void btnPLALast_Click(object sender, EventArgs e)
        {
            PLAPage();

            iplap = Convert.ToInt32((dplarpage * ipladpage) - ipladpage);
            iplapage = Convert.ToInt32(dplarpage);

            PLALoad(iplap);

            btnPLAFirst.Enabled = true;
            btnPLABack.Enabled = true;
            btnPLANext.Enabled = false;
            btnPLALast.Enabled = false;

            cmbPLSOption_SelectedIndexChanged(sender, e);

            EDButtons();

            PLPFC();
        }
        #endregion

        #endregion

        #region Cell Enter
        private void dgvPL_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            DataGridView dgv = (DataGridView)sender;

            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgv.Rows[e.RowIndex];

                psPLNID = row.Cells["No_ID"].Value.ToString();

                psPLTRN = row.Cells["Transaction Number"].Value.ToString();
            }

            PLTColors();
        }
        #endregion

        #region Get Selected Row
        void PLGSRow()
        {
            if (btnPLMonthly.BackColor == Color.LimeGreen)
            {
                foreach (DataGridViewRow row in dgvPLMonthly.SelectedRows)
                {
                    psPLNID = row.Cells["No_ID"].Value.ToString();

                    psPLTRN = row.Cells["Transaction Number"].Value.ToString();
                }
            }

            else if (btnPLQuarterly.BackColor == Color.LimeGreen)
            {
                foreach (DataGridViewRow row in dgvPLQuarterly.SelectedRows)
                {
                    psPLNID = row.Cells["No_ID"].Value.ToString();

                    psPLTRN = row.Cells["Transaction Number"].Value.ToString();
                }
            }

            else if (btnPLAnnually.BackColor == Color.LimeGreen)
            {
                foreach (DataGridViewRow row in dgvPLAnnually.SelectedRows)
                {
                    psPLNID = row.Cells["No_ID"].Value.ToString();

                    psPLTRN = row.Cells["Transaction Number"].Value.ToString();
                }
            }
        }
        #endregion

        #region Delete Payment
        private void btnPLDLogs_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;

            if (btnPLMonthly.BackColor == Color.LimeGreen)
            {
                #region Monthly Services
                DialogResult dr = MessageBox.Show("Are you sure you want to delete this monthly payment?", "Delete Payment", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (dr == DialogResult.No) { }

                else
                {
                    string cQuery = "DELETE FROM tbl_mpaylogs WHERE No_ID = '" + psPLNID + "';";
                    MySqlConnection cConnection = new MySqlConnection(Conn.cString);
                    MySqlCommand cCommand = new MySqlCommand(cQuery, cConnection);
                    MySqlDataReader cReader;

                    try
                    {
                        cConnection.Open();
                        cReader = cCommand.ExecuteReader();
                        while (cReader.Read()) { }

                        string trn = psPLTRN;

                        PLMLoad(iplmp);
                        PLSearch();

                        PLBDefault();
                        EDButtons();

                        cmbPLSOption_SelectedIndexChanged(sender, e);

                        MessageBox.Show("Monthly payment of transaction number " + trn + " has been deleted.", "Delete Payment", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }

                    catch (MySqlException ex)
                    {
                        MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }

                    finally
                    {
                        cConnection.Close();
                    }
                }
                #endregion
            }

            else if (btnPLQuarterly.BackColor == Color.LimeGreen)
            {
                #region Quarterly Services
                DialogResult dr = MessageBox.Show("Are you sure you want to delete this quarterly payment?", "Delete Payment", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (dr == DialogResult.No) { }

                else
                {
                    string cQuery = "DELETE FROM tbl_qpaylogs WHERE No_ID = '" + psPLNID + "';";
                    MySqlConnection cConnection = new MySqlConnection(Conn.cString);
                    MySqlCommand cCommand = new MySqlCommand(cQuery, cConnection);
                    MySqlDataReader cReader;

                    try
                    {
                        cConnection.Open();
                        cReader = cCommand.ExecuteReader();
                        while (cReader.Read()) { }

                        string trn = psPLTRN;

                        PLQLoad(iplqp);
                        PLSearch();

                        PLBDefault();
                        EDButtons();

                        cmbPLSOption_SelectedIndexChanged(sender, e);

                        MessageBox.Show("Quarterly payment of transaction number " + trn + " has been deleted.", "Delete Payment", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }

                    catch (MySqlException ex)
                    {
                        MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }

                    finally
                    {
                        cConnection.Close();
                    }
                }
                #endregion
            }

            else if (btnPLAnnually.BackColor == Color.LimeGreen)
            {
                #region Annually Services
                DialogResult dr = MessageBox.Show("Are you sure you want to delete this annual payment?", "Delete Payment", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (dr == DialogResult.No) { }

                else
                {
                    string cQuery = "DELETE FROM tbl_apaylogs WHERE No_ID = '" + psPLNID + "';";
                    MySqlConnection cConnection = new MySqlConnection(Conn.cString);
                    MySqlCommand cCommand = new MySqlCommand(cQuery, cConnection);
                    MySqlDataReader cReader;

                    try
                    {
                        cConnection.Open();
                        cReader = cCommand.ExecuteReader();
                        while (cReader.Read()) { }

                        string trn = psPLTRN;

                        PLALoad(iplap);
                        PLSearch();

                        PLBDefault();
                        EDButtons();

                        cmbPLSOption_SelectedIndexChanged(sender, e);

                        MessageBox.Show("Annual payment of transaction number " + trn + " has been deleted.", "Delete Payment", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }

                    catch (MySqlException ex)
                    {
                        MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }

                    finally
                    {
                        cConnection.Close();
                    }
                }
                #endregion
            }

            Cursor.Current = Cursors.Default;
        }
        #endregion

        #region Refresh
        private void btnPLRefresh_Click(object sender, EventArgs e)
        {
            PLMLoad(iplmp);
            PLQLoad(iplqp);
            PLALoad(iplap);

            PLMRCDefault();
            PLQRCDefault();
            PLARCDefault();

            cmbPLSOption_SelectedIndexChanged(sender, e);

            PLGSRow();

            PLBDefault();

            EDButtons();
        }
        #endregion

        //----------------------------------------------------------------------// User Logs

        #region Load Table
        string sulsearch = "", sulorder = "", suldate = "", sulpage = "";
        int iulp = 0, iulpage = 1, iuldpage = tcontent;
        double dulcount = 0, dulpage = 0, dultpage = 0, dulrpage = 0;
        void ULLoad(int p)
        {
            string cQueryI = "SELECT COUNT(*) FROM tbl_userlogs" + sulsearch + suldate + ";";
            MySqlConnection cConnectionI = new MySqlConnection(Conn.uString);
            MySqlCommand cCommandI = new MySqlCommand(cQueryI, cConnectionI);
            MySqlDataReader cReaderI;

            try
            {
                cConnectionI.Open();
                cReaderI = cCommandI.ExecuteReader();
                while (cReaderI.Read())
                {
                    dulcount = cReaderI.GetDouble(0);
                }
            }

            catch (MySqlException ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            finally
            {
                cConnectionI.Close();
            }

            MySqlConnection cConn = new MySqlConnection(Conn.uString);
            MySqlCommand cCommand = new MySqlCommand("SELECT No_ID, Username, User_Type AS 'User Type',"
                + " Activity, DATE_FORMAT(Time, '%m-%d-%Y %h:%i %p') AS 'Time', PC_Name AS 'PC Name' FROM tbl_userlogs" + sulsearch + suldate + sulorder + ";", cConn);

            try
            {
                MySqlDataAdapter cAdapter = new MySqlDataAdapter();
                cAdapter.SelectCommand = cCommand;
                cDatasetUL = new DataTable();
                cAdapter.Fill(p, iuldpage, cDatasetUL);
                BindingSource cSource = new BindingSource();
                cSource.DataSource = cDatasetUL;
                dgvULogs.DataSource = cSource;
                cAdapter.Update(cDatasetUL);
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            dgvULogs.Columns[0].Visible = false;
        }
        #endregion

        #region Search

        #region Search Button
        private void btnULNSearch_MouseEnter(object sender, EventArgs e)
        {
            btnULNSearch.Visible = false;
            btnULHSearch.Visible = true;
        }

        private void btnULHSearch_MouseLeave(object sender, EventArgs e)
        {
            btnULNSearch.Visible = true;
            btnULHSearch.Visible = false;
        }

        private void btnULHSearch_MouseDown(object sender, MouseEventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;

            btnULHSearch.Visible = false;
            btnULDSearch.Visible = true;

            ULTSearch();

            Cursor.Current = Cursors.Default;
        }

        private void btnULDSearch_MouseUp(object sender, MouseEventArgs e)
        {
            btnULHSearch.Visible = true;
            btnULDSearch.Visible = false;
        }

        private void btnULDSearch_MouseLeave(object sender, EventArgs e)
        {
            btnULHSearch.Visible = true;
            btnULDSearch.Visible = false;
        }
        #endregion

        #region Search Shortcut Key
        private void txtULSearch_KeyDown(object sender, KeyEventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;

            if (txtULSearch.Focused && e.KeyCode == Keys.Enter)
            {
                ULTSearch();
            }

            Cursor.Current = Cursors.Default;
        }
        #endregion

        #region Search On or Off Focus
        private void txtULSearch_Enter(object sender, EventArgs e)
        {
            if (txtULSearch.ForeColor == Color.DarkGray)
            {
                txtULSearch.Clear();
                txtULSearch.ForeColor = Color.Black;
            }
        }

        private void txtULSearch_Leave(object sender, EventArgs e)
        {
            if (txtULSearch.ForeColor == Color.Black && txtULSearch.Text == "")
            {
                txtULSearch.Text = "Search Username";
                txtULSearch.ForeColor = Color.DarkGray;
            }
        }
        #endregion

        #region Table Search
        void ULTSearch()
        {
            if (txtULSearch.Text == "" || txtULSearch.ForeColor == Color.DarkGray) { }

            else if (txtULSearch.Text.Equals("All", StringComparison.InvariantCultureIgnoreCase))
            {
                sulsearch = "";

                iulpage = 1;
                iulp = 0;

                ULTOption();

                ULLoad(iulp);

                ULRCDefault();
            }

            else
            {
                sulsearch = " WHERE Username LIKE '"+ txtULSearch.Text.Replace("'", "''") + "'";

                iulpage = 1;
                iulp = 0;

                ULTOption();

                ULLoad(iulp);

                ULRCDefault();
            }

            EDButtons();
        }
        #endregion

        #region Order Option
        private void cmbULOOption_DropDown(object sender, EventArgs e)
        {
            cmbULOOption.Items.Remove("Order Option");
            cmbULOOption.ForeColor = Color.Black;
        }

        private void cmbULOOption_DropDownClosed(object sender, EventArgs e)
        {
            if (cmbULOOption.SelectedItem == null)
            {
                if (!cmbULOOption.Items.Contains("Order Option"))
                {
                    cmbULOOption.Items.Insert(0, "Order Option");
                }

                cmbULOOption.Text = "Order Option";
                cmbULOOption.ForeColor = Color.DarkGray;
            }
        }

        private void cmbULOOption_SelectedIndexChanged(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;

            if (cmbULOOption.ForeColor != Color.DarkGray && cmbULOOption.Text != "Order Option")
            {
                ULOOption();

                ULLoad(iulp);
            }

            Cursor.Current = Cursors.Default;
        }

        void ULOOption()
        {
            if (cmbULOOption.Text == "Activity")
            {
                sulorder = " ORDER BY Activity";
            }

            else
            {
                sulorder = " ORDER BY No_ID";
            }
        }
        #endregion

        #region Time Option
        private void cmbULTOption_DropDown(object sender, EventArgs e)
        {
            cmbULTOption.Items.Remove("Time Option");
            cmbULTOption.ForeColor = Color.Black;
        }

        private void cmbULTOption_DropDownClosed(object sender, EventArgs e)
        {
            if (cmbULTOption.SelectedItem == null)
            {
                if (!cmbULTOption.Items.Contains("Time Option"))
                {
                    cmbULTOption.Items.Insert(0, "Time Option");
                }

                cmbULTOption.Text = "Time Option";
                cmbULTOption.ForeColor = Color.DarkGray;
            }
        }

        private void cmbULTOption_SelectedIndexChanged(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;

            if (cmbULTOption.ForeColor != Color.DarkGray && cmbULTOption.Text != "Time Option")
            {
                iulpage = 1;
                iulp = 0;

                ULTOption();

                ULLoad(iulp);

                ULRCDefault();
            }

            Cursor.Current = Cursors.Default;
        }

        private void dtpULDate_ValueChanged(object sender, EventArgs e)
        {
            cmbULTOption_SelectedIndexChanged(sender, e);
        }

        void ULTOption()
        {
            if (cmbULTOption.Text == "Specific Date")
            {
                dtpULDate.Enabled = true;

                if (sulsearch == "")
                {
                    suldate = " WHERE DATE_FORMAT(DATE(Time), '%m-%d-%Y') = '" + dtpULDate.Text + "'";
                }

                else
                {
                    suldate = " AND DATE_FORMAT(DATE(Time), '%m-%d-%Y') = '" + dtpULDate.Text + "'";
                }
            }

            else
            {
                dtpULDate.Enabled = false;

                suldate = "";
            }
        }
        #endregion

        #region Auto Complete Search
        void ULSearch()
        {
            AutoCompleteStringCollection acs = new AutoCompleteStringCollection();

            string cQuery = "SELECT Username FROM tbl_user;";
            MySqlConnection cConnection = new MySqlConnection(Conn.uString);
            MySqlCommand cCommand = new MySqlCommand(cQuery, cConnection);
            MySqlDataReader cReader;

            try
            {
                cConnection.Open();
                cReader = cCommand.ExecuteReader();

                acs.Clear();

                while (cReader.Read())
                {
                    string unm;

                    unm = cReader.GetString(0);
                    acs.Add(unm);
                    acs.Add("All");
                }
            }

            catch (MySqlException ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            finally
            {
                cConnection.Close();
            }

            txtULSearch.AutoCompleteCustomSource = acs;
        }
        #endregion

        #region Enable Tooltip
        private void txtULSearch_ForeColorChanged(object sender, EventArgs e)
        {
            TTip();
        }

        private void cmbULOOption_ForeColorChanged(object sender, EventArgs e)
        {
            TTip();
        }

        private void cmbULTOption_FontChanged(object sender, EventArgs e)
        {
            TTip();
        }
        #endregion

        #endregion

        #region Manage Drop Down
        string ulddstatus = "*Close";

        void ULDOpen()
        {
            if (ulddstatus == "*Open")
            {
                btnULManage.BackColor = Color.LimeGreen;
                bulogs = true;
                flpULDDown.Visible = true;
            }
        }

        void ULDClose()
        {
            bulogs = false;
            thide.Start();
        }

        private void btnULManage_MouseEnter(object sender, EventArgs e)
        {
            if (ulddstatus == "*Open")
            {
                btnULManage.Text = "  Manage   ▼";
                bulogs = true;
                flpULDDown.Visible = true;
            }
        }

        private void btnULManage_MouseLeave(object sender, EventArgs e)
        {
            bulogs = false;
            thide.Start();
        }

        private void btnULAStaff_MouseEnter(object sender, EventArgs e)
        {
            ULDOpen();
        }

        private void btnULAStaff_MouseLeave(object sender, EventArgs e)
        {
            ULDClose();
        }

        private void btnULDelete_MouseEnter(object sender, EventArgs e)
        {
            ULDOpen();
        }

        private void btnULDelete_MouseLeave(object sender, EventArgs e)
        {
            ULDClose();
        }

        private void btnULChange_MouseEnter(object sender, EventArgs e)
        {
            ULDOpen();
        }

        private void btnULChange_MouseLeave(object sender, EventArgs e)
        {
            ULDClose();
        }
        #endregion

        #region Change Navigation
        void ULCDefault()
        {
            flpULAStaff.Visible = false;
            flpULDelete.Visible = false;
            flpULChange.Visible = false;
        }

        void ULNBDefault()
        {
            btnULAStaff.BackColor = Color.FromArgb(64, 64, 64); ;
            btnULAStaff.FlatAppearance.MouseOverBackColor = Color.LimeGreen;
            btnULAStaff.FlatAppearance.MouseDownBackColor = Color.ForestGreen;
            btnULAStaff.Cursor = Cursors.Hand;

            btnULDelete.BackColor = Color.FromArgb(64, 64, 64); ;
            btnULDelete.FlatAppearance.MouseOverBackColor = Color.LimeGreen;
            btnULDelete.FlatAppearance.MouseDownBackColor = Color.ForestGreen;
            btnULDelete.Cursor = Cursors.Hand;

            btnULChange.BackColor = Color.FromArgb(64, 64, 64); ;
            btnULChange.FlatAppearance.MouseOverBackColor = Color.LimeGreen;
            btnULChange.FlatAppearance.MouseDownBackColor = Color.ForestGreen;
            btnULChange.Cursor = Cursors.Hand;
        }

        private void btnULManage_Click(object sender, EventArgs e)
        {
            if (ulddstatus == "*Close")
            {
                ulddstatus = "*Open";

                btnULManage.Text = "  Manage   ▼";
                bulogs = true;
                flpULDDown.Visible = true;
            }

            else
            {
                ulddstatus = "*Close";

                bulogs = false;
                thide.Start();
            }
        }

        private void btnULAStaff_Click(object sender, EventArgs e)
        {
            ULNBDefault();

            btnULAStaff.BackColor = Color.LimeGreen;
            btnULAStaff.FlatAppearance.MouseOverBackColor = Color.LimeGreen;
            btnULAStaff.FlatAppearance.MouseDownBackColor = Color.LimeGreen;
            btnULAStaff.Cursor = Cursors.Default;

            ULCDefault();

            flpULAStaff.Visible = true;
        }

        private void btnULDelete_Click(object sender, EventArgs e)
        {
            ULNBDefault();

            btnULDelete.BackColor = Color.LimeGreen;
            btnULDelete.FlatAppearance.MouseOverBackColor = Color.LimeGreen;
            btnULDelete.FlatAppearance.MouseDownBackColor = Color.LimeGreen;
            btnULDelete.Cursor = Cursors.Default;

            ULCDefault();

            flpULDelete.Visible = true;
        }

        private void btnULChange_Click(object sender, EventArgs e)
        {
            ULNBDefault();

            btnULChange.BackColor = Color.LimeGreen;
            btnULChange.FlatAppearance.MouseOverBackColor = Color.LimeGreen;
            btnULChange.FlatAppearance.MouseDownBackColor = Color.LimeGreen;
            btnULChange.Cursor = Cursors.Default;

            ULCDefault();

            flpULChange.Visible = true;
        }
        #endregion

        #region Pagination
        void ULPage()
        {
            dulpage = dulcount / iuldpage;
            dultpage = Math.Truncate(dulpage);
            sulpage = Convert.ToString(dulpage);

            if (sulpage.Contains("."))
            {
                dulrpage = dultpage + 1;
            }

            else
            {
                dulrpage = dultpage;
            }
        }
        #endregion

        #region Row Count Default
        void ULRCDefault()
        {
            ULPage();

            if (iulpage == 1 && dulrpage <= 1)
            {
                btnULFirst.Enabled = false;
                btnULBack.Enabled = false;
                btnULNext.Enabled = false;
                btnULLast.Enabled = false;
            }

            else if (iulpage == 1 && dulrpage > 1)
            {
                btnULFirst.Enabled = false;
                btnULBack.Enabled = false;
                btnULNext.Enabled = true;
                btnULLast.Enabled = true;
            }

            else if (iulpage > 1 && iulpage < dulrpage)
            {
                btnULFirst.Enabled = true;
                btnULBack.Enabled = true;
                btnULNext.Enabled = true;
                btnULLast.Enabled = true;
            }

            else if (iulpage == dulrpage)
            {
                btnULFirst.Enabled = true;
                btnULBack.Enabled = true;
                btnULNext.Enabled = false;
                btnULLast.Enabled = false;
            }

            EDButtons();
        }
        #endregion

        #region First
        private void btnULFirst_Click(object sender, EventArgs e)
        {
            iulpage = 1;
            iulp = 0;

            ULLoad(iulp);

            ULRCDefault();

            dgvULogs.Focus();
        }
        #endregion

        #region Back
        private void btnULBack_Click(object sender, EventArgs e)
        {
            ULPage();

            iulpage--;

            if (iulpage == 1)
            {
                btnULFirst.Enabled = false;
                btnULBack.Enabled = false;
            }

            if (iulpage < dulrpage)
            {
                btnULNext.Enabled = true;
                btnULLast.Enabled = true;
            }

            iulp = iulp - iuldpage;
            ULLoad(iulp);

            EDButtons();

            dgvULogs.Focus();
        }
        #endregion

        #region Next
        private void btnULNext_Click(object sender, EventArgs e)
        {
            ULPage();

            iulpage++;

            if (iulpage == dulrpage)
            {
                btnULNext.Enabled = false;
                btnULLast.Enabled = false;
            }

            if (iulpage > 1)
            {
                btnULFirst.Enabled = true;
                btnULBack.Enabled = true;
            }

            iulp = iulp + iuldpage;
            ULLoad(iulp);

            EDButtons();

            dgvULogs.Focus();
        }
        #endregion

        #region Last
        private void btnULLast_Click(object sender, EventArgs e)
        {
            ULPage();

            iulp = Convert.ToInt32((dulrpage * iuldpage) - iuldpage);
            iulpage = Convert.ToInt32(dulrpage);

            ULLoad(iulp);

            btnULFirst.Enabled = true;
            btnULBack.Enabled = true;
            btnULNext.Enabled = false;
            btnULLast.Enabled = false;

            EDButtons();

            dgvULogs.Focus();
        }
        #endregion

        #region All Clear
        void ULClear()
        {
            txtULAUsername.Clear();
            txtULAPassword.Clear();
            txtULACPassword.Clear();
            txtULAAUsername.Clear();
            txtULAAPassword.Clear();

            txtULDUsername.Clear();
            txtULDAUsername.Clear();
            txtULDAPassword.Clear();

            txtULCUsername.Clear();
            txtULCNUsername.Clear();
            txtULCNPassword.Clear();
            txtULCCPassword.Clear();
            txtULCAUsername.Clear();
            txtULCAPassword.Clear();
        }
        #endregion

        #region Add Staff
        private void btnULPSAdd_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;

            if (txtULAUsername.Text == "")
            {
                MessageBox.Show("Username field is empty.", "Empty Field", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtULAUsername.Focus();
            }

            else if(txtULAPassword.Text == "")
            {
                MessageBox.Show("Password field is empty.", "Empty Field", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtULAPassword.Focus();
            }

            else if(txtULACPassword.Text == "")
            {
                MessageBox.Show("Confirm Password field is empty.", "Empty Field", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtULACPassword.Focus();
            }

            else if(txtULAAUsername.Text == "")
            {
                MessageBox.Show("Admin Username field is empty.", "Empty Field", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtULAAUsername.Focus();
            }

            else if(txtULAAPassword.Text == "")
            {
                MessageBox.Show("Admin Password field is empty.", "Empty Field", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtULAAPassword.Focus();
            }

            else if (txtULAPassword.Text != txtULACPassword.Text)
            {
                MessageBox.Show("Confirm Password not match.", "Not Match", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtULACPassword.Focus();
            }

            else
            {
                string unm = "", pwd = "";

                string cQuery = "SELECT Username, Password FROM tbl_user WHERE No_ID = 1;";
                MySqlConnection cConnection = new MySqlConnection(Conn.uString);
                MySqlCommand cCommand = new MySqlCommand(cQuery, cConnection);
                MySqlDataReader cReader;

                try
                {
                    cConnection.Open();
                    cReader = cCommand.ExecuteReader();
                    while (cReader.Read())
                    {
                        unm = cReader.GetValue(0).ToString();
                        pwd = cReader.GetValue(1).ToString();
                    }
                }

                catch (MySqlException ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                finally
                {
                    cConnection.Close();
                }

                if(txtULAAUsername.Text != unm || txtULAAPassword.Text != pwd)
                {
                    MessageBox.Show("Invalid Admin Username or Password.", "Invalid Admin Access", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtULAAUsername.Focus();
                }

                else
                {
                    string cQueryI = "INSERT INTO tbl_user(Username, Password, User_Type, Status)"
                        + " VALUES('" + txtULAUsername.Text + "', '" + txtULAPassword.Text + "', 'Staff', 'Not Connected');";
                    MySqlConnection cConnectionI = new MySqlConnection(Conn.uString);
                    MySqlCommand cCommandI = new MySqlCommand(cQueryI, cConnectionI);
                    MySqlDataReader cReaderI;

                    try
                    {
                        cConnectionI.Open();
                        cReaderI = cCommandI.ExecuteReader();
                        while (cReaderI.Read()) { }

                        ULClear();
                        txtULAUsername.Focus();
                        ULSearch();

                        MessageBox.Show("New staff account " + txtULAUsername.Text + " has been saved.", "Add Staff", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }

                    catch (MySqlException ex)
                    {
                        if (ex.Message.Contains("Duplicate entry"))
                        {
                            MessageBox.Show("Username already exist.", "Already Exist", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            txtULAUsername.Focus();
                        }

                        else
                        {
                            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                        }
                    }

                    finally
                    {
                        cConnectionI.Close();
                    }
                }
            }

            Cursor.Current = Cursors.Default;
        }
        #endregion

        #region Delete Staff
        private void btnULDDelete_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;

            string utype = "";

            string cQuery = "SELECT User_Type FROM tbl_user WHERE Username = '" + txtULDUsername.Text + "';";
            MySqlConnection cConnection = new MySqlConnection(Conn.uString);
            MySqlCommand cCommand = new MySqlCommand(cQuery, cConnection);
            MySqlDataReader cReader;

            try
            {
                cConnection.Open();
                cReader = cCommand.ExecuteReader();
                while (cReader.Read())
                {
                    utype = cReader.GetValue(0).ToString();
                }
            }

            catch (MySqlException ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            finally
            {
                cConnection.Close();
            }

            if (txtULDUsername.Text == "")
            {
                MessageBox.Show("Username field is empty.", "Empty Field", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtULDUsername.Focus();
            }

            else if (txtULDAUsername.Text == "")
            {
                MessageBox.Show("Admin Username field is empty.", "Empty Field", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtULDAUsername.Focus();
            }

            else if (txtULDAPassword.Text == "")
            {
                MessageBox.Show("Admin Password field is empty.", "Empty Field", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtULDAPassword.Focus();
            }

            else if (utype != "Staff")
            {
                MessageBox.Show("None staff user cannot be deleted.", "None Staff", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtULDAPassword.Focus();
            }

            else
            {
                string unm = "", pwd = "", iunm = "", cdct = "";
                double z = 0;

                string cQueryI = "SELECT Username, Password FROM tbl_user WHERE No_ID = 1;";
                MySqlConnection cConnectionI = new MySqlConnection(Conn.uString);
                MySqlCommand cCommandI = new MySqlCommand(cQueryI, cConnectionI);
                MySqlDataReader cReaderI;

                try
                {
                    cConnectionI.Open();
                    cReaderI = cCommandI.ExecuteReader();
                    while (cReaderI.Read())
                    {
                        unm = cReaderI.GetValue(0).ToString();
                        pwd = cReaderI.GetValue(1).ToString();
                    }
                }

                catch (MySqlException ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                finally
                {
                    cConnectionI.Close();
                }

                string cQueryII = "SELECT Username FROM tbl_user WHERE Username = '" + txtULDUsername.Text + "';";
                MySqlConnection cConnectionII = new MySqlConnection(Conn.uString);
                MySqlCommand cCommandII = new MySqlCommand(cQueryII, cConnectionII);
                MySqlDataReader cReaderII;

                try
                {
                    cConnectionII.Open();
                    cReaderII = cCommandII.ExecuteReader();
                    while (cReaderII.Read())
                    {
                        iunm = cReaderII.GetValue(0).ToString();
                    }
                }

                catch (MySqlException ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                finally
                {
                    cConnectionII.Close();
                }

                string cQueryIII = "SELECT COUNT(*) FROM tbl_clients WHERE Username = '" + txtULDUsername.Text + "';";
                MySqlConnection cConnectionIII = new MySqlConnection(Conn.cString);
                MySqlCommand cCommandIII = new MySqlCommand(cQueryIII, cConnectionIII);
                MySqlDataReader cReaderIII;

                try
                {
                    cConnectionIII.Open();
                    cReaderIII = cCommandIII.ExecuteReader();
                    while (cReaderIII.Read())
                    {
                        cdct = cReaderIII.GetValue(0).ToString();
                        z = Convert.ToInt32(cdct);
                    }
                }

                catch (MySqlException ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                finally
                {
                    cConnectionIII.Close();
                }

                if (iunm == "")
                {
                    MessageBox.Show("Username not exist.", "Not Exist", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtULDUsername.Focus();
                }

                else if (txtULDAUsername.Text != unm || txtULDAPassword.Text != pwd)
                {
                    MessageBox.Show("Invalid Admin Username or Password.", "Invalid Admin Access", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtULDAUsername.Focus();
                }

                else if (z != 0)
                {
                    MessageBox.Show("There are clients under this user. Transfer it first to other user.", "Transfer First", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                else
                {
                    DialogResult dr = MessageBox.Show("Are you sure you want to delete this staff account?", "Delete Staff", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                    if (dr == DialogResult.No) { }

                    else
                    {
                        string cQueryIV = "DELETE FROM tbl_user WHERE Username = '" + txtULDUsername.Text + "';";
                        MySqlConnection cConnectionIV = new MySqlConnection(Conn.uString);
                        MySqlCommand cCommandIV = new MySqlCommand(cQueryIV, cConnectionIV);
                        MySqlDataReader cReaderIV;

                        try
                        {
                            cConnectionIV.Open();
                            cReaderIV = cCommandIV.ExecuteReader();
                            while (cReaderIV.Read()) { }

                            ULClear();
                            txtULDUsername.Focus();
                            ULSearch();

                            MessageBox.Show("Staff account " + txtULDUsername.Text + " has been deleted.", "Delete Staff", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }

                        catch (MySqlException ex)
                        {
                            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }

                        finally
                        {
                            cConnectionIV.Close();
                        }
                    }
                }
            }

            Cursor.Current = Cursors.Default;
        }
        #endregion

        #region Change
        private void btnChange_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;

            if (txtULCUsername.Text == "")
            {
                MessageBox.Show("Username field is empty.", "Empty Field", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtULCUsername.Focus();
            }

            else if (txtULCNPassword.Text != "" && txtULCCPassword.Text == "")
            {
                MessageBox.Show("Confirm Password field is empty.", "Empty Field", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtULCCPassword.Focus();
            }

            else if (txtULCNPassword.Text == "" && txtULCCPassword.Text != "")
            {
                MessageBox.Show("New Password field is empty.", "Empty Field", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtULCNPassword.Focus();
            }

            else if (txtULCAUsername.Text == "")
            {
                MessageBox.Show("Admin Username field is empty.", "Empty Field", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtULCAUsername.Focus();
            }

            else if (txtULCAPassword.Text == "")
            {
                MessageBox.Show("Admin Password field is empty.", "Empty Field", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtULCAPassword.Focus();
            }

            else if (txtULCNPassword.Text != txtULCCPassword.Text)
            {
                MessageBox.Show("Confirm Password not match.", "Not Match", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtULCCPassword.Focus();
            }

            else if (txtULCNUsername.Text == "" && txtULCNPassword.Text == "" && txtULCCPassword.Text == "")
            {
                MessageBox.Show("Fill the field you want to change.", "Fill Field", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtULCNUsername.Focus();
            }

            else
            {
                string unm = "", pwd = "", iunm = "", ipwd = "";

                string cQuery = "SELECT Username, Password FROM tbl_user WHERE No_ID = 1;";
                MySqlConnection cConnection = new MySqlConnection(Conn.uString);
                MySqlCommand cCommand = new MySqlCommand(cQuery, cConnection);
                MySqlDataReader cReader;

                try
                {
                    cConnection.Open();
                    cReader = cCommand.ExecuteReader();
                    while (cReader.Read())
                    {
                        unm = cReader.GetValue(0).ToString();
                        pwd = cReader.GetValue(1).ToString();
                    }
                }

                catch (MySqlException ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                finally
                {
                    cConnection.Close();
                }

                string cQueryI = "SELECT Username, Password FROM tbl_user WHERE Username = '" + txtULCUsername.Text + "';";
                MySqlConnection cConnectionI = new MySqlConnection(Conn.uString);
                MySqlCommand cCommandI = new MySqlCommand(cQueryI, cConnectionI);
                MySqlDataReader cReaderI;

                try
                {
                    cConnectionI.Open();
                    cReaderI = cCommandI.ExecuteReader();
                    while (cReaderI.Read())
                    {
                        iunm = cReaderI.GetValue(0).ToString();
                        ipwd = cReaderI.GetValue(1).ToString();
                    }
                }

                catch (MySqlException ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                finally
                {
                    cConnectionI.Close();
                }

                if (iunm == "")
                {
                    MessageBox.Show("Username not exist.", "Not Exist", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtULCUsername.Focus();
                }

                else if (txtULCAUsername.Text != unm || txtULCAPassword.Text != pwd)
                {
                    MessageBox.Show("Invalid Admin Username or Password.", "Invalid Admin Access", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtULCAUsername.Focus();
                }

                else
                {
                    string uname = "", pword = "";

                    uname = txtULCNUsername.Text;
                    pword = txtULCNPassword.Text;

                    if(txtULCNUsername.Text == "")
                    {
                        uname = iunm;
                    }

                    if(txtULCNPassword.Text == "")
                    {
                        pword = ipwd;
                    }

                    string cQueryII = "UPDATE tbl_user SET Username = '" + uname + "', Password = '" + pword + "' WHERE Username = '" + txtULCUsername.Text + "';";
                    MySqlConnection cConnectionII = new MySqlConnection(Conn.uString);
                    MySqlCommand cCommandII = new MySqlCommand(cQueryII, cConnectionII);
                    MySqlDataReader cReaderII;

                    try
                    {
                        cConnectionII.Open();
                        cReaderII = cCommandII.ExecuteReader();
                        while (cReaderII.Read()) { }

                        ULClear();
                        txtULCUsername.Focus();

                        MessageBox.Show("Account " + txtULCUsername.Text + " information has been change.", "Change Username or Password", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }

                    catch (MySqlException ex)
                    {
                        if (ex.Message.Contains("Duplicate entry"))
                        {
                            MessageBox.Show("Username already exist.", "Already Exist", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            txtULAUsername.Focus();
                        }

                        else
                        {
                            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                        }
                    }

                    finally
                    {
                        cConnectionII.Close();
                    }
                }
            }

            Cursor.Current = Cursors.Default;
        }
        #endregion

        #region User
        private void btnULMUser_Click(object sender, EventArgs e)
        {
            User w = new User();
            w.ShowDialog();
        }
        #endregion

        #region Start Socket
        void SScocket()
        {
            string cQuery = "SELECT No_ID, IP_Address FROM tbl_user WHERE No_ID = 1 OR No_ID = 2;";
            MySqlConnection cConnection = new MySqlConnection(Conn.uString);
            MySqlCommand cCommand = new MySqlCommand(cQuery, cConnection);
            MySqlDataReader cReader;

            try
            {
                cConnection.Open();
                cReader = cCommand.ExecuteReader();
                while (cReader.Read())
                {
                    psCPort = cReader.GetValue(0).ToString();
                    psCIP = cReader.GetValue(1).ToString();
                }
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            finally
            {
                cConnection.Close();
            }

            try
            {
                sck = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                sck.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);

                epHost = new IPEndPoint(IPAddress.Parse(psHIP), Convert.ToInt32(psHPort));
                sck.Bind(epHost);

                epClient = new IPEndPoint(IPAddress.Parse(psCIP), Convert.ToInt32(psCPort));
                sck.Connect(epClient);

                byte[] buffer = new byte[1500];
                sck.BeginReceiveFrom(buffer, 0, buffer.Length, SocketFlags.None, ref epClient, new AsyncCallback(RDCMessage), buffer);
            }

            catch (Exception) { }
        }
        #endregion

        #region Receive DC Message
        public void RDCMessage(IAsyncResult aResult)
        {
            try
            {
                int ssize = sck.EndReceiveFrom(aResult, ref epClient);

                if (ssize > 0)
                {
                    byte[] bfr = new byte[1500];
                    bfr = (byte[])aResult.AsyncState;
                    ASCIIEncoding aEncode = new ASCIIEncoding();
                    string rmsg = aEncode.GetString(bfr);

                    RLogout();
                    Application.Exit();
                }

            }

            catch (Exception) { }
        }
        #endregion

        #region Refresh
        private void btnULRefresh_Click(object sender, EventArgs e)
        {
            ULClear();
            ULLoad(iulp);
        }
        #endregion

        //----------------------------------------------------------------------// Certificate

        #region Load Table
        string scsearch = "", scs = "", scorder = "", scdrange = "", scdfrom = "", scdto = "", scpage = "";
        int icp = 0, icpage = 1, icdpage = tcontent;
        double dccount = 0, dcpage = 0, dctpage = 0, dcrpage = 0;

        void CLoad(int p)
        {
            string cQueryI = "SELECT COUNT(*) FROM tbl_cert" + scsearch + scdrange + ";";
            MySqlConnection cConnectionI = new MySqlConnection(Conn.cString);
            MySqlCommand cCommandI = new MySqlCommand(cQueryI, cConnectionI);
            MySqlDataReader cReaderI;

            try
            {
                cConnectionI.Open();
                cReaderI = cCommandI.ExecuteReader();
                while (cReaderI.Read())
                {
                    dccount = cReaderI.GetDouble(0);
                    txtCCount.Text = Convert.ToString(dccount);
                }
            }

            catch (MySqlException ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            finally
            {
                cConnectionI.Close();
            }

            MySqlConnection cConn = new MySqlConnection(Conn.cString);
            MySqlCommand cCommand = new MySqlCommand("SELECT No_ID,"
                + " DATE_FORMAT(DATE(Request_Date), '%m-%d-%Y') AS 'Request Date',"
                + " Control_Number AS 'Control Number',"
                + " Client_Name AS 'Client Name',"
                + " Address,"
                + " Report_Date_and_Year AS 'Report Date and Year',"
                + " Purpose,"
                + " CPA_Name AS 'CPA Name',"
                + " CPA_Cert_Number AS 'CPA Certificate Number',"
                + " PRC_BOA_Number AS 'PRC / BOA Number',"
                + " CONCAT(DATE_FORMAT(DATE(PRC_BOA_Valid_From), '%m-%d-%Y'), ' to ', DATE_FORMAT(DATE(PRC_BOA_Valid_To), '%m-%d-%Y')) AS 'PRC / BOA Validity Date',"
                + " SEC_Number AS 'SEC Number',"
                + " CONCAT(DATE_FORMAT(DATE(SEC_Valid_From), '%m-%d-%Y'), ' to ', DATE_FORMAT(DATE(SEC_Valid_To), '%m-%d-%Y')) AS 'SEC Validity Date',"
                + " CDA_CEA_Number AS 'CDA CEA number',"
                + " CONCAT(DATE_FORMAT(DATE(CDA_CEA_Valid_From), '%m-%d-%Y'), ' to ', DATE_FORMAT(DATE(CDA_CEA_Valid_To), '%m-%d-%Y')) AS 'CDA CEA Validity Date',"
                + " BIR_Number AS 'BIR Number',"
                + " CONCAT(DATE_FORMAT(DATE(BIR_Valid_From), '%m-%d-%Y'), ' to ', DATE_FORMAT(DATE(BIR_Valid_To), '%m-%d-%Y')) AS 'BIR Validity Date',"
                + " BSP_Accreditation_Year AS 'BSP Accreditation Year',"
                + " TIN,"
                + " PTR_Number AS 'PTR Number',"
                + " CONCAT(DATE_FORMAT(DATE(PTR_Valid_From), '%m-%d-%Y'), ' to ', DATE_FORMAT(DATE(PTR_Valid_To), '%m-%d-%Y')) AS 'PTR Validity Date',"
                + " PRC_BOA_Valid_From, PRC_BOA_Valid_To,"
                + " SEC_Valid_From, SEC_Valid_To,"
                + " CDA_CEA_Valid_From, CDA_CEA_Valid_To,"
                + " BIR_Valid_From, BIR_Valid_To,"
                + " PTR_Valid_From, PTR_Valid_To"
                + " FROM tbl_cert" + scsearch + scdrange + scorder + ";", cConn);

            try
            {
                MySqlDataAdapter cAdapter = new MySqlDataAdapter();
                cAdapter.SelectCommand = cCommand;
                cDatasetC = new DataTable();
                cAdapter.Fill(p, icdpage, cDatasetC);
                BindingSource cSource = new BindingSource();
                cSource.DataSource = cDatasetC;
                dgvCert.DataSource = cSource;
                cAdapter.Update(cDatasetC);
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            dgvCert.Columns[0].Visible = false;
            dgvCert.Columns[21].Visible = false;
            dgvCert.Columns[22].Visible = false;
            dgvCert.Columns[23].Visible = false;
            dgvCert.Columns[24].Visible = false;
            dgvCert.Columns[25].Visible = false;
            dgvCert.Columns[26].Visible = false;
            dgvCert.Columns[27].Visible = false;
            dgvCert.Columns[28].Visible = false;
            dgvCert.Columns[29].Visible = false;
            dgvCert.Columns[30].Visible = false;
        }
        #endregion

        #region Bottom Buttons Default
        void CBDefault()
        {
            if (dgvCert.RowCount == 0)
            {
                btnCDelete.Enabled = false;
                btnCRPrint.Enabled = false;
            }

            else
            {
                btnCDelete.Enabled = true;
                btnCRPrint.Enabled = true;
            }
        }
        #endregion

        #region Search

        #region Search Button
        private void btnCNSearch_MouseEnter(object sender, EventArgs e)
        {
            btnCNSearch.Visible = false;
            btnCHSearch.Visible = true;
        }

        private void btnCHSearch_MouseLeave(object sender, EventArgs e)
        {
            btnCNSearch.Visible = true;
            btnCHSearch.Visible = false;
        }

        private void btnCHSearch_MouseDown(object sender, MouseEventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;

            btnCHSearch.Visible = false;
            btnCDSearch.Visible = true;

            CTSearch();

            cmbCSOption_SelectedIndexChanged(sender, e);

            Cursor.Current = Cursors.Default;
        }

        private void btnCDSearch_MouseUp(object sender, MouseEventArgs e)
        {
            btnCHSearch.Visible = true;
            btnCDSearch.Visible = false;
        }

        private void btnCDSearch_MouseLeave(object sender, EventArgs e)
        {
            btnCHSearch.Visible = true;
            btnCDSearch.Visible = false;
        }
        #endregion

        #region Search Shortcut Key
        private void txtCSearch_KeyDown(object sender, KeyEventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;

            if (txtCSearch.Focused && e.KeyCode == Keys.Enter)
            {
                CTSearch();

                cmbCSOption_SelectedIndexChanged(sender, e);
            }

            Cursor.Current = Cursors.Default;
        }
        #endregion

        #region Search On or Off Focus
        private void txtCSearch_Enter(object sender, EventArgs e)
        {
            if (txtCSearch.ForeColor == Color.DarkGray)
            {
                txtCSearch.Clear();
                txtCSearch.ForeColor = Color.Black;
            }
        }

        private void txtCSearch_Leave(object sender, EventArgs e)
        {
            if (txtCSearch.ForeColor == Color.Black && txtCSearch.Text == "")
            {
                txtCSearch.Text = "Search";
                txtCSearch.ForeColor = Color.DarkGray;
            }
        }
        #endregion

        #region Table Search
        void CTSearch()
        {
            if (txtCSearch.Text == "" || txtCSearch.ForeColor == Color.DarkGray) { }

            else if (txtCSearch.Text.Equals("All", StringComparison.InvariantCultureIgnoreCase) && cmbCSOption.Text == "All")
            {
                icpage = 1;
                icp = 0;

                scsearch = "";

                CFOption();

                CLoad(icp);

                CRCDefault();
                
                CBDefault();
            }

            else
            {
                icpage = 1;
                icp = 0;

                CSOption();

                CFOption();

                CLoad(icp);

                CRCDefault();

                CBDefault();
            }

            EDButtons();
        }
        #endregion

        #region Search Option
        private void cmbCSOption_DropDown(object sender, EventArgs e)
        {
            cmbCSOption.Items.Remove("Search Option");
            cmbCSOption.ForeColor = Color.Black;
        }

        private void cmbCSOption_DropDownClosed(object sender, EventArgs e)
        {
            if (cmbCSOption.SelectedItem == null)
            {
                if (!cmbCSOption.Items.Contains("Search Option"))
                {
                    cmbCSOption.Items.Insert(0, "Search Option");
                }

                cmbCSOption.Text = "Search Option";
                cmbCSOption.ForeColor = Color.DarkGray;
            }
        }

        private void cmbCSOption_SelectedIndexChanged(object sender, EventArgs e)
        {
            CSearch();
        }

        void CSOption()
        {
            scs = txtCSearch.Text.Replace("'", "''");

            if (cmbCSOption.Text == "Client Name")
            {
                scsearch = " WHERE Client_Name LIKE '" + scs + "'";
            }

            else if (cmbCSOption.Text == "CPA Name")
            {
                scsearch = " WHERE CPA_Name LIKE '" + scs + "'";
            }

            else
            {
                scsearch = " WHERE Control_Number LIKE '" + scs + "'";
            }
        }
        #endregion

        #region Order Option
        private void cmbCOOption_DropDown(object sender, EventArgs e)
        {
            cmbCOOption.Items.Remove("Order Option");
            cmbCOOption.ForeColor = Color.Black;
        }

        private void cmbCOOption_DropDownClosed(object sender, EventArgs e)
        {
            if (cmbCOOption.SelectedItem == null)
            {
                if (!cmbCOOption.Items.Contains("Order Option"))
                {
                    cmbCOOption.Items.Insert(0, "Order Option");
                }

                cmbCOOption.Text = "Order Option";
                cmbCOOption.ForeColor = Color.DarkGray;
            }
        }

        private void cmbCOOption_SelectedIndexChanged(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;

            if (cmbCOOption.ForeColor != Color.DarkGray && cmbCOOption.Text != "Order Option")
            {
                COOption();
                CLoad(icp);
            }

            Cursor.Current = Cursors.Default;
        }

        void COOption()
        {
            if (cmbCOOption.Text == "Client Name")
            {
                scorder = " ORDER BY Client_Name";
            }

            else if (cmbCOOption.Text == "CPA Name")
            {
                scorder = " ORDER BY CPA_Name";
            }

            else if (cmbCOOption.Text == "Frequency")
            {
                scorder = " ORDER BY DATE(Request_Date);";
            }

            else
            {
                scorder = " ORDER BY Control_Number";
            }
        }
        #endregion

        #region Frequency Option
        private void cmbCFOption_DropDown(object sender, EventArgs e)
        {
            cmbCFOption.Items.Remove("Frequency Option");
            cmbCFOption.ForeColor = Color.Black;
        }

        private void cmbCFOption_DropDownClosed(object sender, EventArgs e)
        {
            if (cmbCFOption.SelectedItem == null)
            {
                if (!cmbCFOption.Items.Contains("Frequency Option"))
                {
                    cmbCFOption.Items.Insert(0, "Frequency Option");
                }

                cmbCFOption.Text = "Frequency Option";
                cmbCFOption.ForeColor = Color.DarkGray;
            }
        }

        private void cmbCFOption_SelectedIndexChanged(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;

            CFOption();

            icpage = 1;
            icp = 0;

            CLoad(icp);

            CRCDefault();

            CBDefault();

            EDButtons();

            Cursor.Current = Cursors.Default;
        }

        private void dtpCFrom_ValueChanged(object sender, EventArgs e)
        {
            cmbCFOption_SelectedIndexChanged(sender, e);
        }

        private void dtpCTo_ValueChanged(object sender, EventArgs e)
        {
            cmbCFOption_SelectedIndexChanged(sender, e);
        }

        void CFOption()
        {
            if (cmbCFOption.Text == "Daily")
            {
                dtpCFrom.Enabled = true;
                dtpCTo.Enabled = true;

                dtpCFrom.CustomFormat = "MM-dd-yyyy";
                dtpCTo.CustomFormat = "MM-dd-yyyy";

                dtpCFrom.ShowUpDown = false;
                dtpCTo.ShowUpDown = false;

                DateTime dfrom, dto;

                dfrom = dtpCFrom.Value;
                dto = dtpCTo.Value;

                scdfrom = dfrom.ToString("yyyy-MM-dd");
                scdto = dto.ToString("yyyy-MM-dd");

                psCFDate = dfrom.ToString("MM-dd-yyyy");
                psCTDate = dto.ToString("MM-dd-yyyy");

                if (scsearch == "")
                {
                    scdrange = " WHERE DATE(Request_Date) BETWEEN '" + scdfrom + "' AND '" + scdto + "'";
                }

                else
                {
                    scdrange = " AND DATE(Request_Date) BETWEEN '" + scdfrom + "' AND '" + scdto + "'";
                }
            }

            else if (cmbCFOption.Text == "Monthly")
            {
                dtpCFrom.Enabled = true;
                dtpCTo.Enabled = true;

                dtpCFrom.CustomFormat = "MM-yyyy";
                dtpCTo.CustomFormat = "MM-yyyy";

                dtpCFrom.ShowUpDown = true;
                dtpCTo.ShowUpDown = true;

                DateTime dfrom, dto;
                int m, y;

                dfrom = dtpCFrom.Value;
                dto = dtpCTo.Value;

                m = Convert.ToInt32(dto.ToString("MM"));
                y = Convert.ToInt32(dto.ToString("yyyy"));

                scdfrom = dfrom.ToString("yyyy-MM-01");
                scdto = dto.ToString("yyyy-MM-" + DateTime.DaysInMonth(y, m));

                psCFDate = dfrom.ToString("MM-01-yyyy");
                psCTDate = dto.ToString("MM-" + DateTime.DaysInMonth(y, m) + "-yyyy");

                if (scsearch == "")
                {
                    scdrange = " WHERE DATE(Request_Date) BETWEEN '" + scdfrom + "' AND '" + scdto + "'";
                }

                else
                {
                    scdrange = " AND DATE(Request_Date) BETWEEN '" + scdfrom + "' AND '" + scdto + "'";
                }
            }

            else if (cmbCFOption.Text == "Annually")
            {
                dtpCFrom.Enabled = true;
                dtpCTo.Enabled = true;

                dtpCFrom.CustomFormat = "yyyy";
                dtpCTo.CustomFormat = "yyyy";

                dtpCFrom.ShowUpDown = true;
                dtpCTo.ShowUpDown = true;

                DateTime dfrom, dto;

                dfrom = dtpCFrom.Value;
                dto = dtpCTo.Value;

                scdfrom = dfrom.ToString("yyyy-01-01");
                scdto = dto.ToString("yyyy-12-31");

                psCFDate = dfrom.ToString("01-01-yyyy");
                psCTDate = dto.ToString("12-31-yyyy");

                if (scsearch == "")
                {
                    scdrange = " WHERE DATE(Request_Date) BETWEEN '" + scdfrom + "' AND '" + scdto + "'";
                }

                else
                {
                    scdrange = " AND DATE(Request_Date) BETWEEN '" + scdfrom + "' AND '" + scdto + "'";
                }
            }

            else
            {
                dtpCFrom.Enabled = false;
                dtpCTo.Enabled = false;

                dtpCFrom.CustomFormat = "MM-dd-yyyy";
                dtpCTo.CustomFormat = "MM-dd-yyyy";

                dtpCFrom.ShowUpDown = false;
                dtpCTo.ShowUpDown = false;

                scdrange = "";
            }
        }
        #endregion
        
        #region Auto Complete Search
        void CSearch()
        {
            AutoCompleteStringCollection acs = new AutoCompleteStringCollection();

            string cQuery = "SELECT Control_Number, Client_Name, CPA_Name FROM tbl_cert;";
            MySqlConnection cConnection = new MySqlConnection(Conn.cString);
            MySqlCommand cCommand = new MySqlCommand(cQuery, cConnection);
            MySqlDataReader cReader;

            try
            {
                cConnection.Open();
                cReader = cCommand.ExecuteReader();

                acs.Clear();

                while (cReader.Read())
                {
                    string cnn, cln, cpn;

                    cnn = cReader.GetString(0);
                    cln = cReader.GetString(1);
                    cpn = cReader.GetString(2);

                    if (cmbCSOption.Text == "All")
                    {
                        acs.Add("All");
                        acs.Add(cnn);
                    }

                    else if (cmbCSOption.Text == "Client Name")
                    {
                        acs.Add(cln);
                    }

                    else if (cmbCSOption.Text == "CPA Name")
                    {
                        acs.Add(cpn);
                    }

                    else
                    {
                        acs.Add(cnn);
                    }
                }
            }

            catch (MySqlException ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            finally
            {
                cConnection.Close();
            }

            txtCSearch.AutoCompleteCustomSource = acs;
        }
        #endregion

        #region Enable Tooltip
        private void txtCSearch_ForeColorChanged(object sender, EventArgs e)
        {
            TTip();
        }

        private void cmbCSOption_ForeColorChanged(object sender, EventArgs e)
        {
            TTip();
        }

        private void cmbCOOption_ForeColorChanged(object sender, EventArgs e)
        {
            TTip();
        }

        private void cmbCFOption_ForeColorChanged(object sender, EventArgs e)
        {
            TTip();
        }
        #endregion

        #endregion

        #region Pagination
        void CPage()
        {
            dcpage = dccount / icdpage;
            dctpage = Math.Truncate(dcpage);
            scpage = Convert.ToString(dcpage);

            if (scpage.Contains("."))
            {
                dcrpage = dctpage + 1;
            }

            else
            {
                dcrpage = dctpage;
            }
        }
        #endregion

        #region Row Count Default
        void CRCDefault()
        {
            CPage();

            if (icpage == 1 && dcrpage <= 1)
            {
                btnCFirst.Enabled = false;
                btnCBack.Enabled = false;
                btnCNext.Enabled = false;
                btnCLast.Enabled = false;
            }

            else if (icpage == 1 && dcrpage > 1)
            {
                btnCFirst.Enabled = false;
                btnCBack.Enabled = false;
                btnCNext.Enabled = true;
                btnCLast.Enabled = true;
            }

            else if (icpage > 1 && icpage < dcrpage)
            {
                btnCFirst.Enabled = true;
                btnCBack.Enabled = true;
                btnCNext.Enabled = true;
                btnCLast.Enabled = true;
            }

            else if (icpage == dcrpage)
            {
                btnCFirst.Enabled = true;
                btnCBack.Enabled = true;
                btnCNext.Enabled = false;
                btnCLast.Enabled = false;
            }

            EDButtons();
        }
        #endregion

        #region Pagination Focus Control
        void CPFC()
        {
            dgvCert.Focus();
        }
        #endregion

        #region First
        private void btnCFirst_Click(object sender, EventArgs e)
        {
            icpage = 1;
            icp = 0;

            CLoad(icp);

            cmbCSOption_SelectedIndexChanged(sender, e);

            CRCDefault();

            CPFC();
        }
        #endregion

        #region Back
        private void btnCBack_Click(object sender, EventArgs e)
        {
            CPage();

            icpage--;

            if (icpage == 1)
            {
                btnCFirst.Enabled = false;
                btnCBack.Enabled = false;
            }

            if (icpage < dcrpage)
            {
                btnCNext.Enabled = true;
                btnCLast.Enabled = true;
            }

            icp = icp - icdpage;
            CLoad(icp);

            cmbCSOption_SelectedIndexChanged(sender, e);

            EDButtons();

            CPFC();
        }
        #endregion

        #region Next
        private void btnCNext_Click(object sender, EventArgs e)
        {
            CPage();

            icpage++;

            if (icpage == dcrpage)
            {
                btnCNext.Enabled = false;
                btnCLast.Enabled = false;
            }

            if (icpage > 1)
            {
                btnCFirst.Enabled = true;
                btnCBack.Enabled = true;
            }

            icp = icp + icdpage;
            CLoad(icp);

            cmbCSOption_SelectedIndexChanged(sender, e);

            EDButtons();

            CPFC();
        }
        #endregion

        #region Last
        private void btnCLast_Click(object sender, EventArgs e)
        {
            CPage();

            icp = Convert.ToInt32((dcrpage * icdpage) - icdpage);
            icpage = Convert.ToInt32(dcrpage);

            CLoad(icp);

            btnCFirst.Enabled = true;
            btnCBack.Enabled = true;
            btnCNext.Enabled = false;
            btnCLast.Enabled = false;

            cmbCSOption_SelectedIndexChanged(sender, e);

            EDButtons();

            CPFC();
        }
        #endregion

        #region Cell Enter
        private void dgvCert_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            DataGridView dgv = (DataGridView)sender;

            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgv.Rows[e.RowIndex];

                psCNID = row.Cells["No_ID"].Value.ToString();
                psCNum = row.Cells["Control Number"].Value.ToString();

                Certificate.psPBFrom = row.Cells["PRC_BOA_Valid_From"].Value.ToString();
                Certificate.psPBTo = row.Cells["PRC_BOA_Valid_To"].Value.ToString();
                Certificate.psSFrom = row.Cells["SEC_Valid_From"].Value.ToString();
                Certificate.psSTo = row.Cells["SEC_Valid_To"].Value.ToString();
                Certificate.psCCFrom = row.Cells["CDA_CEA_Valid_From"].Value.ToString();
                Certificate.psCCTo = row.Cells["CDA_CEA_Valid_To"].Value.ToString();
                Certificate.psBFrom = row.Cells["BIR_Valid_From"].Value.ToString();
                Certificate.psBTo = row.Cells["BIR_Valid_To"].Value.ToString();
                Certificate.psPFrom = row.Cells["PTR_Valid_From"].Value.ToString();
                Certificate.psPTo = row.Cells["PTR_Valid_From"].Value.ToString();

                Certificate.psRDate = row.Cells["Request Date"].Value.ToString();
                Certificate.psCNumber = row.Cells["Control Number"].Value.ToString();
                Certificate.psCName = row.Cells["Client Name"].Value.ToString();
                Certificate.psAddress = row.Cells["Address"].Value.ToString();
                Certificate.psRDY = row.Cells["Report Date and Year"].Value.ToString();
                Certificate.psPurpose = row.Cells["Purpose"].Value.ToString();
                Certificate.psCPAName = row.Cells["CPA Name"].Value.ToString();
                Certificate.psCPACNumber = row.Cells["CPA Certificate Number"].Value.ToString();
                Certificate.psPBNumber = row.Cells["PRC / BOA Number"].Value.ToString();
                Certificate.psSECNumber = row.Cells["SEC Number"].Value.ToString();
                Certificate.psCCNumber = row.Cells["CDA CEA Number"].Value.ToString();
                Certificate.psBIRNumber = row.Cells["BIR Number"].Value.ToString();
                Certificate.psBSPYear = row.Cells["BSP Accreditation Year"].Value.ToString();
                Certificate.psTIN = row.Cells["TIN"].Value.ToString();
                Certificate.psPTRNumber = row.Cells["PTR Number"].Value.ToString();
            }
        }
        #endregion

        #region Certificate Clear
        void CRTClear()
        {
            Certificate.psPBFrom = "";
            Certificate.psPBTo = "";
            Certificate.psSFrom = "";
            Certificate.psSTo = "";
            Certificate.psCCFrom = "";
            Certificate.psCCTo = "";
            Certificate.psBFrom = "";
            Certificate.psBTo = "";
            Certificate.psPFrom = "";
            Certificate.psPTo = "";
            Certificate.psRDate = "";
            Certificate.psCNumber = "";
            Certificate.psCName = "";
            Certificate.psAddress = "";
            Certificate.psRDY = "";
            Certificate.psPurpose = "";
            Certificate.psCPAName = "";
            Certificate.psCPACNumber = "";
            Certificate.psPBNumber = "";
            Certificate.psSECNumber = "";
            Certificate.psCCNumber = "";
            Certificate.psBIRNumber = "";
            Certificate.psBSPYear = "";
            Certificate.psTIN = "";
            Certificate.psPTRNumber = "";
        }
        #endregion

        #region Delete
        private void btnCDelete_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("Are you sure you want to delete this certificate record?", "Delete Certificate Record", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (dr == DialogResult.No) { }

            else
            {
                string cQuery = "DELETE FROM tbl_cert WHERE No_ID = '" + psCNID + "';";
                MySqlConnection cConnection = new MySqlConnection(Conn.cString);
                MySqlCommand cCommand = new MySqlCommand(cQuery, cConnection);
                MySqlDataReader cReader;

                try
                {
                    cConnection.Open();
                    cReader = cCommand.ExecuteReader();
                    while (cReader.Read()) { }

                    string cn = psCNum;

                    CLoad(icp);
                    CSearch();

                    CBDefault();
                    EDButtons();

                    cmbCSOption_SelectedIndexChanged(sender, e);

                    MessageBox.Show("Certificate with control number " + cn + " has been deleted.", "Delete Certificate Record", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                catch (MySqlException ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                finally
                {
                    cConnection.Close();
                }
            }
        }
        #endregion

        #region Reprint
        private void btnCRPrint_Click(object sender, EventArgs e)
        {
            Certificate.psWCall = "C";

            Certificate w = new Certificate();
            w.ShowDialog();
        }
        #endregion

        #region Get Selected Row
        void CGSRow()
        {
            foreach (DataGridViewRow row in dgvCert.SelectedRows)
            {
                psCNID = row.Cells["No_ID"].Value.ToString();

                psCNum = row.Cells["Control Number"].Value.ToString();
            }
        }
        #endregion

        #region Refresh
        private void btnCRefresh_Click(object sender, EventArgs e)
        {
            CLoad(icp);

            CRCDefault();

            cmbCSOption_SelectedIndexChanged(sender, e);

            CGSRow();

            CBDefault();

            EDButtons();
        }
        #endregion

        //----------------------------------------------------------------------// Report Back, Backup and Restore

        #region Backup
        private void btnBackup_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;

            string bpath = "";

            SaveFileDialog sfd = new SaveFileDialog();
            string idir = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            sfd.InitialDirectory = idir;
            sfd.Filter = "Backup Files (*.bak)|*.bak";
            sfd.Title = "Backup";

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                bpath = Path.GetFullPath(sfd.FileName);

                try
                {
                    using (MySqlConnection cConnection = new MySqlConnection(Conn.cString))
                    {
                        using (MySqlCommand cCommand = new MySqlCommand())
                        {
                            using (MySqlBackup cBackup = new MySqlBackup(cCommand))
                            {
                                cCommand.Connection = cConnection;
                                cConnection.Open();
                                cBackup.ExportToFile(bpath);
                                cConnection.Close();
                            }
                        }
                    }

                    MessageBox.Show("New backup file " + Path.GetFileName(sfd.FileName) + " has been created.", "Backup", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            Cursor.Current = Cursors.Default;
        }
        #endregion

        #region Restore
        private void btnRestore_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;

            string rpath = "", ibpath = "";

            OpenFileDialog ofd = new OpenFileDialog();
            string idir = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            ofd.InitialDirectory = idir;
            ofd.Title = "Restore";
            ofd.Filter = "Backup File (*.bak)|*.bak;";

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                rpath = Path.GetFullPath(ofd.FileName);
                ibpath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\Nieva\Backup\" + DateTime.Now.ToString("MM-dd-yyyy hh-mm-ss tt") + "";

                if (!Directory.Exists(ibpath))
                {
                    Directory.CreateDirectory(ibpath);
                }

                using (MySqlConnection cConnection = new MySqlConnection(Conn.cString))
                {
                    using (MySqlCommand cCommand = new MySqlCommand())
                    {
                        using (MySqlBackup cBackup = new MySqlBackup(cCommand))
                        {
                            cCommand.Connection = cConnection;
                            cConnection.Open();
                            cBackup.ExportToFile(ibpath + @"\Nieva.bak");
                            cConnection.Close();
                        }
                    }
                }

                try
                {
                    using (MySqlConnection cConnection = new MySqlConnection(Conn.cString))
                    {
                        using (MySqlCommand cCommand = new MySqlCommand())
                        {
                            using (MySqlBackup cBackup = new MySqlBackup(cCommand))
                            {
                                cCommand.Connection = cConnection;
                                cConnection.Open();
                                cBackup.ImportFromFile(rpath);
                                cConnection.Close();
                            }
                        }
                    }

                    MessageBox.Show("Backup file " + Path.GetFileName(ofd.FileName) + " has been restored. The system will refresh very shortly.", "Restore", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    Main w = new Main();
                    this.Hide();
                    w.Show();
                }

                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            Cursor.Current = Cursors.Default;
        }
        #endregion

        #region Report Back
        private void btnRBack_Click(object sender, EventArgs e)
        {
            if (btnCS.BackColor == Color.LimeGreen)
            {
                crViewer.Visible = false;
                tlpCS.Visible = true;
            }

            if (btnBP.BackColor == Color.LimeGreen)
            {
                crViewer.Visible = false;
                tlpBP.Visible = true;
            }

            if (btnPLogs.BackColor == Color.LimeGreen)
            {
                crViewer.Visible = false;
                tlpPL.Visible = true;
            }

            if (btnCert.BackColor == Color.LimeGreen)
            {
                crViewer.Visible = false;
                tlpCert.Visible = true;
            }

            btnRBack.Visible = false;
        }

        private void btnRBack_MouseEnter(object sender, EventArgs e)
        {
            btnRBack.ForeColor = Color.DodgerBlue;
        }

        private void btnRBack_MouseLeave(object sender, EventArgs e)
        {
            btnRBack.ForeColor = Color.Black;
        }
        #endregion

    }
}

public static class ExtensionMethods
{
    public static void DoubleBuffered(this DataGridView dgv, bool setting)
    {
        Type dgvType = dgv.GetType();
        PropertyInfo pi = dgvType.GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic);
        pi.SetValue(dgv, setting, null);
    }
}
