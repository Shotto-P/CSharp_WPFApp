using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using MySql.Data;

namespace RAP_WithWPF.Database
{
    public abstract class ERDAdapter
    {
        //private static bool reportingErrors = false;

        protected const string db = "kit206";
        protected const string user = "kit206";
        protected const string pwd = "kit206";
        protected const string server = "alacritas.cis.utas.edu.au";
        protected static MySqlConnection conn = null;

        //parse string to enum
        public static T ParseEnum<T>(string value)
        {
            return (T)Enum.Parse(typeof(T), value);
        }

        protected static MySqlConnection GetConnection()
        {
            if (conn == null)
            {
                string connStr = String.Format("Database={0};Data Source={1};User Id={2};Password={3};SSL Mode=None", db, server, user, pwd);
                conn = new MySqlConnection(connStr);
            }
            return conn;
        }

    }
}
