using System.Xml;

namespace StudyPart.Core.Data
{
    public class Specialty
    {
        public long ID { get; set; }
        public string SpecialtyName { get; set; }

        public static Specialty Parse(string xml)
        {
            try
            {
                var result = new Specialty();
                Utils.ReadXMLString(xml, reader =>
                {
                    while (!reader.EOF)
                        if (reader.IsStartElement(nameof(Specialty)))
                        {
                            result.ID = long.Parse(reader.GetAttribute(nameof(ID)));
                            result.SpecialtyName = reader.GetAttribute(nameof(SpecialtyName));
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
                writer.WriteStartElement(nameof(Specialty));
                writer.WriteAttributeString(nameof(ID), ID.ToString());
                writer.WriteAttributeString(nameof(SpecialtyName), SpecialtyName);
                writer.WriteEndElement();
            });
        }

    }
}
