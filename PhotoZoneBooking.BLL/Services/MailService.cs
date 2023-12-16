using System.Net;
using System.Net.Mail;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using PhotoZoneBooking.BLL.Configs;
using PhotoZoneBooking.BLL.Dtos;
using PhotoZoneBooking.BLL.Interfaces;
using PhotoZoneBooking.DAL.Entities;

namespace PhotoZoneBooking.BLL.Services;

public class MailService : IMailService
{
    private readonly SmtpClient _smtpClient;
    private readonly UserManager<User> _userManager;

    public MailService(SmtpClient smtpClient, UserManager<User> userManager)
    {
        _smtpClient = smtpClient;
        _userManager = userManager;
    }

    public async Task SendMailAsync(string to, string subject, string message)
    {
        var mail = new MailMessage
        {
            To = { to },
            From = new MailAddress("hoshko.bohdan.m@gmail.com"),
            Subject = subject,
            Body = message
        };

        await _smtpClient.SendMailAsync(mail);
    }

    public async Task SendInvoiceAsync(string to, ReservationDto model)
    {
        var path = Path.Combine(Directory.GetCurrentDirectory(), "EmailTemplates", "invoice.html");
        var htmlString = await File.ReadAllTextAsync(path);
        var user = await _userManager.FindByIdAsync(model.UserId);
        
        var body = htmlString.Replace("@ReservationId", model.Id.ToString())
                             .Replace("@ReservationTitle", model.Title)
                             .Replace("@DateTimeUtcNow", DateTime.UtcNow.ToString())
                             .Replace("@TotalPrice", model.Total.ToString())
                             .Replace("@Fullname", user.FirstName + " " + user.LastName)
                             .Replace("@Start", model.Start.ToString())
                             .Replace("@End", model.End.ToString());

        var mail = new MailMessage
        {
            IsBodyHtml = true,
            From = new MailAddress("hoshko.bohdan.m@gmail.com"),
            Body = body,
            Subject = "Reservation invoice",
            To = { to }
        };
        
        await _smtpClient.SendMailAsync(mail);
    }

    public async Task SendReservationCancelMailAsync(string to, int reservationId)
    {
        var mail = new MailMessage
        {
            To = { to },
            From = new MailAddress("hoshko.bohdan.m@gmail.com"),
            Subject = "Reservation cancelled",
            Body = $"Reservation with id {reservationId} cancelled. Money payed back"
        };

        await _smtpClient.SendMailAsync(mail);
    }
}