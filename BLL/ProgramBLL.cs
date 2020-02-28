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
    class ProgramBLL
    {
        private string programId;
        private string programName;

        public string ProgramId { get => programId; set => programId = value; }
        public string ProgramName { get => programName; set => programName = value; }

        public bool insertLigne()
        {
            return ProgramAD.Program_.insertProgram(programName);
        }

        public bool updateLigne()
        {
            return ProgramAD.Program_.ModifierProgram(ProgramId, ProgramName);
        }

        public bool deleteLigne(List<String[]> list)
        {
            return ProgramAD.Program_.SupprimerProgram(list);
        }

        public bool verifData()
        {
            bool updated = false;

            using (SqlCommand sqlCommand = new SqlCommand("SELECT COUNT(*) from Programmes where ProgName = @ProgName", Connect.Connection))
            {
                Connect.Connection.Open();
                sqlCommand.Parameters.AddWithValue("@ProgName", programName);
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
