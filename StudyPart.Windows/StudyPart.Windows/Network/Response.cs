using System;
using System.Collections.Generic;
using System.Xml;
using StudyPart.Server.Data;

namespace StudyPart.Server.Network
{
    public class Response
    {
        public List<object> Data { get; set; }

        public Response()
        {
            Data = new List<object>();
        }

        public Response(string str) : this(new List<object>(new[] { str }))
        {

        }

        public Response(List<string> data) : this(data.ConvertAll(c => (object)c))
        {

        }

        public Response(List<Group> data) : this(data.ConvertAll(c => (object)c))
        {

        }

        public Response(List<UserAccount> data) : this(data.ConvertAll(c => (object)c))
        {

        }

        public Response(List<Mark> data) : this(data.ConvertAll(c => (object)c))
        {

        }

        public Response(List<Student> data) : this(data.ConvertAll(c => (object)c))
        {

        }

        public Response(List<Subject> data) : this(data.ConvertAll(c => (object)c))
        {

        }

        Response(List<object> data)
        {
            Data = data;
        }

        public override string ToString()
        {
            return Utils.WriteXMLString((ref XmlWriter writer) =>
            {
                writer.WriteStartElement(nameof(Response));
                foreach (var current in Data)
                    if (current is Group || current is Mark || current is Student || current is Subject || current is UserAccount)
                        writer.WriteRaw(current.ToString());
                    else if (current is string)
                        writer.WriteElementString(nameof(String), current.ToString());
                writer.WriteEndElement();
            });
        }
    }
}
