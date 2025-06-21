// <copyright file="Setup.cs" company="Sascha Manns">
// Copyright (c) 2025 Sascha Manns.
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and
// associated documentation files (the “Software”), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all copies or substantial
// portions of the Software.
// 
// THE SOFTWARE IS PROVIDED “AS IS”, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A
// PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR
// COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN
// ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH
// THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
// </copyright>

using System.Text.RegularExpressions;

namespace JobApplicationManager.Infrastructure.Helpers;

public static class Setup
{
    private static NLog.Logger _logger = NLog.LogManager.GetCurrentClassLogger();

    private static readonly string AppDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

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

    private static void CreateDocumentsPath()
    {
        string[] lcvDocsPath = {Path.Combine(AppDataPath, "JobApplicationManager", "Letter_of_Application"), Path.Combine(AppDataPath, "JobApplicationManager", "Curriculum_Vitae"), Path.Combine(AppDataPath, "JobApplicationManager", "Pictures"),
                Path.Combine(AppDataPath, "JobApplicationManager", "Appendix"), Path.Combine(AppDataPath, "JobApplicationManager", "Appendix", "Bibliography"), Path.Combine(AppDataPath, "JobApplicationManager", "Appendix", "Certificates"),
                Path.Combine(AppDataPath, "JobApplicationManager", "Appendix", "Certificates_of_Employment") };
        foreach (string docsPath in lcvDocsPath)
        {
            try
            {
                Directory.CreateDirectory(docsPath);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"Problem while creating directory");
                throw new IOException(ex.Message);
            }
        }

        _logger.Info("Created Path for Documents");
    }

    private static void CopyDocuments()
    {
        string currentDir = AppDomain.CurrentDomain.BaseDirectory;
        string[] extract = Regex.Split(currentDir, "bin");
        string main = extract[0].TrimEnd('\\');
        string targetPath = Path.Combine(AppDataPath, "JobApplicationManager");

        // Letter of Application
        try
        {
            File.Copy(
                Path.Combine(main, "Resources", "UserResources", "Letter_of_Application", "letter_of_application.tex"),
                Path.Combine(targetPath, "Letter_of_Application", "letter_of_application.tex"));
        }
        catch (Exception ex)
        {
            _logger.Error(ex, $"Problem while copying Letter of Application");
            throw new IOException($"Problem while copying Letter of Application: {ex.Message}");
        }
        _logger.Info("Copied Letter of Application");

        // Curriculum Vitae
        try
        {
            File.Copy(Path.Combine(main, "Resources", "UserResources", "Curriculum_Vitae", "curriculum_vitae.tex"), Path.Combine(targetPath, "Curriculum_Vitae", "curriculum_vitae.tex"));
            File.Copy(Path.Combine(main, "Resources", "UserResources", "Curriculum_Vitae", "curriculum_vitae.cls"), Path.Combine(targetPath, "Curriculum_Vitae", "curriculum_vitae.cls"));
        }
        catch (Exception e)
        {
            _logger.Error(e, "Problem while copying the Curriculum Vitae");
            throw new IOException($"Problem while copying Curriculum Vitae {e.Message}");
        }

        _logger.Info("Copied CV Template and Class");

        // Pictures
        string[] picList = Directory.GetFiles(Path.Combine(main, "Resources", "UserResources", "Pictures"), "*");
        string srcDir = Path.Combine(main, "Resources", "UserResources", "Pictures");
        string destDir = Path.Combine(targetPath, "Pictures");

        try
        {
            foreach (string pic in picList)
            {
                string fName = pic.Substring(srcDir.Length + 1);

                // Use the Path.Combine method to safely append the file name to the path.
                // Will overwrite if the destination file already exists.
                File.Copy(Path.Combine(srcDir, fName), Path.Combine(destDir, fName), true);
            }
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Problem while creating Pictures directory");
            throw new IOException($"Problem while creating Pictures directory: {ex.Message}");
        }

        _logger.Info("Copied all default Pictures");

        // Bibliography
        try
        {
            File.Copy(
                Path.Combine(main, "Resources", "UserResources", "Appendix", "Bibliography", "bibliography.bib"),
                Path.Combine(targetPath, "Appendix", "Bibliography", "bibliography.bib"));
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Problem while copying Bibliography");
            throw new IOException($"Problem while copying Bibliography: {ex.Message}");
        }
        _logger.Info("Copied default Bibliography");
    }

    public static void Cleanup()
    {
        string myTmpDir = Path.Combine(Path.GetTempPath(), "JobApplicationManager");

        try
        {
            Directory.Delete(myTmpDir);
        }
        catch (Exception e)
        {
            _logger.Error(e, "Error while Cleanup");
            throw new IOException("Error while Cleanup");
        }

        _logger.Info("Cleaned up TempPath");
    }
}