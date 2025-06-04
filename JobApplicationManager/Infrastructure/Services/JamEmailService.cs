// <copyright file="EmailService.cs" company="Sascha Manns">
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

using Ardalis.GuardClauses;

using JobApplicationManager.Infrastructure.Data;

using MailKit.Net.Smtp;

using MimeKit;

namespace JobApplicationManager.Infrastructure.Services;

public class JamEmailService : IJamEmailService
{
    private readonly ILogger<JamEmailService> _logger;
    private readonly JobApplicationManagerContext _context;

    /// <summary>
    /// Constructor for EmailService
    /// </summary>
    /// <param name="logger">Class logger.</param>
    /// <param name="context">The Context object.</param>
    public JamEmailService(ILogger<JamEmailService> logger, JobApplicationManagerContext context)
    {
        _logger = logger;
        _context = context;
    }

    // ReSharper disable once MethodTooLong
    public async Task SendMessageAsync(MimeMessage message)
    {
        Guard.Against.Null(message);

        if (message.To == null) throw new ArgumentNullException(nameof(message));

        try
        {
            var smtpClient = new SmtpClient();
            //await smtpClient.ConnectAsync(smtpIp, 25, false).ConfigureAwait(false);
            await smtpClient.AuthenticateAsync("email", "password").ConfigureAwait(false);
            await smtpClient.SendAsync(message).ConfigureAwait(false);
            await smtpClient.DisconnectAsync(true).ConfigureAwait(false);
            _logger.LogInformation("Sent email");
        }
#pragma warning disable S2139
        catch (Exception ex)
#pragma warning restore S2139
        {
            _logger.LogError(ex, "Error while sending email: {0}", ex);
            throw;
        }

        _logger.Log(LogLevel.Debug, "Email successful sent.");
    }
}