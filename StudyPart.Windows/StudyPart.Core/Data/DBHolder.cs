using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using static StudyPart.Core.CommonVariables;

namespace StudyPart.Core.Data
{
    public static class DBHolder
    {
        #region Table names
        public const string
            GROUPS = "Groups",
            MARKS = "Marks",
            STUDENTS = "Students",
            SUBJECTS = "Subjects",
            ACCOUNTS = "Accounts",
            DEPARTMENTS = "Departments",
            SPECIALTIES = "Specialties",
            TEACHERS = "Teachers";

        private static readonly string[] TableNames = { GROUPS, MARKS, STUDENTS, SUBJECTS, ACCOUNTS, DEPARTMENTS, SPECIALTIES, TEACHERS };
        #endregion
        #region Property names
        public const string
            ID = "ID",

            DEPARTMENT_ID = "DepartmentID",
            SPECIALTY_ID = "SpecialtyID",
            GROUP_NAME = "GroupName",

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

            SPECIALTY_NAME = "SpecialtyName",

            USER_NAME = "UserName",
            KEY = "Key",
            PERMISSIONS = "Permissions",
            PROFILE_IMAGE = "ProfileImage";
        #endregion
        #region Type names
        public const string
            INTEGER = "INTEGER",
            TEXT = "TEXT",
            BLOB = "BLOB";
        #endregion

        public const int NO_ID = -1, ALL = -2;
        static SQLiteConnection Connection;
        static Dictionary<string, SQLiteDataAdapter> Adapters;
        internal static Dictionary<string, DataTable> Tables;
        static bool NativeAutoIncrement;

        public static void Init(bool nativeAutoIncrement)
        {
            NativeAutoIncrement = nativeAutoIncrement;
            #region Create file if not DB exists
            bool DBExists = File.Exists(DBFileName);
            if (!DBExists)
            {
                if (!Directory.Exists(DataDirectory))
                    Directory.CreateDirectory(DataDirectory);

                SQLiteConnection.CreateFile(DBFileName);
            }
            #endregion
            #region Init connection
            Connection = new SQLiteConnection(string.Format("Data Source={0};Version=3;", DBFileName));
            Connection.Open();
            #endregion
            #region Create tables if DB has just created
            if (!DBExists)
            {
                var command = new SQLiteCommand(Connection);

                command.CreateTable(GROUPS,
                    DEPARTMENT_ID, INTEGER,
                    SPECIALTY_ID, INTEGER,
                    GROUP_NAME, TEXT);

                command.CreateTable(MARKS,
                    STUDENT_ID, INTEGER,
                    SUBJECT_ID, INTEGER,
                    TEST_DATE, TEXT,
                    YEAR_HALF, INTEGER,
                    MARK_VALUE, INTEGER);

                command.CreateTable(STUDENTS,
                    GROUP_ID, INTEGER,
                    FULL_NAME, TEXT);

                command.CreateTable(SUBJECTS,
                    TEACHER_ID, INTEGER,
                    SUBJECT_NAME, TEXT);

                command.CreateTable(ACCOUNTS,
                    STUDENT_ID, INTEGER,
                    FULL_NAME, TEXT,
                    PROFILE_IMAGE, BLOB,
                    USER_NAME, TEXT,
                    KEY, TEXT,
                    PERMISSIONS, INTEGER);

                command.CreateTable(DEPARTMENTS,
                    DEPARTMENT_NAME, TEXT);

                command.CreateTable(SPECIALTIES,
                    SPECIALTY_NAME, TEXT);

                command.CreateTable(TEACHERS,
                    FULL_NAME, TEXT);
            }
            #endregion
            #region Init adapters and tables
            Adapters = new Dictionary<string, SQLiteDataAdapter>();
            Tables = new Dictionary<string, DataTable>();
            foreach (var current in TableNames)
            {
                var adapter = CreateAdapter(current);
                Adapters.Add(current, adapter);
                Tables.Add(current, adapter.CreateTable());
            }
            #endregion
        }

        #region GET

        public static List<Student> GetStudents(int groupId = ALL)
        {
            List<Student> result = new List<Student>();
            IEnumerable<DataRow> selection = groupId == ALL ?
                Tables[STUDENTS].Select() :
                Tables[STUDENTS].Select().Where(c => (long)c[GROUP_ID] == groupId);

            foreach (var current in selection)
                result.Add(new Student
                {
                    ID = (long)current[ID],
                    GroupID = groupId,
                    FullName = (string)current[FULL_NAME]
                });
            return result;
        }

        public static List<Mark> GetMarks(long studentId = ALL)
        {
            List<Mark> result = new List<Mark>();
            IEnumerable<DataRow> selection = studentId == ALL ?
                Tables[MARKS].Select() :
                Tables[MARKS].Select().Where(c => (long)c[STUDENT_ID] == studentId);

            foreach (var current in selection)
                result.Add(new Mark
                {
                    ID = (long)current[ID],
                    StudentID = studentId,
                    SubjectID = (long)current[SUBJECT_ID],
                    MarkValue = (int)current[MARK_VALUE],
                    TestDate = Utils.ConvertFromUnixTimestamp((double)current[TEST_DATE]),
                    YearHalf = (int)current[YEAR_HALF]
                });
            return result;
        }

        public static List<Account> GetAccounts()
        {
            List<Account> result = new List<Account>();
            foreach (var current in Tables[ACCOUNTS].Select())
                result.Add(new Account
                {
                    ID = (long)current[ID],
                    StudentID = (long)current[STUDENT_ID],
                    FullName = (string)current[FULL_NAME],
                    ProfileImage = (byte[])current[PROFILE_IMAGE],
                    UserName = (string)current[USER_NAME],
                    Key = (string)current[KEY],
                    Permissions = (int)current[PERMISSIONS]
                });
            return result;
        }

        public static List<Group> GetGroups()
        {
            List<Group> result = new List<Group>();
            foreach (var current in Tables[GROUPS].Select())
                result.Add(new Group
                {
                    ID = (long)current[ID],
                    DepartmentID = (long)current[DEPARTMENT_ID],
                    SpecialtyID = (long)current[SPECIALTY_ID],
                    GroupName = (string)current[GROUP_NAME]
                });
            return result;
        }

        public static List<Subject> GetSubjects()
        {
            List<Subject> result = new List<Subject>();
            foreach (var current in Tables[SUBJECTS].Select())
                result.Add(new Subject
                {
                    ID = (long)current[ID],
                    TeacherID = (long)current[TEACHER_ID],
                    SubjectName = (string)current[SUBJECT_NAME]
                });
            return result;
        }

        public static List<Department> GetDepartments()
        {
            List<Department> result = new List<Department>();
            foreach (var current in Tables[DEPARTMENTS].Select())
                result.Add(new Department
                {
                    ID = (long)current[ID],
                    DepartmentName = (string)current[DEPARTMENT_NAME]
                });
            return result;
        }

        public static List<Specialty> GetSpecialties()
        {
            List<Specialty> result = new List<Specialty>();
            foreach (var current in Tables[SPECIALTIES].Select())
                result.Add(new Specialty
                {
                    ID = (long)current[ID],
                    SpecialtyName = (string)current[SPECIALTY_NAME]
                });
            return result;
        }

        public static List<Teacher> GetTeachers()
        {
            List<Teacher> result = new List<Teacher>();
            foreach (var current in Tables[TEACHERS].Select())
                result.Add(new Teacher
                {
                    ID = (long)current[ID],
                    FullName = (string)current[FULL_NAME]
                });
            return result;
        }

        #endregion

        #region ADD

        public static void Add(Student student)
        {
            Tables[STUDENTS].Add(student.GroupID, student.FullName);
        }

        public static void Add(Mark mark)
        {
            Tables[MARKS].Add(mark.StudentID, mark.SubjectID, mark.TestDate, mark.YearHalf, mark.MarkValue);
        }

        public static void Add(Account account)
        {
            Tables[ACCOUNTS].Add(account.StudentID, account.FullName, account.ProfileImage, account.UserName, account.Key, account.Permissions);
        }

        public static void Add(Group group)
        {
            Tables[GROUPS].Add(group.DepartmentID, group.SpecialtyID, group.GroupName);
        }

        public static void Add(Subject subject)
        {
            Tables[SUBJECTS].Add(subject.TeacherID, subject.SubjectName);
        }

        public static void Add(Department department)
        {
            Tables[DEPARTMENTS].Add(department.DepartmentName);
        }

        public static void Add(Specialty specialty)
        {
            Tables[SPECIALTIES].Add(specialty.SpecialtyName);
        }

        public static void Add(Teacher teacher)
        {
            Tables[TEACHERS].Add(teacher.FullName);
        }

        static void Add(this DataTable table, params object[] valuesExceptID)
        {
            List<object> data = new List<object>();
            data.Add(NativeAutoIncrement ? NO_ID : table.GetFreeID());//FIXME:Potential bug/shortcoming with linkage on sync!
            data.AddRange(valuesExceptID);
            table.Rows.Add(data.ToArray());
            Update();
        }


        #endregion

        #region SET

        public static void Set(Student student)
        {
            var studentRow = Tables[STUDENTS].Rows.Find(student.ID);
            studentRow[GROUP_ID] = student.GroupID;
            studentRow[FULL_NAME] = student.FullName;
            Update();
        }

        public static void Set(Mark mark)
        {
            var markRow = Tables[MARKS].Rows.Find(mark.ID);
            markRow[STUDENT_ID] = mark.StudentID;
            markRow[SUBJECT_ID] = mark.SubjectID;
            markRow[TEST_DATE] = mark.TestDate;
            markRow[YEAR_HALF] = mark.YearHalf;
            markRow[MARK_VALUE] = mark.MarkValue;
            Update();
        }

        public static void Set(Account account)
        {
            var accountRow = Tables[ACCOUNTS].Rows.Find(account.ID);
            accountRow[STUDENT_ID] = account.StudentID;
            accountRow[FULL_NAME] = account.FullName;
            accountRow[PROFILE_IMAGE] = account.ProfileImage;
            accountRow[USER_NAME] = account.UserName;
            accountRow[KEY] = account.Key;
            accountRow[PERMISSIONS] = account.Permissions;
            Update();
        }

        public static void Set(Group group)
        {
            var groupRow = Tables[GROUPS].Rows.Find(group.ID);
            groupRow[DEPARTMENT_ID] = group.DepartmentID;
            groupRow[SPECIALTY_ID] = group.SpecialtyID;
            groupRow[GROUP_NAME] = group.GroupName;
            Update();
        }

        public static void Set(Subject subject)
        {
            var subjectRow = Tables[SUBJECTS].Rows.Find(subject.ID);
            subjectRow[SUBJECT_NAME] = subject.SubjectName;
            subjectRow[TEACHER_ID] = subject.TeacherID;
            Update();
        }

        public static void Set(Department department)
        {
            Tables[DEPARTMENTS].Rows.Find(department.ID)[DEPARTMENT_NAME] = department.DepartmentName;
            Update();
        }

        public static void Set(Specialty specialty)
        {
            Tables[SPECIALTIES].Rows.Find(specialty.ID)[SPECIALTY_NAME] = specialty.SpecialtyName;
            Update();
        }

        public static void Set(Teacher teacher)
        {
            Tables[TEACHERS].Rows.Find(teacher.ID)[FULL_NAME] = teacher.FullName;
            Update();
        }

        #endregion

        #region REMOVE

        public static void Remove(Student student)
        {
            Tables[STUDENTS].Remove(student.ID);
        }

        public static void Remove(Mark mark)
        {
            Tables[MARKS].Remove(mark.ID);
        }

        public static void Remove(Account account)
        {
            Tables[ACCOUNTS].Remove(account.ID);
        }

        public static void Remove(Group group)
        {
            Tables[GROUPS].Remove(group.ID);
        }

        public static void Remove(Subject subject)
        {
            Tables[SUBJECTS].Remove(subject.ID);
        }

        public static void Remove(Department department)
        {
            Tables[DEPARTMENTS].Remove(department.ID);
        }

        public static void Remove(Specialty specialty)
        {
            Tables[SPECIALTIES].Remove(specialty.ID);
        }

        public static void Remove(Teacher teacher)
        {
            Tables[TEACHERS].Remove(teacher.ID);
        }

        static void Remove(this DataTable table, long id)
        {
            table.Rows.Find(id).Delete();
            Update();
        }

        #endregion

        #region Internal service staff

        static void Update()
        {
            foreach (var current in TableNames)
                Update(current);
            Connection.Close();
            Init(NativeAutoIncrement);
        }

        static void Update(string tableName)
        {
            var adapter = Adapters[tableName];
            var commandBuilder = new SQLiteCommandBuilder(adapter);
            adapter.DeleteCommand = commandBuilder.GetDeleteCommand(true);
            adapter.InsertCommand = commandBuilder.GetInsertCommand(true);
            adapter.UpdateCommand = commandBuilder.GetUpdateCommand(true);
            adapter.Update(Tables[tableName]);
        }

        static DataTable CreateTable(this SQLiteDataAdapter adapter)
        {
            var table = new DataTable();
            adapter.Fill(table);
            table.PrimaryKey = new[] { table.Columns[ID] };
            return table;
        }

        static SQLiteDataAdapter CreateAdapter(string tableName)
        {
            return new SQLiteDataAdapter(string.Format("SELECT * FROM {0}", tableName), Connection)
            {
                AcceptChangesDuringFill = true,
                AcceptChangesDuringUpdate = true
            };
        }

        static void CreateTable(this SQLiteCommand command, string name, params string[] columns)
        {
            if (columns.Length % 2 != 0) throw new ArgumentException("Columns count must be dividable by zero!");
            string columnsPart = string.Format("{0} INTEGER PRIMARY KEY{1},", ID, NativeAutoIncrement ? " AUTOINCREMENT" : "");
            for (int i = 0; i < columns.Length; i += 2)
                columnsPart += string.Format(" {0} {1},", columns[i], columns[i + 1]);

            command.Execute(string.Format("CREATE TABLE {0}({1});", name, columnsPart.TrimEnd(',')));
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
        #endregion
    }
}