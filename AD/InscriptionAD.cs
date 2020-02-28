using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;

namespace Projet_PE.AD
{
    class InscriptionAD
    {
        internal class Inscription
        {
            private static SqlDataAdapter adapter = DataTables.getAdapterInscription();
            private static DataSet ds = DataTables.getDataSet();

            static internal DataTable GetInscription()
            {
                ds.Tables["Inscription"].Clear();
                adapter.Fill(ds, "Inscription");
                return ds.Tables["Inscription"];
            }

            static internal int UpdateInscription()
            {
                if (!ds.Tables["Inscription"].HasErrors)
                {
                    return adapter.Update(ds.Tables["Inscription"]);
                }
                else
                {
                    return -1;
                }
            }

            static internal bool insertInscription(string EtId, string CoursId, DateTime date)
            {
                bool updated = false;

                Connect.Connection.Open();

                SqlCommand cmd = Connect.Connection.CreateCommand();

                cmd.CommandText = "INSERT INTO Inscription (EtId, DateInscription, CoursId) " +
                        "VALUES ( @id , @date, @CoursId)";
                cmd.Parameters.Add("@id", SqlDbType.VarChar, 7);
                cmd.Parameters["@id"].Value = EtId;
                cmd.Parameters.Add("@date", SqlDbType.Date);
                cmd.Parameters["@date"].Value = date;
                cmd.Parameters.Add("@CoursId", SqlDbType.VarChar, 7);
                cmd.Parameters["@CoursId"].Value = CoursId;

                //try
                //{
                    cmd.ExecuteNonQuery();

                    updated = true;
                //}
                //catch (Exception)
                //{
                //    updated = false;
                //}

                Connect.Connection.Close();


                return updated;
            }

            static internal bool ModifierInscription(string id, string EtId, DateTime date, string CoursId)
            {
                bool updated = false;
                Connect.Connection.Open();

                SqlCommand cmd = Connect.Connection.CreateCommand();
                cmd.CommandText = "UPDATE Inscription SET EtId = @studId, DateInscription = @date, CoursId = @coursId" +
                    " WHERE (ID = @id) ";
                cmd.Parameters.Add("@id", SqlDbType.Int);
                cmd.Parameters["@id"].Value = id;
                cmd.Parameters.Add("@studId", SqlDbType.VarChar, 7);
                cmd.Parameters["@studId"].Value = EtId;
                cmd.Parameters.Add("@date", SqlDbType.Date);
                cmd.Parameters["@date"].Value = date;
                cmd.Parameters.Add("@coursid", SqlDbType.VarChar, 7);
                cmd.Parameters["@coursid"].Value = CoursId;

                //try
                //{
                    cmd.ExecuteNonQuery();

                    updated = true;
                //}
                //catch (Exception)
                //{
                //    updated = false;
                //}

                Connect.Connection.Close();
                return updated;
            }

            static internal bool SupprimerInscription(List<String[]> list)
            {
                bool updated = false;
                Connect.Connection.Open();
                SqlCommand cmd = new SqlCommand("", Connect.Connection);
                try
                {
                    cmd.Transaction = Connect.Connection.BeginTransaction();

                    foreach (String[] r in list)
                    {
                        cmd.CommandText = "DELETE from Inscription where (ID = '" +
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
        }
    }
}
