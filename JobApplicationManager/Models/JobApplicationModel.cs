#region copyright
// <copyright file="JobApplicationModel.cs" company="Sascha Manns">
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

using CsvHelper.Configuration.Attributes;

using Constants = JobApplicationManager.Exports.Constants;


namespace JobApplicationManager.Models
{
    public class JobApplicationModel
    {
        [Name(Constants.CsvHeaders.Company)]
        public string Company { get; set; }
        [Name(Constants.CsvHeaders.Jobtitle)]
        public string Jobtitle { get; set; }
        [Name(Constants.CsvHeaders.City)]
        public string City { get; set; }
        [Name(Constants.CsvHeaders.Status)]
        public string Status { get; set; }
        [Name(Constants.CsvHeaders.EmailSent)]
        public DateTime EmailSent { get; set; }
        [Name(Constants.CsvHeaders.JobOfferUrl)]
        public string JobOfferUrl { get; set; }
    }
}
