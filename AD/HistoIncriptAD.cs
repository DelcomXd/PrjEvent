using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

namespace Projet_PE.AD
{
    class HistoIncriptAD
    {
        internal class Histo_Inscript
        {
            private static SqlDataAdapter adapter = DataTables.getAdapterHistoInscript();
            private static DataSet ds = DataTables.getDataSet();

            static internal DataTable GetHistoInscript()
            {
                ds.Tables["Inscription"].Clear();
                adapter.Fill(ds, "Inscription");
                return ds.Tables["histo_inscript"];
            }

            static internal int UpdateHistoInscript()
            {
                if (!ds.Tables["histo_inscript"].HasErrors)
                {
                    return adapter.Update(ds.Tables["histo_inscript"]);
                }
                else
                {
                    return -1;
                }
            }

            static internal void insertHistoInscript(string StudentId, string CoursId, string CoursName, string ProgId, string ProgName, DateTime date)
            {

                Connect.Connection.Open();

                SqlCommand cmd = Connect.Connection.CreateCommand();

                cmd.CommandText = "INSERT INTO histo_inscript (EtId, CoursId, CoursName, ProgId, ProgName, DateInscription) " +
                        "VALUES ( @studentid , @coursid, @coursname, @ProgramId, @ProgName, @date)";

                cmd.Parameters.Add("@studentid", SqlDbType.VarChar, 7);
                cmd.Parameters["@studentid"].Value = StudentId;

                cmd.Parameters.Add("@coursid", SqlDbType.VarChar, 30);
                cmd.Parameters["@coursid"].Value = CoursId;

                cmd.Parameters.Add("@coursname", SqlDbType.VarChar, 30);
                cmd.Parameters["@coursname"].Value = CoursName;

                cmd.Parameters.Add("@ProgramId", SqlDbType.VarChar, 5);
                cmd.Parameters["@ProgramId"].Value = ProgId;

                cmd.Parameters.Add("@ProgName", SqlDbType.VarChar, 30);
                cmd.Parameters["@ProgName"].Value = ProgName;

                cmd.Parameters.Add("@date", SqlDbType.Date);
                cmd.Parameters["@date"].Value = date;

                //try
                //{
                cmd.ExecuteNonQuery();
                
                //}
                //catch (Exception)
                //{
                //    updated = false;
                //}

                Connect.Connection.Close();
                
            }

            static internal void ModifierHistoInscript(string id, string StudentId, string CoursId, string CoursName, string ProgId, DateTime date)
            {
                string progName = selectProgName(ProgId);
                Connect.Connection.Open();

                SqlCommand cmd = Connect.Connection.CreateCommand();
                cmd.CommandText = "UPDATE histo_inscript SET EtId = @EtId, CoursId = @CoursId, CoursName = @CoursName, ProgId = @ProgramId, ProgName = @ProgName" +
                    " WHERE (ID = @id) ";
                cmd.Parameters.Add("@id", SqlDbType.VarChar, 7);
                cmd.Parameters["@id"].Value = id;
                cmd.Parameters.Add("@EtId", SqlDbType.VarChar, 30);
                cmd.Parameters["@EtId"].Value = StudentId;
                cmd.Parameters.Add("@CoursId", SqlDbType.VarChar, 30);
                cmd.Parameters["@CoursId"].Value = CoursId;
                cmd.Parameters.Add("@CoursName", SqlDbType.VarChar, 5);
                cmd.Parameters["@CoursName"].Value = CoursName;
                cmd.Parameters.Add("@ProgramId", SqlDbType.VarChar, 30);
                cmd.Parameters["@ProgramId"].Value = ProgId;
                cmd.Parameters.Add("@ProgName", SqlDbType.VarChar, 30);
                cmd.Parameters["@ProgName"].Value = progName;
                //try
                //{
                cmd.ExecuteNonQuery();
                
                //}
                //catch (Exception)
                //{
                //    updated = false;
                //}

                Connect.Connection.Close();
            }

            static private string selectProgName(string progId)
            {
                string programName = "";
                Connect.Connection.Open();

                SqlCommand cmd = Connect.Connection.CreateCommand();
                cmd.CommandText = "Select ProgName FROM Programmes WHERE ProgId = @ProgramId";
                cmd.Parameters.Add("@ProgramId", SqlDbType.VarChar, 5);
                cmd.Parameters["@ProgramId"].Value = progId;

                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    programName = (string)reader[0];
                }

                Connect.Connection.Close();

                return programName;
            }
        }
    }
}
