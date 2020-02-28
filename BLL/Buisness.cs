using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using Projet_PE.AD;

namespace Projet_PE.BLL
{
    class Buisness
    {
        static public bool VerifyConnection(string name, string pwd, string hash)
        {
            SqlDataAdapter adapterUserLog = new SqlDataAdapter("SELECT COUNT(*) FROM UserLog Where UserName = '" + name +
                    "' AND MotDePasse = '" + Crypt_str.Encrypt_str(pwd, hash) + "'"
                    , Connect.ConnectionString);

            DataTable dt = new DataTable();
            adapterUserLog.Fill(dt);
            if (dt.Rows[0][0].ToString() == "1")
            {
                return true;
            }
            else
                return false;
        }
        
        static public string SelectUserType(string name, string pwd, string hash)
        {
            SqlDataAdapter adapterUserLog = new SqlDataAdapter("SELECT UserType FROM UserLog Where UserName = '" + name +
                       "' AND MotDePasse = '" + Crypt_str.Encrypt_str(pwd, hash) + "'"
                       , Connect.ConnectionString);

            DataTable dt = new DataTable();
            adapterUserLog.Fill(dt);
            return dt.Rows[0][0].ToString();
        }
    }
}
