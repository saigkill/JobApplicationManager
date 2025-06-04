using PdfSharpCore.Pdf;
using PdfSharpCore.Pdf.IO;

using System.Diagnostics;

namespace JobApplicationManager.Domain.LaTEX;

/// <summary>
/// Class for compiling and building the LaTEX documents and it merges all pdfs in one.
/// </summary> 
public static class Build
{
    private static readonly string TmpDir = Saigkill.Toolbox.Generators.TemporaryDirectory.GetTemporaryDirectory();

    public static void PrepareBuild()
    {
        string mytmpDir = Path.Combine(TmpDir, "JobApplicationManager");
        //Directory.CreateDirectory(mytmpDir);
        string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        string srcPath = Path.Combine(appDataPath, "JobApplicationManager");

        // Letter of Application
        File.Copy(Path.Combine(srcPath, "Letter_of_Application", "letter_of_application.tex"), Path.Combine(mytmpDir, "letter_of_application.tex"));

        // Curriculum Vitae
        File.Copy(Path.Combine(srcPath, "Curriculum_Vitae", "curriculum_vitae.tex"), Path.Combine(mytmpDir, "curriculum_vitae.tex"));
        File.Copy(Path.Combine(srcPath, "Curriculum_Vitae", "friggeri-cv.cls"), Path.Combine(mytmpDir, "friggeri-cv.cls"));

        // Bibliography
        File.Copy(Path.Combine(srcPath, "Appendix", "Bibliography", "bibliography.bib"), Path.Combine(mytmpDir, "bibliography.bib"));

        // Personal Data
        File.Copy(Path.Combine(srcPath, "personal_data.tex"), Path.Combine(mytmpDir, "personal_data.tex"));
    }

    public static string GetSubject(string subjectprefix, string jobtitle)
    {
        string subject = subjectprefix + " " + jobtitle;
        return subject;
    }

    public static string GetEmailSubject(string subjectprefix, string jobtitle)
    {
        string subject = subjectprefix + " " + jobtitle;
        subject = subject.Replace(@"\#", @"#");
        subject = subject.Replace(@"\&", @"&");
        return subject;
    }

    /// <summary>
    /// This method creates the job application specific LaTEX file "application_details.tex". It will be used in letter of application. The method doesn't return anything.        
    /// </summary>
    /// <param name="ApplicationConfigModel">Application Model (contains the JobTitle, Company, Contact etc...)</param>
    //public static void CreateApplicationConfig(ApplicationConfigModel acm)
    //{
    //    string[] lines = { "\\def\\jobtitle{" + acm.JobTitle + "}", "\\def\\company{" + acm.Company + "}", "\\def\\contact{" + acm.Contact + "}", "\\def\\street{" + acm.Street + "}", "\\def\\city{" + acm.City + "}", "\\def\\salutation{" + acm.Salutation + "}", "\\def\\subject{" + acm.Subject + "}", "\\def\\addressstring{" + acm.Address + "}" };

    //    string mytmpDir = Path.Combine(tmpDir, "latex_curriculum_vitae");
    //    using StreamWriter outputFile = new StreamWriter(Path.Combine(mytmpDir, "application_details.tex"));
    //    foreach (string line in lines)
    //    {
    //        outputFile.WriteLine(line);
    //    }
    //}

    public static void CompileApplication()
    {
        string mytmpDir = Path.Combine(TmpDir, "JobApplicationManager");
        Directory.SetCurrentDirectory(mytmpDir);

        string[] strCmdText = { "/C pdflatex letter_of_application.tex", "/C xelatex curriculum_vitae.tex", "/C biber curriculum_vitae.bcf", "/C xelatex curriculum_vitae.tex" };

        foreach (string cmd in strCmdText)
        {
            Process process = new Process();
            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                WindowStyle = ProcessWindowStyle.Hidden,
                FileName = "cmd.exe",
                Arguments = cmd
            };
            process.StartInfo = startInfo;
            process.Start();
            process.WaitForExit();
        }
    }

    /// <summary>
    /// This method combines the language specific string for "application documents" with users firstname and familyname. Ex: Application_Documents_Sascha_Manns.pdf
    /// </summary>
    /// <param name="firstname">comes from myuser.firstname (contains users first name)</param>
    /// <param name="familyname">comes from myuser.familyname (contains users surname)</param>
    /// <returns>string finalpdf</returns>
    //public static string GetFinalPdfName(string firstname, string familyname)
    //{
    //    string finalpdf = Properties.Resources.AppDoc + firstname + "_" + familyname + ".pdf";
    //    return finalpdf;
    //}

    /// <summary>
    /// This method combines pdfs. So you get a pdf for your certificates of employment, a seperate pdf for other certificates and a final pdf what contains
    /// letter of application, curriculum vitae and certs.
    /// </summary>
    /// <param name="firstname">comes from myuser.firstname (contains users first name)</param>
    /// <param name="familyname">comes from myuser.familyname (contains users surname)</param
    public static void CombineApplication(string firstname, string familyname)
    {
        string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        string srcPath = Path.Combine(appDataPath, "JobApplicationManager");
        string mytmpDir = Path.Combine(TmpDir, "JobApplicationManager");
        Directory.SetCurrentDirectory(mytmpDir);

        //string cos = Properties.Resources.Cos + firstname + "_" + familyname + ".pdf";
        //string cert = Properties.Resources.Cert + firstname + "_" + familyname + ".pdf";
        //string finalpdf = GetFinalPdfName(firstname, familyname);

        string[] cosEntries = Directory.GetFiles(Path.Combine(srcPath, "Appendix", "Certificates_of_Employment")).OrderByDescending(x => x).ToArray();
        string[] certEntries = Directory.GetFiles(Path.Combine(srcPath, "Appendix", "Certificates")).OrderByDescending(x => x).ToArray();
        //string[] finalEntries = { "letter_of_application.pdf", "curriculum_vitae.pdf", cos, cert };

        // Certificates of Employment
        //MergePdfs(Path.Combine(mytmpDir, cos), cosEntries);

        // Certificates
        //MergePdfs(Path.Combine(mytmpDir, cert), certEntries);

        // Production of the final document
        //MergePdfs(Path.Combine(mytmpDir, finalpdf), finalEntries);
    }

    private static void MergePdfs(string targetPath, params string[] pdfs)
    {
        using PdfDocument targetDoc = new PdfDocument();
        foreach (string pdf in pdfs)
        {
            using PdfDocument pdfDoc = PdfReader.Open(pdf, PdfDocumentOpenMode.Import);
            for (int i = 0; i < pdfDoc.PageCount; i++)
            {
                targetDoc.AddPage(pdfDoc.Pages[i]);
            }
        }
        targetDoc.Save(targetPath);
    }
}
