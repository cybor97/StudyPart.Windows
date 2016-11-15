using System.Xml;

namespace StudyPart.Windows.Data
{
    public class UserAccount
    {
        public string FullName { get; set; }

        public string UserName { get; set; }

        public string Key { get; set; }

        public static bool operator ==(UserAccount v1, UserAccount v2)
        {
            return v1.Equals(v2);
        }

        public static bool operator !=(UserAccount v1, UserAccount v2)
        {
            return !(v1 == v2);
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            return UserName == ((UserAccount)obj).UserName;
        }

        //IDK what to do with it. It's not recommended to use dinamic properties here... So let it be default.
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public static UserAccount Parse(string xml)
        {
            try
            {
                UserAccount result = new UserAccount();
                Utils.ReadXMLString(xml, reader =>
                {
                    while (reader.Read())
                        if (reader.IsStartElement(nameof(UserAccount)))
                        {
                            result.FullName = reader.GetAttribute(nameof(FullName));
                            result.UserName = reader.GetAttribute(nameof(UserName));
                            result.Key = reader.GetAttribute(nameof(Key));
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
                writer.WriteStartElement(nameof(UserAccount));
                writer.WriteAttributeString(nameof(FullName), FullName);
                writer.WriteAttributeString(nameof(UserName), UserName);
                writer.WriteAttributeString(nameof(Key), Key);
                writer.WriteEndElement();
            });
        }
    }
}
