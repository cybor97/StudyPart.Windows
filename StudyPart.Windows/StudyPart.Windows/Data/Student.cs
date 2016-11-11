using System.Collections.Generic;

namespace StudyPart.Windows.Data
{
    class Student
    {
        public long ID { get; set; }

        public string FullName { get; set; }

        public Department Department { get; set; }

        public string GroupName { get; set; }

        public string Speciality { get; set; }

        public List<Mark> Marks { get; set; }

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
