using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

namespace Projet_PE.AD
{
    class PreReqAD
    {

        internal class PreReq
        {
            private static SqlDataAdapter adapter = DataTables.getAdapterPreReq();
            private static DataSet ds = DataTables.getDataSet();

            static internal DataTable GetPreReq()
            {
                ds.Tables["PreReq"].Clear();
                adapter.Fill(ds, "PreReq");
                return ds.Tables["PreReq"];
            }

            static internal int UpdatePreReq()
            {
                if (!ds.Tables["PreReq"].HasErrors)
                {
                    return adapter.Update(ds.Tables["PreReq"]);
                }
                else
                {
                    return -1;
                }
            }

            static internal bool insertPreReq(string CoursId, string PreReq)
            {
                bool updated = false;

                Connect.Connection.Open();

                SqlCommand cmd = Connect.Connection.CreateCommand();

                cmd.CommandText = "INSERT INTO PreReq (CoursId, PreReqId) " +
                        "VALUES ( @id , @PreReq)";
                cmd.Parameters.Add("@id", SqlDbType.VarChar, 7);
                cmd.Parameters["@id"].Value = CoursId;
                cmd.Parameters.Add("@PreReq", SqlDbType.VarChar, 7);
                cmd.Parameters["@PreReq"].Value = PreReq;

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

            static internal bool SupprimerPreReq(List<String[]> list)
            {
                bool updated = false;
                Connect.Connection.Open();
                SqlCommand cmd = new SqlCommand("", Connect.Connection);
                try
                {
                    cmd.Transaction = Connect.Connection.BeginTransaction();

                    foreach (String[] r in list)
                    {
                        cmd.CommandText = "DELETE from PreReq where (ID = '" +
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

            static internal bool ModifierPreReq(string id, string CId, string PreReq)
            {
                bool updated = false;
                Connect.Connection.Open();

                SqlCommand cmd = Connect.Connection.CreateCommand();
                cmd.CommandText = "UPDATE PreReq SET CoursId = @CId, PreReqId = @PreReq" +
                    " WHERE (ID = @id) ";
                cmd.Parameters.Add("@id", SqlDbType.Int);
                cmd.Parameters["@id"].Value = id;
                cmd.Parameters.Add("@CId", SqlDbType.VarChar, 7);
                cmd.Parameters["@CId"].Value = CId;
                cmd.Parameters.Add("@PreReq", SqlDbType.VarChar, 7);
                cmd.Parameters["@PreReq"].Value = PreReq;
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
        }
    }
}
