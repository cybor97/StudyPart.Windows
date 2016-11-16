using System.Data;
using System.Data.SQLite;
using System.IO;
using static StudyPart.Server.CommonVariables;

namespace StudyPart.Server.Data
{
    public static class DBHolder
    {
        public const string GROUPS = "Groups",
            MARKS = "Marks",
            STUDENTS = "Students",
            SUBJECTS = "Subjects",
            ACCOUNTS = "Accounts",
            DEPARTMENTS = "Departments",//TODO:Implement!
            SPECIALTIES = "Specialties",//TODO:Implement!
            TEACHERS = "Teachers";//TODO:Implement!

        static SQLiteConnection Connection;
        static SQLiteDataAdapter Adapter;
        static DataTable Table;

        public static void Init(bool nativeAutoIncrement)
        {
            if (!Directory.Exists(DataDirectory))
            {
                Directory.CreateDirectory(DataDirectory);
                SQLiteConnection.CreateFile(DBFileName);
                Connection = new SQLiteConnection(string.Format("Data Source={0};Version=3;", DBFileName));
                Connection.Open();
                var command = new SQLiteCommand("CREATE TABLE Test(ID INTEGER PRIMARY KEY AUTOINCREMENT, TestName TEXT);", Connection);
                command.ExecuteNonQuery();
                command.CommandText = "INSERT INTO Test VALUES(-1, 'Test completed!!!')";
                command.ExecuteNonQuery();
            }
        }
    }
}
