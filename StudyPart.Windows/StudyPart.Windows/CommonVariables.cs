using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudyPart.Windows
{
    class CommonVariables
    {
        public const int ServerPortNumber = 12006;

        public static string DataDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "StudyPart");
        public static string DBFileName = Path.Combine(DataDirectory, "Data.db");
    }
}
