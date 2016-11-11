using System;

namespace StudyPart.Windows.Data
{
    class Mark
    {
        public long ID { get; set; }

        public Subject Subject { get; set; }

        public DateTime TestDate { get; set; }

        public int MarkValue { get; set; }

        public int YearHalf { get; set; }
    }
}
