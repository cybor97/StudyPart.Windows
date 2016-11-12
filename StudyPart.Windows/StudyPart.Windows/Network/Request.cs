using System.IO;
using System.Text;
using System.Xml;

namespace StudyPart.Windows.Network
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
                using (var reader = XmlReader.Create(new MemoryStream(Encoding.UTF8.GetBytes(xml))))
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
            }
            catch (XmlException)
            {
                //Console.WriteLine(e);
            }
            return result;
        }
    }
}
