#region copyright
// <copyright file="Company.cs" company="Sascha Manns">
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

// Dependencies

namespace JobApplicationManager.LaTEX
{
    /// <summary>
    /// This class is for building a instance of the Company object.
    /// </summary>
    internal class Company
    {
        public string Name { get; set; }
        public string Street { get; set; }
        public string ZIP { get; set; }
        public string City { get; set; }

        /// <summary>
        /// This Constructor sets the given arguments to this class Properties.
        /// </summary>
        /// <param name="coname">comes directly from gui txtCompanyName.Text (Contains companies name)</param>
        /// <param name="costreet">comes directly from the gui txtCompanyStreet.Text (Contains companies street)</param>
        /// <param name="czip">comes directly from the gui txtZIP.Text (Contains companies ZIP)</param>
        /// <param name="cocity">comes directly from the gui txtCity.Text (Contains companies city)</param>
        public Company(string coname, string costreet, string czip, string cocity)
        {
            Name = coname;
            Street = costreet;
            ZIP = czip;
            City = cocity;
        }

        /// <summary>
        /// This constructor will be used in case of we haven't a correct address for the company. It sets just the company name.
        /// </summary>
        /// <param name="coname">comes directly from gui txtCompanyName.Text (Contains companies name)</param>
        public Company(string coname)
        {
            Name = coname;
            Street = string.Empty;
            ZIP = string.Empty;
            City = string.Empty;
        }
    }
}
