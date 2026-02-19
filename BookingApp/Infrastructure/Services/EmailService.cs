using Application.Common;
using Application.DTO;
using Application.Interfaces;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Threading.Tasks;
using brevo_csharp.Api;
using brevo_csharp.Model;

using BrevoConfiguration = brevo_csharp.Client.Configuration;

namespace Infrastructure.Services
{
    public class EmailService : IEmailSender
    {
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<Result<bool>> SendGridEmailAsync(SendEmailDTO emailToSend)
        {
            var apiKey = _configuration["EmailSender:ApiKey"];
            if (string.IsNullOrEmpty(apiKey))
            {
                return Result<bool>.IsFailure("Brevo API key is not configured.");
            }

            BrevoConfiguration.Default.ApiKey.Clear();
            BrevoConfiguration.Default.ApiKey.Add("api-key", apiKey);

            var apiInstance = new TransactionalEmailsApi();

            var senderName = _configuration["EmailSender:FromName"];
            var senderEmail = _configuration["EmailSender:FromEmail"];
            var sender = new SendSmtpEmailSender(senderName, senderEmail);

            var to = new List<SendSmtpEmailTo> { new SendSmtpEmailTo(emailToSend.Email) };

            var email = new SendSmtpEmail(
                sender: sender,
                to: to,
                subject: emailToSend.Subject,
                htmlContent: emailToSend.Body,
                textContent: emailToSend.Body
            );

            try
            {
                await apiInstance.SendTransacEmailAsync(email);
                return Result<bool>.IsSuccess(true);
            }
            catch (Exception e)
            {
                return Result<bool>.IsFailure($"Brevo API Error: {e.Message}");
            }
        }
    }
}