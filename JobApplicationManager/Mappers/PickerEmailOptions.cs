#region copyright
// <copyright file="PickerEmailOptions.cs" company="Sascha Manns">
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

using MailKit.Security;

namespace JobApplicationManager.Mappers
{
    public class PickerEmailOptions
    {

        public static SecureSocketOptions GetSecureSocketOptionObject(string pickerString)
        {
            switch (pickerString)
            {
                case "None":
                    return MailKit.Security.SecureSocketOptions.None;
                case "Auto":
                    return MailKit.Security.SecureSocketOptions.Auto;
                case "SslOnConnect":
                    return MailKit.Security.SecureSocketOptions.SslOnConnect;
                case "StartTls":
                    return MailKit.Security.SecureSocketOptions.StartTls;
                case "StartTlsWhenAvailable":
                    return MailKit.Security.SecureSocketOptions.StartTlsWhenAvailable;
                default:
                    return MailKit.Security.SecureSocketOptions.None;
            }
        }

        public static string GetSecureSocketOptionString(MailKit.Security.SecureSocketOptions options)
        {
            switch (options)
            {
                case MailKit.Security.SecureSocketOptions.None:
                    return "None";
                case MailKit.Security.SecureSocketOptions.Auto:
                    return "Auto";
                case MailKit.Security.SecureSocketOptions.SslOnConnect:
                    return "SslOnConnect";
                case MailKit.Security.SecureSocketOptions.StartTls:
                    return "StartTls";
                case MailKit.Security.SecureSocketOptions.StartTlsWhenAvailable:
                    return "StartTlsWhenAvailable";
                default:
                    return "None";
            }
        }
    }
}
