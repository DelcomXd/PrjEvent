using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;

namespace Projet_PE.AD
{
    static class CoursAD
    {
        internal class Cours
        {
            private static SqlDataAdapter adapter = DataTables.getAdapterCours();
            private static DataSet ds = DataTables.getDataSet();

            static internal DataTable GetCours()
            {
                ds.Tables["Cours"].Clear();
                adapter.Fill(ds, "Cours");
                return ds.Tables["Cours"];
            }

            static internal int UpdateCours()
            {
                if (!ds.Tables["Cours"].HasErrors)
                {
                    return adapter.Update(ds.Tables["Cours"]);
                }
                else
                {
                    return -1;
                }
            }

            static private string CountCours()
            {
                Connect.Connection.Open();

                SqlCommand cmdCount = new SqlCommand("VCodeCours", Connect.Connection);
                cmdCount.CommandType = CommandType.StoredProcedure;

                object obj = cmdCount.ExecuteScalar();

                Connect.Connection.Close();

                return obj.ToString();
            }

            static internal bool insertCours(string CoursName, string ProgId)
            {
                bool updated = false;
                string CoursId = CountCours();

                Connect.Connection.Open();

                SqlCommand cmd = Connect.Connection.CreateCommand();

                cmd.CommandText = "INSERT INTO Cours (CoursId, CoursName, ProgId) " +
                        "VALUES ( @id , @name, @ProgramId)";
                cmd.Parameters.Add("@id", SqlDbType.VarChar, 7);
                cmd.Parameters["@id"].Value = CoursId;
                cmd.Parameters.Add("@name", SqlDbType.VarChar, 30);
                cmd.Parameters["@name"].Value = CoursName;
                cmd.Parameters.Add("@ProgramId", SqlDbType.VarChar, 5);
                cmd.Parameters["@ProgramId"].Value = ProgId;

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

            static internal bool SupprimerCours(List<String[]> list)
            {
                bool updated = false;
                Connect.Connection.Open();
                SqlCommand cmd = new SqlCommand("", Connect.Connection);
                try
                {
                    cmd.Transaction = Connect.Connection.BeginTransaction();

                    foreach (String[] r in list)
                    {
                        cmd.CommandText = "DELETE from Cours where (CoursId = '" +
                            r[0] +
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

            static internal bool ModifierCours(string CId, string CName, string PId)
            {
                bool updated = false;
                Connect.Connection.Open();

                SqlCommand cmd = Connect.Connection.CreateCommand();
                cmd.CommandText = "UPDATE Cours SET CoursName = @name, ProgId = @Pid" +
                    " WHERE (CoursId = @id) ";
                cmd.Parameters.Add("@id", SqlDbType.VarChar, 7);
                cmd.Parameters["@id"].Value = CId;
                cmd.Parameters.Add("@Pid", SqlDbType.VarChar, 5);
                cmd.Parameters["@Pid"].Value = PId;
                cmd.Parameters.Add("@name", SqlDbType.VarChar, 30);
                cmd.Parameters["@name"].Value = CName;
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

            static internal DataTable ProgrammeIdDiponible()
            {
                SqlDataAdapter adapterPId = new SqlDataAdapter(
                        "SELECT ProgId, ProgName FROM Programmes",
                        Connect.ConnectionString);

                DataTable dtPId = new DataTable();
                dtPId.Columns.Add("ProgId", typeof(String));
                dtPId.Columns.Add("ProgName", typeof(String));
                dtPId.Rows.Add(new object[2] { -1, "-- Sélectionnez --" });
                adapterPId.Fill(dtPId);
                return dtPId;
            }

        }
    }
}
