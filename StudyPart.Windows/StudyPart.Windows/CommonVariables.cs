using System;
using System.IO;

namespace StudyPart.Server
{
    class CommonVariables
    {
        public const int ServerPortNumber = 12006;

        public static string DataDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "StudyPart");
        public static string DBFileName = Path.Combine(DataDirectory, "Data.db");
    }
}
