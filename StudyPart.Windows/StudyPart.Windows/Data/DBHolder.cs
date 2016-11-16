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

        public static void Init(bool nativeAutoIncrement=false)
        {
            bool DBExists = File.Exists(DBFileName);
            if (!DBExists)
            {
                if (!Directory.Exists(DataDirectory))
                    Directory.CreateDirectory(DataDirectory);

                SQLiteConnection.CreateFile(DBFileName);
            }
            Connection = new SQLiteConnection(string.Format("Data Source={0};Version=3;", DBFileName));
            Connection.Open();
            if (!DBExists)
            {
                var command = new SQLiteCommand(Connection);
                command.Execute(string.Format("CREATE TABLE {0}({1} INTEGER PRIMARY KEY AUTOINCREMENT, " +
                                                                "{2} INTEGER, " +
                                                                "{3} INTEGER);",
                                    GROUPS,
                                    nameof(Group.ID), nameof(Group.DepartmentID), nameof(Group.SpecialtyID)));
            }
        }

        static void Execute(this SQLiteCommand command, string text)
        {
            command.CommandText = text;
            command.ExecuteNonQuery();
        }
    }
}