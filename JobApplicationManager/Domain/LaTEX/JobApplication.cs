// <copyright file="JobApplication.cs" company="Sascha Manns">
// Copyright (c) 2025 Sascha Manns.
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and
// associated documentation files (the “Software”), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell 
// copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to 
// the following conditions:

// The above copyright notice and this permission notice shall be included in all copies or substantial 
// portions of the Software.

// THE SOFTWARE IS PROVIDED “AS IS”, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, 
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A 
// PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR 
// COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN 
// ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH 
// THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
// </copyright>

using BitlyAPI;

namespace JobApplicationManager.Domain.LaTEX
{
    public class JobApplication
    {
        public string Url { get; set; }
        public string Email { get; set; }
        public string JobTitle { get; set; }
        public string SubjectPrefix { get; set; }

        /// <summary>
        /// That Constructor creates the Application object by using the arguments.
        /// </summary>
        /// <param name="aurl">comes directly from the gui txtUrl.Text</param>
        /// <param name="aemail">comes directly from the gui txtEmail.Text</param>
        /// <param name="ajobtitle">comes directly from the gui txtJobtitle.Text</param>
        public JobApplication(string aurl, string aemail, string ajobtitle)
        {
            Url = aurl;
            Email = aemail;
            JobTitle = FixJobApplication(ajobtitle);
            SubjectPrefix = "Resources.Strings.AppResources.ApplicationSubjectprefix;";
        }

        /// <summary>
        /// This method gets the jobtitle and escapes the hash symbol and the ampersand. Otherwise it breaks the LaTEX build.
        /// </summary>
        /// <param name="ajobtitle">comes from the constructor.</param>
        /// <returns></returns>
        public string FixJobApplication(string ajobtitle)
        {
            string prepare = ajobtitle;
            prepare = prepare.Replace(@"#", @"\#");
            prepare = prepare.Replace(@"&", @"\&");
            return prepare;
        }

        public async Task UseBitLy(string? apkikey, string url)
        {
            if (apkikey != null || apkikey != string.Empty)
            {
                Bitly bitly = new Bitly(apkikey);
                BitlyLink? linkResponse = await bitly.PostShorten(url);
                Url = linkResponse.Link;
            }
        }
    }
}
