using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Reflection;
using MySQLCompare.Models;

namespace MySQLCompare.Settings
{
    class ProfileSettings
    {
        private static string _profileSettingsPath = @"Settings\Profiles.xml";

        public static List<Profile> getProfiles()
        {
            List<Profile> profiles = new List<Profile>();

            // Parse the XML settings file to get the stored MySQL connection profiles
            // System.IO.Stream settingsFile = getSettingsFile();
            string settingsFile = getSettingsFilePath();
            XmlReader settingsReader = XmlReader.Create(settingsFile);
            settingsReader.MoveToContent();

            while (settingsReader.Read())
            {
                if (settingsReader.NodeType == XmlNodeType.Element)
                {
                    if (String.Compare(settingsReader.Name, "Profile", true) == 0)
                    {
                        string Name = "";
                        string Host = "";
                        string Port = "";
                        string Username = "";
                        string Password = "";
                        string Schema = "";

                        // Get all the attributes for this profile
                        while (settingsReader.MoveToNextAttribute())
                        {
                            if (String.Compare(settingsReader.Name, "Name", true) == 0)
                            {
                                Name = settingsReader.Value;
                            }
                            else if (String.Compare(settingsReader.Name, "Host", true) == 0)
                            {
                                Host = settingsReader.Value;
                            }
                            else if (String.Compare(settingsReader.Name, "Port", true) == 0)
                            {
                                Port = settingsReader.Value;
                            }
                            else if (String.Compare(settingsReader.Name, "Username", true) == 0)
                            {
                                Username = settingsReader.Value;
                            }
                            else if (String.Compare(settingsReader.Name, "Password", true) == 0)
                            {
                                Password = settingsReader.Value;
                            }
                            else if (String.Compare(settingsReader.Name, "Schema", true) == 0)
                            {
                                Schema = settingsReader.Value;
                            }
                        }

                        // All attributes are required, so only create the Profile object if we have all attributes
                        if (Name != "" && Host != "" && Port != "" && Username != "" && Password != "" && Schema != "")
                        {
                            // TODO: Catch invalid int exception (FormatException?)

                            int portNum = Int32.Parse(Port);
                            Profile newProfile = new Profile(Name, Host, portNum, Username, Password, Schema);
                            profiles.Add(newProfile);
                        }
                    }
                }
            }

            return profiles;
        }

        // Useful for when embedding the file in the assembly
        protected static System.IO.Stream getSettingsFile()
        {
            return System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream("MySQLCompare.Settings.Profiles.xml");
        }

        // Useful for when copying the file to the output directory
        public static string getSettingsFilePath()
        {
            return System.AppDomain.CurrentDomain.BaseDirectory + _profileSettingsPath;
        }
    }
}
