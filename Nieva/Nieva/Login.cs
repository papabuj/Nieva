using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.Runtime.InteropServices;
using System.Data.SQLite;
using System.Net;
using System.Net.Sockets;

namespace Nieva
{
    public partial class Login : Form
    {
        public static string psUName = "", psUType = "", psMName = "", psCStatus = "", psIP = "", psPort = "";

        public Login()
        {
            InitializeComponent();

            SConnect();

            CODBCDSN();

            psMName = Environment.MachineName;

            psIP = GLIP();
        }

        #region Login
        private void btnLogin_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;

            string aip = "";

            string cQuery = "SELECT IP_Address FROM tbl_user WHERE No_ID = 1;";
            MySqlConnection cConnection = new MySqlConnection(Conn.uString);
            MySqlCommand cCommand = new MySqlCommand(cQuery, cConnection);
            MySqlDataReader cReader;

            try
            {
                cConnection.Open();
                cReader = cCommand.ExecuteReader();
                while (cReader.Read())
                {
                    aip = cReader.GetValue(0).ToString();
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

            string cQueryI = "SELECT * FROM tbl_user WHERE Username = '" + txtUsername.Text.Replace("'", "''") + "' AND Password = '" + txtPassword.Text.Replace("'", "''") + "';";

            MySqlConnection cConnectionI = new MySqlConnection();
            MySqlCommand cCommandI = new MySqlCommand();
            MySqlDataReader cReaderI;

            try
            {
                cConnectionI = new MySqlConnection(Conn.uString);
                cCommandI = new MySqlCommand(cQueryI, cConnectionI);

                cConnectionI.Open();
                cReaderI = cCommandI.ExecuteReader();
                int count = 0;
                while (cReaderI.Read())
                {
                    psPort = cReaderI.GetValue(0).ToString();
                    psUName = cReaderI.GetValue(1).ToString();
                    psUType = cReaderI.GetValue(3).ToString();
                    psCStatus = cReaderI.GetValue(4).ToString();

                    count = count + 1;
                }

                if (count == 1)
                {
                    if(psUType == "DC")
                    {
                        User w = new User();
                        this.Hide();
                        w.Show();
                    }

                    else if(psCStatus != "Connected" && psUType != "DC")
                    {
                        if (psUType == "Admin")
                        {
                            RLogin();
                            Main w = new Main();
                            this.Hide();
                            w.Show();
                        }

                        else if (psUType == "Staff" && aip != "")
                        {
                            RLogin();
                            Main w = new Main();
                            this.Hide();
                            w.Show();
                        }              
                        
                        else
                        {
                            MessageBox.Show("Connect first the admin account to admin pc.", "Admin First", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }

                    else
                    {
                        MessageBox.Show("Account is already connected.", "Already Connected", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        txtUsername.Focus();
                    }
                }

                else
                {
                    MessageBox.Show("Invalid Username or Password.", "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtUsername.Focus();
                }
            }

            catch (Exception ex)
            {
                if (ex.Message.Contains("Access denied for user") || ex.Message.Contains("not of the correct type") || ex.Message.Contains("Unable to connect to any of the specified MySQL hosts."))
                {
                    MessageBox.Show("Can't connect to the server. Open the server pc or configure the server connection.", "Server Connection", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    SConn w = new SConn();
                    this.Hide();
                    w.Show();
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

            Cursor.Current = Cursors.Default;
        }

        void RLogin()
        {
            string cQuery = "INSERT INTO tbl_userlogs(Username, User_Type, Activity, Time, PC_Name) VALUES('" + psUName + "', '" + psUType + "', 'Login', '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm") + "', '" + psMName + "');";
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

            string cQueryI = "UPDATE tbl_user SET Status = 'Connected', IP_Address = '" + psIP + "' WHERE Username = '" + psUName + "';";
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

        #region Cancel
        private void btnCancel_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        #endregion

        #region Shortcut Keys
        private void Login_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnLogin_Click(sender, e);

                e.Handled = true;
                e.SuppressKeyPress = true;
            }

            if (e.KeyCode == Keys.Escape)
            {
                btnCancel_Click(sender, e);
            }
        }
        #endregion

        #region Exit
        private void Login_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = false;
                Application.Exit();
            }
        }
        #endregion

        #region Create ODBC DSN
        void CODBCDSN()
        {
            string DSN = "DSN = Nieva;";
            string Database = "Server = " + cserver + "; Port = " + cport + "; Database = nieva; User = " + cuid + "; Password = " + cpwd + "; Option = 3;";

            bool CreateDS = Utils.CreateDataSource(Utils.ODBC_Drivers.SQLite3, DSN + Database);
        }
        #endregion

        #region ODBC Utils
        public static class Utils
        {
            [DllImport("ODBCCP32.dll")]
            private static extern bool SQLConfigDataSource(IntPtr hwndParent, int fRequest, string lpszDriver, string lpszAttributes);

            public static bool CreateDataSource(string ODBCDriver, string DataSource)
            {
                return SQLConfigDataSource((IntPtr)0, ODBC_Request_Modes.ODBC_ADD_DSN, ODBCDriver, DataSource);
            }

            public static class ODBC_Request_Modes
            {
                public static int ODBC_ADD_DSN = 1;
            }

            public static class ODBC_Drivers
            {
                public static string SQLite3 = "MySQL ODBC 5.3 Unicode Driver";
            }
        }
        #endregion

        #region Server Connection
        public static string cserver = "", cport = "", cuid = "", cpwd = "";

        public static string serverpath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\Nieva\";

        public static string sString = "Data Source = " + serverpath + "server.db; Version = 3;";

        void SConnect()
        {
            string cQuery = "SELECT * FROM tbl_server WHERE No_ID = 1;";
            SQLiteConnection cConnection = new SQLiteConnection(sString);
            SQLiteCommand cCommand = new SQLiteCommand(cQuery, cConnection);
            SQLiteDataReader cReader;

            try
            {
                cConnection.Open();
                cReader = cCommand.ExecuteReader();
                while (cReader.Read())
                {
                    cserver = cReader.GetValue(1).ToString();
                    cport = cReader.GetValue(2).ToString();
                    cuid = cReader.GetValue(3).ToString();
                    cpwd = cReader.GetValue(4).ToString();
                }
            }

            catch (SQLiteException ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            finally
            {
                cConnection.Close();
            }
        }
        #endregion

        #region Get Local IP
        private string GLIP()
        {
            IPHostEntry host = Dns.GetHostEntry(Dns.GetHostName());

            foreach(IPAddress ip in host.AddressList)
            {
                if(ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }

            return "127.0.0.1";
        }
        #endregion
    }
}
