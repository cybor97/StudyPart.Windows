using System;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using static StudyPart.Server.CommonVariables;

namespace StudyPart.Server.Data
{
    public static class DBHolder
    {
        public const string
            GROUPS = "Groups",
            MARKS = "Marks",
            STUDENTS = "Students",
            SUBJECTS = "Subjects",
            ACCOUNTS = "Accounts",
            DEPARTMENTS = "Departments",//TODO:Implement!
            SPECIALTIES = "Specialties",//TODO:Implement!
            TEACHERS = "Teachers";//TODO:Implement!

        public const string
            ID = "ID",

            DEPARTMENT_ID = "DepartmentID",
            SPECIALTY_ID = "SpecialtyID",

            STUDENT_ID = "StudentID",
            SUBJECT_ID = "SubjectID",
            TEST_DATE = "TestDate",
            MARK_VALUE = "MarkValue",
            YEAR_HALF = "YearHalf",

            GROUP_ID = "GroupID",
            FULL_NAME = "FullName",

            TEACHER_ID = "TeacherID",
            SUBJECT_NAME = "SubjectName",

            DEPARTMENT_NAME = "DepartmentName",

            SPECIALTY_NAME = "";

        static SQLiteConnection Connection;
        static SQLiteDataAdapter Adapter;
        static DataTable Table;

        public static void Init(bool nativeAutoIncrement = false)
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
                command.Execute(string.Format("CREATE TABLE {0}({1} INTEGER PRIMARY KEY {2}, " +
                                                                "{3} INTEGER, " +
                                                                "{4} INTEGER);",
                                    GROUPS,
                                    ID,
                                    nativeAutoIncrement ? "AUTOINCREMENT" : "",
                                    DEPARTMENT_ID, SPECIALTY_ID));

                command.Execute(string.Format("CREATE TABLE {0}({1} INTEGER PRIMARY KEY {2}, " +
                                                                "{3} INTEGER, " +
                                                                "{4} INTEGER," +
                                                                "{5} INTEGER," +
                                                                "{6} INTEGER," +
                                                                "{7} INTEGER);",
                                    MARKS,
                                    ID,
                                    nativeAutoIncrement ? "AUTOINCREMENT" : "",
                                    STUDENT_ID, SUBJECT_ID, TEST_DATE, MARK_VALUE, YEAR_HALF));

                command.Execute(string.Format("CREATE TABLE {0}({1} INTEGER PRIMARY KEY {2}, " +
                                                                "{3} INTEGER, " +
                                                                "{4} TEXT);",
                                    STUDENTS,
                                    ID,
                                    nativeAutoIncrement ? "AUTOINCREMENT" : "",
                                    GROUP_ID, FULL_NAME));

                command.Execute(string.Format("CREATE TABLE {0}({1} INTEGER PRIMARY KEY {2}, " +
                                                                "{3} INTEGER, " +
                                                                "{4} TEXT);",
                                    SUBJECTS,
                                    ID,
                                    nativeAutoIncrement ? "AUTOINCREMENT" : "",
                                    TEACHER_ID, SUBJECT_NAME));

                //command.Execute(string.Format("CREATE TABLE {0}({1} INTEGER PRIMARY KEY {2}, " +
                //                                                "{3} INTEGER, " +
                //                                                "{4} TEXT);",
                //                    ACCOUNTS,
                //                    ID,
                //                    nativeAutoIncrement ? "AUTOINCREMENT" : "",
                //                    /* ? */)); TODO: Implement!

                command.Execute(string.Format("CREATE TABLE {0}({1} INTEGER PRIMARY KEY {2}, " +
                                                    "{3} INTEGER, " +
                                                    "{4} TEXT);",
                                    DEPARTMENTS,
                                    ID,
                                    nativeAutoIncrement ? "AUTOINCREMENT" : "",
                                    DEPARTMENT_NAME));

                command.Execute(string.Format("CREATE TABLE {0}({1} INTEGER PRIMARY KEY {2}, " +
                                                                "{3} INTEGER, " +
                                                                "{4} TEXT);",
                                    SPECIALTIES,
                                    ID,
                                    nativeAutoIncrement ? "AUTOINCREMENT" : "",
                                    SPECIALTY_NAME));

                command.Execute(string.Format("CREATE TABLE {0}({1} INTEGER PRIMARY KEY {2}, " +
                                                                "{3} INTEGER, " +
                                                                "{4} TEXT);",
                                    TEACHERS,
                                    ID,
                                    nativeAutoIncrement ? "AUTOINCREMENT" : "",
                                    FULL_NAME));
            }
        }

        static void Execute(this SQLiteCommand command, string text)
        {
            command.CommandText = text;
            command.ExecuteNonQuery();
        }

        public static int GetFreeID(this DataTable table)
        {
            int i = 0;
            while (table.Select().Any(c => int.Parse(c[ID].ToString()) == i))
                i++;
            return i;
        }
    }
}