using System.Xml;

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
            try
            {
                Subject result = new Subject();
                Utils.ReadXMLString(xml, reader =>
                 {
                     while (reader.Read())
                         if (reader.IsStartElement(nameof(Subject)))
                         {
                             result.SubjectName = reader.GetAttribute(nameof(SubjectName));
                             result.TeacherName = reader.GetAttribute(nameof(TeacherName));
                         }
                 });
                return result;
            }
            catch (XmlException)
            {
                return null;
            }
        }

        public override string ToString()
        {
            return Utils.WriteXMLString((ref XmlWriter writer) =>
            {
                writer.WriteStartElement(nameof(Subject));
                writer.WriteAttributeString(nameof(SubjectName), SubjectName);
                writer.WriteAttributeString(nameof(TeacherName), TeacherName);
                writer.WriteEndElement();
            });
        }
    }
}
