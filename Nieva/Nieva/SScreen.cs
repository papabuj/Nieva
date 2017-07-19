using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SQLite;

namespace Nieva
{
    public partial class SScreen : Form
    {
        int i;

        public SScreen()
        {
            InitializeComponent();

            SConnect();

            tmrSplash.Start();
        }

        #region Form Drop Shadow
        private const int CS_DROPSHADOW = 0x00020000;
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ClassStyle |= CS_DROPSHADOW;
                return cp;
            }
        }
        #endregion

        #region Fade In and Fade Out
        private void tmrSplash_Tick(object sender, EventArgs e)
        {
            i++;

            if (i == 30)
            {
                if (cserver == "")
                {
                    tmrSplash.Stop();
                    this.Hide();
                    SConn w = new SConn();
                    w.Show();
                }

                else
                {
                    tmrSplash.Stop();
                    this.Hide();
                    Login w = new Login();
                    w.Show();
                }
            }
        }

        string cserver;

        void SConnect()
        {
            string cQuery = "SELECT * FROM tbl_server WHERE No_ID = 1;";
            SQLiteConnection cConnection = new SQLiteConnection(Login.sString);
            SQLiteCommand cCommand = new SQLiteCommand(cQuery, cConnection);
            SQLiteDataReader cReader;

            try
            {
                cConnection.Open();
                cReader = cCommand.ExecuteReader();
                while (cReader.Read())
                {
                    cserver = cReader.GetValue(1).ToString();
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

    }
}
