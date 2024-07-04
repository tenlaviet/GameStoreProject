using System;
using System.Threading.Tasks;
using System.Net.Mail;
using MailKit.Security;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;

public class MailSettings
{
    public string Mail { get; set; }
    public string DisplayName { get; set; }
    public string Password { get; set; }
    public string Host { get; set; }
    public int Port { get; set; }

}

// Chứa thông tin Email sẽ gửi (Trường hợp này chưa hỗ trợ đính kém file)
public class MailContent
{
    public string To { get; set; }              // Địa chỉ gửi đến
    public string Subject { get; set; }         // Chủ đề (tiêu đề email)
    public string Body { get; set; }            // Nội dung (hỗ trợ HTML) của email

}

public interface IEmailSender
{
    Task SendEmailAsync(string email, string subject, string message);
    Task SendSmsAsync(string number, string message);
}

public class SendMailService : IEmailSender
{

    
    private readonly MailSettings _mailSettings;

    private readonly ILogger<SendMailService> _logger;


    // mailSetting được Inject qua dịch vụ hệ thống
    // Có inject Logger để xuất log
    public SendMailService(IOptions<MailSettings> mailSettings, ILogger<SendMailService> logger)
    {
        _mailSettings = mailSettings.Value;
        _logger = logger;
        _logger.LogInformation("Create SendMailService");
        
    }


    public async Task SendEmailAsync(string email, string subject, string htmlMessage)
    {
        var message = new MimeMessage();
        message.Sender = new MailboxAddress(_mailSettings.DisplayName, _mailSettings.Mail);
        message.From.Add(new MailboxAddress(_mailSettings.DisplayName, _mailSettings.Mail));
        message.To.Add(MailboxAddress.Parse(email));
        message.Subject = subject;


        var builder = new BodyBuilder();
        builder.HtmlBody = htmlMessage;
        message.Body = builder.ToMessageBody();

        // dùng SmtpClient của MailKit
        using var smtp = new MailKit.Net.Smtp.SmtpClient();

        try
        {
            smtp.Connect(_mailSettings.Host, _mailSettings.Port, SecureSocketOptions.StartTls);
            smtp.Authenticate(_mailSettings.Mail, _mailSettings.Password);
            await smtp.SendAsync(message);
        }

        catch (Exception ex)
        {
            // Gửi mail thất bại, nội dung email sẽ lưu vào thư mục mailssave
            System.IO.Directory.CreateDirectory("mailssave");
            var emailsavefile = string.Format(@"mailssave/{0}.eml", Guid.NewGuid());
            await message.WriteToAsync(emailsavefile);

            _logger.LogInformation("Lỗi gửi mail, lưu tại - " + emailsavefile);
            _logger.LogError(ex.Message);
        }

        smtp.Disconnect(true);

        _logger.LogInformation("send mail to " + email);


    }

    public Task SendSmsAsync(string number, string message)
    {
        // Cài đặt dịch vụ gửi SMS tại đây
        System.IO.Directory.CreateDirectory("smssave");
        var emailsavefile = string.Format(@"smssave/{0}-{1}.txt", number, Guid.NewGuid());
        System.IO.File.WriteAllTextAsync(emailsavefile, message);
        return Task.FromResult(0);
    }
}