using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySQLCompare.Models;

namespace MySQLCompare.ViewModels
{
    class ProfileItemViewModel
    {
        private Profile _profile;

        public ProfileItemViewModel()
        {
            _profile = new Profile("New profile", "", 3306, "", "", "");
        }

        public ProfileItemViewModel(Profile profile)
        {
            _profile = profile;
        }

        public Profile Profile
        {
            get
            {
                return _profile;
            }

            set
            {
                _profile = value;
            }
        }

        public string Name
        {
            get
            {
                return _profile.Name;
            }
        }

        public string Host
        {
            get
            {
                return _profile.Host;
            }
        }

        public int Port
        {
            get
            {
                return _profile.Port;
            }
        }

        public string UserName
        {
            get
            {
                return _profile.UserName;
            }
        }

        public string Schema
        {
            get
            {
                return _profile.Schema;
            }
        }
    }
}
