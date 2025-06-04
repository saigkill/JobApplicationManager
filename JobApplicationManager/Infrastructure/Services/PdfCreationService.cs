// <copyright file="PdfCreationService.cs" company="Sascha Manns">
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

using PdfSharpCore.Pdf;
using PdfSharpCore.Pdf.IO;

namespace JobApplicationManager.Infrastructure.Services;

public class PdfCreationService
{
    private static readonly string TmpDir = Saigkill.Toolbox.Generators.TemporaryDirectory.GetTemporaryDirectory();

    public static string GetFinalPdfName(string firstname, string familyname)
    {
        //string finalpdf = Properties.Resources.AppDoc + firstname + "_" + familyname + ".pdf";
        //return finalpdf;
        return String.Empty;
    }

    /// <summary>
    /// This method combines pdfs. So you get a pdf for your certificates of employment, a seperate pdf for other certificates and a final pdf what contains
    /// letter of application, curriculum vitae and certs.
    /// </summary>
    /// <param name="firstname">comes from myuser.firstname (contains users first name)</param>
    /// <param name="familyname">comes from myuser.familyname (contains users surname)</param
    public void CombineApplication(string firstname, string familyname)
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