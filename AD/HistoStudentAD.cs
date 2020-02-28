using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

namespace Projet_PE.AD
{
    class HistoStudentAD
    {
        internal class Histo_Student
        {
            private static SqlDataAdapter adapter = DataTables.getAdapterHistoStudent();
            private static DataSet ds = DataTables.getDataSet();

            static internal DataTable GetHistoStudent()
            {
                ds.Tables["histo_etudiant"].Clear();
                adapter.Fill(ds, "histo_etudiant");
                return ds.Tables["histo_etudiant"];
            }

            static internal int UpdateHistoStudent()
            {
                if (!ds.Tables["histo_etudiant"].HasErrors)
                {
                    return adapter.Update(ds.Tables["histo_etudiant"]);
                }
                else
                {
                    return -1;
                }
            }
            
            static internal void insertHistoStudent(string StudentId, string LName, string FName, string ProgId)
            {
                string progName = selectProgName(ProgId);

                Connect.Connection.Open();

                SqlCommand cmd = Connect.Connection.CreateCommand();

                cmd.CommandText = "INSERT INTO Histo_etudiant (EtId, Nom, Prenom, ProgId, ProgName) " +
                        "VALUES ( @id , @LName, @FName, @ProgramId, @ProgName)";
                cmd.Parameters.Add("@id", SqlDbType.VarChar, 7);
                cmd.Parameters["@id"].Value = StudentId;
                cmd.Parameters.Add("@LName", SqlDbType.VarChar, 30);
                cmd.Parameters["@LName"].Value = LName;
                cmd.Parameters.Add("@FName", SqlDbType.VarChar, 30);
                cmd.Parameters["@FName"].Value = FName;
                cmd.Parameters.Add("@ProgramId", SqlDbType.VarChar, 5);
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

            static internal void ModifierHistoStudent(string EtId, string LName, string FName, string progId)
            {
                string progName = selectProgName(progId);
                Connect.Connection.Open();

                SqlCommand cmd = Connect.Connection.CreateCommand();
                cmd.CommandText = "UPDATE Histo_etudiant SET EtId = @EtId, Nom = @LName, Prenom = @FName, ProgId = @ProgramId, ProgName = @ProgName" +
                    " WHERE (EtId = @id) ";
                cmd.Parameters.Add("@id", SqlDbType.VarChar, 7);
                cmd.Parameters["@id"].Value = EtId;
                cmd.Parameters.Add("@LName", SqlDbType.VarChar, 30);
                cmd.Parameters["@LName"].Value = LName;
                cmd.Parameters.Add("@FName", SqlDbType.VarChar, 30);
                cmd.Parameters["@FName"].Value = FName;
                cmd.Parameters.Add("@ProgramId", SqlDbType.VarChar, 5);
                cmd.Parameters["@ProgramId"].Value = progId;
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
