// <copyright file="JobApplicationModel.cs" company="Sascha Manns">
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

namespace JobApplicationManager.Infrastructure.Exports.Models;

using CsvHelper.Configuration.Attributes;

using JobApplicationManager.Infrastructure.Exports;

/// <summary>
/// Class JobApplicationModel.
/// </summary>
public class CsvExportModel
{
    [Name(Constants.CsvHeaders.Company)]
    public string Company { get; set; } = string.Empty;

    [Name(Constants.CsvHeaders.Jobtitle)]
    public string Jobtitle { get; set; } = string.Empty;

    [Name(Constants.CsvHeaders.City)]
    public string City { get; set; } = string.Empty;

    [Name(Constants.CsvHeaders.Status)]
    public string Status { get; set; } = string.Empty;

    [Name(Constants.CsvHeaders.EmailSent)]
    public DateTime EmailSent { get; set; } = DateTime.UtcNow;

    [Name(Constants.CsvHeaders.JobOfferUrl)]
    public string JobOfferUrl { get; set; } = string.Empty;
}