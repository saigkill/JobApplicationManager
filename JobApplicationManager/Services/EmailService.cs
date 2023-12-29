#region copyright
// <copyright file="Email.cs" company="Sascha Manns">
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

using JobApplicationManager.Mappers;

using MailKit.Net.Smtp;

using MimeKit;
using MimeKit.Utils;

namespace JobApplicationManager.Services
{
    /// <summary>
    /// This class is for creating and sending job applications via email
    /// </summary>
    public class EmailService : IEmailService
    {
        /// <summary>
        /// This is the main method for creating and sending emails. It doesn't returns anything.        
        /// </summary>
        /// <param name="emailModel">Email Message</param>
        public int CreateMessage(EmailModel emailModel)
        {
            string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string lcvPath = Path.Combine(appDataPath, "JobApplicationManager");
            string tmpDir = Path.GetTempPath();
            string mytmpDir = Path.Combine(tmpDir, "JobApplicationManager");

            string firstName = Preferences.Get("UserFirstName", "");
            string surname = Preferences.Get("UserSurname", "");
            string myname = $"{firstName} {surname}";
            string smtpServer = Preferences.Get("EmailSmtpServer", "");
            string serverOption = Preferences.Get("EmailServerOption", "");
            int smtpPort = int.Parse(Preferences.Get("EmailSmtpPort", "0"));
            string smtpUser = Preferences.Get("EmailUser", "");
            string smtpPass = Preferences.Get("EmailPassword", "");
            string myEmail = Preferences.Get("UserEmail", "");

            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(myname, myEmail));
            message.To.Add(new MailboxAddress(emailModel.ContactName, emailModel.CompEmail));
            message.Bcc.Add(new MailboxAddress(myname, myEmail));
            message.Subject = emailModel.Subject;

            BodyBuilder builder = new BodyBuilder
            {
                // Set the plain-text version of the message text
                TextBody = string.Format(@Resources.Strings.AppResources.EmailPlain, emailModel.Salutation, myname)
            };

            // In order to reference selfie.jpg from the html text, we'll need to add it
            // to builder.LinkedResources and then use its Content-Id value in the img src.
            var image = builder.LinkedResources.Add(Path.Combine(lcvPath, "Pictures", "userpic.jpg"));
            image.ContentId = MimeUtils.GenerateMessageId();

            // Set the html version of the message text
            builder.HtmlBody = string.Format(@Resources.Strings.AppResources.EmailHTML, emailModel.Salutation, myname, image.ContentId);

            //// We may also want to attach a calendar event for Monica's party...
            builder.Attachments.Add(Path.Combine(mytmpDir, emailModel.Attachment));

            //// Now we just need to set the message body and we're done
            message.Body = builder.ToMessageBody();

            // ReSharper disable once ComplexConditionExpression
            if (smtpServer == "" || smtpPort.ToString() == "" || smtpUser == "" || smtpPass == "")
            {
                return 1;
            }
            else
            {
                // Sending out
                using var client = new SmtpClient();
                client.Connect(smtpServer, smtpPort, PickerEmailOptions.GetSecureSocketOptionObject(serverOption));
                // Note: only needed if the SMTP server requires authentication
                client.Authenticate(smtpUser, smtpPass);

                client.Send(message);
                client.Disconnect(true);
                return 0;
            }
        }
    }
}
