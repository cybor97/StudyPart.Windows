using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudyPart.Windows.Data
{
    public class Group
    {
        public string DepartmentName { get; set; }
        public string SpecialtyName { get; set; }

        public static Group Parse()
        {

        }

        public override string ToString()
        {
            return base.ToString();
        }
    }
}
