using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.Net;
using System.Net.Sockets;

namespace Nieva
{
    public partial class User : Form
    {
        public static string psHIP = "", psCIP = "", psUName = "", psStatus = "", psCPort = "", psHPort = "", psUType = "";

        DataTable cDatasetU;

        Socket sck;

        EndPoint epHost, epClient;

        public User()
        {
            InitializeComponent();

            dgvUser.DoubleBuffered(true);

            ULoad();

            psHIP = Login.psIP;
            psHPort = Login.psPort;

            if (Login.psUType != "Admin")
            {
                btnClose.Text = "&Logout";
            }

            else
            {
                btnClose.Text = "&Close";
            }

            EDButtons();
        }

        #region Shortcut Keys
        private void User_KeyDown(object sender, KeyEventArgs e)
        {
            if(btnClose.Text == "&Close")
            {
                btnClose_Click(sender, e);
            }
        }
        #endregion

        #region Back Color on Enable or Disable Buttons
        void EDButtons()
        {
            if (btnDC.Enabled == false)
            {
                btnDC.BackColor = Color.Silver;
            }

            else
            {
                btnDC.BackColor = Color.LimeGreen;
            }
        }
        #endregion

        #region Load Table
        void ULoad()
        {
            string user = "";

            if(Login.psUType == "Admin")
            {
                user = " WHERE User_Type = 'Staff'";
            }

            MySqlConnection cConn = new MySqlConnection(Conn.uString);
            MySqlCommand cCommand = new MySqlCommand("SELECT No_ID, Username, User_Type AS 'User Type', Status, IP_Address FROM tbl_user" + user + ";", cConn);

            try
            {
                MySqlDataAdapter cAdapter = new MySqlDataAdapter();
                cAdapter.SelectCommand = cCommand;
                cDatasetU = new DataTable();
                cAdapter.Fill(cDatasetU);
                BindingSource cSource = new BindingSource();
                cSource.DataSource = cDatasetU;
                dgvUser.DataSource = cSource;
                cAdapter.Update(cDatasetU);
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            dgvUser.Columns[0].Visible = false;
            dgvUser.Columns[4].Visible = false;
        }
        #endregion

        #region Refresh
        private void btnRefresh_Click(object sender, EventArgs e)
        {
            ULoad();
        }
        #endregion

        #region Cell Formatting
        private void dgvUser_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            foreach (DataGridViewRow row in dgvUser.Rows)
            {
                if (row.Cells["User Type"].Value.ToString() == "DC")
                {
                    row.Visible = false;
                }
            }
        }
        #endregion

        #region Close
        private void btnClose_Click(object sender, EventArgs e)
        {
            if(Login.psUType == "Admin")
            {
                this.Hide();
            }
            
            else
            {
                Login w = new Login();
                this.Hide();
                w.Show();
            }
        }
        #endregion

        #region Cell Enter
        private void dgvUser_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            DataGridView dgv = (DataGridView)sender;

            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgv.Rows[e.RowIndex];

                psUName = row.Cells["Username"].Value.ToString();
                psStatus = row.Cells["Status"].Value.ToString();
                psUType = row.Cells["User Type"].Value.ToString();
                psCIP = row.Cells["IP_Address"].Value.ToString();
                psCPort = row.Cells["No_ID"].Value.ToString();

                if (psStatus != "Connected")
                {
                    btnDC.Enabled = false;
                    EDButtons();
                }

                else
                {
                    btnDC.Enabled = true;
                    EDButtons();
                }
            }
        }
        #endregion

        #region Start Socket
        void SScocket()
        {
            try
            {
                sck = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                sck.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);

                epHost = new IPEndPoint(IPAddress.Parse(psHIP), Convert.ToInt32(psHPort));
                sck.Bind(epHost);

                epClient = new IPEndPoint(IPAddress.Parse(psCIP), Convert.ToInt32(psCPort));
                sck.Connect(epClient);
            }

            catch (Exception) { }
        }
        #endregion

        #region Disconnect
        private void btnDC_Click(object sender, EventArgs e)
        {
            SScocket();

            try
            {
                string msg = "Disconnect";
                ASCIIEncoding aEncode = new ASCIIEncoding();
                byte[] bfr = new byte[1500];
                bfr = aEncode.GetBytes(msg);
                sck.Send(bfr);
            }

            catch (Exception) { }

            string cQuery = "UPDATE tbl_user SET Status = 'Not Connected', IP_Address = NULL WHERE Username = '" + psUName + "';";
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

            if (psUName == Login.psUName)
            {
                RLogout();
                Application.Exit();
            }

            string uname = psUName;

            ULoad();

            foreach (DataGridViewRow row in dgvUser.Rows)
            {
                if (row.Cells[1].Value.ToString().Equals(uname))
                {
                    dgvUser.CurrentCell = dgvUser[1, row.Index];
                }
            }
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
        }
        #endregion

        #region Exit
        private void User_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (Login.psUType == "Admin")
            {
                this.Hide();
            }

            else
            {
                if (e.CloseReason == CloseReason.UserClosing)
                {
                    e.Cancel = false;
                    Application.Exit();
                }
            }
        }
        #endregion

    }
}
