using System.Collections.Generic;

namespace StudyPart.Windows.Data
{
    public class Department
    {
        public long ID { get; set; }

        public string DepartmentName { get; set; }

        public List<Subject> Subjects { get; set; }



        public static Subject Parse(string xml)
        {
            return null;//TODO:Implement
        }

        public override string ToString()
        {
            return null;//TODO:Implement
        }
    }
}
