using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Nieva
{
    class Conn
    {
        public static string cString = "Server = " + Login.cserver + "; Database = nieva; Port = " + Login.cport + "; Uid = " + Login.cuid + "; Pwd = " + Login.cpwd + ";";

        public static string uString = "Server = " + Login.cserver + "; Database = user; Port = " + Login.cport + "; Uid = " + Login.cuid + "; Pwd = " + Login.cpwd + ";";
    }
}
