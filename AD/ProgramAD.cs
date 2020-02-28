using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;

namespace Projet_PE.AD
{
    static class ProgramAD
    {
        internal class Program_
        {
            private static SqlDataAdapter adapter = DataTables.getAdapterProgram();
            private static DataSet ds = DataTables.getDataSet();

            static internal DataTable GetProgram()
            {
                ds.Tables["Programmes"].Clear();
                adapter.Fill(ds, "Programmes");
                return ds.Tables["Programmes"];
            }

            static internal int UpdateProgram()
            {
                if (!ds.Tables["Programmes"].HasErrors)
                {
                    return adapter.Update(ds.Tables["Programmes"]);
                }
                else
                {
                    return -1;
                }
            }

            static private string CountProgram()
            {
                Connect.Connection.Open();

                SqlCommand cmdCount = new SqlCommand("VCodeProgram", Connect.Connection);
                cmdCount.CommandType = CommandType.StoredProcedure;

                object obj = cmdCount.ExecuteScalar();

                Connect.Connection.Close();

                return obj.ToString();
            }

            static internal bool insertProgram(string ProgramName)
            {
                bool updated = false;
                string ProgId = CountProgram();

                Connect.Connection.Open();

                SqlCommand cmd = Connect.Connection.CreateCommand();

                cmd.CommandText = "INSERT INTO Programmes (ProgId, ProgName) " +
                        "VALUES ( @id , @name)";
                cmd.Parameters.Add("@id", SqlDbType.VarChar, 5);
                cmd.Parameters["@id"].Value = ProgId;
                cmd.Parameters.Add("@name", SqlDbType.VarChar, 30);
                cmd.Parameters["@name"].Value = ProgramName;

                try
                {
                    cmd.ExecuteNonQuery();

                    updated = true;
                }
                catch (Exception)
                {
                    updated = false;
                }

                Connect.Connection.Close();

                return updated;
            }

            static internal bool SupprimerProgram(List<String[]> list)
            {
                bool updated = false;
                Connect.Connection.Open();
                SqlCommand cmd = new SqlCommand("", Connect.Connection);
                try
                {
                    cmd.Transaction = Connect.Connection.BeginTransaction();

                    foreach (String[] r in list)
                    {
                        cmd.CommandText = "DELETE from Programmes where (ProgId = '" +
                            r[0] +
                            "') and (ProgName = '" + r[1] +
                            "')";
                        cmd.ExecuteNonQuery();
                    }
                    cmd.Transaction.Commit();
                    updated = true;
                }
                catch (Exception)
                {
                    updated = false;
                }
                Connect.Connection.Close();
                return updated;
            }

            static internal bool ModifierProgram(string PId, string PName)
            {
                bool updated = false;
                Connect.Connection.Open();

                SqlCommand cmd = Connect.Connection.CreateCommand();
                cmd.CommandText = "UPDATE Programmes SET ProgName = @name" +
                    " WHERE (ProgId = @id) ";
                cmd.Parameters.Add("@id", SqlDbType.VarChar, 5);
                cmd.Parameters["@id"].Value = PId;
                cmd.Parameters.Add("@name", SqlDbType.VarChar, 30);
                cmd.Parameters["@name"].Value = PName;
                try
                {
                    cmd.ExecuteNonQuery();

                    updated = true;
                }
                catch (Exception)
                {
                    updated = false;
                }

                Connect.Connection.Close();
                return updated;
            }


            static internal List<string> RecupNomColonne(string name)
            {
                List<string> prog_cn = new List<string>();
                prog_cn.Add("--Sélectionner--");
                Connect.Connection.Open();
                SqlCommand cmd = Connect.Connection.CreateCommand();
                cmd.CommandText = "SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = '" + name + "'; ";

                using (SqlDataReader obj = cmd.ExecuteReader())
                {
                    if (obj.HasRows)
                    {
                        while (obj.Read())
                        {
                            string item = obj.GetString(obj.GetOrdinal("COLUMN_NAME"));
                            prog_cn.Add(item);
                        }
                    }
                }

                Connect.Connection.Close();

                return prog_cn;
            }

            static internal DataTable Recherche(string column, string rec)
            {
                SqlDataAdapter adapterProgram;
                DataSet data = new DataSet();

                adapterProgram = new SqlDataAdapter("SELECT * FROM Programmes WHERE " + column + " = '" + rec + "'"
                , Connect.ConnectionString);
                adapterProgram.Fill(data, "Programmes");

                data.Tables["Programmes"].Columns["ProgId"].AllowDBNull = false;
                data.Tables["Programmes"].Columns["ProgName"].AllowDBNull = false;

                data.Tables["Programmes"].PrimaryKey = new DataColumn[1]
                        { data.Tables["Programmes"].Columns["ProgId"]};

                SqlCommandBuilder builder = new SqlCommandBuilder(adapterProgram);
                adapterProgram.UpdateCommand = builder.GetUpdateCommand();

                return data.Tables["Programmes"];
            }
        }
    }
}
