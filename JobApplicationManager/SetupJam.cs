#region copyright
// <copyright file="SetupJam.cs" company="Sascha Manns">
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

using System.Text.RegularExpressions;

namespace JobApplicationManager
{
    /// <summary>
    /// This class contains methods for doing the setup
    /// </summary>
    public static class SetupJam
    {
        private static readonly string AppDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

        /// <summary>
        /// This method copies by the first run a couple of files to the AppData directory.
        /// </summary>
        public static void CheckDocumentsPath()
        {
            string jamDocsPath = Path.Combine(AppDataPath, "JobApplicationManager", "Letter_of_Application", "letter_of_application.tex");
            bool jamDocsPathExists = File.Exists(jamDocsPath);
            if (!jamDocsPathExists)
            {
                CreateDocumentsPath();
                CopyDocuments();
            }
        }

        /// <summary>
        /// This method just creates the directory structure, what lcv expected.
        /// </summary>
        private static void CreateDocumentsPath()
        {
            string[] lcvDocsPath = {Path.Combine(AppDataPath, "JobApplicationManager", "Letter_of_Application"), Path.Combine(AppDataPath, "JobApplicationManager", "Curriculum_Vitae"), Path.Combine(AppDataPath, "JobApplicationManager", "Pictures"),
                Path.Combine(AppDataPath, "JobApplicationManager", "Appendix"), Path.Combine(AppDataPath, "JobApplicationManager", "Appendix", "Bibliography"), Path.Combine(AppDataPath, "JobApplicationManager", "Appendix", "Certificates"),
                Path.Combine(AppDataPath, "JobApplicationManager", "Appendix", "Certificates_of_Employment") };
            foreach (string docsPath in lcvDocsPath)
            {
                Directory.CreateDirectory(docsPath);
            }
        }

        /// <summary>
        /// This method copies a couple of needed files to the AppData directory.
        /// </summary>
        private static void CopyDocuments()
        {
            // https://stackoverflow.com/questions/17782707/how-to-cut-out-a-part-of-a-path/17782741
            string currentDir = AppDomain.CurrentDomain.BaseDirectory;
            string[] extract = Regex.Split(currentDir, "bin");
            string main = extract[0].TrimEnd('\\');
            string targetPath = Path.Combine(AppDataPath, "JobApplicationManager");

            // Letter of Application
            File.Copy(Path.Combine(main, "Resources", "UserResources", "Letter_of_Application", "letter_of_application.tex"), Path.Combine(targetPath, "Letter_of_Application", "letter_of_application.tex"));

            // Curriculum Vitae
            File.Copy(Path.Combine(main, "Resources", "UserResources", "Curriculum_Vitae", "curriculum_vitae.tex"), Path.Combine(targetPath, "Curriculum_Vitae", "curriculum_vitae.tex"));
            File.Copy(Path.Combine(main, "Resources", "UserResources", "Curriculum_Vitae", "friggeri-cv.cls"), Path.Combine(targetPath, "Curriculum_Vitae", "friggeri-cv.cls"));

            // Pictures
            string[] picList = Directory.GetFiles(Path.Combine(main, "Resources", "UserResources", "Pictures"), "*");
            string srcDir = Path.Combine(main, "Resources", "UserResources", "Pictures");
            string destDir = Path.Combine(targetPath, "Pictures");
            foreach (string pic in picList)
            {
                // Remove path from the file name.
                string fName = pic.Substring(srcDir.Length + 1);

                // Use the Path.Combine method to safely append the file name to the path.
                // Will overwrite if the destination file already exists.
                File.Copy(Path.Combine(srcDir, fName), Path.Combine(destDir, fName), true);
            }

            // Bibliography
            File.Copy(Path.Combine(main, "Resources", "UserResources", "Appendix", "Bibliography", "bibliography.bib"), Path.Combine(targetPath, "Appendix", "Bibliography", "bibliography.bib"));

            // Empty CSV file
            File.Copy(Path.Combine(main, "Resources", "UserResources", "CSV", "JobApplications.csv"), Path.Combine(targetPath, "JobApplications.csv"));

            // Database
            File.Copy(Path.Combine(main, "Resources", "UserResources", "Database", "JobApplicationManager.sqlite3"), Path.Combine(targetPath, "JobApplicationsManager.sqlite3"));
        }

        /// <summary>
        /// Adds a temp folder
        /// </summary>
        public static void CheckTmpPath()
        {
            string tmpDir = Path.GetTempPath();
            string myTmpDir = Path.Combine(tmpDir, "JobApplicationManager");

            try
            {
                Directory.SetCurrentDirectory(myTmpDir);
            }
            catch
            {
                Directory.CreateDirectory(myTmpDir);
            }
        }

        /// <summary>
        /// This method cleans up the temporary directory.
        /// </summary>
        public static void Cleanup()
        {
            string tmpDir = Path.GetTempPath();
            string myTmpDir = Path.Combine(tmpDir, "JobApplicationManager");
            Directory.SetCurrentDirectory(myTmpDir);

            string[] delete = Directory.GetFiles(myTmpDir);

            foreach (string del in delete)
            {
                File.Delete(del);
            }
        }
    }
}