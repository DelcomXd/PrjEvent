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
    class InscriptionBLL
    {

        private string id;
        private string studentId;
        private DateTime date;
        private string coursId;
        private string nom;
        private string prenom;

        public string Id { get => id; set => id = value; }
        public DateTime Date { get => date; set => date = value; }
        public string CoursId { get => coursId; set => coursId = value; }
        public string StudentId { get => studentId; set => studentId = value; }
        public string Nom { get => nom; set => nom = value; }
        public string Prenom { get => prenom; set => prenom = value; }

        public bool insertLigne()
        {
            HistoIncriptAD.Histo_Inscript.insertHistoInscript(this.studentId,this.coursId,SelectCoursNameID(CoursId),SelectProgId(this.nom,this.prenom),SelectProgNameID(SelectProgId(this.nom,this.prenom)),this.date);
            return InscriptionAD.Inscription.insertInscription(this.studentId,this.coursId, this.date);
        }

        public bool verifData()
        {
            bool updated = false;

            using (SqlCommand sqlCommand = new SqlCommand("SELECT COUNT(*) from Inscription" +
                " where EtId = @etid " +
                "AND CoursId = @coursid ", Connect.Connection))
            {
                Connect.Connection.Open();
                sqlCommand.Parameters.AddWithValue("@etid", studentId);
                sqlCommand.Parameters.AddWithValue("@coursid", coursId);
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
            return InscriptionAD.Inscription.ModifierInscription(this.id, this.studentId, this.date, this.coursId);
        }

        public bool deleteLigne(List<String[]> list)
        {
            return InscriptionAD.Inscription.SupprimerInscription(list);
        }
        
        static internal string SelectProgId(string nom, string prenom)
        {
            string progID = "";

            Connect.Connection.Open();

            SqlCommand cmd = Connect.Connection.CreateCommand();

            cmd.CommandText = "SELECT ProgId FROM Etudiants " +
                    "WHERE Nom = @Lname " +
                    "AND Prenom = @Fname";
            cmd.Parameters.Add("@Lname", SqlDbType.VarChar, 30);
            cmd.Parameters["@Lname"].Value = nom;
            cmd.Parameters.Add("@Fname", SqlDbType.VarChar, 30);
            cmd.Parameters["@Fname"].Value = prenom;
            cmd.ExecuteNonQuery();

            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    progID = (string)reader[0];
                }
            }

            Connect.Connection.Close();

            return progID;
        }

        public DataTable SelectCours(string progId)
        {
            SqlDataAdapter adapterPId = new SqlDataAdapter(
                    "SELECT CoursId, CoursName FROM Cours " +
                    "WHERE ProgId = '" + progId + "'",
                    Connect.ConnectionString);

            DataTable dtPId = new DataTable();
            dtPId.Columns.Add("CoursId", typeof(String));
            dtPId.Columns.Add("CoursName", typeof(String));
            dtPId.Rows.Add(new object[2] { -1, "-- Sélectionnez --" });
            adapterPId.Fill(dtPId);
            return dtPId;
        }

        public DataTable SelectEtudiantNom()
        {
            SqlDataAdapter adapterPId = new SqlDataAdapter(
                    "SELECT DISTINCT Nom FROM Etudiants",
                    Connect.ConnectionString);

            DataTable dtPId = new DataTable();
            dtPId.Columns.Add("Nom", typeof(String));
            dtPId.Rows.Add(new object[1] {"-- Sélectionnez --" });
            adapterPId.Fill(dtPId);
            return dtPId;
        }

        public DataTable SelectEtudiantPrenom(string nom)
        {
            SqlDataAdapter adapterPId = new SqlDataAdapter(
                    "SELECT EtId, Prenom, ProgId FROM Etudiants " +
                    "WHERE Nom = '" + nom + "'",
                    Connect.ConnectionString);

            DataTable dtPId = new DataTable();
            dtPId.Columns.Add("EtId", typeof(String));
            dtPId.Columns.Add("Prenom", typeof(String));
            dtPId.Columns.Add("Nom", typeof(String));
            dtPId.Rows.Add(new object[2] { -1, "-- Sélectionnez --" });
            adapterPId.Fill(dtPId);
            return dtPId;
        }

        static internal string SelectEtudiantNomID(string studId)
        {
            string nom = "";
            Connect.Connection.Open();

            SqlCommand cmd = Connect.Connection.CreateCommand();

            cmd.CommandText = "SELECT Nom FROM Etudiants " +
                    "WHERE EtId = @etid";
            cmd.Parameters.Add("@etid", SqlDbType.VarChar, 7);
            cmd.Parameters["@etid"].Value = studId;
            cmd.ExecuteNonQuery();

            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    nom = (string)reader[0];
                }
            }

            Connect.Connection.Close();

            return nom;
        }

        static internal string SelectEtudiantPrenomID(string studId)
        {
            string prenom = "";
            Connect.Connection.Open();

            SqlCommand cmd = Connect.Connection.CreateCommand();

            cmd.CommandText = "SELECT Prenom FROM Etudiants " +
                    "WHERE EtId = @etid";
            cmd.Parameters.Add("@etid", SqlDbType.VarChar, 7);
            cmd.Parameters["@etid"].Value = studId;
            cmd.ExecuteNonQuery();

            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    prenom = (string)reader[0];
                }
            }

            Connect.Connection.Close();

            return prenom;
        }

        static internal string SelectCoursNameID(string CID)
        {
            string nom = "";
            Connect.Connection.Open();

            SqlCommand cmd = Connect.Connection.CreateCommand();

            cmd.CommandText = "SELECT CoursName FROM Cours " +
                    "WHERE CoursId = @coursid";
            cmd.Parameters.Add("@coursid", SqlDbType.VarChar, 7);
            cmd.Parameters["@coursid"].Value = CID;
            cmd.ExecuteNonQuery();

            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    nom = (string)reader[0];
                }
            }

            Connect.Connection.Close();

            return nom;
        }

        static internal string SelectProgNameID(string PID)
        {
            string nom = "";
            Connect.Connection.Open();

            SqlCommand cmd = Connect.Connection.CreateCommand();

            cmd.CommandText = "SELECT ProgName FROM Programmes " +
                    "WHERE ProgId = @progid";
            cmd.Parameters.Add("@progid", SqlDbType.VarChar, 5);
            cmd.Parameters["@progid"].Value = PID;
            cmd.ExecuteNonQuery();

            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    nom = (string)reader[0];
                }
            }

            Connect.Connection.Close();

            return nom;
        }
    }
}
