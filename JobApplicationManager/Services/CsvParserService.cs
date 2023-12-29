#region copyright
// <copyright file="CsvParserService.cs" company="Sascha Manns">
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
using System.Text;

using CsvHelper;

using JobApplicationManager.Exceptions;
using JobApplicationManager.Mappers;
#endregion


namespace JobApplicationManager.Services
{
    internal class CsvParserService : ICsvParserService
    {
        public List<JobApplicationModel> ReadCsvFileToJobApplicationModel(string path)
        {
            try
            {
                using var reader = new StreamReader(path, Encoding.Default);
                using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
                csv.Context.RegisterClassMap<JobApplicationMap>();
                var records = csv.GetRecords<JobApplicationModel>().ToList();
                return records;
            }
            catch (UnauthorizedAccessException e)
            {
                throw new JAMException(e.Message);
            }
            catch (FieldValidationException e)
            {
                throw new JAMException(e.Message);
            }
            catch (CsvHelperException e)
            {
                throw new JAMException(e.Message);
            }
            catch (System.Exception e)
            {
                throw new JAMException(e.Message);
            }
        }

        public void WriteNewCsvFile(string path, List<JobApplicationModel> jobApplicationModels)
        {
            using StreamWriter sw = new StreamWriter(path, false, new UTF8Encoding(true));
            using CsvWriter cw = new CsvWriter(sw, CultureInfo.InvariantCulture);
            // cw.Configuration.Delimiter = ",";
            cw.WriteHeader<JobApplicationModel>();
            cw.NextRecord();
            foreach (JobApplicationModel app in jobApplicationModels)
            {
                cw.WriteRecord<JobApplicationModel>(app);
                cw.NextRecord();
            }
        }
    }
}
