using System;
using System.Xml;

namespace StudyPart.Server.Data
{
    public class Mark
    {
        public long ID { get; set; }

        public long SubjectID { get; set; }

        public DateTime TestDate { get; set; }//TODO: Implement universal format provider

        public int MarkValue { get; set; }

        public int YearHalf { get; set; }

        public static bool operator ==(Mark v1, Mark v2)
        {
            return v1.Equals(v2);
        }

        public static bool operator !=(Mark v1, Mark v2)
        {
            return !(v1 == v2);
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            Mark mark = (Mark)obj;
            return SubjectID == mark.SubjectID &&
                TestDate == mark.TestDate &&
                MarkValue == mark.MarkValue &&
                YearHalf == mark.YearHalf;
        }

        //IDK what to do with it. It's not recommended to use dinamic properties here... So let it be default.
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public static Mark Parse(string xml)
        {
            try
            {
                var result = new Mark();
                Utils.ReadXMLString(xml, reader =>
                 {
                     while (!reader.EOF)
                         if (reader.IsStartElement(nameof(Mark)))
                         {
                             result.ID = long.Parse(reader.GetAttribute(nameof(ID)));
                             result.TestDate = DateTime.Parse(reader.GetAttribute(nameof(TestDate)));
                             result.MarkValue = int.Parse(reader.GetAttribute(nameof(MarkValue)));
                             result.YearHalf = int.Parse(reader.GetAttribute(nameof(YearHalf)));
                             reader.Read();
                         }
                         else if (reader.IsStartElement(nameof(SubjectID)))
                             result.SubjectID = long.Parse(reader.ReadOuterXml());
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
                writer.WriteStartElement(nameof(Mark));
                writer.WriteAttributeString(nameof(ID), ID.ToString());
                writer.WriteAttributeString(nameof(TestDate), TestDate.ToString());
                writer.WriteAttributeString(nameof(MarkValue), MarkValue.ToString());
                writer.WriteAttributeString(nameof(YearHalf), YearHalf.ToString());

                writer.WriteElementString(nameof(SubjectID), SubjectID.ToString());

                writer.WriteEndElement();
            });
        }
    }
}
