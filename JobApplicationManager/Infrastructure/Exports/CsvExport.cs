// <copyright file="CsvExport.cs" company="Sascha Manns">
// Copyright (c) 2025 Sascha Manns.
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and
// associated documentation files (the “Software”), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell 
// copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to 
// the following conditions:
//
// The above copyright notice and this permission notice shall be included in all copies or substantial 
// portions of the Software.

// THE SOFTWARE IS PROVIDED “AS IS”, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, 
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A 
// PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR 
// COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN 
// ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH 
// THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
// </copyright>

using JobApplicationManager.Infrastructure.Exports.Models;

namespace JobApplicationManager.Infrastructure.Exports;

using JobApplicationManager.Infrastructure.Mappers;

using Microsoft.Extensions.Localization;

using Saigkill.Toolbox.Services;

/// <summary>
/// Provides functionality for exporting job application data to a CSV file.
/// </summary>
/// <remarks>This class includes methods for writing job application details to a CSV file stored in the
/// user's application data folder. It is designed to facilitate the management of job application records by
/// appending new entries to an existing CSV file.</remarks>
public class CsvExport(ICsvService csvService, IStringLocalizer localizer)
{
    public void WriteCsv(string company, string jobtitle, string city, string joburl)
    {
        var appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        var srcPath = Path.Combine(appDataPath, "latex_curriculum_vitae");

        var path = Path.Combine(srcPath, "JobApplications.csv");
        var classMap = new JobApplicationMap();
        var result = csvService.Read(path, ",", classMap, "de-DE");

        var jobApplicationToAdd = new CsvExportModel
        {
            Company = company,
            Jobtitle = jobtitle,
            City = city,
            Status = localizer["EmailSent"],
            EmailSent = DateTime.Today,
            JobOfferUrl = joburl
        };

        result.Add(jobApplicationToAdd);
        csvService.WriteAsync(result, path, ",", "de-DE");
    }
}