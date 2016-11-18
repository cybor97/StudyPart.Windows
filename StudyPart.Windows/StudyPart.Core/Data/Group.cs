using System.Xml;
using static StudyPart.Core.Data.DBHolder;
namespace StudyPart.Core.Data
{
    public class Group
    {
        public long ID { get; set; }
        public long DepartmentID { get; set; }
        public long SpecialtyID { get; set; }
        public string GroupName { get; set; }

        public string DepartmentOut
        {
            get
            {
                return Tables[DEPARTMENTS].Rows.Find(DepartmentID)?[DEPARTMENT_NAME]?.ToString();
            }
        }

        public string SpecialtyOut
        {
            get
            {
                return Tables[SPECIALTIES].Rows.Find(SpecialtyID)?[SPECIALTY_NAME]?.ToString();
            }
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
                             result.GroupName = reader.GetAttribute(nameof(GroupName));
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
                writer.WriteAttributeString(nameof(GroupName), GroupName);
                writer.WriteEndElement();
            });
        }
    }
}