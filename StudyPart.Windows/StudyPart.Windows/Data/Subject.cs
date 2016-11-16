using System.Xml;

namespace StudyPart.Server.Data
{
    public class Subject
    {
        public long ID { get; set; }

        public string SubjectID { get; set; }

        public string TeacherID { get; set; }


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
            return SubjectID == subject.SubjectID && TeacherID == subject.TeacherID;
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
                             result.ID = long.Parse(reader.GetAttribute(nameof(ID)));
                             result.SubjectID = reader.GetAttribute(nameof(SubjectID));
                             result.TeacherID = reader.GetAttribute(nameof(TeacherID));
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
                writer.WriteAttributeString(nameof(ID), ID.ToString());
                writer.WriteAttributeString(nameof(SubjectID), SubjectID);
                writer.WriteAttributeString(nameof(TeacherID), TeacherID);
                writer.WriteEndElement();
            });
        }
    }
}
