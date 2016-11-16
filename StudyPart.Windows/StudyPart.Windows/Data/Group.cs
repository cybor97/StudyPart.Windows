using System.Xml;

namespace StudyPart.Server.Data
{
    public class Group
    {
        public long ID { get; set; }
        public long DepartmentID { get; set; }
        public long SpecialtyID { get; set; }

        public static bool operator ==(Group v1, Group v2)
        {
            return v1.Equals(v2);
        }

        public static bool operator !=(Group v1, Group v2)
        {
            return !(v1 == v2);
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            Group group = (Group)obj;
            return DepartmentID == group.DepartmentID && SpecialtyID == group.SpecialtyID;
        }

        //IDK what to do with it. It's not recommended to use dinamic properties here... So let it be default.
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public static Group Parse(string xml)
        {
            try
            {
                Group result = new Group();
                Utils.ReadXMLString(xml, reader =>
                 {
                     while (reader.Read())
                         if (reader.IsStartElement(nameof(Group)))
                         {
                             result.ID = long.Parse(reader.GetAttribute(nameof(ID)));
                             result.DepartmentID = long.Parse(reader.GetAttribute(nameof(DepartmentID)));
                             result.SpecialtyID = long.Parse(reader.GetAttribute(nameof(SpecialtyID)));
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
                writer.WriteStartElement(nameof(Group));
                writer.WriteAttributeString(nameof(ID), ID.ToString());
                writer.WriteAttributeString(nameof(DepartmentID), DepartmentID.ToString());
                writer.WriteAttributeString(nameof(SpecialtyID), SpecialtyID.ToString());
                writer.WriteEndElement();
            });
        }
    }
}