using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using StudyPart.Core;
using StudyPart.Core.Data;

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
        #region Constructors-converters
        public Response(List<string> data) : this(data.ConvertAll(c => (object)c))
        {

        }

        public Response(List<Group> data) : this(data.ConvertAll(c => (object)c))
        {

        }

        public Response(List<Account> data) : this(data.ConvertAll(c => (object)c))
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

        public Response(List<Department> data) : this(data.ConvertAll(c => (object)c))
        {

        }

        public Response(List<Specialty> data) : this(data.ConvertAll(c => (object)c))
        {

        }

        public Response(List<Teacher> data) : this(data.ConvertAll(c => (object)c))
        {

        }
        #endregion


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
                    if (current is string)
                        writer.WriteElementString(nameof(String), current.ToString());
                    else if (new[]
                                {
                                    nameof(Group),
                                    nameof(Mark),
                                    nameof(Student),
                                    nameof(Subject),
                                    nameof(Account),
                                    nameof(Department),
                                    nameof(Specialty),
                                    nameof(Teacher)
                                }.Contains(current.GetType().Name))
                        writer.WriteRaw(current.ToString());
                writer.WriteEndElement();
            });
        }
    }
}
