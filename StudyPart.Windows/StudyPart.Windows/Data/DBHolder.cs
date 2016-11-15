using System.Data;
using System.Data.SQLite;
using System.IO;
using static StudyPart.Windows.CommonVariables;

namespace StudyPart.Windows.Data
{
    public static class DBHolder
    {
        static SQLiteConnection Connection;
        static SQLiteDataAdapter Adapter;
        static DataTable Table;

        public static void Init()
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
