using System;
using System.Xml;
using static StudyPart.Core.Data.DBHolder;

namespace StudyPart.Core.Data
{
    public class Account
    {
        public const int PERMISSION_READ = 1,//001
            PERMISSION_WRITE = 2,//010
            PERMISSION_MANAGE = 4;//100

        public long ID { get; set; }

        public long StudentID { get; set; }

        public string FullName { get; set; }

        public byte[] ProfileImage { get; set; }

        public string UserName { get; set; }

        public string Key { get; set; }

        public int Permissions { get; set; }

        public string StudentOut
        {
            get
            {
                return Tables[STUDENTS].Rows.Find(StudentID)?[FULL_NAME]?.ToString();
            }
        }

        public string PermissionsOut
        {
            get
            {
                return string.Format("{0}{1}{2}",
                    Read ? "Чтение" : "",
                    Write ? "Редактирование" : "",
                    Manage ? "Управление" : "");
            }
        }

        bool Read { get { return (Permissions & PERMISSION_READ) == PERMISSION_READ; } }
        bool Write { get { return (Permissions & PERMISSION_WRITE) == PERMISSION_WRITE; } }
        bool Manage { get { return (Permissions & PERMISSION_MANAGE) == PERMISSION_MANAGE; } }

        public static Account Parse(string xml)
        {
            try
            {
                Account result = new Account();
                Utils.ReadXMLString(xml, reader =>
                {
                    while (reader.Read())
                        if (reader.IsStartElement(nameof(Account)))
                        {
                            result.ID = long.Parse(reader.GetAttribute(nameof(ID)));
                            result.FullName = reader.GetAttribute(nameof(FullName));
                            result.UserName = reader.GetAttribute(nameof(UserName));
                            result.Key = reader.GetAttribute(nameof(Key));
                            reader.Read();
                        }
                        else if (reader.IsStartElement(nameof(ProfileImage)))
                            result.ProfileImage = Convert.FromBase64String(reader.ReadOuterXml());
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
                writer.WriteStartElement(nameof(Account));
                writer.WriteAttributeString(nameof(ID), ID.ToString());
                writer.WriteAttributeString(nameof(FullName), FullName);
                writer.WriteAttributeString(nameof(UserName), UserName);
                writer.WriteAttributeString(nameof(Key), Key);

                writer.WriteElementString(nameof(ProfileImage), Convert.ToBase64String(ProfileImage));

                writer.WriteEndElement();
            });
        }
    }
}
