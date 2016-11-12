namespace StudyPart.Windows.Network
{
    public static class RequestProcessor
    {
        public static Response Process(this Request req)
        {
            var result = new Response();
            switch (req.Type)
            {
                case "TEST": return new Response("OK");
                case "GET":
                    switch (req.In)
                    {
                    }
                    break;
                case "ADD":
                    switch (req.In)
                    {
                    }
                    return new Response("OK");
                case "SET":
                    switch (req.In)
                    {
                    }
                    return new Response("OK");
                case "DELETE":
                case "REMOVE":
                    switch (req.In)
                    {
                    }
                    return new Response("OK");
            }
            return result;
        }
    }
}
