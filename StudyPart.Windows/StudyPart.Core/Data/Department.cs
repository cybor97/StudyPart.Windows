using System.Xml;
using StudyPart.Core;

namespace StudyPart.Core.Data
{
    public class Department
    {
        public long ID { get; set; }
        public string DepartmentName { get; set; }

        public static Department Parse(string xml)
        {
            try
            {
                var result = new Department();
                Utils.ReadXMLString(xml, reader =>
                {
                    while (!reader.EOF)
                        if (reader.IsStartElement(nameof(Department)))
                        {
                            result.ID = long.Parse(reader.GetAttribute(nameof(ID)));
                            result.DepartmentName = reader.GetAttribute(nameof(DepartmentName));
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
                writer.WriteStartElement(nameof(Department));
                writer.WriteAttributeString(nameof(ID), ID.ToString());
                writer.WriteAttributeString(nameof(DepartmentName), DepartmentName);
                writer.WriteEndElement();
            });
        }
    }
}
