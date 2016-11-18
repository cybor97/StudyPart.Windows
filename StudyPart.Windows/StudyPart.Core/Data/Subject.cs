using System.Xml;
using static StudyPart.Core.Data.DBHolder;
namespace StudyPart.Core.Data
{
    public class Subject
    {
        public long ID { get; set; }

        public long TeacherID { get; set; }

        public string SubjectName { get; set; }

        public string TeacherOut
        {
            get
            {
                return Tables[TEACHERS].Rows.Find(TeacherID)?[FULL_NAME]?.ToString();
            }
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
                             result.TeacherID = long.Parse(reader.GetAttribute(nameof(TeacherID)));
                             result.SubjectName = reader.GetAttribute(nameof(SubjectName));
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
                writer.WriteAttributeString(nameof(TeacherID), TeacherID.ToString());
                writer.WriteAttributeString(nameof(SubjectName), SubjectName);
                writer.WriteEndElement();
            });
        }
    }
}
