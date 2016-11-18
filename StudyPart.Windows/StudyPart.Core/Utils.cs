using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Xml;

namespace StudyPart.Core
{
    public static partial class Utils
    {
        #region XML
        public delegate void XmlWriterCreatedDelegate(ref XmlWriter writer);

        public static string WriteXMLString(XmlWriterCreatedDelegate onWriterCreated)
        {
            var stream = new MemoryStream();
            var writer = XmlWriter.Create(stream, new XmlWriterSettings { Indent = true, OmitXmlDeclaration = true });
            onWriterCreated?.Invoke(ref writer);
            writer.Flush();
            return Encoding.UTF8.GetString(stream.GetBuffer()).Replace('\0', ' ');
        }
        public delegate void XmlReaderCreatedDelegate(XmlReader reader);

        public static void ReadXMLString(string xml, XmlReaderCreatedDelegate onReaderCreated)
        {
            onReaderCreated?.Invoke(XmlReader.Create(new MemoryStream(Encoding.UTF8.GetBytes(xml.Replace('\0', ' ')))));
        }
        #endregion
        #region Encryption

        public static string GetHash(string source, string key)
        {
            SHA1 sha1 = SHA1.Create();
            sha1.Initialize();
            byte[] bytes = sha1.ComputeHash(Encoding.UTF8.GetBytes(source));
            byte[] keyBytes = Encoding.UTF8.GetBytes(key);
            for (int i = 0; i < bytes.Length; i++)
                foreach (byte currentKeyBlock in keyBytes)
                    bytes[i] ^= currentKeyBlock;
            return Encoding.UTF8.GetString(bytes);
        }

        #endregion

        public static DateTime ConvertFromUnixTimestamp(double timestamp)
        {
            return new DateTime(1970, 1, 1, 0, 0, 0, 0).AddSeconds(timestamp);
        }

        public static double ConvertToUnixTimestamp(this DateTime date)
        {
            return Math.Floor((date - new DateTime(1970, 1, 1, 0, 0, 0, 0)).TotalSeconds);
        }
    }
}
