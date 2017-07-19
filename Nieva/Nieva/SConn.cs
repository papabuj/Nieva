using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SQLite;
using MySql.Data.MySqlClient;

namespace Nieva
{
    public partial class SConn : Form
    {
        public SConn()
        {
            InitializeComponent();

            txtServer.Text = Login.cserver;
            txtPort.Text = Login.cport;
            txtUsername.Text = Login.cuid;
            txtPassword.Text = Login.cpwd;
        }

        #region Ok
        private void btnOk_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;

            if (txtServer.Text == "")
            {
                MessageBox.Show("Server field is empty.", "Empty Field", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            else if (txtPort.Text == "")
            {
                MessageBox.Show("Port field is empty.", "Empty Field", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            else if(txtUsername.Text == "")
            {
                MessageBox.Show("Username field is empty.", "Empty Field", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            else if(txtPassword.Text == "")
            {
                MessageBox.Show("Password field is empty.", "Empty Field", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            else
            {
                string cQuery = "UPDATE tbl_server SET Server = '" + txtServer.Text + "', Port = '" + txtPort.Text + "',"
                + " Username = '" + txtUsername.Text + "', Password = '" + txtPassword.Text + "' WHERE No_ID = 1;";
                SQLiteConnection cConnection = new SQLiteConnection(Login.sString);
                SQLiteCommand cCommand = new SQLiteCommand(cQuery, cConnection);
                SQLiteDataReader cReader;

                try
                {
                    cConnection.Open();
                    cReader = cCommand.ExecuteReader();
                    while (cReader.Read()) { }

                }

                catch (SQLiteException ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                finally
                {
                    cConnection.Close();
                }

                string crConn = "Server = " + txtServer.Text + "; Port = " + txtPort.Text + "; Uid = " + txtUsername.Text + "; Pwd = " + txtPassword.Text + ";";

                string crNonQuery = "CREATE DATABASE IF NOT EXISTS `nieva`";
                MySqlConnection crConnection = new MySqlConnection();
                MySqlCommand crCommand = new MySqlCommand();

                try
                {
                    crConnection = new MySqlConnection(crConn);
                    crCommand = new MySqlCommand(crNonQuery, crConnection);

                    crConnection.Open();

                    crCommand.ExecuteNonQuery();

                    DUser();
                    CNieva();
                    CUser();
                    Application.Restart();
                }

                catch (Exception ex)
                {
                    if (ex.Message.Contains("Access denied for user") || ex.Message.Contains("not of the correct type") || ex.Message.Contains("Unable to connect to any of the specified MySQL hosts."))
                    {
                        MessageBox.Show("Server connection failed. Check and correct all fields for the server connection.", "Server Connection", MessageBoxButtons.OK, MessageBoxIcon.Error);

                        txtServer.Focus();
                    }

                    else
                    {
                        MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }

                finally
                {
                    crConnection.Close();
                }             
            }

            Cursor.Current = Cursors.Default;
        }

        void DUser()
        {
            string crConn = "Server = " + txtServer.Text + "; Port = " + txtPort.Text + "; Uid = " + txtUsername.Text + "; Pwd = " + txtPassword.Text + ";";

            string crNonQueryI = "CREATE DATABASE IF NOT EXISTS `user`";
            MySqlConnection crConnectionI = new MySqlConnection();
            MySqlCommand crCommandI = new MySqlCommand();

            try
            {
                crConnectionI = new MySqlConnection(crConn);
                crCommandI = new MySqlCommand(crNonQueryI, crConnectionI);

                crConnectionI.Open();

                crCommandI.ExecuteNonQuery();
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            finally
            {
                crConnectionI.Close();
            }
        }
        #endregion

        #region Nieva Table
        void CNieva()
        {
            string crConn = "Server = " + txtServer.Text + "; Database = nieva; Port = " + txtPort.Text + "; Uid = " + txtUsername.Text + "; Pwd = " + txtPassword.Text + ";";

            string crNonQuery = "CREATE TABLE IF NOT EXISTS `tbl_abillpay` ("
                + " `No_ID` int(11) NOT NULL AUTO_INCREMENT,"
                + " `Bill_Date` varchar(45) DEFAULT NULL,"
                + " `Client_ID` varchar(45) DEFAULT NULL,"
                + " `Retainers_Fee_A` varchar(45) DEFAULT NULL,"
                + " `Professional_Fee_A` varchar(45) DEFAULT NULL,"
                + " `Service_Fee_A` varchar(45) DEFAULT NULL,"
                + " `D1701` varchar(45) DEFAULT NULL,"
                + " `D1702` varchar(45) DEFAULT NULL,"
                + " `D1604CF` varchar(45) DEFAULT NULL,"
                + " `D1604E` varchar(45) DEFAULT NULL,"
                + " `Municipal_License` varchar(45) DEFAULT NULL,"
                + " `COR` varchar(45) DEFAULT NULL,"
                + " `Bookkeeping_A` varchar(45) DEFAULT NULL,"
                + " `Inventory_A` varchar(45) DEFAULT NULL,"
                + " `Remarks` varchar(45) DEFAULT NULL,"
                + " PRIMARY KEY(`No_ID`)"
                + " ) ENGINE = InnoDB DEFAULT CHARSET = utf8;";

            MySqlConnection crConnection = new MySqlConnection();
            MySqlCommand crCommand = new MySqlCommand();

            try
            {
                crConnection = new MySqlConnection(crConn);
                crCommand = new MySqlCommand(crNonQuery, crConnection);

                crConnection.Open();

                crCommand.ExecuteNonQuery();
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            finally
            {
                crConnection.Close();
            }

            string crNonQueryI = "CREATE TABLE IF NOT EXISTS `tbl_apaylogs` ("
                + " `No_ID` int(11) NOT NULL AUTO_INCREMENT,"
                + " `Payment_Date` varchar(45) DEFAULT NULL,"
                + " `Transaction_Number` varchar(45) DEFAULT NULL,"
                + " `Client_ID` varchar(45) DEFAULT NULL,"
                + " `Retainers_Fee_A` varchar(45) DEFAULT NULL,"
                + " `Professional_Fee_A` varchar(45) DEFAULT NULL,"
                + " `Service_Fee_A` varchar(45) DEFAULT NULL,"
                + " `D1701` varchar(45) DEFAULT NULL,"
                + " `D1702` varchar(45) DEFAULT NULL,"
                + " `D1604CF` varchar(45) DEFAULT NULL,"
                + " `D1604E` varchar(45) DEFAULT NULL,"
                + " `Municipal_License` varchar(45) DEFAULT NULL,"
                + " `COR` varchar(45) DEFAULT NULL,"
                + " `Bookkeeping_A` varchar(45) DEFAULT NULL,"
                + " `Inventory_A` varchar(45) DEFAULT NULL,"
                + " PRIMARY KEY(`No_ID`)"
                + " ) ENGINE = InnoDB DEFAULT CHARSET = utf8;";

            MySqlConnection crConnectionI = new MySqlConnection();
            MySqlCommand crCommandI = new MySqlCommand();

            try
            {
                crConnectionI = new MySqlConnection(crConn);
                crCommandI = new MySqlCommand(crNonQueryI, crConnectionI);

                crConnectionI.Open();

                crCommandI.ExecuteNonQuery();
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            finally
            {
                crConnectionI.Close();
            }

            string crNonQueryII = "CREATE TABLE IF NOT EXISTS `tbl_aservices` ("
                + " `No_ID` int(11) NOT NULL AUTO_INCREMENT,"
                + " `Client_ID` varchar(45) DEFAULT NULL,"
                + " `Retainers_Fee_A` varchar(45) DEFAULT NULL,"
                + " `Professional_Fee_A` varchar(45) DEFAULT NULL,"
                + " `Service_Fee_A` varchar(45) DEFAULT NULL,"
                + " `D1701` varchar(45) DEFAULT NULL,"
                + " `D1702` varchar(45) DEFAULT NULL,"
                + " `D1604CF` varchar(45) DEFAULT NULL,"
                + " `D1604E` varchar(45) DEFAULT NULL,"
                + " `Municipal_License` varchar(45) DEFAULT NULL,"
                + " `COR` varchar(45) DEFAULT NULL,"
                + " `Bookkeeping_A` varchar(45) DEFAULT NULL,"
                + " `Inventory_A` varchar(45) DEFAULT NULL,"
                + " PRIMARY KEY(`No_ID`)"
                + " ) ENGINE = InnoDB DEFAULT CHARSET = utf8;";

            MySqlConnection crConnectionII = new MySqlConnection();
            MySqlCommand crCommandII = new MySqlCommand();

            try
            {
                crConnectionII = new MySqlConnection(crConn);
                crCommandII = new MySqlCommand(crNonQueryII, crConnectionII);

                crConnectionII.Open();

                crCommandII.ExecuteNonQuery();
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            finally
            {
                crConnectionII.Close();
            }

            string crNonQueryIII = "CREATE TABLE IF NOT EXISTS `tbl_clients` ("
                + " `No_ID` int(11) NOT NULL AUTO_INCREMENT,"
                + " `Client_ID` varchar(45) DEFAULT NULL,"
                + " `Last_Name` varchar(45) DEFAULT NULL,"
                + " `First_Name` varchar(45) DEFAULT NULL,"
                + " `Middle_Initial` varchar(45) DEFAULT NULL,"
                + " `Business_Name` varchar(45) DEFAULT NULL,"
                + " `Nature_of_Business` varchar(45) DEFAULT NULL,"
                + " `Business_or_Owner_TIN` varchar(45) DEFAULT NULL,"
                + " `Tax_Type` varchar(45) DEFAULT NULL,"
                + " `Address` varchar(45) DEFAULT NULL,"
                + " `Contact_Number` varchar(45) DEFAULT NULL,"
                + " `Email_Address` varchar(45) DEFAULT NULL,"
                + " `SEC_Registration_Number` varchar(45) DEFAULT NULL,"
                + " `SEC_Registration_Date` varchar(45) DEFAULT NULL,"
                + " `DTI_Number` varchar(45) DEFAULT NULL,"
                + " `DTI_Issuance_Date` varchar(45) DEFAULT NULL,"
                + " `COR_Number` varchar(45) DEFAULT NULL,"
                + " `COR_Date` varchar(45) DEFAULT NULL,"
                + " `SSS_Number` varchar(45) DEFAULT NULL,"
                + " `Pag_IBIG_Number` varchar(45) DEFAULT NULL,"
                + " `PhilHealth_Number` varchar(45) DEFAULT NULL,"
                + " `Group_Name` varchar(45) DEFAULT NULL,"
                + " `Sub_Group_Name` varchar(45) DEFAULT NULL,"
                + " `Group_Year` varchar(45) DEFAULT NULL,"
                + " `Status` varchar(45) DEFAULT NULL,"
                + " `Username` varchar(45) DEFAULT NULL,"
                + " PRIMARY KEY(`No_ID`),"
                + " UNIQUE KEY `Client_ID_UNIQUE` (`Client_ID`),"
                + " UNIQUE KEY `Business_Name_UNIQUE` (`Business_Name`)"
                + " ) ENGINE = InnoDB DEFAULT CHARSET = utf8;";

            MySqlConnection crConnectionIII = new MySqlConnection();
            MySqlCommand crCommandIII = new MySqlCommand();

            try
            {
                crConnectionIII = new MySqlConnection(crConn);
                crCommandIII = new MySqlCommand(crNonQueryIII, crConnectionIII);

                crConnectionIII.Open();

                crCommandIII.ExecuteNonQuery();
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            finally
            {
                crConnectionIII.Close();
            }

            string crNonQueryIV = "CREATE TABLE IF NOT EXISTS `tbl_mbillpay` ("
                + " `No_ID` int(11) NOT NULL AUTO_INCREMENT,"
                + " `Bill_Date` varchar(45) DEFAULT NULL,"
                + " `Client_ID` varchar(45) DEFAULT NULL,"
                + " `Retainers_Fee_M` varchar(45) DEFAULT NULL,"
                + " `Professional_Fee_M` varchar(45) DEFAULT NULL,"
                + " `Service_Fee_M` varchar(45) DEFAULT NULL,"
                + " `VAT` varchar(45) DEFAULT NULL,"
                + " `Non_VAT` varchar(45) DEFAULT NULL,"
                + " `D1601C` varchar(45) DEFAULT NULL,"
                + " `D1601E` varchar(45) DEFAULT NULL,"
                + " `SSS_ER` varchar(45) DEFAULT NULL,"
                + " `PHIC_ER` varchar(45) DEFAULT NULL,"
                + " `Pag_IBIG_ER` varchar(45) DEFAULT NULL,"
                + " `SSS_EE` varchar(45) DEFAULT NULL,"
                + " `PHIC_EE` varchar(45) DEFAULT NULL,"
                + " `Pag_IBIG_EE` varchar(45) DEFAULT NULL,"
                + " `Certification_Fee` varchar(45) DEFAULT NULL,"
                + " `Bookkeeping_M` varchar(45) DEFAULT NULL,"
                + " `Inventory_M` varchar(45) DEFAULT NULL,"
                + " `Remarks` varchar(45) DEFAULT NULL,"
                + " PRIMARY KEY (`No_ID`)"
                + " ) ENGINE =InnoDB DEFAULT CHARSET=utf8;";

            MySqlConnection crConnectionIV = new MySqlConnection();
            MySqlCommand crCommandIV = new MySqlCommand();

            try
            {
                crConnectionIV = new MySqlConnection(crConn);
                crCommandIV = new MySqlCommand(crNonQueryIV, crConnectionIV);

                crConnectionIV.Open();

                crCommandIV.ExecuteNonQuery();
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            finally
            {
                crConnectionIV.Close();
            }

            string crNonQueryV = "CREATE TABLE IF NOT EXISTS `tbl_mpaylogs` ("
                + " `No_ID` int(11) NOT NULL AUTO_INCREMENT,"
                + " `Transaction_Number` varchar(45) DEFAULT NULL,"
                + " `Payment_Date` varchar(45) DEFAULT NULL,"
                + " `Client_ID` varchar(45) DEFAULT NULL,"
                + " `Retainers_Fee_M` varchar(45) DEFAULT NULL,"
                + " `Professional_Fee_M` varchar(45) DEFAULT NULL,"
                + " `Service_Fee_M` varchar(45) DEFAULT NULL,"
                + " `VAT` varchar(45) DEFAULT NULL,"
                + " `Non_VAT` varchar(45) DEFAULT NULL,"
                + " `D1601C` varchar(45) DEFAULT NULL,"
                + " `D1601E` varchar(45) DEFAULT NULL,"
                + " `SSS_ER` varchar(45) DEFAULT NULL,"
                + " `PHIC_ER` varchar(45) DEFAULT NULL,"
                + " `Pag_IBIG_ER` varchar(45) DEFAULT NULL,"
                + " `SSS_EE` varchar(45) DEFAULT NULL,"
                + " `PHIC_EE` varchar(45) DEFAULT NULL,"
                + " `Pag_IBIG_EE` varchar(45) DEFAULT NULL,"
                + " `Certification_Fee` varchar(45) DEFAULT NULL,"
                + " `Bookkeeping_M` varchar(45) DEFAULT NULL,"
                + " `Inventory_M` varchar(45) DEFAULT NULL,"
                + " PRIMARY KEY(`No_ID`)"
                + " ) ENGINE = InnoDB DEFAULT CHARSET = utf8;";

            MySqlConnection crConnectionV = new MySqlConnection();
            MySqlCommand crCommandV = new MySqlCommand();

            try
            {
                crConnectionV = new MySqlConnection(crConn);
                crCommandV = new MySqlCommand(crNonQueryV, crConnectionV);

                crConnectionV.Open();

                crCommandV.ExecuteNonQuery();
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            finally
            {
                crConnectionV.Close();
            }

            string crNonQueryVI = "CREATE TABLE IF NOT EXISTS `tbl_mservices` ("
                + " `No_ID` int(11) NOT NULL AUTO_INCREMENT,"
                + " `Client_ID` varchar(45) DEFAULT NULL,"
                + " `Retainers_Fee_M` varchar(45) DEFAULT NULL,"
                + " `Professional_Fee_M` varchar(45) DEFAULT NULL,"
                + " `Service_Fee_M` varchar(45) DEFAULT NULL,"
                + " `VAT` varchar(45) DEFAULT NULL,"
                + " `Non_VAT` varchar(45) DEFAULT NULL,"
                + " `D1601C` varchar(45) DEFAULT NULL,"
                + " `D1601E` varchar(45) DEFAULT NULL,"
                + " `SSS_ER` varchar(45) DEFAULT NULL,"
                + " `PHIC_ER` varchar(45) DEFAULT NULL,"
                + " `Pag_IBIG_ER` varchar(45) DEFAULT NULL,"
                + " `SSS_EE` varchar(45) DEFAULT NULL,"
                + " `PHIC_EE` varchar(45) DEFAULT NULL,"
                + " `Pag_IBIG_EE` varchar(45) DEFAULT NULL,"
                + " `Certification_Fee` varchar(45) DEFAULT NULL,"
                + " `Bookkeeping_M` varchar(45) DEFAULT NULL,"
                + " `Inventory_M` varchar(45) DEFAULT NULL,"
                + " PRIMARY KEY(`No_ID`)"
                + " ) ENGINE = InnoDB DEFAULT CHARSET = utf8;";

            MySqlConnection crConnectionVI = new MySqlConnection();
            MySqlCommand crCommandVI = new MySqlCommand();

            try
            {
                crConnectionVI = new MySqlConnection(crConn);
                crCommandVI = new MySqlCommand(crNonQueryVI, crConnectionVI);

                crConnectionVI.Open();

                crCommandVI.ExecuteNonQuery();
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            finally
            {
                crConnectionVI.Close();
            }

            string crNonQueryVII = "CREATE TABLE IF NOT EXISTS `tbl_qbillpay` ("
                + " `No_ID` int(11) NOT NULL AUTO_INCREMENT,"
                + " `Bill_Date` varchar(45) DEFAULT NULL,"
                + " `Client_ID` varchar(45) DEFAULT NULL,"
                + " `Retainers_Fee_Q` varchar(45) DEFAULT NULL,"
                + " `Professional_Fee_Q` varchar(45) DEFAULT NULL,"
                + " `Service_Fee_Q` varchar(45) DEFAULT NULL,"
                + " `D1701Q` varchar(45) DEFAULT NULL,"
                + " `D1702Q` varchar(45) DEFAULT NULL,"
                + " `Bookkeeping_Q` varchar(45) DEFAULT NULL,"
                + " `Inventory_Q` varchar(45) DEFAULT NULL,"
                + " `Remarks` varchar(45) DEFAULT NULL,"
                + " PRIMARY KEY(`No_ID`)"
                + " ) ENGINE = InnoDB DEFAULT CHARSET = utf8;";

            MySqlConnection crConnectionVII = new MySqlConnection();
            MySqlCommand crCommandVII = new MySqlCommand();

            try
            {
                crConnectionVII = new MySqlConnection(crConn);
                crCommandVII = new MySqlCommand(crNonQueryVII, crConnectionVII);

                crConnectionVII.Open();

                crCommandVII.ExecuteNonQuery();
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            finally
            {
                crConnectionVII.Close();
            }

            string crNonQueryVIII = "CREATE TABLE IF NOT EXISTS `tbl_qpaylogs` ("
                + " `No_ID` int(11) NOT NULL AUTO_INCREMENT,"
                + " `Payment_Date` varchar(45) DEFAULT NULL,"
                + " `Transaction_Number` varchar(45) DEFAULT NULL,"
                + " `Client_ID` varchar(45) DEFAULT NULL,"
                + " `Retainers_Fee_Q` varchar(45) DEFAULT NULL,"
                + " `Professional_Fee_Q` varchar(45) DEFAULT NULL,"
                + " `Service_Fee_Q` varchar(45) DEFAULT NULL,"
                + " `D1701Q` varchar(45) DEFAULT NULL,"
                + " `D1702Q` varchar(45) DEFAULT NULL,"
                + " `Bookkeeping_Q` varchar(45) DEFAULT NULL,"
                + " `Inventory_Q` varchar(45) DEFAULT NULL,"
                + " PRIMARY KEY(`No_ID`)"
                + " ) ENGINE = InnoDB DEFAULT CHARSET = utf8;";

            MySqlConnection crConnectionVIII = new MySqlConnection();
            MySqlCommand crCommandVIII = new MySqlCommand();

            try
            {
                crConnectionVIII = new MySqlConnection(crConn);
                crCommandVIII = new MySqlCommand(crNonQueryVIII, crConnectionVIII);

                crConnectionVIII.Open();

                crCommandVIII.ExecuteNonQuery();
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            finally
            {
                crConnectionVIII.Close();
            }

            string crNonQueryIX = "CREATE TABLE IF NOT EXISTS `tbl_qservices` ("
                + " `No_ID` int(11) NOT NULL AUTO_INCREMENT,"
                + " `Client_ID` varchar(45) DEFAULT NULL,"
                + " `Retainers_Fee_Q` varchar(45) DEFAULT NULL,"
                + " `Professional_Fee_Q` varchar(45) DEFAULT NULL,"
                + " `Service_Fee_Q` varchar(45) DEFAULT NULL,"
                + " `D1701Q` varchar(45) DEFAULT NULL,"
                + " `D1702Q` varchar(45) DEFAULT NULL,"
                + " `Bookkeeping_Q` varchar(45) DEFAULT NULL,"
                + " `Inventory_Q` varchar(45) DEFAULT NULL,"
                + " PRIMARY KEY(`No_ID`)"
                + " ) ENGINE = InnoDB DEFAULT CHARSET = utf8;";

            MySqlConnection crConnectionIX = new MySqlConnection();
            MySqlCommand crCommandIX = new MySqlCommand();

            try
            {
                crConnectionIX = new MySqlConnection(crConn);
                crCommandIX = new MySqlCommand(crNonQueryIX, crConnectionIX);

                crConnectionIX.Open();

                crCommandIX.ExecuteNonQuery();
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            finally
            {
                crConnectionIX.Close();
            }

            string crNonQueryX = "CREATE OR REPLACE VIEW `vw_mpaylogs` AS SELECT Transaction_Number, Client_ID,"
                + " SUM(IFNULL(CAST(Retainers_Fee_M AS DECIMAL(20, 2)), 0)) AS 'Retainers_Fee_M',"
                + " SUM(IFNULL(CAST(Professional_Fee_M AS DECIMAL(20, 2)), 0)) AS 'Professional_Fee_M',"
                + " SUM(IFNULL(CAST(Service_Fee_M AS DECIMAL(20, 2)), 0)) AS 'Service_Fee_M',"
                + " SUM(IFNULL(CAST(VAT AS DECIMAL(20, 2)), 0)) AS 'VAT',"
                + " SUM(IFNULL(CAST(Non_VAT AS DECIMAL(20, 2)), 0)) AS 'Non_VAT',"
                + " SUM(IFNULL(CAST(D1601C AS DECIMAL(20, 2)), 0)) AS 'D1601C',"
                + " SUM(IFNULL(CAST(D1601E AS DECIMAL(20, 2)), 0)) AS 'D1601E',"
                + " SUM(IFNULL(CAST(SSS_ER AS DECIMAL(20, 2)), 0)) AS 'SSS_ER',"
                + " SUM(IFNULL(CAST(PHIC_ER AS DECIMAL(20, 2)), 0)) AS 'PHIC_ER',"
                + " SUM(IFNULL(CAST(Pag_IBIG_ER AS DECIMAL(20, 2)), 0)) AS 'Pag_IBIG_ER',"
                + " SUM(IFNULL(CAST(SSS_EE AS DECIMAL(20, 2)), 0)) AS 'SSS_EE',"
                + " SUM(IFNULL(CAST(PHIC_EE AS DECIMAL(20, 2)), 0)) AS 'PHIC_EE',"
                + " SUM(IFNULL(CAST(Pag_IBIG_EE AS DECIMAL(20, 2)), 0)) AS 'Pag_IBIG_EE',"
                + " SUM(IFNULL(CAST(Certification_Fee AS DECIMAL(20, 2)), 0)) AS 'Certification_Fee',"
                + " SUM(IFNULL(CAST(Bookkeeping_M AS DECIMAL(20, 2)), 0)) AS 'Bookkeeping_M',"
                + " SUM(IFNULL(CAST(Inventory_M AS DECIMAL(20, 2)), 0)) AS 'Inventory_M'"
                + " FROM tbl_mpaylogs GROUP BY Transaction_Number;";

            MySqlConnection crConnectionX = new MySqlConnection();
            MySqlCommand crCommandX = new MySqlCommand();

            try
            {
                crConnectionX = new MySqlConnection(crConn);
                crCommandX = new MySqlCommand(crNonQueryX, crConnectionX);

                crConnectionX.Open();

                crCommandX.ExecuteNonQuery();
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            finally
            {
                crConnectionX.Close();
            }

            string crNonQueryXI = "CREATE OR REPLACE VIEW `vw_qpaylogs` AS SELECT Transaction_Number, Client_ID,"
                + " SUM(IFNULL(CAST(Retainers_Fee_Q AS DECIMAL(20, 2)), 0)) AS 'Retainers_Fee_Q',"
                + " SUM(IFNULL(CAST(Professional_Fee_Q AS DECIMAL(20, 2)), 0)) AS 'Professional_Fee_Q',"
                + " SUM(IFNULL(CAST(Service_Fee_Q AS DECIMAL(20, 2)), 0)) AS 'Service_Fee_Q',"
                + " SUM(IFNULL(CAST(D1701Q AS DECIMAL(20, 2)), 0)) AS 'D1701Q',"
                + " SUM(IFNULL(CAST(D1702Q AS DECIMAL(20, 2)), 0)) AS 'D1702Q',"
                + " SUM(IFNULL(CAST(Bookkeeping_Q AS DECIMAL(20, 2)), 0)) AS 'Bookkeeping_Q',"
                + " SUM(IFNULL(CAST(Inventory_Q AS DECIMAL(20, 2)), 0)) AS 'Inventory_Q'"
                + " FROM tbl_qpaylogs GROUP BY Transaction_Number;";

            MySqlConnection crConnectionXI = new MySqlConnection();
            MySqlCommand crCommandXI = new MySqlCommand();

            try
            {
                crConnectionXI = new MySqlConnection(crConn);
                crCommandXI = new MySqlCommand(crNonQueryXI, crConnectionXI);

                crConnectionXI.Open();

                crCommandXI.ExecuteNonQuery();
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            finally
            {
                crConnectionXI.Close();
            }

            string crNonQueryXII = "CREATE OR REPLACE VIEW `vw_apaylogs` AS SELECT Transaction_Number, Client_ID,"
                + " SUM(IFNULL(CAST(Retainers_Fee_A AS DECIMAL(20, 2)), 0)) AS 'Retainers_Fee_A',"
                + " SUM(IFNULL(CAST(Professional_Fee_A AS DECIMAL(20, 2)), 0)) AS 'Professional_Fee_A',"
                + " SUM(IFNULL(CAST(Service_Fee_A AS DECIMAL(20, 2)), 0)) AS 'Service_Fee_A',"
                + " SUM(IFNULL(CAST(D1701 AS DECIMAL(20, 2)), 0)) AS 'D1701',"
                + " SUM(IFNULL(CAST(D1702 AS DECIMAL(20, 2)), 0)) AS 'D1702',"
                + " SUM(IFNULL(CAST(D1604CF AS DECIMAL(20, 2)), 0)) AS 'D1604CF',"
                + " SUM(IFNULL(CAST(D1604E AS DECIMAL(20, 2)), 0)) AS 'D1604E',"
                + " SUM(IFNULL(CAST(Municipal_License AS DECIMAL(20, 2)), 0)) AS 'Municipal_License',"
                + " SUM(IFNULL(CAST(COR AS DECIMAL(20, 2)), 0)) AS 'COR',"
                + " SUM(IFNULL(CAST(Bookkeeping_A AS DECIMAL(20, 2)), 0)) AS 'Bookkeeping_A',"
                + " SUM(IFNULL(CAST(Inventory_A AS DECIMAL(20, 2)), 0)) AS 'Inventory_A'"
                + " FROM tbl_apaylogs GROUP BY Transaction_Number;";

            MySqlConnection crConnectionXII = new MySqlConnection();
            MySqlCommand crCommandXII = new MySqlCommand();

            try
            {
                crConnectionXII = new MySqlConnection(crConn);
                crCommandXII = new MySqlCommand(crNonQueryXII, crConnectionXII);

                crConnectionXII.Open();

                crCommandXII.ExecuteNonQuery();
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            finally
            {
                crConnectionXII.Close();
            }

            string crNonQueryXIII = "CREATE TABLE IF NOT EXISTS `tbl_cert` ("
                + " `No_ID` int(11) NOT NULL AUTO_INCREMENT,"
                + " `Request_Date` varchar(45) DEFAULT NULL,"
                + " `Control_Number` varchar(45) DEFAULT NULL,"
                + " `Client_Name` varchar(45) DEFAULT NULL,"
                + " `Address` varchar(45) DEFAULT NULL,"
                + " `Report_Date_and_Year` varchar(45) DEFAULT NULL,"
                + " `Purpose` varchar(45) DEFAULT NULL,"
                + " `CPA_Name` varchar(45) DEFAULT NULL,"
                + " `CPA_Cert_Number` varchar(45) DEFAULT NULL,"
                + " `PRC_BOA_Number` varchar(45) DEFAULT NULL,"
                + " `PRC_BOA_Valid_From` varchar(45) DEFAULT NULL,"
                + " `PRC_BOA_Valid_To` varchar(45) DEFAULT NULL,"
                + " `SEC_Number` varchar(45) DEFAULT NULL,"
                + " `SEC_Valid_From` varchar(45) DEFAULT NULL,"
                + " `SEC_Valid_To` varchar(45) DEFAULT NULL,"
                + " `CDA_CEA_Number` varchar(45) DEFAULT NULL,"
                + " `CDA_CEA_Valid_From` varchar(45) DEFAULT NULL,"
                + " `CDA_CEA_Valid_To` varchar(45) DEFAULT NULL,"
                + " `BIR_Number` varchar(45) DEFAULT NULL,"
                + " `BIR_Valid_From` varchar(45) DEFAULT NULL,"
                + " `BIR_Valid_To` varchar(45) DEFAULT NULL,"
                + " `BSP_Accreditation_Year` varchar(45) DEFAULT NULL,"
                + " `TIN` varchar(45) DEFAULT NULL,"
                + " `PTR_Number` varchar(45) DEFAULT NULL,"
                + " `PTR_Valid_From` varchar(45) DEFAULT NULL,"
                + " `PTR_Valid_To` varchar(45) DEFAULT NULL,"
                + " PRIMARY KEY (`No_ID`),"
                + " UNIQUE KEY `Control_Number_UNIQUE` (`Control_Number`)"
                + " ) ENGINE=InnoDB AUTO_INCREMENT=13 DEFAULT CHARSET=utf8;";

            MySqlConnection crConnectionXIII = new MySqlConnection();
            MySqlCommand crCommandXIII = new MySqlCommand();

            try
            {
                crConnectionXIII = new MySqlConnection(crConn);
                crCommandXIII = new MySqlCommand(crNonQueryXIII, crConnectionXIII);

                crConnectionXIII.Open();

                crCommandXIII.ExecuteNonQuery();
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            finally
            {
                crConnectionXIII.Close();
            }
        }
        #endregion

        #region User Table
        void CUser()
        {
            string crConn = "Server = " + txtServer.Text + "; Database = user; Port = " + txtPort.Text + "; Uid = " + txtUsername.Text + "; Pwd = " + txtPassword.Text + ";";

            string crNonQuery = "CREATE TABLE IF NOT EXISTS `tbl_user` ("
                + " `No_ID` int(11) NOT NULL AUTO_INCREMENT,"
                + " `Username` varchar(45) DEFAULT NULL,"
                + " `Password` varchar(45) DEFAULT NULL,"
                + " `User_Type` varchar(45) DEFAULT NULL,"
                + " `Status` varchar(45) DEFAULT NULL,"
                + " `IP_Address` varchar(45) DEFAULT NULL,"
                + " PRIMARY KEY(`No_ID`),"
                + " UNIQUE KEY `Username_UNIQUE` (`Username`)"
                + " ) ENGINE = InnoDB DEFAULT CHARSET = utf8;";

            MySqlConnection crConnection = new MySqlConnection();
            MySqlCommand crCommand = new MySqlCommand();

            try
            {
                crConnection = new MySqlConnection(crConn);
                crCommand = new MySqlCommand(crNonQuery, crConnection);

                crConnection.Open();

                crCommand.ExecuteNonQuery();
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            finally
            {
                crConnection.Close();
            }

            string crNonQueryI = "CREATE TABLE IF NOT EXISTS `tbl_userlogs` ("
                + " `No_ID` int(11) NOT NULL AUTO_INCREMENT,"
                + " `Username` varchar(45) DEFAULT NULL,"
                + " `User_Type` varchar(45) DEFAULT NULL,"
                + " `Activity` varchar(45) DEFAULT NULL,"
                + " `Time` varchar(45) DEFAULT NULL,"
                + " `PC_Name` varchar(45) DEFAULT NULL,"
                + " PRIMARY KEY(`No_ID`)"
                + " ) ENGINE = InnoDB DEFAULT CHARSET = utf8;";

            MySqlConnection crConnectionI = new MySqlConnection();
            MySqlCommand crCommandI = new MySqlCommand();

            try
            {
                crConnectionI = new MySqlConnection(crConn);
                crCommandI = new MySqlCommand(crNonQueryI, crConnectionI);

                crConnectionI.Open();

                crCommandI.ExecuteNonQuery();
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            finally
            {
                crConnectionI.Close();
            }

            string cQuery = "INSERT IGNORE INTO tbl_user(No_ID, Username, Password, User_Type, Status) VALUES('1', 'admin', 'admin', 'Admin', 'Not Connected');";
            MySqlConnection cConnection = new MySqlConnection(crConn);
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

            string cQueryI = "INSERT IGNORE INTO tbl_user(No_ID, Username, Password, User_Type) VALUES('2', 'emdiscon', 'emdiscon', 'DC');";
            MySqlConnection cConnectionI = new MySqlConnection(crConn);
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
        #endregion

        #region Cancel
        private void btnCancel_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        #endregion

        #region Shortcut Keys
        private void SConn_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnOk_Click(sender, e);

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
        private void SConn_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = false;
                Application.Exit();
            }
        }
        #endregion

    }
}
