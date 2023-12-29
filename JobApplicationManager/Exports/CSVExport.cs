#region copyright
// <copyright file="CSVExport.cs" company="Sascha Manns">
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
#region dependencies
#endregion

namespace JobApplicationManager.Exports
{
    public static class CsvExport
    {
        public static void WriteCSV(string company, string jobtitle, string city, string joburl)
        {
            string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string srcPath = Path.Combine(appDataPath, "latex_curriculum_vitae");

            var csvParserService = new CsvParserService();
            var path = Path.Combine(srcPath, "JobApplications.csv");
            var result = csvParserService.ReadCsvFileToJobApplicationModel(path);

            var jobApplicationToAdd = new JobApplicationModel()
            {
                Company = company,
                Jobtitle = jobtitle,
                City = city,
                Status = Resources.Strings.AppResources.CsvEmailSent,
                EmailSent = DateTime.Today,
                JobOfferUrl = joburl
            };

            result.Add(jobApplicationToAdd);
            csvParserService.WriteNewCsvFile(path, result);
        }
    }
}
