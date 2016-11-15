using System.Collections.Generic;
using System.Xml;

namespace StudyPart.Windows.Data
{
    public class Student
    {
        public long ID { get; set; }

        public string FullName { get; set; }

        public Group Group { get; set; }

        public List<Mark> Marks { get; set; }

        public static bool operator ==(Student v1, Student v2)
        {
            return v1.Equals(v2);
        }

        public static bool operator !=(Student v1, Student v2)
        {
            return !(v1 == v2);
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            Student student = (Student)obj;
            return FullName == student.FullName && Group == student.Group;
        }

        //IDK what to do with it. It's not recommended to use dinamic properties here... So let it be default.
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public static Student Parse(string xml)
        {
            try
            {
                var result = new Student();
                Utils.ReadXMLString(xml, reader =>
                {
                    while (!reader.EOF)
                        if (reader.IsStartElement(nameof(Student)))
                        {
                            result.FullName = reader.GetAttribute(nameof(FullName));
                            reader.Read();
                        }
                        else if (reader.IsStartElement(nameof(Group)))
                            result.Group = Group.Parse(reader.ReadOuterXml());
                        else if (reader.IsStartElement(nameof(Mark)))
                        {
                            if (result.Marks == null) result.Marks = new List<Mark>();
                            result.Marks.Add(Mark.Parse(reader.ReadOuterXml()));
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
                writer.WriteStartElement(nameof(Student));
                writer.WriteAttributeString(nameof(FullName), FullName);
                if (Group != null)
                    writer.WriteRaw(Group.ToString());
                if (Marks != null)
                    foreach (var current in Marks)
                        writer.WriteRaw(current.ToString());
                writer.WriteEndElement();
            });
        }
    }
}
