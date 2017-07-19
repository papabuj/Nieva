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
    public partial class Remarks : Form
    {

        public static string psFrequency = "", psNID = "";

        public Remarks()
        {
            InitializeComponent();

            txtRemarks.Text = Main.psRContent;

            txtRemarks.Focus();
        }

        #region Shortcut Keys
        private void Remarks_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnROk_Click(sender, e);
            }

            if (e.KeyCode == Keys.Escape)
            {
                btnRCancel_Click(sender, e);
            }
        }
        #endregion

        #region Ok
        private void btnROk_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;

            if (psFrequency == "*Monthly")
                {
                    #region Monthly Services
                    string cQuery = "UPDATE tbl_mbillpay SET Remarks = '" + txtRemarks.Text + "' WHERE No_ID = '" + psNID + "';";
                    MySqlConnection cConnection = new MySqlConnection(Conn.cString);
                    MySqlCommand cCommand = new MySqlCommand(cQuery, cConnection);
                    MySqlDataReader cReader;

                    try
                    {
                        cConnection.Open();
                        cReader = cCommand.ExecuteReader();
                        while (cReader.Read()) { }

                        Main.psRemarks = "*Monthly";

                        this.Hide();

                        MessageBox.Show("Remarks for TRN-" + psNID + " for mothly bill has been saved.", "Remarks", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                    string cQuery = "UPDATE tbl_qbillpay SET Remarks = '" + txtRemarks.Text + "' WHERE No_ID = '" + psNID + "';";
                    MySqlConnection cConnection = new MySqlConnection(Conn.cString);
                    MySqlCommand cCommand = new MySqlCommand(cQuery, cConnection);
                    MySqlDataReader cReader;

                    try
                    {
                        cConnection.Open();
                        cReader = cCommand.ExecuteReader();
                        while (cReader.Read()) { }

                        Main.psRemarks = "*Quarterly";

                        this.Hide();

                        MessageBox.Show("Remarks for TRN-" + psNID + " for quarterly bill has been saved.", "Remarks", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                    string cQuery = "UPDATE tbl_abillpay SET Remarks = '" + txtRemarks.Text + "' WHERE No_ID = '" + psNID + "';";
                    MySqlConnection cConnection = new MySqlConnection(Conn.cString);
                    MySqlCommand cCommand = new MySqlCommand(cQuery, cConnection);
                    MySqlDataReader cReader;

                    try
                    {
                        cConnection.Open();
                        cReader = cCommand.ExecuteReader();
                        while (cReader.Read()) { }

                        Main.psRemarks = "*Annually";

                        this.Hide();

                        MessageBox.Show("Remarks for TRN-" + psNID + " for annual bill has been saved.", "Remarks", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
            
            Cursor.Current = Cursors.Default;
        }
        #endregion

        #region Cancel
        private void btnRCancel_Click(object sender, EventArgs e)
        {
            this.Hide();
        }
        #endregion

    }
}
