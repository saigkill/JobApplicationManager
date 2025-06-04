// <copyright file="ReminderService.cs" company="Sascha Manns">
// Copyright (c) 2025 Sascha Manns.
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and
// associated documentation files (the “Software”), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all copies or substantial
// portions of the Software.
// 
// THE SOFTWARE IS PROVIDED “AS IS”, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A
// PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR
// COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN
// ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH
// THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
// </copyright>

using JobApplicationManager.Infrastructure.Data;
using JobApplicationManager.Resources.Localize;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

using MimeKit;

namespace JobApplicationManager.Infrastructure.Services;

public class ReminderService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly TimeSpan _interval = TimeSpan.FromHours(1); // Intervall für die Abfrage
    private readonly IStringLocalizer<SharedResource> _localizer;

    public ReminderService(IServiceProvider serviceProvider, IStringLocalizer<SharedResource> localizer)
    {
        _serviceProvider = serviceProvider;
        _localizer = localizer;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<JobApplicationManagerContext>();
                var emailService = scope.ServiceProvider.GetRequiredService<IJamEmailService>();

                var reminderSubject = _localizer["ReminderSubject"];
                var reminderText = _localizer["ReminderText"];
                var today = DateTime.UtcNow;
                var reminderThreshold = today.AddDays(-14);

                var overdueInterviews = await dbContext.JobApplications
                    .Where(a =>
                        (a.FirstInterview.HasValue && a.FirstInterview.Value <= reminderThreshold &&
                         a.SecondInterview == null) ||
                        (a.SecondInterview.HasValue && a.SecondInterview.Value <= reminderThreshold &&
                         a.ThirdInterview == null) ||
                        (a.ThirdInterview.HasValue && a.ThirdInterview.Value <= reminderThreshold) ||
                        (a.EmailSent <= reminderThreshold)).Include(jobApplication => jobApplication.User)
                    .Include(jobApplication => jobApplication.Company)
                    .ToListAsync(cancellationToken: stoppingToken);

                foreach (var application in overdueInterviews)
                {
                    var contact = await dbContext.Contacts.Where(c => c.CompanyId == application.CompanyId).FirstOrDefaultAsync(cancellationToken: stoppingToken);
                    var message = new MimeMessage();
                    message.From.Add(new MailboxAddress("Job Application Manager Reminder", application.User.Email));
                    message.To.Add(new MailboxAddress(application.User.Firstname + " " + application.User.Surname,
                        application.User.Email));
                    message.Subject = reminderSubject + " " + application.Jobtitle;
                    message.Body = new TextPart("plain")
                    {
                        Text = string.Format(reminderText, application.User.Firstname, application.Company?.Name ?? "Unknown Company", application.Jobtitle, contact?.Email ?? "No contact email available")
                    };
                    await emailService.SendMessageAsync(message);
                }

            }

            await Task.Delay(_interval, stoppingToken); // Wartezeit bis zur nächsten Ausführung
        }
    }
}
