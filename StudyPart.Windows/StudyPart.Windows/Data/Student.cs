using System.Collections.Generic;

namespace StudyPart.Windows.Data
{
    public class Student
    {
        public long ID { get; set; }

        public string FullName { get; set; }

        public Group Group { get; set; }

        public List<Mark> Marks { get; set; }

        public static Subject Parse(string xml)
        {
            return null;//TODO:Implement
        }

        public string ToXML()
        {
            return null;
        }
        public override string ToString()
        {
            return null;//TODO:Implement
        }
    }
}
