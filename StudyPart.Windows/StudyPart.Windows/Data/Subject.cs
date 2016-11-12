namespace StudyPart.Windows.Data
{
    public class Subject
    {
        public long ID { get; set; }

        public string SubjectName { get; set; }

        public string TeacherName { get; set; }

        public static bool operator ==(Subject v1, Subject v2)
        {
            return v1.Equals(v2);
        }

        public static bool operator !=(Subject v1, Subject v2)
        {
            return !(v1 == v2);
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            Subject subject = (Subject)obj;
            return SubjectName == subject.SubjectName && TeacherName == subject.TeacherName;
        }

        //IDK what to do with it. It's not recommended to use dinamic properties here... So let it be default.
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }


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
