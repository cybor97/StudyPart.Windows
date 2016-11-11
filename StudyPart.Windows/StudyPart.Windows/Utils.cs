using System.IO;
using System.Text;
using System.Xml;

namespace StudyPart.Windows
{
    public static partial class Utils
    {
        #region XML
        public delegate void XmlWriterCreatedDelegate(ref XmlWriter writer);
        /// <summary>
        /// XML writing simplifier
        /// </summary>
        /// <param name="onWriterCreated">Parameter should be a function with parameter: (ref XmlWriter writer)</param>
        /// <returns>XML string</returns>
        public static string WriteXMLString(XmlWriterCreatedDelegate onWriterCreated)
        {
            var stream = new MemoryStream();
            var writer = XmlWriter.Create(stream, new XmlWriterSettings { Indent = true, OmitXmlDeclaration = true });
            onWriterCreated?.Invoke(ref writer);
            writer.Flush();
            return Encoding.UTF8.GetString(stream.GetBuffer()).Replace('\0', ' ');
        }
        public delegate void XmlReaderCreatedDelegate(XmlReader reader);
        /// <summary>
        /// XML reading simplifier
        /// </summary>
        /// <param name="xml">XML string</param>
        /// <param name="onReaderCreated">Parameter should be a function with parameter: (XmlReader reader)</param>
        public static void ReadXMLString(string xml, XmlReaderCreatedDelegate onReaderCreated)
        {
            onReaderCreated?.Invoke(XmlReader.Create(new MemoryStream(Encoding.UTF8.GetBytes(xml.Replace('\0', ' ')))));
        }
        #endregion
    }
}
