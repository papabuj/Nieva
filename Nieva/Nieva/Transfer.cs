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
    public partial class Transfer : Form
    {
        public Transfer()
        {
            InitializeComponent();

            PUser();
        }

        #region Shortcut Keys
        private void Transfer_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnTOk_Click(sender, e);
            }

            if (e.KeyCode == Keys.Escape)
            {
                btnTCancel_Click(sender, e);
            }
        }
        #endregion

        #region Populate Username
        void PUser()
        {
            string cQuery = "SELECT Username FROM tbl_user WHERE User_Type = 'Admin' OR User_Type = 'Staff';";
            MySqlConnection cConnection = new MySqlConnection(Conn.uString);
            MySqlCommand cCommand = new MySqlCommand(cQuery, cConnection);
            MySqlDataReader cReader;

            try
            {
                cConnection.Open();
                cReader = cCommand.ExecuteReader();
                while (cReader.Read())
                {
                    string user = cReader.GetString(0);
                    cmbTTo.Items.Add(user);
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

        #region Ok
        private void btnTOk_Click(object sender, EventArgs e)
        {
            if(cmbTTo.Text == "")
            {
                Main.psTransfer = "*ETransfer";
                this.Hide();
            }

            else
            {
                string cQuery = "UPDATE tbl_clients SET Username = '" + cmbTTo.Text + "'" + Main.psTSearch + ";";
                MySqlConnection cConnection = new MySqlConnection(Conn.cString);
                MySqlCommand cCommand = new MySqlCommand(cQuery, cConnection);
                MySqlDataReader cReader;

                try
                {
                    cConnection.Open();
                    cReader = cCommand.ExecuteReader();
                    while (cReader.Read()) { }

                    Main.psTransfer = "*STransfer";
                    this.Hide();

                    MessageBox.Show("Clients has been successfully transfered to " + cmbTTo.Text + ".", "Transfered", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

        #region Cancel
        private void btnTCancel_Click(object sender, EventArgs e)
        {
            this.Hide();
        }
        #endregion
    }
}
