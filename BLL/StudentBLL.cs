using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Projet_PE.AD;
using System.Data;
using System.Data.SqlClient;

namespace Projet_PE.BLL
{
    class StudentBLL
    {
        private string studentId;
        private string studentFN;
        private string studentLN;
        private string programId;
        private DateTime initial;

        public string StudentId { get => studentId; set => studentId = value; }
        public string StudentFN { get => studentFN; set => studentFN = value; }
        public string StudentLN { get => studentLN; set => studentLN = value; }
        public string ProgramId { get => programId; set => programId = value; }
        public DateTime Initial { get => initial; set => initial = value; }

        public bool insertLigne()
        {
            return StudentAD.Student.insertStudent(studentLN, studentFN,programId,initial);
        }

        public bool verifData()
        {
            bool updated = false;

            using (SqlCommand sqlCommand = new SqlCommand("SELECT COUNT(*) from Etudiants"+
                " where Nom = @LName " +
                "AND Prenom = @FName " + 
                "AND ProgId = @ProgramId ", Connect.Connection))
            {
                Connect.Connection.Open();
                sqlCommand.Parameters.AddWithValue("@LName", studentLN);
                sqlCommand.Parameters.AddWithValue("@FName", studentFN);
                sqlCommand.Parameters.AddWithValue("@ProgramId", programId);
                SqlDataReader reader = sqlCommand.ExecuteReader();

                if (reader.Read())
                {
                    if ((int)reader[0] > 0)
                    {
                        updated = false;
                    }
                    else
                    {
                        updated = true;
                    }
                }
                Connect.Connection.Close();
            }

            return updated;
        }

        public bool updateLigne()
        {
            return StudentAD.Student.ModifierStudent(studentId,studentLN,studentFN,programId);
        }

        public bool deleteLigne(List<String[]> list)
        {
            return StudentAD.Student.SupprimerStudent(list);
        }
    }
}
