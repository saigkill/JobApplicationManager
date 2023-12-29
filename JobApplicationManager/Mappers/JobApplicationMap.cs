#region copyright
// <copyright file="JobApplicationMap.cs" company="Sascha Manns">
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
using CsvHelper.Configuration;
#endregion

namespace JobApplicationManager.Mappers
{
    public sealed class JobApplicationMap : ClassMap<JobApplicationModel>
    {
        public JobApplicationMap()
        {
            Map(m => m.Company).Name(Constants.CsvHeaders.Company);
            Map(m => m.Jobtitle).Name(Constants.CsvHeaders.Jobtitle);
            Map(m => m.City).Name(Constants.CsvHeaders.City);
            Map(m => m.Status).Name(Constants.CsvHeaders.Status);
            Map(m => m.EmailSent).Name(Constants.CsvHeaders.EmailSent);
            Map(m => m.JobOfferUrl).Name(Constants.CsvHeaders.JobOfferUrl);
        }
    }
}
