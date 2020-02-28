using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using Projet_PE.AD;

namespace Projet_PE.BLL
{
    class PreReqBLL
    {
        private string id;
        private string prereq;
        private string coursid;

        public string Id { get => id; set => id = value; }
        public string Prereq { get => prereq; set => prereq = value; }
        public string Coursid { get => coursid; set => coursid = value; }

        public bool insertLigne()
        {
            return PreReqAD.PreReq.insertPreReq(this.coursid, this.prereq);
        }

        public bool updateLigne()
        {
            return PreReqAD.PreReq.ModifierPreReq(this.id, this.coursid, this.prereq);
        }

        public bool deleteLigne(List<String[]> list)
        {
            return PreReqAD.PreReq.SupprimerPreReq(list);
        }

        public DataTable SelectTableCours()
        {
            SqlDataAdapter adapterPId = new SqlDataAdapter(
                    "SELECT CoursId, CoursName FROM Cours ",
                    Connect.ConnectionString);

            DataTable dtPId = new DataTable();
            dtPId.Columns.Add("CoursId", typeof(String));
            dtPId.Columns.Add("CoursName", typeof(String));
            dtPId.Rows.Add(new object[2] { -1, "-- Sélectionnez --" });
            adapterPId.Fill(dtPId);
            return dtPId;
        }

        public bool verifData()
        {
            bool updated = false;

            using (SqlCommand sqlCommand = new SqlCommand("SELECT COUNT(*) from PreReq where CoursId = @CoursId AND PreReqId = @PreReq", Connect.Connection))
            {
                Connect.Connection.Open();
                sqlCommand.Parameters.AddWithValue("@CoursId", coursid);
                sqlCommand.Parameters.AddWithValue("@PreReq", prereq);
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
