// <copyright file="LatexBuildService.cs" company="Sascha Manns">
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

using JobApplicationManager.Application.Interfaces;
using JobApplicationManager.Domain.Interfaces;
using JobApplicationManager.Domain.Models;
using JobApplicationManager.Infrastructure.Helpers;
using JobApplicationManager.Resources;

using Microsoft.Extensions.Localization;

using System.Diagnostics;

namespace JobApplicationManager.Infrastructure.Services;

public class LatexBuildService(ILogger<LatexBuildService> logger, IUserEntraIdService userEntraIdService, IStringLocalizer<SfResources> localizer) : ILatexBuildService
{
    private static readonly string TmpDir = Saigkill.Toolbox.Generators.TemporaryDirectory.GetTemporaryDirectory();
    private readonly string _currentUser = userEntraIdService.GetUserId() ?? "unknown_user";

    public void GenerateApplication(ApplicationConfigModel acm)
    {
        PrepareBuild();
        CreateApplicationConfig(acm);
        CompileApplication();

        Setup.Cleanup();
    }

    public void PrepareBuild()
    {
        string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        string srcPath = Path.Combine(appDataPath, "JobApplicationManager");
        string tempBuildDir = Path.Combine(TmpDir, "JobApplicationManager", _currentUser);

        try
        {
            // Letter of Application
            File.Copy(Path.Combine(srcPath, "Letter_of_Application", "letter_of_application.tex"), Path.Combine(tempBuildDir, "letter_of_application.tex"));

            // Curriculum Vitae
            File.Copy(Path.Combine(srcPath, "Curriculum_Vitae", "curriculum_vitae.tex"), Path.Combine(tempBuildDir, "curriculum_vitae.tex"));
            File.Copy(Path.Combine(srcPath, "Curriculum_Vitae", "curriculum_vitae.cls"), Path.Combine(tempBuildDir, "curriculum_vitae.cls"));

            // Bibliography
            File.Copy(Path.Combine(srcPath, "Appendix", "Bibliography", "bibliography.bib"), Path.Combine(tempBuildDir, "bibliography.bib"));

            // Personal Data
            File.Copy(Path.Combine(srcPath, "personal_data.tex"), Path.Combine(tempBuildDir, "personal_data.tex"));
        }
        catch (Exception e)
        {
            logger.LogError(e, "Copying failed");
            throw new IOException($"Copying failed: {e.Message}");
        }

    }

    public string GetSubject(string jobtitle)
    {
        string subjectprefix = localizer["Subjectprefix"];
        string subject = subjectprefix + " " + jobtitle;
        return subject;
    }

    public string GetEmailSubject(string jobtitle)
    {
        string subjectprefix = localizer["Subjectprefix"];
        string subject = subjectprefix + " " + jobtitle;
        subject = subject.Replace(@"\#", @"#");
        subject = subject.Replace(@"\&", @"&");
        return subject;
    }

    public void CreateApplicationConfig(ApplicationConfigModel acm)
    {
        string[] lines = { "\\def\\jobtitle{" + acm.JobTitle + "}", "\\def\\company{" + acm.Company + "}", "\\def\\contact{" + acm.Contact + "}", "\\def\\street{" + acm.Street + "}", "\\def\\city{" + acm.City + "}", "\\def\\salutation{" + acm.Salutation + "}", "\\def\\subject{" + acm.Subject + "}", "\\def\\addressstring{" + acm.Address + "}" };
        string tempBuildDir = Path.Combine(TmpDir, "JobApplicationManager", _currentUser);
        using StreamWriter outputFile = new StreamWriter(Path.Combine(tempBuildDir, "application_details.tex"));
        foreach (string line in lines)
        {
            outputFile.WriteLine(line);
        }
        outputFile.Close();
    }

    public void CompileApplication()
    {
        string tempBuildDir = Path.Combine(TmpDir, "JobApplicationManager", _currentUser);
        Directory.SetCurrentDirectory(tempBuildDir);

        string[] commands = {
            "pdflatex letter_of_application.tex",
            "xelatex curriculum_vitae.tex",
            "biber curriculum_vitae.bcf",
            "xelatex curriculum_vitae.tex"
        };

        foreach (string cmd in commands)
        {
            Process process = new Process();
            ProcessStartInfo startInfo = new ProcessStartInfo();

            if (OperatingSystem.IsWindows())
            {
                startInfo.FileName = "cmd.exe";
                startInfo.Arguments = $"/C {cmd}";
            }
            else // Linux oder macOS
            {
                startInfo.FileName = "/bin/bash";
                startInfo.Arguments = $"-c \"{cmd}\"";
            }

            startInfo.WindowStyle = ProcessWindowStyle.Hidden;
            startInfo.RedirectStandardOutput = true;
            startInfo.RedirectStandardError = true;
            startInfo.UseShellExecute = false;

            process.StartInfo = startInfo;
            process.Start();
            string output = process.StandardOutput.ReadToEnd();
            string error = process.StandardError.ReadToEnd();
            process.WaitForExit();

            // Optional: Logging
            if (!string.IsNullOrWhiteSpace(output))
                logger.LogError(output);
            if (!string.IsNullOrWhiteSpace(error))
                logger.LogError(error);
        }
    }
}