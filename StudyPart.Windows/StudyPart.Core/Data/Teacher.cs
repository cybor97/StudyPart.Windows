using System.Xml;

namespace StudyPart.Core.Data
{
    public class Teacher
    {
        public long ID { get; set; }
        public string FullName { get; set; }

        public static Teacher Parse(string xml)
        {
            try
            {
                var result = new Teacher();
                Utils.ReadXMLString(xml, reader =>
                {
                    while (!reader.EOF)
                        if (reader.IsStartElement(nameof(Teacher)))
                        {
                            result.ID = long.Parse(reader.GetAttribute(nameof(ID)));
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
                writer.WriteStartElement(nameof(Teacher));
                writer.WriteAttributeString(nameof(ID), ID.ToString());
                writer.WriteAttributeString(nameof(FullName), FullName);
                writer.WriteEndElement();
            });
        }
    }
}
