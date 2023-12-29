#region copyright
// <copyright file="Contact.cs" company="Sascha Manns">
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
    /// This class is for instancing a Contact object.
    /// </summary>
    internal class Contact
    {
        public string Name { get; set; }

        public string Gender { get; set; }

        public string Salutation { get; set; }

        /// <summary>
        /// This Constructor sets the arguments values to the Properties and instances the object.
        /// </summary>
        /// <param name="cname">comes from the gui txtContactName.Text (Contains the name of the contact)</param>
        /// <param name="gname">comes from the gui (Contains the contacts gender)</param>
        public Contact(string cname, string gname)
        {
            Name = cname;
            Gender = gname;
            Salutation = GetSalutation(Name, Gender);
        }

        /// <summary>
        /// This method creates from the contact name and gender the right salutation. Ex. Dear Mr. Manns
        /// </summary>
        /// <param name="cname">comes from the gui txtContactName.Text (Contains the name of the contact)</param>
        /// <param name="gname">comes from the gui (Contains the contacts gender)</param>
        /// <returns>string salutation</returns>
        private string GetSalutation(string cname, string gname)
        {
            string salutation;

            if (string.IsNullOrEmpty(cname))
            {
                salutation = $"{Resources.Strings.AppResources.ContactSalutationUnknown},";
            }
            else if (gname == Resources.Strings.AppResources.ContactGenderMale)
            {
                salutation = $"{Resources.Strings.AppResources.ContactSalutationMale} {cname},";
            }
            else
            {
                salutation = $"{Resources.Strings.AppResources.ContactSalutationFemale} {cname},";
            }
            return salutation;
        }

        /// <summary>
        /// This method combines the arguments to create a address string for using in the letter of application.
        /// </summary>
        /// <param name="company">comes from company.Name (Contains the company name)</param>
        /// <param name="contactname">comes from contact.Name (Contains the contact name)</param>
        /// <param name="cgender">comes from contact.Gender (Contains contacts gender)</param>
        /// <param name="cstreet">comes from company.Street (Contains companies street)</param>
        /// <param name="czip">comes from company.ZIP (contains companies zip)</param>
        /// <param name="ccity">comes from company.City (Contains companies city)</param>
        /// <returns>string addressline</returns>
        public string Addressline(string company, string contactname, string cgender, string cstreet, string czip, string ccity)
        {
            string addressline;
            company = company.Replace(@"#", @"\#");
            company = company.Replace(@"&", @"\&");

            addressline = company + "\\\\";
            if (string.IsNullOrEmpty(contactname) || cgender == Resources.Strings.AppResources.ContactGenderUnknown)
            {
                addressline += Resources.Strings.AppResources.ContactHRDepartment + " \\\\";
            }
            else
            {
                if (cgender == Resources.Strings.AppResources.ContactGenderMale)
                {
                    addressline = addressline + Resources.Strings.AppResources.ContactAddressMale + " " + contactname + "\\\\";
                }
                else
                {
                    addressline = addressline + Resources.Strings.AppResources.ContactAddressFemale + " " + contactname + "\\\\";
                }
            }

            if (!string.IsNullOrEmpty(cstreet))
            {
                addressline = addressline + cstreet + "\\\\";
            }

            if (!string.IsNullOrEmpty(czip) && czip != "0")
            {
                addressline += czip + " ";
            }

            if (!string.IsNullOrEmpty(ccity))
            {
                addressline += ccity;
            }

            return addressline;
        }
    }
}
