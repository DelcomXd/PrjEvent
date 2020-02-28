using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;

namespace Projet_PE.AD
{
    class StudentAD
    {
        internal class Student
        {
            private static SqlDataAdapter adapter = DataTables.getAdapterStudent();
            private static DataSet ds = DataTables.getDataSet();

            static internal DataTable GetStudent()
            {
                ds.Tables["Etudiants"].Clear();
                adapter.Fill(ds, "Etudiants");
                return ds.Tables["Etudiants"];
            }

            static internal int UpdateStudent()
            {
                if (!ds.Tables["Etudiants"].HasErrors)
                {
                    return adapter.Update(ds.Tables["Etudiants"]);
                }
                else
                {
                    return -1;
                }
            }

            static private string CountStud()
            {
                Connect.Connection.Open();

                SqlCommand cmdCount = new SqlCommand("VCodeStudent", Connect.Connection);
                cmdCount.CommandType = CommandType.StoredProcedure;

                object obj = cmdCount.ExecuteScalar();

                Connect.Connection.Close();

                return obj.ToString();
            }

            static internal bool insertStudent(string studentLName, string studentFName, string programId, DateTime initial)
            {
                bool updated = false;
                string studentId = CountStud();

                Connect.Connection.Open();

                SqlCommand cmd = Connect.Connection.CreateCommand();

                cmd.CommandText = "INSERT INTO Etudiants (EtId, Nom, Prenom, ProgId, DateInitial) " +
                        "VALUES ( @id , @Lname, @Fname, @Pid, @Date)";
                cmd.Parameters.Add("@id", SqlDbType.VarChar, 7);
                cmd.Parameters["@id"].Value = studentId;
                cmd.Parameters.Add("@Lname", SqlDbType.VarChar, 30);
                cmd.Parameters["@Lname"].Value = studentLName;
                cmd.Parameters.Add("@Fname", SqlDbType.VarChar, 30);
                cmd.Parameters["@Fname"].Value = studentFName;
                cmd.Parameters.Add("@Pid", SqlDbType.VarChar, 5);
                cmd.Parameters["@Pid"].Value = programId;
                cmd.Parameters.Add("@Date", SqlDbType.Date);
                cmd.Parameters["@Date"].Value = initial;

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

                if (updated)
                {
                    HistoStudentAD.Histo_Student.insertHistoStudent(studentId, studentLName, studentFName, programId);
                }

                return updated;
            }

            static internal bool SupprimerStudent(List<String[]> list)
            {
                bool updated = false;
                Connect.Connection.Open();
                SqlCommand cmd = new SqlCommand("", Connect.Connection);
                try
                {
                    cmd.Transaction = Connect.Connection.BeginTransaction();

                    foreach (String[] r in list)
                    {
                        cmd.CommandText = "DELETE from Etudiants where (EtId = '" +
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

            static internal bool ModifierStudent(string EId, string LName, string FName, string PId)
            {
                bool updated = false;
                Connect.Connection.Open();

                SqlCommand cmd = Connect.Connection.CreateCommand();
                cmd.CommandText = "UPDATE Etudiants SET Nom = @LName, Prenom = @FName, ProgId = @Pid" +
                    " WHERE (EtId = @id) ";
                cmd.Parameters.Add("@id", SqlDbType.VarChar, 7);
                cmd.Parameters["@id"].Value = EId;
                cmd.Parameters.Add("@Pid", SqlDbType.VarChar, 5);
                cmd.Parameters["@Pid"].Value = PId;
                cmd.Parameters.Add("@LName", SqlDbType.VarChar, 30);
                cmd.Parameters["@LName"].Value = LName;
                cmd.Parameters.Add("@FName", SqlDbType.VarChar, 30);
                cmd.Parameters["@FName"].Value = FName;
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
