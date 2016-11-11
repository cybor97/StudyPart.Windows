using System.Collections.Generic;

namespace StudyPart.Windows.Data
{
    class Department
    {
        public long ID { get; set; }

        public string DepartmentName { get; set; }

        public List<Subject> Subjects { get; set; }
    }
}
