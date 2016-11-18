using System;
using System.IO;

namespace StudyPart.Core
{
    public class CommonVariables
    {
        public const int ServerPortNumber = 12006;

        public static string DataDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "StudyPart");
        public static string DBFileName = Path.Combine(DataDirectory, "Data.db");
        public static string UniversalDateTimeFormat = "dd.MM.yyyy";
    }
}
