using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MySQLCompare.Models
{
    class Profile
    {
        public Profile(string name, string host, int port, string userName, string password, string schema)
        {
            Name = name;
            Host = host;
            Port = port;
            UserName = userName;
            Password = password;
            Schema = schema;
        }

        public override string ToString()
        {
            return String.Format("{0} = {1}@{2}:{3}.{4}", Name, UserName, Host, Port, Schema);
        }

        public string Name
        {
            get;
            set;
        }

        public string Host
        {
            get;
            set;
        }

        public int Port
        {
            get;
            set;
        }

        public string UserName
        {
            get;
            set;
        }

        public string Password
        {
            get;
            set;
        }

        public string Schema
        {
            get;
            set;
        }
    }
}
