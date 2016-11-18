using System.Xml;
using static StudyPart.Core.Data.DBHolder;
namespace StudyPart.Core.Data
{
    public class Student
    {
        public long ID { get; set; }

        public long GroupID { get; set; }

        public string FullName { get; set; }

        public string GroupOut
        {
            get
            {
                return Tables[GROUPS].Rows.Find(GroupID)?[GROUP_NAME]?.ToString();
            }
        }

        public static Student Parse(string xml)
        {
            try
            {
                var result = new Student();
                Utils.ReadXMLString(xml, reader =>
                {
                    while (!reader.EOF)
                        if (reader.IsStartElement(nameof(Student)))
                        {
                            result.ID = long.Parse(reader.GetAttribute(nameof(ID)));
                            result.GroupID = long.Parse(reader.GetAttribute(nameof(GroupID)));
                            result.FullName = reader.GetAttribute(nameof(FullName));
                            reader.Read();
                        }
                        else reader.Read();
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
                writer.WriteStartElement(nameof(Student));
                writer.WriteAttributeString(nameof(ID), ID.ToString());
                writer.WriteAttributeString(nameof(FullName), FullName);
                writer.WriteAttributeString(nameof(GroupID), GroupID.ToString());
                writer.WriteEndElement();
            });
        }
    }
}
