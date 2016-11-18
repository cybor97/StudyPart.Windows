using System;
using System.Globalization;
using System.Xml;
using static StudyPart.Core.Data.DBHolder;
using static StudyPart.Core.CommonVariables;

namespace StudyPart.Core.Data
{
    public class Mark
    {
        public long ID { get; set; }

        public long StudentID { get; set; }

        public long SubjectID { get; set; }

        public DateTime TestDate { get; set; }

        public int MarkValue { get; set; }

        public int YearHalf { get; set; }

        public string TestDateOut
        {
            get
            {
                return TestDate.ToString(UniversalDateTimeFormat);
            }
        }

        public string StudentOut
        {
            get
            {
                return Tables[STUDENTS].Rows.Find(StudentID)?[FULL_NAME]?.ToString();
            }
        }

        public string SubjectOut
        {
            get
            {
                return Tables[SUBJECTS].Rows.Find(SubjectID)?[SUBJECT_NAME]?.ToString();
            }
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
                             result.TestDate = DateTime.ParseExact(reader.GetAttribute(nameof(TestDate)), UniversalDateTimeFormat, CultureInfo.InvariantCulture);
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
                writer.WriteAttributeString(nameof(TestDate), TestDate.ToString(UniversalDateTimeFormat));
                writer.WriteAttributeString(nameof(MarkValue), MarkValue.ToString());
                writer.WriteAttributeString(nameof(YearHalf), YearHalf.ToString());

                writer.WriteElementString(nameof(SubjectID), SubjectID.ToString());

                writer.WriteEndElement();
            });
        }
    }
}
