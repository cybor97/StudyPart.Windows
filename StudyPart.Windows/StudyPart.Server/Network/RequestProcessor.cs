using StudyPart.Core.Network;
using static StudyPart.Core.Data.DBHolder;
namespace StudyPart.Server.Network
{
    public static class RequestProcessor
    {
        public static Response Process(this Request req)//TODO: Implement me!
        {
            var result = new Response();
            switch (req.Type)
            {
                case "TEST": return new Response("OK");
                case "LOG_IN":
                    break;
                case "REGISTER":
                    break;
                case "GET":
                    switch (req.In)
                    {
                        case GROUPS:
                            break;
                        case MARKS:
                            break;
                        case STUDENTS:
                            break;
                        case SUBJECTS:
                            break;
                        case ACCOUNTS:
                            break;
                        case DEPARTMENTS:
                            break;
                        case SPECIALTIES:
                            break;
                        case TEACHERS:
                            break;
                    }
                    break;
                case "ADD":
                    switch (req.In)
                    {
                        case GROUPS:
                            break;
                        case MARKS:
                            break;
                        case STUDENTS:
                            break;
                        case SUBJECTS:
                            break;
                        case ACCOUNTS:
                            break;
                        case DEPARTMENTS:
                            break;
                        case SPECIALTIES:
                            break;
                        case TEACHERS:
                            break;
                    }
                    return new Response("OK");
                case "SET":
                    switch (req.In)
                    {
                        case GROUPS:
                            break;
                        case MARKS:
                            break;
                        case STUDENTS:
                            break;
                        case SUBJECTS:
                            break;
                        case ACCOUNTS:
                            break;
                        case DEPARTMENTS:
                            break;
                        case SPECIALTIES:
                            break;
                        case TEACHERS:
                            break;
                    }
                    return new Response("OK");
                case "DELETE":
                case "REMOVE":
                    switch (req.In)
                    {
                        case GROUPS:
                            break;
                        case MARKS:
                            break;
                        case STUDENTS:
                            break;
                        case SUBJECTS:
                            break;
                        case ACCOUNTS:
                            break;
                        case DEPARTMENTS:
                            break;
                        case SPECIALTIES:
                            break;
                        case TEACHERS:
                            break;
                    }
                    return new Response("OK");
            }
            return result;
        }
    }
}