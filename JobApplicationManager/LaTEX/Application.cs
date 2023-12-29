#region copyright
// <copyright file="Application.cs" company="Sascha Manns">
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
using BitlyAPI;

namespace JobApplicationManager.LaTEX
{
    /// <summary>
    /// This is a class for instancing the Application object
    /// </summary>
    internal class Application
    {
        public string URL { get; set; }
        public string Email { get; set; }
        public string Jobtitle { get; set; }
        public string SubjectPrefix { get; set; }

        /// <summary>
        /// That Constructor creates the Application object by using the arguments.
        /// </summary>
        /// <param name="aurl">comes directly from the gui txtUrl.Text</param>
        /// <param name="aemail">comes directly from the gui txtEmail.Text</param>
        /// <param name="ajobtitle">comes directly from the gui txtJobtitle.Text</param>
        public Application(string aurl, string aemail, string ajobtitle)
        {
            URL = aurl;
            Email = aemail;
            Jobtitle = FixJobApplication(ajobtitle);
            SubjectPrefix = Resources.Strings.AppResources.ApplicationSubjectprefix;
        }

        /// <summary>
        /// This method gets the jobtitle and escapes the hash symbol and the ampersand. Otherwise it breaks the LaTEX build.
        /// </summary>
        /// <param name="ajobtitle">comes from the constructor.</param>
        /// <returns></returns>
        public string FixJobApplication(string ajobtitle)
        {
            string _prepare = ajobtitle;
            _prepare = _prepare.Replace(@"#", @"\#");
            _prepare = _prepare.Replace(@"&", @"\&");
            return _prepare;
        }

        public async Task UseBitLy(string apkikey, string url)
        {
            var bitly = new Bitly(apkikey);
            var linkResponse = await bitly.PostShorten(url);
            URL = linkResponse.Link;
        }
    }
}