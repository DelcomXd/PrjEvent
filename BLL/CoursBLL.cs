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
    class CoursBLL
    {
        private string coursId;
        private string coursName;
        private string programId;

        public string ProgramId { get => programId; set => programId = value; }
        public string CoursId { get => coursId; set => coursId = value; }
        public string CoursName { get => coursName; set => coursName = value; }

        public bool insertLigne()
        {
            return CoursAD.Cours.insertCours(coursName,programId);
        }

        public bool updateLigne()
        {
            return CoursAD.Cours.ModifierCours(coursId,coursName,programId);
        }

        public bool deleteLigne(List<String[]> list)
        {
            return CoursAD.Cours.SupprimerCours(list);
        }

        public DataTable selectProgId()
        {
            return CoursAD.Cours.ProgrammeIdDiponible();
        }

        public DataTable selectProgId(string progId)
        {
            return CoursAD.Cours.ProgrammeIdDiponible();
        }

        public bool verifData()
        {
            bool updated = false;

            using (SqlCommand sqlCommand = new SqlCommand("SELECT COUNT(*) from Cours where CoursName = @coursName AND ProgId = @ProgId", Connect.Connection))
            {
                Connect.Connection.Open();
                sqlCommand.Parameters.AddWithValue("@coursName", coursName);
                sqlCommand.Parameters.AddWithValue("@ProgId", programId);
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
    }
}
