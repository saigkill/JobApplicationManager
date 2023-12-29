#region copyright
// <copyright file="User.cs" company="Sascha Manns">
// Copyright (C) 2023 Sascha Manns, Sascha.Manns@outlook.de
//
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation 
// files (the “Software”), to deal in the Software without restriction, including without limitation the rights to use, copy, 
// modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the 
// Software is furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED “AS IS”, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE 
// WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
// HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, 
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//
// @author: Sascha Manns, Sascha.Manns@outlook.de
// @copyright: 2023, Sascha Manns, https://dev.azure.com/saigkill/JobApplicationManager
// </copyright>
#endregion

// Dependencies

namespace JobApplicationManager.LaTEX
{
    /// <summary>
    /// This class instances the User object.
    /// </summary>
    internal class User
    {
        public string Firstname { get; set; }

        public string Familyname { get; set; }

        public string Street { get; set; }
        public string Zip { get; set; }

        public string City { get; set; }

        public string Phone { get; set; }

        public string Email { get; set; }

        public string Homepage { get; set; }

        public string SmtpServer { set; get; }

        public string SmtpUser { set; get; }

        public string SmtpPass { set; get; }

        public int SmtpPort { set; get; }

        public string BitLyToken { get; set; }

        /// <summary>
        /// That constructor uses the config file to set up all user relevant information.
        /// </summary>
        public User()
        {
            Firstname = Preferences.Get("UserFirstName", "");
            Familyname = Preferences.Get("UserSurname", "");
            Street = Preferences.Get("UserStreet", "");
            City = Preferences.Get("UserCity", "");
            Zip = Preferences.Get("UserZip", "");
            Phone = Preferences.Get("UserPhone", "");
            Email = Preferences.Get("UserEmail", "");
            Homepage = Preferences.Get("UserHomepage", "");
            SmtpServer = Preferences.Get("EmailServer", "");
            SmtpUser = Preferences.Get("EmailUser", "");
            SmtpPass = Preferences.Get("EmailPass", "");
            SmtpPort = Convert.ToInt32(Preferences.Get("EmailSmtpPort", ""));
            BitLyToken = Preferences.Get("ExternalBitLyToken", "");

            UserFile();
        }

        /// <summary>
        /// This method uses the information from the Properties to create the LaTEX user configuration file "personal_data.tex" what will be used by letter of application and also for curriculum_vitae.
        /// </summary>
        private void UserFile()
        {
            string[] lines = { "\\def\\firstname{" + Firstname + "}", "\\def\\familyname{" + Familyname + "}", "\\def\\mystreet{" + Street + "}", "\\def\\mycity{" + City + "}", "\\def\\myphone{" + Phone + "}", "\\def\\myemail{" + Email + "}", "\\def\\myblog{" + Homepage + "}" };

            // Set a variable to the Documents path.
            string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string lcvDataPath = Path.Combine(appDataPath, "latex_curriculum_vitae");
            Directory.CreateDirectory(lcvDataPath);

            // Write the string array to a new file named "WriteLines.txt".
            using StreamWriter outputFile = new StreamWriter(Path.Combine(lcvDataPath, "personal_data.tex"));
            foreach (string line in lines)
            {
                outputFile.WriteLine(line);
            }
        }
    }
}
