using StudyPart.Core.Data;

namespace StudyPart.Server
{
    public static class Program
    {
        static void Main()
        {
            DBHolder.Init(true);
        }
    }
}
