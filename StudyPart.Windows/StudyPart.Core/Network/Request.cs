using System.Xml;

namespace StudyPart.Core.Network
{
    public class Request
    {
        public string Type { get; set; }

        public string In { get; set; }

        public string By { get; set; }

        public string Data { get; set; }

        public static Request Parse(string xml)
        {
            Request result = new Request();
            try
            {
                Utils.ReadXMLString(xml, reader =>
                 {
                     while (!reader.EOF)
                         if (reader.IsStartElement(nameof(Type)))
                             result.Type = reader.ReadElementContentAsString();
                         else if (reader.IsStartElement(nameof(In)))
                             result.In = reader.ReadElementContentAsString();
                         else if (reader.IsStartElement(nameof(By)))
                             result.By = reader.ReadElementContentAsString();
                         else if (reader.IsStartElement(nameof(Data)))
                             result.Data = reader.ReadInnerXml();
                         else
                             reader.Read();
                 });
            }
            catch (XmlException)
            {
                //Console.WriteLine(e);
            }
            return result;
        }
        public override string ToString()
        {
            return Utils.WriteXMLString((ref XmlWriter writer) =>
            {
                writer.WriteStartElement(nameof(Request));
                writer.WriteElementString(nameof(Type), Type);
                writer.WriteElementString(nameof(In), In);
                writer.WriteElementString(nameof(By), By);
                writer.WriteElementString(nameof(Data), Data);
                writer.WriteEndElement();
            });
        }
    }
}
