using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace Nieva
{
    public partial class PBills : Form
    {
        public static string psSearch = "", psFrequency = "", psUName = "";

        public PBills()
        {
            InitializeComponent();
        }

        #region Shortcut Keys
        private void PBills_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnPBOk_Click(sender, e);
            }

            if (e.KeyCode == Keys.Escape)
            {
                btnPBCancel_Click(sender, e);
            }
        }
        #endregion

        #region DatePicker Value Change
        private void dtpBDate_ValueChanged(object sender, EventArgs e)
        {
            dtpBDate.CustomFormat = "                MM-dd-yyyy";
        }
        #endregion

        #region Ok
        string sstatus = "";

        private void btnPBOk_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;

            if (psSearch == "" && psUName == "")
            {
                sstatus = " WHERE c.Status = 'Active'";
            }

            else
            {
                sstatus = " AND c.Status = 'Active'";
            }

            if (dtpBDate.CustomFormat == " ")
            {
                Main.psPBills = "*EBDate";
                this.Hide();
            }

            else
            {
                string bd = "";

                if (dtpBDate.CustomFormat == "                MM-dd-yyyy")
                {
                    DateTime bdate = dtpBDate.Value;
                    bd = bdate.ToString("yyyy-MM-dd");
                }

                if (psFrequency == "*Monthly")
                {
                    #region Monthly Services
                    string cQuery = "INSERT INTO tbl_mbillpay(Bill_Date, Client_ID, Retainers_Fee_M, Professional_Fee_M,"
                        + " Service_Fee_M, VAT, Non_VAT, D1601C, D1601E, SSS_ER, PHIC_ER, Pag_IBIG_ER, SSS_EE, PHIC_EE, Pag_IBIG_EE,"
                        + " Certification_Fee, Bookkeeping_M, Inventory_M) SELECT '" + bd + "', s.Client_ID, s.Retainers_Fee_M, s.Professional_Fee_M, s.Service_Fee_M, s.VAT,"
                        + " s.Non_VAT, s.D1601C, s.D1601E, s.SSS_ER, s.PHIC_ER, s.Pag_IBIG_ER, s.SSS_EE, s.PHIC_EE, s.Pag_IBIG_EE, Certification_Fee,"
                        + " s.Bookkeeping_M, s.Inventory_M FROM tbl_mservices s LEFT JOIN tbl_clients c ON s.Client_ID = c.Client_ID" + psSearch + psUName + sstatus + ";";
                    MySqlConnection cConnection = new MySqlConnection(Conn.cString);
                    MySqlCommand cCommand = new MySqlCommand(cQuery, cConnection);
                    MySqlDataReader cReader;

                    try
                    {
                        cConnection.Open();
                        cReader = cCommand.ExecuteReader();
                        while (cReader.Read()) { }

                        this.Hide();

                        MessageBox.Show("Bills for monthly services produced successfully.", "Produced Bills", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

                }

                else if (psFrequency == "*Quarterly")
                {
                    #region Quarterly Services
                    string cQuery = "INSERT INTO tbl_qbillpay(Bill_Date, Client_ID, Retainers_Fee_Q, Professional_Fee_Q,"
                        + " Service_Fee_Q, D1701Q, D1702Q, Bookkeeping_Q, Inventory_Q) SELECT '" + bd + "', s.Client_ID,"
                        + " s.Retainers_Fee_Q, s.Professional_Fee_Q, s.Service_Fee_Q, s.D1701Q, s.D1702Q, s.Bookkeeping_Q, s.Inventory_Q"
                        + " FROM tbl_qservices s LEFT JOIN tbl_clients c ON s.Client_ID = c.Client_ID" + psSearch + psUName + sstatus + ";";
                    MySqlConnection cConnection = new MySqlConnection(Conn.cString);
                    MySqlCommand cCommand = new MySqlCommand(cQuery, cConnection);
                    MySqlDataReader cReader;

                    try
                    {
                        cConnection.Open();
                        cReader = cCommand.ExecuteReader();
                        while (cReader.Read()) { }

                        this.Hide();

                        MessageBox.Show("Bills for quarterly services produced successfully.", "Produced Bills", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                }

                else if (psFrequency == "*Annually")
                {
                    #region Annually Services
                    string cQuery = "INSERT INTO tbl_abillpay(Bill_Date, Client_ID, Retainers_Fee_A, Professional_Fee_A,"
                        + " Service_Fee_A, D1701, D1702, D1604CF, D1604E, Municipal_License, COR, Bookkeeping_A, Inventory_A)"
                        + " SELECT '" + bd + "', s.Client_ID, s.Retainers_Fee_A, s.Professional_Fee_A, s.Service_Fee_A, s.D1701,"
                        + " s.D1702, s.D1604CF, s.D1604E, s.Municipal_License, s.COR, s.Bookkeeping_A, s.Inventory_A"
                        + " FROM tbl_aservices s LEFT JOIN tbl_clients c ON s.Client_ID = c.Client_ID" + psSearch + psUName + sstatus + ";";
                    MySqlConnection cConnection = new MySqlConnection(Conn.cString);
                    MySqlCommand cCommand = new MySqlCommand(cQuery, cConnection);
                    MySqlDataReader cReader;

                    try
                    {
                        cConnection.Open();
                        cReader = cCommand.ExecuteReader();
                        while (cReader.Read()) { }

                        this.Hide();

                        MessageBox.Show("Bills for annual services produced successfully.", "Produced Bills", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                }
            }

            Cursor.Current = Cursors.Default;
        }
        #endregion

        #region Cancel
        private void btnPBCancel_Click(object sender, EventArgs e)
        {
            this.Hide();
        }
        #endregion

    }
}
