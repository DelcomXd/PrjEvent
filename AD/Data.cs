using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using System.Security.Cryptography;

namespace Projet_PE.AD
{
    internal class Connect
    {
        private String connectionString;
        private SqlConnection con;
        private Connect()
        {
            SqlConnectionStringBuilder cs = new SqlConnectionStringBuilder();
            cs.DataSource = "(local)";
            cs.InitialCatalog = "PROGEVENT";
            cs.UserID = "sa";
            cs.Password = "Ilove99acb";
            this.connectionString = cs.ConnectionString;
            this.con = new SqlConnection(this.connectionString);
        }
        static private Connect singleton = new Connect();
        static internal SqlConnection Connection { get => singleton.con; }
        static internal String ConnectionString { get => singleton.connectionString; }
    }

    internal class Crypt_str
    {
        static public string Encrypt_str(string password, string hash)
        {
            byte[] data = UTF8Encoding.UTF8.GetBytes(password);
            using (MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider())
            {
                byte[] keys = md5.ComputeHash(UTF8Encoding.UTF8.GetBytes(hash));
                using (TripleDESCryptoServiceProvider tripleDes = new TripleDESCryptoServiceProvider() { Key = keys, Mode = CipherMode.ECB, Padding = PaddingMode.PKCS7 })
                {
                    ICryptoTransform transform = tripleDes.CreateEncryptor();
                    byte[] results = transform.TransformFinalBlock(data, 0, data.Length);
                    return Convert.ToBase64String(results, 0, results.Length);
                }
            }
        }

        static public string Decrypt_str(string password, string hash)
        {
            byte[] data = Convert.FromBase64String(password);
            using (MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider())
            {
                byte[] keys = md5.ComputeHash(UTF8Encoding.UTF8.GetBytes(hash));
                using (TripleDESCryptoServiceProvider tripleDes = new TripleDESCryptoServiceProvider() { Key = keys, Mode = CipherMode.ECB, Padding = PaddingMode.PKCS7 })
                {
                    ICryptoTransform transform = tripleDes.CreateDecryptor();
                    byte[] results = transform.TransformFinalBlock(data, 0, data.Length);
                    return UTF8Encoding.UTF8.GetString(results);
                }
            }
        }
    }

    internal class DataTables
    {
        private SqlDataAdapter adapterStudent;
        private SqlDataAdapter adapterProgram;
        private SqlDataAdapter adapterCours;
        private SqlDataAdapter adapterInscription;
        private SqlDataAdapter adapterHistoStudent;
        private SqlDataAdapter adapterHistoInscript;
        private SqlDataAdapter adapterPreReq;

        private DataSet ds = new DataSet();

        private void loadStudent()
        {
            adapterStudent = new SqlDataAdapter("SELECT * FROM Etudiants"
                , Connect.ConnectionString);
            adapterStudent.Fill(ds, "Etudiants");

            ds.Tables["Etudiants"].Columns["EtId"].AllowDBNull = false;
            ds.Tables["Etudiants"].Columns["Nom"].AllowDBNull = false;
            ds.Tables["Etudiants"].Columns["Prenom"].AllowDBNull = false;
            ds.Tables["Etudiants"].Columns["ProgID"].AllowDBNull = true;
            ds.Tables["Etudiants"].Columns["DateInitial"].AllowDBNull = true;

            ds.Tables["Etudiants"].PrimaryKey = new DataColumn[1]
                    { ds.Tables["Etudiants"].Columns["EtId"]};

            ForeignKeyConstraint FK_Et_ProgId = new ForeignKeyConstraint("FK_Et_ProgId",
           new DataColumn[]{
                ds.Tables["Programmes"].Columns["ProgId"]
           },
           new DataColumn[] {
                ds.Tables["Etudiants"].Columns["ProgId"],
           }
           );
            FK_Et_ProgId.DeleteRule = Rule.None;

            SqlCommandBuilder builder = new SqlCommandBuilder(adapterStudent);
            adapterStudent.UpdateCommand = builder.GetUpdateCommand();
        }

        private void loadProgram()
        {
            adapterProgram = new SqlDataAdapter("SELECT * FROM Programmes ORDER BY ProgId"
                , Connect.ConnectionString);
            adapterProgram.Fill(ds, "Programmes");

            ds.Tables["Programmes"].Columns["ProgId"].AllowDBNull = false;
            ds.Tables["Programmes"].Columns["ProgName"].AllowDBNull = false;

            ds.Tables["Programmes"].PrimaryKey = new DataColumn[1]
                    { ds.Tables["Programmes"].Columns["ProgId"]};

            SqlCommandBuilder builder = new SqlCommandBuilder(adapterProgram);
            adapterProgram.UpdateCommand = builder.GetUpdateCommand();
        }

        private void loadCours()
        {
            adapterCours = new SqlDataAdapter("SELECT * FROM Cours ORDER BY CoursId"
                , Connect.ConnectionString);
            adapterCours.Fill(ds, "Cours");

            ds.Tables["Cours"].Columns["CoursId"].AllowDBNull = true;
            ds.Tables["Cours"].Columns["CoursName"].AllowDBNull = false;
            ds.Tables["Cours"].Columns["ProgId"].AllowDBNull = false;

            ds.Tables["Cours"].PrimaryKey = new DataColumn[1]
                    { ds.Tables["Cours"].Columns["CoursId"]};

            ForeignKeyConstraint FK_Cours_ProgId = new ForeignKeyConstraint("FK_Cours_ProgId",
           new DataColumn[]{
                ds.Tables["Programmes"].Columns["ProgId"]
           },
           new DataColumn[] {
                ds.Tables["Cours"].Columns["ProgId"],
           }
           );
            FK_Cours_ProgId.DeleteRule = Rule.None;

            SqlCommandBuilder builder = new SqlCommandBuilder(adapterCours);
            adapterCours.UpdateCommand = builder.GetUpdateCommand();
        }

        private void loadInscription()
        {
            adapterInscription = new SqlDataAdapter("SELECT * FROM Inscription ORDER BY EtId"
                , Connect.ConnectionString);
            adapterInscription.Fill(ds, "Inscription");

            ds.Tables["Inscription"].Columns["ID"].AllowDBNull = false;
            ds.Tables["Inscription"].Columns["EtId"].AllowDBNull = false;
            ds.Tables["Inscription"].Columns["DateInscription"].AllowDBNull = false;
            ds.Tables["Inscription"].Columns["CoursId"].AllowDBNull = false;

            ds.Tables["Inscription"].PrimaryKey = new DataColumn[1]
                    { ds.Tables["Inscription"].Columns["ID"]};

            ForeignKeyConstraint FK_Inscript_EtId = new ForeignKeyConstraint("FK_Inscript_ProgId",
           new DataColumn[]{
                ds.Tables["Etudiants"].Columns["EtId"]
           },
           new DataColumn[] {
                ds.Tables["Inscription"].Columns["EtId"],
           }
           );

            ForeignKeyConstraint FK_Inscript_CoursId = new ForeignKeyConstraint("FK_Inscript_CoursId",
           new DataColumn[]{
                ds.Tables["Cours"].Columns["CoursId"]
           },
           new DataColumn[] {
                ds.Tables["Inscription"].Columns["CoursId"],
           }
           );
            FK_Inscript_EtId.DeleteRule = Rule.None;
            FK_Inscript_CoursId.DeleteRule = Rule.None;

            SqlCommandBuilder builder = new SqlCommandBuilder(adapterInscription);
            adapterInscription.UpdateCommand = builder.GetUpdateCommand();
        }

        private void loadHistoStudent()
        {
            adapterHistoStudent = new SqlDataAdapter("SELECT * FROM histo_etudiant" +
                " ORDER BY EtId"
                , Connect.ConnectionString);
            adapterHistoStudent.Fill(ds, "histo_etudiant");

            ds.Tables["histo_etudiant"].Columns["ID"].AllowDBNull = false;
            ds.Tables["histo_etudiant"].Columns["EtId"].AllowDBNull = false;
            ds.Tables["histo_etudiant"].Columns["Nom"].AllowDBNull = false;
            ds.Tables["histo_etudiant"].Columns["Prenom"].AllowDBNull = false;
            ds.Tables["histo_etudiant"].Columns["ProgId"].AllowDBNull = false;
            ds.Tables["histo_etudiant"].Columns["ProgName"].AllowDBNull = false;

            ds.Tables["histo_etudiant"].PrimaryKey = new DataColumn[1]
                    { ds.Tables["histo_etudiant"].Columns["ID"]};

            ForeignKeyConstraint FK_histoStu_EtId = new ForeignKeyConstraint("FK_histoStu_EtId",
           new DataColumn[]{
                ds.Tables["Etudiants"].Columns["EtId"]
           },
           new DataColumn[] {
                ds.Tables["histo_etudiant"].Columns["EtId"],
           }
           );

            ForeignKeyConstraint FK_histoStu_ProgId = new ForeignKeyConstraint("FK_histoStu_ProgId",
           new DataColumn[]{
                ds.Tables["Programmes"].Columns["ProgId"]
           },
           new DataColumn[] {
                ds.Tables["histo_etudiant"].Columns["ProgId"],
           }
           );
            FK_histoStu_EtId.DeleteRule = Rule.None;
            FK_histoStu_ProgId.DeleteRule = Rule.None;

            SqlCommandBuilder builder = new SqlCommandBuilder(adapterHistoStudent);
            adapterHistoStudent.UpdateCommand = builder.GetUpdateCommand();
        }

        private void loadHistoInscript()
        {
            adapterHistoInscript = new SqlDataAdapter("SELECT * FROM histo_inscript" +
                " ORDER BY ID"
                , Connect.ConnectionString);
            adapterHistoInscript.Fill(ds, "histo_inscript");

            ds.Tables["histo_inscript"].Columns["ID"].AllowDBNull = false;
            ds.Tables["histo_inscript"].Columns["EtId"].AllowDBNull = false;

            ds.Tables["histo_inscript"].Columns["CoursId"].AllowDBNull = false;
            ds.Tables["histo_inscript"].Columns["CoursName"].AllowDBNull = false;

            ds.Tables["histo_inscript"].Columns["ProgId"].AllowDBNull = false;
            ds.Tables["histo_inscript"].Columns["ProgName"].AllowDBNull = false;

            ds.Tables["histo_inscript"].Columns["DateInscription"].AllowDBNull = false;
            ds.Tables["histo_inscript"].Columns["NoteFinale"].AllowDBNull = true;
            ds.Tables["histo_inscript"].Columns["Situation"].AllowDBNull = true;

            ds.Tables["histo_inscript"].PrimaryKey = new DataColumn[1]
                    { ds.Tables["histo_inscript"].Columns["ID"]};

            ForeignKeyConstraint FK_histoInscript_EtId = new ForeignKeyConstraint("FK_histoInscript_EtId",
           new DataColumn[]{
                ds.Tables["Etudiants"].Columns["EtId"]
           },
           new DataColumn[] {
                ds.Tables["histo_inscript"].Columns["EtId"],
           }
           );

            ForeignKeyConstraint FK_histoInscript_CoursId = new ForeignKeyConstraint("FK_histoInscript_CoursId",
           new DataColumn[]{
                ds.Tables["Cours"].Columns["CoursId"]
           },
           new DataColumn[] {
                ds.Tables["histo_inscript"].Columns["CoursId"],
           }
           );

            ForeignKeyConstraint FK_histoStu_ProgId = new ForeignKeyConstraint("FK_histoStu_ProgId",
           new DataColumn[]{
                ds.Tables["Programmes"].Columns["ProgId"]
           },
           new DataColumn[] {
                ds.Tables["histo_etudiant"].Columns["ProgId"],
           }
           );
            FK_histoInscript_EtId.DeleteRule = Rule.None;
            FK_histoInscript_CoursId.DeleteRule = Rule.None;
            FK_histoStu_ProgId.DeleteRule = Rule.None;

            SqlCommandBuilder builder = new SqlCommandBuilder(adapterHistoInscript);
            adapterHistoInscript.UpdateCommand = builder.GetUpdateCommand();
        }

        private void loadPreReq()
        {
            adapterPreReq = new SqlDataAdapter("SELECT * FROM PreReq"
                , Connect.ConnectionString);
            adapterPreReq.Fill(ds, "PreReq");

            ds.Tables["PreReq"].Columns["ID"].AllowDBNull = false;
            ds.Tables["PreReq"].Columns["CoursId"].AllowDBNull = true;
            ds.Tables["PreReq"].Columns["PreReqId"].AllowDBNull = true;

            ds.Tables["PreReq"].PrimaryKey = new DataColumn[1]
                    { ds.Tables["PreReq"].Columns["ID"]};

            ForeignKeyConstraint FK_PreReq_CoursId = new ForeignKeyConstraint("FK_PreReq_CoursId",
           new DataColumn[]{
                ds.Tables["Cours"].Columns["CoursId"]
           },
           new DataColumn[] {
                ds.Tables["PreReq"].Columns["CoursId"],
           }
           );

            ForeignKeyConstraint FK_PreReq_CoursId_ = new ForeignKeyConstraint("FK_PreReq_CoursId_",
           new DataColumn[]{
                ds.Tables["Cours"].Columns["CoursId"]
           },
           new DataColumn[] {
                ds.Tables["PreReq"].Columns["PreReqId"],
           }
           );
            FK_PreReq_CoursId.DeleteRule = Rule.None;
            FK_PreReq_CoursId_.DeleteRule = Rule.None;

            SqlCommandBuilder builder = new SqlCommandBuilder(adapterPreReq);
            adapterPreReq.UpdateCommand = builder.GetUpdateCommand();
        }

        private DataTables()
        {
            loadProgram();
            loadStudent();
            loadCours();
            loadInscription();
            loadHistoStudent();
            loadHistoInscript();
            loadPreReq();
        }

        static private DataTables singleton = new DataTables();

        internal static SqlDataAdapter getAdapterProgram()
        {
            return singleton.adapterProgram;
        }

        internal static SqlDataAdapter getAdapterStudent()
        {
            return singleton.adapterStudent;
        }

        internal static SqlDataAdapter getAdapterCours()
        {
            return singleton.adapterCours;
        }

        internal static SqlDataAdapter getAdapterInscription()
        {
            return singleton.adapterInscription;
        }

        internal static SqlDataAdapter getAdapterHistoStudent()
        {
            return singleton.adapterHistoStudent;
        }

        internal static SqlDataAdapter getAdapterHistoInscript()
        {
            return singleton.adapterHistoInscript;
        }

        internal static SqlDataAdapter getAdapterPreReq()
        {
            return singleton.adapterPreReq;
        }

        internal static DataSet getDataSet()
        {
            return singleton.ds;
        }
    }
}
